package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;

/**
 * User: mthoresen
 * Date: Mar 18, 2010
 * Time: 9:01:26 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "INBOUND_FAX_NOS")
public class InboundFaxNosEntity implements Serializable {
    private String faxno;

    @Id
    @Column(name = "FAXNO", nullable = false, length = 100)
    public String getFaxno() {
        return faxno;
    }

    public void setFaxno(String faxno) {
        this.faxno = faxno;
    }

    private String locCode;

    @Basic
    @Column(name = "LOC_CODE", nullable = false, length = 5)
    public String getLocCode() {
        return locCode;
    }

    public void setLocCode(String locCode) {
        this.locCode = locCode;
    }

    private String activeFlag;

    @Basic
    @Column(name = "ACTIVE_FLAG", length = 1)
    public String getActiveFlag() {
        return activeFlag;
    }

    public void setActiveFlag(String activeFlag) {
        this.activeFlag = activeFlag;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        InboundFaxNosEntity that = (InboundFaxNosEntity) o;

        if (activeFlag != null ? !activeFlag.equals(that.activeFlag) : that.activeFlag != null) return false;
        if (faxno != null ? !faxno.equals(that.faxno) : that.faxno != null) return false;
        if (locCode != null ? !locCode.equals(that.locCode) : that.locCode != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = faxno != null ? faxno.hashCode() : 0;
        result = 31 * result + (locCode != null ? locCode.hashCode() : 0);
        result = 31 * result + (activeFlag != null ? activeFlag.hashCode() : 0);
        return result;
    }
}
