package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 10:02:34 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "INB_ATTRIB_MAP_VAL")
public class InbAttribMapValEntity implements Serializable {

    private Long id;

    @Id
    @Column(name = "ID", nullable = false, length = 12)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="inbAttribMapVal_id_sequence")
    @SequenceGenerator(name="inbAttribMapVal_id_sequence", sequenceName="OPS_TRACKING.SEQ_INB_ATTRIB_MAP_VAL")
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private String inbAttribCode;

    @Basic
    @Column(name = "INB_ATTRIB_CODE", nullable = false, length = 100)
    public String getInbAttribCode() {
        return inbAttribCode;
    }

    public void setInbAttribCode(String inbAttribCode) {
        this.inbAttribCode = inbAttribCode;
    }

    private String mappedValue;

    @Basic
    @Column(name = "MAPPED_VALUE", nullable = false, length = 500)
    public String getMappedValue() {
        return mappedValue;
    }

    public void setMappedValue(String mappedValue) {
        this.mappedValue = mappedValue;
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

        InbAttribMapValEntity that = (InbAttribMapValEntity) o;

        if (id != that.id) return false;
        if (activeFlag != null ? !activeFlag.equals(that.activeFlag) : that.activeFlag != null) return false;
        if (descr != null ? !descr.equals(that.descr) : that.descr != null) return false;
        if (inbAttribCode != null ? !inbAttribCode.equals(that.inbAttribCode) : that.inbAttribCode != null)
            return false;
        if (mappedValue != null ? !mappedValue.equals(that.mappedValue) : that.mappedValue != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = id.intValue();
        result = 31 * result + (inbAttribCode != null ? inbAttribCode.hashCode() : 0);
        result = 31 * result + (mappedValue != null ? mappedValue.hashCode() : 0);
        result = 31 * result + (descr != null ? descr.hashCode() : 0);
        result = 31 * result + (activeFlag != null ? activeFlag.hashCode() : 0);
        return result;
    }
}
