namespace OpsTrackingModel
{
    [NHibernate.Mapping.Attributes.Class(Table = "V_PC_TRADE_SUMMARY")]
    public class SummaryData
    {
        private const System.Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";

        private System.Int32 _id;
        private System.Int32 _tradeId;
        private string _trdSysCode;
        private int _version;
        private System.DateTime _currentBusnDt;
        private int _recentInd;
        private string _cmt;
        private System.DateTime _lastUpdateTimeStampGmt;
        private System.DateTime _lastTrdEditTimeStampGmt;
        private string _readyForFinalApprovalFlag;
        private string _hasProblemFlag;
        private string _finalApprovalFlag;
        private System.DateTime _finalApprovalTimeStampGmt;
        private string _opsDetActFlag;
        private System.Int32 _transactionSeq;
        private string _bkrRqmt;
        private string _bkrMeth;
        private string _bkrStatus;
        private System.Int32 _bkrDbUpd;
        private string _setcRqmt;
        private string _setcMeth;
        private string _setcStatus;
        private System.Int32 _setcDbUpd;
        private string _cptyRqmt;
        private string _cptyMeth;
        private string _cptyStatus;
        private System.Int32 _cptyDbUpd;
        private string _noConfRqmt;
        private string _noConfMeth;
        private string _noConfStatus;
        private System.Int32 _noConfDbUpd;
        private string _verblRqmt;
        private string _verblMeth;
        private string _verblStatus;
        private System.Int32 _verblDbUpd;
        private string _groupXref;
        private System.DateTime _inceptionDt;
        private string _cdtyCode;
        private System.DateTime _tradeDt;
        private string _xRef;
        private string _cptySn;
        private float _qtyTot;
        private float _qty;
        private string _uomDurCode;
        private string _locationSn;
        private string _priceDescr;
        private System.DateTime _startDt;
        private System.DateTime _endDt;
        private string _book;
        private string _tradeTypeCode;
        private string _sttlType;
        private string _brokerSn;
        private string _comm;
        private string _buySellInd;
        private string _refSn;
        private string _payPrice;
        private string _recPrice;
        private string _seCptySn;
        private string _tradeStatCode;
        private string _cdtyGrpCode;
        private string _brokerPrice;
        private string _optnStrikePrice;
        private string _optnPremPrice;
        private string _optnPutCallInd;
        private string _priority;
        private string _plAmt;
        private string _efsFlag;
        private string _efsCptySn;
        private string _archiveFlag;

        /// <summary>Archive Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "ARCHIVE_FLAG")]
        public virtual string ArchiveFlag
        {
            get { return _archiveFlag; }
            set { _archiveFlag = value; }
        }

        /// <summary>EFS Cpty Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "EFS_CPTY_SN")]
        public virtual string EfsCptySn
        {
            get { return _efsCptySn; }
            set { _efsCptySn = value; }
        }

        /// <summary>EFS Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "EFS_FLAG")]
        public virtual string EfsFlag
        {
            get { return _efsFlag; }
            set { _efsFlag = value; }
        }

        /// <summary>PL Amount</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PL_AMT")]
        public virtual string PlAmt
        {
            get { return _plAmt; }
            set { _plAmt = value; }
        }

        /// <summary>Priority</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PRIORITY")]
        public virtual string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>Option Put Call Indicator</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_PUT_CALL_IND")]
        public virtual string OptnPutCallInd
        {
            get { return _optnPutCallInd; }
            set { _optnPutCallInd = value; }
        }

        /// <summary>Option Premium Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_PREM_PRICE")]
        public virtual string OptnPremPrice
        {
            get { return _optnPremPrice; }
            set { _optnPremPrice = value; }
        }

        /// <summary>Option Strike Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_STRIKE_PRICE")]
        public virtual string OptnStrikePrice
        {
            get { return _optnStrikePrice; }
            set { _optnStrikePrice = value; }
        }

        /// <summary>Broker Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_PRICE")]
        public virtual string BrokerPrice
        {
            get { return _brokerPrice; }
            set { _brokerPrice = value; }
        }

        /// <summary>Cdty Group Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CDTY_GRP_CODE")]
        public virtual string CdtyGrpCode
        {
            get { return _cdtyGrpCode; }
            set { _cdtyGrpCode = value; }
        }

        /// <summary>Trade  Stat Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_STAT_CODE")]
        public virtual string TradeStatCode
        {
            get { return _tradeStatCode; }
            set { _tradeStatCode = value; }
        }

