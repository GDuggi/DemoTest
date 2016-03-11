package aff.confirm.common.efet;

import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: srajaman
 * Date: Mar 26, 2009
 * Time: 8:51:20 AM
 */
public class EFETDeliveryPeriod {

    private Date startDate;
    private Date endDate;
    private double qty;
    private double fixedPrice;

    private DecimalFormat df = new DecimalFormat("#0.####");
    private SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");

    public Date getPaymentDate() {
        return paymentDate;
    }

    public String getPaymentDateFmt() {
        if ( paymentDate != null) {
            return sdfDate.format(paymentDate);
        }
        return "";
    }

    public void setPaymentDate(Date paymentDate) {
        this.paymentDate = paymentDate;
    }

    private Date paymentDate;

    public Date getStartDate() {
        return startDate;
    }

    public String getStartDateFmt() {
        if (startDate != null) {
            return sdfDate.format(startDate);
        }
        return "";
    }

    public void setStartDate(Date startDate) {
        this.startDate = startDate;
    }

    public String getEndDateFmt() {
        if (endDate != null) {
            return sdfDate.format(endDate);
        }
        return "";
    }

    public Date getEndDate() {
        return endDate;
    }

    public void setEndDate(Date endDate) {
        this.endDate = endDate;
    }
    public String getQtyFmt(){
        return df.format(qty);
    }
    public double getQty() {
        return qty;
    }

    public void setQty(double qty) {
        this.qty = qty;
    }

    public String getFixedPriceFmt() {
        return df.format(fixedPrice);
    }

    public double getFixedPrice() {
        return fixedPrice;
    }

    public void setFixedPrice(double fixedPrice) {
        this.fixedPrice = fixedPrice;
    }
}
