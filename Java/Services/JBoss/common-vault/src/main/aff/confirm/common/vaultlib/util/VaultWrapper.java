package aff.confirm.common.vaultlib.util;

import aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSLocator;
import aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoapStub;
import org.jboss.logging.Logger;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.rpc.ServiceException;
import javax.xml.rpc.holders.StringHolder;
import java.io.*;



public class VaultWrapper {
	private static Logger log = Logger.getLogger(VaultWrapper.class);

	private String endpointUrl;
	private SempraDocWSSoapStub stub; 
	
	public VaultWrapper(String url,String userName,String password) throws ServiceException{
		this.endpointUrl = url;
		SempraDocWSLocator wsLoc = new SempraDocWSLocator();
		wsLoc.setSempraDocWSSoapEndpointAddress(url);
		stub = (SempraDocWSSoapStub) wsLoc.getSempraDocWSSoap();
		stub.setUsername(userName);
		stub.setPassword(password);
		
		
	}
	public VaultStatusInfo retrieveDocument(VaultInfo vi) throws ParserConfigurationException, SAXException, IOException{
	
		VaultStatusInfo statusInfo = null;
		String documentList = null;
		documentList = stub.getDocumentList(vi.getDocRepository(), vi.getFieldNames(), vi.getFieldValues(), true, vi.getDslName());
		Logger.getLogger(this.getClass()).info("Vault Document List Return XML = " +documentList );
		statusInfo = getResource(documentList);
		if (!"-1".equals(statusInfo.getResourceId())) {
			StringHolder holder = new StringHolder();
			StringHolder result = new StringHolder();
			StringHolder type = new StringHolder();
			stub.getLatestDocString(vi.getDocRepository(), statusInfo.getResourceId(), vi.getDslName(),result,holder,type);
			statusInfo.setData(holder.value.getBytes());
			statusInfo.setResultCode(1); // success
		}
		else {
			statusInfo.setResultCode(-1);
		}
		return statusInfo;
			
	}
	private VaultStatusInfo getResource(String documentList) throws ParserConfigurationException, SAXException, IOException {
		String resource = "-1";
		
		VaultStatusInfo vsi = new VaultStatusInfo();
		vsi.setResourceId("-1"); // default id
		DocumentBuilderFactory builderFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder parser = builderFactory.newDocumentBuilder();
	    InputStream in = new ByteArrayInputStream(documentList.getBytes("UTF-8")); 
	    Document doc =parser.parse(in);
	    in.close();
		NodeList nodeList = doc.getElementsByTagName("ResourceString");
		if (nodeList != null && nodeList.getLength() > 0) {
			 resource = nodeList.item(0).getTextContent();
			 vsi.setResourceId(resource);
		}
		return vsi;
	}
	public VaultStatusInfo storeDocument(VaultInfo vi) throws SAXException, IOException, ParserConfigurationException{

		String s = stub.hello("test");
		log.info(s);
		VaultStatusInfo vsi = null;
		String returnXml = null;
		returnXml =stub.importDocByStream(vi.getDocRepository(), vi.getData(), "", vi.getFieldNames(), vi.getFieldValues(), vi.getUserName(), vi.getDslName());
		vsi = parseStatus(returnXml);
		return vsi;
		
	}
	private VaultStatusInfo parseStatus(String returnXml) throws SAXException, IOException, ParserConfigurationException {


		VaultStatusInfo vsi = new VaultStatusInfo();
		 DocumentBuilderFactory builderFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder parser = builderFactory.newDocumentBuilder();
        InputStream in = new ByteArrayInputStream(returnXml.getBytes("UTF-8")); 
        Document doc =parser.parse(in);
        in.close();
		NodeList nodeList = doc.getElementsByTagName("ResultCode");
		String resultCode = nodeList.item(0).getTextContent();
		nodeList = doc.getElementsByTagName("ResultValue");
		String resultValue = nodeList.item(0).getTextContent();
		vsi.setResultCode(Integer.parseInt(resultCode));
		vsi.setResultValue(resultValue);
		return vsi;
	}

	public void getDocument() {
		
	}
	public static void main(String[] arg) throws IOException, SAXException, ParserConfigurationException {
		
		try {
			VaultWrapper vw = new VaultWrapper("http://stdoc1/SempraDocumentWebService/SempraDocWS.asmx","SEMPRATRADING\\AXRobot","!@Dealsheets");
			VaultInfo vi = new VaultInfo();
			vi.setDocRepository("OPSMANAGER_CONTRACTS");
			vi.setDslName("APPXTENDER");
			vi.setUserName("srajaman");
			vi.setFieldNames("TRD_SYS_ID|CONTRACT_ID");
			vi.setFieldValues("572929|7190");
			VaultStatusInfo vsi = vw.retrieveDocument(vi);
			File f = new File("C:\\temp\\temp.rtf");
			FileOutputStream fi = new FileOutputStream(f);
			fi.write(vsi.getData());
			fi.close();
			
		//	vw.retrieveDocument(vi);
			System.out.println("Code = " + vsi.getResultCode() + ",Text = " + vsi.getResultValue());
			
		} catch (ServiceException e) {
			e.printStackTrace();
		} 
		
	}
}
