/*
 * User: islepini
 * Date: Sep 26, 2002
 * Time: 5:07:16 PM
 * To change template for new class use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.mail;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.service.BasicMBeanSupport;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.mail.Message;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.naming.Context;
import javax.naming.Name;
import javax.naming.NamingException;
import java.io.File;
import java.io.Serializable;
import java.net.InetAddress;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.StringTokenizer;

@Startup
@Singleton
public class MailNotifier extends BasicMBeanSupport implements MailNotifierMBean, Serializable {
    private String jndiName;
    private String hostName;
    private String fromName;
    private String startUpShutDownEventEmailList = "";
    transient private MailNotifierShutdownThread shutDownThread ;
    private String winNTserviceName = "";
    private String messageOnStartUp = "";
    private String messageOnShutDown = "";
    private String subjectOnStartUp = "";
    private String subjectOnShutDown = "";

    public MailNotifier() {
        super("affinity.utils:service=MailNotifier");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
       super.preDestroy();
    }

    public String getFromName() {
        return fromName;
    }

    public void setFromName(String fromName) {
        this.fromName = fromName;
    }

    public String testDir() {
      String result = null;
        //TODO:RN What the?
      File scannedDir = new File("\\\\prod15vs\\DealSheetExportFiles");
      if(scannedDir.isDirectory()){
         result = "dir";
      }else
         result ="not dir";
      return result;
    }

    public String getWinNTserviceName() {
        return winNTserviceName;
    }

    public void setWinNTserviceName(String winNTserviceName) {
        this.winNTserviceName = winNTserviceName;
    }

    public String getMessageOnStartUp() {
        return messageOnStartUp;
    }

    public void setMessageOnStartUp(String messageOnStartUp) {
        this.messageOnStartUp = messageOnStartUp;
    }

    public String getMessageOnShutDown() {
        return messageOnShutDown;
    }

    public void setMessageOnShutDown(String messageOnShutDown) {
        this.messageOnShutDown = messageOnShutDown;
    }

    public String getSubjectOnStartUp() {
        return subjectOnStartUp;
    }

    public void setSubjectOnStartUp(String subjectOnStartUp) {
        this.subjectOnStartUp = subjectOnStartUp;
    }

    public String getSubjectOnShutDown() {
        return subjectOnShutDown;
    }

    public void setSubjectOnShutDown(String subjectOnShutDown) {
        this.subjectOnShutDown = subjectOnShutDown;
    }

/*
    public Element getNotifyGroups() {
        return notifyGroups;
    }
*/

/*
    public void setNotifyGroups(Element value) throws Exception {
        parseNotifyGroups(value);
        notifyGroups = value;
    }
*/

/*    private void parseNotifyGroups(Element element) throws Exception {
        notifyList.clear();
        String rootTag = element.getTagName();
        if (!rootTag.equals("groups"))
        {
            throw new DeploymentException("Not a resource adapter deployment " +
                "descriptor because its root tag, '" +
                rootTag + "', is not 'groupes'");
        }
        invokeChildren(element);
 ///       Runtime.getRuntime().exec()
    }*/

