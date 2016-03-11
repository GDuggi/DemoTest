package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Date;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Sep 19, 2012
 * Time: 1:03:19 PM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "V_PC_TRADE_SUMMARY")
public class VPcTradeSummaryEntity implements Serializable {
    private String trdSysCode;

    @Basic
    @Column(name = "TRD_SYS_CODE", nullable = false, length = 5)
    public String getTrdSysCode() {
        return trdSysCode;
    }

    public void setTrdSysCode(String trdSysCode) {
        this.trdSysCode = trdSysCode;
    }

    private Long version;

    @Basic
    @Column(name = "VERSION", length = 0, precision = -127)
    public Long getVersion() {
        return version;
    }

    public void setVersion(Long version) {
        this.version = version;
    }

    private Timestamp currentBusnDt;

    @Basic
    @Column(name = "CURRENT_BUSN_DT", length = 7)
    public Timestamp getCurrentBusnDt() {
        return currentBusnDt;
    }

    public void setCurrentBusnDt(Timestamp currentBusnDt) {
        this.currentBusnDt = currentBusnDt;
    }

    private Long recentInd;

    @Basic
    @Column(name = "RECENT_IND", length = 0, precision = -127)
    public Long getRecentInd() {
        return recentInd;
    }

    public void setRecentInd(Long recentInd) {
        this.recentInd = recentInd;
    }

    private String cmt;

