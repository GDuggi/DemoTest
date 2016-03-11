package aff.confirm.common.util;

import java.sql.Timestamp;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.Locale;

/**
 * User: ifrankel
 * Date: Sep 9, 2003
 * Time: 3:15:54 PM
 * To change this template use Options | File Templates.
 */
public class DateUtils {
    public static final SimpleDateFormat sdfDateTime = new SimpleDateFormat("yyyyddMM_hhmmss", Locale.US);

    /**
     * Returns number of days in a month
     * @param pYear - Actual year
     * @param pMonth - jan=1, feb=2
     * @return
     */
    public static int getDaysInMonth(int pYear, int pMonth){
        int days = 0;

        // Create a calendar object of the desired month
        Calendar cal = new GregorianCalendar(pYear, pMonth - 1, 1);

        // Get the number of days in that month
        days = cal.getActualMaximum(Calendar.DAY_OF_MONTH); // 28

        return days;
    }

    /**
     * Gets the current date and time in the following format:
     * 2003-07-30 10:34:02
     * @return
     * //20031201_092559
     */
    public static String getDateTimeStamp() {
        Calendar calendar = Calendar.getInstance();
        calendar.setLenient(false);
        Date date = calendar.getTime();
        long millis = date.getTime();
        Timestamp timestamp = new Timestamp(millis);
        //2003-07-30 10:34:02.846
        String ts = timestamp.toString().substring(0,19);
        return ts;
    }


    /**
     * Gets the current date in the following format:
     * 2003-07-30
     * @return
     */
    public static String getDateStamp() {
        Calendar calendar = Calendar.getInstance();
        calendar.setLenient(false);
        Date date = calendar.getTime();
        long millis = date.getTime();
        Timestamp timestamp = new Timestamp(millis);
        //2003-07-30 10:34:02.846
        String ds = timestamp.toString().substring(0,10);
        return ds;
    }


    /**
     * Returns a calendar from a dateTime string
     * @param pDateTime formatted as follows: 2003-07-30 10:34:02
     * @return
     */
    public static Calendar getCalendarFromDateTime(String pDateTime){
        Integer temp;
        temp = Integer.valueOf(pDateTime.substring(0,4));
        int year = temp.intValue();
        temp = Integer.valueOf(pDateTime.substring(5,7));
        int month = temp.intValue();
        //month is zero-based
        month--;
        temp = Integer.valueOf(pDateTime.substring(8,10));
        int date = temp.intValue();
        temp = Integer.valueOf(pDateTime.substring(11,13));
        int hour = temp.intValue();
        temp = Integer.valueOf(pDateTime.substring(14,16));
        int minute = temp.intValue();
        temp = Integer.valueOf(pDateTime.substring(17,19));
        int second = temp.intValue();

        Calendar calendar = Calendar.getInstance();
        //calendar.setLenient(false);
        calendar.set(year, month, date, hour, minute, second);
        return calendar;
    }


    /**
     * Returns a dateTime string from a calendar date
     * @param  pCalendar calendar set to the required date
     * @return formatted as follows: 2003-07-30 10:34:02
     */
    public static String getDateTimeFromCalendar(Calendar pCalendar){
        String dateTime = "";
        int year = pCalendar.get(Calendar.YEAR);
        int month = pCalendar.get(Calendar.MONTH);
        int date = pCalendar.get(Calendar.DATE);
        int hour = pCalendar.get(Calendar.HOUR_OF_DAY); //HOUR_OF_DAY = 24 hour clock
        int minute = pCalendar.get(Calendar.MINUTE);
        int second = pCalendar.get(Calendar.SECOND);
        String strYear = String.valueOf(year);
        String strMonth = StringUtils.zeroFill(month,2);
        String strDate = StringUtils.zeroFill(date,2);
        String strHour = StringUtils.zeroFill(hour,2);
        String strMinute = StringUtils.zeroFill(minute,2);
        String strSecond = StringUtils.zeroFill(second,2);

        dateTime = strYear + "-" +
                   strMonth + "-" +
                   strDate + " " +
                   strHour + ":" +
                   strMinute + ":" +
                   strSecond;
        return dateTime;
    }

    /**
     *
     * @param pDate - Date to be incremented
     * @param pDays - Days to increment
     * @return - Incremented Date
     */
    public static Date incrementDate(Date pDate, int pDays) {
        Date newDate = pDate;
        Calendar calDate = Calendar.getInstance();
        calDate.setTime(pDate);
        calDate.add(Calendar.DATE, pDays);
        newDate = calDate.getTime();
        return newDate;
    }

    /**
     * Returns date and time in following format: 20031201_092559
     * @return
     */
    public static String getDateTimeString(){
        //return sdfDateTime.format(Calendar.getInstance().getTime());
        return sdfDateTime.format(new Date());
     /*   Calendar calendar = Calendar.getInstance();
        calendar.setLenient(false);
        Date date = calendar.getTime();
        long millis = date.getTime();
        Timestamp timestamp = new Timestamp(millis);
        String dateTimeString = "";
        dateTimeString =
            timestamp.toString().substring(0,4) +   //yyyy
            timestamp.toString().substring(5,7) +   //mm
            timestamp.toString().substring(8,10) +  //dd
            "_" +
            timestamp.toString().substring(11,13) +  //hh
            timestamp.toString().substring(14,16) +  //nn
            timestamp.toString().substring(17,19);   //ss

        return dateTimeString;*/
    }

     public static Date getDateTimeString(String date) throws ParseException {
        //return sdfDateTime.format(Calendar.getInstance().getTime());
        return sdfDateTime.parse(date);
     /*   Calendar calendar = Calendar.getInstance();
        calendar.setLenient(false);
        Date date = calendar.getTime();
        long millis = date.getTime();
        Timestamp timestamp = new Timestamp(millis);
        String dateTimeString = "";
        dateTimeString =
            timestamp.toString().substring(0,4) +   //yyyy
            timestamp.toString().substring(5,7) +   //mm
            timestamp.toString().substring(8,10) +  //dd
            "_" +
            timestamp.toString().substring(11,13) +  //hh
            timestamp.toString().substring(14,16) +  //nn
            timestamp.toString().substring(17,19);   //ss

        return dateTimeString;*/
    }


}
