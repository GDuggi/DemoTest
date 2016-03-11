package aff.confirm.common.vaultlib.util;

import java.io.Serializable;

public class VaultInfo implements Serializable{

	public static final String _CONTRACT_FIELD_NAMES = "TRD_SYS_CODE|TRD_SYS_ID|CONTRACT_ID|SE_CPTY_SN|CPTY_SN|DATE_SENT|SIGNED_FLAG|RQMT_ID|TRADE_RQMT_CONFIRM_ID|TRADE_DT|CDTY_CODE|CDTY_GROUP|STTL_TYPE";
	public static final String _SEARCH_CONTRACT_FIELD_NAMES = "TRADE_RQMT_CONFIRM_ID";
	
	private String docRepository;
	private byte[] data;
	private String fieldNames;
	private String fieldValues;
	private String userName;
	private String dslName;
	
	
	public String getDocRepository() {
		return docRepository;
	}
	public void setDocRepository(String docRepository) {
		this.docRepository = docRepository;
	}
	public byte[] getData() {
		return data;
	}
	public void setData(byte[] data) {
		this.data = data;
	}
	public String getFieldNames() {
		return fieldNames;
	}
	public void setFieldNames(String fieldNames) {
		this.fieldNames = fieldNames;
	}
	public String getFieldValues() {
		return fieldValues;
	}
	public void setFieldValues(String fieldValues) {
		this.fieldValues = fieldValues;
	}
	public String getUserName() {
		return userName;
	}
	public void setUserName(String userName) {
		this.userName = userName;
	}
	public String getDslName() {
		return dslName;
	}
	public void setDslName(String dslName) {
		this.dslName = dslName;
	}
	
}
