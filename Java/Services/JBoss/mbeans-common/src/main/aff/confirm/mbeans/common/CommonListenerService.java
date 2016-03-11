package aff.confirm.mbeans.common;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.mbeans.common.exceptions.StopServiceException;
import org.jboss.logging.Logger;

import javax.mail.Message;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;

/**
 * User: mthoresen
 * Date: Jun 19, 2009
 * Time: 11:15:04 AM
 */
public abstract class CommonListenerService extends TaskService implements CommonListenerServiceMBean {
    private boolean processing = false;
    private int terminalErrorCount;
    private boolean stopServiceException = false;
    private int currentErrorCount;
    private String emailFrom;
    private String emailTo;
    private Logger mylogger = Logger.getLogger(this.getClass().getSimpleName());
    private int loggingCounter = 0;

    public CommonListenerService(String objectNameStr) {
        super(objectNameStr);
    }

    public String getHostName() {
        try {
            return InetAddress.getLocalHost().getHostName().toUpperCase();
        } catch (UnknownHostException e) {
            printError("ERROR getting hostname", e);
            return "";
        }
    }

    public boolean isProcessing() {
        return processing;
    }

    public int getTerminalErrorCount() {
        return terminalErrorCount;
    }

    public void setTerminalErrorCount(int errorCount) {
        this.terminalErrorCount = errorCount;
    }

    public String getEmailFrom() {
        return this.emailFrom;
    }

    public void setEmailFrom(String emailFrom) {
        this.emailFrom = emailFrom;
    }

    public String getEmailTo() {
        return this.emailTo;
    }

    public void setEmailTo(String emailTo) {
        this.emailTo = emailTo;
    }

    protected void printInfo(String msg) {
        mylogger.info(msg);
    }

    protected void printError(String msg, Throwable t) {
        Logger.getLogger(this.getClass().getName()).log(Logger.Level.ERROR, msg, t);
    }

    public void executeTimerEventNow() {
        printInfo("Inside executeTimerEventNow Event!");
        if(processing)
            return;
        try {
            executeTimerEvent();
            cleanUp();
        } catch (StopServiceException e) {
            try {
                printError("ERROR", e );
                sendMail("Stop Service Exception: " + this.getClass().getSimpleName(), e.getMessage() + ". Please examine log files for error messages.");
                stopServiceException = true;
                stopService();
            } catch (Exception e1) {
                Logger.getLogger( this.getClass()).error("ERROR", e);
            }
        }
    }

    public void incErrorCount(int value) throws StopServiceException {
        this.currentErrorCount = this.currentErrorCount + 1;
        if (this.currentErrorCount >= this.terminalErrorCount)
            throw new StopServiceException("Max. Error count Reached for: " + this.getClass().getSimpleName());
    }

    public void resetErrorCount() {
        this.currentErrorCount = 0;
    }

    public void poll() {
     //   super.handleNotification2(notification, object);
        if(! isProcessing()){
            try {
                executeTimerEvent();
                cleanUp();
            } catch (StopServiceException e) {
                try {
                    printError("ERROR", e);
                    sendMail("Stop Service Exception: " + this.getClass().getSimpleName(), e.getMessage() + ". Please examine log files for error messages.");
                    stopServiceException = true;
                    stopService();
                } catch (Exception e1) {
                    Logger.getLogger( this.getClass()).error("ERROR", e);
                }
            }
        } 
    }

    private void cleanUp() {
        processing = false;
    }

    protected void executeTimerEvent() throws StopServiceException {
        processing = true;
        loggingCounter++;

        if (loggingCounter == 60) {
            printInfo(this.getClass().getSimpleName() + " executed 60th timer event since last log message.");
            loggingCounter = 0;
        }
    }



    protected void initService() throws Exception{
        printInfo("Initializing Service: " + this.getClass().getSimpleName());
    }

    protected void sendMail(String subject,String content) {
        Session session = null;
        try {
            session = JndiUtil.lookup("java:/Mail");
            MimeMessage m = new MimeMessage(session);
            m.setFrom( new InternetAddress(emailFrom));
            m.setRecipients(Message.RecipientType.TO, emailTo);
            m.setSubject(this.getHostName() + ":" + subject);
            m.setSentDate(new Date());
            m.setContent(content,"text/plain");
            Transport.send(m);
        } catch (Exception e) {
            log.error(e);
        }
    }

