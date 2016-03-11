package aff.confirm.common.efet;

import aff.confirm.common.efet.dao.*;
import aff.confirm.common.efet.datarec.EFETBFIXML_DataRec;
import aff.confirm.common.efet.datarec.EFETOptionSchedule_DataRec;
import aff.confirm.common.efet.datarec.EFETSubmitXML_DataRec;
import aff.confirm.common.efet.datarec.EFETTradeSummary_DataRec;
import aff.confirm.common.util.*;
import aff.confirm.common.util.XMLUtils;
import org.jboss.logging.Logger;
import sempra.trade.daylight.DayLightSavingsFinder;
import sempra.trade.daylight.IDayLightSavings;
import com.sun.rowset.CachedRowSetImpl;
import javax.sql.rowset.CachedRowSet;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.*;

//import sun.jdbc.rowset.CachedRowSet;

public class EFETProcessor {
    //private FileStore fileStore;
    private final int ENGLAND_AND_WALES = 31041;
    private final int NBP = 17791;
    private final int TTF  =45901;
    private final int ZEBRUGGE = 17811;
    private final int EVERYDLVY = 50;
    private Connection opsTrackingConnection;
    private Connection affinityConnection;
    private String documentUsage = "";
    private String databaseTNSName;
    private EFETDeliveryPointArea_DAO efetDeliveryPointAreaDAO;
    //private EFETLoadTypeDAO efetLoadTypeDAO;
    private EFETHubCptyMapping_DAO efetHubCptyMappingDAO;
    private EFETDelivery_DAO efetDeliveryDAO;
    private EFETNotifyAgentMapping_DAO efetNotifyAgentDAO;
    private DecimalFormat df = new DecimalFormat("#0.####");
    private boolean eicMissing = false;

    public boolean isEicMissing() {
        return eicMissing ;
    }
    public EFETProcessor(Connection pAffinityConnection, Connection pOpsTrackingConnection, String pDocumentUsage, String pDatabaseTNSName)
            throws Exception {
        this.affinityConnection = pAffinityConnection;
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.documentUsage = pDocumentUsage;
        this.databaseTNSName = pDatabaseTNSName;
        efetDeliveryDAO = new EFETDelivery_DAO(affinityConnection);
        efetDeliveryPointAreaDAO = new EFETDeliveryPointArea_DAO(opsTrackingConnection);
        //efetLoadTypeDAO = new EFETLoadTypeDAO(opsTrackingConnection);
        efetHubCptyMappingDAO = new EFETHubCptyMapping_DAO(opsTrackingConnection);
        efetNotifyAgentDAO = new EFETNotifyAgentMapping_DAO(opsTrackingConnection);
    }


