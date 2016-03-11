package aff.confirm.services.inboundpreprocessor;



import java.io.*;
import java.sql.*;
import java.util.*;
import java.util.Date;
import java.util.regex.Pattern;
import java.util.regex.Matcher;


import org.jboss.logging.Logger;

/**
 * User: srajaman
 * Date: Mar 8, 2007
 * Time: 10:25:05 AM
 */
public class InBoundDocProcessor {
    private static Logger log = Logger.getLogger(InBoundDocProcessor.class.getName());

    private static final String sql = "Select distinct trade_id from ops_tracking.v_awaited_docs where rqmt_event_dt >= trunc(sysdate)-? ";
    private static final String _update_proc = "{call ops_tracking.pkg_inbound.p_update_assn_doc(?,?,?)}";

    private  Vector awaitedTrades = null;
    private  Date lastRefreshTime = null;
    

    public Vector processDirectory(Connection conn,String scanDir,String archiveDir,String errorDir ,String tifDir, boolean isArchive,String searchPatternList,int days) throws SQLException,FileNotFoundException,IOException{



        Vector errorList = new Vector();
        getAwaitedDocs(conn,days);
        if (awaitedTrades == null || awaitedTrades.size() <= 0) {
           return errorList;
        }

        File dir = new File(scanDir);
        if (!dir.isDirectory()){
            return errorList;
        }
        /*
        FileFilter filter = new FileFilter() {
             public boolean accept(File file) {
                 return file.isFile() && file.getName().toLowerCase().endsWith("txt");
             }
        };
        */

        FileFilter filter = new FileFilter() {
             public boolean accept(File file) {
                 return file.isFile() ;
             }
        };
        
        File[] files = dir.listFiles(filter);
        int fileCount = files.length;
        for (int i=0;i<fileCount; i++){
            File currentFile = files[i];

            
            String fileName = currentFile.getName();
            FileReader reader = null;
            int bufReadSize;
            Logger.getLogger(this.getClass()).info("Processing File name = " + scanDir + "\\" +fileName);
            try {
                // if the file is tif or non text file , just move the file
                // into different folder

               if (currentFile.getName().toLowerCase().endsWith(".txt"))  {
                    StringBuffer stringBuf = new StringBuffer();
                    reader = new FileReader(currentFile);
                    BufferedReader bufReader = new BufferedReader(reader);
                    char[] buf  = new char[1024];
                    while (bufReader.ready()){
                        bufReadSize =bufReader.read(buf);
                        stringBuf.append(buf,0,bufReadSize);
                    }
                    reader.close();
                    Vector tradeIdList = searchTradeId(stringBuf,searchPatternList);
                    if ("Y".equalsIgnoreCase(updateDb(conn,tradeIdList,fileName))) {
                        archiveFile(currentFile.getAbsolutePath(),stringBuf,archiveDir,isArchive);
                    }
                    else { // put it in the error directory
                        archiveFile(currentFile.getAbsolutePath(),stringBuf,errorDir,true);
                        errorList.add(currentFile.getName());
                    }
               }
               else { // not a text file, so just move the files
                   if (!currentFile.getName().toLowerCase().endsWith(".blk")) {
                        moveFile(currentFile.getAbsolutePath(),tifDir);
                   }
               }

            } catch (FileNotFoundException e) {
                Logger.getLogger(this.getClass()).error(e.getStackTrace());
                throw e;
            } catch (IOException e) {
                Logger.getLogger(this.getClass()).error(e.getStackTrace());
                throw e;
            }

        }
        return errorList;

    }
    private void moveFile(String fullFileName, String dir) {
        File f = new File(fullFileName);
        if ( f.exists()) {

            File descFile = new File(dir,f.getName());
            if (descFile.exists()){
                descFile.delete();
            }
            f.renameTo(descFile);
        }
    }
    
    private void archiveFile(String fullFileName,StringBuffer stringBuf ,String archiveDir, boolean archive) {

        try {
            if (archive){
                File file = new File(fullFileName);
                String fileName = file.getName();
           // check for dest file exists, if so deletes.
                String destFileName = archiveDir + "\\" + fileName;
                FileWriter writer = new FileWriter(destFileName);
                BufferedWriter bufWriter = new BufferedWriter(writer);
                bufWriter.write(stringBuf.toString());
                bufWriter.close();
                writer.close();
            }
            File file = new File(fullFileName);
            file.delete();
        }
        catch (IOException e){
              log.error( "ERROR", e );
        }

    }

