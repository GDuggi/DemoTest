package aff.confirm.common.efet.info;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 12:48:09 PM
 */
public class EFETSpreadInfo {

    private String payer;
    private double amount;
    private String currency;
    private EFETFxInfo fxInfo;


    public String getPayer() {
        return payer;
    }

    public void setPayer(String payer) {
        this.payer = payer;
    }

    public double getAmount() {
        return amount;
    }

    public void setAmount(double amount) {
        this.amount = amount;
    }

    public String getCurrency() {
        return currency;
    }

    public void setCurrency(String currency) {
        this.currency = currency;
    }

    public EFETFxInfo getFxInfo() {
        return fxInfo;
    }

    public void setFxInfo(EFETFxInfo fxInfo) {
        this.fxInfo = fxInfo;
    }
}
