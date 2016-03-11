package aff.confirm.opsmanager.opssubpub.opstrackingmodel;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;
//import com.gigaspaces.annotation.pojo.SpaceProperty;

import java.io.Serializable;
import java.util.Date;

//@SpaceClass
public class SummaryData implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/**
	 * 
	 */
	private long _id = -1;
    private long _tradeId = -1;
    private long _transactionSeq = -1;
    private long _bkrDbUpd = -1;
    private long _setcDbUpd = -1;
    private long _cptyDbUpd = -1;
    private long _noconfDbUpd = -1;
    private long _verblDbUpd = -1;
    private float _qtyTot = -1;
    private float _qty = -1;
    private long _version = -1;
    private long _recentInd = -1;
    private Date _inceptionDt;
    private Date _tradeDt;
    private Date _startDt;
    private Date _endDt;
    private Date _currentBusnDt;
    private Date _lastUpdateTimestampGmt;
    private Date _lastTrdEditTimestampGmt;
    private Date _finalApprovalTimestampGmt;
    private String _trdSysCode;
    private String _cmt;
    private String _cptyTradeId;
    private String _readyForFinalApprovalFlag;
    private String _hasProblemFlag;
    private String _finalApprovalFlag;
    private String _opsDetActFlag;
    private String _bkrRqmt;
    private String _bkrMeth;
    private String _bkrStatus;
    private String _setcRqmt;
    private String _setcMeth;
    private String _setcStatus;
    private String _cptyRqmt;
    private String _cptyMeth;
    private String _cptyStatus;
    private String _noconfRqmt;
    private String _noconfMeth;
    private String _noconfStatus;
    private String _verblRqmt;
    private String _verblMeth;
    private String _verblStatus;
    private String _groupXref;
    private String _cdtyCode;
    private String _xref;
    private String _cptySn;
    private String _uomDurCode;
    private String _locationSn;
    private String _book;
    private String _tradeTypeCode;
    private String _sttlType;
    private String _brokerSn;
    private String _comm;
    private String _buySellInd;
    private String _refSn;
    private String _payPrice;
    private String _recPrice;
    private String _seCptySn;
    private String _tradeStatCode;
    private String _cdtyGrpCode;
    private String _brokerPrice;
    private String _optnStrikePrice;
    private String _optnPremPrice;
    private String _optnPutCallInd;
    private String _priority;
    private String _plAmt;
    private String _efsFlag;
    private String _efsCptySn;
    private String _archiveFlag;
    private String _priceDesc;
    private String _cptyLn;
    private String _readyForReplyFlag;
    private String _migrateInd;
    private String _additionalConfirmSent;
    private String _analystName;
    private String _isTestBook;

    private String _tradeSysTicket;
    private String _tradeDesc;
    private String _quantityDescription;

    private String _bookingCoSn;
    private long   _bookingCoId = -1;
    private long   _cptyId = -1;
    private String _brokerLegalName;
    private int    _brokerId = -1;
    private String _cptyLegalName;

    private String _trader;
    private String _transportDesc;
    private String _permissionKey;


	//@SpaceId    //	Sets this field (ID) to be used in the construction
  //  @SpaceRouting
  //  @SpaceProperty(nullValue="-1")
    public long get_id() {
        return _id;
    }

    public void set_id(long _id) {
        this._id = _id;
    }

 //   @SpaceProperty(index = SpaceProperty.IndexType.BASIC, nullValue="-1")
    public long get_tradeId() {
        return _tradeId;
    }

    public void set_tradeId(long _tradeId) {
        this._tradeId = _tradeId;
    }

    public String get_trdSysCode() {
        return _trdSysCode;
    }

    public void set_trdSysCode(String _trdSysCode) {
        this._trdSysCode = _trdSysCode;
    }

  //  @SpaceProperty(nullValue="-1")
    public long get_version() {
        return _version;
    }

    public void set_version(long _version) {
        this._version = _version;
    }

    public Date get_currentBusnDt() {
        return _currentBusnDt;
    }

    public void set_currentBusnDt(Date _currentBusnDt) {
        this._currentBusnDt = _currentBusnDt;
    }

 //   @SpaceProperty(nullValue="-1")
    public long get_recentInd() {
        return _recentInd;
    }

    public void set_recentInd(long _recentInd) {
        this._recentInd = _recentInd;
    }

    public String get_cmt() {
        return _cmt;
    }

    public void set_cmt(String _cmt) {
        this._cmt = _cmt;
    }

    public String get_cptyTradeId() {
        return _cptyTradeId;
    }

    public void set_cptyTradeId(String _cptyTradeId) {
        this._cptyTradeId = _cptyTradeId;
    }

    public Date get_lastUpdateTimestampGmt() {
        return _lastUpdateTimestampGmt;
    }

    public void set_lastUpdateTimestampGmt(Date _lastUpdateTimestampGmt) {
        this._lastUpdateTimestampGmt = _lastUpdateTimestampGmt;
    }

    public Date get_lastTrdEditTimestampGmt() {
        return _lastTrdEditTimestampGmt;
    }

    public void set_lastTrdEditTimestampGmt(Date _lastTrdEditTimestampGmt) {
        this._lastTrdEditTimestampGmt = _lastTrdEditTimestampGmt;
    }

    public String get_readyForFinalApprovalFlag() {
        return _readyForFinalApprovalFlag;
    }

    public void set_readyForFinalApprovalFlag(String _readyForFinalApprovalFlag) {
        this._readyForFinalApprovalFlag = _readyForFinalApprovalFlag;
    }

    public String get_hasProblemFlag() {
        return _hasProblemFlag;
    }

    public void set_hasProblemFlag(String _hasProblemFlag) {
        this._hasProblemFlag = _hasProblemFlag;
    }

    public String get_finalApprovalFlag() {
        return _finalApprovalFlag;
    }

    public void set_finalApprovalFlag(String _finalApprovalFlag) {
        this._finalApprovalFlag = _finalApprovalFlag;
    }

    public Date get_finalApprovalTimestampGmt() {
        return _finalApprovalTimestampGmt;
    }

    public void set_finalApprovalTimestampGmt(Date _finalApprovalTimestampGmt) {
        this._finalApprovalTimestampGmt = _finalApprovalTimestampGmt;
    }

    public String get_opsDetActFlag() {
        return _opsDetActFlag;
    }

    public void set_opsDetActFlag(String _opsDetActFlag) {
        this._opsDetActFlag = _opsDetActFlag;
    }

 //   @SpaceProperty(index = SpaceProperty.IndexType.BASIC, nullValue="-1")
    public long get_transactionSeq() {
        return _transactionSeq;
    }

    public void set_transactionSeq(long _transactionSeq) {
        this._transactionSeq = _transactionSeq;
    }

    public String get_bkrRqmt() {
        return _bkrRqmt;
    }

    public void set_bkrRqmt(String _bkrRqmt) {
        this._bkrRqmt = _bkrRqmt;
    }

    public String get_bkrMeth() {
        return _bkrMeth;
    }

    public void set_bkrMeth(String _bkrMeth) {
        this._bkrMeth = _bkrMeth;
    }

    public String get_bkrStatus() {
        return _bkrStatus;
    }

    public void set_bkrStatus(String _bkrStatus) {
        this._bkrStatus = _bkrStatus;
    }

  //  @SpaceProperty(nullValue="-1")
    public long get_bkrDbUpd() {
        return _bkrDbUpd;
    }

    public void set_bkrDbUpd(long _bkrDbUpd) {
        this._bkrDbUpd = _bkrDbUpd;
    }

    public String get_setcRqmt() {
        return _setcRqmt;
    }

    public void set_setcRqmt(String _setcRqmt) {
        this._setcRqmt = _setcRqmt;
    }

    public String get_setcMeth() {
        return _setcMeth;
    }

    public void set_setcMeth(String _setcMeth) {
        this._setcMeth = _setcMeth;
    }

    public String get_setcStatus() {
        return _setcStatus;
    }

    public void set_setcStatus(String _setcStatus) {
        this._setcStatus = _setcStatus;
    }

 //   @SpaceProperty(nullValue="-1")
    public long get_setcDbUpd() {
        return _setcDbUpd;
    }

    public void set_setcDbUpd(long _setcDbUpd) {
        this._setcDbUpd = _setcDbUpd;
    }

    public String get_cptyRqmt() {
        return _cptyRqmt;
    }

    public void set_cptyRqmt(String _cptyRqmt) {
        this._cptyRqmt = _cptyRqmt;
    }

    public String get_cptyMeth() {
        return _cptyMeth;
    }

    public void set_cptyMeth(String _cptyMeth) {
        this._cptyMeth = _cptyMeth;
    }

    public String get_cptyStatus() {
        return _cptyStatus;
    }

    public void set_cptyStatus(String _cptyStatus) {
        this._cptyStatus = _cptyStatus;
    }

