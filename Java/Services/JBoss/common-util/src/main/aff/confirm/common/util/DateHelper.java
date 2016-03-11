/*
 * User: vveloso
 * Date: Sep 12, 2003
 * Time: 4:49:01 PM
 */
package aff.confirm.common.util;

import org.jboss.logging.Logger;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;



public class DateHelper {
    private static Logger log = Logger.getLogger( DateHelper.class.getName() );

    public static int[][] daysTable = {
        {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31},
        {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}};

    public static String[] monthNames = {
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec"
    };

    public static String[] weekDayNames = {
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
    };

    public static final int JAN = 0;
    public static final int FEB = 1;
    public static final int MAR = 2;
    public static final int APR = 3;
    public static final int MAY = 4;
    public static final int JUN = 5;
    public static final int JUL = 6;
    public static final int AUG = 7;
    public static final int SEP = 8;
    public static final int OCT = 9;
    public static final int NOV = 10;
    public static final int DEC = 11;
    public static final int NUM_MONTHS = 12;

    private static final int LEAP_YEAR_DAYS = 366;
    private static final int NON_LEAP_YEAR_DAYS = 365;

    public static boolean isLeapYear(int year) {
        return ((year % 4) == 0) && (((year % 100) != 0) || ((year % 400) == 0));
    }

    public static int encodeDate(int year, int month, int day) {
        return (year * 10000) + (month * 100) + day;
    }

    public final static int DOW_MONDAY = 1;
    public final static int DOW_TUESDAY = 2;
    public final static int DOW_WEDNESDAY = 3;
    public final static int DOW_THURSDAY = 4;
    public final static int DOW_FRIDAY = 5;
    public final static int DOW_SATURDAY = 6;
    public final static int DOW_SUNDAY = 0;

