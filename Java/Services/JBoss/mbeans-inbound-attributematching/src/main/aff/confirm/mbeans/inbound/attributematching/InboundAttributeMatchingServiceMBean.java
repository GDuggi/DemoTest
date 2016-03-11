package aff.confirm.mbeans.inbound.attributematching;

import aff.confirm.mbeans.common.CommonListenerServiceMBean;

/**
 * User: mthoresen
 * Date: Jun 24, 2009
 * Time: 1:51:24 PM
 */
public interface InboundAttributeMatchingServiceMBean extends CommonListenerServiceMBean {
    String getFileScanDir();
    void setFileScanDir(String fileScanDir);

    String getFileFailedDir();
    void setFileFailedDir(String fileFailedDir);

    String getFileProcessedDir();
    void setFileProcessedDir(String fileProcessedDir);

    String getFileDiscardDir();
    void setFileDiscardDir(String fileDiscardDir);

    String getLuceneDataDir();
    void setLuceneDataDir(String luceneDataDir);

    String getLuceneIndexDir();
    void setLuceneIndexDir(String luceneIndexDir);

    String getIndexFileTypes();
    void setIndexFileTypes(String indexFileTypes);
}
