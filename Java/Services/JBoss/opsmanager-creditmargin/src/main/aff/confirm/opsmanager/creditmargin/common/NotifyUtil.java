package aff.confirm.opsmanager.creditmargin.common;

import org.jboss.logging.Logger;

import javax.mail.MessagingException;
import java.io.UnsupportedEncodingException;
import java.net.InetAddress;
import java.net.UnknownHostException;

/**
 * User: srajaman
 * Date: Dec 11, 2008
 * Time: 2:05:03 PM
 */
public class NotifyUtil {

    public static String smtpHost;
    public static String smtpPort;
    public static String notifyAddr;

    private static String fromAddress;



    public static void sendMail(String toAddress,String subject, String body){



        try {
            MailUtils mail = new MailUtils(smtpHost,smtpPort);
            fromAddress = "JBossOn" + InetAddress.getLocalHost().getHostName().toUpperCase() + "@sempratrading.com";
            mail.sendMail(toAddress,toAddress,fromAddress,fromAddress,subject,body,"");
        } catch (UnknownHostException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        } catch (MessagingException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        } catch (UnsupportedEncodingException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        }
    }



}
