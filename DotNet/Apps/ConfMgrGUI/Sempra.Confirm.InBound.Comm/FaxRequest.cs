using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Sempra.Confirm.InBound.Comm
{
    public abstract class GatewayRequest
    {
        private readonly IList<AttachDocument> documents = new List<AttachDocument>();

        public GatewayRequest()
        {
            Vault = new VaultInfo();
            Action = new RequestAction();
        }

        public string AppSender { get; set; }

        public string RequestFileCreationPath { get; set; }

        public string AppCode { get; set; }

        public string AppReference { get; set; }

        public string Comment { get; set; }

        public CallbackAction ReceiptAction { get; set; }
        public RequestAction Action { get; set; }

        public VaultInfo Vault { get; set; }

        public IList<AttachDocument> Documents
        {
            get { return new List<AttachDocument>(documents); }
        }

        public void AddDocument(string documentName, int totalPages, byte[] documentContents)
        {
            documents.Add(new AttachDocument
            {
                Location = "A",
                DocumentFileName = documentName,
                TotalPages = totalPages,
                DocumentContents = documentContents
            });
        }        
    }

    public class FaxRequest : GatewayRequest
    {
        public const string _METHOD = "FAX";


        public FaxRequest()
        {
            Date = new DateTime();
        }


        public string EmailAddress { get; set; }

        public string SendMethod { get; set; }

        public string CoverSheetTitle { get; set; }

        public DateTime Date { get; set; }

        public string CountryCode { get; set; }

        public string AreaCode { get; set; }

        public string FaxNumber { get; set; }

        public string Recipient { get; set; }

        public string CoverSheetToName { get; set; }

        public string CoverSheetToCompany { get; set; }

        public string CoverSheetFromName { get; set; }

        public string CoverSheetFromCompany { get; set; }

        public string CoverSheetRegarding { get; set; }

        public string CoverSheetMessage { get; set; }
    }

    public class RequestAction
    {
        public CallbackAction SuccessAction;
        public CallbackAction FailureAction;
    }

    public class AttachDocument
    {
        public string Location { get; set; }
        public int TotalPages { get; set; }
        public string DocumentFileName { get; set; }
        public byte[] DocumentContents { get; set; }

        public AttachDocument()
        {
            Location = "A";
        }
    }

    public class CallbackAction
    {
        public string MethodType { get; set; }
        public string MethodValue { get; set; }

        public CallbackAction(string methodType, string methodValue)
        {
            MethodType = methodType;
            MethodValue = methodValue;
        }

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(MethodType) && !string.IsNullOrWhiteSpace(MethodValue); }
        }

        public static bool IsNullOrInvalid(CallbackAction callbackAction)
        {
            return callbackAction == null || !callbackAction.IsValid;
        }
    }

    public class VaultInfo 
    {
        private string name;
        private string onBehalfofUser;
        private string title;
        private string author;
        private string description;
        private Hashtable vaultIndex = new Hashtable();

        public Hashtable VaultIndex
        {
            get { return vaultIndex; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string OnBehalfofUser
        {
            get { return onBehalfofUser; }
            set { onBehalfofUser = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public void AddIndex(string vaultId, string vaultValue)
        {
            if (vaultValue == null && "".Equals(vaultValue) )
            {
                if (vaultIndex.ContainsKey(vaultId))
                {
                    vaultIndex.Remove(vaultId);
                }
            }
            else 
            {
                vaultIndex[vaultId] = vaultValue;
            }
            
        }

    }
}
