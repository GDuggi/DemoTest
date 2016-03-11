 package aff.confirm.jboss.utils;

 import javax.jms.JMSException;
 import javax.jms.Message;
 import javax.jms.QueueSender;
 import java.sql.Connection;

 public class Sender
 {
   private QueueSender sender;
   private Connection connnection;

   public Sender(QueueSender sender, Connection connnection)
   {
     this.sender = sender;
     this.connnection = connnection;
   }

   public void send(Message newMessage, int nonPersistent, int i, int qExpirationTime)
     throws JMSException
   {
     this.sender.send(newMessage, nonPersistent, i, qExpirationTime);
   }
 }
