/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.symphonyprocessor;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import java.sql.SQLException;
import java.io.File;

public interface SymphonyProcessorServiceMBean extends TaskServiceMBean{

    String getFileDropDir();
    void setFileDropDir(String pFileDropDir );

    String getProcessedFileDir();
    void setProcessedFileDir(String pProcessedFileDir);

    void setSTAQueueName(String STAQueueName);
    javax.management.ObjectName getSTAQueue() throws MalformedObjectNameException;

    void processFromTradeIDList(String pTradeIDList);

//    void setMSSqlDBInfoName(String pSymphonyDBInfoName);
//    ObjectName getMSSqlDBInfo() throws MalformedObjectNameException;

    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;

    void setTradeMessageWebServiceURL(String pTradeMessageWebServiceURL);
    String getTradeMessageWebServiceURL();

    void setTradeMessageRootTagName(String pTradeMessageRootTagName);
    String getTradeMessageRootTagName();

    //String getLogDbInfoName();
    //void setLogDbInfoName(String pLogDbInfoName);

    String getScanningMode();
    void setScanningMode(String pStartMode);

    String startScanning();
    String stopScanning();

    //String getAppFeedFolder();
    //void setAppFeedFolder(String appFeedFolder);

    String testDir();

    String publishSymphonyMessageIntoQueueFromIDCommaList(String pQueueName, String pCommaList) throws SQLException;
    String publishSymphonyMessageIntoQueueFromIDToID(String pQueueName, int pStartID, int pEndID) throws SQLException;

    String enterTradeIntoOpsManagerByTicketCommaList(String pTicketIdOrCommaList) throws SQLException;
    //String voidOpsManagerTradeByTicketCommaList(String pTicketIdOrCommaList) throws SQLException;

    void setPreserveXMLEnabled(boolean pPreserveXMLEnabled);
    boolean getPreserveXMLEnabled();

    void setIgnoreNonCriticalBlankFields(boolean pIgnoreCriticalBlankFields);
    boolean getIgnoreNonCriticalBlankFields();

    String getStopServiceNotifyAddress();
    void setStopServiceNotifyAddress(String pStopServiceNotifyAddress);

    String getSmtpHost();
    void setSmtpHost(String pSMTPHost);

    String getSmtpPort();
    void setSmtpPort(String pSMTPPort);
}
