package aff.confirm.mbeans.inbound.vaultprocessor;

import aff.confirm.mbeans.common.CommonListenerServiceMBean;

/**
 * User: mthoresen
 * Date: Aug 31, 2009
 * Time: 11:48:10 AM
 */
public interface VaultProcessorServiceMBean extends CommonListenerServiceMBean {
    String getFileScanDir();
    void setFileScanDir(String fileScanDir);

    String getFileFailedDir();
    void setFileFailedDir(String fileFailedDir);

    String getFileProcessedDir();
    void setFileProcessedDir(String fileProcessedDir);

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

    String getFileTypes();
    void setFileTypes(String fileTypes);

    String getVaultFolderName();
    void setVaultFolderName(String vaultFolderName);

    String getFieldNames();
    void setFieldNames(String fieldNames);

    String getDslName();
    void setDslName(String dslName);

    String getChkFnlApprvFlag();
    void setChkFnlApprvFlag(String chkFnlApprvFlag);

    String getLocationCode();
    void setLocationCode(String locationCode);
}
