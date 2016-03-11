package aff.confirm.mbeans.inbound.attributematching.luceneservices;

import org.apache.lucene.analysis.Analyzer;
import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.index.IndexReader;
import org.apache.lucene.queryParser.ParseException;
import org.apache.lucene.queryParser.QueryParser;
import org.apache.lucene.search.HitCollector;
import org.apache.lucene.search.IndexSearcher;
import org.apache.lucene.search.Query;

import java.io.File;
import java.io.IOException;
import java.util.BitSet;

/**
 * User: mthoresen
 * Date: Jun 18, 2009
 * Time: 1:37:34 PM
 */
public class Searcher {
    public static void search(File indexDir, String field, String q) throws IOException, ParseException {

        IndexReader reader = IndexReader.open(indexDir);

        IndexSearcher searcher = new IndexSearcher(reader);

        Analyzer analyzer = new StandardAnalyzer();
        final BitSet bits = new BitSet(reader.maxDoc());


        QueryParser parser = new QueryParser(field, analyzer);
        Query query = parser.parse(q);

        searcher.search(query, new HitCollector() {
            public void collect(int doc, float score) {
              bits.set(doc);
            }
          });
    }
}
