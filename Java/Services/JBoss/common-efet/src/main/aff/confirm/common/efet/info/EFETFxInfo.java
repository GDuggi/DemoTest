package aff.confirm.common.efet.info;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 12:32:12 PM
 */
public class EFETFxInfo {

    private String fxReference;
    private String fxMethod;
    private double fxRate;

    public String getFxReference() {
        return fxReference;
    }

    public void setFxReference(String fxReference) {
        this.fxReference = fxReference;
    }

    public String getFxMethod() {
        return fxMethod;
    }

    public void setFxMethod(String fxMethod) {
        this.fxMethod = fxMethod;
    }

    public double getFxRate() {
        return fxRate;
    }

    public void setFxRate(double fxRate) {
        this.fxRate = fxRate;
    }
}
