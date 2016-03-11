using System;
using System.Windows.Forms;
using DBAccess;
using DevExpress.XtraEditors;
using log4net;
using OpsTrackingModel;
using Sempra.Confirm.InBound.Comm;
using Sempra.Confirm.InBound.ImageEdit;
using System.Linq;

namespace ConfirmInbound
{
    internal class DocumentTransmitter
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (DocumentTransmitter));
        private static readonly string PRODUCT_INB_APP_CODE = "CNF";
        private const string FORM_NAME = "DocumentTransmitter";
        private const string FORM_ERROR_CAPTION = "Document Transmitter Form Error";
        private IImagesDal imagesDal;
        private TransmitDestination transmitDestination;

        private IVaulter vaulter;
        private IXmitRequestDal xmitRequestDal;
        
        public bool UserCanceled { get; private set; }

        public TransmitDestination TransmitDestination
        {
            get { return transmitDestination; }
        }

        public DocumentTransmitter(IImagesDal imagesDal, IVaulter vaulter, IXmitRequestDal xmitRequestDal, TransmitDestination transmitDestination = null)
        {
            this.imagesDal = imagesDal;
            this.transmitDestination = transmitDestination;
            this.vaulter = vaulter;
            this.xmitRequestDal = xmitRequestDal;
        }

        public void SendToGateway(AssociatedDoc assDoc, SummaryData sumData)
        {                       
            if (assDoc == null)
            {
                throw new Exception("Unable to find an associated doc to transmit." + Environment.NewLine +
                     "Error CNF-421 in " + FORM_NAME + ".SendToGateway().");
            }

            if (sumData == null)
            {
                throw new Exception("Unable to transmit an associated document without an attached summary data." + Environment.NewLine +
                     "Error CNF-422 in " + FORM_NAME + ".SendToGateway().");
            }

            DetermineTransmitDestination(assDoc);                
            if (transmitDestination == null)
            {
                UserCanceled = true;
                return;
            }

            int xmitRequestId = xmitRequestDal.SaveAssociatedDocumentXmitRequest(assDoc.Id,
                transmitDestination.Type, transmitDestination.Value, InboundSettings.UserName);

            var callbackGen = new TransmitCallbackGenerator(xmitRequestId);

            var destinations = transmitDestination.Value.Split(';');
            foreach (string destin in destinations)
            {
                var request = new FaxRequest { FaxNumber = destin, AppCode = PRODUCT_INB_APP_CODE };

                request.AppSender = InboundSettings.UserName;

                request.ReceiptAction = new CallbackAction("http",
                    callbackGen.GenerateUrl(TransmitCallbackAction.Queued));


                request.Action.SuccessAction = new CallbackAction("http",
                    callbackGen.GenerateUrl(TransmitCallbackAction.Success));

                request.Action.FailureAction = new CallbackAction("http",
                    callbackGen.GenerateUrl(TransmitCallbackAction.Failed));

                request.AppReference = Convert.ToString(sumData.TradeSysTicket);
                request.Recipient = sumData.CptySn;

                AddDocumentToRequest(assDoc, request);

                var email = new Emailer();
                if (transmitDestination.Type == TransmitDestinationType.EMAIL)
                {
                    request.SendMethod = "EMAIL";
                    request.EmailAddress = request.FaxNumber;
                    request.FaxNumber = "";
                    email.SendToFaxGateway(InboundSettings.EmailHost,
                        InboundSettings.TransmissionGatewayEmailFromAddress,
                        InboundSettings.TransmissionGatewayEmailToAddress,
                        request,
                        "");

                }
                else
                {
                    request.SendMethod = "FAX";
                    email.SendToFaxGateway(InboundSettings.EmailHost,
                        InboundSettings.TransmissionGatewayEmailFromAddress,
                        InboundSettings.TransmissionGatewayEmailToAddress,
                        request,
                        "");
                }
            }
        }

        private void DetermineTransmitDestination(AssociatedDoc assDoc)
        {
            if (transmitDestination.IsValid)
            {
                if (!InboundSettings.IsProductionSystem && !transmitDestination.IsValidNonProdSendToAddress())
                {
                    throw new Exception(String.Format(
                        "Unable to send to '{0}' from a non-production system.", transmitDestination.Value) + Environment.NewLine +
                         "Error CNF-423 in " + FORM_NAME + ".DetermineTransmitDestination().");
                }
                return;                
            }

            using (var rqstForm = new frmAssignFaxNo())           
            {
                rqstForm.SetFaxNumbers("",assDoc.XmitValue ?? "");
                rqstForm.SetIsAssociatedDoc(true);
                rqstForm.ShowDialog();                               
                transmitDestination = rqstForm.TransmitDestination;                
            }
        }
        
        private void AddDocumentToRequest(AssociatedDoc assDoc, FaxRequest request)
        {
            var originalSelection = ImagesEventManager.Instance.CurrentSelected;
            var associatedDocsDto = originalSelection;
            bool wasChanged = false;
            try
            {
                if (originalSelection.DocsId != assDoc.Id || originalSelection.Type != ImagesDtoType.Associated)
                {
                    associatedDocsDto = imagesDal.GetByDocId(assDoc.Id, ImagesDtoType.Associated);
                    ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(associatedDocsDto, false));
                    wasChanged = true;
                }

                var currentEditor = TifEditor.GetCurrentEditor();
                var publishToBytes = currentEditor.PublishToBytes();
                associatedDocsDto.MarkupImage = publishToBytes;
                imagesDal.Update(associatedDocsDto);                
                vaulter.VaultAssociatedDoc(assDoc.Id, null);

                request.AddDocument(assDoc.FileName, currentEditor.TotalPages, publishToBytes);
            }
            finally
            {
                if (wasChanged)
                {
                    ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(originalSelection, false));
                }                
            }
        }

        private static void LogAndDisplayException(string errorMessagePrefix, Exception ex)
        {
            Logger.Error(errorMessagePrefix + ex.Message, ex);
            XtraMessageBox.Show("Error CNF-424: An error occurred while processing ErrorMessage: " + errorMessagePrefix + "." + Environment.NewLine +
                   "Error CNF-424 in " + FORM_NAME + ".LogAndDisplayException(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
