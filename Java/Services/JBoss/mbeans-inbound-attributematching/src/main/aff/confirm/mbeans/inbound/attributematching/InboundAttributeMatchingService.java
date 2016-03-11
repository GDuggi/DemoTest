package aff.confirm.mbeans.inbound.attributematching;

import aff.confirm.common.daoinbound.inbound.ejb3.InbAttribDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.InbAttribMapPhraseDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.InbAttribMapValDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.InboundDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.InbAttribEntity;
import aff.confirm.common.daoinbound.inbound.model.InbAttribMapPhraseEntity;
import aff.confirm.common.daoinbound.inbound.model.InbAttribMapValEntity;
import aff.confirm.common.daoinbound.inbound.model.InboundDocsEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.mbeans.common.CommonListenerService;
import aff.confirm.mbeans.common.exceptions.StopServiceException;
import org.apache.lucene.analysis.Analyzer;
import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.document.DateTools;
import org.apache.lucene.document.Document;
import org.apache.lucene.document.Field;
import org.apache.lucene.index.IndexReader;
import org.apache.lucene.index.IndexWriter;
import org.apache.lucene.queryParser.QueryParser;
import org.apache.lucene.search.IndexSearcher;
import org.apache.lucene.search.Query;
import org.apache.lucene.search.ScoreDoc;
import org.apache.lucene.search.TopDocs;
import org.jboss.logging.Logger;

import javax.naming.InitialContext;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;


/**
 * User: mthoresen
 * Date: Jun 24, 2009
 * Time: 1:54:45 PM
 */
public class InboundAttributeMatchingService extends CommonListenerService implements InboundAttributeMatchingServiceMBean {
    private static Logger log = Logger.getLogger(InboundAttributeMatchingService.class);

    private InboundDocsDAOLocal inboundDocsDAOBean;
    private InbAttribMapPhraseDAOLocal inbAttribMapPhraseDAOBean;
    private InbAttribMapValDAOLocal inbAttribMapValDAOBean;
    private InbAttribDAOLocal inbAttribDAOBean;
    //    private InitialContext ctx;
    private List<String> fileTypes;

    // Attributes
    private String fileScanDir;
    private String fileFailedDir;
    private String fileProcessedDir;
    private String fileDiscardDir;
    private String luceneDataDir;
    private String luceneIndexDir;
    private String indexFileTypes;

    public InboundAttributeMatchingService() {
        super("affinity.inbound:service=InboundAttributeMatchingService");
    }

    public String getIndexFileTypes() {
        return indexFileTypes;
    }

    public void setIndexFileTypes(String indexFileTypes) {
        this.indexFileTypes = indexFileTypes;
    }

    public String getLuceneDataDir() {
        return luceneDataDir;
    }

    public void setLuceneDataDir(String luceneDataDir) {
        this.luceneDataDir = luceneDataDir;
    }

    public String getLuceneIndexDir() {
        return luceneIndexDir;
    }

    public void setLuceneIndexDir(String luceneIndexDir) {
        this.luceneIndexDir = luceneIndexDir;
    }

    public String getFileScanDir() {
        return this.fileScanDir;
    }

    public void setFileScanDir(String fileScanDir) {
        this.fileScanDir = fileScanDir;
    }

    public String getFileFailedDir() {
        return this.fileFailedDir;
    }

    public void setFileFailedDir(String fileFailedDir) {
        this.fileFailedDir = fileFailedDir;
    }

    public String getFileProcessedDir() {
        return this.fileProcessedDir;
    }

    public void setFileProcessedDir(String fileProcessedDir) {
        this.fileProcessedDir = fileProcessedDir;
    }

    public String getFileDiscardDir() {
        return this.fileDiscardDir;
    }

    public void setFileDiscardDir(String fileDiscardDir) {
        this.fileDiscardDir = fileDiscardDir;
    }

