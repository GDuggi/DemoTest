package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.daoinbound.inbound.ejb3.VActiveAssociatedDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.VActiveAssociatedDocsEntity;
import aff.confirm.common.util.JndiTool;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.AssociatedDoc;

import javax.naming.InitialContext;
import java.util.Calendar;
import java.util.List;

/**
 * User: mthoresen
 * Date: Sep 20, 2012
 * Time: 8:19:37 AM
 */
public class AssocDocSubPubMsgProcessor extends BaseSubPubMsgProcessor{

    private VActiveAssociatedDocsDAOLocal adoBean;

    public AssocDocSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
    }

    protected void initAdo(InitialContext ctx) throws Exception {
        adoBean = JndiTool.lu(ctx, "java:global/InboundDocsDAOLib/VActiveAssociatedDocsDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VActiveAssociatedDocsDAOLocal");
    }

    public void processMessage(List<String> msgList) {
        long id = Long.parseLong(msgList.get(1));

        VActiveAssociatedDocsEntity entity = new VActiveAssociatedDocsEntity();
        this.mylogger.info("Getting Entity for Associated DocID: " + id);
        // TRADE SUMMARY DATA
        entity = adoBean.findById(new Long(id),false);
        postMessage(entity, id);
    }

    private void postMessage(VActiveAssociatedDocsEntity entity, long assocDocId) {
        AssociatedDoc data = null;
        data = new AssociatedDoc();
        Calendar cal = Calendar.getInstance();
        try{
            if(entity != null){
                data.set_id(entity.getId());
                data.set_inboundDocsId(entity.getInboundDocsId());
                data.set_indexVal(entity.getIndexVal());
                data.set_trdSysTicket(entity.getTrdSysTicket());
                data.set_trdSysCode(entity.getTrdSysCode());
                data.set_fileName(entity.getFileName());
                data.set_tradeId(entity.getTradeId());
                data.set_docStatusCode(entity.getDocStatusCode());

                data.set_associatedBy(entity.getAssociatedBy());
                if(entity.getAssociatedDt() != null){
                    cal.setTime(entity.getAssociatedDt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_associatedDt(new java.util.Date(entity.getAssociatedDt().getTime()));
                    }else data.set_associatedDt(null);
                }

                data.set_finalApprovedBy(entity.getFinalApprovedBy());
                if(entity.getFinalApprovedDt() != null){
                    cal.setTime(entity.getFinalApprovedDt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_associatedDt(new java.util.Date(entity.getFinalApprovedDt().getTime()));
                    }else data.set_finalApprovedDt(null);
                }

                data.set_disputedBy(entity.getDisputedBy());
                if(entity.getDisputedDt() != null){
                    cal.setTime(entity.getDisputedDt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_associatedDt(new java.util.Date(entity.getDisputedDt().getTime()));
                    }else data.set_disputedDt(null);
                }

                data.set_discardedBy(entity.getDiscardedBy());
                if(entity.getDiscardedDt() != null){
                    cal.setTime(entity.getDiscardedDt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_associatedDt(new java.util.Date(entity.getDiscardedDt().getTime()));
                    }else data.set_discardedDt(null);
                }

                data.set_vaultedBy(entity.getVaultedBy());
                if(entity.getVaultedDt() != null){
                    cal.setTime(entity.getVaultedDt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_associatedDt(new java.util.Date(entity.getVaultedDt().getTime()));
                    }else data.set_vaultedDt(null);
                }

                data.set_cdtyGroupCode(entity.getCdtyGroupCode());
                data.set_cptyShortName(entity.getCptySn());
                data.set_brokerShortName(entity.getBrokerSn());
                data.set_docTypeCode(entity.getDocTypeCode());
                data.set_secondValidateReqFlag(entity.getSecValidateReqFlag());
                data.set_tradeRqmtId(entity.getTradeRqmtId());

                data.set_tradeFinalApprovalFlag(entity.getTradeFinalApprovalFlag());
                data.set_xmitStatusCode(entity.getXmitStatusCode());
                data.set_xmitValue(entity.getXmitValue());
                data.set_sentTo(entity.getSentTo());

            } else{
                data.set_id(assocDocId);
                data.set_tradeFinalApprovalFlag("Y");
            }
            OpsManagerMessage message = new OpsManagerMessage();
            message.setData(data);
            message.setMessageType("assoc");
            this.sendMessage(message);
        } catch(Exception ex){
            this.mylogger.error("Error posting message:" + ex.getMessage(),ex);
        }
    }
}