        /// <summary>Sempra Company</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SE_CPTY_SN")]
        public virtual string SeCptySn
        {
            get { return _seCptySn; }
            set { _seCptySn = value; }
        }

        /// <summary>Receive Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "REC_PRICE")]
        public virtual string RecPrice
        {
            get { return _recPrice; }
            set { _recPrice = value; }
        }

        /// <summary>Pay Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PAY_PRICE")]
        public virtual string PayPrice
        {
            get { return _payPrice; }
            set { _payPrice = value; }
        }

        /// <summary>Reference Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "REF_SN")]
        public virtual string RefSn
        {
            get { return _refSn; }
            set { _refSn = value; }
        }

        /// <summary>Buy Sell Indicator</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BUY_SELL_IND")]
        public virtual string BuySellInd
        {
            get { return _buySellInd; }
            set { _buySellInd = value; }
        }

        /// <summary>COMM</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "COMM")]
        public virtual string Comm
        {
            get { return _comm; }
            set { _comm = value; }
        }

        /// <summary>Broker Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_SN")]
        public virtual string BrokerSn
        {
            get { return _brokerSn; }
            set { _brokerSn = value; }
        }

        /// <summary>Settle Type</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "STTL_TYPE")]
        public virtual string SttlType
        {
            get { return _sttlType; }
            set { _sttlType = value; }
        }

        /// <summary>Trade Type Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_TYPE_CODE")]
        public virtual string TradeTypeCode
        {
            get { return _tradeTypeCode; }
            set { _tradeTypeCode = value; }
        }

        /// <summary>Book</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BOOK")]
        public virtual string Book
        {
            get { return _book; }
            set { _book = value; }
        }

        /// <summary>End Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "END_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime EndDt
        {
            get { return _endDt; }
            set { _endDt = value; }
        }

        /// <summary>Start Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "START_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime StartDt
        {
            get { return _startDt; }
            set { _startDt = value; }
        }

        /// <summary>Price Description</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PRICE_DESC")]
        public virtual string PriceDescr
        {
            get { return _priceDescr; }
            set { _priceDescr = value; }
        }

        /// <summary>Location Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LOCATION_SN")]
        public virtual string LocationSn
        {
            get { return _locationSn; }
            set { _locationSn = value; }
        }

        /// <summary>UOM Duration Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "UOM_DUR_CODE")]
        public virtual string UomDurCode
        {
            get { return _uomDurCode; }
            set { _uomDurCode = value; }
        }

        /// <summary>Quantity</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "QTY")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual float Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        /// <summary>Quantity Total</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "QTY_TOT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual float QtyTot
        {
            get { return _qtyTot; }
            set { _qtyTot = value; }
        }

        /// <summary>Cpty Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_SN")]
        public virtual string CptySn
        {
            get { return _cptySn; }
            set { _cptySn = value; }
        }

        /// <summary>XRef</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "XREF")]
        public virtual string XRef
        {
            get { return _xRef; }
            set { _xRef = value; }
        }

        /// <summary>Trade Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime TradeDt
        {
            get { return _tradeDt; }
            set { _tradeDt = value; }
        }

        /// <summary>CDTY Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CDTY_CODE")]
        public virtual string CdtyCode
        {
            get { return _cdtyCode; }
            set { _cdtyCode = value; }
        }

        /// <summary>Inception Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INCEPTION_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime InceptionDt
        {
            get { return _inceptionDt; }
            set { _inceptionDt = value; }
        }

        /// <summary>Group XREF</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "GROUP_XREF")]
        public virtual string GroupXref
        {
            get { return _groupXref; }
            set { _groupXref = value; }
        }

        /// <summary> Verbal Rqmt DB UPD</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_DB_UPD")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 VerblDbUpd
        {
            get { return _verblDbUpd; }
            set { _verblDbUpd = value; }
        }

        /// <summary> Verbal Rqmt Status</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_STATUS")]
        public virtual string VerblStatus
        {
            get { return _verblStatus; }
            set { _verblStatus = value; }
        }

        /// <summary> Verbal Rqmt Method</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_METH")]
        public virtual string VerblMeth
        {
            get { return _verblMeth; }
            set { _verblMeth = value; }
        }

        /// <summary> Verbal Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_RQMT")]
        public virtual string VerblRqmt
        {
            get { return _verblRqmt; }
            set { _verblRqmt = value; }
        }

