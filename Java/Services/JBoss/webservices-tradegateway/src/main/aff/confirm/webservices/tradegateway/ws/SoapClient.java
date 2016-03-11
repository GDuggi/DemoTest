package aff.confirm.webservices.tradegateway.ws;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/25/13
 * Time: 1:57 PM
 */
public class SoapClient {


    public String postSoapRequest(String serverUrl, String soapXML, String soapAction) throws IOException {

        URL  url = new URL(serverUrl);
        URLConnection connection = url.openConnection();
        HttpURLConnection httpConnection= (HttpURLConnection) connection;

        byte[] bytes = soapXML.getBytes();

        httpConnection.setRequestProperty("Content-Length",String.valueOf(bytes.length));
        httpConnection.setRequestProperty("Content-Type","text/xml");
        httpConnection.setRequestProperty("SOAPAction",soapAction);
        httpConnection.setRequestMethod("POST");
        httpConnection.setDoInput(true);
        httpConnection.setDoOutput(true);


        OutputStream out  = httpConnection.getOutputStream();
        out.write(soapXML.getBytes());
        out.close();

        InputStreamReader reader = new InputStreamReader(httpConnection.getInputStream());
        BufferedReader bufReader = new BufferedReader(reader);

        StringBuilder sb  = new StringBuilder();
        String inputLine;
        while ( ( inputLine= bufReader.readLine()) != null ) {
            sb.append(inputLine);
        }
         bufReader.close();

        return sb.toString();
    }

}
