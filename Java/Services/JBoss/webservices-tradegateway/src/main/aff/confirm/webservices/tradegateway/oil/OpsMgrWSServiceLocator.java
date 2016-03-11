/**
 * OpsMgrWSServiceLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package aff.confirm.webservices.tradegateway.oil;

public class OpsMgrWSServiceLocator extends org.apache.axis.client.Service implements aff.confirm.webservices.tradegateway.oil.OpsMgrWSService {

    public OpsMgrWSServiceLocator() {
    }


    public OpsMgrWSServiceLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public OpsMgrWSServiceLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for OpsMgrWSPort
    private java.lang.String OpsMgrWSPort_address = "http://localhost:8080/AmphoraWebServices/AmphoraOpsMgrWS";

    public java.lang.String getOpsMgrWSPortAddress() {
        return OpsMgrWSPort_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String OpsMgrWSPortWSDDServiceName = "OpsMgrWSPort";

    public java.lang.String getOpsMgrWSPortWSDDServiceName() {
        return OpsMgrWSPortWSDDServiceName;
    }

    public void setOpsMgrWSPortWSDDServiceName(java.lang.String name) {
        OpsMgrWSPortWSDDServiceName = name;
    }

    public aff.confirm.webservices.tradegateway.oil.OpsMgrWS getOpsMgrWSPort() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(OpsMgrWSPort_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getOpsMgrWSPort(endpoint);
    }

    public aff.confirm.webservices.tradegateway.oil.OpsMgrWS getOpsMgrWSPort(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            aff.confirm.webservices.tradegateway.oil.OpsMgrWSServiceSoapBindingStub _stub = new aff.confirm.webservices.tradegateway.oil.OpsMgrWSServiceSoapBindingStub(portAddress, this);
            _stub.setPortName(getOpsMgrWSPortWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setOpsMgrWSPortEndpointAddress(java.lang.String address) {
        OpsMgrWSPort_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (aff.confirm.webservices.tradegateway.oil.OpsMgrWS.class.isAssignableFrom(serviceEndpointInterface)) {
                aff.confirm.webservices.tradegateway.oil.OpsMgrWSServiceSoapBindingStub _stub = new aff.confirm.webservices.tradegateway.oil.OpsMgrWSServiceSoapBindingStub(new java.net.URL(OpsMgrWSPort_address), this);
                _stub.setPortName(getOpsMgrWSPortWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("There is no stub implementation for the interface:  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("OpsMgrWSPort".equals(inputPortName)) {
            return getOpsMgrWSPort();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tc/OpsMgrWS", "OpsMgrWSService");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tc/OpsMgrWS", "OpsMgrWSPort"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("OpsMgrWSPort".equals(portName)) {
            setOpsMgrWSPortEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" Cannot set Endpoint Address for Unknown Port" + portName);
        }
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