    private static Date lastDate;
    static{
        try {
            lastDate = new SimpleDateFormat( "MM/dd/yyyy" ).parse("12/31/2299");
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }
    private static String[] dayOfWeekString = new String[]{"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};

    public static String getDayOfWeekAsString(int dowInt) {
        return dayOfWeekString[dowInt];
    }

    private static int dayOfWeekData[] = {0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};

    private static final ThreadLocalGregorianCalendar CALENDAR = new ThreadLocalGregorianCalendar();
    /**
     * Gets the day of the week
     *
     * @param aDate
     * @return Returns 0 to 6, 0 - Sunday, 6-Saturday
     */

    public static int getDayOfWeek(int aDate) {
        int year = aDate / 10000;
        int year2 = year;
        int month = (aDate - (year * 10000)) / 100;
        int day = (aDate - (year * 10000) - (month * 100));

        year2 = year;
        if (month < 3) year2--;	// Account for leap years.
        return (year2 + (year2 / 4) - (year2 / 100) + (year2 / 400) + dayOfWeekData[month - 1] + day) % 7;
    }

    public static int addDays( int currentDate, int daysToAdd )
    {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = (currentDate - (year * 10000) - (month * 100));

        int direction = ( daysToAdd > 0 ) ? 1 : -1;
        int absDaysToAdd = Math.abs( daysToAdd );

        while( absDaysToAdd > 0 )
        {
            absDaysToAdd--;
            day += direction;

            if ( !dateIsvalid( year, month, day ) )
            {
                        //Invalid Date, move month to next month
                day = 1;
                month++;
                if (month > 12) {
                    //Moved into next year must move year too
                    month = 1;
                    year++;
                }
            }
        }

        return encodeDate( year, month, day );
    }

    public static int getNextday(int currentDate) {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = (currentDate - (year * 10000) - (month * 100));

        //Increase the date to next day...
        day++;

        //Check if the day went into the next month
        if (dateIsvalid(year, month, day)) {
            return encodeDate(year, month, day);
        } else {
            //Invalid Date, move month to next month
            day = 1;
            month++;
            if (month > 12) {
                //Moved into next year must move year too
                month = 1;
                year++;
            }
            return encodeDate(year, month, day);
        }
    }

    public static int getPriorDay(int currentDate)
    {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = (currentDate - (year * 10000) - (month * 100));

        //Increase the date to next day...
        day--;

        //Check if the day went into prior month
        if ( dateIsvalid( year, month, day ) )
        {
            return encodeDate( year, month, day );
        }

        int priorDlvryMonth = getPriorDeliveryMonth( currentDate );
        year = getYear( priorDlvryMonth );
        month = getMonth( priorDlvryMonth );

        return encodeDate( year, month, getDaysInMonth( month, isLeapYear( year ) ) );                
    }


    public static int getPriorDeliveryMonth(int currentDate) {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = (currentDate - (year * 10000) - (month * 100));
        day = 1;
        month--;
        if (month <= 0) {
            //Moved into prior year must move year backwards one
            month = 12;
            year--;
        }
        return encodeDate(year, month, day);

    }

    public static int getNextDeliveryMonth(int currentDate) {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = 1;
        month++;
        if (month > 12) {
            //Moved into next year must move year too
            month = 1;
            year++;
        }
        return encodeDate(year, month, day);

    }

    public static int getDeliveryMonth(int currentDate)
    {
        return getFirstDateOfMonth( currentDate );
    }

    public static int getFirstDateOfMonth(int currentDate)
    {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        int day = 1;
        return encodeDate(year, month, day);
    }

    public static int getFirstDayOfWeekInMonth(int currentDate, int dayOfWeek )
    {
        if ( dayOfWeek < DOW_SUNDAY || dayOfWeek > DOW_SATURDAY )
        {
            throw new IllegalArgumentException( "Invalid day of the week passed in to getFirstDayOfWeekInMonth()" );
        }

        int firstOfMonth = getFirstDateOfMonth( currentDate );
        while( getDayOfWeek( firstOfMonth ) != dayOfWeek )
        {
            firstOfMonth = getNextday( firstOfMonth );
        }

        return firstOfMonth;
    }


    /**
     * Checks if the date parameters, month and day are valid, ie: month betwen 1 and 12, and day between 1 and (28, 29, 30 ,31)?.
     *
     * @param year
     * @param month
     * @param day
     * @return
     */
    public static boolean dateIsvalid(int year, int month, int day) {
        //Day cannot be less than 1
        if (day < 1) return false;

        //month must be between 1 and 12
        if ((month < 1) || (month > 12)) {
            //Invalid month
            return false;
        }

        //Get the offset into the days array
        int leapYearInt = isLeapYear(year) ? 1 : 0;

        //Get the maximum date for the month
        int maxDay = daysTable[leapYearInt][(month - 1)];

        //Day cannot be greater than maximum day in month
        if (day > maxDay) return false;

        return true;
    }



    /**
     * Calculate months between startDate and endDate, when equal return 0
     * @param startDate
     * @param endDate
     * @return
     */
    public static int getMonthsBetween(int startDate, int endDate) {

        int startYear = startDate / 10000;
        int startMonth = (startDate - (startYear * 10000)) / 100;

        int endYear = endDate / 10000;
        int endMonth = (endDate - (endYear * 10000)) / 100;


        return ( ( endYear - startYear) * 12 ) + endMonth - startMonth;

    }


/*

    public static void main(String[] args) {
        int iters = Integer.parseInt(args[0]);
        java.util.Date aDate = new java.util.Date();
        int xxx = DateHelper.encodeDate( aDate);
        System.out.println( "xxx = " + xxx );
        DateHelper.encodeDate2( aDate);


        long start = System.currentTimeMillis();
        for (int i = 0; i < iters; i++)
            DateHelper.encodeDate( aDate);
        System.out.println( "total = " + (System.currentTimeMillis() - start) );

        start = System.currentTimeMillis();
        for (int i = 0; i < iters; i++)
            DateHelper.encodeDate2( aDate);
        System.out.println( "total = " + (System.currentTimeMillis() - start) );
    }
*/

    /**
     * Compares the month and year component of two dates.
     *
     * @param aDate
     * @param compareToDate
     * @return +1 if aDate is in a greater month than compareToDate, 0 if in the same month, -1 if in a lesser month.
     */
    public static int compareMonths(int aDate, int compareToDate) {
        int year1 = aDate / 10000;
        int year2 = compareToDate / 10000;
        if (year1 > year2) return 1;
        if (year1 < year2) return -1;
        int month1 = (aDate - (year1 * 10000)) / 100;
        int month2 = (compareToDate - (year2 * 10000)) / 100;
        if (month1 > month2) return 1;
        if (month1 < month2) return -1;
        return 0;
    }

    /**
     * Compares the year component of two dates.
     *
     * @param aDate
     * @param compareToDate
     * @return +1 if aDate is in a greater year than compareToDate, 0 if in the same year, -1 if in a lesser year.
     */
    public static int compareYear(int aDate, int compareToDate) {
        int year1 = aDate / 10000;
        int year2 = compareToDate / 10000;
        if (year1 > year2) return 1;
        if (year1 < year2) return -1;
        return 0;
    }

    public static String formatDate_EEEE(int aDate) {
        return weekDayNames[getDayOfWeek(aDate)];
    }

    public static String formatDate_DD_MMM_YYYY(int aDate) {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return day + "-" + monthNames[month - 1] + "-" + year;
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_DD_MMM_YYYY(" + aDate + ")", e);
            return aDate + "";
        }

    }

    public static int encodeDate(java.util.Date aDate) {
        GregorianCalendar calendar = CALENDAR.getGregorianCalendar();
        calendar.setTime( aDate );
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH)+1;
        int day = calendar.get(Calendar.DATE);
        return encodeDate( year, month, day );
    }

