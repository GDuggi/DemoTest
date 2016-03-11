package aff.confirm.common.dao;

import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class JBossSettingsDAO extends DAOBase {
    //Service Names
    public final String SVC_ALL_SERVICES = "AllServices";
    public final String SVC_EC_STATUS_POLLING = "EConfirmStatusPollingService";

    //Setting Names
    public final String SET_PRODUCTION_HOST_NAME = "ProductionHostName";
    public final String SET_ELEC_NOTIFY_NAMES = "ElecNotifyNames";
    public final String SET_NGAS_NOTIFY_NAMES = "NgasNotifyNames";
    public final String SET_ALLCDTY_NOTIFY_NAMES = "AllCdtyNotifyNames";

    public JBossSettingsDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from ops_tracking.jboss_settings where ACTIVE_FLAG = 'Y'";
        refreshCRS();
    }


    public String getSettingValue(String pServiceName, String pSettingName) throws SQLException {
        String serviceName = "";
        String settingName = "";
        String settingValue = EMPTY_STRING;
        crs.beforeFirst();
        while (crs.next()) {
            serviceName = crs.getString("SERVICE_NAME");
            settingName = crs.getString("SETTING_NAME");
            if (serviceName.equalsIgnoreCase(pServiceName) &&
                settingName.equalsIgnoreCase(pSettingName) ) {
                settingValue = crs.getString("SETTING_VALUE");
                break;
            }
        }
        return settingValue;
    }

}
