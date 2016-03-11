package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class ContractFaxData implements Serializable{


	private long cptyId;
	private String shortName;
	private long phoneId;
	private String phoneTypeCode;
	private String activeFlag;
	private String countryPhoneCode;
	private String areaCode;
	private String localNumber;
	private String designationCode;
	private String dsgActiveFlag;
	private String description;
	public long getCptyId() {
		return cptyId;
	}
	public void setCptyId(long cptyId) {
		this.cptyId = cptyId;
	}
	public String getShortName() {
		return shortName;
	}
	public void setShortName(String shortName) {
		this.shortName = shortName;
	}
	public long getPhoneId() {
		return phoneId;
	}
	public void setPhoneId(long phoneId) {
		this.phoneId = phoneId;
	}
	public String getPhoneTypeCode() {
		return phoneTypeCode;
	}
	public void setPhoneTypeCode(String phoneTypeCode) {
		this.phoneTypeCode = phoneTypeCode;
	}
	public String getActiveFlag() {
		return activeFlag;
	}
	public void setActiveFlag(String activeFlag) {
		this.activeFlag = activeFlag;
	}
	public String getCountryPhoneCode() {
		return countryPhoneCode;
	}
	public void setCountryPhoneCode(String countryPhoneCode) {
		this.countryPhoneCode = countryPhoneCode;
	}
	public String getAreaCode() {
		return areaCode;
	}
	public void setAreaCode(String areaCode) {
		this.areaCode = areaCode;
	}
	public String getLocalNumber() {
		return localNumber;
	}
	public void setLocalNumber(String localNumber) {
		this.localNumber = localNumber;
	}
	public String getDesignationCode() {
		return designationCode;
	}
	public void setDesignationCode(String designationCode) {
		this.designationCode = designationCode;
	}
	public String getDsgActiveFlag() {
		return dsgActiveFlag;
	}
	public void setDsgActiveFlag(String dsgActiveFlag) {
		this.dsgActiveFlag = dsgActiveFlag;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	
}
