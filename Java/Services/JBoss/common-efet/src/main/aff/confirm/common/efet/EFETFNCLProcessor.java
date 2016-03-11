package aff.confirm.common.efet;

import aff.confirm.common.efet.datarec.EFETFNCLSubmitXML_DataRec;
import aff.confirm.common.efet.info.AffIndexMappingInfo;
import aff.confirm.common.efet.info.EFETFxInfo;
import aff.confirm.common.efet.info.EFETPriceInfo;
import aff.confirm.common.efet.info.EFETSpreadInfo;
import aff.confirm.common.util.XMLUtils;

import java.sql.*;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.Hashtable;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 11:13:05 AM
 */
public class EFETFNCLProcessor {

    private static final String _FIXED_MODEL = "Fixed";
    private static final String _FLOAT_MODEL = "Float";
    private static final String _FIXED_SWAP_TRANS = "FXD_SWP";
    private static final String _FLAOT_SWAP_TRANS = "FLT_SWP";
   
    private Connection affinityConnection;
    private Connection opsTrackingConnection;
    private String docUsage;

    private boolean eicMissing = false;

    //mapping data cache, will be loaded the object is created.
    private Hashtable affIndexMapping;
    private Hashtable  fxIndexMapping;
    private Hashtable fxIndexMethodMapping;

    private Hashtable conversionFactors;

    public boolean isEicMissing() {
        return eicMissing ;
    }

    public EFETFNCLProcessor(Connection pAffinityConnection, Connection pOpsTrackingConnection, String pDocumentUsage, String pDatabaseTNSName)
            throws Exception {

         this.affinityConnection = pAffinityConnection;
         this.opsTrackingConnection  = pOpsTrackingConnection;
         this.docUsage = pDocumentUsage;
         loadAffIndexEfetCdtyRefMapping();
         loadFxIndexMapping();
         loadFxIndexMethodMapping();
         loadConversionFactory();
    }

    private void loadConversionFactory() throws SQLException {

        String sql = "select * from infinity_mgr.uom_cnv";
        Statement stmt = null;
        ResultSet rs = null;
        conversionFactors = new Hashtable();
        try{
            stmt = this.opsTrackingConnection.createStatement();
            rs = stmt.executeQuery(sql);
            while (rs.next()){
                String fromUOM = rs.getString("from_uom_code");
                String toUOM = rs.getString("to_uom_code");
                double convFactor = rs.getDouble("factor");
                Hashtable conversion = (Hashtable) conversionFactors.get(fromUOM);
                if ( conversion == null) {
                    conversion = new Hashtable();
                    conversionFactors.put(fromUOM,conversion);
                }
                conversion.put(toUOM,new Double(convFactor));
            }
        }
        finally {
            try {

                if ( rs != null) {
                    rs.close();
                }
                if ( stmt != null) {
                    stmt.close();
                }
            }
            catch (Exception e){

            }
        }
    }

    private void loadAffIndexEfetCdtyRefMapping() throws SQLException {

        String sql = "Select * from efet.aff_indx_mapping order by aff_indx_name";
        Statement stmt = null;
        ResultSet rs = null;
        affIndexMapping = new Hashtable();
        try{
            stmt = this.opsTrackingConnection.createStatement();
            rs = stmt.executeQuery(sql);
            while (rs.next()){
                String affName = rs.getString("aff_indx_name");
                String efetCdtyRefCode = rs.getString("efet_cdty_ref_code");
                int roundingValue = rs.getInt("rounding_to");
                String pricingDate = rs.getString("pricing_date");
                String deliveryDate = rs.getString("delivery_date");
                String specifiedPrice = rs.getString("specified_price");
                AffIndexMappingInfo affIndexMap = new AffIndexMappingInfo();
                affIndexMap.setAffIndexName(affName);
                affIndexMap.setEfetCdtyRefCode(efetCdtyRefCode);
                affIndexMap.setRoundingValue(roundingValue);
                affIndexMap.setPricingDate(pricingDate);
                affIndexMap.setDeliveryDate(deliveryDate);
                affIndexMap.setSpecifiedPrice(specifiedPrice);

               affIndexMapping.put(affName, affIndexMap);
            }

        }
        finally {
            try {

                if ( rs != null) {
                    rs.close();
                }
                if ( stmt != null) {
                    stmt.close();
                }
            }
            catch (Exception e){

            }
        }

    }

