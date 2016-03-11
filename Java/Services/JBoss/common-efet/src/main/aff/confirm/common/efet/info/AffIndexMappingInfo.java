package aff.confirm.common.efet.info;

/**
 * User: srajaman
 * Date: Jul 29, 2009
 * Time: 9:21:01 AM
 */
public class AffIndexMappingInfo {

    private String affIndexName;
    private String efetCdtyRefCode;
    private int roundingValue;
    private String pricingDate;
    private String deliveryDate;
    private String specifiedPrice;

    public String getAffIndexName() {
        return affIndexName;
    }

    public void setAffIndexName(String affIndexName) {
        this.affIndexName = affIndexName;
    }

    public String getEfetCdtyRefCode() {
        return efetCdtyRefCode;
    }

    public void setEfetCdtyRefCode(String efetCdtyRefCode) {
        this.efetCdtyRefCode = efetCdtyRefCode;
    }

    public int getRoundingValue() {
        return roundingValue;
    }

    public void setRoundingValue(int roundingValue) {
        this.roundingValue = roundingValue;
    }

    public String getPricingDate() {
        return pricingDate;
    }

    public void setPricingDate(String pricingDate) {
        this.pricingDate = pricingDate;
    }

    public String getDeliveryDate() {
        return deliveryDate;
    }

    public void setDeliveryDate(String deliveryDate) {
        this.deliveryDate =   deliveryDate;
    }

    public String getSpecifiedPrice() {
        return specifiedPrice;
    }

    public void setSpecifiedPrice(String specifiedPrice) {
        this.specifiedPrice = specifiedPrice;
    }
}
