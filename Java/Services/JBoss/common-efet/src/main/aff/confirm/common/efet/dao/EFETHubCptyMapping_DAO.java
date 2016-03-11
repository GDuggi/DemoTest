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
public class EFETHubCptyMapping_DAO extends DAOBase {

    public EFETHubCptyMapping_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from efet.hub_cpty_mapping";
    }

 
    public String getCptyHubCode(String pDeliveryPointArea, String pCptyEicCode)
            throws Exception {
        String cptyHubCode = "?";
        String deliveryPointArea = "";
        String cptyEicCode = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            cptyEicCode = crs.getString("CPTY_EIC_CODE");

            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea) &&
                 cptyEicCode.equalsIgnoreCase(pCptyEicCode)) {
                cptyHubCode = crs.getString("CPTY_HUB_CODE");
                break;
            }
        }
        return cptyHubCode;
    }


}
