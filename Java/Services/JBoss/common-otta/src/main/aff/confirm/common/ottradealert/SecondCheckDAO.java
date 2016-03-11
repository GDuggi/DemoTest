package aff.confirm.common.ottradealert;

import aff.confirm.common.dao.DAOBase;

import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class SecondCheckDAO extends DAOBase {

    public SecondCheckDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from ops_tracking.second_check";
        refreshCRS();
    }


    public boolean isDoSecondCheck(OpsTrackingSecondCheck_rec pOTSecondCheck_rec)
            throws SQLException, ParseException {
        boolean doSecondCheck = false;
        boolean rqmtCode = false;
        boolean seCptySn = false;
        boolean cptySn = false;
        boolean cdtyCode = false;
        boolean sttlTypeCode = false;
        crs.beforeFirst();
        while (crs.next()) {
            rqmtCode = isFieldOK(pOTSecondCheck_rec.rqmtCode, crs.getString("RQMT_CODE"));
            seCptySn = isFieldOK(pOTSecondCheck_rec.seCptySn, crs.getString("SE_CPTY_SN"));
            cptySn = isFieldOK(pOTSecondCheck_rec.cptySn, crs.getString("CPTY_SN"));
            cdtyCode = isFieldOK(pOTSecondCheck_rec.cdtyCode, crs.getString("CDTY_CODE"));
            sttlTypeCode = isFieldOK(pOTSecondCheck_rec.sttlType, crs.getString("STTL_TYPE_CODE"));

            if (rqmtCode && seCptySn && cptySn && cdtyCode && sttlTypeCode){
                doSecondCheck = true;
                break;
            }
        }
        return doSecondCheck;
    }

    private boolean isFieldOK(String pTradeDataValue, String pTableDataValue){
        boolean isOK = false;
        if (pTableDataValue == null)
            isOK = true;
        else if (pTableDataValue.equalsIgnoreCase(pTradeDataValue))
            isOK = true;

        return isOK;
    }

}
