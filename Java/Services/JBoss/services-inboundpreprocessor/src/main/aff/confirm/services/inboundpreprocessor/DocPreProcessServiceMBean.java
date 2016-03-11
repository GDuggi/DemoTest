package aff.confirm.services.inboundpreprocessor;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import aff.confirm.jboss.common.exceptions.StopServiceException;

/**
 * User: srajaman
 * Date: Mar 6, 2007
 * Time: 11:07:27 AM
 */
public interface DocPreProcessServiceMBean extends TaskServiceMBean {
       String getScanDirectory();
       void setScanDirectory(String scanDir);
       boolean getArchive();
       void setArchive(boolean archive);
       void setSmtpHost(String smtpHost);
       String getSmtpHost();
       String getSmtpPort();
       void setSmtpPort(String port);
       void setArchiveDirectory(String dir);
       String getArchiveDirectory();
       void setErrorDirectory(String dir);
       String getErrorDirectory();
       void setSearchPattern(String searchPattern);
       String getSearchPattern();
       void setTifDirectory(String tifDir);
       String getTifDirectory();
    
       void setTradeSearchDuration(Integer tradeSearch);
       Integer getTradeSearchDuration(); 

       void  processDirectoryNow() throws StopServiceException;

    String getEnv();
    void setEnv(String pEnv);


}
