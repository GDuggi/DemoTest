package aff.confirm.jboss.jms;

import org.jboss.logging.Logger;

import javax.mail.MessagingException;
import java.io.UnsupportedEncodingException;
import java.net.InetAddress;
import java.net.UnknownHostException;

/**
 * User: srajaman
 * Date: Jul 16, 2008
 * Time: 8:30:10 AM
 */
public class NotifyUtil {

    public static String smtpHost;
    public static String smtpPort;
    public static String notifyAddr;

    private static String fromAddress;



    public static void sendMail(String subject, String body){



        try {
            MailUtils mail = new MailUtils(smtpHost,smtpPort);
            fromAddress = "JBossOn" + InetAddress.getLocalHost().getHostName().toUpperCase() + "@sempratrading.com";
            mail.sendMail(notifyAddr,notifyAddr,fromAddress,fromAddress,subject,body,"");
        } catch (UnknownHostException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        } catch (MessagingException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        } catch (UnsupportedEncodingException e) {
            Logger.getLogger(NotifyUtil.class).error(e.getMessage());
        }
    }


    
}
