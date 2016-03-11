/**
 * SempraDocWSSoap.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package aff.confirm.webservices.sempradocws.lib.org.tempuri;

public interface SempraDocWSSoap extends java.rmi.Remote {

    /**
     * Imports a document to a vault defined by Web.Config file.
     */
    public java.lang.String importDocIntoVault(java.lang.String docRepository, java.lang.String docFileURL, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, boolean deleteFileAfterVault, java.lang.String dslName) throws java.rmi.RemoteException;

    /**
     * Imports a document to a vault defined by Web.Config file.
     */
    public java.lang.String importDocIntoVaultByVaultType(java.lang.String vaultType, java.lang.String docRepository, java.lang.String docFileURL, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, boolean deleteFileAfterVault, java.lang.String dslName) throws java.rmi.RemoteException;

    /**
     * Import document by stream
     */
    public java.lang.String importDocByStream(java.lang.String docRepository, byte[] data, java.lang.String fileType, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, java.lang.String dslName) throws java.rmi.RemoteException;

    /**
     * Import document by stream using specific vault
     */
    public java.lang.String importDocByStreamByVaultType(java.lang.String vaultType, java.lang.String docRepository, byte[] data, java.lang.String fileType, java.lang.String fieldNames, java.lang.String fieldValues, java.lang.String onBehalfUserName, java.lang.String dslName) throws java.rmi.RemoteException;

    /**
     * Retrieves document from vault defined by Web.Config file. Writes
     * this document to specific directory.
     */
    public void getDocFromVault(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String directory, java.lang.String fileName, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getDocFromVaultResult, javax.xml.rpc.holders.StringHolder fileLocation) throws java.rmi.RemoteException;

    /**
     * Retrieves document from vault by vault type (AX or SP). Writes
     * this document to specific directory.
     */
    public void getDocFromVaultByVaultType(java.lang.String vaultType, java.lang.String docRepository, java.lang.String docResourceID, java.lang.String directory, java.lang.String fileName, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getDocFromVaultByVaultTypeResult, javax.xml.rpc.holders.StringHolder fileLocation) throws java.rmi.RemoteException;

    /**
     * Gets the latest version document.  Returns document stream
     * back to caller.
     */
    public void getLatestDocStream(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getLatestDocStreamResult, javax.xml.rpc.holders.ByteArrayHolder docStream, javax.xml.rpc.holders.StringHolder fileType) throws java.rmi.RemoteException;

    /**
     * Gets the latest version of a document.  Returns document string
     * back to caller.
     */
    public void getLatestDocString(java.lang.String docRepository, java.lang.String docResourceID, java.lang.String dslName, javax.xml.rpc.holders.StringHolder getLatestDocStringResult, javax.xml.rpc.holders.StringHolder docStream, javax.xml.rpc.holders.StringHolder fileType) throws java.rmi.RemoteException;

    /**
     * Get list of ducument IDs based on indexes and value criteria.
     * Returns latest version only if latestVersionOnly flag is true
     */
    public java.lang.String getDocumentList(java.lang.String docRepository, java.lang.String fieldNames, java.lang.String fieldValues, boolean latestVersionOnly, java.lang.String dslName) throws java.rmi.RemoteException;

    /**
     * Test WS:  Returns Hello to client
     */
    public java.lang.String hello(java.lang.String callerName) throws java.rmi.RemoteException;
}
