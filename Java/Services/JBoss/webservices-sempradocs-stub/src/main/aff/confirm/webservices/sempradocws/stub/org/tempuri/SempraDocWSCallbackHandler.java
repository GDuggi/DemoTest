
/**
 * SempraDocWSCallbackHandler.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis2 version: 1.4.1  Built on : Aug 19, 2008 (10:13:39 LKT)
 */

    package aff.confirm.webservices.sempradocws.stub.org.tempuri;

/**
     *  SempraDocWSCallbackHandler Callback class, Users can extend this class and implement
     *  their own receiveResult and receiveError methods.
     */
    public abstract class SempraDocWSCallbackHandler{



    protected Object clientData;

    /**
    * User can pass in any object that needs to be accessed once the NonBlocking
    * Web service call is finished and appropriate method of this CallBack is called.
    * @param clientData Object mechanism by which the user can pass in user data
    * that will be avilable at the time this callback is called.
    */
    public SempraDocWSCallbackHandler(Object clientData){
        this.clientData = clientData;
    }

    /**
    * Please use this constructor if you don't want to set any clientData
    */
    public SempraDocWSCallbackHandler(){
        this.clientData = null;
    }

    /**
     * Get the client data
     */

     public Object getClientData() {
        return clientData;
     }

        
           /**
            * auto generated Axis2 call back method for ImportDocByStream method
            * override this method for handling normal response from ImportDocByStream operation
            */
           public void receiveResultImportDocByStream(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.ImportDocByStreamResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from ImportDocByStream operation
           */
            public void receiveErrorImportDocByStream(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for GetDocumentList method
            * override this method for handling normal response from GetDocumentList operation
            */
           public void receiveResultGetDocumentList(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.GetDocumentListResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from GetDocumentList operation
           */
            public void receiveErrorGetDocumentList(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for ImportDocByStreamByVaultType method
            * override this method for handling normal response from ImportDocByStreamByVaultType operation
            */
           public void receiveResultImportDocByStreamByVaultType(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.ImportDocByStreamByVaultTypeResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from ImportDocByStreamByVaultType operation
           */
            public void receiveErrorImportDocByStreamByVaultType(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for GetDocFromVaultByVaultType method
            * override this method for handling normal response from GetDocFromVaultByVaultType operation
            */
           public void receiveResultGetDocFromVaultByVaultType(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.GetDocFromVaultByVaultTypeResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from GetDocFromVaultByVaultType operation
           */
            public void receiveErrorGetDocFromVaultByVaultType(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for Hello method
            * override this method for handling normal response from Hello operation
            */
           public void receiveResultHello(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.HelloResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from Hello operation
           */
            public void receiveErrorHello(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for GetDocFromVault method
            * override this method for handling normal response from GetDocFromVault operation
            */
           public void receiveResultGetDocFromVault(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.GetDocFromVaultResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from GetDocFromVault operation
           */
            public void receiveErrorGetDocFromVault(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for ImportDocIntoVaultByVaultType method
            * override this method for handling normal response from ImportDocIntoVaultByVaultType operation
            */
           public void receiveResultImportDocIntoVaultByVaultType(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.ImportDocIntoVaultByVaultTypeResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from ImportDocIntoVaultByVaultType operation
           */
            public void receiveErrorImportDocIntoVaultByVaultType(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for GetLatestDocString method
            * override this method for handling normal response from GetLatestDocString operation
            */
           public void receiveResultGetLatestDocString(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.GetLatestDocStringResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from GetLatestDocString operation
           */
            public void receiveErrorGetLatestDocString(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for GetLatestDocStream method
            * override this method for handling normal response from GetLatestDocStream operation
            */
           public void receiveResultGetLatestDocStream(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.GetLatestDocStreamResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from GetLatestDocStream operation
           */
            public void receiveErrorGetLatestDocStream(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for ImportDocIntoVault method
            * override this method for handling normal response from ImportDocIntoVault operation
            */
           public void receiveResultImportDocIntoVault(
                    aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub.ImportDocIntoVaultResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from ImportDocIntoVault operation
           */
            public void receiveErrorImportDocIntoVault(java.lang.Exception e) {
            }
                


    }
    