//    @SpaceProperty(nullValue="-1")
    public long get_cptyDbUpd() {
        return _cptyDbUpd;
    }

    public void set_cptyDbUpd(long _cptyDbUpd) {
        this._cptyDbUpd = _cptyDbUpd;
    }

    public String get_noconfRqmt() {
        return _noconfRqmt;
    }

    public void set_noconfRqmt(String _noconfRqmt) {
        this._noconfRqmt = _noconfRqmt;
    }

    public String get_noconfMeth() {
        return _noconfMeth;
    }

    public void set_noconfMeth(String _noconfMeth) {
        this._noconfMeth = _noconfMeth;
    }

    public String get_noconfStatus() {
        return _noconfStatus;
    }

    public void set_noconfStatus(String _noconfStatus) {
        this._noconfStatus = _noconfStatus;
    }

  //  @SpaceProperty(nullValue="-1")
    public long get_noconfDbUpd() {
        return _noconfDbUpd;
    }

    public void set_noconfDbUpd(long _noconfDbUpd) {
        this._noconfDbUpd = _noconfDbUpd;
    }

    public String get_verblRqmt() {
        return _verblRqmt;
    }

    public void set_verblRqmt(String _verblRqmt) {
        this._verblRqmt = _verblRqmt;
    }

    public String get_verblMeth() {
        return _verblMeth;
    }

    public void set_verblMeth(String _verblMeth) {
        this._verblMeth = _verblMeth;
    }

    public String get_verblStatus() {
        return _verblStatus;
    }

    public void set_verblStatus(String _verblStatus) {
        this._verblStatus = _verblStatus;
    }

 //   @SpaceProperty(nullValue="-1")
    public long get_verblDbUpd() {
        return _verblDbUpd;
    }

    public void set_verblDbUpd(long _verblDbUpd) {
        this._verblDbUpd = _verblDbUpd;
    }

    public String get_groupXref() {
        return _groupXref;
    }

    public void set_groupXref(String _groupXref) {
        this._groupXref = _groupXref;
    }

    public Date get_inceptionDt() {
        return _inceptionDt;
    }

    public void set_inceptionDt(Date _inceptionDt) {
        this._inceptionDt = _inceptionDt;
    }

    public String get_cdtyCode() {
        return _cdtyCode;
    }

    public void set_cdtyCode(String _cdtyCode) {
        this._cdtyCode = _cdtyCode;
    }

    public Date get_tradeDt() {
        return _tradeDt;
    }

    public void set_tradeDt(Date _tradeDt) {
        this._tradeDt = _tradeDt;
    }

    public String get_xref() {
        return _xref;
    }

    public void set_xref(String _xref) {
        this._xref = _xref;
    }

    public String get_cptySn() {
        return _cptySn;
    }

    public void set_cptySn(String _cptySn) {
        this._cptySn = _cptySn;
    }

 //   @SpaceProperty(nullValue="-1")
    public float get_qtyTot() {
        return _qtyTot;
    }

    public void set_qtyTot(float _qtyTot) {
        this._qtyTot = _qtyTot;
    }

 //   @SpaceProperty(nullValue="-1")
    public float get_qty() {
        return _qty;
    }

    public void set_qty(float _qty) {
        this._qty = _qty;
    }

    public String get_uomDurCode() {
        return _uomDurCode;
    }

    public void set_uomDurCode(String _uomDurCode) {
        this._uomDurCode = _uomDurCode;
    }

    public String get_locationSn() {
        return _locationSn;
    }

    public void set_locationSn(String _locationSn) {
        this._locationSn = _locationSn;
    }

    public Date get_startDt() {
        return _startDt;
    }

    public void set_startDt(Date _startDt) {
        this._startDt = _startDt;
    }

    public Date get_endDt() {
        return _endDt;
    }

    public void set_endDt(Date _endDt) {
        this._endDt = _endDt;
    }

    public String get_book() {
        return _book;
    }

    public void set_book(String _book) {
        this._book = _book;
    }

    public String get_tradeTypeCode() {
        return _tradeTypeCode;
    }

    public void set_tradeTypeCode(String _tradeTypeCode) {
        this._tradeTypeCode = _tradeTypeCode;
    }

    public String get_sttlType() {
        return _sttlType;
    }

    public void set_sttlType(String _sttlType) {
        this._sttlType = _sttlType;
    }

    public String get_brokerSn() {
        return _brokerSn;
    }

    public void set_brokerSn(String _brokerSn) {
        this._brokerSn = _brokerSn;
    }

    public String get_comm() {
        return _comm;
    }

    public void set_comm(String _comm) {
        this._comm = _comm;
    }

    public String get_buySellInd() {
        return _buySellInd;
    }

    public void set_buySellInd(String _buySellInd) {
        this._buySellInd = _buySellInd;
    }

    public String get_refSn() {
        return _refSn;
    }

    public void set_refSn(String _refSn) {
        this._refSn = _refSn;
    }

    public String get_payPrice() {
        return _payPrice;
    }

    public void set_payPrice(String _payPrice) {
        this._payPrice = _payPrice;
    }

    public String get_recPrice() {
        return _recPrice;
    }

    public void set_recPrice(String _recPrice) {
        this._recPrice = _recPrice;
    }

    public String get_seCptySn() {
        return _seCptySn;
    }

    public void set_seCptySn(String _seCptySn) {
        this._seCptySn = _seCptySn;
    }

    public String get_tradeStatCode() {
        return _tradeStatCode;
    }

    public void set_tradeStatCode(String _tradeStatCode) {
        this._tradeStatCode = _tradeStatCode;
    }

    public String get_cdtyGrpCode() {
        return _cdtyGrpCode;
    }

    public void set_cdtyGrpCode(String _cdtyGrpCode) {
        this._cdtyGrpCode = _cdtyGrpCode;
    }

    public String get_brokerPrice() {
        return _brokerPrice;
    }

    public void set_brokerPrice(String _brokerPrice) {
        this._brokerPrice = _brokerPrice;
    }

    public String get_optnStrikePrice() {
        return _optnStrikePrice;
    }

    public void set_optnStrikePrice(String _optnStrikePrice) {
        this._optnStrikePrice = _optnStrikePrice;
    }

    public String get_optnPremPrice() {
        return _optnPremPrice;
    }

    public void set_optnPremPrice(String _optnPremPrice) {
        this._optnPremPrice = _optnPremPrice;
    }

    public String get_optnPutCallInd() {
        return _optnPutCallInd;
    }

    public void set_optnPutCallInd(String _optnPutCallInd) {
        this._optnPutCallInd = _optnPutCallInd;
    }

    public String get_priority() {
        return _priority;
    }

    public void set_priority(String _priority) {
        this._priority = _priority;
    }

    public String get_plAmt() {
        return _plAmt;
    }

    public void set_plAmt(String _plAmt) {
        this._plAmt = _plAmt;
    }

    public String get_efsFlag() {
        return _efsFlag;
    }

    public void set_efsFlag(String _efsFlag) {
        this._efsFlag = _efsFlag;
    }

    public String get_efsCptySn() {
        return _efsCptySn;
    }

    public void set_efsCptySn(String _efsCptySn) {
        this._efsCptySn = _efsCptySn;
    }

    public String get_archiveFlag() {
        return _archiveFlag;
    }

    public void set_archiveFlag(String _archiveFlag) {
        this._archiveFlag = _archiveFlag;
    }

	public void set_priceDesc(String _priceDescr) {
		this._priceDesc = _priceDescr;
	}

	public String get_priceDesc() {
		return _priceDesc;
	}
	
    public String get_cptyLn() {
		return _cptyLn;
	}

	public void set_cptyLn(String ln) {
		_cptyLn = ln;
	}
	
	public String get_readyForReplyFlag() {
		return _readyForReplyFlag;
	}

	public void set_readyForReplyFlag(String readyForReplyFlag) {
		_readyForReplyFlag = readyForReplyFlag;
	}
	
	public String get_migrateInd() {
		return _migrateInd;
	}

	public void set_migrateInd(String ind) {
		_migrateInd = ind;
	}
	
	public String get_additionalConfirmSent() {
		return _additionalConfirmSent;
	}

	public void set_additionalConfirmSent(String _additionalConfirmSent) {
		this._additionalConfirmSent = _additionalConfirmSent;
	}

	public String get_analystName() {
		return _analystName;
	}

	public void set_analystName(String _analystName) {
		this._analystName = _analystName;
	}
	
	public String get_isTestBook() {
		return _isTestBook;
	}

	public void set_isTestBook(String _isTestBook) {
		this._isTestBook = _isTestBook;
	}

    public String get_tradeSysTicket()
    {
        return _tradeSysTicket;
    }

    public void set_tradeSysTicket(String _tradeSysTicket)
    {
        this._tradeSysTicket = _tradeSysTicket;
    }

    public String get_tradeDesc()
    {
        return _tradeDesc;
    }

    public void set_tradeDesc(String _tradeDesc)
    {
        this._tradeDesc = _tradeDesc;
    }

    public String get_quantityDescription()
    {
        return _quantityDescription;
    }

    public void set_quantityDescription(String _quantityDescription)
    {
        this._quantityDescription = _quantityDescription;
    }

    public String get_bookingCoSn()
    {
        return _bookingCoSn;
    }

    public void set_bookingCoSn(String _bookingCoSn)
    {
        this._bookingCoSn = _bookingCoSn;
    }

    public long get_bookingCoId()
    {
        return _bookingCoId;
    }

    public void set_bookingCoId(long _bookingCoId)
    {
        this._bookingCoId = _bookingCoId;
    }

    public long get_cptyId()
    {
        return _cptyId;
    }

    public void set_cptyId(long _cptyId)
    {
        this._cptyId = _cptyId;
    }

    public String get_brokerLegalName()
    {
        return _brokerLegalName;
    }

    public void set_brokerLegalName(String _brokerLegalName)
    {
        this._brokerLegalName = _brokerLegalName;
    }

    public int get_brokerId()
    {
        return _brokerId;
    }

    public void set_brokerId(int _brokerId)
    {
        this._brokerId = _brokerId;
    }

    public String get_cptyLegalName()
    {
        return _cptyLegalName;
    }

    public void set_cptyLegalName(String _cptyLegalName)
    {
        this._cptyLegalName = _cptyLegalName;
    }

    public String get_trader()
    {
        return _trader;
    }

    public void set_trader(String _trader)
    {
        this._trader = _trader;
    }

    public String get_transportDesc()
    {
        return _transportDesc;
    }

    public void set_transportDesc(String _transportDesc)
    {
        this._transportDesc = _transportDesc;
    }

    public String get_permissionKey()
    {
        return _permissionKey;
    }

    public void set_permissionKey(String _permissionKey)
    {
        this._permissionKey = _permissionKey;
    }

    @Override
	public String toString(){
		return  _id +"|"+_tradeId+"|"+_transactionSeq+"|"+_bkrDbUpd+"|"+_setcDbUpd+"|"+_cptyDbUpd+"|"+_noconfDbUpd+"|"+_verblDbUpd+"|"+_qtyTot+"|"+_qty+"|"+_version+"|"+_recentInd+"|"+_inceptionDt+"|"+_tradeDt+"|"+_startDt+"|"+_endDt+"|"+_currentBusnDt+"|"+_lastUpdateTimestampGmt+"|"+_lastTrdEditTimestampGmt+"|"+_finalApprovalTimestampGmt+"|"+_trdSysCode+"|"+_cmt+"|"+_cptyTradeId+"|"+_readyForFinalApprovalFlag+"|"+_hasProblemFlag+"|"+_finalApprovalFlag+"|"+_opsDetActFlag+"|"+_bkrRqmt+"|"+_bkrMeth+"|"+_bkrStatus+"|"+_setcRqmt+"|"+_setcMeth+"|"+_setcStatus+"|"+_cptyRqmt+"|"+_cptyMeth+"|"+_cptyStatus+"|"+_noconfRqmt+"|"+_noconfMeth+"|"+_noconfStatus+"|"+_verblRqmt+"|"+_verblMeth+"|"+_verblStatus+"|"+_groupXref+"|"+_cdtyCode+"|"+_xref+"|"+_cptySn+"|"+_uomDurCode+"|"+_locationSn+"|"+_book+"|"+_tradeTypeCode+"|"+_sttlType+"|"+_brokerSn+"|"+_comm+"|"+_buySellInd+"|"+_refSn+"|"+_payPrice+"|"+_recPrice+"|"+_seCptySn+"|"+_tradeStatCode+"|"+_cdtyGrpCode+"|"+_brokerPrice+"|"+_optnStrikePrice+"|"+_optnPremPrice+"|"+_optnPutCallInd+"|"+_priority+"|"+_plAmt+"|"+_efsFlag+"|"+_efsCptySn+"|"+_archiveFlag+"|"+_priceDesc+"|"+_cptyLn+"|"+_readyForReplyFlag+"|"+_migrateInd+"|"+_additionalConfirmSent+"|"+_analystName+"|"+_isTestBook;
   	}
}
