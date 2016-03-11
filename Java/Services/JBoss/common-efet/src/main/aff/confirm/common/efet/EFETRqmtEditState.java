package aff.confirm.common.efet;

import aff.confirm.common.dbqueue.QEFETTradeAlert;
import aff.confirm.common.efet.dao.EFETAgreement_DAO;
import aff.confirm.common.efet.dao.EFETCptyEIC_DAO;
import aff.confirm.common.efet.dao.EFET_DAO;

import java.sql.SQLException;
import java.text.DecimalFormat;

//import sempra.common.ottradealert.OpsTrackingTRADE_RQMT_dao;
//import sempra.common.ottradealert.OpsTrackingTradeAlertDataRec;

/**
 * User: ifrankel
 * Date: Jul 17, 2006
 * Obtains info for trade, stores data, and returns data according to state.
 */
public class EFETRqmtEditState {
    static final public String EFET_CPTY = "EFET";
    static final public String EFET_BROKER = "EFBKR";

    //public boolean 

    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String efetRqmt;
    private EFETAgreement_DAO efetAgreementDAO;
    private EFETCptyEIC_DAO efetCptyEICDAO;
    private QEFETTradeAlert qEFETTradeAlert;
   // private OpsTrackingTRADE_RQMT_dao otTRADE_RQMT_dao;
    private EFET_DAO efetDAO;
   // private OpsTrackingTradeAlertDataRec opsTrackingTradeAlertDataRec;

    private DecimalFormat df = new DecimalFormat("#0");

    public EFETRqmtEditState(java.sql.Connection pOpsTrackingConnection,
                             java.sql.Connection pAffinityConnection,
                             String pEfetRqmt) throws SQLException, Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
        this.efetRqmt = pEfetRqmt;
        efetAgreementDAO = new EFETAgreement_DAO(opsTrackingConnection);
        efetCptyEICDAO = new EFETCptyEIC_DAO(opsTrackingConnection);
    }



}
