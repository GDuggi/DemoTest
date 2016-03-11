package aff.confirm.common.util;

import java.sql.Connection;

/**
 * User: ifrankel
 * Date: Jun 14, 2004
 * Time: 8:01:19 AM
 * To change this template use Options | File Templates.
 */
public class Schedule {
    private Connection connection;
    private String scheduleCode;
    private String scheduleTime;
    public Schedule(String pScheduleCode, String pScheduleTime, Connection pConnection){
        this.connection = pConnection;
        this.scheduleCode = pScheduleCode;
        this.scheduleTime = pScheduleTime;
    }
}
