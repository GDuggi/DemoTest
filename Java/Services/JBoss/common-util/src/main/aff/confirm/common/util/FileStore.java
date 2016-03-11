package aff.confirm.common.util;

import org.jdom.Document;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;
import org.jdom.output.Format;
import org.jdom.output.XMLOutputter;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.StringReader;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

/**
 * User: ifrankel
 * Date: Mar 24, 2004
 * Time: 8:34:19 AM
 * This class maintains a directory structure for storing files.
 * It automatically deletes directories that have expired.
 */
public class FileStore {
    private final SimpleDateFormat sdfDirName = new SimpleDateFormat("dd-MMM-yy");
    private final SimpleDateFormat sdfTimeOnly = new SimpleDateFormat("hhmmss");
    private String rootDir;
    private int expireDays;
    private String hostName;
    private String baseDir;

    public FileStore(String pRootDir, String pServiceName, int pExpireDays)
            throws UnknownHostException, Exception {
        this.rootDir = pRootDir;
        this.expireDays = pExpireDays;
        hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        baseDir = rootDir + "\\" + hostName + "\\" + pServiceName;

        //Initialize the base directory.
        File fileBaseDir = new File(baseDir);
        if (!fileBaseDir.exists())
            fileBaseDir.mkdirs();
        if (!fileBaseDir.exists())
            throw new Exception("FileStore constructor: Could not create FileStore directories: " + baseDir);
    }

    private void removeExpiredDirectories(Date pDate) throws ParseException {
        //cal with the current date minus the expire days.
        Calendar cal = Calendar.getInstance();
        cal.setLenient(false);
        cal.setTime(pDate);
        cal.add(Calendar.DATE, expireDays);

        //setup the expDate to test below. It is set with
        //each iteration of the directory loop.
        File fileBaseDir = new File(baseDir);
        Calendar expCal = Calendar.getInstance();
        Date expDate = null;
        String[] files = fileBaseDir.list();
        if (files != null){
            //Iterate through all the directories in the base path.
            for (int i = 0; i < files.length; i++) {
                String fullSubDirPath = baseDir + "\\" + files[i];
                File fileSubDir = new File(fullSubDirPath);

                //set the expCal from the current directory being tested.
                expDate = sdfDirName.parse(files[i]);
                expCal.setTime(expDate);

                //If directory currently being tested is older than the current date minus the
                //expiration date, then first delete all files in the directory then
                //delete the directory itself.
                if (expCal.before(cal)) {
                    String[] subDirFiles = fileSubDir.list();
                    for (int j = 0; j < subDirFiles.length; j++) {
                        File fileToDelete = new File(fullSubDirPath + "\\" + subDirFiles[j]);
                        fileToDelete.delete();
                        fileToDelete = null;
                    }
                    fileSubDir.delete();
                }
                fileSubDir = null;
                fullSubDirPath = null;
            }
        }
        fileBaseDir = null;
        cal = null;
        expCal = null;

    }


    /**
     * 1. Determine new current directory.
     * 2. Clear out expired directories.
     * 3. Create new directory if doesn't exist.
     * @return full path of current directory
     */
    private String getCurrentDirectory(Date pDate) throws ParseException {
        String currentDirectory = "";
        String tipDirectory = sdfDirName.format(pDate);
        currentDirectory = baseDir + "\\" + tipDirectory;
        File fileCurrDir = new File(currentDirectory);
        if (!fileCurrDir.exists())
                fileCurrDir.mkdir();

        return currentDirectory;
    }


    public void storeAsTextFile(String pFileName, String pTextToStore)
            throws ParseException, IOException {
        Date now = new Date();
        removeExpiredDirectories(now);
        String currentDirectory = getCurrentDirectory(now);
        String filePathName = currentDirectory + "\\" + pFileName;
        FileWriter fileWriter = null;
        fileWriter = new FileWriter(filePathName);
        fileWriter.write(pTextToStore);
        fileWriter.flush();
        fileWriter.close();
        fileWriter = null;
    }


    /**
     * A Convenience class. Returns string used
     * for building a filename prior to calling method.
     * @return
     */
    public String getTimeStamp(){
        Date now = new Date();
        String currentTime = sdfTimeOnly.format(now);
        return currentTime;
    }


    public void storeXMLAsFile(String pXML, String pFilePathName)
            throws JDOMException, IOException {



        Document doc = null;
        SAXBuilder saxBuilder = null;
        saxBuilder = new SAXBuilder();
        doc = saxBuilder.build(new StringReader(pXML));
        XMLOutputter outputter = new XMLOutputter();
        Format format  = outputter.getFormat();
        format.setIndent("   ");
        format.setLineSeparator("\n");
        //outputter.setNewlines(true);
       // outputter.setIndent("   ");
        outputter.output(doc, new FileWriter(pFilePathName));
    }

    /*public void storeXMLAsFile(String pXML, String pFilePrefix, Date pFileNameDate)
            throws JDOMException, IOException, ParseException {
        String currentDirectory = getCurrentDirectory(pFileNameDate);
        String currentTime = sdfTimeOnly.format(pFileNameDate);
        //String filePathName = currentDirectory + "\\" + pFilePrefix + "_" + currentTime + ".xml";
        String filePathName = currentDirectory + "\\" + currentTime + "_" + pFilePrefix + ".xml";
        storeXMLAsFile(pXML,filePathName);
    }*/


    /*public void storeXMLAsFile(String pXML, String pFilePrefix)
            throws JDOMException, IOException, ParseException {
        Date now = new Date();
        String currentDirectory = getCurrentDirectory(now);
        String currentTime = sdfTimeOnly.format(new Date());
        String filePathName = "";
        filePathName = currentDirectory + "\\" + pFilePrefix + "_" + currentTime + ".xml";
        storeXMLAsFile(pXML,filePathName,"");
    }*/


}
