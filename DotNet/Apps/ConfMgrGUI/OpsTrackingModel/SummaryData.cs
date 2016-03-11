namespace OpsTrackingModel
{
    [System.Serializable]
    [NHibernate.Mapping.Attributes.Class(Table = "V_PC_TRADE_SUMMARY")]
    public class SummaryData:IOpsDataObj
    {
        //private const System.Int64 NULL_VALUE_INT64 = -1;
        private const System.Int32 NULL_VALUE_INT32 = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";
        public static double incAmount = 0;  //Israel 6/10/2015 -- changed back from GS increment of 1 to 0

        private string _additionalConfirmSent;
        private string _analystName;
        private string _archiveFlag;
        private System.Int32 _bookingCoId;
        private string _bookingCoSn;
        private System.Int32 _bkrDbUpd;
        private string _bkrMeth;
        private string _bkrRqmt;
        private string _bkrStatus;
        private string _book;
        private System.Int32 _brokerId;
        private string _brokerLegalName;
        private string _brokerPrice;
        private string _brokerSn;
        private string _buySellInd;
        private string _cdtyCode;
        private System.Int32 _cptyDbUpd;
        private string _cdtyGrpCode;
        private string _cmt;
        //private string _comm;
        private System.Int32 _cptyId;
        private string _cptyLegalName;
        //private string _cptyLn;
        private string _cptyMeth;
        private string _cptyRqmt;
        private string _cptySn;
        private string _cptyStatus;
        private string _cptyTradeId;
        private System.DateTime _currentBusnDt;
        //private string _efsCptySn;
        //private string _efsFlag;
        private System.DateTime _endDt;
        private string _finalApprovalFlag;
        private System.DateTime _finalApprovalTimestampGmt;
        private string _groupXref;
        private string _hasProblemFlag;
        private System.Int32 _id;
        private System.DateTime _inceptionDt;
        private string _isTestBook;
        private System.DateTime _lastTrdEditTimestampGmt;
        private System.DateTime _lastUpdateTimestampGmt;
        private string _locationSn;
        private string _migrateInd;
        private System.Int32 _noconfDbUpd;
        private string _noconfMeth;
        private string _noconfRqmt;
        private string _noconfStatus;
        private string _opsDetActFlag;
        private string _optnPremPrice;
        private string _optnPutCallInd;
        private string _optnStrikePrice;
        //private string _payPrice;
        private string _permissionKey;
        private string _plAmt;
        private string _priceDesc;
        private string _priority;
        private string _qryCode;
        //private float _qty;
        private string _QtyDesc;
        private float _qtyTot;
        private string _readyForFinalApprovalFlag;
        private string _readyForReplyFlag;
        private System.Int32 _recentInd;
        //private string _recPrice;
        private string _refSn;
        //private string _seCptySn;
        private System.Int32 _setcDbUpd;
        private string _setcMeth;
        private string _setcRqmt;
        private string _setcStatus;
        private System.DateTime _startDt;
        private string _sttlType;
        private string _tradeDesc;
        private System.DateTime _tradeDt;
        private System.Int32 _tradeId;
        private string _trader;
        private string _transportDesc;
        private string _tradeStatCode;
        private string _tradeTypeCode;
        private System.Int32 _transactionSeq;
        private string _trdSysCode;
        private string _tradeSysTicket;
        private System.Int32 _verblDbUpd;
        //private string _uomDurCode;
        private string _verblMeth;
        private string _verblRqmt;
        private string _verblStatus;
        private System.Int32 _version;
        private string _xref;

        public SummaryData()
        {
            this._additionalConfirmSent = null;
            this._analystName = null;
            this._archiveFlag = null;
            this._bkrDbUpd = NULL_VALUE_INT32; 
            this._bkrMeth = null;
            this._bkrRqmt = null;
            this._bkrStatus = null;
            this._book = null;
            this._bookingCoId = NULL_VALUE_INT32;
            this._bookingCoSn = null;
            this._brokerId = NULL_VALUE_INT32;
            this._brokerLegalName = null;
            this._brokerPrice = null;
            this._brokerSn = null;
            this._buySellInd = null;
            this._cdtyCode = null;
            this._cdtyGrpCode = null;
            this._cmt = null;
            //this._comm = null;
            this._cptyId = NULL_VALUE_INT32;
            this._cptyDbUpd = NULL_VALUE_INT32;
            this._cptyLegalName = null;
            //this._cptyLn = null;
            this._cptyMeth = null;
            this._cptyRqmt = null;
            this._cptySn = null;
            this._cptyStatus = null;
            this._cptyTradeId = null;
            this._currentBusnDt = System.DateTime.Parse(_nullDateTime);
            //this._efsCptySn = null;
            //this._efsFlag = null;
            this._endDt = System.DateTime.Parse(_nullDateTime);
            this._finalApprovalFlag = null;
            this._finalApprovalTimestampGmt = System.DateTime.Parse(_nullDateTime);
            this._groupXref = null;
            this._hasProblemFlag = null;
            this._id = NULL_VALUE_INT32;
            this._inceptionDt = System.DateTime.Parse(_nullDateTime);
            this._isTestBook = null;
            this._lastTrdEditTimestampGmt = System.DateTime.Parse(_nullDateTime);
            this._lastUpdateTimestampGmt = System.DateTime.Parse(_nullDateTime);
            this._locationSn = null;
            this._noconfDbUpd = NULL_VALUE_INT32;
            this._noconfMeth = null;
            this._noconfRqmt = null;
            this._noconfStatus = null;
            this._opsDetActFlag = null;
            this._optnPremPrice = null;
            this._optnPutCallInd = null;
            this._optnStrikePrice = null;
            //this._payPrice = null;
            this._permissionKey = null;
            this._plAmt = null;
            this._priceDesc = null;
            this._priority = null;
            this._qryCode = "N";
            //this._qty = NULL_VALUE_INT64;
            this._QtyDesc = null;
            this._qtyTot = NULL_VALUE_INT32;
            this._readyForFinalApprovalFlag = null;
            this._readyForReplyFlag = null;
            //this._recPrice = null;
            this._recentInd = NULL_VALUE_INT32; ;
            this._refSn = null;
            //this._seCptySn = null;
            this._setcDbUpd = NULL_VALUE_INT32; ;
            this._setcMeth = null;
            this._setcRqmt = null;
            this._setcStatus = null;
            this._startDt = System.DateTime.Parse(_nullDateTime);
            this._sttlType = null;
            this._tradeDesc = null;
            this._tradeDt = System.DateTime.Parse(_nullDateTime);
            this._tradeId = NULL_VALUE_INT32;
            this._trader = null;
            this._tradeStatCode = null;
            this._tradeTypeCode = null;
            this._transactionSeq = NULL_VALUE_INT32;
            this._transportDesc = null;
            this._trdSysCode = null;
            this._tradeSysTicket = null;
            //this._uomDurCode = null;
            this._verblDbUpd = NULL_VALUE_INT32; ;
            this._verblMeth = null;
            this._verblRqmt = null;
            this._verblStatus = null;
            this._version = NULL_VALUE_INT32; ;
            this._xref = null;
        }

        public string getSelect()
        {
            return Properties.Settings.Default.SummaryData;
        }

        public string AdditionalConfirmSent
        {
            get { return _additionalConfirmSent; }
            set { _additionalConfirmSent = value; }
        }

        public string AnalystName
        {
            get { return _analystName; }
            set { _analystName = value; }
        }

        /// <summary>Archive Flag</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "ARCHIVE_FLAG")]
        public virtual string ArchiveFlag
        {
            get { return _archiveFlag; }
            set { _archiveFlag = value; }
        }

        /// <summary> Booking Co Id </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BOOKING_CO_ID")]
        public virtual System.Int32 BookingCoId
        {
            get { return _bookingCoId; }
            set { _bookingCoId = value; }
        }

        /// <summary> Booking Company Short Name </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BOOKING_CO_SN")]
        public virtual string BookingCoSn
        {
            get { return _bookingCoSn; }
            set { _bookingCoSn = value; }
        }

        /// <summary> Broker Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_METH")]
        public virtual string BkrMeth
        {
            get { return _bkrMeth; }
            set { _bkrMeth = value; }
        }

        /// <summary> Broker DB Update </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_DB_UPD")]
        public virtual System.Int32 BkrDbUpd
        {
            get { return _bkrDbUpd; }
            set { _bkrDbUpd = value; }
        }

        /// <summary> Broker Requirement </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_RQMT")]
        public virtual string BkrRqmt
        {
            get { return _bkrRqmt; }
            set { _bkrRqmt = value; }
        }

        /// <summary> Broker Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BKR_STATUS")]
        public virtual string BkrStatus
        {
            get { return _bkrStatus; }
            set { _bkrStatus = value; }
        }

        /// <summary>Book</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BOOK")]
        public virtual string Book
        {
            get { return _book; }
            set { _book = value; }
        }

        /// <summary> Brokern Id </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_ID")]
        public virtual System.Int32 BrokerId
        {
            get { return _brokerId; }
            set { _brokerId = value; }
        }

        /// <summary> Broker Legal Name </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_LEGAL_NAME")]
        public virtual string BrokerLegalName
        {
            get { return _brokerLegalName; }
            set { _brokerLegalName = value; }
        }

        /// <summary>Broker Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_PRICE")]
        public virtual string BrokerPrice
        {
            get { return _brokerPrice; }
            set { _brokerPrice = value; }
        }

        /// <summary>Broker Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BROKER_SN")]
        public virtual string BrokerSn
        {
            get { return _brokerSn; }
            set { _brokerSn = value; }
        }

        /// <summary>Buy Sell Indicator</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "BUY_SELL_IND")]
        public virtual string BuySellInd
        {
            get { return _buySellInd; }
            set { _buySellInd = value; }
        }

        /// <summary>CDTY Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CDTY_CODE")]
        public virtual string CdtyCode
        {
            get { return _cdtyCode; }
            set { _cdtyCode = value; }
        }

        /// <summary>Cdty Group Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CDTY_GRP_CODE")]
        public virtual string CdtyGrpCode
        {
            get { return _cdtyGrpCode; }
            set { _cdtyGrpCode = value; }
        }

        /// <summary> Comment </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CMT")]
        public virtual string Cmt
        {
            get { return _cmt; }
            set { _cmt = value; }
        }

        /// <summary>COMM</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "COMM")]
        //public virtual string Comm
        //{
        //    get { return _comm; }
        //    set { _comm = value; }
        //}

        //public string CptyLn
        //{
        //    get { return _cptyLn; }
        //    set { _cptyLn = value; }
        //}

        /// <summary> Counterparty Paper DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_DB_UPD")]
        public virtual System.Int32 CptyDbUpd
        {
            get { return _cptyDbUpd; }
            set { _cptyDbUpd = value; }
        }


        /// <summary> Counterparty Id </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_ID")]
        public virtual System.Int32 CptyId
        {
            get { return _cptyId; }
            set { _cptyId = value; }
        }

        /// <summary> Counterparty Legal Name </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_LEGAL_NAME")]
        public virtual string CptyLegalName
        {
            get { return _cptyLegalName; }
            set { _cptyLegalName = value; }
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

        /// <summary>Cpty Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_SN")]
        public virtual string CptySn
        {
            get { return _cptySn; }
            set { _cptySn = value; }
        }

        /// <summary> Counterparty Paper Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_STATUS")]
        public virtual string CptyStatus
        {
            get { return _cptyStatus; }
            set { _cptyStatus = value; }
        }

        /// <summary> Counterparty Trade Id </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CPTY_TRADE_ID")]
        public virtual string CptyTradeId
        {
            get { return _cptyTradeId; }
            set { _cptyTradeId = value; }
        }

        /// <summary> Trade Version </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "CURRENT_BUSN_DT")]
        public virtual System.DateTime CurrentBusnDt
        {
            get
            {
                return _currentBusnDt.AddHours(incAmount);
            }
            set { _currentBusnDt = value; }
        }

        /// <summary>EFS Cpty Short Name</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "EFS_CPTY_SN")]
        //public virtual string EfsCptySn
        //{
        //    get { return _efsCptySn; }
        //    set { _efsCptySn = value; }
        //}

        /// <summary>EFS Flag</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "EFS_FLAG")]
        //public virtual string EfsFlag
        //{
        //    get { return _efsFlag; }
        //    set { _efsFlag = value; }
        //}

        /// <summary>End Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "END_DT")]
        public virtual System.DateTime EndDt
        {
            get
            {
                //return (_endDt.AddHours(incAmount)); 
                return _endDt;
            }
            set
            { // Because of bug in GS 6.5, we need to add 1 hour to the time retrieved from GS
                _endDt = value;
            }
        }

        /// <summary> Final Approval Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FINAL_APPROVAL_FLAG")]
        public virtual string FinalApprovalFlag
        {
            get { return _finalApprovalFlag; }
            set { _finalApprovalFlag = value; }
        }

        /// <summary> Final Approval Time Stamp GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FINAL_APPROVAL_TIMESTAMP_GMT")]
        public virtual System.DateTime FinalApprovalTimestampGmt
        {
            get
            {
                return _finalApprovalTimestampGmt.AddHours(incAmount);
            }
            set
            {
                _finalApprovalTimestampGmt = value;
            }
        }

        /// <summary>Group XREF</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "GROUP_XREF")]
        public virtual string GroupXref
        {
            get { return _groupXref; }
            set { _groupXref = value; }
        }

        /// <summary> Has Problem Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "HAS_PROBLEM_FLAG")]
        public virtual string HasProblemFlag
        {
            get { return _hasProblemFlag; }
            set { _hasProblemFlag = value; }
        }

        /// <summary> Gets the unique identifier. </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
        [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_TRADE_DATA")]
        public virtual System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Inception Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INCEPTION_DT")]
        public virtual System.DateTime InceptionDt
        {
            get
            {
                return _inceptionDt.AddHours(incAmount);
            }
            set
            {
                _inceptionDt = value;
            }
        }

        public string IsTestBook
        {
            get { return _isTestBook; }
            set { _isTestBook = value; }
        }

        /// <summary> Last Trade Edit Time Stamp in GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LAST_TRD_EDIT_TIMESTAMP_GMT")]
        public virtual System.DateTime LastTrdEditTimestampGmt
        {
            get
            {
                return (_lastTrdEditTimestampGmt.AddHours(incAmount));
            }
            set
            {
                _lastTrdEditTimestampGmt = value;
            }
        }

        /// <summary> Last Update Time Stamp in GMT </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LAST_UPDATE_TIMESTAMP_GMT")]
        public virtual System.DateTime LastUpdateTimestampGmt
        {
            get
            {
                return (_lastUpdateTimestampGmt.AddHours(incAmount));
            }
            set
            {
                _lastUpdateTimestampGmt = value;
            }
        }

        /// <summary>Location Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "LOCATION_SN")]
        public virtual string LocationSn
        {
            get { return _locationSn; }
            set { _locationSn = value; }
        }

        public string MigrateInd
        {
            get { return _migrateInd; }
            set { _migrateInd = value; }
        }

        /// <summary> No Confirm DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_DB_UPD")]
        public virtual System.Int32 NoconfDbUpd
        {
            get { return _noconfDbUpd; }
            set { _noconfDbUpd = value; }
        }

        /// <summary> No Confirm Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_METH")]
        public virtual string NoconfMeth
        {
            get { return _noconfMeth; }
            set { _noconfMeth = value; }
        }

        /// <summary> No Confirm Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_RQMT")]
        public virtual string NoconfRqmt
        {
            get { return _noconfRqmt; }
            set { _noconfRqmt = value; }
        }

        /// <summary> No Confirm Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "NOCONF_STATUS")]
        public virtual string NoconfStatus
        {
            get { return _noconfStatus; }
            set { _noconfStatus = value; }
        }

        /// <summary> Ops Determine Action Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPS_DET_ACT_FLAG")]
        public virtual string OpsDetActFlag
        {
            get { return _opsDetActFlag; }
            set { _opsDetActFlag = value; }
        }

        /// <summary>Option Premium Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_PREM_PRICE")]
        public virtual string OptnPremPrice
        {
            get { return _optnPremPrice; }
            set { _optnPremPrice = value; }
        }

        /// <summary>Option Put Call Indicator</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_PUT_CALL_IND")]
        public virtual string OptnPutCallInd
        {
            get { return _optnPutCallInd; }
            set { _optnPutCallInd = value; }
        }

        /// <summary>Option Strike Price</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "OPTN_STRIKE_PRICE")]
        public virtual string OptnStrikePrice
        {
            get { return _optnStrikePrice; }
            set { _optnStrikePrice = value; }
        }

        /// <summary>Pay Price</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PAY_PRICE")]
        //public virtual string PayPrice
        //{
        //    get { return _payPrice; }
        //    set { _payPrice = value; }
        //}

        /// <summary>PL Amount</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PERMISSION_KEY")]
        public virtual string PermissionKey
        {
            get { return _permissionKey; }
            set { _permissionKey = value; }
        }

        /// <summary>PL Amount</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PL_AMT")]
        public virtual string PlAmt
        {
            get { return _plAmt; }
            set { _plAmt = value; }
        }

        /// <summary>Price Description</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PRICE_DESC")]
        public virtual string PriceDesc
        {
            get { return _priceDesc; }
            set { _priceDesc = value; }
        }

        /// <summary>Priority</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "PRIORITY")]
        public virtual string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public string QryCode
        {
            get { return _qryCode; }
            set { _qryCode = value; }
        }

        /// <summary>Quantity</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "QTY")]
        //public virtual float Qty
        //{
        //    get { return _qty; }
        //    set { _qty = value; }
        //}

        /// <summary> Qty Desc </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "QTY_DESC")]
        public virtual string QuantityDescription
        {
            get { return _QtyDesc; }
            set { _QtyDesc = value; }
        }

        /// <summary>Quantity Total</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "QTY_TOT")]
        public virtual float QtyTot
        {
            get { return _qtyTot; }
            set { _qtyTot = value; }
        }

        /// <summary> Ready for Final Approval Flag </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "READY_FOR_FINAL_APPROVAL_FLAG")]
        public virtual string ReadyForFinalApprovalFlag
        {
            get { return _readyForFinalApprovalFlag; }
            set { _readyForFinalApprovalFlag = value; }
        }

        public string ReadyForReplyFlag
        {
            get { return _readyForReplyFlag; }
            set { _readyForReplyFlag = value; }
        }

        /// <summary> Recent Indicator </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "RECENT_IND")]
        public virtual System.Int32 RecentInd
        {
            get { return _recentInd; }
            set { _recentInd = value; }
        }

        /// <summary>Receive Price</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "REC_PRICE")]
        //public virtual string RecPrice
        //{
        //    get { return _recPrice; }
        //    set { _recPrice = value; }
        //}

        /// <summary>Reference Short Name</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "REF_SN")]
        public virtual string RefSn
        {
            get { return _refSn; }
            set { _refSn = value; }
        }

        public string RplyRdyToSndFlag  // added for database loading support
        {
            set { ReadyForReplyFlag = value; }
        }

        /// <summary>Sempra Company</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SE_CPTY_SN")]
        //public virtual string SeCptySn
        //{
        //    get { return _seCptySn; }
        //    set { _seCptySn = value; }
        //}

        /// <summary> Our Paper DB UPD </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_DB_UPD")]
        public virtual System.Int32 SetcDbUpd
        {
            get { return _setcDbUpd; }
            set { _setcDbUpd = value; }
        }

        /// <summary> Our Paper Method </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_METH")]
        public virtual string SetcMeth
        {
            get { return _setcMeth; }
            set { _setcMeth = value; }
        }

        /// <summary> Our Paper Rqmt </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_RQMT")]
        public virtual string SetcRqmt
        {
            get { return _setcRqmt; }
            set { _setcRqmt = value; }
        }

        /// <summary> Our Paper Status </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "SETC_STATUS")]
        public virtual string SetcStatus
        {
            get { return _setcStatus; }
            set { _setcStatus = value; }
        }

        /// <summary>Start Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "START_DT")]
        public virtual System.DateTime StartDt
        {
            get
            {
                //return (_startDt.AddHours(incAmount)); 
                return _startDt;
            }
            set
            {
                _startDt = value;
            }
        }

        /// <summary>Settle Type</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "STTL_TYPE")]
        public virtual string SttlType
        {
            get { return _sttlType; }
            set { _sttlType = value; }
        }

        /// <summary> Trade Desc </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_DESC")]
        public virtual string TradeDesc
        {
            get { return _tradeDesc; }
            set { _tradeDesc = value; }
        }

        /// <summary>Trade Date</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_DT")]
        public virtual System.DateTime TradeDt
        {
            get
            {
                return (_tradeDt.AddHours(incAmount));

            }
            set
            {
                _tradeDt = value;
            }
        }

        /// <summary> Trade ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_ID")]
        public virtual System.Int32 TradeId
        {
            get { return _tradeId; }
            set { _tradeId = value; }
        }

        /// <summary>Trade  Stat Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_STAT_CODE")]
        public virtual string TradeStatCode
        {
            get { return _tradeStatCode; }
            set { _tradeStatCode = value; }
        }

        /// <summary>Trade Type Code</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADE_TYPE_CODE")]
        public virtual string TradeTypeCode
        {
            get { return _tradeTypeCode; }
            set { _tradeTypeCode = value; }
        }

        /// <summary>Trader</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRADER")]
        public virtual string Trader
        {
            get { return _trader; }
            set { _trader = value; }
        }

        /// <summary> Transaction Sequence </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRANSACTION_SEQ")]
        public virtual System.Int32 TransactionSeq
        {
            get { return _transactionSeq; }
            set { _transactionSeq = value; }
        }

        /// <summary> Transport Description </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRANSPORT_DESC")]
        public virtual string TransportDesc
        {
            get { return _transportDesc; }
            set { _transportDesc = value; }
        }

        /// <summary> Trade Sys Code </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRD_SYS_CODE")]
        public virtual string TrdSysCode
        {
            get { return _trdSysCode; }
            set { _trdSysCode = value; }
        }

        /// <summary> Trade Sys Ticket </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "TRD_SYS_TICKET")]
        public virtual string TradeSysTicket
        {
            get { return _tradeSysTicket; }
            set { _tradeSysTicket = value; }
        }

        /// <summary>UOM Duration Code</summary>
        //[NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "UOM_DUR_CODE")]
        //public virtual string UomDurCode
        //{
        //    get { return _uomDurCode; }
        //    set { _uomDurCode = value; }
        //}

        /// <summary> Verbal Rqmt DB UPD</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_DB_UPD")]
        public virtual System.Int32 VerblDbUpd
        {
            get { return _verblDbUpd; }
            set { _verblDbUpd = value; }
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

        /// <summary> Verbal Rqmt Status</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERBL_STATUS")]
        public virtual string VerblStatus
        {
            get { return _verblStatus; }
            set { _verblStatus = value; }
        }

        /// <summary> Trade Version </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "VERSION")]
        public virtual System.Int32 Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>XRef</summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "XREF")]
        public virtual string Xref
        {
            get { return _xref; }
            set { _xref = value; }
        }

    }
}
