package aff.confirm.opsmanager.opssubpub.opstrackingmodel;

import aff.confirm.common.daoinbound.inbound.model.VPcTradeSummaryEntity;

import java.sql.Timestamp;
import java.util.Calendar;
import java.util.Date;

/**
 * User: hblumenf
 * Date: 9/21/2015
 * Time: 11:56 AM
 * Copyright Amphora Inc. 2015
 */
public class EntityConverter
{
    private static ThreadLocal<Calendar> sCalendar =
            new ThreadLocal<Calendar>() {
                @Override
                protected Calendar initialValue()
                {
                    return Calendar.getInstance();
                }
            };

    public static SummaryData createSummaryData(VPcTradeSummaryEntity entity)
    {
        if (entity == null) return null;

        SummaryData summaryData = null;
        Calendar cal = sCalendar.get();
        summaryData = new SummaryData();

        summaryData.set_trdSysCode(entity.getTrdSysCode());
        summaryData.set_version(entity.getVersion());
        if(entity.getCurrentBusnDt() != null){
            summaryData.set_currentBusnDt(new java.util.Date(entity.getCurrentBusnDt().getTime()));
        }
        summaryData.set_recentInd(entity.getRecentInd());
        summaryData.set_cmt(entity.getCmt());
        summaryData.set_cptyTradeId(entity.getCptyTradeId());

        summaryData.set_lastUpdateTimestampGmt(convertTimestamp(entity.getLastUpdateTimestampGmt()));

        summaryData.set_lastTrdEditTimestampGmt(convertTimestamp(entity.getLastTrdEditTimestampGmt()));

        summaryData.set_readyForFinalApprovalFlag(entity.getReadyForFinalApprovalFlag());
        summaryData.set_hasProblemFlag(entity.getHasProblemFlag());

        summaryData.set_finalApprovalFlag(entity.getFinalApprovalFlag());

        summaryData.set_finalApprovalTimestampGmt(convertTimestamp(entity.getFinalApprovalTimestampGmt()));

        summaryData.set_opsDetActFlag(entity.getOpsDetActFlag());
        summaryData.set_transactionSeq(entity.getTransactionSeq());

        summaryData.set_bkrRqmt(entity.getBkrRqmt());
        summaryData.set_bkrMeth(entity.getBkrMeth());
        summaryData.set_bkrStatus(entity.getBkrStatus());
        if(entity.getBkrDbUpd() != null){
            summaryData.set_bkrDbUpd(entity.getBkrDbUpd());
        }

        summaryData.set_setcRqmt(entity.getSetcRqmt());
        summaryData.set_setcMeth(entity.getSetcMeth());
        summaryData.set_setcStatus(entity.getSetcStatus());
        if(entity.getSetcDbUpd() != null) {
            summaryData.set_setcDbUpd(entity.getSetcDbUpd());
        }

        summaryData.set_cptyRqmt(entity.getCptyRqmt());
        summaryData.set_cptyMeth(entity.getCptyMeth());
        summaryData.set_cptyStatus(entity.getCptyStatus());
        if(entity.getCptyDbUpd() != null){
            summaryData.set_cptyDbUpd(entity.getCptyDbUpd());
        }

        summaryData.set_noconfRqmt(entity.getNoconfRqmt());
        summaryData.set_noconfMeth(entity.getNoconfMeth());
        summaryData.set_noconfStatus(entity.getNoconfStatus());
        if(entity.getNoconfDbUpd() != null){
            summaryData.set_noconfDbUpd(entity.getNoconfDbUpd());
        }

        summaryData.set_verblRqmt(entity.getVerblRqmt());
        summaryData.set_verblMeth(entity.getVerblMeth());
        summaryData.set_verblStatus(entity.getVerblStatus());
        if(entity.getVerblDbUpd() != null){
            summaryData.set_verblDbUpd(entity.getVerblDbUpd());
        }
        summaryData.set_groupXref(entity.getGroupXref());
        summaryData.set_id(entity.getId());
        summaryData.set_tradeId(entity.getTradeId());

        summaryData.set_inceptionDt(entity.getInceptionDt());

        summaryData.set_cdtyCode(entity.getCdtyCode());

        summaryData.set_tradeDt(convertDate(entity.getTradeDt()));

        summaryData.set_xref(entity.getXref());
        summaryData.set_cptySn(entity.getCptySn());

        summaryData.set_qtyTot(entity.getQtyTot());

        summaryData.set_locationSn(entity.getLocationSn());

        summaryData.set_startDt(convertDate(entity.getStartDt()));
        summaryData.set_endDt(convertDate(entity.getEndDt()));

        summaryData.set_book(entity.getBook());
        summaryData.set_tradeTypeCode(entity.getTradeTypeCode());
        summaryData.set_sttlType(entity.getSttlType());
        summaryData.set_brokerSn(entity.getBrokerSn());
        summaryData.set_buySellInd(entity.getBuySellInd());
        summaryData.set_refSn(entity.getRefSn());
        summaryData.set_seCptySn(entity.getSeCptySn());
        summaryData.set_tradeStatCode(entity.getTradeStatCode());
        summaryData.set_brokerPrice(entity.getBrokerPrice());
        summaryData.set_optnStrikePrice(entity.getOptnStrikePrice());
        summaryData.set_optnPremPrice(entity.getOptnPremPrice());
        summaryData.set_optnPutCallInd(entity.getOptnPutCallInd());
        summaryData.set_priority(entity.getPriority());
        summaryData.set_plAmt(entity.getPlAmt());
        summaryData.set_archiveFlag(entity.getArchiveFlag());
        summaryData.set_priceDesc(entity.getPriceDesc());
        summaryData.set_migrateInd(entity.getMigrateInd());
        summaryData.set_analystName(entity.getAnalystName());
        summaryData.set_additionalConfirmSent(entity.getAdditionalConfirmSent());
        summaryData.set_isTestBook(entity.getTestBook());
        summaryData.set_readyForReplyFlag(entity.getRplyRdyToSndFlag());
        summaryData.set_tradeSysTicket(entity.getTradeSystemTicket());
        summaryData.set_tradeDesc(entity.getTradeDescription());
        summaryData.set_quantityDescription(entity.getQuantityDescription());

        summaryData.set_bookingCoSn(entity.getBookingCompanyShortName());
        summaryData.set_bookingCoId(getNullOkIntValue(entity.getBookingCompanyId()));
        summaryData.set_cptyId(getNullOkIntValue(entity.getCounterPartyId()));
        summaryData.set_brokerLegalName(entity.getBrokerLegalName());
        summaryData.set_brokerId(getNullOkIntValue(entity.getBrokerId()));
        summaryData.set_cptyLegalName(entity.getCounterPartyLegalName());
        summaryData.set_trader(entity.getTrader());
        summaryData.set_transportDesc(entity.getTransportDecription());
        summaryData.set_cdtyGrpCode(entity.getCommodityGroupCode());
        summaryData.set_permissionKey(entity.getPermissionKey());

        return summaryData;

    }

    private static Date convertTimestamp(Timestamp startDt)
    {
        Date summaryDate = null;
        if(startDt != null)
        {
            Calendar calendar = sCalendar.get();
            calendar.setTime(startDt);
            summaryDate = (calendar.get(Calendar.YEAR) > 1) ?
                    new Date(startDt.getTime()) :
                    null;
        }
        return summaryDate;
    }

    private static Date convertDate(java.sql.Date date)
    {
        return new java.util.Date(date.getTime());
    }


    private static int getNullOkIntValue(Integer val)
    {
        return val == null ? -1 : val;
    }



}
