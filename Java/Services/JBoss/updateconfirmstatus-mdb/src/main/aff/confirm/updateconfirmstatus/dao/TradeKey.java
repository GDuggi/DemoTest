package aff.confirm.updateconfirmstatus.dao;

/**
 * User: hblumenf
 * Date: 11/23/2015
 * Time: 12:41 PM
 * Copyright Amphora Inc. 2015
 */
public class TradeKey
{
    private final String tradeSysCode;
    private final String tradeSysTicket;

    public String getTradeSysCode()
    {
        return tradeSysCode;
    }

    public String getTradeSysTicket()
    {
        return tradeSysTicket;
    }

    public TradeKey(String tradeSysCode, String tradeSysTicket)
    {
        this.tradeSysCode = tradeSysCode;
        this.tradeSysTicket = tradeSysTicket;
    }

    @Override
    public boolean equals(Object o)
    {
        if (this == o)
        {
            return true;
        }
        if (o == null || getClass() != o.getClass())
        {
            return false;
        }

        TradeKey tradeKey = (TradeKey) o;

        if (tradeSysCode != null ? !tradeSysCode.equals(tradeKey.tradeSysCode) : tradeKey.tradeSysCode != null)
        {
            return false;
        }
        return !(tradeSysTicket != null ? !tradeSysTicket.equals(tradeKey.tradeSysTicket) :
                tradeKey.tradeSysTicket != null);

    }

    @Override
    public int hashCode()
    {
        int result = tradeSysCode != null ? tradeSysCode.hashCode() : 0;
        result = 31 * result + (tradeSysTicket != null ? tradeSysTicket.hashCode() : 0);
        return result;
    }

    @Override
    public String toString()
    {
        return "TradeKey{" +
                "tradeSysCode='" + tradeSysCode + '\'' +
                ", tradeSysTicket='" + tradeSysTicket + '\'' +
                '}';
    }
}
