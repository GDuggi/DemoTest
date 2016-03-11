package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.Date;

import javax.xml.bind.annotation.XmlElement;

public class TradeSummary implements Serializable{
	

	private String trdSysCode;
	private long _version;
	private Date _currentBusnDt;
	private long _recentInd;
	private String _cmt;
	private Date _lastUpdateTimestampGmt;
	private Date _lastTrdEditTimestampGmt;
	private String _finalApprovalFlag;
	private Date _finalApprovalTimestampGmt;
	private String _opsDetActFlag;
	private long _transactionSeq;
	private String _bkrRqmt;
	private String _bkrMeth;
	private String _bkrStatus;
	private long _bkrDbUpd;
	private String _setcRqmt;
	private String _setcMeth;
	private String _setcStatus;
	private long _setcDbUpd;
	private String _cptyRqmt;
	private String _cptyMeth;
	private String _cptyStatus;
	private long _cptyDbUpd;
	private String _noconfRqmt;
	private String _noconfMeth;
	private String _noconfStatus;
	private long _noconfDbUpd;
	private String _verblRqmt;
	private String _verblMeth;
	private String _verblStatus;
	private long _verblDbUpd;
	private long _id;
	private long _tradeId;
	private Date _inceptionDt;
	private String _cdtyCode;
	private Date _tradeDt;
	private String _xref;
	private String _cptySn;
	private float _qtyTot;
	private float _qty;
	private String _uomDurCode;
	private String _locationSn;
	private String _priceDesc;
	private Date _startDt;
	private Date _endDt;
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
	private String archiveFlag;
	private String groupXRef;
    private String _cptyLn;
    private String _readyForReplyFlag;
    private String _migrateInd;
    private String _analystName;
    private String _isTestBook;
    private String _additionalConfirmSent;
    
    

	
	
