package cnf.docflow.data;

/**
 * Created by jvega on 9/21/2015.
 */
public class ConfirmRequirementSendToData {
        private String rqmtSendTo_transmitMethodInd;
        private String rqmtSendTo_faxCountryCode;
        private String rqmtSendTo_faxAreaCode;
        private String rqmtSendTo_faxLocalNumber;
        private String rqmtSendTo_emailAddress;

    public ConfirmRequirementSendToData() {
        this.rqmtSendTo_transmitMethodInd = "";
        this.rqmtSendTo_faxCountryCode = "";
        this.rqmtSendTo_faxAreaCode = "";
        this.rqmtSendTo_faxLocalNumber = "";
        this.rqmtSendTo_emailAddress = "";
    }

    @Override
    public String toString() {
        return "ConfirmRequirementSendToData{" +
                "rqmtSendTo_transmitMethodInd='" + rqmtSendTo_transmitMethodInd + '\'' +
                ", rqmtSendTo_faxCountryCode='" + rqmtSendTo_faxCountryCode + '\'' +
                ", rqmtSendTo_faxAreaCode='" + rqmtSendTo_faxAreaCode + '\'' +
                ", rqmtSendTo_faxLocalNumber='" + rqmtSendTo_faxLocalNumber + '\'' +
                ", rqmtSendTo_emailAddress='" + rqmtSendTo_emailAddress + '\'' +
                '}';
    }

    public String getRqmtSendTo_transmitMethodInd() {
        return rqmtSendTo_transmitMethodInd;
    }

    public void setRqmtSendTo_transmitMethodInd(String rqmtSendTo_transmitMethodInd) {
        this.rqmtSendTo_transmitMethodInd = rqmtSendTo_transmitMethodInd;
    }

    public String getRqmtSendTo_faxCountryCode() {
        return rqmtSendTo_faxCountryCode;
    }

    public void setRqmtSendTo_faxCountryCode(String rqmtSendTo_faxCountryCode) {
        this.rqmtSendTo_faxCountryCode = rqmtSendTo_faxCountryCode;
    }

    public String getRqmtSendTo_faxAreaCode() {
        return rqmtSendTo_faxAreaCode;
    }

    public void setRqmtSendTo_faxAreaCode(String rqmtSendTo_faxAreaCode) {
        this.rqmtSendTo_faxAreaCode = rqmtSendTo_faxAreaCode;
    }

    public String getRqmtSendTo_faxLocalNumber() {
        return rqmtSendTo_faxLocalNumber;
    }

    public void setRqmtSendTo_faxLocalNumber(String rqmtSendTo_faxLocalNumber) {
        this.rqmtSendTo_faxLocalNumber = rqmtSendTo_faxLocalNumber;
    }

    public String getRqmtSendTo_emailAddress() {
        return rqmtSendTo_emailAddress;
    }

    public void setRqmtSendTo_emailAddress(String rqmtSendto_emailAddress) {
        this.rqmtSendTo_emailAddress = rqmtSendto_emailAddress;
    }
}