/*
    private void invokeChildren(Element element) throws Exception
    {
        NodeList children = element.getChildNodes();
        for (int i = 0; i < children.getLength(); ++i){
            Node node = children.item(i);
            if (node.getNodeType() == Node.ELEMENT_NODE)
            {
                Element child = (Element)node;
                String tag = child.getTagName();
                if (!tag.equals("group"))
                {
                    throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its tag, '" +
                    tag + "', is not 'group'");
                }
                if (!child.hasAttribute("name"))
                {
                    throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its tag, '" +
                    tag + "', has not attribute 'name'");
                }
                String groupName = child.getAttribute("name");
                if (!child.hasAttribute("members"))
                {
                    throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its tag, '" +
                    tag + "', has not attribute 'members'");
                }
                String members = child.getAttribute("members");
                notifyList.add(new EmailGroup(groupName,members));
            }
        }
    }
*/

    public void create() throws Exception {
        shutDownThread = new MailNotifier.MailNotifierShutdownThread();
        Runtime.getRuntime().addShutdownHook(shutDownThread);
    }

    public void destroy(){
        Runtime.getRuntime().removeShutdownHook(shutDownThread);
    }

    public String getJndiName()
    {
       return jndiName;
    }

    public void setJndiName(String jndiName) throws NamingException
    {
       String oldName = this.jndiName;
       this.jndiName = jndiName;
    }

    private void init() {
        try {
            hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
            StringTokenizer st = new StringTokenizer(startUpShutDownEventEmailList, ",");
            List addresses = new LinkedList();
            while (st.hasMoreTokens())
                addresses.add(st.nextToken());

            // 3/13/2015 Israel -- Removed because flakey.
            //sendMail(subjectOnStartUp, messageOnStartUp, addresses);
        } catch (Exception e) {
            log.error( "ERROR", e );
        }
    }

    public String getStartUpShutDownEventEmailList() {
        return startUpShutDownEventEmailList;
    }

    public void setStartUpShutDownEventEmailList(String value) {
        startUpShutDownEventEmailList = value;
    }

    public void startService()  {
         init();
    }

   private static Context createContext(Context rootContext, Name name) throws NamingException
   {
       Context subctx = rootContext;
       for(int n = 0; n < name.size(); n ++)
       {
           String atom = name.get(n);
           try
           {
               Object obj = subctx.lookup(atom);
               subctx = (Context) obj;
           }
           catch(NamingException e)
           {	// No binding exists, create a subcontext
               subctx = subctx.createSubcontext(atom);
           }
       }

       return subctx;
   }

   public void sendMail(String subject,String content, String addresses) {
         StringTokenizer st = new StringTokenizer(addresses, ",");
         while(st.hasMoreTokens())
         {
                String address = st.nextToken();
                InternetAddress[] to = new InternetAddress[1];
                try{
                    to[0] = new InternetAddress(address);
                    sendMail(subject,content, to);
                } catch (AddressException e) {
                    Logger.getLogger(this.getClass().getName()).error(e);
                }
         }

   }

   public void sendMailToGroup(String subject,String content, String groupName) throws NamingException {
/*
       for (int i = 0; i < notifyList.size() ; i++){
            EmailGroup tempGroup = (EmailGroup)notifyList.get(i);
            if (tempGroup.groupName.equals(groupName)){
                sendMail(subject,content,tempGroup.members);
                return;
            }
        }
        throw new NamingException("Group with name: "+groupName+" not found");
*/
   }

   public void sendMail(String subject,String content, String[] addresses) {
        InternetAddress[] to = new InternetAddress[addresses.length];
        for (int i=0;i<addresses.length;i++){
            try{
                to[i] = new InternetAddress(addresses[i]);
            } catch (AddressException e) {
                Logger.getLogger(this.getClass().getName()).error(e);
            }
        }
        sendMail(subject,content, to);
   }

   public void sendMail(String subject,String content, InternetAddress[] intAddresses) {
       Session session = null;
       try {
           // 3/13/2015 Israel -- Craps out here on startup.
           session = JndiUtil.lookup("java:/Mail");
           MimeMessage m = new MimeMessage(session);
           m.setFrom( new InternetAddress(fromName+hostName));
           m.setRecipients(Message.RecipientType.TO, intAddresses);
           m.setSubject(subject);
           m.setSentDate(new Date());
           m.setContent(content,"text/plain");
           Transport.send(m);
       } catch (Exception e) {
           Logger.getLogger(this.getClass().getName()).error(e);
       }

   }

    public void sendMail(String subject, String content, List addresses) {
        if ( getState() == STARTED ) {
            InternetAddress[] to = new InternetAddress[addresses.size()];
            for (int i=0;i<addresses.size();i++){
                try{
                    to[i] = new InternetAddress(addresses.get(i).toString());
                } catch (AddressException e) {
                    Logger.getLogger(this.getClass().getName()).error(e);
                }
            }
            sendMail(subject,content, to);
        }
    }

    class MailNotifierShutdownThread extends Thread {
       public void run() {
/*
          try{
            int windowhandle =  MessageSupport.findWindow(null,winNTserviceName);
            if (windowhandle <= 0)
               log.error("Failed to notify WinNt service. WinNT Service with name: "+winNTserviceName+ " not found");
            else{
               int messageid = MessageSupport.registerWindowMessage("UM_JVM_CLOSING");
               if (windowhandle <= 0)
                log.error("Failed to notify WinNt service. Couldn't register window message UM_JVM_CLOSING");
               else
                MessageSupport.sendMessage(windowhandle,messageid,0,0);
            }
          } catch (Exception e) {
            log.error(e);
          }
*/
          try{
              StringTokenizer st = new StringTokenizer(startUpShutDownEventEmailList, ",");
              List addresses = new LinkedList();
              while (st.hasMoreTokens())
                 addresses.add(st.nextToken());
              sendMail(subjectOnShutDown, messageOnShutDown,addresses);
          } catch (Exception e) {
              Logger.getLogger(this.getClass().getName()).error(e);
          }
       }
    }

}
