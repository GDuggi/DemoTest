package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 10:02:33 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "INB_ATTRIB")
public class InbAttribEntity implements Serializable {
    public static final String CPTY_SN   = "CPTY_SN";
    public static final String CDTY_CODE = "CDTY_CODE";
    public static final String BRKR_SN   = "BRKR_SN";
    private String code;

    @Id
    @Column(name = "CODE", nullable = false, length = 100)
    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    private String descr;

    @Basic
    @Column(name = "DESCR", length = 500)
    public String getDescr() {
        return descr;
    }

    public void setDescr(String descr) {
        this.descr = descr;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        InbAttribEntity that = (InbAttribEntity) o;

        if (code != null ? !code.equals(that.code) : that.code != null) return false;
        if (descr != null ? !descr.equals(that.descr) : that.descr != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = code != null ? code.hashCode() : 0;
        result = 31 * result + (descr != null ? descr.hashCode() : 0);
        return result;
    }
}
