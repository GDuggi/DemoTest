package cnf.docflow.util;

import java.io.*;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import javax.activation.DataHandler;
import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;
import javax.mail.util.ByteArrayDataSource;

/**
 * Created by jvega on 7/24/2015.
 */
public class EmailUtils implements Serializable {

    private static Session mailSession;
    private static String to;
    private static String[] toAddrs;
    private static String from;
    private static String subject;
    private static String body;
    private static String contentType;
    private static String[] mailAttachments;
    private static String mailAttachmentBody;

    private static void send() throws Exception {
        Message message = new MimeMessage(mailSession);

        message.setFrom(new InternetAddress(from));
        for (int j=0;j < toAddrs.length; j++){
            Address toAddress = new InternetAddress(toAddrs[j]);
            message.addRecipient(Message.RecipientType.TO,toAddress);
        }
        message.setSubject(subject);
        if (!mailAttachmentBody.isEmpty() || (mailAttachments != null && mailAttachments.length > 0)) {
            BodyPart messageBodyPart = new MimeBodyPart();
            messageBodyPart.setContent(body, contentType);

            Multipart multipart = new MimeMultipart();
            //add body of message
            multipart.addBodyPart(messageBodyPart);

            if (!mailAttachmentBody.isEmpty()) {
                ByteArrayDataSource dsAttachment = new ByteArrayDataSource(mailAttachmentBody.getBytes(), contentType);
                MimeBodyPart attachmentPart = new MimeBodyPart();
                attachmentPart.setDisposition(Part.ATTACHMENT);
                attachmentPart.setDataHandler(new DataHandler(dsAttachment));
                String fileName = "Mercuria_ConfirmMessage_" + new SimpleDateFormat("yyyyMMdd_HHmmss").format(Calendar.getInstance().getTime()) + ".xml";
                attachmentPart.setFileName(fileName);
                //add attachment from received message
                multipart.addBodyPart(attachmentPart);
            }

            if (mailAttachments != null && mailAttachments.length > 0) {
                for (String filePath : mailAttachments) {
                    MimeBodyPart attachmentPart = new MimeBodyPart();
                    attachmentPart.attachFile(filePath);
                    //add file attachment(s)
                    multipart.addBodyPart(attachmentPart);
                }
            }
            message.setContent(multipart);
        } else {
            message.setContent(body, contentType);
        }
        //System.out.println("Message Set as "+ contentType);
       // message.writeTo(System.out);
        Transport.send(message, message.getAllRecipients());
    }

    public static void sendMail(Session pSession, String pFrom, String pTo, String pSubject, String pBody) throws Exception {
        sendMail(pSession,pFrom,pTo,pSubject,pBody,null,null);
    }

    public static void sendMail(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String pAttachmentBody) throws Exception {
        sendMail(pSession,pFrom,pTo,pSubject,pBody,pAttachmentBody,null);
    }

    public static void sendMail(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String[] pAttachments) throws Exception {
        sendMail(pSession, pFrom, pTo, pSubject,pBody,null,pAttachments);
    }

    private static void sendMail(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String pAttachmentBody, String[] pAttachments ) throws Exception {
            mailSession = pSession;
            from = pFrom;
            toAddrs = pTo.split(",");
            subject = pSubject;
            body = pBody;
            mailAttachmentBody = pAttachmentBody;
            mailAttachments = pAttachments;
            if (mailAttachments == null & mailAttachmentBody == null) {
                contentType = "text/plain";
            } else {
                contentType = "text/html; charset=UTF-8";
            }
            send();
    }

    public static void sendMailHTML(Session pSession, String pFrom, String pTo, String pSubject, String pBody) throws Exception {
        sendMailHTML(pSession,pFrom,pTo,pSubject,pBody,null,null);
    }

    public static void sendMailHTML(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String pAttachmentBody) throws Exception {
        sendMailHTML(pSession,pFrom,pTo,pSubject,pBody,pAttachmentBody,null);
    }

    public static void sendMailHTML(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String[] pAttachments) throws Exception {
        sendMailHTML(pSession,pFrom,pTo,pSubject,pBody,null,pAttachments);
    }

    private static void sendMailHTML(Session pSession, String pFrom, String pTo, String pSubject, String pBody, String pAttachmentBody, String[] pAttachments ) throws Exception {
        mailSession = pSession;
        from = pFrom;
        toAddrs = pTo.split(",");
        subject = pSubject;
        body = pBody;
        mailAttachmentBody = pAttachmentBody;
        mailAttachments = pAttachments;
        contentType = "text/html";
        send();
    }

}