    @Override
    protected void initService() throws Exception {
        try {
            super.initService();
            StringTokenizer st = null;
            InitialContext ctx;
            int i = 0;

            while (i < 10) {
                try {
                    i++;
                    inboundDocsDAOBean = JndiUtil.lookup("java:global/InboundDocsDAOLib/InboundDocsDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.InboundDocsDAOLocal");
                    inbAttribMapValDAOBean = JndiUtil.lookup("java:global/InboundDocsDAOLib/InbAttribMapValDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.InbAttribMapValDAOLocal");
                    inbAttribMapPhraseDAOBean = JndiUtil.lookup("java:global/InboundDocsDAOLib/InbAttribMapPhraseDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.InbAttribMapPhraseDAOLocal");
                    inbAttribDAOBean = JndiUtil.lookup("java:global/InboundDocsDAOLib/InbAttribDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.InbAttribDAOLocal");
                    break;
                } catch (Exception e) {
                    if (i >= 9) {
                        log.error("Error: stopping service...", e );
                        throw e;
                    }
                    Thread.sleep(2 * 1000);
                }
            }
            initDirectories();
            st = new StringTokenizer(indexFileTypes, ",");
            fileTypes = new ArrayList<String>();
            while (st.hasMoreTokens()) {
                fileTypes.add(st.nextToken());
            }


        } catch (Exception e2) {
            log.error("initService: " + e2.getMessage());
        }
    }

    @Override
    protected void executeTimerEvent() throws StopServiceException {
        super.executeTimerEvent();
        List fileList = getFileList(fileScanDir);
        // Prep for indexing
        if (fileList.size() > 0) {
            moveFiles(fileList, fileScanDir, luceneDataDir, fileDiscardDir, fileTypes);
            createIndex();
            processFiles();
            fileList = getFileList(luceneDataDir);
            moveFiles(fileList, luceneDataDir, fileProcessedDir, fileDiscardDir, fileTypes);
            updateProcessedFlag(fileList);
        }
    }

    private void updateProcessedFlag(List fileList) {
        String tifFile;
        for (Object aFileList : fileList) {
            File sourceFile = (File) aFileList;
            tifFile = sourceFile.getName().replace(".txt", ".tif");
            InboundDocsEntity template = new InboundDocsEntity();
            List<InboundDocsEntity> list;
            template.setFileName(tifFile);
            list = inboundDocsDAOBean.findByExample(template, "");
            for (InboundDocsEntity inbDoc : list) {
                inbDoc.setProcFlag("Y");
                inboundDocsDAOBean.makePersistent(inbDoc);
            }
        }
    }

    private void processFiles() {
        InbAttribMapPhraseEntity mapPhraseTemplate = new InbAttribMapPhraseEntity();
        InbAttribMapValEntity mapValTemplate = new InbAttribMapValEntity();
        List<InbAttribEntity> attributeTypes;
        List<InbAttribMapPhraseEntity> phrases;
        List<InbAttribMapValEntity> mappedValues;
        List<String> matched;

        attributeTypes = inbAttribDAOBean.findAll();
        for (InbAttribEntity attributeType : attributeTypes) {
            mapValTemplate.setInbAttribCode(attributeType.getCode());
            mappedValues = inbAttribMapValDAOBean.findByExample(mapValTemplate, "");

            for (InbAttribMapValEntity mappedValue : mappedValues) {
                mapPhraseTemplate.setInbAttribMapValId(mappedValue.getId());
                phrases = inbAttribMapPhraseDAOBean.findByExample(mapPhraseTemplate, "");
                if (phrases.size() > 0) {
                    matched = findMatchingDocuments(phrases);
                    processMatchedDocuments(matched, mappedValue);
                }
            }
        }
    }

    private void processMatchedDocuments(List<String> matched, InbAttribMapValEntity mappedValue) {
        InboundDocsEntity template = new InboundDocsEntity();
        List<InboundDocsEntity> list;
        for (String fileName : matched) {
            File file = new File(fileName);
            template.setFileName(file.getName().replace(".txt", ".tif"));
            list = inboundDocsDAOBean.findByExample(template, "");
            for (InboundDocsEntity inbDoc : list) {
                inbDoc.setMappedValue(mappedValue.getInbAttribCode(), mappedValue.getMappedValue());
                inboundDocsDAOBean.makePersistent(inbDoc);
            }
        }
    }

