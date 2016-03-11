package aff.confirm.updateconfirmstatus.dao;

/**
 * User: hblumenf
 * Date: 11/23/2015
 * Time: 3:51 PM
 * Copyright Amphora Inc. 2015
 */
public class TradeKeyLookupException
    extends Exception
{
    public TradeKeyLookupException()
    {
    }

    public TradeKeyLookupException(String message)
    {
        super(message);
    }

    public TradeKeyLookupException(String message, Throwable cause)
    {
        super(message, cause);
    }

    public TradeKeyLookupException(Throwable cause)
    {
        super(cause);
    }

    public TradeKeyLookupException(String message, Throwable cause, boolean enableSuppression,
                                   boolean writableStackTrace)
    {
        super(message, cause, enableSuppression, writableStackTrace);
    }
}