    public String getCANXML(String pCancelDocumentId, EFETTradeSummary_DataRec pEFETTradeSummaryDataRec, String pReceiverType)
            throws Exception, ParseException {
        String xmlText = "";
        String receiverRole = "";
        if (pReceiverType.equalsIgnoreCase("C"))
            receiverRole = "Trader";
        else
            receiverRole = "Broker";

        xmlText = XMLUtils.EFET_XML_HEADER;
        xmlText = xmlText + XMLUtils.buildTagItem(0, "Cancellation", "", XMLUtils.TAG_OPEN, XMLUtils.EFET_HEADER_ATTRIBS);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentID", pCancelDocumentId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentUsage", documentUsage, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "SenderID", pEFETTradeSummaryDataRec.senderId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverID", pEFETTradeSummaryDataRec.receiverId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverRole", receiverRole, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReferencedDocumentID", pEFETTradeSummaryDataRec.documentId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReferencedDocumentVersion", String.valueOf(pEFETTradeSummaryDataRec.documentVersion), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(0, "Cancellation", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;
    }


    public String getCNFXML(double pTradeID, String pDocumentId, int pDocumentVersion, String pReceiverRole, String[] args)
            throws Exception, ParseException {
        String xmlString = "";
        eicMissing = false;
        EFETSubmitXML_DataRec efetSubmitXMLDataRec;
        efetSubmitXMLDataRec = getEfetSubmitXMLDataRec(pTradeID);
        efetSubmitXMLDataRec.documentUsage = documentUsage;
        if (pReceiverRole.equalsIgnoreCase("C")){
            efetSubmitXMLDataRec.receiverRole = "Trader";
        } else if (pReceiverRole.equalsIgnoreCase("B")) {
            efetSubmitXMLDataRec.receiverRole = "Broker";
            efetSubmitXMLDataRec.receiverId = efetSubmitXMLDataRec.brokerId;
        } else throw new Exception("EFETProcessor.getCNFXML: Unsupported pReceiverRole: "+
                pReceiverRole + ": TradeId=" + df.format(efetSubmitXMLDataRec.tradeID));

        efetSubmitXMLDataRec.documentId = pDocumentId;
        efetSubmitXMLDataRec.setDocumentVersion(pDocumentVersion);
        args[0] = efetSubmitXMLDataRec.senderId;
        args[1] = efetSubmitXMLDataRec.receiverId;

        //Used to work around the phys-only infinity_mgr.v_efet_xml_data call
        //efetSubmitXMLDataRec.buyerParty = efetSubmitXMLDataRec.receiverId;
        //efetSubmitXMLDataRec.sellerParty = efetSubmitXMLDataRec.senderId;

        if ("?".equalsIgnoreCase(efetSubmitXMLDataRec.buyerParty) || "?".equalsIgnoreCase(efetSubmitXMLDataRec.sellerParty)) {
            eicMissing = true;
        }
        if (efetSubmitXMLDataRec.xmlDataRowFound)
            xmlString = buildCNFXML(efetSubmitXMLDataRec);
        else
            xmlString = "XML_DATA_ROW_NOT_FOUND";

        return xmlString;
    }

    public String getBFIXML(EFETBFIXML_DataRec pEFETBFIXMLDataRec)
            throws Exception, ParseException {
        String xmlText = "";
        xmlText = XMLUtils.EFET_XML_HEADER;
        xmlText = xmlText + XMLUtils.buildTagItem(0, "BrokerFeeInformation", "", XMLUtils.TAG_OPEN, XMLUtils.EFET_HEADER_ATTRIBS);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentID", pEFETBFIXMLDataRec.documentId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentUsage", pEFETBFIXMLDataRec.documentUsage, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "SenderID", pEFETBFIXMLDataRec.senderId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverID", pEFETBFIXMLDataRec.receiverId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverRole", pEFETBFIXMLDataRec.receiverRole, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentVersion", pEFETBFIXMLDataRec.documentVersion, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "LinkedTo", pEFETBFIXMLDataRec.linkedTo, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalFee", pEFETBFIXMLDataRec.totalFee, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "FeeCurrency", pEFETBFIXMLDataRec.feeCurrency, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(0, "BrokerFeeInformation", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;
    }


    public String buildCNFXML(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec) throws Exception {
        String xmlText = "";
        boolean isEmissionTrade = checkEmissionTrade(pEFETSubmitXMLDataRec);
        xmlText = XMLUtils.EFET_XML_HEADER;
        xmlText = xmlText + XMLUtils.buildTagItem(0, "TradeConfirmation", "", XMLUtils.TAG_OPEN, XMLUtils.EFET_HEADER_ATTRIBS);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentID", pEFETSubmitXMLDataRec.documentId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentUsage", pEFETSubmitXMLDataRec.documentUsage, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "SenderID", pEFETSubmitXMLDataRec.senderId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverID", pEFETSubmitXMLDataRec.receiverId, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "ReceiverRole", pEFETSubmitXMLDataRec.receiverRole, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "DocumentVersion", pEFETSubmitXMLDataRec.documentVersion, XMLUtils.TAG_OPEN_CLOSED, "");
        if (!isEmissionTrade) {
            xmlText = xmlText + XMLUtils.buildTagItem(1, "Market", pEFETSubmitXMLDataRec.market, XMLUtils.TAG_OPEN_CLOSED, "");
        }
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Commodity", pEFETSubmitXMLDataRec.commodity, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TransactionType", pEFETSubmitXMLDataRec.transactionType, XMLUtils.TAG_OPEN_CLOSED, "");
        if (!isEmissionTrade) {
            xmlText = xmlText + XMLUtils.buildTagItem(1, "DeliveryPointArea", pEFETSubmitXMLDataRec.deliveryPointArea, XMLUtils.TAG_OPEN_CLOSED, "");
        }
        xmlText = xmlText + XMLUtils.buildTagItem(1, "BuyerParty", pEFETSubmitXMLDataRec.buyerParty, XMLUtils.TAG_OPEN_CLOSED, "");

        xmlText = xmlText + XMLUtils.buildTagItem(1, "SellerParty", pEFETSubmitXMLDataRec.sellerParty, XMLUtils.TAG_OPEN_CLOSED, "");
        if (!isEmissionTrade) {
            xmlText = xmlText + XMLUtils.buildTagItem(1, "LoadType", pEFETSubmitXMLDataRec.loadType, XMLUtils.TAG_OPEN_CLOSED, "");
        }
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Agreement", pEFETSubmitXMLDataRec.agreement, XMLUtils.TAG_OPEN_CLOSED, "");

        String useFractUnitAttrib = "";
        if (!isEmissionTrade && pEFETSubmitXMLDataRec.useFractionalCapUnit) {
            useFractUnitAttrib = "UseFractionUnit=\"true\"";
        }
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Currency", pEFETSubmitXMLDataRec.currency, XMLUtils.TAG_OPEN_CLOSED, useFractUnitAttrib);
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalVolume", pEFETSubmitXMLDataRec.totalVolume, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalVolumeUnit", pEFETSubmitXMLDataRec.totalVolumeUnit, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "TradeDate", pEFETSubmitXMLDataRec.tradeDate, XMLUtils.TAG_OPEN_CLOSED, "");

        if (!isEmissionTrade) {
            xmlText = xmlText + XMLUtils.buildTagItem(1, "CapacityUnit", pEFETSubmitXMLDataRec.capacityUnit, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(1, "PriceUnit", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "Currency", pEFETSubmitXMLDataRec.priceUnitCurrency, XMLUtils.TAG_OPEN_CLOSED, useFractUnitAttrib);

            xmlText = xmlText + XMLUtils.buildTagItem(2, "CapacityUnit", pEFETSubmitXMLDataRec.priceUnitCapacityUnit, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(1, "PriceUnit", "", XMLUtils.TAG_CLOSED, "");

            xmlText = xmlText + XMLUtils.buildTagItem(1, "TimeIntervalQuantities", "", XMLUtils.TAG_OPEN, "");
            //boolean sameQtyPrice = false;
            boolean basicGas = false;
            boolean basicPower = false;
            boolean hourCompute = false; // to com

            //Israel 10/30/2006 Removed because it seems to be an unnecessary test.
            //sameQtyPrice = (pEFETSubmitXMLDataRec.sameQtyFlag.equalsIgnoreCase("Y") &&
            //  pEFETSubmitXMLDataRec.samePriceFlag.equalsIgnoreCase("Y"));

            hourCompute = pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Gas") &&
                       ( pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("DAY")  || pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("CUSTM")  )&&
                     pEFETSubmitXMLDataRec.computeHourVolDayDelivery.equalsIgnoreCase("Y");

            basicGas = pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Gas") && //sameQtyPrice &&
                (pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("DAY") ||
                pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("MONTH") || pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("HOUR")  );
            basicPower = pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Power") && //sameQtyPrice &&
                (pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("HOUR") ||
                pEFETSubmitXMLDataRec.qtyPerDurationCode.equalsIgnoreCase("ALLHR")) &&
                pEFETSubmitXMLDataRec.useAffDeliveryTableFlag.equalsIgnoreCase("N");

            // currently the affinity does not support hourly delivery for the Gas deals
            // we are here converting the day delivery into hourly delivery

            if ( hourCompute ) {
                xmlText = xmlText +  getTimeIntervalQuantitiesFromDayToHourly(pEFETSubmitXMLDataRec);
            }
            //If there is a single start and end datetime use the data that has just been loaded into these fields
            else if ( basicGas || basicPower){
                xmlText = xmlText + getTimeIntervalQuanitity(pEFETSubmitXMLDataRec.deliveryStartDateAndTime,
                        pEFETSubmitXMLDataRec.deliveryEndDateAndTime, pEFETSubmitXMLDataRec.qtyPer,
                        pEFETSubmitXMLDataRec.price);
            }
            else if (!pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Gas"))
                // else
                //If there are multiple periods use this routine.
                xmlText = xmlText + getTimeIntervalQuantitiesTimeChangeConsolidate(pEFETSubmitXMLDataRec);
                //xmlText = xmlText + getTimeIntervalQuantities(pEFETSubmitXMLDataRec);

            else throw new Exception("EFETProcessor.getEfetSubmitXML: Unsupported trade type: "+
                        pEFETSubmitXMLDataRec.commodity + ": " + pEFETSubmitXMLDataRec.tradeID);

            xmlText = xmlText + XMLUtils.buildTagItem(1, "TimeIntervalQuantities", "", XMLUtils.TAG_CLOSED, "");
        }

        xmlText = xmlText + XMLUtils.buildTagItem(1, "TotalContractValue", pEFETSubmitXMLDataRec.totalContractValue, XMLUtils.TAG_OPEN_CLOSED, "");

        if ( isEmissionTrade ) {  // elements only for EUA trade details.
            xmlText = xmlText + XMLUtils.buildTagItem(1,"EUATradeDetails","",XMLUtils.TAG_OPEN,"");
            xmlText = xmlText + XMLUtils.buildTagItem(2,"Price",pEFETSubmitXMLDataRec.price,XMLUtils.TAG_OPEN_CLOSED,"");
            xmlText = xmlText + XMLUtils.buildTagItem(2,"EmissionsDeliveryDate",getDateOnly(pEFETSubmitXMLDataRec.deliveryStartDateAndTime),XMLUtils.TAG_OPEN_CLOSED,"");
            xmlText = xmlText + XMLUtils.buildTagItem(1,"EUATradeDetails","",XMLUtils.TAG_CLOSED,"");
        }
        // adding Option trades information


        boolean insertHubCodeInfo = pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Gas");// &&
                //Israel 12/21/2006 - changed to accommodate additional gas locations as they get added.
                //(pEFETSubmitXMLDataRec.locationSn.equalsIgnoreCase("NBP") ||
                // pEFETSubmitXMLDataRec.locationSn.equalsIgnoreCase("ZEEBRUGGE HUB"));
        boolean isUKPower = pEFETSubmitXMLDataRec.commodity.equalsIgnoreCase("Power") &&
                pEFETSubmitXMLDataRec.locationSn.equalsIgnoreCase("ENGLAND AND WALES");

        if (insertHubCodeInfo){
            String buyerHubCode = efetHubCptyMappingDAO.getCptyHubCode(pEFETSubmitXMLDataRec.deliveryPointArea, pEFETSubmitXMLDataRec.buyerParty);
            String sellerHubCode = efetHubCptyMappingDAO.getCptyHubCode(pEFETSubmitXMLDataRec.deliveryPointArea, pEFETSubmitXMLDataRec.sellerParty);
            xmlText = xmlText + XMLUtils.buildTagItem(1, "HubCodificationInformation", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "BuyerHubCode", buyerHubCode, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "SellerHubCode", sellerHubCode, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(1, "HubCodificationInformation", "", XMLUtils.TAG_CLOSED, "");
        }
        else if (isUKPower){
            //A ? means no EIC code has been entered. In that case, supply the NotifyAgentID so we know
            //what data needs to be entered into the table.
            String netaTransmissionCharges = "Schedule 5 off";
            xmlText = xmlText + XMLUtils.buildTagItem(1, "AccountAndChargeInformation", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "SellerEnergyAccountIdentification", pEFETSubmitXMLDataRec.netaSellAccount, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "BuyerEnergyAccountIdentification", pEFETSubmitXMLDataRec.netaBuyAccount, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "NotificationAgent", pEFETSubmitXMLDataRec.notifyAgentEicCode, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "TransmissionChargeIdentification", netaTransmissionCharges, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(1, "AccountAndChargeInformation", "", XMLUtils.TAG_CLOSED, "");
        }

        if ("OPT".equalsIgnoreCase(pEFETSubmitXMLDataRec.transactionType)) {
                    xmlText = xmlText + XMLUtils.buildTagItem(1,"OptionDetails","",XMLUtils.TAG_OPEN,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"OptionsType",pEFETSubmitXMLDataRec.optionTypeInd,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"OptionWriter",pEFETSubmitXMLDataRec.optionWriter,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"OptionHolder",pEFETSubmitXMLDataRec.optionHolder,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"OptionStyle",pEFETSubmitXMLDataRec.optionStyleCode,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"StrikePrice",pEFETSubmitXMLDataRec.strikePrice,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"PremiumRate",pEFETSubmitXMLDataRec.premRate,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"PremiumCurrency",pEFETSubmitXMLDataRec.priceUnitCurrency,XMLUtils.TAG_OPEN_CLOSED,"");

                    if (!isEmissionTrade) {
                        xmlText = xmlText + XMLUtils.buildTagItem(2,"PremiumUnit","",XMLUtils.TAG_OPEN,"");
                        xmlText = xmlText + XMLUtils.buildTagItem(3,"Currency",pEFETSubmitXMLDataRec.premUnitCcyCode, XMLUtils.TAG_OPEN_CLOSED,"");
                        xmlText = xmlText + XMLUtils.buildTagItem(3,"Capacity",pEFETSubmitXMLDataRec.priceUnitCapacityUnit, XMLUtils.TAG_OPEN_CLOSED,"");
                        xmlText = xmlText + XMLUtils.buildTagItem(2,"PremiumUnit","",XMLUtils.TAG_CLOSED,"");
                    }

                    xmlText = xmlText + XMLUtils.buildTagItem(2,"TotalPremiumValue",pEFETSubmitXMLDataRec.premTotalValue,XMLUtils.TAG_OPEN_CLOSED,"");
                    xmlText = xmlText + XMLUtils.buildTagItem(2,"PremiumPaymentDate",pEFETSubmitXMLDataRec.premPaymentDate,XMLUtils.TAG_OPEN_CLOSED,"");

                    Vector optionSchedule = pEFETSubmitXMLDataRec.optionShedule;
                    if ( optionSchedule == null || optionSchedule.size() <=0 ) {
                        throw new Exception( "OPTION SCHEDULE MISSING");
                    }

                    if (isEmissionTrade ) {
                       EFETOptionSchedule_DataRec optionRec = (EFETOptionSchedule_DataRec) optionSchedule.get(0);
                       xmlText = xmlText + XMLUtils.buildTagItem(2,"ExerciseDateTime",optionRec.getEFETExerciseDateTime(),XMLUtils.TAG_OPEN_CLOSED,"");
                    }
                    else {
                         xmlText = xmlText + getOptionSchedulesXML(pEFETSubmitXMLDataRec,optionSchedule);

                    }
                    xmlText = xmlText + XMLUtils.buildTagItem(1,"OptionDetails","",XMLUtils.TAG_CLOSED,"");
             }  // end of option details section

        boolean insertAgents = (isUKPower || pEFETSubmitXMLDataRec.agentType.length()>1);
        if (insertAgents)
            xmlText = xmlText + XMLUtils.buildTagItem(1, "Agents", "", XMLUtils.TAG_OPEN, "");

        if (isUKPower){
            xmlText = xmlText + XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "AgentType", "ECVNA", XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(4, "ECVNA", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(5, "BSCPartyID", pEFETSubmitXMLDataRec.bscPartyId, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(5, "BuyerEnergyAccount", pEFETSubmitXMLDataRec.netaBuyAccount, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(5, "SellerEnergyAccount", pEFETSubmitXMLDataRec.netaSellAccount, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(5, "SellerID", pEFETSubmitXMLDataRec.ecvnaSellerId, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(5, "BuyerID", pEFETSubmitXMLDataRec.ecvnaBuyerId, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(4, "ECVNA", "", XMLUtils.TAG_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_CLOSED, "");
        }

        if (pEFETSubmitXMLDataRec.agentType.length()>1){
            xmlText = xmlText + XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_OPEN, "");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "AgentType", pEFETSubmitXMLDataRec.agentType, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "AgentName", pEFETSubmitXMLDataRec.agentName, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "Broker", "", XMLUtils.TAG_OPEN, "");

            String brokerId = pEFETSubmitXMLDataRec.brokerId;
            if (brokerId == null)
                brokerId = "BR??";
            xmlText = xmlText + XMLUtils.buildTagItem(4, "BrokerID", brokerId, XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "Broker", "", XMLUtils.TAG_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "Agent", "", XMLUtils.TAG_CLOSED, "");
        }

        if (insertAgents)
            xmlText = xmlText + XMLUtils.buildTagItem(1, "Agents", "", XMLUtils.TAG_CLOSED, "");

        xmlText = xmlText + XMLUtils.buildTagItem(1, "TraderName", pEFETSubmitXMLDataRec.traderName, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(0, "TradeConfirmation", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;
    }

    private String getTimeIntervalQuantitiesFromDayToHourly(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec) throws Exception {

        String xmlText  = "";
        Date deliveryDt;
        int numberDayHour=0;
         DecimalFormat df = new DecimalFormat("#0.####");

        String timeSchedCode = getTimeSchedCode(affinityConnection,pEFETSubmitXMLDataRec.otcLocCdtyId);

        IDayLightSavings dayLightSavings = DayLightSavingsFinder.getInstance(databaseTNSName).findDayLightSavings();
        double dTradeId = Double.parseDouble(pEFETSubmitXMLDataRec.tradeID);
        CachedRowSet crs = efetDeliveryDAO.getCRS(dTradeId);
        int crsSize = crs.size();
        crs.beforeFirst();
        if (crs.next()) {
            deliveryDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            int dayLightOffset = dayLightSavings.getAdjustment(deliveryDt, timeSchedCode);
            numberDayHour = 24  + dayLightOffset;
            double dailyQty = Math.abs(crs.getDouble("DAILY_QTY"));
            double hrsQty = dailyQty/numberDayHour;
            String strQty = df.format( hrsQty);
            xmlText = xmlText + getTimeIntervalQuanitity(pEFETSubmitXMLDataRec.deliveryStartDateAndTime,
                    pEFETSubmitXMLDataRec.deliveryEndDateAndTime, strQty,
                    pEFETSubmitXMLDataRec.price);
        }
        return xmlText;

    }


    private String  getOptionSchedulesXML(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec, Vector optionSchedule) {
        String xmlText = "";

        xmlText = XMLUtils.buildTagItem(2,"ExerciseSchedule","",XMLUtils.TAG_OPEN,"");
        if (    isSameExerciseDt(optionSchedule) && !"Power".equalsIgnoreCase(pEFETSubmitXMLDataRec.commodity)) { // same exercise date for all trades
            EFETOptionSchedule_DataRec optionRec = (EFETOptionSchedule_DataRec) optionSchedule.get(0);
            xmlText = xmlText + XMLUtils.buildTagItem(3,"Exercise","",XMLUtils.TAG_OPEN,"");
            xmlText = xmlText + XMLUtils.buildTagItem(3,"DeliveryStartDateTime",pEFETSubmitXMLDataRec.deliveryStartDateAndTime,XMLUtils.TAG_OPEN_CLOSED,"");
            xmlText = xmlText + XMLUtils.buildTagItem(3,"DeliveryEndDateTime",pEFETSubmitXMLDataRec.deliveryEndDateAndTime,XMLUtils.TAG_OPEN_CLOSED,"");
            xmlText = xmlText + XMLUtils.buildTagItem(3,"ExerciseDateTime",optionRec.getEFETExerciseDateTime(),XMLUtils.TAG_OPEN_CLOSED,"");
            xmlText = xmlText + XMLUtils.buildTagItem(3,"Exercise","",XMLUtils.TAG_CLOSED,"");
        }
        else {

              for (int i=0;i<optionSchedule.size(); ++i) {
                  EFETOptionSchedule_DataRec optionRec = (EFETOptionSchedule_DataRec) optionSchedule.get(i);
                  xmlText = xmlText + XMLUtils.buildTagItem(3,"Exercise","",XMLUtils.TAG_OPEN,"");
                  xmlText = xmlText + XMLUtils.buildTagItem(3,"DeliveryStartDateTime",optionRec.getEFETDeliveryStartDateTime(),XMLUtils.TAG_OPEN_CLOSED,"");
                  xmlText = xmlText + XMLUtils.buildTagItem(3,"DeliveryEndDateTime",optionRec.getEEFETDeliveryEndDateTime(),XMLUtils.TAG_OPEN_CLOSED,"");
                  xmlText = xmlText + XMLUtils.buildTagItem(3,"ExerciseDateTime",optionRec.getEFETExerciseDateTime(),XMLUtils.TAG_OPEN_CLOSED,"");
                  xmlText = xmlText + XMLUtils.buildTagItem(3,"Exercise","",XMLUtils.TAG_CLOSED,"");
               }
         }
        xmlText = xmlText + XMLUtils.buildTagItem(2,"ExerciseSchedule","",XMLUtils.TAG_CLOSED,"");
        return xmlText;

    }

    private boolean isSameExerciseDt(Vector optionSchedule) {
        boolean isSame = true;
        Date prevExerciseDt = null;

        if ( optionSchedule != null) {
            for (int i=0;i<optionSchedule.size(); ++i) {
                EFETOptionSchedule_DataRec optionRec = (EFETOptionSchedule_DataRec) optionSchedule.get(i);
                if ( prevExerciseDt != null && optionRec.exerciseDateTime != null){
                    if ( prevExerciseDt.getTime() != optionRec.exerciseDateTime.getTime()) {
                        isSame = false;
                        break;
                    }
                }
                prevExerciseDt = optionRec.exerciseDateTime;
            }
        }
        return isSame;
    }

    private String getDateOnly(String deliveryStartDateAndTime) {
        int len = deliveryStartDateAndTime.indexOf("T");
        if (len > 0) {
            return deliveryStartDateAndTime.substring(0,len);
        }
        return deliveryStartDateAndTime;
    }

    private boolean checkEmissionTrade(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec) {
        return (EFET_DAO.EMISSION_PHASE_1.equalsIgnoreCase(pEFETSubmitXMLDataRec.commodity) || EFET_DAO.EMISSION_PHASE_2.equalsIgnoreCase(pEFETSubmitXMLDataRec.commodity) || EFET_DAO.EMISSION_PHASE_3.equalsIgnoreCase(pEFETSubmitXMLDataRec.commodity) );
    }

    private String getTimeIntervalQuantitiesTimeChangeConsolidate(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec)
            throws Exception {
        SimpleDateFormat sdfLocalDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm");
        SimpleDateFormat sdfTestDt = new SimpleDateFormat("MM/dd/yyyy");
        SimpleDateFormat sdfLocalEfet = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        DecimalFormat df = new DecimalFormat("#0.####");

        String xmlText = "";
        //EFETDeliveryDAO efetDeliveryDAO = new EFETDeliveryDAO(affinityConnection);
        int hrsMask = 0;
        int year = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        int i = 0;
        int crsSize = 0;
        String[] stringArray = new String[24];
        SempraBitSet bitSet;
        Calendar cal = Calendar.getInstance();
        Date deliveryDt = new Date();
        Date startDt = new Date();
        Date endDt = new Date();
        CachedRowSet crs;
        double dTradeId = Double.parseDouble(pEFETSubmitXMLDataRec.tradeID);
        crs = efetDeliveryDAO.getCRS(dTradeId);
        crsSize = crs.size();
        double[][][][] dateArray = new double[22][13][32][24];
        crs.beforeFirst();
        while (crs.next()) {
            if (crs.getRow() == 1)
                startDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            endDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            hrsMask = crs.getInt("HRS_MASK");
            deliveryDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            cal.setTime(deliveryDt);
            year = cal.get(Calendar.YEAR) - 2000;
            month = cal.get(Calendar.MONTH) + 1; //Calendar returns a 0-based month.
            day = cal.get(Calendar.DATE);
            bitSet = SempraBitSet.maskToBitSet(hrsMask);
            stringArray = bitSet.toStringArray();


            for (i = 0; i < 24; i++) {
                hour = i;
                if (stringArray[i].equalsIgnoreCase("1")) {
                    dateArray[year][month][day][hour] = Math.abs(crs.getDouble("HRLY_QTY"));
                }
            }
        }

        cal.setTime(startDt);
        year = cal.get(Calendar.YEAR) - 2000;
        month = cal.get(Calendar.MONTH) + 1; //Calendar returns a 0-based month.
        day = cal.get(Calendar.DATE);
        hour = 0;
        cal.setTime(endDt);
        int endYear = cal.get(Calendar.YEAR) - 2000;
        int endMonth = cal.get(Calendar.MONTH) + 1;
        int endDay = cal.get(Calendar.DATE);
        int cumDailyHours = 0;
        double lastHourlyQty = 0;
        int iDates = 0;
        IDayLightSavings dayLightSavings = DayLightSavingsFinder.getInstance().findDayLightSavings();
        int dayLightOffset;
        int hoursInDate;
        boolean clockChanged = false;
        String timeSchedCode = "";
        timeSchedCode = getTimeSchedCode(affinityConnection,pEFETSubmitXMLDataRec.otcLocCdtyId);

        String yearStr = "";
        String monthStr = "";
        String dayStr = "";
        String hourStr = "";
        double dailyHours = 0;
      //  Date[] startDates = new Date[crsSize*3];
      //  Date[] endDates = new Date[crsSize*3];
        String[] startDates = new String[crsSize*3];
        String[] endDates = new String[crsSize*3];
        double[] hourlyQty = new double[crsSize*3];
        double[] totalHours = new double[crsSize*3];
        boolean setFirstDate = true;
        boolean setLastDate = false;
        boolean hourlyQtyChanged = false;
        Date testDt = new Date();
        int yearInit = year;
        int monthInit = month;
        int dayInit = day;
        boolean hr2Qty = false;
        yearLoop:
            for (year=yearInit;year<endYear+1;year++){
                monthLoop:
                for (month=monthInit;month<13;month++){
                    monthInit = 1;
                    for (day=dayInit;day<32;day++){
                        dayInit = 1;
                        testDt = sdfTestDt.parse(month+"/"+day+"/"+(year+2000));
                        //System.out.println("testDt="+sdfTestDt.format(testDt));
                        for (hour=0;hour<24;hour++){
                            hr2Qty = false;
                            hourlyQtyChanged = (lastHourlyQty != 0 &&
                                lastHourlyQty != dateArray[year][month][day][hour]);

                            //Test to see if it is a time change day
                            //skip hourly changed if hoursInDate=25 or 23/hour=1
                            clockChanged = isClockChange(dayLightSavings, testDt, timeSchedCode, hour);
                            //If hourly qty changed then close off previous period.
                            //a new one will then be started.
                            if (hourlyQtyChanged && !clockChanged){
                                yearStr = Integer.toString(2000+year);
                                monthStr = StringUtils.zeroFill(month,2);
                                dayStr = StringUtils.zeroFill(day,2);
                                hourStr = StringUtils.zeroFill(hour,2);
                                endDates[iDates] =   yearStr + "-" +  monthStr + "-" + dayStr + "T" + hourStr + ":00:00" ;
                                /* ******* Convert date to string
                                endDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                        yearStr + " " + hourStr + ":00");
                                */

                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                lastHourlyQty = 0;
                                setFirstDate = true;
                                setLastDate = false;
                                dailyHours = 0;
                                iDates++;
                            }
                            else if (  clockChanged){
                                int dayOffSet =   getClockChangeOffset(dayLightSavings, testDt,timeSchedCode, hour);

                                if (dayOffSet < 0) {
                                    hourlyQtyChanged = (lastHourlyQty != 0 &&
                                                         lastHourlyQty != dateArray[year][month][day][hour+1]);
                                    hour = hour - dayOffSet ;
                                }
                                else if (dayOffSet > 0){
                                    hourlyQtyChanged = (lastHourlyQty != 0 &&
                                                    lastHourlyQty != ( (dateArray[year][month][day][hour])/2));
                                    hr2Qty = true;
                                 }
                                if (hourlyQtyChanged) {
                                    yearStr = Integer.toString(2000+year);
                                    monthStr = StringUtils.zeroFill(month,2);
                                    dayStr = StringUtils.zeroFill(day,2);
                                    hourStr = StringUtils.zeroFill(hour,2);
                                     endDates[iDates] =   yearStr + "-" +  monthStr + "-" + dayStr + "T" + hourStr + ":00:00" ;
                                /* ******* Convert date to string
                                    endDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                        yearStr + " " + hourStr + ":00");
                                */
                                    hourlyQty[iDates] = lastHourlyQty;
                                    totalHours[iDates] = dailyHours;
                                    lastHourlyQty = 0;
                                    setFirstDate = true;
                                    setLastDate = false;
                                    dailyHours = 0;
                                    iDates++;
                                }
                            }

                            if (dateArray[year][month][day][hour] > 0){
                                if (setFirstDate == true){
                                    yearStr = Integer.toString(2000+year);
                                    monthStr = StringUtils.zeroFill(month,2);
                                    dayStr = StringUtils.zeroFill(day,2);
                                    hourStr = StringUtils.zeroFill(hour,2);
                                    startDates[iDates] =   yearStr + "-" +  monthStr + "-" + dayStr + "T" + hourStr + ":00:00" ;
                                /* ******* Convert date to string

                                    startDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                            yearStr + " " + hourStr + ":00");
                                */
                                    setFirstDate = false;
                                    setLastDate = true;
                                }
                            }
                            else  if (setLastDate == true && !clockChanged) {
                                yearStr = Integer.toString(2000+year);
                                monthStr = StringUtils.zeroFill(month,2);
                                dayStr = StringUtils.zeroFill(day,2);
                                hourStr = StringUtils.zeroFill(hour,2);
                                 endDates[iDates] =   yearStr + "-" +  monthStr + "-" + dayStr + "T" + hourStr + ":00:00" ;
                                /* ******* Convert date to string
                                endDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                        yearStr + " " + hourStr + ":00");
                                */
                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                lastHourlyQty = 0;
                                setFirstDate = true;
                                setLastDate = false;
                                dailyHours = 0;
                                iDates++;
                            }
                            if (hr2Qty == true) {
                                lastHourlyQty = dateArray[year][month][day][hour]/2;
                            }
                            else {
                                lastHourlyQty = dateArray[year][month][day][hour];
                            }
                            dailyHours += dateArray[year][month][day][hour];
                        }
                        if (year == endYear &&
                            month == endMonth &&
                            day == endDay){
                            //For delivery intervals ending at the exact day, the routine
                            //will hit this code before it can hit the above code to enter
                            //the last date. In this case, it executes it here.
                            if (setLastDate == true) {
                                yearStr = Integer.toString(2000 + year);
                                monthStr = StringUtils.zeroFill(month, 2);
                                dayStr = StringUtils.zeroFill(day, 2);
                                hourStr = StringUtils.zeroFill(hour, 2);
                                // samy: here we have hour 24 and we will not have timezone issue to convert into date
                                // and converting into string since it is last day of the year
                                endDates[iDates] = sdfLocalEfet.format(sdfLocalDateTime.parse(monthStr + "/" + dayStr + "/" +
                                        yearStr + " " + hourStr + ":00"));

                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                dailyHours = 0;
                                iDates++;
                            }
                            break yearLoop;
                        }

                        //Make sure no extra processing is done on months less than 31 days.
                        if (day == DateUtils.getDaysInMonth(year,month)){
                            continue monthLoop;
                        }
                    }
                }
            }


        //Read the consolidated blocks and generate a TimeInterval period for each one.
        String[] startDtStr = new String[iDates];
        String[] endDtStr = new String[iDates];
        String[] capacity = new String[iDates];
        int totalCapacity = 0;
        for (i=0; i<iDates; i++){
            /*if (startDtStr[i] == null)
                break;*/
            startDtStr[i] = startDates[i];
            endDtStr[i] = endDates[i];
            /* change from date to string
            startDtStr[i] = sdfLocalEfet.format(startDates[i]);
            endDtStr[i] = sdfLocalEfet.format(endDates[i]);
            */
          //  capacity[i] = Double.toString(hourlyQty[i]);
            capacity[i] = df.format(hourlyQty[i]);
            xmlText = xmlText + getTimeIntervalQuanitity(startDtStr[i],endDtStr[i],
                        capacity[i], pEFETSubmitXMLDataRec.price);

            totalCapacity += totalHours[i];
        }

        //This is done when the data is retrieved.
        /*double totalContractValue = 0;
        double price = 0;
        price = Double.parseDouble(pEFETSubmitXMLDataRec.price);
        totalContractValue = (price * totalCapacity);
        //contractPrice = df.format(price);
        pEFETSubmitXMLDataRec.setTotalContractValue(totalContractValue);*/
        return xmlText;
    }

    private boolean isClockChange(IDayLightSavings pDayLightSavings, Date pDate, String pTimeSchedCode, int pHour) {
        boolean clockChange = false;
        int dayLightOffset;
        int hoursInDate = 24;
        dayLightOffset = pDayLightSavings.getAdjustment(pDate, pTimeSchedCode);
        if (dayLightOffset == 1)
            hoursInDate  = 25;
        else if (dayLightOffset == -1)
            hoursInDate  = 23;

        clockChange = (hoursInDate == 23 && (pHour == 1 || pHour == 2)) ||
                      (hoursInDate == 25 && (pHour == 1 || pHour == 2 || pHour == 3));

        //clockChange = hoursInDate != 24;
        return clockChange;
    }

    private int getClockChangeOffset( IDayLightSavings pDayLightSavings, Date pDate,String pTimeSchedCode, int pHour) {

        int dayLightOffset = 0;
        dayLightOffset = pDayLightSavings.getAdjustment(pDate, pTimeSchedCode);
        if ("BPT".equalsIgnoreCase(pTimeSchedCode)){  // UK time zone
            if (  ( dayLightOffset < 0 && pHour != 1) || ( dayLightOffset > 0 && pHour != 1)) {
                dayLightOffset = 0;
            }
        }
        else { // Contentental time zone
            if (  ( dayLightOffset < 0 && pHour != 2) || ( dayLightOffset > 0 && pHour != 2)) {
                dayLightOffset = 0;
            }
        }
        return   dayLightOffset;
    }

    private String getTimeSchedCode(Connection pAffConnection, int pOtcLocCdtyId)
            throws SQLException {
        String timeSchedCode = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = pAffConnection.prepareStatement("select time_sched_code from " +
                    "infinity_mgr.otc_loc_cdty where id = ?");
            statement.setInt(1, pOtcLocCdtyId);
            rs = statement.executeQuery();
            if (rs.next()) {
                timeSchedCode = (rs.getString("time_sched_code"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return timeSchedCode;
    }


    private String getTimeIntervalQuanitity(String pEfetStartDate, String pEfetEndDate,
                                            String pContractCapacity, String pContractPrice) {
        String xmlText = "";
        xmlText = XMLUtils.buildTagItem(2, "TimeIntervalQuantity", "", XMLUtils.TAG_OPEN, "");
        xmlText = xmlText + XMLUtils.buildTagItem(3, "DeliveryStartDateAndTime", pEfetStartDate, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(3, "DeliveryEndDateAndTime", pEfetEndDate, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(3, "ContractCapacity", pContractCapacity, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(3, "Price", pContractPrice, XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(2, "TimeIntervalQuantity", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;
    }

    public EFETSubmitXML_DataRec getEfetSubmitXMLDataRec(double pTradeID)
            throws Exception, ParseException {
        EFETSubmitXML_DataRec efetSubmitXMLDataRec;
        efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();
        String fractionalCapUnit = "";
        String qtyUom = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = affinityConnection.prepareStatement("SELECT * from infinity_mgr.v_efet_xml_data " +
                    "where prmnt_trade_id = ? ");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                efetSubmitXMLDataRec.xmlDataRowFound = true;
                efetSubmitXMLDataRec.setTradeId(rs.getDouble("PRMNT_TRADE_ID"));
                efetSubmitXMLDataRec.senderId = rs.getString("SENDER_ID");
                efetSubmitXMLDataRec.receiverId = rs.getString("RECEIVER_ID");
                //Israel - New EFET broker match
                efetSubmitXMLDataRec.brokerEicId = rs.getString("BROKER_EIC_ID");
                efetSubmitXMLDataRec.market = rs.getString("MARKET");
                efetSubmitXMLDataRec.commodity = rs.getString("COMMODITY");
                efetSubmitXMLDataRec.transactionType = rs.getString("TRANSACTION_TYPE");
                efetSubmitXMLDataRec.locationSn = rs.getString("LOCATION_SN");
                //efetSubmitXMLDataRec.deliveryPointArea = rs.getString("DELIVERY_POINT_AREA");
                int otcLocCdtyId = rs.getInt("OTC_LOC_CDTY_ID");
                efetSubmitXMLDataRec.deliveryPointArea = efetDeliveryPointAreaDAO.getDeliveryPointArea(otcLocCdtyId);
                efetSubmitXMLDataRec.buyerParty = rs.getString("BUYER_ID");
                efetSubmitXMLDataRec.sellerParty = rs.getString("SELLER_ID");
                String qtyUomDurationCode = rs.getString("QTY_PER_DURATION_CODE");
                /*if (efetSubmitXMLDataRec.commodity.equalsIgnoreCase("Gas"))
                    efetSubmitXMLDataRec.loadType = "Base";
                else
                    efetSubmitXMLDataRec.loadType = efetLoadTypeDAO.getLoadType(qtyUomDurationCode);*/
                //As per 3.1 spec, LoadType is now always Custom for power, Base for gas.
                if ( efetSubmitXMLDataRec.commodity.equalsIgnoreCase("Power"))
                    efetSubmitXMLDataRec.loadType = "Custom";
                else
                    efetSubmitXMLDataRec.loadType = "Base";
                efetSubmitXMLDataRec.otcLocCdtyId = rs.getInt("OTC_LOC_CDTY_ID");

                //Israel 7/28/15 -- Replaced by getting agreement directly from the database.
                //efetSubmitXMLDataRec.agreement = getAgreementType(efetSubmitXMLDataRec.commodity, efetSubmitXMLDataRec.otcLocCdtyId);
                efetSubmitXMLDataRec.agreement = rs.getString("GOV_AGRMNT");

                efetSubmitXMLDataRec.currency = rs.getString("STTL_CCY_CODE");
                double totalVolume = rs.getDouble("TOTAL_NOM_QTY");
                efetSubmitXMLDataRec.setTotalVolume(totalVolume);
                efetSubmitXMLDataRec.totalVolumeUnit = rs.getString("QTY_UOM_CODE");
                efetSubmitXMLDataRec.tradeDate = rs.getString("TRADE_DT");

                efetSubmitXMLDataRec.computeHourVolDayDelivery = efetDeliveryPointAreaDAO.getHourCalcForDayDeliveryFlag(efetSubmitXMLDataRec.deliveryPointArea);

                efetSubmitXMLDataRec.useAffDeliveryTableFlag =
                    efetDeliveryPointAreaDAO.getUseAffDeliveryTableFlag(efetSubmitXMLDataRec.deliveryPointArea);
                int startHour = efetDeliveryPointAreaDAO.getDeliveryStartHour(efetSubmitXMLDataRec.deliveryPointArea);
                efetSubmitXMLDataRec.setDeliveryStartDateAndTime(rs.getDate("TRADE_START_DT"),startHour);
                efetSubmitXMLDataRec.setDeliveryEndDateAndTime(rs.getDate("TRADE_END_DT"),startHour);
                efetSubmitXMLDataRec.setContractCapacity(totalVolume);
                double dPrice = 0;
                String payPriceModel = rs.getString("PAY_PRICE_MODEL");
                if (payPriceModel == null || payPriceModel.equalsIgnoreCase("")){
                    dPrice = rs.getDouble("REC_PRICE");
                    qtyUom = rs.getString("REC_UOM");
                }
                else {
                    dPrice = rs.getDouble("PAY_PRICE");
                    qtyUom = rs.getString("PAY_UOM");
                }

                //fractionalCapUnit = rs.getString("FRACTIONAL_CAPACITY_UNIT");
                fractionalCapUnit = efetDeliveryPointAreaDAO.getFractionalCapacityUnit(otcLocCdtyId);
                if (fractionalCapUnit == null)
                    fractionalCapUnit = "NONE";
                efetSubmitXMLDataRec.useFractionalCapUnit = !fractionalCapUnit.equalsIgnoreCase("NONE");
                efetSubmitXMLDataRec.setStrikePrice(dPrice);
                if (efetSubmitXMLDataRec.useFractionalCapUnit){
                    efetSubmitXMLDataRec.capacityUnit = fractionalCapUnit;
                    efetSubmitXMLDataRec.priceUnitCapacityUnit = fractionalCapUnit;
                    dPrice = dPrice * 100;
                }
                else {
                    efetSubmitXMLDataRec.capacityUnit = qtyUom;
                    efetSubmitXMLDataRec.priceUnitCapacityUnit = qtyUom;
                }

                efetSubmitXMLDataRec.setPrice(dPrice);
                efetSubmitXMLDataRec.setTotalContractValue(dPrice * totalVolume);

                efetSubmitXMLDataRec.priceUnitCurrency = rs.getString("PRICE_CCY_CODE");
                double qtyPer = rs.getDouble("QTY_PER");
                efetSubmitXMLDataRec.setQtyPer(qtyPer);

                String brokerSn = rs.getString("BROKER_SN");
                if ((brokerSn != null) && (!brokerSn.equalsIgnoreCase(""))) {
                    efetSubmitXMLDataRec.agentType = "Broker";
                    efetSubmitXMLDataRec.agentName = rs.getString("BROKER_LN");
                    efetSubmitXMLDataRec.brokerId = rs.getString("BROKER_ID");
                }
                efetSubmitXMLDataRec.traderName = rs.getString("TRADER");
                efetSubmitXMLDataRec.qtyPerDurationCode = rs.getString("QTY_PER_DURATION_CODE");
                efetSubmitXMLDataRec.sameQtyFlag = rs.getString("SAME_QTY_FLAG");
                efetSubmitXMLDataRec.samePriceFlag = rs.getString("SAME_PRICE_FLAG");

                //Israel - New EFET broker match
                efetSubmitXMLDataRec.bkrFeeTotal = rs.getDouble("BKR_FEE_TOTAL");
                efetSubmitXMLDataRec.bkrFeeCcy = rs.getString("BKR_FEE_CCY");

                efetSubmitXMLDataRec.netaBuyAccount = rs.getString("NETA_BUY_ACCT");
                if (efetSubmitXMLDataRec.netaBuyAccount == null) {
                    efetSubmitXMLDataRec.netaBuyAccount = "";
                }
                efetSubmitXMLDataRec.netaSellAccount = rs.getString("NETA_SELL_ACCT");
                if (efetSubmitXMLDataRec.netaSellAccount == null) {
                    efetSubmitXMLDataRec.netaSellAccount = "";
                }
                //efetSubmitXMLDataRec.extTrnsmssnInd = rs.getString("EXT_TRNSMSSN_IND");
                efetSubmitXMLDataRec.notifyAgentId = rs.getString("NOTIFY_AGENT_ID");
                //efetSubmitXMLDataRec.netaTransmissionCharges = rs.getString("NETA_TRANSMISSION_CHARGES");
                //efetSubmitXMLDataRec.notifyAgentEicCode = rs.getString("NOTIFY_AGENT_EIC_CODE");
                efetSubmitXMLDataRec.notifyAgentEicCode = efetNotifyAgentDAO.getEICCode(efetSubmitXMLDataRec.notifyAgentId);
                efetSubmitXMLDataRec.ecvnaBuyerId = rs.getString("ECVNA_BUYER_ID");
                efetSubmitXMLDataRec.ecvnaSellerId = rs.getString("ECVNA_SELLER_ID");
                //efetSubmitXMLDataRec.bscPartyId = rs.getString("BSC_PARTY_ID");
                efetSubmitXMLDataRec.bscPartyId = efetNotifyAgentDAO.getECVNAId(efetSubmitXMLDataRec.notifyAgentId);

                if (efetSubmitXMLDataRec.locationSn.equalsIgnoreCase("ENGLAND AND WALES") &&
                    efetSubmitXMLDataRec.commodity.equalsIgnoreCase("Power") &&
                    qtyUomDurationCode.equalsIgnoreCase("HOUR")){
                    double days = 0;
                    int startDate = DateHelper.encodeDate(rs.getDate("TRADE_START_DT"));
                    int endDate = DateHelper.encodeDate(rs.getDate("TRADE_END_DT"));
                    days = DateHelper.daysBetween(startDate,endDate);
                    double dailyVol = totalVolume / qtyPer;
                    /*if (dailyVol == 24)
                        efetSubmitXMLDataRec.loadType = "Base";
                    else if (dailyVol/days == 24)
                        efetSubmitXMLDataRec.loadType = "Base";
                    else
                        efetSubmitXMLDataRec.loadType = "Custom";*/
                }
                // added 6/13/07 to support Options trading
                if ("OPT".equalsIgnoreCase(efetSubmitXMLDataRec.transactionType)) {
                    efetSubmitXMLDataRec.optionTypeInd = rs.getString("OPTION_TYPE_IND");
                    efetSubmitXMLDataRec.optionStyleCode = rs.getString("OPTION_STYLE_CODE");
                    efetSubmitXMLDataRec.optionHolder = rs.getString("OPTION_HOLDER");
                    efetSubmitXMLDataRec.optionWriter = rs.getString("OPTION_WRITER");
                    efetSubmitXMLDataRec.setPremRate(rs.getDouble("PREM_RATE"));
                    efetSubmitXMLDataRec.premUnitCcyCode = rs.getString("PREM_UNIT_CCY_CODE");
                    efetSubmitXMLDataRec.premUnitUomCode = rs.getString("PREM_UNIT_UOM_CODE");
                    efetSubmitXMLDataRec.setPremTotalValue(rs.getDouble("PREM_TOTAL_VALUE"));
                    efetSubmitXMLDataRec.premPaymentDate = rs.getString("PREM_PAYMENT_DT");
                    Vector optionScheudles = getOptionSchedules(efetSubmitXMLDataRec,pTradeID,startHour);
                    efetSubmitXMLDataRec.optionShedule = optionScheudles;
                }
            }
            else
                efetSubmitXMLDataRec.xmlDataRowFound = false;

        } finally {
            try {
                statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
        }
        return efetSubmitXMLDataRec;
    }

    public EFETBFIXML_DataRec getEfetBFIDataRec(double pTradeID, String pDocumentId,
                                                      int pDocumentVersion, String pLinkedTo)
            throws Exception {
        EFETBFIXML_DataRec efetBFIXMLDataRec = new EFETBFIXML_DataRec();
        EFETSubmitXML_DataRec efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();

        efetSubmitXMLDataRec = getEfetSubmitXMLDataRec(pTradeID);
        efetBFIXMLDataRec.init();
        efetBFIXMLDataRec.documentId = pDocumentId;
        efetBFIXMLDataRec.setDocumentVersion(pDocumentVersion);
        efetBFIXMLDataRec.documentUsage = documentUsage;
        efetBFIXMLDataRec.senderId = efetSubmitXMLDataRec.senderId;
        efetBFIXMLDataRec.receiverId = efetSubmitXMLDataRec.brokerId;
        efetBFIXMLDataRec.receiverRole = "Broker";
        efetBFIXMLDataRec.linkedTo = pLinkedTo;
        efetBFIXMLDataRec.setTotalFee(efetSubmitXMLDataRec.bkrFeeTotal);
        efetBFIXMLDataRec.feeCurrency = efetSubmitXMLDataRec.bkrFeeCcy;
        return efetBFIXMLDataRec;
    }


    private boolean isDailyExpiry(double pTradeId) throws SQLException {
        boolean isDailyExpiry = false;
        String sql = "select * from infinity_mgr.option_trade_leg where prmnt_trade_leg_price_id in ( select ID From " +
                     " infinity_mgr.trade_leg_price where prmnt_trade_id = ? ) and exp_dt = '31-DEC-2299' and active_flag = 'Y'";


        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = affinityConnection.prepareStatement(sql);
            statement.setDouble(1,pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
               int optionExpModelId = rs.getInt("OPTN_EXP_DT_MODEL_ID");
               isDailyExpiry = (optionExpModelId == EVERYDLVY);
            }

        }
        finally {
             try {
                statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
        }

        return isDailyExpiry;

    }

    private Vector getFixedExerciseSchedules(double pTradeId, int startHour, EFETSubmitXML_DataRec efetSubmitXMLDataRec) throws Exception {

            Vector optionSchedules = new Vector();
            PreparedStatement statement = null;
            ResultSet rs = null;

             String expiryTime = efetDeliveryPointAreaDAO.getOptionExprTimes(efetSubmitXMLDataRec.deliveryPointArea);
            String fixedExpiryTime = "15:00:00";
            // get the expiry for this location.
            if (expiryTime != null && !"".equals(expiryTime)) {
                String[] timeList = expiryTime.split(",");
                if (timeList.length>1) {
                    fixedExpiryTime = timeList[0];
                }
            }

            try {
                statement = affinityConnection.prepareStatement("Select * From infinity_mgr.v_efet_option_schedule where prmnt_trade_id = ?");
                statement.setDouble(1,pTradeId);
                rs = statement.executeQuery();
                while (rs.next()) {
                    EFETOptionSchedule_DataRec optionRec = new EFETOptionSchedule_DataRec();
                    optionRec.setTradeId(rs.getDouble("PRMNT_TRADE_ID"));
                    optionRec.setDeliveryStartDateTime(rs.getDate("DELIVERY_START_DT"),startHour);
                    optionRec.setDeliveryEndDateTime(rs.getDate("DELIVERY_END_DT"),startHour);
                    optionRec.setExerciseDateTime(getOptionExpDateTime(rs.getDate("EXERCISE_DT"),fixedExpiryTime));
                    optionSchedules.add(optionRec);

                }

            }
            finally {
                 try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
                if (rs != null) {
                    try {
                        rs.close();
                    } catch (SQLException e) {
                    }
                    rs = null;
                }
            }
            return optionSchedules;
        }

    private Vector getOptionSchedules(EFETSubmitXML_DataRec efetSubmitXMLDataRec, double pTradeId, int startHour) throws Exception {

        Vector optionSchedules ;
        if (isDailyExpiry(pTradeId)){
            optionSchedules = getDailyExerciseSchedules(pTradeId,efetSubmitXMLDataRec,startHour);
        }
        else {
            //optionSchedules = getFixedExerciseSchedules(pTradeId,startHour,efetSubmitXMLDataRec);
           optionSchedules= getFixedExerciseSchedulesUsingDlvry(pTradeId,startHour,efetSubmitXMLDataRec);
        }
        return optionSchedules;
    }

    private Vector  getFixedExerciseSchedulesUsingDlvry(double pTradeId, int startHour, EFETSubmitXML_DataRec efetSubmitXMLDataRec) throws Exception {

        Vector optionSchedules = new Vector();
            PreparedStatement statement = null;
            ResultSet rs = null;

             String expiryTime = efetDeliveryPointAreaDAO.getOptionExprTimes(efetSubmitXMLDataRec.deliveryPointArea);
            String fixedExpiryTime = "15:00:00";
            // get the expiry for this location.
            if (expiryTime != null && !"".equals(expiryTime)) {
                String[] timeList = expiryTime.split(",");
                if (timeList.length>1) {
                    fixedExpiryTime = timeList[0];
                }
            }

            try {
                statement = affinityConnection.prepareStatement("Select * From infinity_mgr.v_efet_option_schedule where prmnt_trade_id = ?");
                statement.setDouble(1,pTradeId);
                rs = statement.executeQuery();
                Date exerciseDate = null;

                if (rs.next()) {
                     exerciseDate = rs.getDate("EXERCISE_DT");
                }
                Hashtable deliverySchedules = getOptionDeliveryTimeIntervalQuantities(efetSubmitXMLDataRec);
                Enumeration keys = deliverySchedules.keys();

                while (keys.hasMoreElements()) {
                    EFETOptionSchedule_DataRec optionRec = new EFETOptionSchedule_DataRec();
                    optionRec.setTradeId(pTradeId);
                    Date stDate =(Date)keys.nextElement();
                    Date eDate = (Date) deliverySchedules.get(stDate);
                    optionRec.setDeliveryStartDateTime(stDate);
                    optionRec.setDeliveryEndDateTime(eDate);
                    optionRec.setExerciseDateTime(getOptionExpDateTime(exerciseDate,fixedExpiryTime));
                    optionSchedules.add(optionRec);
                }

            }
            finally {
                 try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
                if (rs != null) {
                    try {
                        rs.close();
                    } catch (SQLException e) {
                    }
                    rs = null;
                }
            }
            return optionSchedules;


    }
    private Vector getDailyExerciseSchedules(double pTradeId, EFETSubmitXML_DataRec efetSubmitXMLDataRec, int startHour) throws Exception {

        String sql = " select expire_dt  from infinity_mgr.option_exercise where prmnt_option_trade_leg_id in (select id  from infinity_mgr.option_trade_leg where prmnt_trade_leg_price_id in ( select ID From " +
                     " infinity_mgr.trade_leg_price where prmnt_trade_id = ? )) " +
                     " and acctng_exp_dt = '31-DEC-2299' and active_flag = 'Y' " +
                     " order by expire_dt ";

        Vector optionSchedules = new Vector();
        Hashtable deliverySchedules = getOptionDeliveryTimeIntervalQuantities(efetSubmitXMLDataRec);
        PreparedStatement statement = null;
        ResultSet rs = null;
        SimpleDateFormat sdf  = new SimpleDateFormat("MM/dd/yyyy");
        boolean matchFound = false;
        String expiryTime = efetDeliveryPointAreaDAO.getOptionExprTimes(efetSubmitXMLDataRec.deliveryPointArea);
        String dailyExpiryTime = "12:00:00";
        // get the expiry for this location.
        if (expiryTime != null && !"".equals(expiryTime)) {
            String[] timeList = expiryTime.split(",");
            if (timeList.length>1) {
                dailyExpiryTime = timeList[1];
            }
        }

        try {
             statement = affinityConnection.prepareStatement(sql);
             statement.setDouble(1,pTradeId);
             rs = statement.executeQuery();
             while (rs.next()) {
                   EFETOptionSchedule_DataRec optionRec = new EFETOptionSchedule_DataRec();
                   optionRec.setTradeId(pTradeId);
                   Date deliveryDate = getOptionDeliveryDate(rs.getDate("EXPIRE_DT"));
                   // get the start date and time
                   Enumeration keys = deliverySchedules.keys();
                   Date startDate = null;
                   matchFound = false;
                   while (keys.hasMoreElements()) {
                       startDate = (Date) keys.nextElement();
                       if (sdf.format(startDate).equals(sdf.format(deliveryDate))) {
                           matchFound = true;
                           break;
                        }
                   }
                   if (matchFound) {
                        optionRec.deliveryStartDateTime = startDate;
                        optionRec.deliveryEndDateTime = (Date) deliverySchedules.get(startDate);
                   }
                   else {
                        optionRec.setDeliveryStartDateTime(deliveryDate,startHour);
                        optionRec.setDeliveryEndDateTime(deliveryDate,startHour);
                   }
                   optionRec.setExerciseDateTime(getOptionExpDateTime(rs.getDate("EXPIRE_DT"),dailyExpiryTime));
                   optionSchedules.add(optionRec);

              }
         }
         finally {
                 try {
                     statement.close();
                 } catch (SQLException e) {
                    }
                    statement = null;
                    if (rs != null) {
                        try {
                            rs.close();
                            rs.close();
                        } catch (SQLException e) {
                    }
                    rs = null;
                    }
         }
         return optionSchedules;
     }

    private Date getOptionExpDateTime(Date date, String expiryTime) {

        if (date!= null ) {
            Calendar cal = Calendar.getInstance();
            cal.setTime(date);
            if ( cal.get(Calendar.HOUR_OF_DAY) == 0 && cal.get(Calendar.MINUTE) == 0 && cal.get(Calendar.SECOND) ==0) {
                SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss");
                try {
                    Date dt= sdf.parse(expiryTime);
                    Calendar cal2 = Calendar.getInstance();
                    cal2.setTime(dt);
                    cal.set(Calendar.HOUR_OF_DAY,cal2.get(Calendar.HOUR_OF_DAY));
                    cal.set(Calendar.MINUTE,cal2.get(Calendar.MINUTE));
                    cal.set(Calendar.SECOND,cal2.get(Calendar.SECOND));

                } catch (ParseException e) {
                  Logger.getLogger(EFETProcessor.class).error("Parsing error for default option expiry time( " + expiryTime +")" );
                }

            }
            date = cal.getTime();
        }
        return date;
    }

    private Date getOptionDeliveryDate(java.util.Date dt) {
        Date delDt = null;
        Calendar cal = Calendar.getInstance();
        cal.setTime(dt);
        while (true) {
            cal.add(Calendar.DATE,1);
            if (cal.get(Calendar.DAY_OF_WEEK) != Calendar.SATURDAY && cal.get(Calendar.DAY_OF_WEEK) != Calendar.SUNDAY ) {
                break;
            }
        }
        delDt = cal.getTime();
        return delDt;
    }

    private Hashtable getOptionDeliveryTimeIntervalQuantities(EFETSubmitXML_DataRec pEFETSubmitXMLDataRec)
            throws Exception {
        SimpleDateFormat sdfLocalDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm");
        SimpleDateFormat sdfTestDt = new SimpleDateFormat("MM/dd/yyyy");
        SimpleDateFormat sdfLocalEfet = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");


        //EFETDeliveryDAO efetDeliveryDAO = new EFETDeliveryDAO(affinityConnection);
        int hrsMask = 0;
        int year = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        int i = 0;
        int crsSize = 0;
        String[] stringArray = new String[24];
        SempraBitSet bitSet;
        Calendar cal = Calendar.getInstance();
        Date deliveryDt = new Date();
        Date startDt = new Date();
        Date endDt = new Date();
        CachedRowSet crs;
        double dTradeId = Double.parseDouble(pEFETSubmitXMLDataRec.tradeID);
        crs = efetDeliveryDAO.getCRS(dTradeId);
        crsSize = crs.size();
        int[][][][] dateArray = new int[20][13][32][24];
        crs.beforeFirst();
        while (crs.next()) {
            if (crs.getRow() == 1)
                startDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            endDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            hrsMask = crs.getInt("HRS_MASK");
            deliveryDt = efetDeliveryDAO.getUtilDateFromCRSString("DLVRY_DT", crs);
            cal.setTime(deliveryDt);
            year = cal.get(Calendar.YEAR) - 2000;
            month = cal.get(Calendar.MONTH) + 1; //Calendar returns a 0-based month.
            day = cal.get(Calendar.DATE);
            bitSet = SempraBitSet.maskToBitSet(hrsMask);
            stringArray = bitSet.toStringArray();

            for (i = 0; i < 24; i++) {
                hour = i;
                if (stringArray[i].equalsIgnoreCase("1")) {
                    dateArray[year][month][day][hour] = Math.abs(crs.getInt("HRLY_QTY"));
                }
            }
        }

        cal.setTime(startDt);
        year = cal.get(Calendar.YEAR) - 2000;
        month = cal.get(Calendar.MONTH) + 1; //Calendar returns a 0-based month.
        day = cal.get(Calendar.DATE);
        hour = 0;
        cal.setTime(endDt);
        int endYear = cal.get(Calendar.YEAR) - 2000;
        int endMonth = cal.get(Calendar.MONTH) + 1;
        int endDay = cal.get(Calendar.DATE);
        int cumDailyHours = 0;
        int lastHourlyQty = 0;
        int iDates = 0;
        IDayLightSavings dayLightSavings = DayLightSavingsFinder.getInstance(databaseTNSName).findDayLightSavings();
        int dayLightOffset;
        int hoursInDate;
        boolean clockChanged = false;
        String timeSchedCode = "";
        timeSchedCode = getTimeSchedCode(affinityConnection,pEFETSubmitXMLDataRec.otcLocCdtyId);

        String yearStr = "";
        String monthStr = "";
        String dayStr = "";
        String hourStr = "";
        int dailyHours = 0;
        Date[] startDates = new Date[crsSize*3];
        Date[] endDates = new Date[crsSize*3];
        int[] hourlyQty = new int[crsSize*3];
        int[] totalHours = new int[crsSize*3];
        boolean setFirstDate = true;
        boolean setLastDate = false;
        boolean hourlyQtyChanged = false;
        Date testDt = new Date();
        int yearInit = year;
        int monthInit = month;
        int dayInit = day;
        Hashtable hs = new Hashtable();
        yearLoop:
            for (year=yearInit;year<endYear+1;year++){
                monthLoop:
                for (month=monthInit;month<13;month++){
                    monthInit = 1;
                    for (day=dayInit;day<32;day++){
                        dayInit = 1;
                        testDt = sdfTestDt.parse(month+"/"+day+"/"+(year+2000));
                        //System.out.println("testDt="+sdfTestDt.format(testDt));
                        for (hour=0;hour<24;hour++){
                            hourlyQtyChanged = (lastHourlyQty != 0 &&
                                lastHourlyQty != dateArray[year][month][day][hour]);

                            //Test to see if it is a time change day
                            //skip hourly changed if hoursInDate=25 or 23/hour=1
                            clockChanged = isClockChange(dayLightSavings, testDt, timeSchedCode, hour);

                            //If hourly qty changed then close off previous period.
                            //a new one will then be started.
                            if (hourlyQtyChanged && !clockChanged){
                                yearStr = Integer.toString(2000+year);
                                monthStr = StringUtils.zeroFill(month,2);
                                dayStr = StringUtils.zeroFill(day,2);
                                hourStr = StringUtils.zeroFill(hour,2);
                                endDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                        yearStr + " " + hourStr + ":00");
                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                lastHourlyQty = 0;
                                setFirstDate = true;
                                setLastDate = false;
                                dailyHours = 0;
                                iDates++;
                            }

                            if (dateArray[year][month][day][hour] > 0){
                                if (setFirstDate == true){
                                    yearStr = Integer.toString(2000+year);
                                    monthStr = StringUtils.zeroFill(month,2);
                                    dayStr = StringUtils.zeroFill(day,2);
                                    hourStr = StringUtils.zeroFill(hour,2);
                                    startDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                            yearStr + " " + hourStr + ":00");
                                    setFirstDate = false;
                                    setLastDate = true;
                                }
                            }
                            else  if (setLastDate == true && !clockChanged) {
                                yearStr = Integer.toString(2000+year);
                                monthStr = StringUtils.zeroFill(month,2);
                                dayStr = StringUtils.zeroFill(day,2);
                                hourStr = StringUtils.zeroFill(hour,2);
                                endDates[iDates] = sdfLocalDateTime.parse( monthStr + "/" + dayStr + "/" +
                                                        yearStr + " " + hourStr + ":00");
                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                lastHourlyQty = 0;
                                setFirstDate = true;
                                setLastDate = false;
                                dailyHours = 0;
                                iDates++;
                            }
                            lastHourlyQty = dateArray[year][month][day][hour];
                            dailyHours += dateArray[year][month][day][hour];
                        }
                        if (year == endYear &&
                            month == endMonth &&
                            day == endDay){
                            //For delivery intervals ending at the exact day, the routine
                            //will hit this code before it can hit the above code to enter
                            //the last date. In this case, it executes it here.
                            if (setLastDate == true) {
                                yearStr = Integer.toString(2000 + year);
                                monthStr = StringUtils.zeroFill(month, 2);
                                dayStr = StringUtils.zeroFill(day, 2);
                                hourStr = StringUtils.zeroFill(hour, 2);
                                endDates[iDates] = sdfLocalDateTime.parse(monthStr + "/" + dayStr + "/" +
                                        yearStr + " " + hourStr + ":00");
                                hourlyQty[iDates] = lastHourlyQty;
                                totalHours[iDates] = dailyHours;
                                dailyHours = 0;
                                iDates++;
                            }
                            break yearLoop;
                        }

                        //Make sure no extra processing is done on months less than 31 days.
                        if (day == DateUtils.getDaysInMonth(year,month)){
                            continue monthLoop;
                        }
                    }
                }
            }



        for (i=0; i<iDates; i++){
            /*if (startDtStr[i] == null)
                break;*/
            hs.put(startDates[i],endDates[i]);

        }
        return hs;
    }

    private String getAgreementType(String pCdty, int pOtcCdtLocId){
        String agreementType;
        agreementType = "";
        if (pCdty.equalsIgnoreCase("Power") && pOtcCdtLocId == ENGLAND_AND_WALES) {
            agreementType = "GTMA";
        }
        else if (pCdty.equalsIgnoreCase("Power")) {
            agreementType = "EFET";
        }
        else if (pCdty.equalsIgnoreCase("Gas") && pOtcCdtLocId == TTF) {
            agreementType = "EFET";
        }
        else if (pCdty.equalsIgnoreCase("Gas") && pOtcCdtLocId == NBP) {
            agreementType = "NBP97";
        }
        else if (pCdty.equalsIgnoreCase("Gas") && pOtcCdtLocId == ZEBRUGGE) {
            agreementType = "Zebrugge";
        }
        else if (pCdty.equalsIgnoreCase(EFET_DAO.EMISSION_PHASE_1) || pCdty.equalsIgnoreCase(EFET_DAO.EMISSION_PHASE_2) || pCdty.equalsIgnoreCase(EFET_DAO.EMISSION_PHASE_3)) {
            agreementType = "IETA";
        }
        else
            agreementType = "EFET";

        return agreementType;
    }

/*    public String getCanTradeXML(double pTradeID) throws SQLException {
        String xmlString = null;
        OracleCallableStatement statement = null;
        ResultSet rs = null;
        try {
            String callSqlStatement = "{call econfirm_v1.pkg_econfirm_xml.p_get_econfirm_cancel_xml(?,?)}";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.registerOutParameter(2, oracle.jdbc.driver.OracleTypes.CURSOR);
            statement.executeQuery();
            rs = statement.getCursor(2);
            while (rs.next()) {
                xmlString = rs.getString("xmlstring");
            }
        } finally {
            try {
                statement.close();
            } catch (SQLException e) { }
            statement = null;
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) { }
                rs = null;
            }
            //return xmlString;
        }
        return xmlString;
    }*/

    public static void main(String[] args) {
     }


}