    private void loadFxIndexMapping() throws SQLException {

        String sql = "Select * from efet.fx_indx_mapping";
        Statement stmt = null;
        ResultSet rs = null;
        fxIndexMapping = new Hashtable();
        try{
            stmt = this.opsTrackingConnection.createStatement();
            rs = stmt.executeQuery(sql);
            while (rs.next()){
               String affName = rs.getString("aff_fx_index_sn");
               String efetName = rs.getString("efet_fx_reference");
               fxIndexMapping.put(affName,efetName);
            }

        }
        finally {
            try {

                if ( rs != null) {
                    rs.close();
                }
                if ( stmt != null) {
                    stmt.close();
                }
            }
            catch (Exception e){

            }
        }

    }
    private void loadFxIndexMethodMapping() throws SQLException {

        String sql = "Select * from efet.fx_indx_meth_mapping";
        Statement stmt = null;
        ResultSet rs = null;
        fxIndexMethodMapping = new Hashtable();
        try{
            stmt = this.opsTrackingConnection.createStatement();
            rs = stmt.executeQuery(sql);
            while (rs.next()){
               String affName = rs.getString("aff_fx_indx_method_code");
               String efetName = rs.getString("efet_fx_indx_method_ref");
               fxIndexMethodMapping.put(affName,efetName);
            }

        }
        finally {
            try {

                if ( rs != null) {
                    rs.close();
                }
                if ( stmt != null) {
                    stmt.close();
                }
            }
            catch (Exception e){

            }
        }

    }
    public String getCNFXML(double pTradeId, String pDocumentId, int pDocumentVersion, String pReceiverRole, String[] args)
            throws Exception, ParseException {

        String xmlString = "";
        String receiverRole = "Trader";


        eicMissing = false;
        EFETFNCLSubmitXML_DataRec xmlDataRec = getTrade(pTradeId);
        xmlDataRec.setDocumentId(pDocumentId);
        xmlDataRec.setDocumentVersion(pDocumentVersion);
        if ("B".equalsIgnoreCase(pReceiverRole)) {
           receiverRole="Broker";
           xmlDataRec.setReceiverId(xmlDataRec.getBrokerId());
        }
        xmlDataRec.setReceiverRole(receiverRole);

        args[0] = xmlDataRec.getSenderId();
        args[1] = xmlDataRec.getReceiverId();

        if ("?".equalsIgnoreCase(xmlDataRec.getBuyerParty()) || "?".equalsIgnoreCase(xmlDataRec.getSellerParty())) {
             eicMissing = true;
        }
        if (xmlDataRec.isDataFound())
            xmlString =  getXML(xmlDataRec);
        else
           xmlString = "XML_DATA_ROW_NOT_FOUND";

        return xmlString;

    }

