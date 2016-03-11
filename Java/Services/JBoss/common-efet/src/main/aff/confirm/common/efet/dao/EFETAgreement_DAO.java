package aff.confirm.common.efet.dao;

import aff.confirm.common.dao.DAOBase;

import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: April 18, 2005
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class EFETAgreement_DAO extends DAOBase {

    public EFETAgreement_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from efet.v_agreement";
        //Israel 7/9/15 Needs to execute each time agreement is checked so user doesn't have to
        //restart change every time the table is updated. Moved call to method below.
        //refreshCRS();
    }

 
    public boolean isAgreementExist(int pCmpnyMstrId, int pCptyMstrId, String pCdtyCode,
                                    String pInstType, String pLocationSN, java.sql.Date pTradeDt,
                                    String pEntityType,String pTradeTypeCode)
            throws Exception {
        boolean agreementExists = false;
        int companyMstrId = -1;
        int cptyMstrId = -1;
        String cdtyCode = "";
        String instType = "";
        String locationSN = "";
        java.sql.Date goLiveDt;
        String entityType = "";
        String tradeTypeCode = "";

        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            companyMstrId = crs.getInt("COMPANY_MSTR_ID");
            cptyMstrId = crs.getInt("CPTY_MSTR_ID");
            cdtyCode = crs.getString("CDTY_CODE");
            instType = crs.getString("INST_TYPE");
            locationSN = crs.getString("LOCATION_SN");
            goLiveDt = getDateFromCRSString("LIVE_TRADE_DT");
            entityType = crs.getString("ENTITY_TYPE");
            tradeTypeCode = crs.getString("TRADE_TYPE_CODE");
            if (tradeTypeCode == null ) {
                tradeTypeCode = "ENRGY";
            }
            //goLiveDt = crs.getDate("LIVE_TRADE_DT");

            if ( companyMstrId == pCmpnyMstrId &&
                 cptyMstrId == pCptyMstrId &&
                 cdtyCode.equalsIgnoreCase(pCdtyCode) &&
                 instType.equalsIgnoreCase(pInstType) &&
                 locationSN.equalsIgnoreCase(pLocationSN) &&
                 pTradeDt.compareTo(goLiveDt) >= 0 &&
                 entityType.equalsIgnoreCase(pEntityType) &&
                 tradeTypeCode.equalsIgnoreCase(pTradeTypeCode) ) {
                 agreementExists = true;
                break;
            }
        }
        return agreementExists;
    }


}
