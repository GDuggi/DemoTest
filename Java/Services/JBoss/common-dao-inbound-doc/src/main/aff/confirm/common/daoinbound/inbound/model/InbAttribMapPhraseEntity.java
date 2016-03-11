package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 10:02:34 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "INB_ATTRIB_MAP_PHRASE")
public class InbAttribMapPhraseEntity implements Serializable {

    private Long id;

    @Id
    @Column(name = "ID", nullable = false, length = 12)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="inbAttribMapPhrase_id_sequence")
    @SequenceGenerator(name="inbAttribMapPhrase_id_sequence", sequenceName="OPS_TRACKING.seq_inb_attrib_map_phrase")
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private Long inbAttribMapValId;

    @Column(name = "INB_ATTRIB_MAP_VAL_ID", nullable = false, length = 22)
    public Long getInbAttribMapValId() {
        return inbAttribMapValId;
    }

    public void setInbAttribMapValId(Long inbAttribMapValId) {
        this.inbAttribMapValId = inbAttribMapValId;
    }

    private String phrase;

    @Basic
    @Column(name = "PHRASE", nullable = false, length = 1000)
    public String getPhrase() {
        return phrase;
    }

    public void setPhrase(String phrase) {
        this.phrase = phrase;
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

        InbAttribMapPhraseEntity that = (InbAttribMapPhraseEntity) o;

        if (id != that.id) return false;
        if (inbAttribMapValId != that.inbAttribMapValId) return false;
        if (activeFlag != null ? !activeFlag.equals(that.activeFlag) : that.activeFlag != null) return false;
        if (phrase != null ? !phrase.equals(that.phrase) : that.phrase != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = id.intValue();
        result = 31 * result + (inbAttribMapValId != null ? inbAttribMapValId.hashCode() : 0);
        result = 31 * result + (phrase != null ? phrase.hashCode() : 0);
        result = 31 * result + (activeFlag != null ? activeFlag.hashCode() : 0);
        return result;
    }
}
