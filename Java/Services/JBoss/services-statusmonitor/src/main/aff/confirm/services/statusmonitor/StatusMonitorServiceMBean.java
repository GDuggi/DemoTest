package aff.confirm.services.statusmonitor;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;

public interface StatusMonitorServiceMBean extends TaskServiceMBean{

    //void executeTimerEventNow() throws StopServiceException;
    void setAutoRestartEnabled() throws StopServiceException;
    String resetMarginRespProcFlag() throws StopServiceException;
    /*String startEConfirmStatusPolling() throws StopServiceException;*/

    boolean getAutoRestartEnabled() throws StopServiceException;
    String getDSProcessorStatus() throws StopServiceException;

    

    String getEConfirmMonitor2State() throws StopServiceException;
    String getEConfirmStatusPollingState() throws StopServiceException;
    String getEConfirmTradeAlertStatus() throws StopServiceException;
    String getEConfirmTradeAlert2Status() throws StopServiceException;

    String getEFETMonitorState() throws StopServiceException;
    String getEFETStatusPollingState() throws StopServiceException;
    String getEFETTradeAlertStatus() throws StopServiceException;
    String getEFETTradeSubmitterStatus() throws StopServiceException;

    String getICTSProcessorState() throws StopServiceException;
    String getOpsTrackingTradeAlertStatus() throws StopServiceException;
    String getSTAProcessorStatus() throws StopServiceException;
    String getTALoggerStatus() throws StopServiceException;
    String getTrackingAlertState() throws StopServiceException;

    String getRTPResenderState() throws StopServiceException;
    String getRTPublisherStatus() throws StopServiceException;
    String getDBInfoState() throws StopServiceException;
    String getICTSDBInfoState() throws StopServiceException;
    String getIntegrityCheckServiceState() throws StopServiceException;
    String getDemurrageServiceState() throws StopServiceException;

    String getOpsTrackingFinalApproveServiceState() throws StopServiceException;
    String getOpsTrackingPriorityCalcServiceState() throws StopServiceException;

    String getMailNotifierState() throws StopServiceException;
    String getVaultAlertServiceStatus() throws StopServiceException;
    String getVaultImportServiceStatus() throws StopServiceException;
    String getEditValueDateNotifyServiceStatus() throws StopServiceException;
    String getEditAutoEntryNotifyServiceStatus() throws StopServiceException;

    String getOPS_TRACKING_ACTIVITY_ALERT() throws StopServiceException;

    String getDBInfoName();
    void setDBInfoName(String pDBInfoName);
    String getSmtpHost();
    void setSmtpHost(String smtpHost );
    String getSmtpPort();
    void setSmtpPort(String smtpPort);
    String getSendToAddress();
    void setSendToAddress(String pSendToAddress);

    String getEnv();
    void setEnv(String pEnv);

}
