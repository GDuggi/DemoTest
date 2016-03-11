package aff.confirm.common.util;

import java.util.GregorianCalendar;

/**
 * User: hblumenf
 * Date: Jun 9, 2005
 * Time: 4:58:04 PM
 */
public class ThreadLocalGregorianCalendar extends ThreadLocal {
    protected Object initialValue()
    {
        return new GregorianCalendar();
    }

    public GregorianCalendar getGregorianCalendar()
    {
        return (GregorianCalendar)get();
    }

}