    private List<String> findMatchingDocuments(List<InbAttribMapPhraseEntity> phrases) {
        List<String> fileNames = new LinkedList();
        IndexReader reader = null;
        try {
            reader = IndexReader.open(luceneIndexDir);
            IndexSearcher searcher = new IndexSearcher(reader);

            Analyzer analyzer = new StandardAnalyzer();
            final BitSet bits = new BitSet(reader.maxDoc());

            if (reader.maxDoc() > 0) {
                String q = buildSearchQuery(phrases);
                QueryParser parser = new QueryParser("contents", analyzer);
                parser.setDefaultOperator(QueryParser.AND_OPERATOR);
                Query query = parser.parse(q);

                TopDocs topDocs = searcher.search(query, reader.maxDoc());

                for (ScoreDoc scoreDoc : topDocs.scoreDocs) {
                    Document doc = reader.document(scoreDoc.doc);
                    fileNames.add(doc.getField("path").stringValue());
                }
            }
            return fileNames;
        } catch (Exception e) {
            log.error( "ERROR", e );
        }
        return fileNames;
    }

    private String buildSearchQuery(List<InbAttribMapPhraseEntity> phrases) {
        String query = "";
        for (InbAttribMapPhraseEntity phrase : phrases) {
            query = query + "\"" + phrase.getPhrase() + "\" ";
        }
        return query.trim();
    }

    private void createIndex() throws StopServiceException {
        File indexDir = new File(luceneIndexDir);
        File indexData = new File(luceneDataDir);

        IndexWriter writer;

        if (!indexData.exists() || !indexData.isDirectory()) {
            throw new StopServiceException(indexData + " does not exist, or is not a valid directory.");
        }
        try {
            writer = new IndexWriter(indexDir, new StandardAnalyzer(), true, IndexWriter.MaxFieldLength.UNLIMITED);
            writer.setUseCompoundFile(false);
            indexDirectory(writer, indexData);
            writer.optimize();
            writer.close();
        } catch (IOException e) {
            throw new StopServiceException(e.getMessage());
        }
    }

    private void indexDirectory(IndexWriter writer, File dir) throws StopServiceException {
        File[] files = dir.listFiles();

        for (int i = 0; i < files.length; i++) {
            File file = files[i];
            if (file.isDirectory()) {
                indexDirectory(writer, file);
            } else indexFile(writer, file);
        }
    }

    private void indexFile(IndexWriter writer, File f) {
        try {
            Document doc = new Document();
            doc.add(new Field("path", f.getPath(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.add(new Field("modified", DateTools.timeToString(f.lastModified(), DateTools.Resolution.MINUTE),
                    Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.add(new Field("contents", new FileReader(f)));
            writer.addDocument(doc);
            printInfo(f.getName() + " Has been indexed for searching.");
        } catch (Exception e) {
            printInfo(e.getMessage());
        }
    }

    private void initDirectories() {
        File fileDir = new File(this.fileScanDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileFailedDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileProcessedDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileDiscardDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }

        fileDir = new File(this.luceneDataDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }

        fileDir = new File(this.luceneIndexDir);
        if (!fileDir.exists()) {
            fileDir.mkdirs();
        }
    }

    class FileComparator implements Comparator {
        public int compare(Object o1, Object o2) {
            long modified1 = ((File) o1).lastModified();
            long modified2 = ((File) o2).lastModified();
            if (modified1 == modified2)
                return 0;
            else {
                if (modified1 < modified2)
                    return -1;
                else
                    return 1;
            }
        }
    }

    public static void main(String[] args) {
/*
        InitialContext ctx;
        InbAttribMapValDAORemote bean;
        List<InbAttribMapValEntity> list;
        try {
            Properties env = new Properties( );
            env.put(Context.PROVIDER_URL, "localhost:1099");
            env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
            env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
            ctx = new InitialContext(env);

            bean = (InbAttribMapValDAORemote) ctx.lookup("InbAttribMapValDAOBean/remote");
            list = bean.findAll();

            System.out.println("Total Found: " + list.size());
        } catch (Exception e) {
            e.printStackTrace();
        }
*/
        // lucene testing...
    }

}
