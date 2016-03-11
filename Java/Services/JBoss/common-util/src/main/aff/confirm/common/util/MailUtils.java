package aff.confirm.common.util;

import javax.activation.DataHandler;
import javax.activation.FileDataSource;
import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;
import java.io.UnsupportedEncodingException;
import java.util.Date;
import java.util.Properties;

/**
 * User: ifrankel
 * Date: Aug 6, 2003
 * Time: 2:36:20 PM
 * To change this template use Options | File Templates.
 */
public class MailUtils {
    private String host;
    private String port;
    private Properties props;
    Session session;

    public MailUtils(String pHost, String pPort) {
        this.host = pHost;
        this.port = pPort;
        props = System.getProperties();
        props.put("mail.smtp.host", host);
        // This isn't documented, but is  handy for other ports
        props.put("mail.smtp.port", port);
        session = Session.getDefaultInstance(props);
        /*session = Session.getDefaultInstance(props, new Authenticator() {
            protected PasswordAuthentication getPasswordAuthentication() {
                PasswordAuthentication passwordAuthentication = new PasswordAuthentication("ifrankel","xxPasswordxx");
                return passwordAuthentication;
            }
        });*/
        //session.setDebug(true);
    }

    public void sendMail(String pSendToAddress, String pSendToName, String pSentFromAddress, String pSentFromName,
                         String pSubject, String pBodyText, String pAttachmentFilename)
            throws MessagingException, UnsupportedEncodingException {
        MimeMessage msg = new MimeMessage(session);

        //Create the text part and set the text
        MimeBodyPart textPart = new MimeBodyPart();
        textPart.setText(pBodyText);

        // attach the file to the message
        MimeBodyPart attachPart = new MimeBodyPart();
        if (pAttachmentFilename.length() > 0) {
            FileDataSource fds = new FileDataSource(pAttachmentFilename);
            attachPart.setDataHandler(new DataHandler(fds));
            attachPart.setFileName(fds.getName());
        }

        // create the Multipart and its parts to it
        Multipart multipart = new MimeMultipart();
        multipart.addBodyPart(textPart);

        if (pAttachmentFilename.length() > 0) {
            multipart.addBodyPart(attachPart);
        }

        //InternetAddress[] sendToRecipient = {new InternetAddress(pSendToAddress, pSendToName)};
        msg.setContent(multipart);
        msg.setFrom(new InternetAddress(pSentFromAddress, pSentFromName));

        //Handles multiple to addresses
        String[] splitAddress;
        splitAddress = pSendToAddress.split(",");
        Address toAddrs[] = new InternetAddress[splitAddress.length];
        int count;
        for (count=0; count < splitAddress.length; count++){
            toAddrs[count] = new InternetAddress(splitAddress[count]);
            //These sleeps are necessary when doing multiple toAddresses
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) { }
        }

        msg.setRecipients(Message.RecipientType.TO, toAddrs);

        /*toAddrs[0] = new InternetAddress("ifrankel@sempratrading.com");
        toAddrs[1] = new InternetAddress("dfrankel@sempratrading.com");
        toAddrs[2] = new InternetAddress("lgordon@sempratrading.com");
        toAddrs[3] = new InternetAddress("skwok@sempratrading.com");*/

        //Handles single address
        //InternetAddress[] sendToRecipient = {new InternetAddress(splitAddress[0], pSendToName)};
        //msg.setRecipients(Message.RecipientType.TO, sendToRecipient);

        msg.setSentDate(new Date());
        msg.setSubject(pSubject);
        //These sleeps are necessary when doing multiple toAddresses
        try {
                Thread.sleep(1000);
            } catch (InterruptedException e) { }

        Transport.send(msg);
    }
    
    public void sendHTMLMail(String pSendToAddress, String pSendToName, String pSentFromAddress, String pSentFromName,
                         String pSubject, String pHTMLText)
            throws MessagingException, UnsupportedEncodingException {
        MimeMessage msg = new MimeMessage(session);
        msg.setContent(pHTMLText, "text/html");
        
        msg.setFrom(new InternetAddress(pSentFromAddress, pSentFromName));

        //Handles multiple to addresses
        String[] splitAddress;
        splitAddress = pSendToAddress.split(",");
        Address toAddrs[] = new InternetAddress[splitAddress.length];
        int count;
        for (count=0; count < splitAddress.length; count++){
            toAddrs[count] = new InternetAddress(splitAddress[count]);
            //These sleeps are necessary when doing multiple toAddresses
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) { }
        }

        msg.setRecipients(Message.RecipientType.TO, toAddrs);

        msg.setSentDate(new Date());
        msg.setSubject(pSubject);

        //These sleeps are necessary when doing multiple toAddresses
        try {
                Thread.sleep(1000);
            } catch (InterruptedException e) { }

        Transport.send(msg);
    }

}
