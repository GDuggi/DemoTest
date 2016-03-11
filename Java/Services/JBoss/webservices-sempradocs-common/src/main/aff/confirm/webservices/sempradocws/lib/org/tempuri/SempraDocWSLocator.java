/**
 * SempraDocWSLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package aff.confirm.webservices.sempradocws.lib.org.tempuri;

public class SempraDocWSLocator extends org.apache.axis.client.Service implements aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWS {

    public SempraDocWSLocator() {
    }


    public SempraDocWSLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public SempraDocWSLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for SempraDocWSSoap
    private java.lang.String SempraDocWSSoap_address = "http://yon00150/SempraDocumentWebService/SempraDocWS.asmx";

    public java.lang.String getSempraDocWSSoapAddress() {
        return SempraDocWSSoap_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String SempraDocWSSoapWSDDServiceName = "SempraDocWSSoap";

    public java.lang.String getSempraDocWSSoapWSDDServiceName() {
        return SempraDocWSSoapWSDDServiceName;
    }

    public void setSempraDocWSSoapWSDDServiceName(java.lang.String name) {
        SempraDocWSSoapWSDDServiceName = name;
    }

    public aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap getSempraDocWSSoap() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(SempraDocWSSoap_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getSempraDocWSSoap(endpoint);
    }

    public aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap getSempraDocWSSoap(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoapStub _stub = new aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoapStub(portAddress, this);
            _stub.setPortName(getSempraDocWSSoapWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setSempraDocWSSoapEndpointAddress(java.lang.String address) {
        SempraDocWSSoap_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoap.class.isAssignableFrom(serviceEndpointInterface)) {
                aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoapStub _stub = new aff.confirm.webservices.sempradocws.lib.org.tempuri.SempraDocWSSoapStub(new java.net.URL(SempraDocWSSoap_address), this);
                _stub.setPortName(getSempraDocWSSoapWSDDServiceName());
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
        if ("SempraDocWSSoap".equals(inputPortName)) {
            return getSempraDocWSSoap();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tempuri.org/", "SempraDocWS");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tempuri.org/", "SempraDocWSSoap"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("SempraDocWSSoap".equals(portName)) {
            setSempraDocWSSoapEndpointAddress(address);
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