    public static int getToday()
    {
        return encodeDate( new Date( System.currentTimeMillis() ) );
    }

/*
    public static int encodeDate2(java.util.Date aDate) {
        return encodeDate(aDate.getYear() + 1900, aDate.getMonth() + 1, aDate.getDate());
    }
*/

    public static String formatDate_YYYYMMDD(int aDate) {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return year + ((month < 10) ? "0" : "") + month + ((day < 10) ? "0" : "") + day;
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_YYYYMMDD(" + aDate + ")", e);
            return aDate + "";
        }
    }

    public static String formatDate_YYYYMMDD(int aDate, String separator) {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return year + separator + ((month < 10) ? "0" : "") + month + separator + ((day < 10) ? "0" : "") + day;
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_YYYYMMDD(" + aDate + "," + separator + ")", e);
            return aDate + "";
        }
    }

    public static String formatDate_MMDDYYYY(int aDate) {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return ((month < 10) ? "0" : "") + month + "/" + ((day < 10) ? "0" : "") + day + "/" + year;
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_YYYYMMDD(" + aDate + ")", e);
            return aDate + "";
        }
    }


    public static String formatDate_MMMYY(int aDate) {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return monthNames[month - 1] + ("" + year).substring(2);
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_DD_MMM_YYYY(" + aDate + ")", e);
            return aDate + "";
        }
    }

    public static String formatDate_MMM_YYYY( int aDate )
    {
        try {
            int year = aDate / 10000;
            int month = (aDate - (year * 10000)) / 100;
            int day = (aDate - (year * 10000) - (month * 100));

            return monthNames[month - 1] + ("-" + year);
        } catch (Throwable e) {
            log.error( "Error in call to DateHelper.formatDate_DD_MMM_YYYY(" + aDate + ")", e);
            return aDate + "";
        }

    }

    public static int getYear(int currentDate) {
        //Decode the date into tree components, year, month, day
        return currentDate / 10000;
    }

    public static int getMonth(int currentDate) {
        //Decode the date into tree components, year, month, day
        int year = getYear(currentDate);
        return (currentDate - (year * 10000)) / 100;
    }

    public static int addMonths( int currentDate, int months )
    {
        int year = getYear( currentDate );
        int month = getMonth( currentDate );
        int day = getDay( currentDate );

        month += months;
        if ( month < JAN )
        {
            year--;
            month += NUM_MONTHS;
        }
        else if ( month > DEC )
        {
            year++;
            month -= NUM_MONTHS;
        }

        return encodeDate( year, month, day );
    }

    /**
     * Gets the Day, ie: 15-DEC-2004 will return 15.
     *
     * @param currentDate
     * @return
     */
    public static int getDay(int currentDate) {
        return (currentDate - ((currentDate / 100)) * 100);
    }

    public static int getDayOfMonth(int currentDate) {
        //Decode the date into tree components, year, month, day
        int year = currentDate / 10000;
        int month = (currentDate - (year * 10000)) / 100;
        return (currentDate - (year * 10000) - (month * 100));
    }

    public static int getDaysInMonth(int month, boolean isLeap) {
        return daysTable[isLeap ? 1 : 0][(month - 1)];
    }

    public static int makeEndOfMonthdate(int currentDate) {
        int year = getYear(currentDate);
        int month = getMonth(currentDate);
        int day = getDaysInMonth(month, isLeapYear(year));
        return encodeDate(year, month, day);
    }

    public static int makeFirstOfMonthdate(int currentDate) {
        int year = getYear(currentDate);
        int month = getMonth(currentDate);
        return encodeDate(year, month, 1);
    }

    public static boolean isLastDateInMonth(int currentDate) {
        int yyear = getYear(currentDate);
        int month = getMonth(currentDate);
        int day = getDaysInMonth(month, isLeapYear(yyear));
        int lastDateInMonth = encodeDate(yyear, month, day);
        return (lastDateInMonth == currentDate);
    }

    public static java.sql.Date makeJavaDate(int aDate) {
        int year = aDate / 10000;
        int month = (aDate - (year * 10000)) / 100;
        int day = (aDate - (year * 10000) - (month * 100));
        return new java.sql.Date(year - 1900, month - 1, day);
    }

    public static java.util.Date createJavaDateFromString_YYYYMMDD(String businessDateString) {
        int aDate = Integer.parseInt(businessDateString);
        return makeJavaDate(aDate);
    }

    public static boolean dateIsvalid(int busnDate) {
        int year = busnDate / 10000;
        int month = (busnDate - (year * 10000)) / 100;
        int day = (busnDate - (year * 10000) - (month * 100));
        return dateIsvalid(year, month, day);
    }

    public static int parseString_MMDDYYYY(String inputData) throws Exception {
        int state = 0; //0-month, 1-day, 2-year
        String readString = "";
        int month = 0;
        int day = 0;
        int year = 0;
        for (int c = 0; c < inputData.length(); c++) {
            char character = inputData.charAt(c);
            if ((character >= '0') && (character <= '9')) {
                readString += character;
            }
            switch (state) {
                case 0:
                    if (readString.length() >= 2) {
                        month = Integer.valueOf(readString).intValue();
                        state = 1;
                        readString = "";
                    }
                    ;
                    break;
                case 1:
                    if (readString.length() >= 2) {
                        day = Integer.valueOf(readString).intValue();
                        state = 2;
                        readString = "";
                    }
                    ;
                    break;
                case 2:
                    if (readString.length() >= 4) {
                        year = Integer.valueOf(readString).intValue();
                        state = 3;
                        readString = "";
                    }
                    ;
                    break;
            }

            if (state == 3) {
                //Finished...
                return encodeDate(year, month, day);
            }
        }
        throw new Exception("Invalid Date");
    }

    public static int daysBetween(int startDt, int endDt) {
        int counter = 0;
        int runningDate = startDt;
        while (runningDate < endDt) {
            counter++;
            runningDate = getNextday(runningDate);
        }
        return counter;
    }

    /**
     * Gets the days between two dates, inclusive of those dates (ie : from 1-JAN-2004 to 1-JAN-2004 returns 1)
     * @param startDt
     * @param endDt
     * @return
     */
    public static int getDaysBetween(int startDt, int endDt) {
        if (startDt == endDt) return 1;

        if (startDt > endDt) {
            int tmp = startDt;
            startDt = endDt;
            endDt = tmp;
        }

        //Start Date
        int sYear = startDt / 10000;
        int sMonth = (startDt - (sYear * 10000)) / 100;
        int sDay = (startDt - (sYear * 10000) - (sMonth * 100));

        //End Date
        int eYear = endDt / 10000;
        int eMonth = (endDt - (eYear * 10000)) / 100;
        int eDay = (endDt - (eYear * 10000) - (eMonth * 100));

        return getDaysBetween(sYear, sMonth, sDay, eYear, eMonth, eDay);
    }

    /**
     * Gets the days between two dates, inclusive of those dates (ie : from 1-JAN-2004 to 1-JAN-2004 returns 1)
     * @param sYear
     * @param sMonth
     * @param sDay
     * @param eYear
     * @param eMonth
     * @param eDay
     * @return
     */
    private static int getDaysBetween(int sYear, int sMonth, int sDay, int eYear, int eMonth, int eDay) {

        //If start Date is less than end Date return 0
        if (sYear > eYear) return 0;
        if ((sYear == eYear) && (sMonth > eMonth)) return 0;
        if ((sYear == eYear) && (sMonth == eMonth) && (sDay > eDay)) return 0;

        int totalDays = 0;

        if (sYear < eYear) {
            //In Different Years

            //Count the full years between
            int curYear = sYear + 1;
            while (curYear < eYear) {
                totalDays += getDaysInYear(curYear);
                curYear++;
            }
            ;

            //get the leading range + traling range
            totalDays += (getDaysBetween(sYear, sMonth, sDay, sYear, 12, 31) + getDaysBetween(eYear, 1, 1, eYear, eMonth, eDay));
            return totalDays;
        } else if (sMonth < eMonth) {
            //In Different Months
            boolean leapYear = isLeapYear(sYear);

            //Count the full Months between
            int curMonth = sMonth + 1;
            while (curMonth < eMonth) {
                totalDays += getDaysInMonth(curMonth, leapYear);
                curMonth++;
            }
            //get the leading days range + traling days range
            totalDays += (getDaysBetween(sYear, sMonth, sDay, sYear, sMonth, getDaysInMonth(sMonth, leapYear)) + getDaysBetween(eYear, eMonth, 1, eYear, eMonth, eDay));

            return totalDays;
        } else {
            totalDays = eDay - sDay + 1;
            return totalDays;
        }
    }

    private static int getDaysInYear(int year) {
        if (isLeapYear(year)) return LEAP_YEAR_DAYS; else return NON_LEAP_YEAR_DAYS;
    }

    public static Date getMaxDate(){
        return lastDate;
    }


}
