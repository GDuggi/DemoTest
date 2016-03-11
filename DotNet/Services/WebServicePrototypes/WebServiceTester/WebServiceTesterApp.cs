using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WebServiceTester
{
    class WebServiceTesterApp
    {

        private static ILog Logger
        {
            get { return LogManager.GetLogger(typeof(WebServiceTester.WebServiceTesterApp)); }
        }


        static void Main(string[] args)
        {
            Logger.Info("*** Start");

            try
            {
                GetCounterparty_getAgreementList();
                GetCounterparty_getSendTo();
                GetCounterparty_storeDocumentSendTo();

                GetDocument_getDealSheet();
                GetDocument_getConfirmation();

                ConfirmationsManager_getPermissionKeys();
                ConfirmationsManager_getConfirmationTemplates();
                ConfirmationsManager_TradeConfirmationStatusChangeResponse();
            }
            catch( Exception ex ) {
                Console.WriteLine( String.Format( "ERROR:  {0}", ex.ToString()) );
            }

            Console.WriteLine("Hit Enter to Exit");
            Console.ReadLine();
        }

        static void GetCounterparty_getAgreementList()
        {
            var client = new CounterpartyService.CounterpartyClient();

            var request = new CounterpartyService.GetAgreementListRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.bookingCompanyShortName = "bookingCompanyShortName";
            request.cptyShortName = "cptyShortName";

            Logger.InfoFormat("GetCptyInfo GetAgreementListRequest(" +
                "tradingSystemCode={0}" +
                ", bookingCompanyShortName={1}" +
                ", cptyShortName={2})",
                request.tradingSystemCode, request.bookingCompanyShortName, request.cptyShortName);

            var response = client.getAgreementList(request);

            Logger.InfoFormat("GetCptyInfo GetAgreementListResponse(agreements={0})",
                String.Join(",", response.agreements.ToList().Select(x => x.agreementTypeCode)));
        }

        static void GetCounterparty_getSendTo()
        {
            var client = new CounterpartyService.CounterpartyClient();

            var request = new CounterpartyService.GetDocumentSendToRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.cptyShortName = "cptyShortName";
            request.documentTypeCode = "documentTypeCode";
            request.cdtyCode = "cdtyCode";
            request.settlementTypeCode = "settlementTypeCode";

            Logger.InfoFormat("GetCptyInfoService GetDocumentSendToRequest(" +
                    "tradingSystemCode={0}" +
                    ", cptyShortName={1}" +
                    ", documentType={2}" +
                    ", cdtyCode={3}" +
                    ", settlementType={4})",
                 request.tradingSystemCode == null ? "null" : request.tradingSystemCode,
                 request.cptyShortName == null ? "null" : request.cptyShortName,
                 request.documentTypeCode == null ? "null" : request.documentTypeCode,
                 request.cdtyCode == null ? "null" : request.cdtyCode,
                 request.settlementTypeCode == null ? "null" : request.settlementTypeCode
             );

            var response = client.getDocumentSendTo(request);

            Logger.InfoFormat("GetCptyInfoService: GetDocumentSendToResponse(sendTos={0})",
              String.Join(",", response.sendTos.ToList().Select( x =>
                  String.Format("transmitMethodInd={0}, {1})", x.transmitMethodInd, 
                    x.transmitMethodInd == CounterpartyService.TransmitMethodInd.EMAIL
                        ? String.Format( "email={0}", x.emailAddress )
                        : String.Format( "fax={0} {1} {2}", x.emailAddress, x.faxCountryCode, x.faxAreaCode, x.faxLocalNumber)) )
              ));
        }

        static void GetCounterparty_storeDocumentSendTo()
        {
            var client = new CounterpartyService.CounterpartyClient();
            var request = new CounterpartyService.StoreDocumentSendToRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.cptyShortName = "cptyShortName ";
            request.documentTypeCode = "documentTypeCode";
            request.cdtyCode = "cdtyCode";
            request.settlementTypeCode = "settlementTypeCode";
            List<CounterpartyService.sendTo> sendTos = new List<CounterpartyService.sendTo>();

            var sendTo1 = new CounterpartyService.sendTo();
            sendTo1.transmitMethodInd = CounterpartyService.TransmitMethodInd.EMAIL;
            sendTo1.emailAddress = "joe@blow.com";
            sendTos.Add( sendTo1 );
            
            var response = client.storeDocumentSendTo( request );

            Logger.InfoFormat("GetCptyInfoService: StoreDocumentSendToResponse(success={0})", response.success );
        }

        static void GetDocument_getDealSheet()
        {
            var client = new GetDocumentService.GetDocumentClient();
            var request = new GetDocumentService.GetDealSheetRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.tradingSystemKey = "tradingSystemKey";

            Logger.InfoFormat("GetDocument GetDealSheetRequest(" +
                "tradingSystemCode={0}" +
                ", tradingSystemKey={1})",
                request.tradingSystemCode == null ? "null" : request.tradingSystemCode,
                request.tradingSystemKey == null ? "null" : request.tradingSystemKey);

            var response = client.getDealSheet(request);

            Logger.InfoFormat("GetDocument GetDealSheetResponse(" +
                "documentInd={0}" +
                ", url={1}" +
                ", documentLength={2})",
                response.objectFormatInd.ToString(),
                response.url == null ? "null" : response.url,
                response.objectStream == null ? "null" : "" + response.objectStream.Length);

        }

        static void GetDocument_getConfirmation()
        {
            var client = new GetDocumentService.GetDocumentClient();
            var request = new GetDocumentService.GetConfirmationRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.tradingSystemKey = "tradingSystemKey";

            Logger.InfoFormat("GetDocument GetConfirmationRequest(" +
                "tradingSystemCode={0}" +
                ", tradingSystemKey={1})",
                request.tradingSystemCode == null ? "null" : request.tradingSystemCode,
                request.tradingSystemKey == null ? "null" : request.tradingSystemKey);


            var response = client.getConfirmation(request);

            Logger.InfoFormat("GetDocument GetConfirmationResponse(" +
                "objectFormatInd={0}" +
                ", confirmCompleteFlag={1}" +
                ", url={2}" +
                ", objectStreamLength={3})",
                response.objectFormatInd.ToString(),
                response.confirmCompleteFlag.ToString(),
                response.url == null ? "null" : response.url,
                response.objectStream == null ? "null" : "" + response.objectStream.Length);
        }

        private static void ConfirmationsManager_getPermissionKeys()
        {
            var client = new ConfirmationsManagerService.ConfirmationsManagerClient();
            var request = new ConfirmationsManagerService.GetPermissionKeysRequest();
            request.userId = "jblow";
            request.applicationName = "CONFIRMS";

            var response = client.getPermissionKeys(request);

            Logger.InfoFormat("ConfirmationsManager GetPermissionKeysResponse(" +
                            "superUserFlag={0}" +
                          ", permissionKeyCodes={1})",
                          response.superUserFlag,
                          String.Join(",", response.permissionKeyCodes.ToList() ) );

        }

        private static void ConfirmationsManager_getConfirmationTemplates()
        {
            var client = new ConfirmationsManagerService.ConfirmationsManagerClient();
            var request = new ConfirmationsManagerService.GetConfirmationTemplatesRequest();
            request.tradingSystemCode = "tradingSystemCode";

            var response = client.getConfirmationTemplates(request);

            Logger.InfoFormat("ConfirmationsManager GetConfirmationTemplatesResponse(" +
                          "confirmationTemplates[{0}])",
                          String.Join(",", response.confirmationTemplates.ToList().Select( confirmationTemplate =>
                            String.Format("confirmationTemplate: templateName={0}, Attributes[{1}]", confirmationTemplate.templateName, 
                                String.Join( ",", confirmationTemplate.attributes.ToList().Select( attribute => 
                                    String.Format( "attribute: name={0}, value={1}", attribute.name, attribute.value )
                                ) )
                          
                            )
                         ) ) );
            
        }

        private static void ConfirmationsManager_TradeConfirmationStatusChangeResponse()
        {
            var client = new ConfirmationsManagerService.ConfirmationsManagerClient();
            var request = new ConfirmationsManagerService.TradeConfirmationStatusChangeRequest();
            request.tradingSystemCode = "tradingSystemCode";
            request.tradingSystemKey = "tradingSystemKey";
            request.confirmationStatusCode = "confirmationStatusCode";

            var response = client.tradeConfirmationStatusChange(request);

            Logger.InfoFormat("ConfirmationsManager TradeConfirmationStatusChangeResponse(" +
                          "success={0})", response.success );
                          
        }
        
    }
}
