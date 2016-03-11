/******************************************************************************\
* NHibernate Entity: InboundDoc
\*/

namespace OpsTrackingModel
{
    [System.Serializable]
    [NHibernate.Mapping.Attributes.Class(Table = "INBOUND_DOCS")]
    public class InboundDoc
    {
        private const System.Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";
        public static string DISCARDED = "DISCARDED";
        public static string OPEN = "OPEN";
        public static string CLOSED = "CLOSED";
        public static string IGNORED = "IGNORE";

        private System.Int32 _id;

        private string _callerRef;
        private string _sentTo;

        private System.DateTime _rcvdTS = System.DateTime.Now;

        private string _fileName;
        private string _sender;
        private string _cmt;
        private string _docStatusCode;
        private string _hasAutoAsctedFlag;
        private string _docUserName = "";
        private System.Int32 _workingTradeID = 0;

        /// <summary> Gets the unique identifier. </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
            [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
            [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_INBOUND_DOCS")]
        public virtual System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary> Time stamp of when document was received. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "RCVD_TS")]
        public virtual System.DateTime RcvdTS
        {
            get { return _rcvdTS; }
            set { _rcvdTS = value; }
        }

        /// <summary> Who sent this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "CALLER_REF")]
        public virtual string CallerRef
        {
            get { return _callerRef; }
            set { _callerRef = value; }
        }

        /// <summary> Sent to. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "SENT_TO")]
        public virtual string SentTo
        {
            get { return _sentTo; }
            set { _sentTo = value; }
        }

        /// <summary> Name of this inbound document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "FILE_NAME")]
        public virtual string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary> Who sent this document. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "SENDER")]
        public virtual string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        /// <summary> User comments. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "CMT")]
        public virtual string Cmt
        {
            get { return _cmt; }
            set { _cmt = value; }
        }

        /// <summary> Document status code. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "DOC_STATUS_CODE")]
        public virtual string DocStatusCode
        {
            get { return _docStatusCode; }
            set { _docStatusCode = value; }
        }

        /// <summary> Has been auto associated. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "HAS_AUTO_ASCTED_FLAG")]
        public virtual string HasAutoAsctedFlag
        {
            get { return _hasAutoAsctedFlag; }
            set { _hasAutoAsctedFlag = value; }
        }

        public virtual string DocUserName
        {
            get { return this._docUserName; }
            set { this._docUserName = value; }
        }

        public virtual System.Int32 WorkingTradeID
        {
            get { return this._workingTradeID; }
            set { this._workingTradeID = value; }
        }

        /// <summary> Default constructor. </summary>
        public InboundDoc()
        {
            this._id = InboundDoc._nullNumberValue;
            this._rcvdTS = System.DateTime.Parse(InboundDoc._nullDateTime);
            this._callerRef = null;
            this._sentTo = null;
            this._fileName = null;
            this._sender = null;
            this._cmt = null;
            this._docStatusCode = null;
            this._hasAutoAsctedFlag = null;
        }

    }
}
