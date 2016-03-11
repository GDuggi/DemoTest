namespace OpsTrackingModel
{
    [NHibernate.Mapping.Attributes.Class(Table = "V_PC_TRADE_RQMT")]
    public class RqmtData
    {
        private const System.Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";


        private System.Int32 _id;
        private System.Int32 _tradeId;
        private System.Int32 _rqmtTradeNotifyId;
        private string _rqmt;
        private string _status;
        private System.DateTime _completedDate;
        private System.DateTime _completedTimeStampGmt;
        private string _reference;
        private System.Int32 _cancelTradeNotifyId;
        private string _cmt;
        private string _secondCheckFlag;
        private System.Int32 _transactionSeq;
        private string _finalApprovalFlag;
        private string _displayText;
        private string _category;
        private string _terminalFlag;
        private string _problemFlag;
        private string _guiColorCode;
        private string _delphiConstant;
        private string _prelimAppr;

        /// <summary>Preliminary Approver</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PRELIM_APPR")]
        public virtual string PrelimAppr
        {
            get { return _prelimAppr; }
            set { _prelimAppr = value; }
        }

        /// <summary>Delphi Constant</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "DELPHI_CONSTANT")]
        public virtual string DelphiConstant
        {
            get { return _delphiConstant; }
            set { _delphiConstant = value; }
        }

        /// <summary>GUI Color Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "GUI_COLOR_CODE")]
        public virtual string GuiColorCode
        {
            get { return _guiColorCode; }
            set { _guiColorCode = value; }
        }

        /// <summary>Problem Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PROBLEM_FLAG")]
        public virtual string ProblemFlag
        {
            get { return _problemFlag; }
            set { _problemFlag = value; }
        }

        /// <summary>Terminal Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TERMINAL_FLAG")]
        public virtual string TerminalFlag
        {
            get { return _terminalFlag; }
            set { _terminalFlag = value; }
        }

        /// <summary>Category</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CATEGORY")]
        public virtual string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>Display Text</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "DISPLAY_TEXT")]
        public virtual string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        /// <summary>Final Approval Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FINAL_APPROVAL_FLAG")]
        public virtual string FinalApprovalFlag
        {
            get { return _finalApprovalFlag; }
            set { _finalApprovalFlag = value; }
        }

        /// <summary>Transaction Sequence</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRANSACTION_SEQ")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = RqmtData._nullNumberValue)]
        public virtual System.Int32 TransactionSeq
        {
            get { return _transactionSeq; }
            set { _transactionSeq = value; }
        }

        /// <summary>Second Check Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SECOND_CHECK_FLAG")]
        public virtual string SecondCheckFlag
        {
            get { return _secondCheckFlag; }
            set { _secondCheckFlag = value; }
        }

        /// <summary>Comment</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CMT")]
        public virtual string Cmt
        {
            get { return _cmt; }
            set { _cmt = value; }
        }

        /// <summary>Cancel Trade Notify ID</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CANCEL_TRADE_NOTIFY_ID")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = RqmtData._nullNumberValue)]
        public virtual System.Int32 CancelTradeNotifyId
        {
            get { return _cancelTradeNotifyId; }
            set { _cancelTradeNotifyId = value; }
        }

        /// <summary>Reference</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "REFERENCE")]
        public virtual string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        /// <summary>Completed Date GMT</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "COMPLETED_TIMESTAMP_GMT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime CompletedTimeStampGmt
        {
            get { return _completedTimeStampGmt; }
            set { _completedTimeStampGmt = value; }
        }

        /// <summary>Completed Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "COMPLETED_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime CompletedDate
        {
            get { return _completedDate; }
            set { _completedDate = value; }
        }

        /// <summary>Rqmt Status</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "STATUS")]
        public virtual string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>Rqmt</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "RQMT")]
        public virtual string Rqmt
        {
            get { return _rqmt; }
            set { _rqmt = value; }
        }

        /// <summary>Rqmt Trade Notify ID</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "RQMT_TRADE_NOTIFY_ID")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = RqmtData._nullNumberValue)]
        public virtual System.Int32 RqmtTradeNotifyId
        {
            get { return _rqmtTradeNotifyId; }
            set { _rqmtTradeNotifyId = value; }
        }

        /// <summary>Trade ID</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_ID")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = RqmtData._nullNumberValue)]
        public virtual System.Int32 TradeId
        {
            get { return _tradeId; }
            set { _tradeId = value; }
        }

        /// <summary> Trade Rqmt ID </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
            [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
            [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_TRADE_DATA")]
        [GigaSpaces.Core.Metadata.SpaceID]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = RqmtData._nullNumberValue)]
        public virtual System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public RqmtData()
        {
            this._id = _nullNumberValue;
            this._tradeId = _nullNumberValue;
            this._rqmtTradeNotifyId = _nullNumberValue;
            this._rqmt = null;
            this._status = null;
            this._completedDate = System.DateTime.Parse(_nullDateTime);
            this._completedTimeStampGmt = System.DateTime.Parse(_nullDateTime);
            this._reference = null;
            this._cancelTradeNotifyId = _nullNumberValue;
            this._cmt = null;
            this._secondCheckFlag = null;
            this._transactionSeq = _nullNumberValue;
            this._finalApprovalFlag = null;
            this._displayText = null;
            this._category = null;
            this._terminalFlag = null;
            this._problemFlag = null;
            this._guiColorCode = null;
            this._delphiConstant = null;
            this._prelimAppr = null;
        }

    }
}