    private String getXML(EFETFNCLSubmitXML_DataRec pXmlDataRec) {
        String xmlText = "";

        xmlText = XMLUtils.EFET_XML_HEADER;
        xmlText = xmlText + XMLUtils.buildTagItem(0, "TradeConfirmation", "", XMLUtils.TAG_OPEN, XMLUtils.EFET_HEADER_ATTRIBS);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentID", pXmlDataRec.getDocumentId(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentUsage", this.docUsage, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "SenderID", pXmlDataRec.getSenderId(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverID", pXmlDataRec.getReceiverId(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverRole", pXmlDataRec.getReceiverRole(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentVersion", "" + pXmlDataRec.getDocumentVersion(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TransactionType",  pXmlDataRec.getTransactionType(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "BuyerParty",  pXmlDataRec.getBuyerParty(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "SellerParty", pXmlDataRec.getSellerParty(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Agreement",  pXmlDataRec.getAgreement(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Currency", pXmlDataRec.getSttlCurrency(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalVolume", pXmlDataRec.getTotalVolumeFmt(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalVolumeUnit", pXmlDataRec.getTotalVolumeUnit(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TradeDate", pXmlDataRec.getTradeDateFmt(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + getFixedPriceXML(pXmlDataRec);
        xmlText = xmlText + getFloatPriceXML(pXmlDataRec);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Rounding", "" +pXmlDataRec.getRounding(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "CommonPricing", "" +pXmlDataRec.getCommonPricing(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "EffectiveDate", "" +pXmlDataRec.getEffectiveDateFmt(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TerminationDate", "" +pXmlDataRec.getTerminationDateFmt(), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + getDeliveryPeriodXML(pXmlDataRec);
        if (pXmlDataRec.getBrokerName() != null) {
            xmlText = xmlText + getAgentsXML(pXmlDataRec);
        }
        xmlText = xmlText + XMLUtils.buildTagItem(0, "TradeConfirmation", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;

    }

    private String getDeliveryPeriodXML(EFETFNCLSubmitXML_DataRec pXmlDataRec) {
        String xmlDlvryText = "";
        ArrayList deliveryList = pXmlDataRec.getDeliveryList();

        xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriods", "", XMLUtils.TAG_OPEN,"");
        for ( int i=0;i<deliveryList.size();++i){
            EFETDeliveryPeriod edp = (EFETDeliveryPeriod) deliveryList.get(i);
            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriod", "", XMLUtils.TAG_OPEN,"");
            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriodStartDate", edp.getStartDateFmt(), XMLUtils.TAG_OPEN_CLOSED,"");
            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriodEndDate", edp.getEndDateFmt(), XMLUtils.TAG_OPEN_CLOSED,"");
            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriodNotionalQuantity", edp.getQtyFmt(), XMLUtils.TAG_OPEN_CLOSED,"");

            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "PaymentDate",edp.getPaymentDateFmt(), XMLUtils.TAG_OPEN_CLOSED,"");

            if (edp.getFixedPrice() > 0 ){
                xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "FixedPrice", edp.getFixedPriceFmt(), XMLUtils.TAG_OPEN_CLOSED,"");
            }
            xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriod", "", XMLUtils.TAG_CLOSED,"");
        }
        xmlDlvryText = xmlDlvryText + XMLUtils.buildTagItem(1, "DeliveryPeriods", "", XMLUtils.TAG_CLOSED,"");

        return xmlDlvryText;
    }

    private String getFixedPriceXML(EFETFNCLSubmitXML_DataRec pXmlDataRec){

        String xmlText = "";
        ArrayList priceInfos = pXmlDataRec.getPriceInfos();
        for ( int i=0;i<priceInfos.size(); ++i){
            EFETPriceInfo epi = (EFETPriceInfo) priceInfos.get(i);
            if (_FIXED_MODEL.equalsIgnoreCase(epi.getPriceType())){
                xmlText = xmlText +  XMLUtils.buildTagItem(1, "FixedPriceInformation", "", XMLUtils.TAG_OPEN,"");
                xmlText = xmlText +  XMLUtils.buildTagItem(2, "FixedPricePayer", epi.getPayer(), XMLUtils.TAG_OPEN_CLOSED,"");
                xmlText = xmlText +  XMLUtils.buildTagItem(1, "FixedPriceInformation", "", XMLUtils.TAG_CLOSED,"");
            }
        }
        return xmlText;
    }

    private String getFloatPriceXML(EFETFNCLSubmitXML_DataRec pXmlDataRec){

        String xmlText = "";
        ArrayList priceInfos = pXmlDataRec.getPriceInfos();
        for ( int i=0;i<priceInfos.size(); ++i){
            EFETPriceInfo epi = (EFETPriceInfo) priceInfos.get(i);
            if (_FLOAT_MODEL.equalsIgnoreCase(epi.getPriceType())){
                xmlText = xmlText +  XMLUtils.buildTagItem(1, "FloatPriceInformation", "", XMLUtils.TAG_OPEN,"");
                xmlText = xmlText +  XMLUtils.buildTagItem(2, "FloatPricePayer", epi.getPayer(), XMLUtils.TAG_OPEN_CLOSED,"");
                xmlText = xmlText + getCommodityReferenceXML(epi);
                xmlText = xmlText +  XMLUtils.buildTagItem(1, "FloatPriceInformation", "", XMLUtils.TAG_CLOSED,"");
            }
        }
        return xmlText;
    }

    private String getCommodityReferenceXML(EFETPriceInfo epi) {

        StringBuilder xmlRefText = new StringBuilder();

        EFETCommodityRef ecr =  epi.getCommodityRef();
        xmlRefText.append(XMLUtils.buildTagItem(2, "CommodityReferences", "", XMLUtils.TAG_OPEN,""));
        xmlRefText.append(XMLUtils.buildTagItem(3, "CommodityReference", "", XMLUtils.TAG_OPEN,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "CommodityReferencePrice", ecr.getReferencePrice(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "IndexCommodity", ecr.getIndexCommodity(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "IndexCurrencyUnit", ecr.getIndexCurrencyUnit(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "IndexCapacityUnit", ecr.getIndexCapacityUnit(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "SpecifiedPrice", ecr.getSpecfiedPrice(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "Factor", ""+ecr.getFactor(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "DeliveryDate", ecr.getDeliveryDate(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(4, "PricingDate", ecr.getPricingDate(), XMLUtils.TAG_OPEN_CLOSED,""));
        if ( ecr.isConversionRateRequired()) {
            xmlRefText.append(XMLUtils.buildTagItem(4,"CRCapacityConversionRate",ecr.getConversinRate()+"",XMLUtils.TAG_OPEN_CLOSED,""));
        }
        if (ecr.getFxInfo() != null){
            xmlRefText.append(getFxInfoXML(ecr.getFxInfo()));
        }
        if (ecr.getSpreadInfo() != null){
            xmlRefText.append(getSpreadXML(ecr.getSpreadInfo()));
        }
        xmlRefText.append(getCalcPeriodsXML(ecr));
        xmlRefText.append(XMLUtils.buildTagItem(3, "CommodityReference", "", XMLUtils.TAG_CLOSED,""));
        xmlRefText.append(XMLUtils.buildTagItem(2, "CommodityReferences", "", XMLUtils.TAG_CLOSED,""));

        return xmlRefText.toString();

    }

    private String getFxInfoXML(EFETFxInfo fxInfo){

        StringBuilder xmlFxText = new StringBuilder();
        if ( fxInfo != null) {
            xmlFxText.append(XMLUtils.buildTagItem(2, "FXInformation", "", XMLUtils.TAG_OPEN,""));
            if (fxInfo.getFxRate() != 0 ){
               xmlFxText.append(XMLUtils.buildTagItem(3, "FXRate", ""+fxInfo.getFxRate(), XMLUtils.TAG_OPEN_CLOSED,""));
            }
            else {
               xmlFxText.append(XMLUtils.buildTagItem(3, "FXReference", fxInfo.getFxReference(), XMLUtils.TAG_OPEN_CLOSED,""));
                xmlFxText.append(XMLUtils.buildTagItem(3, "FXMethod", fxInfo.getFxMethod(), XMLUtils.TAG_OPEN_CLOSED,""));
            }
            xmlFxText.append(XMLUtils.buildTagItem(2, "FXInformation", "", XMLUtils.TAG_CLOSED,""));
        }
        return xmlFxText.toString();
    }
    private String getSpreadXML(EFETSpreadInfo spreadInfo) {

        if ( spreadInfo == null){
            return "";
        }
        StringBuilder xmlSpread = new StringBuilder();
        
        xmlSpread.append(XMLUtils.buildTagItem(3, "SpreadInformation", "", XMLUtils.TAG_OPEN,""));
        xmlSpread.append(XMLUtils.buildTagItem(4,"SpreadPayer",spreadInfo.getPayer(),XMLUtils.TAG_OPEN_CLOSED,""));
        xmlSpread.append(XMLUtils.buildTagItem(4,"Spread","" + spreadInfo.getAmount(),XMLUtils.TAG_OPEN_CLOSED,""));
        if ( spreadInfo.getCurrency() != null){
            xmlSpread.append(XMLUtils.buildTagItem(4,"SpreadCurrencyUnit", spreadInfo.getCurrency(),XMLUtils.TAG_OPEN_CLOSED,""));
            if (spreadInfo.getFxInfo() != null){
                xmlSpread.append(getFxInfoXML(spreadInfo.getFxInfo()));
            }
        }
        xmlSpread.append(XMLUtils.buildTagItem(3, "SpreadInformation", "", XMLUtils.TAG_CLOSED,""));

        return xmlSpread.toString();
    }

    private String getCalcPeriodsXML(EFETCommodityRef ecr) {

        StringBuilder xmlCalcPeriods = new StringBuilder();

        ArrayList periods = ecr.getCalcPeriods();
        xmlCalcPeriods.append(XMLUtils.buildTagItem(2,"CalculationPeriods","",XMLUtils.TAG_OPEN,""));
        if ( periods != null) {
            for (int i=0;i<periods.size();++i){
                EFETCalcPeriod ecp = (EFETCalcPeriod) periods.get(i);
                xmlCalcPeriods.append(XMLUtils.buildTagItem(3,"CalculationPeriod","",XMLUtils.TAG_OPEN,""));
                xmlCalcPeriods.append(XMLUtils.buildTagItem(4,"StartDate",ecp.getStartDateFmt(),XMLUtils.TAG_OPEN_CLOSED,""));
                xmlCalcPeriods.append(XMLUtils.buildTagItem(4,"EndDate",ecp.getEndDateFmt(),XMLUtils.TAG_OPEN_CLOSED,""));
                xmlCalcPeriods.append(XMLUtils.buildTagItem(3,"CalculationPeriod","",XMLUtils.TAG_CLOSED,""));
            }
        }
        xmlCalcPeriods.append(XMLUtils.buildTagItem(2,"CalculationPeriods","",XMLUtils.TAG_CLOSED,""));
        return xmlCalcPeriods.toString();
    }

    private String getAgentsXML(EFETFNCLSubmitXML_DataRec pSubmitRec){

        StringBuilder xmlAgentText = new StringBuilder();


        xmlAgentText.append(XMLUtils.buildTagItem(1, "Agents", "", XMLUtils.TAG_OPEN,""));
        xmlAgentText.append(XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_OPEN,""));
        xmlAgentText.append(XMLUtils.buildTagItem(3, "AgentType", "Broker", XMLUtils.TAG_OPEN_CLOSED,""));
        xmlAgentText.append(XMLUtils.buildTagItem(3, "AgentName", pSubmitRec.getBrokerName(), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlAgentText.append(XMLUtils.buildTagItem(3, "Broker", "", XMLUtils.TAG_OPEN,""));
        xmlAgentText.append(XMLUtils.buildTagItem(4, "BrokerID", (pSubmitRec.getBrokerId()==null?"?":pSubmitRec.getBrokerId()), XMLUtils.TAG_OPEN_CLOSED,""));
        xmlAgentText.append(XMLUtils.buildTagItem(3, "Broker", "", XMLUtils.TAG_CLOSED,""));
        xmlAgentText.append(XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_CLOSED,""));
        xmlAgentText.append(XMLUtils.buildTagItem(1, "Agents", "", XMLUtils.TAG_CLOSED,""));


        return xmlAgentText.toString();

    }

    public EFETFNCLSubmitXML_DataRec getTrade(double tradeId) throws SQLException {
        String sql = "select * from  infinity_mgr.v_efet_fncl_xml_data where prmnt_trade_id = ? order by start_dt";

        PreparedStatement  ps = null;
        ResultSet rs = null;
        EFETFNCLSubmitXML_DataRec dataRec = new EFETFNCLSubmitXML_DataRec();
        dataRec.setDataFound(false);
        ArrayList deliverPeriods = new  ArrayList();
        dataRec.setDeliveryList(deliverPeriods);

        EFETPriceInfo payPrice = new EFETPriceInfo();
        EFETPriceInfo recPrice = new EFETPriceInfo();
        try {
            ps = this.affinityConnection.prepareStatement(sql);
            ps.setDouble(1,tradeId);
            rs = ps.executeQuery();

            boolean updateHeader = false;
            while (rs.next()){

                if ( updateHeader == false) {
                    dataRec.setDataFound(true);
                    dataRec.setTradeId(tradeId);
                    dataRec.setSenderId(rs.getString("sender_id"));
                    dataRec.setReceiverId(rs.getString("receiver_id"));
                    dataRec.setAgreement("ISDA");
                    dataRec.setSttlCurrency(rs.getString("sttl_ccy_code"));
                    dataRec.setTotalVolume(rs.getDouble("total_nom_qty"));
                    dataRec.setTotalVolumeUnit(rs.getString("qty_uom_code"));
                    dataRec.setTradeDate(rs.getDate("trade_dt"));
                   // int rounding = rs.getInt("pay_price_prcsn");
                     int rounding = getRoundingValue(rs.getString("pay_curve"),rs.getString("rec_curve"));
                    dataRec.setRounding(rounding);
                    dataRec.setEffectiveDate(rs.getDate("effective_dt"));
                    dataRec.setTerminationDate(rs.getDate("termination_dt"));
                    dataRec.setCommonPricing("false");
                    dataRec.setBrokerId(rs.getString("broker_id"));
                    dataRec.setBrokerName(rs.getString("broker_ln"));

                    payPrice.setPayer(rs.getString("sender_id"));
                    recPrice.setPayer(rs.getString("receiver_id"));


                    updateHeader = true;
                }
                getPriceInfo(rs,payPrice,recPrice,deliverPeriods);
            }
            if  ( dataRec.isDataFound()) {
                setBuyerSeller(dataRec,payPrice,recPrice);
                ArrayList priceInfo = new ArrayList();
                priceInfo.add(payPrice);
                priceInfo.add(recPrice);
                dataRec.setPriceInfos(priceInfo);
            }

        }
        finally {

            try {
                if (rs != null) {
                    rs.close();
                }
                if (ps != null){
                    ps.close();
                }
            }
            catch (Exception ef){

             }

        }
        return dataRec;

    }

    private int getRoundingValue(String payIndex, String recIndex  ) {
        int roundingValue = 2;

        if ( this.affIndexMapping != null) {

            AffIndexMappingInfo affIndexMap = null;
            affIndexMap =  (AffIndexMappingInfo) affIndexMapping.get(payIndex);
            if ( affIndexMap == null) {
                affIndexMap = (AffIndexMappingInfo) affIndexMapping.get(recIndex);
            }
            if (affIndexMap != null){
                roundingValue = affIndexMap.getRoundingValue();
            }
            
        }
        return roundingValue;

    }

    private void setBuyerSeller(EFETFNCLSubmitXML_DataRec dataRec, EFETPriceInfo payPrice, EFETPriceInfo recPrice) {

        if ( _FIXED_MODEL.equalsIgnoreCase(payPrice.getPriceType()) || _FIXED_MODEL.equalsIgnoreCase(recPrice.getPriceType())) {
           dataRec.setTransactionType(_FIXED_SWAP_TRANS);

           if (_FIXED_MODEL.equalsIgnoreCase(payPrice.getPriceType())){
               dataRec.setBuyerParty(payPrice.getPayer());
               dataRec.setSellerParty(recPrice.getPayer());
           }
           else {
                 dataRec.setBuyerParty(recPrice.getPayer());
                 dataRec.setSellerParty(payPrice.getPayer());
           }
       }
       else {
             dataRec.setTransactionType(_FLAOT_SWAP_TRANS);
             String floatPayer1 = payPrice.getPayer();
             String floatPayer2 = recPrice.getPayer();

             if  ( floatPayer1.compareTo(floatPayer2) > 0) {
                 dataRec.setBuyerParty(floatPayer1);
                 dataRec.setSellerParty(floatPayer2);
              }
             else {
                dataRec.setBuyerParty(floatPayer2);
                dataRec.setSellerParty(floatPayer1);
             }
       }
    }
    
    private void getPriceInfo(ResultSet rs, EFETPriceInfo payPrice, EFETPriceInfo recPrice, ArrayList deliveryPeriods) throws SQLException {

        String payCurve = rs.getString("pay_curve");
        String recCurve = rs.getString("rec_curve");
        boolean isFixed = false;
        double fixedPrice =0;


        if ( _FIXED_MODEL.equalsIgnoreCase(payCurve)) {
            payPrice.setPriceType(_FIXED_MODEL);
            fixedPrice = rs.getDouble("pay_price");
            isFixed = true;
        }
        else {
            payPrice.setPriceType(_FLOAT_MODEL);
            computeCommodityRef(rs,payPrice,"P");
        }
        if ( _FIXED_MODEL.equalsIgnoreCase(recCurve)) {
            recPrice.setPriceType(_FIXED_MODEL);
            fixedPrice = rs.getDouble("rec_price");
            isFixed = true;
        }
        else {
            recPrice.setPriceType(_FLOAT_MODEL);
            computeCommodityRef(rs,recPrice,"R");
        }

        EFETDeliveryPeriod  dp = new EFETDeliveryPeriod();
        dp.setStartDate(rs.getDate("start_dt"));
        dp.setEndDate(rs.getDate("end_dt"));
        dp.setQty(rs.getDouble("total_leg_qty"));
        dp.setPaymentDate(rs.getDate("val_dt"));
        if (isFixed) {
            dp.setFixedPrice(fixedPrice);
        }
        deliveryPeriods.add(dp);
    }

    private void computeCommodityRef(ResultSet rs, EFETPriceInfo price,String payRecFlag) throws SQLException {

        String refPrice = null;
        String indexCommodity = null;
        String currency = null;
        String uom;
        String specifiedPrice = "Average";
        int factor = 1;
        String deliveryDate = "Calculation_Period";
        String pricingData = "Monthly";
        String pricingModel = null;
        double prmntLegId =0;
        double spreadAmount = 0;
        String sttlCurrency = null;

        if ("P".equalsIgnoreCase(payRecFlag))  {
            refPrice = getEfetCdtyRefCode(rs.getString("pay_curve"));
            currency = rs.getString("pay_index_ccy");
            pricingModel = rs.getString("pay_price_model");
            uom = rs.getString("pay_index_uom");
            spreadAmount = rs.getDouble("pay_price");
            pricingData = getECMPriceInfo(rs.getString("pay_curve"));
            deliveryDate = getECMDeliveryInfo(rs.getString("pay_curve"));
            specifiedPrice = getECMSpecifiedPrice(rs.getString("pay_curve"));
        }
        else {
            refPrice = getEfetCdtyRefCode(rs.getString("rec_curve"));
            currency = rs.getString("rec_index_ccy");
            pricingModel = rs.getString("rec_price_model");
            uom = rs.getString("rec_index_uom");
            spreadAmount = rs.getDouble("rec_price");
            pricingData = getECMPriceInfo(rs.getString("rec_curve"));
            deliveryDate = getECMDeliveryInfo(rs.getString("rec_curve"));
            specifiedPrice = getECMSpecifiedPrice(rs.getString("rec_curve"));
        }

        EFETCommodityRef commRef = price.getCommodityRef();
        if (commRef == null) {
            commRef = new EFETCommodityRef();
            price.setCommodityRef(commRef);
        }

        indexCommodity = rs.getString("cdty_code");
        prmntLegId = rs.getDouble("prmnt_trade_leg_id");
        sttlCurrency = rs.getString("sttl_ccy_code");
        if (!sttlCurrency.equalsIgnoreCase(currency)){
           commRef.setFxInfo(getFxInfo(rs));
        }

        commRef.setReferencePrice(refPrice);
        commRef.setIndexCommodity(indexCommodity);
        commRef.setIndexCapacityUnit(uom);
        commRef.setIndexCurrencyUnit(currency);
        commRef.setSpecfiedPrice(specifiedPrice);
        commRef.setFactor(factor);
        commRef.setDeliveryDate(deliveryDate);
        commRef.setPricingDate(pricingData);
        commRef.setPriceModel(pricingModel);

        if (!uom.equalsIgnoreCase(rs.getString("qty_uom_code"))) {
            commRef.setConversionRateRequired(true);
            commRef.setConversinRate(getConversionRate(rs.getString("qty_uom_code"),uom));
            
        }

        if (spreadAmount != 0){
            EFETSpreadInfo esi = new EFETSpreadInfo();
            esi.setAmount(Math.abs(spreadAmount));
            if (spreadAmount > 0) {
                esi.setPayer(rs.getString("receiver_id"));
            }
            else {
                esi.setPayer(rs.getString("sender_id"));
            }

            if (!sttlCurrency.equalsIgnoreCase(currency)) {
                esi.setCurrency(currency);
                esi.setFxInfo(getFxInfo(rs));
            }
            commRef.setSpreadInfo(esi);
        }

        ArrayList calcPeriods = commRef.getCalcPeriods();
        if ( calcPeriods == null){
            calcPeriods = new ArrayList();
            commRef.setCalcPeriods(calcPeriods);
        }
      //  calculatePeriod(calcPeriods,prmntLegId,payRecFlag);
        setCalculatedPeriod(calcPeriods, rs.getDate("start_dt"),rs.getDate("end_dt"));
    }

    private double getConversionRate(String fromUOM, String toUOM) {

        double conversionRate = 1;
        fromUOM  = decodeUOM(fromUOM).toUpperCase();
        toUOM   =  decodeUOM(toUOM).toUpperCase();
        Hashtable hs = (Hashtable) this.conversionFactors.get(fromUOM);
        if (hs != null){
            Double d = (Double) hs.get(toUOM);
            if (d != null) {
                conversionRate = d.doubleValue();
            }
        }
        return conversionRate;
    }

    private String decodeUOM(String uom){
        String returnValue = uom;
        if ("MWH".equalsIgnoreCase(uom)) {
            returnValue = "MW";
        }
        else if ( "KWH".equalsIgnoreCase(uom)) {
            returnValue = "KW";
        }
        return returnValue;
    }

    private EFETFxInfo getFxInfo(ResultSet rs) throws SQLException {

        EFETFxInfo fxInfo = new EFETFxInfo();
        double fixRate = 0;

        fixRate = rs.getDouble("fx_equiv_fixed_rate");
        if (fixRate != 0){ // it is fixed rate conversion
            fxInfo.setFxRate(fixRate);
        }
        else {
            String fxReference = rs.getString("fx_index_sn");
            String fxMethod = rs.getString("fx_equiv_meth_ind");
            if ( fxReference == null) {
                fxReference = "?";
            }
            if ( fxMethod == null){
                fxMethod = "?";
            }
            fxInfo.setFxReference(getFxIndexMappingValue(fxReference));
            fxInfo.setFxMethod(getFxIndexMethodValue(fxMethod));
        }
        return fxInfo;
    }

    private String getFxIndexMappingValue(String indexName){
        String returnValue = "?";
        if (fxIndexMapping != null) {
            returnValue = (String) fxIndexMapping.get(indexName);
            if ( returnValue == null){
                returnValue = "?";
            }
        }
        return returnValue;
    }

    private String getFxIndexMethodValue(String methodInd){

        String returnValue = "?";
        if (fxIndexMethodMapping != null) {
            returnValue = (String) fxIndexMethodMapping.get(methodInd);
            if ( returnValue == null){
                returnValue = "?";
            }
        }
        return returnValue;
    }
    
    private String getECMPriceInfo(String affIndexName) {
        String returnPriceInfo = "?";
        if (this.affIndexMapping != null){
             AffIndexMappingInfo affIndexMap = (AffIndexMappingInfo) this.affIndexMapping.get(affIndexName);
            if (affIndexMap != null){
                if (affIndexMap.getPricingDate() != null) {
                    returnPriceInfo = affIndexMap.getPricingDate();
                }
            }
        }
        return returnPriceInfo;
    }

    private String getECMDeliveryInfo(String affIndexName) {
        String returnDeliveryInfo = "?";
        if (this.affIndexMapping != null){
             AffIndexMappingInfo affIndexMap = (AffIndexMappingInfo) this.affIndexMapping.get(affIndexName);
            if (affIndexMap != null){
                if (affIndexMap.getDeliveryDate() != null) {
                    returnDeliveryInfo = affIndexMap.getDeliveryDate();
                }
            }
        }
        return returnDeliveryInfo;
    }

    private String getECMSpecifiedPrice(String affIndexName) {
        String returnSpecifiedPrice = "?";
        if (this.affIndexMapping != null){
             AffIndexMappingInfo affIndexMap = (AffIndexMappingInfo) this.affIndexMapping.get(affIndexName);
            if (affIndexMap != null){
                if (affIndexMap.getSpecifiedPrice() != null) {
                    returnSpecifiedPrice = affIndexMap.getSpecifiedPrice();
                }
            }
        }
        return returnSpecifiedPrice;
    }

    private String getEfetCdtyRefCode(String affIndexName) {

        String efetCdtyRefCode = "?";
        if (this.affIndexMapping != null) {
            AffIndexMappingInfo affIndexMap = (AffIndexMappingInfo) affIndexMapping.get(affIndexName);
            if (affIndexMap != null){
                if (affIndexMap.getEfetCdtyRefCode() != null) {
                    efetCdtyRefCode = affIndexMap.getEfetCdtyRefCode();
                }
            }
        }
        return efetCdtyRefCode;
    }

    private void calculatePeriod(ArrayList calcPeriods,double prmntLegId, String payRecFlag) throws SQLException {

        String sql= "select pf.fix_dt " +
                "from infinity_mgr.trade_leg_price tlp," +
                "infinity_mgr.price_fix pf " +
                "where tlp.PRMNT_ID = pf.PRMNT_TRADE_LEG_PRICE_ID " +
                "and tlp.ACTIVE_FLAG = 'Y' " +
                "and tlp.EXP_DT = '31-DEC-2299' " +
                "and pf.EXP_DT  = '31-DEC-2299' " +
                "and tlp.factor = ? " +
                "and tlp.PRICE_TYPE_IND = 'T' " +
                "and tlp.PRMNT_TRADE_LEG_ID = ? " +
                "and pf.ACTIVE_FLAG = 'Y' " +
                "order by pf.FIX_DT" ;
     //   --and (tlp.POST_ORDER_NUM <> 2  and tlp.PRICE_FNCTN_ID <> 31 )
        int factor = 1;

        if ("P".equalsIgnoreCase(payRecFlag)) { // payment flag
            factor = -1;
        }

        PreparedStatement ps = null;
        ResultSet rsPrice = null;
        try {
            ps = this.affinityConnection.prepareCall(sql);
            ps.setInt(1,factor);
            ps.setDouble(2,prmntLegId);
            rsPrice = ps.executeQuery();
            while (rsPrice.next()){
                   Date fixDate = rsPrice.getDate("fix_dt");
                  setCalculatedPeriod(calcPeriods,fixDate,fixDate);
            }
        }
        finally {
            try {
                if (rsPrice != null){
                    rsPrice.close();
                }
                if (ps != null){
                    ps.close();
                }
            }
            catch (Exception e){
                
            }
        }
    }

    private void setCalculatedPeriod(ArrayList calcPeriods, Date stDate, Date endDate) {

        boolean isAddNew = true;
        int length = calcPeriods.size();
        if ( length > 0) {
            EFETCalcPeriod cp = (EFETCalcPeriod) calcPeriods.get(length-1);
            Date lastDate = cp.getEndDate();
            if (isNextDay(lastDate,endDate)) {
                cp.setEndDate(endDate);
                isAddNew = false;
            }
        }
        if (isAddNew) {
            EFETCalcPeriod cp = new EFETCalcPeriod();
            cp.setStartDate(stDate);
            cp.setEndDate(endDate);
            calcPeriods.add(cp);
        }
    }

    private boolean isNextDay(Date lastDate, Date endDate) {

        if ( lastDate.getTime() == endDate.getTime()) { //same date, so return true
            return true;    
        }
        Calendar cal =  Calendar.getInstance();
        cal.setTime(lastDate);
        cal.roll(Calendar.DATE,true);
        int day = cal.get(Calendar.DATE);
        int month = cal.get(Calendar.MONTH);
        int year = cal.get(Calendar.YEAR);
        cal.setTime(endDate);
        return ( day == cal.get(Calendar.DATE) && month == cal.get(Calendar.MONTH) && year == cal.get(Calendar.YEAR));
    }

}