	public String getTrdSysCode() {
		return trdSysCode;
	}
	@XmlElement(name="TrdSysCode")
	public void setTrdSysCode(String tradingSystemCode) {
		this.trdSysCode = tradingSystemCode;
	}
	public long get_version() {
		return _version;
	}
	@XmlElement(name="Version")
	public void set_version(long version) {
		this._version = version;
	}
	public Date get_currentBusnDt() {
		return _currentBusnDt;
	}
	@XmlElement(name="CurrentBusnDt")
	public void set_currentBusnDt(Date currentBusnDate) {
		this._currentBusnDt = currentBusnDate;
	}
	public long get_recentInd() {
		return _recentInd;
	}
	@XmlElement(name="RecentInd")
	public void set_recentInd(long recentIndicator) {
		this._recentInd = recentIndicator;
	}
	public String get_cmt() {
		return _cmt;
	}
	@XmlElement(name="Cmt")
	public void set_cmt(String comment) {
		this._cmt = comment;
	}
	public Date get_lastUpdateTimestampGmt() {
		return _lastUpdateTimestampGmt;
	}
	@XmlElement(name="LastUpdateTimestampGmt")
	public void set_lastUpdateTimestampGmt(Date lastUpdateDateTime) {
		this._lastUpdateTimestampGmt = lastUpdateDateTime;
	}
	public Date get_lastTrdEditTimestampGmt() {
		return _lastTrdEditTimestampGmt;
	}
	@XmlElement(name="LastTrdEditTimestampGmt")
	public void set_lastTrdEditTimestampGmt(Date lastTradeEditDateTime) {
		this._lastTrdEditTimestampGmt = lastTradeEditDateTime;
	}
	public String get_finalApprovalFlag() {
		return _finalApprovalFlag;
	}
	@XmlElement(name="FinalApprovalFlag")
	public void set_finalApprovalFlag(String finalApprovalFlag) {
		this._finalApprovalFlag = finalApprovalFlag;
	}
	public Date get_finalApprovalTimestampGmt() {
		return _finalApprovalTimestampGmt;
	}
	@XmlElement(name="FinalApprovalTimestampGmt")
	public void set_finalApprovalTimestampGmt(Date finalApprovalDateTime) {
		this._finalApprovalTimestampGmt = finalApprovalDateTime;
	}
	public String get_opsDetActFlag() {
		return _opsDetActFlag;
	}
	@XmlElement(name="OpsDetActFlag")
	public void set_opsDetActFlag(String opsDetermineActionFlag) {
		this._opsDetActFlag = opsDetermineActionFlag;
	}
	public long get_transactionSeq() {
		return _transactionSeq;
	}
	@XmlElement(name="TransactionSeq")
	public void set_transactionSeq(long transactionSeq) {
		this._transactionSeq = transactionSeq;
	}
	public String get_bkrRqmt() {
		return _bkrRqmt;
	}
	@XmlElement(name="BkrRqmt")
	public void set_bkrRqmt(String brokerRqmt) {
		this._bkrRqmt = brokerRqmt;
	}
	public String get_bkrMeth() {
		return _bkrMeth;
	}
	@XmlElement(name="BkrMeth")
	public void set_bkrMeth(String brokerMethod) {
		this._bkrMeth = brokerMethod;
	}
	public String get_bkrStatus() {
		return _bkrStatus;
	}
	@XmlElement(name="BkrStatus")
	public void set_bkrStatus(String brokerStatus) {
		this._bkrStatus = brokerStatus;
	}
	public long get_bkrDbUpd() {
		return _bkrDbUpd;
	}
	@XmlElement(name="BkrDbUpd")
	public void set_bkrDbUpd (long brokerDbUpdate) {
		this._bkrDbUpd = brokerDbUpdate;
	}
	public String get_setcRqmt() {
		return _setcRqmt;
	}
	@XmlElement(name="SetcRqmt")
	public void set_setcRqmt(String rbsSempraRqmt) {
		this._setcRqmt = rbsSempraRqmt;
	}
	public String get_setcMeth() {
		return _setcMeth;
	}
	@XmlElement(name="SetcMeth")
	public void set_setcMeth(String rbsSempraMethod) {
		this._setcMeth = rbsSempraMethod;
	}
	public String get_setcStatus() {
		return _setcStatus;
	}
	@XmlElement(name="SetcStatus")
	public void set_setcStatus(String rbsSempraStatus) {
		this._setcStatus = rbsSempraStatus;
	}
	public long get_setcDbUpd() {
		return _setcDbUpd;
	}
	@XmlElement(name="SetcDbUpd")
	public void set_setcDbUpd(long rbsSempraDbUpdate) {
		this._setcDbUpd = rbsSempraDbUpdate;
	}
	public String get_cptyRqmt() {
		return _cptyRqmt;
	}
	@XmlElement(name="CptyRqmt")
	public void set_cptyRqmt(String cptyRqmt) {
		this._cptyRqmt = cptyRqmt;
	}
	public String get_cptyMeth() {
		return _cptyMeth;
	}
	@XmlElement(name="CptyMeth")
	public void set_cptyMeth(String cptyMethod) {
		this._cptyMeth = cptyMethod;
	}
	public String get_cptyStatus() {
		return _cptyStatus;
	}
	@XmlElement(name="CptyStatus")
	public void set_cptyStatus(String cptyStatus) {
		this._cptyStatus = cptyStatus;
	}
	public long get_cptyDbUpd() {
		return _cptyDbUpd;
	}
	@XmlElement(name="CptyDbUpd")
	public void set_cptyDbUpd(long cptyDbUpdate) {
		this._cptyDbUpd = cptyDbUpdate;
	}
	public String get_noconfRqmt() {
		return _noconfRqmt;
	}
	@XmlElement(name="NoconfRqmt")
	public void set_noconfRqmt(String noCnfRqmt) {
		this._noconfRqmt = noCnfRqmt;
	}
	public String get_noconfMeth() {
		return _noconfMeth;
	}
	@XmlElement(name="NoconfMeth")
	public void set_noconfMeth(String noCnfMethod) {
		this._noconfMeth = noCnfMethod;
	}
	public String get_noconfStatus() {
		return _noconfStatus;
	}
	@XmlElement(name="NoconfStatus")
	public void set_noconfStatus(String noCnfStatus) {
		this._noconfStatus = noCnfStatus;
	}
	public long get_noconfDbUpd() {
		return _noconfDbUpd;
	}
	