        /// <summary> No Confirm DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_DB_UPD")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 NoConfDbUpd
        {
            get { return _noConfDbUpd; }
            set { _noConfDbUpd = value; }
        }

        /// <summary> No Confirm Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_STATUS")]
        public virtual string NoConfStatus
        {
            get { return _noConfStatus; }
            set { _noConfStatus = value; }
        }

        /// <summary> No Confirm Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_METH")]
        public virtual string NoConfMeth
        {
            get { return _noConfMeth; }
            set { _noConfMeth = value; }
        }

        /// <summary> No Confirm Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_RQMT")]
        public virtual string NoConfRqmt
        {
            get { return _noConfRqmt; }
            set { _noConfRqmt = value; }
        }

        /// <summary> Counterparty Paper DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_DB_UPD")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 CptyDbUpd
        {
            get { return _cptyDbUpd; }
            set { _cptyDbUpd = value; }
        }

        /// <summary> Counterparty Paper Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_STATUS")]
        public virtual string CptyStatus
        {
            get { return _cptyStatus; }
            set { _cptyStatus = value; }
        }

        /// <summary> Counterparty Paper Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_METH")]
        public virtual string CptyMeth
        {
            get { return _cptyMeth; }
            set { _cptyMeth = value; }
        }

        /// <summary> Counterparty Paper Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_RQMT")]
        public virtual string CptyRqmt
        {
            get { return _cptyRqmt; }
            set { _cptyRqmt = value; }
        }

        /// <summary> Sempra Paper DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_DB_UPD")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 SetcDbUpd
        {
            get { return _setcDbUpd; }
            set { _setcDbUpd = value; }
        }

        /// <summary> Sempra Paper Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_STATUS")]
        public virtual string SetcStatus
        {
            get { return _setcStatus; }
            set { _setcStatus = value; }
        }

        /// <summary> Sempra Paper Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_METH")]
        public virtual string SetcMeth
        {
            get { return _setcMeth; }
            set { _setcMeth = value; }
        }

        /// <summary> Sempra Paper Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_RQMT")]
        public virtual string SetcRqmt
        {
            get { return _setcRqmt; }
            set { _setcRqmt = value; }
        }

        /// <summary> Broker DB Update </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_DB_UPD")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 BkrDbUpd
        {
            get { return _bkrDbUpd; }
            set { _bkrDbUpd = value; }
        }

        /// <summary> Broker Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_STATUS")]
        public virtual string BkrStatus
        {
            get { return _bkrStatus; }
            set { _bkrStatus = value; }
        }

        /// <summary> Broker Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_METH")]
        public virtual string BkrMeth
        {
            get { return _bkrMeth; }
            set { _bkrMeth = value; }
        }

        /// <summary> Broker Requirement </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_RQMT")]
        public virtual string BkrRqmt
        {
            get { return _bkrRqmt; }
            set { _bkrRqmt = value; }
        }

        /// <summary> Transaction Sequence </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRANSACTION_SEQ")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 TransactionSeq
        {
            get { return _transactionSeq; }
            set { _transactionSeq = value; }
        }

        /// <summary> Ops Determine Action Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPS_DET_ACT_FLAG")]
        public virtual string OpsDetActFlag
        {
            get { return _opsDetActFlag; }
            set { _opsDetActFlag = value; }
        }

        /// <summary> Final Approval Time Stamp GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FINAL_APPROVAL_TIMESTAMP_GMT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime FinalApprovalTimeStampGmt
        {
            get { return _finalApprovalTimeStampGmt; }
            set { _finalApprovalTimeStampGmt = value; }
        }

        /// <summary> Final Approval Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FINAL_APPROVAL_FLAG")]
        public virtual string FinalApprovalFlag
        {
            get { return _finalApprovalFlag; }
            set { _finalApprovalFlag = value; }
        }

        /// <summary> Has Problem Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "HAS_PROBLEM_FLAG")]
        public virtual string HasProblemFlag
        {
            get { return _hasProblemFlag; }
            set { _hasProblemFlag = value; }
        }

        /// <summary> Ready for Final Approval Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "READY_FOR_FINAL_APPROVAL_FLAG")]
        public virtual string ReadyForFinalApprovalFlag
        {
            get { return _readyForFinalApprovalFlag; }
            set { _readyForFinalApprovalFlag = value; }
        }

        /// <summary> Last Trade Edit Time Stamp in GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LAST_TRD_EDIT_TIMESTAMP_GMT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime LastTrdEditTimeStampGmt
        {
            get { return _lastTrdEditTimeStampGmt; }
            set { _lastTrdEditTimeStampGmt = value; }
        }

