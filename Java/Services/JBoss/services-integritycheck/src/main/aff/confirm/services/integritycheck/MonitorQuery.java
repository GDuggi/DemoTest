/**
 * User: islepini
 * Date: Sep 16, 2003
 * Time: 1:58:38 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.services.integritycheck;

public class MonitorQuery {
    private String name;
    private String sql;

    public String getName() {
        return name;
    }

    public String getSql() {
        return sql;
    }

    public int getTreshhold() {
        return treshhold;
    }

    private int treshhold;

    public MonitorQuery(String name, String sql, int treshhold) {
        this.name = name;
        this.sql = sql;
        this.treshhold = treshhold;
    }

}