    protected List getFileList(String dirLocation) throws StopServiceException{
        File scannedDir = new File(dirLocation);
        List fileList = new LinkedList();
        File[] scannedFile = scannedDir.listFiles();
        if (scannedDir.isDirectory())
        {
          if (scannedFile != null)
          {
              for (File scanFile : scannedFile) {
                  if (scanFile.isFile()) {
                    fileList.add(scanFile);
                  }
              }
          }
        }
        scannedDir = null;
        scannedFile = null;
        return fileList;
    }

    protected void moveFiles(List fileList, String sourceDir, String destDir, String discardDir, List<String>fileTypes) throws StopServiceException{
        boolean validFile = false;
        boolean renamedFile = false;
        System.gc();
        for (Object aFileList : fileList) {
            File sourceFile = (File) aFileList;
            if(sourceFile.exists()){
                validFile = false;
                for (String fileExtension : fileTypes) {
                    if (sourceFile.getName().indexOf(fileExtension) > 0){
                        validFile = true;
                        break;
                    }
                }
                if(validFile){
                    File processedFile = new File(destDir + "/" + sourceFile.getName());
                    if(processedFile.exists()){
                        renamedFile = processedFile.delete();
                        if(renamedFile == false){
                            printError("Unable to rename file: " + sourceFile.getName(), null );
                        }
                    }
                    renamedFile = sourceFile.renameTo(processedFile);
                    if(renamedFile == false){
                        printError("Unable to rename file: " + sourceFile.getName(), null);
                    }
                }else{
                    File discardFile = new File(discardDir + "/" + sourceFile.getName());
                    renamedFile = sourceFile.renameTo(discardFile);
                    if(renamedFile == false){
                        printError("Unable to rename file: " + sourceFile.getName(), null);
                    }
                }
            }
        }
    }

    protected void removeFiles(List fileList) throws StopServiceException{
        System.gc();
        for (Object aFileList : fileList) {
            File sourceFile = (File) aFileList;
            sourceFile.delete();
        }
    }

    protected void moveFile(File sourceFile, String sourceDir, String destDir) throws StopServiceException{
     //   String destFilePath = destDir + "/" + sourceFile.getName();
     //   File destFile = new File(destFilePath);

     //   if(destFile.exists()){
         //   destFile.delete();
       // }
       // sourceFile.renameTo(destFile);
    }

    protected byte[] getFileStream(String fileName) throws IOException {
       InputStream is = null;
       File file;
       try{
           file = new File(fileName);
           is = new FileInputStream(file);
           long size = file.length();
           byte[] fileData = new byte[(int)size];
           is.read(fileData);
           return fileData;
       }
       finally {
           if(is != null){
               is.close();
               is = null;
           }
       }
    }


    @Override
    protected void onServiceStarting() throws Exception {
        try {
            // super.subscribe(true);
            processing = false;
            currentErrorCount = 0;
            stopServiceException = false;
            printInfo("Start Service: " + this.getClass().getSimpleName() + "...");
            initService();
            sendMail("Service has started: " + this.getClass().getSimpleName(), this.getClass().getSimpleName() + " has been started.");
            printInfo("Service Started: " + this.getClass().getSimpleName() + ".");
        } catch (Exception e) {
            printError("Start Service Exception: " + this.getClass().getSimpleName(), e);
            sendMail("Service has Failed To Start: " + this.getClass().getSimpleName(), this.getClass().getSimpleName() + " has failed to start. " + e.getMessage());
            try {
                stopService();
            } catch (Exception e1) {
                Logger.getLogger( this.getClass()).error("ERROR", e1);
            }
        }
    }

    @Override
    protected void onServiceStoping() {
        mylogger.info("Service is stopping.");
        if(stopServiceException == false){
            sendMail(this.getClass().getSimpleName() + " Stopped.", this.getClass().getSimpleName() + " has been stopped");
        }
    }
}