    @Basic
    @Column(name = "CMT")
    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }

    private String cptyTradeId;

    @Basic
    @Column(name = "CPTY_TRADE_ID")
    public String getCptyTradeId() {
        return cptyTradeId;
    }

    public void setCptyTradeId(String cptyTradeId) {
        this.cptyTradeId = cptyTradeId;
    }

    private Timestamp lastUpdateTimestampGmt;

    @Basic
    @Column(name = "LAST_UPDATE_TIMESTAMP_GMT", nullable = false, length = 7)
    public Timestamp getLastUpdateTimestampGmt() {
        return lastUpdateTimestampGmt;
    }

    public void setLastUpdateTimestampGmt(Timestamp lastUpdateTimestampGmt) {
        this.lastUpdateTimestampGmt = lastUpdateTimestampGmt;
    }

    private Timestamp lastTrdEditTimestampGmt;

    @Basic
    @Column(name = "LAST_TRD_EDIT_TIMESTAMP_GMT", nullable = false, length = 7)
    public Timestamp getLastTrdEditTimestampGmt() {
        return lastTrdEditTimestampGmt;
    }

    public void setLastTrdEditTimestampGmt(Timestamp lastTrdEditTimestampGmt) {
        this.lastTrdEditTimestampGmt = lastTrdEditTimestampGmt;
    }

    private String readyForFinalApprovalFlag;

    @Basic
    @Column(name = "READY_FOR_FINAL_APPROVAL_FLAG", length = 1)
    public String getReadyForFinalApprovalFlag() {
        return readyForFinalApprovalFlag;
    }

    public void setReadyForFinalApprovalFlag(String readyForFinalApprovalFlag) {
        this.readyForFinalApprovalFlag = readyForFinalApprovalFlag;
    }

    private String hasProblemFlag;

    @Basic
    @Column(name = "HAS_PROBLEM_FLAG", nullable = false, length = 1)
    public String getHasProblemFlag() {
        return hasProblemFlag;
    }

    public void setHasProblemFlag(String hasProblemFlag) {
        this.hasProblemFlag = hasProblemFlag;
    }

    private String finalApprovalFlag;

    @Basic
    @Column(name = "FINAL_APPROVAL_FLAG", nullable = false, length = 1)
    public String getFinalApprovalFlag() {
        return finalApprovalFlag;
    }

    public void setFinalApprovalFlag(String finalApprovalFlag) {
        this.finalApprovalFlag = finalApprovalFlag;
    }

    private Timestamp finalApprovalTimestampGmt;

    @Basic
    @Column(name = "FINAL_APPROVAL_TIMESTAMP_GMT", length = 7)
    public Timestamp getFinalApprovalTimestampGmt() {
        return finalApprovalTimestampGmt;
    }

    public void setFinalApprovalTimestampGmt(Timestamp finalApprovalTimestampGmt) {
        this.finalApprovalTimestampGmt = finalApprovalTimestampGmt;
    }

    private String opsDetActFlag;

    @Basic
    @Column(name = "OPS_DET_ACT_FLAG", length = 1)
    public String getOpsDetActFlag() {
        return opsDetActFlag;
    }

    public void setOpsDetActFlag(String opsDetActFlag) {
        this.opsDetActFlag = opsDetActFlag;
    }

    private Long transactionSeq;

    @Basic
    @Column(name = "TRANSACTION_SEQ", length = 12)
    public Long getTransactionSeq() {
        return transactionSeq;
    }

    public void setTransactionSeq(Long transactionSeq) {
        this.transactionSeq = transactionSeq;
    }

    private String bkrRqmt;

    @Basic
    @Column(name = "BKR_RQMT", length = 10)
    public String getBkrRqmt() {
        return bkrRqmt;
    }

    public void setBkrRqmt(String bkrRqmt) {
        this.bkrRqmt = bkrRqmt;
    }

    private String bkrMeth;

    @Basic
    @Column(name = "BKR_METH", length = 15)
    public String getBkrMeth() {
        return bkrMeth;
    }

    public void setBkrMeth(String bkrMeth) {
        this.bkrMeth = bkrMeth;
    }

    private String bkrStatus;

    @Basic
    @Column(name = "BKR_STATUS", length = 10)
    public String getBkrStatus() {
        return bkrStatus;
    }

    public void setBkrStatus(String bkrStatus) {
        this.bkrStatus = bkrStatus;
    }

    private Long bkrDbUpd;

    @Basic
    @Column(name = "BKR_DB_UPD", length = 12)
    public Long getBkrDbUpd() {
        return bkrDbUpd;
    }

    public void setBkrDbUpd(Long bkrDbUpd) {
        this.bkrDbUpd = bkrDbUpd;
    }

    private String setcRqmt;

    @Basic
    @Column(name = "SETC_RQMT", length = 10)
    public String getSetcRqmt() {
        return setcRqmt;
    }

    public void setSetcRqmt(String setcRqmt) {
        this.setcRqmt = setcRqmt;
    }

    private String setcMeth;

    @Basic
    @Column(name = "SETC_METH", length = 13)
    public String getSetcMeth() {
        return setcMeth;
    }

    public void setSetcMeth(String setcMeth) {
        this.setcMeth = setcMeth;
    }

    private String setcStatus;

    @Basic
    @Column(name = "SETC_STATUS", length = 10)
    public String getSetcStatus() {
        return setcStatus;
    }

    public void setSetcStatus(String setcStatus) {
        this.setcStatus = setcStatus;
    }

    private Long setcDbUpd;

    @Basic
    @Column(name = "SETC_DB_UPD", length = 12)
    public Long getSetcDbUpd() {
        return setcDbUpd;
    }

    public void setSetcDbUpd(Long setcDbUpd) {
        this.setcDbUpd = setcDbUpd;
    }

    private String cptyRqmt;

    @Basic
    @Column(name = "CPTY_RQMT", length = 10)
    public String getCptyRqmt() {
        return cptyRqmt;
    }

    public void setCptyRqmt(String cptyRqmt) {
        this.cptyRqmt = cptyRqmt;
    }

    private String cptyMeth;

    @Basic
    @Column(name = "CPTY_METH", length = 13)
    public String getCptyMeth() {
        return cptyMeth;
    }

    public void setCptyMeth(String cptyMeth) {
        this.cptyMeth = cptyMeth;
    }

    private String cptyStatus;

    @Basic
    @Column(name = "CPTY_STATUS", length = 10)
    public String getCptyStatus() {
        return cptyStatus;
    }

    public void setCptyStatus(String cptyStatus) {
        this.cptyStatus = cptyStatus;
    }

    private Long cptyDbUpd;

    @Basic
    @Column(name = "CPTY_DB_UPD", length = 12)
    public Long getCptyDbUpd() {
        return cptyDbUpd;
    }

    public void setCptyDbUpd(Long cptyDbUpd) {
        this.cptyDbUpd = cptyDbUpd;
    }

    private String noconfRqmt;

    @Basic
    @Column(name = "NOCONF_RQMT", length = 10)
    public String getNoconfRqmt() {
        return noconfRqmt;
    }

    public void setNoconfRqmt(String noconfRqmt) {
        this.noconfRqmt = noconfRqmt;
    }

    private String noconfMeth;

    @Basic
    @Column(name = "NOCONF_METH", length = 30)
    public String getNoconfMeth() {
        return noconfMeth;
    }

    public void setNoconfMeth(String noconfMeth) {
        this.noconfMeth = noconfMeth;
    }

    private String noconfStatus;

    @Basic
    @Column(name = "NOCONF_STATUS", length = 10)
    public String getNoconfStatus() {
        return noconfStatus;
    }

    public void setNoconfStatus(String noconfStatus) {
        this.noconfStatus = noconfStatus;
    }

    private Long noconfDbUpd;

    @Basic
    @Column(name = "NOCONF_DB_UPD", length = 12)
    public Long getNoconfDbUpd() {
        return noconfDbUpd;
    }

    public void setNoconfDbUpd(Long noconfDbUpd) {
        this.noconfDbUpd = noconfDbUpd;
    }

    private String verblRqmt;

    @Basic
    @Column(name = "VERBL_RQMT", length = 10)
    public String getVerblRqmt() {
        return verblRqmt;
    }

    public void setVerblRqmt(String verblRqmt) {
        this.verblRqmt = verblRqmt;
    }

    private String verblMeth;

    @Basic
    @Column(name = "VERBL_METH", length = 5)
    public String getVerblMeth() {
        return verblMeth;
    }

    public void setVerblMeth(String verblMeth) {
        this.verblMeth = verblMeth;
    }

    private String verblStatus;

    @Basic
    @Column(name = "VERBL_STATUS", length = 10)
    public String getVerblStatus() {
        return verblStatus;
    }

    public void setVerblStatus(String verblStatus) {
        this.verblStatus = verblStatus;
    }

    private Long verblDbUpd;

    @Basic
    @Column(name = "VERBL_DB_UPD", length = 12)
    public Long getVerblDbUpd() {
        return verblDbUpd;
    }

    public void setVerblDbUpd(Long verblDbUpd) {
        this.verblDbUpd = verblDbUpd;
    }

    private String groupXref;

    @Basic
    @Column(name = "GROUP_XREF", length = 20)
    public String getGroupXref() {
        return groupXref;
    }

    public void setGroupXref(String groupXref) {
        this.groupXref = groupXref;
    }

    private Long id;

    @Basic
    @Column(name = "ID", nullable = false, length = 12)
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private Long tradeId;

    @Id
    @Column(name = "TRADE_ID", nullable = false, length = 12)
    public Long getTradeId() {
        return tradeId;
    }

    public void setTradeId(Long tradeId) {
        this.tradeId = tradeId;
    }

    private Date inceptionDt;

    @Basic
    @Column(name = "INCEPTION_DT", length = 7)
    public Date getInceptionDt() {
        return inceptionDt;
    }

    public void setInceptionDt(Date inceptionDt) {
        this.inceptionDt = inceptionDt;
    }

    private String cdtyCode;

    @Basic
    @Column(name = "CDTY_CODE", length = 10)
    public String getCdtyCode() {
        return cdtyCode;
    }

    public void setCdtyCode(String cdtyCode) {
        this.cdtyCode = cdtyCode;
    }

    private Date tradeDt;

    @Basic
    @Column(name = "TRADE_DT", length = 7)
    public Date getTradeDt() {
        return tradeDt;
    }

    public void setTradeDt(Date tradeDt) {
        this.tradeDt = tradeDt;
    }

    private String xref;

    @Basic
    @Column(name = "XREF", length = 20)
    public String getXref() {
        return xref;
    }

    public void setXref(String xref) {
        this.xref = xref;
    }

    private String cptySn;

    @Basic
    @Column(name = "CPTY_SN", length = 20)
    public String getCptySn() {
        return cptySn;
    }

    public void setCptySn(String cptySn) {
        this.cptySn = cptySn;
    }

    private Long qtyTot;

    @Basic
    @Column(name = "QTY_TOT", length = 24, precision = 12)
    public Long getQtyTot() {
        return qtyTot;
    }

    public void setQtyTot(Long qtyTot) {
        this.qtyTot = qtyTot;
    }

    private String locationSn;

    @Basic
    @Column(name = "LOCATION_SN", length = 20)
    public String getLocationSn() {
        return locationSn;
    }

    public void setLocationSn(String locationSn) {
        this.locationSn = locationSn;
    }

    private String priceDesc;

    @Basic
    @Column(name = "PRICE_DESC", length = 50)
    public String getPriceDesc() {
        return priceDesc;
    }

    public void setPriceDesc(String priceDesc) {
        this.priceDesc = priceDesc;
    }

    private Date startDt;

    @Basic
    @Column(name = "START_DT", length = 7)
    public Date getStartDt() {
        return startDt;
    }

    public void setStartDt(Date startDt) {
        this.startDt = startDt;
    }

    private Date endDt;

    @Basic
    @Column(name = "END_DT", length = 7)
    public Date getEndDt() {
        return endDt;
    }

    public void setEndDt(Date endDt) {
        this.endDt = endDt;
    }

    private String book;

    @Basic
    @Column(name = "BOOK", length = 20)
    public String getBook() {
        return book;
    }

    public void setBook(String book) {
        this.book = book;
    }

    private String tradeTypeCode;

    @Basic
    @Column(name = "TRADE_TYPE_CODE", length = 10)
    public String getTradeTypeCode() {
        return tradeTypeCode;
    }

    public void setTradeTypeCode(String tradeTypeCode) {
        this.tradeTypeCode = tradeTypeCode;
    }

    private String sttlType;

    @Basic
    @Column(name = "STTL_TYPE", length = 10)
    public String getSttlType() {
        return sttlType;
    }

    public void setSttlType(String sttlType) {
        this.sttlType = sttlType;
    }

    private String brokerSn;

    @Basic
    @Column(name = "BROKER_SN", length = 20)
    public String getBrokerSn() {
        return brokerSn;
    }

    public void setBrokerSn(String brokerSn) {
        this.brokerSn = brokerSn;
    }

    private String buySellInd;

    @Basic
    @Column(name = "BUY_SELL_IND", length = 1)
    public String getBuySellInd() {
        return buySellInd;
    }

    public void setBuySellInd(String buySellInd) {
        this.buySellInd = buySellInd;
    }

    private String refSn;

    @Basic
    @Column(name = "REF_SN", length = 20)
    public String getRefSn() {
        return refSn;
    }

    public void setRefSn(String refSn) {
        this.refSn = refSn;
    }


    private String seCptySn;

    @Basic
    @Column(name = "SE_CPTY_SN", length = 10)
    public String getSeCptySn() {
        return seCptySn;
    }

    public void setSeCptySn(String seCptySn) {
        this.seCptySn = seCptySn;
    }

    private String tradeStatCode;

    @Basic
    @Column(name = "TRADE_STAT_CODE", length = 10)
    public String getTradeStatCode() {
        return tradeStatCode;
    }

    public void setTradeStatCode(String tradeStatCode) {
        this.tradeStatCode = tradeStatCode;
    }

    private String brokerPrice;

    @Basic
    @Column(name = "BROKER_PRICE", length = 60)
    public String getBrokerPrice() {
        return brokerPrice;
    }

    public void setBrokerPrice(String brokerPrice) {
        this.brokerPrice = brokerPrice;
    }

    private String optnStrikePrice;

    @Basic
    @Column(name = "OPTN_STRIKE_PRICE", length = 60)
    public String getOptnStrikePrice() {
        return optnStrikePrice;
    }

    public void setOptnStrikePrice(String optnStrikePrice) {
        this.optnStrikePrice = optnStrikePrice;
    }

    private String optnPremPrice;

    @Basic
    @Column(name = "OPTN_PREM_PRICE", length = 60)
    public String getOptnPremPrice() {
        return optnPremPrice;
    }

    public void setOptnPremPrice(String optnPremPrice) {
        this.optnPremPrice = optnPremPrice;
    }

    private String optnPutCallInd;

    @Basic
    @Column(name = "OPTN_PUT_CALL_IND", length = 1)
    public String getOptnPutCallInd() {
        return optnPutCallInd;
    }

    public void setOptnPutCallInd(String optnPutCallInd) {
        this.optnPutCallInd = optnPutCallInd;
    }

    private String priority;

    @Basic
    @Column(name = "PRIORITY", length = 40)
    public String getPriority() {
        return priority;
    }

    public void setPriority(String priority) {
        this.priority = priority;
    }

    private String plAmt;

    @Basic
    @Column(name = "PL_AMT", length = 20)
    public String getPlAmt() {
        return plAmt;
    }

    public void setPlAmt(String plAmt) {
        this.plAmt = plAmt;
    }

    private String archiveFlag;

    @Basic
    @Column(name = "ARCHIVE_FLAG", length = 1)
    public String getArchiveFlag() {
        return archiveFlag;
    }

    public void setArchiveFlag(String archiveFlag) {
        this.archiveFlag = archiveFlag;
    }

    private String rplyRdyToSndFlag;

    @Basic
    @Column(name = "RPLY_RDY_TO_SND_FLAG", length = 1)
    public String getRplyRdyToSndFlag() {
        return rplyRdyToSndFlag;
    }

    public void setRplyRdyToSndFlag(String rplyRdyToSndFlag) {
        this.rplyRdyToSndFlag = rplyRdyToSndFlag;
    }

    private String migrateInd;

    @Basic
    @Column(name = "MIGRATE_IND", length = 1)
    public String getMigrateInd() {
        return migrateInd;
    }

    public void setMigrateInd(String migrateInd) {
        this.migrateInd = migrateInd;
    }

    private String analystName;

    @Basic
    @Column(name = "ANALYST_NAME", length = 4000)
    public String getAnalystName() {
        return analystName;
    }

    public void setAnalystName(String analystName) {
        this.analystName = analystName;
    }

    private String additionalConfirmSent;

    @Basic
    @Column(name = "ADDITIONAL_CONFIRM_SENT", length = 10)
    public String getAdditionalConfirmSent() {
        return additionalConfirmSent;
    }

    public void setAdditionalConfirmSent(String additionalConfirmSent) {
        this.additionalConfirmSent = additionalConfirmSent;
    }

    private String tradeSystemTicket;

    @Basic
    @Column(name = "TRD_SYS_TICKET", length = 50)
    public String getTradeSystemTicket()
    {
        return tradeSystemTicket;
    }

    public void setTradeSystemTicket(String tradeSystemTicket)
    {
        this.tradeSystemTicket = tradeSystemTicket;
    }

    private String tradeDescription;

    @Basic
    @Column(name = "TRADE_DESC", length = 200)
    public String getTradeDescription()
    {
        return tradeDescription;
    }

    public void setTradeDescription(String tradeDescription)
    {
        this.tradeDescription = tradeDescription;
    }

    private String quantityDescription;

    @Basic
    @Column(name = "QTY_DESC", length = 200)
    public String getQuantityDescription()
    {
        return quantityDescription;
    }

    public void setQuantityDescription(String quantityDescription)
    {
        this.quantityDescription = quantityDescription;
    }

    private String isTestBook;

    @Basic
    @Column(name = "IS_TEST_BOOK", length = 1)
    public String getTestBook() {
        return isTestBook;
    }

    public void setTestBook(String testBook) {
        isTestBook = testBook;
    }

    private String bookingCompanyShortName;
    @Basic
    @Column(name = "BOOKING_CO_SN", length = 10)
    public String getBookingCompanyShortName()
    {
        return bookingCompanyShortName;
    }

    public void setBookingCompanyShortName(String bookingCompanyShortName)
    {
        this.bookingCompanyShortName = bookingCompanyShortName;
    }

    private Integer bookingCompanyId;

    @Basic
    @Column(name = "booking_co_id")
    public Integer getBookingCompanyId()
    {
        return bookingCompanyId;
    }

    public void setBookingCompanyId(Integer bookingCompanyId)
    {
        this.bookingCompanyId = bookingCompanyId;
    }

    private Integer counterPartyId;

    @Basic
    @Column(name = "CPTY_ID")
    public Integer getCounterPartyId()
    {
        return counterPartyId;
    }

    public void setCounterPartyId(Integer counterPartyId)
    {
        this.counterPartyId = counterPartyId;
    }

    private String brokerLegalName;

    @Basic
    @Column(name = "BROKER_LEGAL_NAME", length = 255)
    public String getBrokerLegalName()
    {
        return brokerLegalName;
    }

    public void setBrokerLegalName(String brokerLegalName)
    {
        this.brokerLegalName = brokerLegalName;
    }

    private Integer brokerId;

    @Basic
    @Column(name = "BROKER_ID")
    public Integer getBrokerId()
    {
        return brokerId;
    }

    public void setBrokerId(Integer brokerId)
    {
        this.brokerId = brokerId;
    }

    private String counterPartyLegalName;

    @Basic
    @Column(name = "CPTY_LEGAL_NAME", length = 255)
    public String getCounterPartyLegalName()
    {
        return counterPartyLegalName;
    }

    public void setCounterPartyLegalName(String counterPartyLegalName)
    {
        this.counterPartyLegalName = counterPartyLegalName;
    }

    private String trader;

    @Basic
    @Column(name = "TRADER", length = 100)
    public String getTrader()
    {
        return trader;
    }

    public void setTrader(String trader)
    {
        this.trader = trader;
    }

    private String transportDecription;

    @Basic
    @Column(name = "TRANSPORT_DESC", length = 200)
    public String getTransportDecription()
    {
        return transportDecription;
    }

    public void setTransportDecription(String transportDecription)
    {
        this.transportDecription = transportDecription;
    }

    private String commodityGroupCode;

    @Basic
    @Column(name = "CDTY_GRP_CODE", length = 5)
    public String getCommodityGroupCode()
    {
        return commodityGroupCode;
    }

    private String permissionKey;

    @Basic
    @Column(name = "PERMISSION_KEY", length = 20)
    public String getPermissionKey()
    {
        return permissionKey;
    }

    public void setPermissionKey(String permissionKey)
    {
        this.permissionKey = permissionKey;
    }

    public void setCommodityGroupCode(String commodityGroupCode)
    {
        this.commodityGroupCode = commodityGroupCode;
    }

    @Override
    public boolean equals(Object o)
    {
        if (this == o)
        {
            return true;
        }
        if (o == null || getClass() != o.getClass())
        {
            return false;
        }

        VPcTradeSummaryEntity that = (VPcTradeSummaryEntity) o;

        if (trdSysCode != null ? !trdSysCode.equals(that.trdSysCode) : that.trdSysCode != null)
        {
            return false;
        }
        if (version != null ? !version.equals(that.version) : that.version != null)
        {
            return false;
        }
        if (currentBusnDt != null ? !currentBusnDt.equals(that.currentBusnDt) : that.currentBusnDt != null)
        {
            return false;
        }
        if (recentInd != null ? !recentInd.equals(that.recentInd) : that.recentInd != null)
        {
            return false;
        }
        if (cmt != null ? !cmt.equals(that.cmt) : that.cmt != null)
        {
            return false;
        }
        if (cptyTradeId != null ? !cptyTradeId.equals(that.cptyTradeId) : that.cptyTradeId != null)
        {
            return false;
        }
        if (lastUpdateTimestampGmt != null ? !lastUpdateTimestampGmt.equals(that.lastUpdateTimestampGmt) :
                that.lastUpdateTimestampGmt != null)
        {
            return false;
        }
        if (lastTrdEditTimestampGmt != null ? !lastTrdEditTimestampGmt.equals(that.lastTrdEditTimestampGmt) :
                that.lastTrdEditTimestampGmt != null)
        {
            return false;
        }
        if (readyForFinalApprovalFlag != null ? !readyForFinalApprovalFlag.equals(that.readyForFinalApprovalFlag) :
                that.readyForFinalApprovalFlag != null)
        {
            return false;
        }
        if (hasProblemFlag != null ? !hasProblemFlag.equals(that.hasProblemFlag) : that.hasProblemFlag != null)
        {
            return false;
        }
        if (finalApprovalFlag != null ? !finalApprovalFlag.equals(that.finalApprovalFlag) :
                that.finalApprovalFlag != null)
        {
            return false;
        }
        if (finalApprovalTimestampGmt != null ? !finalApprovalTimestampGmt.equals(that.finalApprovalTimestampGmt) :
                that.finalApprovalTimestampGmt != null)
        {
            return false;
        }
        if (opsDetActFlag != null ? !opsDetActFlag.equals(that.opsDetActFlag) : that.opsDetActFlag != null)
        {
            return false;
        }
        if (transactionSeq != null ? !transactionSeq.equals(that.transactionSeq) : that.transactionSeq != null)
        {
            return false;
        }
        if (bkrRqmt != null ? !bkrRqmt.equals(that.bkrRqmt) : that.bkrRqmt != null)
        {
            return false;
        }
        if (bkrMeth != null ? !bkrMeth.equals(that.bkrMeth) : that.bkrMeth != null)
        {
            return false;
        }
        if (bkrStatus != null ? !bkrStatus.equals(that.bkrStatus) : that.bkrStatus != null)
        {
            return false;
        }
        if (bkrDbUpd != null ? !bkrDbUpd.equals(that.bkrDbUpd) : that.bkrDbUpd != null)
        {
            return false;
        }
        if (setcRqmt != null ? !setcRqmt.equals(that.setcRqmt) : that.setcRqmt != null)
        {
            return false;
        }
        if (setcMeth != null ? !setcMeth.equals(that.setcMeth) : that.setcMeth != null)
        {
            return false;
        }
        if (setcStatus != null ? !setcStatus.equals(that.setcStatus) : that.setcStatus != null)
        {
            return false;
        }
        if (setcDbUpd != null ? !setcDbUpd.equals(that.setcDbUpd) : that.setcDbUpd != null)
        {
            return false;
        }
        if (cptyRqmt != null ? !cptyRqmt.equals(that.cptyRqmt) : that.cptyRqmt != null)
        {
            return false;
        }
        if (cptyMeth != null ? !cptyMeth.equals(that.cptyMeth) : that.cptyMeth != null)
        {
            return false;
        }
        if (cptyStatus != null ? !cptyStatus.equals(that.cptyStatus) : that.cptyStatus != null)
        {
            return false;
        }
        if (cptyDbUpd != null ? !cptyDbUpd.equals(that.cptyDbUpd) : that.cptyDbUpd != null)
        {
            return false;
        }
        if (noconfRqmt != null ? !noconfRqmt.equals(that.noconfRqmt) : that.noconfRqmt != null)
        {
            return false;
        }
        if (noconfMeth != null ? !noconfMeth.equals(that.noconfMeth) : that.noconfMeth != null)
        {
            return false;
        }
        if (noconfStatus != null ? !noconfStatus.equals(that.noconfStatus) : that.noconfStatus != null)
        {
            return false;
        }
        if (noconfDbUpd != null ? !noconfDbUpd.equals(that.noconfDbUpd) : that.noconfDbUpd != null)
        {
            return false;
        }
        if (verblRqmt != null ? !verblRqmt.equals(that.verblRqmt) : that.verblRqmt != null)
        {
            return false;
        }
        if (verblMeth != null ? !verblMeth.equals(that.verblMeth) : that.verblMeth != null)
        {
            return false;
        }
        if (verblStatus != null ? !verblStatus.equals(that.verblStatus) : that.verblStatus != null)
        {
            return false;
        }
        if (verblDbUpd != null ? !verblDbUpd.equals(that.verblDbUpd) : that.verblDbUpd != null)
        {
            return false;
        }
        if (groupXref != null ? !groupXref.equals(that.groupXref) : that.groupXref != null)
        {
            return false;
        }
        if (id != null ? !id.equals(that.id) : that.id != null)
        {
            return false;
        }
        if (tradeId != null ? !tradeId.equals(that.tradeId) : that.tradeId != null)
        {
            return false;
        }
        if (inceptionDt != null ? !inceptionDt.equals(that.inceptionDt) : that.inceptionDt != null)
        {
            return false;
        }
        if (cdtyCode != null ? !cdtyCode.equals(that.cdtyCode) : that.cdtyCode != null)
        {
            return false;
        }
        if (tradeDt != null ? !tradeDt.equals(that.tradeDt) : that.tradeDt != null)
        {
            return false;
        }
        if (xref != null ? !xref.equals(that.xref) : that.xref != null)
        {
            return false;
        }
        if (cptySn != null ? !cptySn.equals(that.cptySn) : that.cptySn != null)
        {
            return false;
        }
        if (qtyTot != null ? !qtyTot.equals(that.qtyTot) : that.qtyTot != null)
        {
            return false;
        }
        if (locationSn != null ? !locationSn.equals(that.locationSn) : that.locationSn != null)
        {
            return false;
        }
        if (priceDesc != null ? !priceDesc.equals(that.priceDesc) : that.priceDesc != null)
        {
            return false;
        }
        if (startDt != null ? !startDt.equals(that.startDt) : that.startDt != null)
        {
            return false;
        }
        if (endDt != null ? !endDt.equals(that.endDt) : that.endDt != null)
        {
            return false;
        }
        if (book != null ? !book.equals(that.book) : that.book != null)
        {
            return false;
        }
        if (tradeTypeCode != null ? !tradeTypeCode.equals(that.tradeTypeCode) : that.tradeTypeCode != null)
        {
            return false;
        }
        if (sttlType != null ? !sttlType.equals(that.sttlType) : that.sttlType != null)
        {
            return false;
        }
        if (brokerSn != null ? !brokerSn.equals(that.brokerSn) : that.brokerSn != null)
        {
            return false;
        }
        if (buySellInd != null ? !buySellInd.equals(that.buySellInd) : that.buySellInd != null)
        {
            return false;
        }
        if (refSn != null ? !refSn.equals(that.refSn) : that.refSn != null)
        {
            return false;
        }
        if (seCptySn != null ? !seCptySn.equals(that.seCptySn) : that.seCptySn != null)
        {
            return false;
        }
        if (tradeStatCode != null ? !tradeStatCode.equals(that.tradeStatCode) : that.tradeStatCode != null)
        {
            return false;
        }
        if (brokerPrice != null ? !brokerPrice.equals(that.brokerPrice) : that.brokerPrice != null)
        {
            return false;
        }
        if (optnStrikePrice != null ? !optnStrikePrice.equals(that.optnStrikePrice) : that.optnStrikePrice != null)
        {
            return false;
        }
        if (optnPremPrice != null ? !optnPremPrice.equals(that.optnPremPrice) : that.optnPremPrice != null)
        {
            return false;
        }
        if (optnPutCallInd != null ? !optnPutCallInd.equals(that.optnPutCallInd) : that.optnPutCallInd != null)
        {
            return false;
        }
        if (priority != null ? !priority.equals(that.priority) : that.priority != null)
        {
            return false;
        }
        if (plAmt != null ? !plAmt.equals(that.plAmt) : that.plAmt != null)
        {
            return false;
        }
        if (archiveFlag != null ? !archiveFlag.equals(that.archiveFlag) : that.archiveFlag != null)
        {
            return false;
        }
        if (rplyRdyToSndFlag != null ? !rplyRdyToSndFlag.equals(that.rplyRdyToSndFlag) : that.rplyRdyToSndFlag != null)
        {
            return false;
        }
        if (migrateInd != null ? !migrateInd.equals(that.migrateInd) : that.migrateInd != null)
        {
            return false;
        }
        if (analystName != null ? !analystName.equals(that.analystName) : that.analystName != null)
        {
            return false;
        }
        if (additionalConfirmSent != null ? !additionalConfirmSent.equals(that.additionalConfirmSent) :
                that.additionalConfirmSent != null)
        {
            return false;
        }
        if (tradeSystemTicket != null ? !tradeSystemTicket.equals(that.tradeSystemTicket) :
                that.tradeSystemTicket != null)
        {
            return false;
        }
        if (tradeDescription != null ? !tradeDescription.equals(that.tradeDescription) : that.tradeDescription != null)
        {
            return false;
        }
        if (quantityDescription != null ? !quantityDescription.equals(that.quantityDescription) :
                that.quantityDescription != null)
        {
            return false;
        }
        if (isTestBook != null ? !isTestBook.equals(that.isTestBook) : that.isTestBook != null)
        {
            return false;
        }
        if (bookingCompanyShortName != null ? !bookingCompanyShortName.equals(that.bookingCompanyShortName) :
                that.bookingCompanyShortName != null)
        {
            return false;
        }
        if (bookingCompanyId != null ? !bookingCompanyId.equals(that.bookingCompanyId) : that.bookingCompanyId != null)
        {
            return false;
        }
        if (counterPartyId != null ? !counterPartyId.equals(that.counterPartyId) : that.counterPartyId != null)
        {
            return false;
        }
        if (brokerLegalName != null ? !brokerLegalName.equals(that.brokerLegalName) : that.brokerLegalName != null)
        {
            return false;
        }
        if (brokerId != null ? !brokerId.equals(that.brokerId) : that.brokerId != null)
        {
            return false;
        }
        if (counterPartyLegalName != null ? !counterPartyLegalName.equals(that.counterPartyLegalName) :
                that.counterPartyLegalName != null)
        {
            return false;
        }
        if (trader != null ? !trader.equals(that.trader) : that.trader != null)
        {
            return false;
        }
        if (transportDecription != null ? !transportDecription.equals(that.transportDecription) :
                that.transportDecription != null)
        {
            return false;
        }
        return !(commodityGroupCode != null ? !commodityGroupCode.equals(that.commodityGroupCode) :
                that.commodityGroupCode != null);

    }

    @Override
    public int hashCode()
    {
        int result = trdSysCode != null ? trdSysCode.hashCode() : 0;
        result = 31 * result + (version != null ? version.hashCode() : 0);
        result = 31 * result + (currentBusnDt != null ? currentBusnDt.hashCode() : 0);
        result = 31 * result + (recentInd != null ? recentInd.hashCode() : 0);
        result = 31 * result + (cmt != null ? cmt.hashCode() : 0);
        result = 31 * result + (cptyTradeId != null ? cptyTradeId.hashCode() : 0);
        result = 31 * result + (lastUpdateTimestampGmt != null ? lastUpdateTimestampGmt.hashCode() : 0);
        result = 31 * result + (lastTrdEditTimestampGmt != null ? lastTrdEditTimestampGmt.hashCode() : 0);
        result = 31 * result + (readyForFinalApprovalFlag != null ? readyForFinalApprovalFlag.hashCode() : 0);
        result = 31 * result + (hasProblemFlag != null ? hasProblemFlag.hashCode() : 0);
        result = 31 * result + (finalApprovalFlag != null ? finalApprovalFlag.hashCode() : 0);
        result = 31 * result + (finalApprovalTimestampGmt != null ? finalApprovalTimestampGmt.hashCode() : 0);
        result = 31 * result + (opsDetActFlag != null ? opsDetActFlag.hashCode() : 0);
        result = 31 * result + (transactionSeq != null ? transactionSeq.hashCode() : 0);
        result = 31 * result + (bkrRqmt != null ? bkrRqmt.hashCode() : 0);
        result = 31 * result + (bkrMeth != null ? bkrMeth.hashCode() : 0);
        result = 31 * result + (bkrStatus != null ? bkrStatus.hashCode() : 0);
        result = 31 * result + (bkrDbUpd != null ? bkrDbUpd.hashCode() : 0);
        result = 31 * result + (setcRqmt != null ? setcRqmt.hashCode() : 0);
        result = 31 * result + (setcMeth != null ? setcMeth.hashCode() : 0);
        result = 31 * result + (setcStatus != null ? setcStatus.hashCode() : 0);
        result = 31 * result + (setcDbUpd != null ? setcDbUpd.hashCode() : 0);
        result = 31 * result + (cptyRqmt != null ? cptyRqmt.hashCode() : 0);
        result = 31 * result + (cptyMeth != null ? cptyMeth.hashCode() : 0);
        result = 31 * result + (cptyStatus != null ? cptyStatus.hashCode() : 0);
        result = 31 * result + (cptyDbUpd != null ? cptyDbUpd.hashCode() : 0);
        result = 31 * result + (noconfRqmt != null ? noconfRqmt.hashCode() : 0);
        result = 31 * result + (noconfMeth != null ? noconfMeth.hashCode() : 0);
        result = 31 * result + (noconfStatus != null ? noconfStatus.hashCode() : 0);
        result = 31 * result + (noconfDbUpd != null ? noconfDbUpd.hashCode() : 0);
        result = 31 * result + (verblRqmt != null ? verblRqmt.hashCode() : 0);
        result = 31 * result + (verblMeth != null ? verblMeth.hashCode() : 0);
        result = 31 * result + (verblStatus != null ? verblStatus.hashCode() : 0);
        result = 31 * result + (verblDbUpd != null ? verblDbUpd.hashCode() : 0);
        result = 31 * result + (groupXref != null ? groupXref.hashCode() : 0);
        result = 31 * result + (id != null ? id.hashCode() : 0);
        result = 31 * result + (tradeId != null ? tradeId.hashCode() : 0);
        result = 31 * result + (inceptionDt != null ? inceptionDt.hashCode() : 0);
        result = 31 * result + (cdtyCode != null ? cdtyCode.hashCode() : 0);
        result = 31 * result + (tradeDt != null ? tradeDt.hashCode() : 0);
        result = 31 * result + (xref != null ? xref.hashCode() : 0);
        result = 31 * result + (cptySn != null ? cptySn.hashCode() : 0);
        result = 31 * result + (qtyTot != null ? qtyTot.hashCode() : 0);
        result = 31 * result + (locationSn != null ? locationSn.hashCode() : 0);
        result = 31 * result + (priceDesc != null ? priceDesc.hashCode() : 0);
        result = 31 * result + (startDt != null ? startDt.hashCode() : 0);
        result = 31 * result + (endDt != null ? endDt.hashCode() : 0);
        result = 31 * result + (book != null ? book.hashCode() : 0);
        result = 31 * result + (tradeTypeCode != null ? tradeTypeCode.hashCode() : 0);
        result = 31 * result + (sttlType != null ? sttlType.hashCode() : 0);
        result = 31 * result + (brokerSn != null ? brokerSn.hashCode() : 0);
        result = 31 * result + (buySellInd != null ? buySellInd.hashCode() : 0);
        result = 31 * result + (refSn != null ? refSn.hashCode() : 0);
        result = 31 * result + (seCptySn != null ? seCptySn.hashCode() : 0);
        result = 31 * result + (tradeStatCode != null ? tradeStatCode.hashCode() : 0);
        result = 31 * result + (brokerPrice != null ? brokerPrice.hashCode() : 0);
        result = 31 * result + (optnStrikePrice != null ? optnStrikePrice.hashCode() : 0);
        result = 31 * result + (optnPremPrice != null ? optnPremPrice.hashCode() : 0);
        result = 31 * result + (optnPutCallInd != null ? optnPutCallInd.hashCode() : 0);
        result = 31 * result + (priority != null ? priority.hashCode() : 0);
        result = 31 * result + (plAmt != null ? plAmt.hashCode() : 0);
        result = 31 * result + (archiveFlag != null ? archiveFlag.hashCode() : 0);
        result = 31 * result + (rplyRdyToSndFlag != null ? rplyRdyToSndFlag.hashCode() : 0);
        result = 31 * result + (migrateInd != null ? migrateInd.hashCode() : 0);
        result = 31 * result + (analystName != null ? analystName.hashCode() : 0);
        result = 31 * result + (additionalConfirmSent != null ? additionalConfirmSent.hashCode() : 0);
        result = 31 * result + (tradeSystemTicket != null ? tradeSystemTicket.hashCode() : 0);
        result = 31 * result + (tradeDescription != null ? tradeDescription.hashCode() : 0);
        result = 31 * result + (quantityDescription != null ? quantityDescription.hashCode() : 0);
        result = 31 * result + (isTestBook != null ? isTestBook.hashCode() : 0);
        result = 31 * result + (bookingCompanyShortName != null ? bookingCompanyShortName.hashCode() : 0);
        result = 31 * result + (bookingCompanyId != null ? bookingCompanyId.hashCode() : 0);
        result = 31 * result + (counterPartyId != null ? counterPartyId.hashCode() : 0);
        result = 31 * result + (brokerLegalName != null ? brokerLegalName.hashCode() : 0);
        result = 31 * result + (brokerId != null ? brokerId.hashCode() : 0);
        result = 31 * result + (counterPartyLegalName != null ? counterPartyLegalName.hashCode() : 0);
        result = 31 * result + (trader != null ? trader.hashCode() : 0);
        result = 31 * result + (transportDecription != null ? transportDecription.hashCode() : 0);
        result = 31 * result + (commodityGroupCode != null ? commodityGroupCode.hashCode() : 0);
        return result;
    }
}