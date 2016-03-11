package aff.confirm.opsmanager.servlet.transmissiongateway;

import javax.print.attribute.standard.QueuedJobCount;

/**
 * User: hblumenf
 * Date: 11/10/2015
 * Time: 8:46 AM
 * Copyright Amphora Inc. 2015
 */
enum GatewayStatus
{
    Success("S"),
    Queued("Q"),
    Failed("F");

    private String databaseInd;

    GatewayStatus(String databaseInd)
    {
        this.databaseInd = databaseInd;
    }

    public String getDatabaseInd()
    {
        return databaseInd;
    }

    public static GatewayStatus parse(String str)
    {
        if (str == null)
        {
            throw new IllegalArgumentException("Unable to parse a null string into a GatewayStatus");
        }

        str = str.trim();
        for (GatewayStatus status : values())
        {
            if (status.name().equalsIgnoreCase(str))
            {
                return status;
            }
        }

        throw new UnsupportedOperationException("Unable to match '" + str + "' into a GatewayStatus");
    }
}
