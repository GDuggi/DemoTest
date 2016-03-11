package aff.confirm.common.efet.dao;

import aff.confirm.common.dao.DAOBase;

import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class EFETDeliveryPointArea_DAO extends DAOBase {

    public EFETDeliveryPointArea_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from efet.v_delivery_point_area_mapping";
    }

 
    public String getDeliveryPointArea(int pOtcLocCdtyId)
            throws Exception {
        String deliveryPointArea = "";
        int otcLocCdtyId = 0;
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            otcLocCdtyId = crs.getInt("OTC_LOC_CDTY_ID");
            if ( otcLocCdtyId == pOtcLocCdtyId) {
                deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
                break;
            }
        }
        return deliveryPointArea;
    }

    /*public boolean useHubCptyInfo(String pDeliveryPointArea) throws SQLException {
        boolean isHubCptyInfo = false;
        String deliveryPointArea = "";
        String useHubCptyInfoFlag = "N";
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea)) {
                useHubCptyInfoFlag = crs.getString("USE_HUB_CPTY_INFO_FLAG");
                isHubCptyInfo = useHubCptyInfoFlag.equalsIgnoreCase("Y");
                break;
            }
        }

        return isHubCptyInfo;
    }*/

    public int getDeliveryStartHour(String pDeliveryPointArea)
            throws Exception {
        int startHour = 0;
        String deliveryPointArea = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea)) {
                startHour = crs.getInt("DELIVERY_START_HOUR");
                break;
            }
        }
        return startHour;
    }

    public String getUseAffDeliveryTableFlag(String pDeliveryPointArea)
            throws Exception {
        String useAffDeliveryTableFlag = "?";
        String deliveryPointArea = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea)) {
                useAffDeliveryTableFlag = crs.getString("USE_AFF_DELIVERY_TABLE_FLAG");
                break;
            }
        }
        return useAffDeliveryTableFlag;
    }

    public String getFractionalCapacityUnit(int pOtcLocCdtyId)
            throws Exception {
        String fractionalCapacityUnit = "NONE";
        int otcLocCdtyId = 0;
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            otcLocCdtyId = crs.getInt("OTC_LOC_CDTY_ID");
            if ( otcLocCdtyId == pOtcLocCdtyId) {
                fractionalCapacityUnit = crs.getString("FRACTIONAL_CAPACITY_UNIT");
                break;
            }
        }
        return fractionalCapacityUnit;
    }

    public String getHourCalcForDayDeliveryFlag(String pDeliveryPointArea) throws Exception {

        String  hourCalcFlag = "N";
        String deliveryPointArea = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea)) {
                hourCalcFlag = crs.getString("HOURLY_VOL_DAY_DELIVERY_FLAG");
                break;
            }
        }
        return hourCalcFlag;
    }

    public String getOptionExprTimes(String pDeliveryPointArea) throws Exception {

        String exprFixedTime = "15:00:00";
        String exprDailyTime = "10:00:00";

        String deliveryPointArea = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            deliveryPointArea = crs.getString("DELIVERY_POINT_AREA");
            if ( deliveryPointArea.equalsIgnoreCase(pDeliveryPointArea)) {
                exprFixedTime = crs.getString("FIXED_OPTION_EXPIRY_TIME");
                if ( exprFixedTime == null) {
                    exprFixedTime = "15:00:00";
                }
                exprDailyTime = crs.getString("DAILY_OPTION_EXPIRY_TIME");
                if ( exprDailyTime == null) {
                    exprDailyTime = "12:00:00";
                }
                break;
            }
        }
        return exprFixedTime + "," + exprDailyTime;


    }

}
