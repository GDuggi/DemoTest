package aff.confirm.common.efet.datarec;

import aff.confirm.common.efet.dao.EFET_DAO;

import java.text.DecimalFormat;
import java.util.Calendar;
import java.util.Date;

/**
 * User: srajaman
 * Date: Jun 13, 2007
 * Time: 3:25:46 PM
 */
public class EFETOptionSchedule_DataRec {

    private static final int EXERCISE_HOUR = 15;
    private static final int EXERCISE_MINUTES = 0;
    private static final int EXERCISE_SECONDS = 0;

    public String tradeId ;
    public java.util.Date deliveryStartDateTime ;
    public java.util.Date deliveryEndDateTime;
    public Date exerciseDateTime;


    public EFETOptionSchedule_DataRec() {
        init();
    }
    public void init() {
        tradeId = "";
        deliveryStartDateTime = null;
        deliveryEndDateTime = null;
        exerciseDateTime = null;
    }


    public void setDeliveryStartDateTime(Date pDeliveryStartDateTime,int hours) {

         Calendar calStart = Calendar.getInstance();
           calStart.setTime(pDeliveryStartDateTime);
        //Sometimes calStart.add(hour, pHour) was adding an extra hour.
        //Using set instead of add solves the problem.
        calStart.set(Calendar.HOUR, hours);
        this.deliveryStartDateTime = calStart.getTime();

    }
    public void setDeliveryStartDateTime(Date pDeliveryStartDateTime) {


        this.deliveryStartDateTime = pDeliveryStartDateTime;

    }

    public void setDeliveryEndDateTime(Date pDeliveryEndDateTime, int hours) {
        Calendar calEnd = Calendar.getInstance();
        calEnd.setTime(pDeliveryEndDateTime);
        calEnd.add(Calendar.DATE,1);
        calEnd.set(Calendar.HOUR,hours);
        this.deliveryEndDateTime = calEnd.getTime();
       // this.deliveryStartDateTime = EFETDAO.sdfEfet.format(pDeliveryEndDateTime);
    }
    public void setDeliveryEndDateTime(Date pDeliveryEndDateTime) {
        this.deliveryEndDateTime = pDeliveryEndDateTime;
       // this.deliveryStartDateTime = EFETDAO.sdfEfet.format(pDeliveryEndDateTime);
    }

    public void setExerciseDateTime(Date pExerciseDateTime) {
       // this.exerciseDateTime = EFETDAO.sdfEfet.format(pExerciseDateTime);
        if (pExerciseDateTime != null ){
            pExerciseDateTime = updateExerciseTime(pExerciseDateTime);
        }
        this.exerciseDateTime = pExerciseDateTime;
    }

    private Date updateExerciseTime(Date pExerciseDateTime) {

        Calendar cal = Calendar.getInstance();
        cal.setTime(pExerciseDateTime);
        if ( cal.get(Calendar.HOUR_OF_DAY) == 0 && cal.get(Calendar.MINUTE) == 0 && cal.get(Calendar.SECOND) ==0) {
            cal.set(Calendar.HOUR_OF_DAY,EXERCISE_HOUR);
            cal.set(Calendar.MINUTE,EXERCISE_MINUTES);
            cal.set(Calendar.SECOND,EXERCISE_SECONDS);
        }
        return cal.getTime();
    }
    
    public void setTradeId(double pTradeId) {
        DecimalFormat df = new DecimalFormat("#0");
        this.tradeId = df.format(pTradeId);
    }
    public String getEFETDeliveryStartDateTime() {
        return getEFETDateFormat(this.deliveryStartDateTime);
    }
    public String getEEFETDeliveryEndDateTime() {
        return getEFETDateFormat(this.deliveryEndDateTime);
    }

    public String getEFETExerciseDateTime() {
        return getEFETDateFormat(this.exerciseDateTime);
    }

    public static String getEFETDateFormat(Date dt) {
        if (dt == null) {return "";}
        return EFET_DAO.sdfEfet.format(dt);
    }
}
