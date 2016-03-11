package aff.confirm.common.efet;

import aff.confirm.common.efet.info.EFETFxInfo;
import aff.confirm.common.efet.info.EFETSpreadInfo;

import java.util.ArrayList;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 12:22:06 PM
 */
public class EFETCommodityRef {

    private String referencePrice;
    private String indexCommodity;
    private String indexCurrencyUnit;
    private String indexCapacityUnit;
    private String payRecCurrencyUnit;
    private String payRecCapacityUnit;
    private String specfiedPrice;
    private String deliveryDate;
    private String pricingDate;
    private double indexCap;
    private double indexFloor;
    private double conversinRate;
    private EFETFxInfo fxInfo;
    private EFETSpreadInfo spreadInfo;
    private ArrayList calcPeriods;
    private int factor;
    private String priceModel;

    private boolean conversionRateRequired = false;

    public boolean isConversionRateRequired() {
        return conversionRateRequired;
    }

    public void setConversionRateRequired(boolean conversionRateRequired) {
        this.conversionRateRequired = conversionRateRequired;
    }

    public String getPriceModel() {
        return priceModel;
    }

    public void setPriceModel(String priceModel) {
        this.priceModel = priceModel;
    }

    public int getFactor() {
        return factor;
    }

    public void setFactor(int factor) {
        this.factor = factor;
    }

    public String getIndexCurrencyUnit() {
        return indexCurrencyUnit;
    }

    public void setIndexCurrencyUnit(String indexCurrencyUnit) {
        this.indexCurrencyUnit = indexCurrencyUnit;
    }

    public String getReferencePrice() {
        return referencePrice;
    }

    public void setReferencePrice(String referencePrice) {
        this.referencePrice = referencePrice;
    }

    public String getIndexCommodity() {
        return indexCommodity;
    }

    public void setIndexCommodity(String indexCommodity) {
        this.indexCommodity = indexCommodity;
    }

    public String getIndexCapacityUnit() {
        return indexCapacityUnit;
    }

    public void setIndexCapacityUnit(String indexCapacityUnit) {
        this.indexCapacityUnit = indexCapacityUnit;
    }

    public String getSpecfiedPrice() {
        return specfiedPrice;
    }

    public void setSpecfiedPrice(String specfiedPrice) {
        this.specfiedPrice = specfiedPrice;
    }

    public String getDeliveryDate() {
        return deliveryDate;
    }

    public void setDeliveryDate(String deliveryDate) {
        this.deliveryDate = deliveryDate;
    }

    public String getPricingDate() {
        return pricingDate;
    }

    public void setPricingDate(String pricingDate) {
        this.pricingDate = pricingDate;
    }

    public double getIndexCap() {
        return indexCap;
    }

    public void setIndexCap(double indexCap) {
        this.indexCap = indexCap;
    }

    public double getIndexFloor() {
        return indexFloor;
    }

    public void setIndexFloor(double indexFloor) {
        this.indexFloor = indexFloor;
    }

    public double getConversinRate() {
        return conversinRate;
    }

    public void setConversinRate(double conversinRate) {
        this.conversinRate = conversinRate;
    }

    public EFETFxInfo getFxInfo() {
        return fxInfo;
    }

    public void setFxInfo(EFETFxInfo fxInfo) {
        this.fxInfo = fxInfo;
    }

    public EFETSpreadInfo getSpreadInfo() {
        return spreadInfo;
    }

    public void setSpreadInfo(EFETSpreadInfo spreadInfo) {
        this.spreadInfo = spreadInfo;
    }

    public String getPayRecCurrencyUnit() {
        return payRecCurrencyUnit;
    }

    public void setPayRecCurrencyUnit(String payRecCurrencyUnit) {
        this.payRecCurrencyUnit = payRecCurrencyUnit;
    }

    public String getPayRecCapacityUnit() {
        return payRecCapacityUnit;
    }

    public void setPayRecCapacityUnit(String payRecCapacityUnit) {
        this.payRecCapacityUnit = payRecCapacityUnit;
    }

    public ArrayList getCalcPeriods() {
        return calcPeriods;
    }

    public void setCalcPeriods(ArrayList calcPeriods) {
        this.calcPeriods = calcPeriods;
    }
}