	@XmlElement(name="NoconfDbUpd") // *****************************************
	public void set_noconfDbUpd(long noCnfUpdate) {
		this._noconfDbUpd = noCnfUpdate;
	}
	public String get_verblRqmt() {
		return _verblRqmt;
	}
	@XmlElement(name="VerblRqmt")
	public void set_verblRqmt(String verbalRqmt) {
		this._verblRqmt = verbalRqmt;
	}
	public String get_verblMeth() {
		return _verblMeth;
	}
	@XmlElement(name="VerblMeth")
	public void set_verblMeth(String verbalMethod) {
		this._verblMeth = verbalMethod;
	}
	public String get_verblStatus() {
		return _verblStatus;
	}
	@XmlElement(name="VerblStatus")
	public void set_verblStatus(String verbalStatus) {
		this._verblStatus = verbalStatus;
	}
	public long get_verblDbUpd() {
		return _verblDbUpd;
	}
	@XmlElement(name="VerblDbUpd")
	public void set_verblDbUpd(long verbalUpdate) {
		this._verblDbUpd = verbalUpdate;
	}
	public long get_id() {
		return _id;
	}
	@XmlElement(name="Id")
	public void set_id(long id) {
		this._id = id;
	}
	public long get_tradeId() {
		return _tradeId;
	}
	@XmlElement(name="TradeId")
	public void set_tradeId(long tradeId) {
		this._tradeId = tradeId;
	}
	public Date get_inceptionDt() {
		return _inceptionDt;
	}
	@XmlElement(name="InceptionDt")
	public void set_inceptionDt(Date inceptionDate) {
		this._inceptionDt = inceptionDate;
	}
	
	public String get_cdtyCode() {
		return _cdtyCode;
	}
	@XmlElement(name="CdtyCode")
	public void set_cdtyCode(String cdtyCode) {
		this._cdtyCode = cdtyCode;
	}
	public Date get_tradeDt() {
		return _tradeDt;
	}
	@XmlElement(name="TradeDt")
	public void set_tradeDt(Date tradeDate) {
		this._tradeDt = tradeDate;
	}
	public String get_xref() {
		return _xref;
	}
	@XmlElement(name="Xref")
	public void set_xref(String ref) {
		_xref = ref;
	}
	public String get_cptySn() {
		return _cptySn;
	}
	@XmlElement(name="CptySn")
	public void set_cptySn(String cptySn) {
		this._cptySn = cptySn;
	}
	public float get_qtyTot() {
		return _qtyTot;
	}
	@XmlElement(name="QtyTot")
	public void set_qtyTot(float qtyTotal) {
		this._qtyTot = qtyTotal;
	}
	public float get_qty() {
		return _qty;
	}
	@XmlElement(name="Qty")
	public void set_qty(float qty) {
		this._qty = qty;
	}
	public String get_uomDurCode() {
		return _uomDurCode;
	}
	@XmlElement(name="UomDurCode")
	public void set_uomDurCode(String uomDurationCode) {
		this._uomDurCode = uomDurationCode;
	}
	public String get_locationSn() {
		return _locationSn;
	}
	@XmlElement(name="LocationSn")
	public void set_locationSn(String locationSn) {
		this._locationSn = locationSn;
	}
	public String get_priceDesc() {
		return _priceDesc;
	}
	@XmlElement(name="PriceDesc")
	public void set_priceDesc(String priceDesc) {
		this._priceDesc = priceDesc;
	}
	public Date get_startDt() {
		return _startDt;
	}
	@XmlElement(name="StartDt")
	public void set_startDt(Date startDate) {
		this._startDt = startDate;
	}
	public Date get_endDt() {
		return _endDt;
	}
	@XmlElement(name="EndDt")
	public void set_endDt(Date endDate) {
		this._endDt = endDate;
	}
	public String get_book() {
		return _book;
	}
	@XmlElement(name="Book")
	public void set_book(String book) {
		this._book = book;
	}
	public String get_tradeTypeCode() {
		return _tradeTypeCode;
	}
	@XmlElement(name="TradeTypeCode")
	public void set_tradeTypeCode(String tradeTypeCode) {
		this._tradeTypeCode = tradeTypeCode;
	}
	public String get_sttlType() {
		return _sttlType;
	}
	@XmlElement(name="SttlType")
	public void set_sttlType(String tradeSttlType) {
		this._sttlType = tradeSttlType;
	}
	public String get_brokerSn() {
		return _brokerSn;
	}
	@XmlElement(name="BrokerSn")
	public void set_brokerSn(String brokerSn) {
		this._brokerSn = brokerSn;
	}
	public String get_comm() {
		return _comm;
	}
	@XmlElement(name="Comm")
	public void set_comm(String commission) {
		this._comm = commission;
	}
	public String get_buySellInd() {
		return _buySellInd;
	}
	@XmlElement(name="BuySellInd")
	public void set_buySellInd(String buySellIndicator) {
		this._buySellInd = buySellIndicator;
	}
	public String get_refSn() {
		return _refSn;
	}
	@XmlElement(name="RefSn")
	public void set_refSn(String refSn) {
		this._refSn = refSn;
	}
	public String get_payPrice() {
		return _payPrice;
	}
	@XmlElement(name="PayPrice")
	public void set_payPrice(String payPrice) {
		this._payPrice = payPrice;
	}
	public String get_recPrice() {
		return _recPrice;
	}
	@XmlElement(name="RecPrice")
	public void set_recPrice(String recPrice) {
		this._recPrice = recPrice;
	}
	public String get_seCptySn() {
		return _seCptySn;
	}
	@XmlElement(name="SeCptySn")
	public void set_seCptySn(String rbsSempraSn) {
		this._seCptySn = rbsSempraSn;
	}
	public String get_tradeStatCode() {
		return _tradeStatCode;
	}
	@XmlElement(name="TradeStatCode")
	public void set_tradeStatCode(String tradeStatCode) {
		this._tradeStatCode = tradeStatCode;
	}
	public String get_cdtyGrpCode() {
		return _cdtyGrpCode;
	}
	@XmlElement(name="CdtyGrpCode")
	public void set_cdtyGrpCode(String cdtyGroupCode) {
		this._cdtyGrpCode = cdtyGroupCode;
	}
	
