package aff.confirm.jboss.dbinfo;

class IpInfo {

    String ipAddress;
    String port;
    String sid;

    IpInfo(String ipAddress, String port) {
        this.ipAddress = ipAddress;
        this.port = port;
    }

    IpInfo(String ipAddress, String port, String sid) {
        this.ipAddress = ipAddress;
        this.port = port;
        this.sid = sid;
    }


    static final String ORACLE_THIN_CLIENT_PREFIX = "jdbc:oracle:thin:@";
    static String makeOracleThinClientURL(IpInfo ipInfo) {
        if (ipInfo.sid == null)
            throw new IllegalArgumentException("Oracle sid can't be null[ip=" + ipInfo.ipAddress + ":port=" + ipInfo.port + "]");

        return ORACLE_THIN_CLIENT_PREFIX + "//" + ipInfo.ipAddress + ":" + ipInfo.port + "/" + ipInfo.sid;

        //jdbc:oracle:thin:@//ct-prod1:1521/prod
    }
}
