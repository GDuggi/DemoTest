package aff.confirm.webservices.tradegateway.oil;

public class OpsMgrWSProxy implements aff.confirm.webservices.tradegateway.oil.OpsMgrWS {
  private String _endpoint = null;
  private aff.confirm.webservices.tradegateway.oil.OpsMgrWS opsMgrWS = null;
  
  public OpsMgrWSProxy() {
    _initOpsMgrWSProxy();
  }
  
  public OpsMgrWSProxy(String endpoint) {
    _endpoint = endpoint;
    _initOpsMgrWSProxy();
  }
  
  private void _initOpsMgrWSProxy() {
    try {
      opsMgrWS = (new aff.confirm.webservices.tradegateway.oil.OpsMgrWSServiceLocator()).getOpsMgrWSPort();
      if (opsMgrWS != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)opsMgrWS)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)opsMgrWS)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (opsMgrWS != null)
      ((javax.xml.rpc.Stub)opsMgrWS)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public aff.confirm.webservices.tradegateway.oil.OpsMgrWS getOpsMgrWS() {
    if (opsMgrWS == null)
      _initOpsMgrWSProxy();
    return opsMgrWS;
  }
  
  public java.lang.String getTradeAlertOpsTrackXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception{
    if (opsMgrWS == null)
      _initOpsMgrWSProxy();
    return opsMgrWS.getTradeAlertOpsTrackXML(tradeId);
  }
  
  public java.lang.String getContractFeedXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception{
    if (opsMgrWS == null)
      _initOpsMgrWSProxy();
    return opsMgrWS.getContractFeedXML(tradeId);
  }
  
  public java.lang.String getTradeAlertMessageXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception{
    if (opsMgrWS == null)
      _initOpsMgrWSProxy();
    return opsMgrWS.getTradeAlertMessageXML(tradeId);
  }
  
  
}