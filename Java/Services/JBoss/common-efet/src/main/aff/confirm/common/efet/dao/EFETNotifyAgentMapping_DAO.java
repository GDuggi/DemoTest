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
public class EFETNotifyAgentMapping_DAO extends DAOBase {

    public EFETNotifyAgentMapping_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from efet.notify_agent_mapping";
    }

 
    public String getEICCode(String pNotifyAgentCode)
            throws Exception {
        String eicCode = "?";
        String notifyAgentCode = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            notifyAgentCode = crs.getString("NOTIFY_AGENT_CODE");
            if ( notifyAgentCode.equalsIgnoreCase(pNotifyAgentCode)) {
                eicCode = crs.getString("EIC_CODE");
                break;
            }
        }
        return eicCode;
    }

    public String getECVNAId(String pNotifyAgentCode)
            throws Exception {
        String ecvnaId = "?";
        String notifyAgentCode = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            notifyAgentCode = crs.getString("NOTIFY_AGENT_CODE");
            if ( notifyAgentCode.equalsIgnoreCase(pNotifyAgentCode)) {
                ecvnaId = crs.getString("ECVNA_ID");
                break;
            }
        }
        return ecvnaId;
    }

}
