 package aff.confirm.jboss.utils;

 import javax.jms.JMSException;
 import javax.jms.Message;
 import javax.jms.TopicPublisher;
 import javax.jms.TopicSession;
 import java.awt.*;
 import org.jboss.logging.Logger;


 public class RTRefreshLabel {
     private static Logger log = Logger.getLogger(RTRefreshLabel.class);

     TopicPublisher tp;
   TopicSession ts;
   private String value = "";

   public RTRefreshLabel(TopicSession ts, TopicPublisher tp)
     throws HeadlessException
   {
     this.tp = tp;
     this.ts = ts;
   }

   public String getValue() {
     return this.value;
   }

   public void setValue(String value) {
     this.value = value;
     log.info(value);
     try {
       Message msg = this.ts.createMessage();
       msg.setStringProperty("ClassName", "RTRefreshLabel");
       msg.setStringProperty("value", value);
       this.tp.publish(msg, 1, 7, 5000L);
     } catch (JMSException e) {
         log.error( "ERROR", e);
     }
   }
 }
