package aff.confirm.common.efet.info;

import aff.confirm.common.efet.EFETCommodityRef;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 12:20:27 PM
 */
public class EFETPriceInfo {

    private String priceType;
    private String payer;
    private String fixedCurreny;
    private String fixedUom;
    private double converionRate;
    
    private EFETCommodityRef commodityRef;

    public String getPriceType() {
        return priceType;
    }

    public void setPriceType(String priceType) {
        this.priceType = priceType;
    }

    public String getPayer() {
        return payer;
    }

    public void setPayer(String payer) {
        this.payer = payer;
    }

    public String getFixedCurreny() {
        return fixedCurreny;
    }

    public void setFixedCurreny(String fixedCurreny) {
        this.fixedCurreny = fixedCurreny;
    }

    public String getFixedUom() {
        return fixedUom;
    }

    public void setFixedUom(String fixedUom) {
        this.fixedUom = fixedUom;
    }

    public double getConverionRate() {
        return converionRate;
    }

    public void setConverionRate(double converionRate) {
        this.converionRate = converionRate;
    }

    public EFETCommodityRef getCommodityRef() {
        return commodityRef;
    }

    public void setCommodityRef(EFETCommodityRef commodityRef) {
        this.commodityRef = commodityRef;
    }
}
