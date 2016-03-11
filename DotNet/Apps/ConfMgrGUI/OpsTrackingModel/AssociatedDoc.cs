namespace OpsTrackingModel
{
    [System.Serializable]
    [NHibernate.Mapping.Attributes.Class(Table = "ASSOCIATED_DOCS")]
    public class AssociatedDoc:IOpsDataObj
    {
        private const System.Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";

        public static string BROKER_PAPER = "XQBBP";
        public static string CPTY_PAPER = "XQCCP";
        public static string SEMPRA_PAPER = "XQCSP";
        public static string RS_RECEIVED = "RECVD";
        public static string RS_APPROVED = "APPR";
        public static string RS_CANCEL = "CXL";
        public static string RS_DISPUTED = "DISP";
        //public static string RS_EXCTD = "EXCTD";
        public static string RS_SIGNED = "SIGNED";
        public static string RS_PRELIM = "PRELIM";
        public static string RS_EXPCT = "EXPCT"; // BROKER AND CP PAPER
        public static string RS_SENT = "SENT";  // Our Paper


        public static string ASSOCIATED = "ASSOCIATED";
        public static string UNASSOCIATED = "UNASSOCIATED";
        public static string RESERVED = "RESERVED";
        public static string APPROVED = "APPROVED";
        public static string PRELIM = "PRE-APPROVED";
        public static string DISCARDED = "DISCARDED";
        public static string OPEN = "OPEN";
        public static string CLOSED = "CLOSED";
        public static string DISPUTED = "DISPUTED";
        public static string VAULTED = "VAULTED";

        public static string[] terminalPaperCodes = new string[6] { "ACPTD", "APPR", "CPRCV", "SIGNED", "PSTVL", "VERBL" };

        // Added for Confirmations.  Can't update/associated a document if rqmt status code is in one of the following states...
        public static string[] sempraPaperCodes = new string[6] {"NEW","PREP","EXT_REVIEW","TRADER","MGR","OK_TO_SEND"};

        
        private System.Int32 _id;

        private System.Int32 _inboundDocsId;

        private System.Int32 _indexVal;

        private System.Int32 _tradeRqmtId;

        private System.Int32 _tradeId;

        private string _trdSysTicket;
        private string _trdSysCode;
        private string _fileName;
        private string _docStatusCode;
        private string _associatedBy;

        private System.DateTime _associatedDt;

        private string _finalApprovedBy;

        private System.DateTime _finalApprovedDt;

        private string _disputedBy;
        private System.DateTime _disputedDt;

        private string _discardedBy;
        private System.DateTime _discardedDt;

        private string _vaultedBy;
        private System.DateTime _vaultedDt;

        private string _cdtyGroupCode;
        private string _cptyShortName;
        private string _brokerShortName;
        private string _docTypeCode;
        private string _secondValidateReqFlag = "N";
        private string _tradeFinalApprovalFlag = "N";
        private string _xmitStatusCode;
        private string _xmitValue;
        private string _sentTo;

        private bool multipleAssociatedDocs = false;


        /// <summary> Gets the unique identifier. </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
            [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
            [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_ASSOCIATED_DOCS")]
        public virtual System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary> Parent Document ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INBOUND_DOCS_ID")]
        public virtual System.Int32 InboundDocsId
        {
            get { return _inboundDocsId; }
            set { _inboundDocsId = value; }
        }

        /// <summary> Index Value </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INDEX_VAL")]
        public virtual System.Int32 IndexVal
        {
            get { return _indexVal; }
            set { _indexVal = value; }
        }

        /// <summary>Trading Systen Ticket</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRD_SYS_TICKET")]
        public virtual string TrdSysTicket
        {
            get { return _trdSysTicket; }
            set { _trdSysTicket = value; }
        }

        /// <summary>Trading Systen Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRD_SYS_CODE")]
        public virtual string TrdSysCode
        {
            get { return _trdSysCode; }
            set { _trdSysCode = value; }
        }

        /// <summary> Name of this inbound document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "FILE_NAME")]
        public virtual string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary> Associated Trade ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_ID")]
        public virtual System.Int32 TradeId
        {
            get { return _tradeId; }
            set { _tradeId = value; }
        }

        /// <summary> Document status code. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DOC_STATUS_CODE")]
        public virtual string DocStatusCode
        {
            get { return _docStatusCode; }
            set { _docStatusCode = value; }
        }

        /// <summary> Who associated this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "ASSOCIATED_BY")]
        public virtual string AssociatedBy
        {
            get { return _associatedBy; }
            set { _associatedBy = value; }
        }

        /// <summary> Time stamp of when document was accociated. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "ASSOCIATED_DT")]
        public virtual System.DateTime AssociatedDt
        {
            get { return _associatedDt; }
            set { _associatedDt = value; }
        }

        /// <summary> Who final approved this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "FINAL_APPROVED_BY")]
        public virtual string FinalApprovedBy
        {
            get { return _finalApprovedBy; }
            set { _finalApprovedBy = value; }
        }

        /// <summary> Time stamp of when document was final approved. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "FINAL_APPROVED_DT")]
        public virtual System.DateTime FinalApprovedDt
        {
            get { return _finalApprovedDt; }
            set { _finalApprovedDt = value; }
        }

        /// <summary> Who disputed this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DISPUTED_BY")]
        public virtual string DisputedBy
        {
            get { return _disputedBy; }
            set { _disputedBy = value; }
        }

        /// <summary> Time stamp of when document was disputed. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DISPUTED_DT")]
        public virtual System.DateTime DisputedDt
        {
            get { return _disputedDt; }
            set { _disputedDt = value; }
        }

        /// <summary> Who discarded this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DISCARDED_BY")]
        public virtual string DiscardedBy
        {
            get { return _discardedBy; }
            set { _discardedBy = value; }
        }

        /// <summary> Time stamp of when document was discarded. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DISCARDED_DT")]
        public virtual System.DateTime DiscardedDt
        {
            get { return _discardedDt; }
            set { _discardedDt = value; }
        }

        /// <summary> Who vaulted this document. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "VAULTED_BY")]
        public virtual string VaultedBy
        {
            get { return _vaultedBy; }
            set { _vaultedBy = value; }
        }

        /// <summary> Time stamp of when document was vaulted. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "VAULTED_DT")]
        public virtual System.DateTime VaultedDt
        {
            get { return _vaultedDt; }
            set { _vaultedDt = value; }
        }

        /// <summary> Cdty Group Code. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "CDTY_GROUP_CODE")]
        public virtual string CdtyGroupCode
        {
            get { return _cdtyGroupCode; }
            set { _cdtyGroupCode = value; }
        }

        /// <summary> Cpty Short Name. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "CPTY_SN")]
        public virtual string CptyShortName
        {
            get { return _cptyShortName; }
            set { _cptyShortName = value; }
        }

        public virtual string CptySn
        {
            set { CptyShortName = value; }
        }

        /// <summary> Broker Short Name. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "BROKER_SN")]
        public virtual string BrokerShortName
        {
            get { return _brokerShortName; }
            set { _brokerShortName = value; }
        }

        public virtual string BrokerSn
        {
            set { BrokerShortName = value; }
        }

        /// <summary> Document type code. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DOC_TYPE_CODE")]
        public virtual string DocTypeCode
        {
            get { return _docTypeCode; }
            set { _docTypeCode = value; }
        }

        /// <summary> Document type code. </summary>                 
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "SEC_VALIDATE_REQ_FLAG")]
        public virtual string SecondValidateReqFlag
        {
            get { return _secondValidateReqFlag; }
            set { _secondValidateReqFlag = value; }
        }

        public virtual string SecValidateReqFlag
        {
            set { SecondValidateReqFlag = value; }
        }

        /// <summary> Trade Requirement ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "TRADE_RQMT_ID")]
        public virtual System.Int32 TradeRqmtId
        {
            get { return _tradeRqmtId; }
            set { _tradeRqmtId = value; }
        }

        public virtual bool MultipleAssociatedDocs
        {
            get { return multipleAssociatedDocs; }
            set { multipleAssociatedDocs = value; }
        }

        public string TradeFinalApprovalFlag
        {
            get { return _tradeFinalApprovalFlag; }
            set { _tradeFinalApprovalFlag = value; }
        }

        public string XmitStatusCode
        {
            get { return _xmitStatusCode; }
            set { _xmitStatusCode = value; }
        }

        public string XmitValue
        {
            get { return _xmitValue; }
            set { _xmitValue = value; }
        }

        public string SentTo
        {
            get { return _sentTo; }
            set { _sentTo = value; }
        }

        /// <summary> Default constructor. </summary>
        public AssociatedDoc()
        {
            this._id = AssociatedDoc._nullNumberValue;
            this._inboundDocsId = AssociatedDoc._nullNumberValue;
            this._indexVal = AssociatedDoc._nullNumberValue;
            this._tradeRqmtId = AssociatedDoc._nullNumberValue;
            this._tradeId = AssociatedDoc._nullNumberValue;
            this._trdSysTicket = null;
            this._trdSysCode = null;
            this._fileName = null;
            this._docStatusCode = null;
            this._associatedBy = null;
            this._associatedDt = System.DateTime.Parse(AssociatedDoc._nullDateTime);
            this._finalApprovedBy = null;
            this._finalApprovedDt = System.DateTime.Parse(AssociatedDoc._nullDateTime);
            this._disputedBy = null;
            this._disputedDt = System.DateTime.Parse(AssociatedDoc._nullDateTime);
            this._discardedBy = null;
            this._discardedDt = System.DateTime.Parse(AssociatedDoc._nullDateTime);
            this._vaultedBy = null;
            this._vaultedDt = System.DateTime.Parse(AssociatedDoc._nullDateTime);
            this._cdtyGroupCode = null;
            this._cptyShortName = null;
            this._brokerShortName = null;
            this._docTypeCode = null;
            this._secondValidateReqFlag = null;
            this._tradeFinalApprovalFlag = null;
            this._xmitStatusCode = null;
            this._xmitValue = null;
            this._sentTo = null;
        }

        public void SetDocStatus()
        {
            if ((this.DocTypeCode == AssociatedDoc.BROKER_PAPER) || (this.DocTypeCode == AssociatedDoc.CPTY_PAPER))
            {
                if ("Y".Equals(this.SecondValidateReqFlag))
                {
                    if (this.DocStatusCode == AssociatedDoc.RS_PRELIM || this.DocStatusCode == AssociatedDoc.PRELIM)
                    {
                        this.DocStatusCode = AssociatedDoc.RS_APPROVED;
                    }
                    else
                    {
                        this.DocStatusCode = AssociatedDoc.RS_PRELIM;
                    }
                }
                else
                {
                    this.DocStatusCode = AssociatedDoc.RS_APPROVED;
                }
            }
            else // THIS IS Our Paper..
            {
                this.DocStatusCode = AssociatedDoc.RS_SIGNED;
            }
        }

        public void UnAssociate()
        {
            this.CdtyGroupCode = null;
            this.CptyShortName = null;
            this.BrokerShortName = null;

            if (this.DocTypeCode == null)
            {
                return;  // this was an automatched document.
            }


            if ((this.DocTypeCode == AssociatedDoc.BROKER_PAPER) || (this.DocTypeCode == AssociatedDoc.CPTY_PAPER))
            {
                this.DocStatusCode = AssociatedDoc.RS_EXPCT;
            }
            else // THIS IS Our Paper..
            {
                this.DocStatusCode = AssociatedDoc.RS_SENT;
            }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.AssociatedDocs;
        }
    }
}
