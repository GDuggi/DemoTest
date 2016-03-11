package aff.confirm.webservices.sempradocws.lib.org.tempuri;

public class SempraDocWSSoapProxy implements aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap {
  private String _endpoint = null;
  private aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap sempraDocWSSoap = null;
  
  public SempraDocWSSoapProxy() {
    _initSempraDocWSSoapProxy();
  }
  
  public SempraDocWSSoapProxy(String endpoint) {
    _endpoint = endpoint;
    _initSempraDocWSSoapProxy();
  }
  
  private void _initSempraDocWSSoapProxy() {
    try {
      sempraDocWSSoap = (new aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSLocator()).getSempraDocWSSoap();
      if (sempraDocWSSoap != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)sempraDocWSSoap)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)sempraDocWSSoap)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (sempraDocWSSoap != null)
      ((javax.xml.rpc.Stub)sempraDocWSSoap)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap getSempraDocWSSoap() {
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap;
  }
  
  public java.lang.String importDocIntoVault(java.lang.String docRepository, java.lang.String docFileURL, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, boolean deleteFileAfterVault, java.lang.String dslName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.importDocIntoVault(docRepository, docFileURL, fieldNames, fieldValues, onBehalfUserName, deleteFileAfterVault, dslName);
  }
  
  public java.lang.String importDocIntoVaultByVaultType(java.lang.String vaultType, java.lang.String docRepository, java.lang.String docFileURL, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, boolean deleteFileAfterVault, java.lang.String dslName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.importDocIntoVaultByVaultType(vaultType, docRepository, docFileURL, fieldNames, fieldValues, onBehalfUserName, deleteFileAfterVault, dslName);
  }
  
  public java.lang.String importDocByStream(java.lang.String docRepository, byte[] data, java.lang.String fileType, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, java.lang.String dslName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.importDocByStream(docRepository, data, fileType, fieldNames, fieldValues, onBehalfUserName, dslName);
  }
  
  public java.lang.String importDocByStreamByVaultType(java.lang.String vaultType, java.lang.String docRepository, byte[] data, java.lang.String fileType, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, java.lang.String dslName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.importDocByStreamByVaultType(vaultType, docRepository, data, fileType, fieldNames, fieldValues, onBehalfUserName, dslName);
  }
  
  public void getDocFromVault(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String directory, java.lang.String fileName, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getDocFromVaultResult, javax.xml.rpc.holders.StringHolder fileLocation) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    sempraDocWSSoap.getDocFromVault(docRepository, docResourceID, directory, fileName, dslName, getDocFromVaultResult, fileLocation);
  }
  
  public void getDocFromVaultByVaultType(java.lang.String vaultType, java.lang.String docRepository, java.lang.String docResourceID, java.lang.String directory, java.lang.String fileName, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getDocFromVaultByVaultTypeResult, javax.xml.rpc.holders.StringHolder fileLocation) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    sempraDocWSSoap.getDocFromVaultByVaultType(vaultType, docRepository, docResourceID, directory, fileName, dslName, getDocFromVaultByVaultTypeResult, fileLocation);
  }
  
  public void getLatestDocStream(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getLatestDocStreamResult, javax.xml.rpc.holders.ByteArrayHolder docStream, javax.xml.rpc.holders.StringHolder fileType) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    sempraDocWSSoap.getLatestDocStream(docRepository, docResourceID, dslName, getLatestDocStreamResult, docStream, fileType);
  }
  
  public void getLatestDocString(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getLatestDocStringResult, javax.xml.rpc.holders.StringHolder docStream, javax.xml.rpc.holders.StringHolder fileType) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    sempraDocWSSoap.getLatestDocString(docRepository, docResourceID, dslName, getLatestDocStringResult, docStream, fileType);
  }
  
  public java.lang.String getDocumentList(java.lang.String docRepository, java.lang.String fieldNames, java.lang.String fieldValues, boolean latestVersionOnly, java.lang.String dslName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.getDocumentList(docRepository, fieldNames, fieldValues, latestVersionOnly, dslName);
  }
  
  public java.lang.String hello(java.lang.String callerName) throws java.rmi.RemoteException{
    if (sempraDocWSSoap == null)
      _initSempraDocWSSoapProxy();
    return sempraDocWSSoap.hello(callerName);
  }
  
  
}