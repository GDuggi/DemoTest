package aff.confirm.mbeans.inbound.attributematching.luceneservices;

import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.index.IndexWriter;

import java.io.File;
import org.jboss.logging.Logger;


/**
 * User: mthoresen
 * Date: Jun 18, 2009
 * Time: 1:22:33 PM
 */
public class Indexer {
    private static Logger log = Logger.getLogger(Indexer.class);

    public static int index(File indexDir, File dataDir) throws Exception {
        log.info("INSIDE INDEXER CONSTRUCTER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if(!dataDir.exists() || !dataDir.isDirectory()){
            throw new Exception(dataDir + " does not exist, or is not a valid directory.");
        }
        log.info("indexDir: " + indexDir.getName());
        log.info("dataDir: " + indexDir.getName());
        IndexWriter writer = new IndexWriter(indexDir, new StandardAnalyzer(), true, IndexWriter.MaxFieldLength.UNLIMITED);
        writer.setUseCompoundFile(false);

        indexDirectory(writer, dataDir);
        int numIndexed = writer.maxDoc();
        writer.optimize();
        writer.close();
        return numIndexed;
    }

    private static void indexDirectory(IndexWriter writer, File file) throws Exception{
        // do not try to index files that cannot be read
        if (file.canRead()) {
          if (file.isDirectory()) {
            String[] files = file.list();
            // an IO error could occur
            if (files != null) {
              for (int i = 0; i < files.length; i++) {
                indexDirectory(writer, new File(file, files[i]));
              }
            }
          } else {
              log.info("adding " + file);
            try {
              writer.addDocument(FileDocument.Document(file));
            }
            // at least on windows, some temporary files raise this exception with an "access denied" message
            // checking if the file can be read doesn't help
            catch (Exception ex) {
              throw ex;
            }
          }
        }
    }
}