        /// <summary> Last Update Time Stamp in GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LAST_UPDATE_TIMESTAMP_GMT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime LastUpdateTimeStampGmt
        {
            get { return _lastUpdateTimeStampGmt; }
            set { _lastUpdateTimeStampGmt = value; }
        }

        /// <summary> Comment </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CMT")]
        public virtual string Cmt
        {
            get { return _cmt; }
            set { _cmt = value; }
        }

        /// <summary> Recent Indicator </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "RECENT_IND")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual int RecentInd
        {
            get { return _recentInd; }
            set { _recentInd = value; }
        }

        /// <summary> Trade Version </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CURRENT_BUSN_DT")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = _nullDateTime)]
        public virtual System.DateTime CurrentBusnDt
        {
            get { return _currentBusnDt; }
            set { _currentBusnDt = value; }
        }

        /// <summary> Trade Version </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERSION")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary> Trade Sys Code </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRD_SYS_CODE")]
        public virtual string TrdSysCode
        {
            get { return _trdSysCode; }
            set { _trdSysCode = value; }
        }

        /// <summary> Trade ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_ID")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 TradeId
        {
            get { return _tradeId; }
            set { _tradeId = value; }
        }

        /// <summary> Gets the unique identifier. </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
            [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
            [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_TRADE_DATA")]
        [GigaSpaces.Core.Metadata.SpaceProperty(NullValue = SummaryData._nullNumberValue)]
        public virtual System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public SummaryData()
        {
            this._id = _nullNumberValue;
            this._tradeId = _nullNumberValue;
            this._qtyTot = _nullNumberValue;
            this._qty = _nullNumberValue;
            this._version = _nullNumberValue; ;
            this._currentBusnDt = System.DateTime.Parse(_nullDateTime);
            this._recentInd = _nullNumberValue; ;
            this._lastUpdateTimeStampGmt = System.DateTime.Parse(_nullDateTime);
            this._lastTrdEditTimeStampGmt = System.DateTime.Parse(_nullDateTime);
            this._finalApprovalTimeStampGmt = System.DateTime.Parse(_nullDateTime);
            this._transactionSeq = _nullNumberValue; ;
            this._bkrDbUpd = _nullNumberValue; ;
            this._setcDbUpd = _nullNumberValue; ;
            this._cptyDbUpd = _nullNumberValue; ;
            this._noConfDbUpd = _nullNumberValue; ;
            this._verblDbUpd = _nullNumberValue; ;
            this._inceptionDt = System.DateTime.Parse(_nullDateTime);
            this._tradeDt = System.DateTime.Parse(_nullDateTime);
            this._startDt = System.DateTime.Parse(_nullDateTime);
            this._endDt = System.DateTime.Parse(_nullDateTime);
            this._trdSysCode = null;
            this._cmt = null;
            this._readyForFinalApprovalFlag = null;
            this._hasProblemFlag = null;
            this._finalApprovalFlag = null;
            this._opsDetActFlag = null;
            this._bkrRqmt = null;
            this._bkrMeth = null;
            this._bkrStatus = null;
            this._setcRqmt = null;
            this._setcMeth = null;
            this._setcStatus = null;
            this._cptyRqmt = null;
            this._cptyMeth = null;
            this._cptyStatus = null;
            this._noConfRqmt = null;
            this._noConfMeth = null;
            this._noConfStatus = null;
            this._verblRqmt = null;
            this._verblMeth = null;
            this._verblStatus = null;
            this._groupXref = null;
            this._cdtyCode = null;
            this._xRef = null;
            this._cptySn = null;
            this._uomDurCode = null;
            this._locationSn = null;
            this._priceDescr = null;
            this._book = null;
            this._tradeTypeCode = null;
            this._sttlType = null;
            this._brokerSn = null;
            this._comm = null;
            this._buySellInd = null;
            this._refSn = null;
            this._payPrice = null;
            this._recPrice = null;
            this._seCptySn = null;
            this._tradeStatCode = null;
            this._cdtyGrpCode = null;
            this._brokerPrice = null;
            this._optnStrikePrice = null;
            this._optnPremPrice = null;
            this._optnPutCallInd = null;
            this._priority = null;
            this._plAmt = null;
            this._efsFlag = null;
            this._efsCptySn = null;
            this._archiveFlag = null;
        }
    }
}