    private String updateDb(Connection conn, Vector tradeIdList, String fileName) throws SQLException {

        String returnInd = "Y";
        if ( tradeIdList == null || tradeIdList.isEmpty()){
            return returnInd;
        }
        String tifFileName = getTifFileName(fileName);
        Collections.sort(tradeIdList);

        CallableStatement cstmt = null;

        try {
            cstmt = conn.prepareCall(_update_proc);
            cstmt.registerOutParameter(3,java.sql.Types.VARCHAR);

            Iterator it= tradeIdList.iterator();
            String prevTradeId = "";
            while (it.hasNext()){
                 String currentTradeId = (String) it.next();
                if (!prevTradeId.equalsIgnoreCase(currentTradeId)){
                     int tradeId = Integer.parseInt(currentTradeId,10);
                    cstmt.setInt(1,tradeId);
                    cstmt.setString(2,tifFileName);
                    cstmt.execute();
                    returnInd = cstmt.getString(3);
                    if ( "N".equalsIgnoreCase(returnInd)) {
                         break;
                    }
                }
                prevTradeId = currentTradeId;
            }
        }
        finally {
            if (cstmt != null){
                try {
                    cstmt.close();
                    cstmt = null;
                }
                catch ( SQLException e){

                }

            }
        }
        return returnInd;

        
    }

    private String getTifFileName(String fileName) {

        int extPos = fileName.lastIndexOf(".");
        String tifFileName = fileName.substring(0,extPos) + ".tif" ;
        return tifFileName;

    }


    private Vector searchTradeId(StringBuffer stringBuf, String searchPatternList) throws SQLException {

        Vector tradeIdList = new Vector();
        boolean isFound = false;
        String sFileContent = stringBuf.toString();

        if (awaitedTrades != null && awaitedTrades.size() > 0) {
            int listSize =  awaitedTrades.size();
            for (int i =0;i<listSize ; ++i) {
                String strTradeId =  (String) awaitedTrades.get(i);
                String tradeIdPattern = searchPatternList.replaceAll("~TradeId~",strTradeId);
                Pattern pattern = Pattern.compile(tradeIdPattern,Pattern.CASE_INSENSITIVE);

               // Logger.getLogger(this.getClass()).info("Search Pattern = " + tradeIdPattern);
              //  Logger.getLogger(this.getClass()).info("Incoming Trade Id = " + strTradeId);
                Matcher match = pattern.matcher(sFileContent);
                if (match.find()) {
                    Logger.getLogger(this.getClass()).info("Trade Id " + strTradeId + " is found in the awaited docs " );
                    tradeIdList.add(strTradeId);
                }
            /*
            int pos = sFileContent.indexOf(strTradeId);
            if (pos > 0 ) {
               int endPos = pos + strTradeId.length() -1;
               if (endPos < sFileContent.length()) {
                   endPos++;
               }
               String nextChar = sFileContent.substring(pos-1,endPos+1);
               System.out.println("Incoming Trading id =" + strTradeId +"; Padded = " + nextChar); 
               if ( nextChar.trim().equalsIgnoreCase(strTradeId)) {
                Logger.getLogger(this.getClass()).info("Trade Id " + strTradeId + " is in found in the awaited docs? " );
                tradeIdList.add(strTradeId);
               }
             }
               */

            }
        }
        return tradeIdList;

    }

    private void getAwaitedDocs(Connection conn,int days) throws SQLException{


       PreparedStatement statement =null;
       ResultSet rs = null;

        if ( isDataRefreshed() ) {
            try {
                 awaitedTrades = new Vector();
                 statement = conn.prepareStatement(sql);
                 statement.setInt(1,days);
                 rs = statement.executeQuery();
                 Logger.getLogger("InBoundDocProcessor").info("Awaited Trades from Db are loading...");
                 while (rs.next()) {
                     int tradeId = rs.getInt(1);
                     String strTradeId = "" + tradeId;
                     awaitedTrades.add(strTradeId);
                  }
                  Logger.getLogger("InBoundDocProcessor").info("Awaited Trades from Db load is complete.");
                  lastRefreshTime = new Date();
                }
                catch (Exception e) {
                    log.error( "ERROR", e );
                    throw new SQLException(e.getMessage(), e);
                }
                finally {
                    try {
                        if (rs != null) { rs.close();}
                        if (statement != null) { statement.close();}
                    }
                    catch (SQLException e){

                    }
                }

      }

    }

    private boolean isDataRefreshed() {
        if ( awaitedTrades == null ) {
            return true;
        }
        if ( lastRefreshTime == null ) {
            return true;
        }
        Date now = new Date();
        if ( now.getTime() - lastRefreshTime.getTime() > 60 * 60 * 1000 ) {
            Logger.getLogger("InBoundDocProcessor").info("Set to refresh the data");
            return true;
        }
        return false;

    }
}