	public String get_brokerPrice() {
		return _brokerPrice;
	}
	@XmlElement(name="BrokerPrice")
	public void set_brokerPrice(String brokerPrice) {
		this._brokerPrice = brokerPrice;
	}
	
	public String get_optnStrikePrice() {
		return _optnStrikePrice;
	}
	
	@XmlElement(name="OptnStrikePrice")
	public void set_optnStrikePrice(String optionStrikePrice) {
		this._optnStrikePrice = optionStrikePrice;
	}
	public String get_optnPremPrice() {
		return _optnPremPrice;
	}
	@XmlElement(name="OptnPremPrice")
	public void set_optnPremPrice(String optionPremPrice) {
		this._optnPremPrice = optionPremPrice;
	}
	public String get_optnPutCallInd() {
		return _optnPutCallInd;
	}
	@XmlElement(name="OptnPutCallInd")
	public void set_optnPutCallInd(String optionPutCallIndicator) {
		this._optnPutCallInd = optionPutCallIndicator;
	}
	public String get_priority() {
		return _priority;
	}
	@XmlElement(name="Priority")
	public void set_priority(String priority) {
		this._priority = priority;
	}
	public String get_plAmt() {
		return _plAmt;
	}
	
	@XmlElement(name="PlAmt")
	public void set_plAmt(String plAmt) {
		this._plAmt = plAmt;
	}
	public String get_efsFlag() {
		return _efsFlag;
	}
	
	@XmlElement(name="EfsFlag")
	public void set_efsFlag(String efsFlag) {
		this._efsFlag = efsFlag;
	}
	public String get_efsCptySn() {
		return _efsCptySn;
	}
	@XmlElement(name="EfsCptySn")
	public void set_efsCptySn(String efsCptySn) {
		this._efsCptySn = efsCptySn;
	}
	
	@XmlElement(name="ArchiveFlag")
	public String getArchiveFlag() {
		return archiveFlag;
	}
	
	public void setArchiveFlag(String archiveFlag) {
		this.archiveFlag = archiveFlag;
	}
	@XmlElement(name="GroupXref")
	public String getGroupXRef() {
		return groupXRef;
	}
	
	public void setGroupXRef(String groupXRef) {
		this.groupXRef = groupXRef;
	}
	
	@XmlElement(name="CptyLn")
	public String get_cptyLn() {
		return _cptyLn;
	}
	public void set_cptyLn(String ln) {
		_cptyLn = ln;
	}
	@XmlElement(name="ReadyForReplyFlag")
	public String get_readyForReplyFlag() {
		return _readyForReplyFlag;
	}
	public void set_readyForReplyFlag(String forReplyFlag) {
		_readyForReplyFlag = forReplyFlag;
	}
	public String get_migrateInd() {
		return _migrateInd;
	}
	@XmlElement(name="MigrateInd")
	public void set_migrateInd(String ind) {
		_migrateInd = ind;
	}
	
	
	public String get_analystName() {
		return _analystName;
	}
	@XmlElement(name="AnalystName")
	public void set_analystName(String name) {
		_analystName = name;
	}
	
	public String get_isTestBook() {
		return _isTestBook;
	}
	@XmlElement(name="IsTestBook")
	public void set_isTestBook(String testBook) {
		_isTestBook = testBook;
	}
	
	public String get_additionalConfirmSent() {
		return _additionalConfirmSent;
	}
	@XmlElement(name="AdditionalConfirmSent")
	public void set_additionalConfirmSent(String confirmSent) {
		_additionalConfirmSent = confirmSent;
	}
	

}
