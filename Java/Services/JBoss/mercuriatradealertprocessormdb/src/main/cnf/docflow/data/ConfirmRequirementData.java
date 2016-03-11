package cnf.docflow.data;

import java.util.LinkedList;
import java.util.List;

/**
 * Created by jvega on 7/15/2015.
 */
public class ConfirmRequirementData {
        //private String rqmt_Action;
        //private String rqmt_Method;
        //private String rqmt_Party;
        private String rqmt_Workflow;
        private String rqmt_Template;
        private String rqmt_PreparerCanSend;
       //SendTos
        List<ConfirmRequirementSendToData> ConfRqmtSendToList;

        private int other_TradeRqmtID;

    public ConfirmRequirementData() {
        //this.rqmt_Action = "";
        //this.rqmt_Method = "";
        //this.rqmt_Party = "";
        this.rqmt_Workflow="";
        this.rqmt_Template="";
        this.rqmt_PreparerCanSend="";
        List<ConfirmRequirementSendToData> ConfRqmtSendToList = new LinkedList<ConfirmRequirementSendToData>();
    }

   /* @Override
    public String toString() {
        return "ConfirmRequirementData{" +
                "rqmt_Action='" + rqmt_Action + '\'' +
                ", rqmt_Method='" + rqmt_Method + '\'' +
                ", rqmt_Party='" + rqmt_Party + '\'' +
                '}';
    }*/

    /*public String getRqmt_Action() {
        return rqmt_Action;
    }

    public void setRqmt_Action(String rqmt_Action) {
        this.rqmt_Action = rqmt_Action;
    }

    public String getRqmt_Method() {
        return rqmt_Method;
    }

    public void setRqmt_Method(String rqmt_Method) {
        this.rqmt_Method = rqmt_Method;
    }

    public String getRqmt_Party() {
        return rqmt_Party;
    }

    public void setRqmt_Party(String rqmt_Party) {
        this.rqmt_Party = rqmt_Party;
    }*/

    @Override
    public String toString() {
        return "ConfirmRequirementData{" +
                "rqmt_Workflow='" + rqmt_Workflow + '\'' +
                ", rqmt_Template='" + rqmt_Template + '\'' +
                ", rqmt_PreparerCanSend='" + rqmt_PreparerCanSend + '\'' +
                ", ConfRqmtSendToList=" + ConfRqmtSendToList +
                '}';
    }

    public String getRqmt_Workflow() {
        return rqmt_Workflow;
    }

    public void setRqmt_Workflow(String rqmt_Workflow) {
        this.rqmt_Workflow = rqmt_Workflow;
    }

    public String getRqmt_Template() {
        return rqmt_Template;
    }

    public void setRqmt_Template(String rqmt_Template) {
        this.rqmt_Template = rqmt_Template;
    }

    public String getRqmt_PreparerCanSend() {
        return rqmt_PreparerCanSend;
    }

    public void setRqmt_PreparerCanSend(String rqmt_PreparerCanSend) {
        this.rqmt_PreparerCanSend = rqmt_PreparerCanSend;
    }

    public List<ConfirmRequirementSendToData> getConfRqmtSendToList() {
        return ConfRqmtSendToList;
    }

    public void setConfRqmtSendToList(List<ConfirmRequirementSendToData> confRqmtSendToList) {
        ConfRqmtSendToList = confRqmtSendToList;
    }

    public int getOther_TradeRqmtID() {
        return other_TradeRqmtID;
    }

    public void setOther_TradeRqmtID(int other_TradeRqmtID) {
        this.other_TradeRqmtID = other_TradeRqmtID;
    }
}
