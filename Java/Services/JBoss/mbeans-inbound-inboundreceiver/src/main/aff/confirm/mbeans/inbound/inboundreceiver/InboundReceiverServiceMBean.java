package aff.confirm.mbeans.inbound.inboundreceiver;

import aff.confirm.mbeans.common.CommonListenerServiceMBean;

/**
 * User: mthoresen
 * Date: Jun 23, 2009
 * Time: 2:20:54 PM
 */
public interface InboundReceiverServiceMBean extends CommonListenerServiceMBean {
    String getFileScanDir();
    void setFileScanDir(String fileScanDir);

    String getFileFailedDir();
    void setFileFailedDir(String fileFailedDir);

    String getFileProcessedDir();
    void setFileProcessedDir(String fileProcessedDir);

    String getFileDiscardDir();
    void setFileDiscardDir(String fileDiscardDir);

    String getOcrScanDir();
    void setOcrScanDir(String ocrScanDir);

    String getVaultUrl();
    void setVaultUrl(String vaultUrl);

    String getUserName();
    void setUserName(String userName);

    String getPassword();
    void setPassword(String password);

    String getDomain();
    void setDomain(String domain);

    String getHost();
    void setHost(String host);

    String getVaultFolderName();
    void setVaultFolderName(String vaultFolderName);

    String getFieldNames();
    void setFieldNames(String fieldNames);

    String getDslName();
    void setDslName(String dslName);
    
}
