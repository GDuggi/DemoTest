package cnf.docflow.mdbclient;

/**
 * Created by jvega on 7/2/2015.
 * Used for testing MDB only
 */
import org.w3c.dom.Document;

import java.io.*;
import java.util.Hashtable;
import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

public class TestMercuriaTradeAlertProcessorMDB
{
    public final static String JNDI_FACTORY="org.jboss.naming.remote.client.InitialContextFactory";
    //*************** Connection Factory JNDI name *************************
    public final static String JMS_FACTORY="jms/RemoteConnectionFactory";

    //*************** Overwritten by cmd line, values are just examples *************************
    /*
    private static String hostUrlProd = "remote://cnf01inf01:4447";
    private static String destinationNameProd = "mercuria.confirmsMgr.tradeNotification";
    private static String userProd = "cnf.docflow";
    private static String pwdProd ="Amphora-123";

    private static String hostUrlDev = "remote://aff01inf01:4447";
    private static String destinationNameDev = "jms/queue/sempra.sif.ping";//"jms/queue/TestQ";
    private static String userDev ="sempra.sif.jboss";//"aff.confirm.workflow";
    private static String pwdDev ="sempra";// "Amphora-123";
    */
    //example configuration program arguments:
    //        remote://aff01inf01:4447 jms/queue/sempra.sif.ping sempra.sif.jboss sempra
    //        remote://cnf01inf01:4447 jms/queue/mercuria.confirmsMgr.tradeNotification sempra.ops.jboss sempra C:\Projects\AppDev\JavaProjects\Docflow\TestMessages\TradeAlertMessage_NEW_test05.xml
    private static String hostUrl = " ";
    private static String destinationName = "";
    private static String user ="";
    private static String pwd ="";
    private static String msgFile="";

    private ConnectionFactory conFactory;
    private Connection con;
    private Session session;
    private MessageProducer sender;
    private Destination destination;
    private TextMessage msg;

    public void init(Context ctx, String destinationName)throws NamingException, JMSException
    {
        conFactory = (ConnectionFactory) ctx.lookup(JMS_FACTORY);
        // Change the UserName & Password
        con = conFactory.createConnection( user, pwd );
        session = con.createSession(false, Session.AUTO_ACKNOWLEDGE);
        System.out.println( "Looking up destination " + destinationName );
        destination = (Destination) ctx.lookup(destinationName);
        sender = session.createProducer(destination);
        msg = session.createTextMessage();
        con.start();
    }

    public void send(String message) throws JMSException {
        msg.setText(message);
        sender.send(msg);
    }

    public void close() throws JMSException {
        sender.close();
        session.close();
        con.close();
    }

    public static void main(String[] args) throws Exception {
        if (args.length < 4 || args.length > 5) {
            System.out.println("Usage: java QueueSend <hornetq-host:port> <queue> <user> <password> <file>\n\t .e.g java QueueSend remote://172.17.137.244:4447 Question admin admin11");
            return;
        }
        int i = 0;

        hostUrl = args[i++];
        destinationName = args[i++];
        user = args[i++];
        pwd=args[i++];
        if (args.length == 5) {
            msgFile = args[i++];
        }
        InitialContext ic = getInitialContext(hostUrl);

        TestMercuriaTradeAlertProcessorMDB qs = new TestMercuriaTradeAlertProcessorMDB();
        qs.init(ic, destinationName);
        //readAndSend(qs);
        if (args.length == 4) {
            readFileNameandSend(qs);
        } else{
            readFileandSend(qs,msgFile);
        }

        qs.close();
    }

    private static void readAndSend(TestMercuriaTradeAlertProcessorMDB qs) throws IOException, JMSException
    {
        String line="Test Message Body with counter = ";
        BufferedReader br=new BufferedReader(new InputStreamReader(System.in));
        boolean readFlag=true;
        System.out.println("Start Sending Messages (Enter QUIT to Stop):");
        while(readFlag)
        {
            System.out.print("<Msg_Sender> ");
            String msg=br.readLine();
            if(msg.equals("QUIT") || msg.equals("quit"))
            {
                qs.send(msg);
                System.exit(0);
            }
            qs.send(msg);
            System.out.println();
        }
        br.close();
    }

    private static void readFileNameandSend(TestMercuriaTradeAlertProcessorMDB qs) throws IOException, JMSException {
        String line = "file name and location = ";
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        boolean readFlag = true;
        System.out.println("Enter Path and File Name:");
        while (readFlag) {
            System.out.print("<Msg_Sender> ");
            String msg = br.readLine();
            if (msg.equals("QUIT") || msg.equals("quit")) {
                System.exit(0);
            } else {
                if (!msg.isEmpty()) {
                    String content = convertXMLFileToString(msg);
                    qs.send(content);
                }
                    System.out.println();
            }
        }
        br.close();
    }

    private static void readFileandSend(TestMercuriaTradeAlertProcessorMDB qs, String msgFile) throws IOException, JMSException {
        if (!msgFile.isEmpty()) {
            File file = new File(msgFile);
            if (file.exists()) {
                String content = convertXMLFileToString(msgFile);
                qs.send(content);
            }
        }
        System.out.println();
    }

    private static InitialContext getInitialContext(String url) throws NamingException
    {
        Hashtable env = new Hashtable();
        env.put(Context.INITIAL_CONTEXT_FACTORY, JNDI_FACTORY);
        env.put(Context.PROVIDER_URL, url);
        env.put(Context.SECURITY_PRINCIPAL, user);
        env.put(Context.SECURITY_CREDENTIALS, pwd);
        return new InitialContext(env);
    }

    private static String convertXMLFileToString(String fileName)
    {
        try{
            DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
            InputStream inputStream = new FileInputStream(new File(fileName));
            Document doc = documentBuilderFactory.newDocumentBuilder().parse(inputStream);
            StringWriter stw = new StringWriter();
            Transformer serializer = TransformerFactory.newInstance().newTransformer();
            serializer.transform(new DOMSource(doc), new StreamResult(stw));
            return stw.toString();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }
}
