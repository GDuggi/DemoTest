package aff.confirm.mbeans.inbound.attributematching.luceneservices;

import org.apache.lucene.document.DateTools;
import org.apache.lucene.document.Document;
import org.apache.lucene.document.Field;

import java.io.File;
import java.io.FileReader;

/**
 * User: mthoresen
 * Date: Jun 18, 2009
 * Time: 12:36:39 PM
 */
public class FileDocument {
    public FileDocument() {}

    public static Document Document(File f) throws java.io.FileNotFoundException {
        Document doc =  new Document();
        doc.add(new Field("path", f.getPath(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        doc.add(new Field("modified",DateTools.timeToString(f.lastModified(), DateTools.Resolution.MINUTE),
                Field.Store.YES, Field.Index.NOT_ANALYZED));
        doc.add(new Field("contents", new FileReader(f)));

        return doc;
    }
}
