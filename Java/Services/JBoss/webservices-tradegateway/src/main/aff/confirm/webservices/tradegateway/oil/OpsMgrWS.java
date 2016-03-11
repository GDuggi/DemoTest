/**
 * OpsMgrWS.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package aff.confirm.webservices.tradegateway.oil;

public interface OpsMgrWS extends java.rmi.Remote {
    public java.lang.String getTradeAlertOpsTrackXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception;
    public java.lang.String getContractFeedXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception;
    public java.lang.String getTradeAlertMessageXML(int tradeId) throws java.rmi.RemoteException, aff.confirm.webservices.tradegateway.oil.Exception;
}
