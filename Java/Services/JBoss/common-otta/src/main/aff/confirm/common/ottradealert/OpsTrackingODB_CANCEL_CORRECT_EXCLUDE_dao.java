package aff.confirm.common.ottradealert;

import aff.confirm.common.dao.DAOBase;

import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: June 26, 2009
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_dao extends DAOBase {

    public OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_dao(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from ops_tracking.odb_cancel_correct_exclude";
        refreshCRS();
    }


    public int isDoExclude(OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_rec pOTCxlCorrExcludeRec)
            throws SQLException, ParseException {
        int excludeTableId = 0;
        boolean auditTypeCode = false;
        boolean seCptySn = false;
        boolean cptySn = false;
        boolean colName = false;
        boolean bookSn = false;
        crs.beforeFirst();
        while (crs.next()) {
            auditTypeCode = isFieldOK(crs.getString("AUDIT_TYPE_CODE"), pOTCxlCorrExcludeRec.auditTypeCode);
            seCptySn = isFieldOK(crs.getString("SE_CPTY_SN"), pOTCxlCorrExcludeRec.seCptySn);
            cptySn = isFieldOK(crs.getString("CPTY_SN"), pOTCxlCorrExcludeRec.cptySn);
            colName = isFieldOK(crs.getString("COL_NAME"), pOTCxlCorrExcludeRec.colName);
            bookSn = isFieldOK(crs.getString("BOOK_SN"), pOTCxlCorrExcludeRec.bookSn);

            if (auditTypeCode && seCptySn && cptySn && colName && bookSn){
                excludeTableId = crs.getInt("ID");
                break;
            }
        }
        return excludeTableId;
    }

    private boolean isFieldOK(String pTableDataValue, String pTradeDataValue){
        boolean isOK = false;
        if (pTableDataValue == null)
            isOK = true;
        else if (pTableDataValue.equalsIgnoreCase(pTradeDataValue))
            isOK = true;

        return isOK;
    }

}