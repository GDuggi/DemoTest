package aff.confirm.common.efet;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 1:04:06 PM
 */
public class EFETCalcPeriod {

    private Date startDate;
    private Date endDate;

    private SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");

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
        if (endDate != null){
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
}
