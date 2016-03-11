package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class FaxLogSentRequest implements Serializable {

	public static enum _FAX_TELEX_CODE   {EMAIL,FAX,TELEX}  
	public static enum _DOC_TYPE   {CNF,INB}
	
	private long tradeId;
	private String sender;
	private _FAX_TELEX_CODE faxTelexCode;
	private String faxTelexNumber;
	private _DOC_TYPE docType;
	private String docTypeRef;
	
	private long associatedDocId;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getSender() {
		return sender;
	}
	public void setSender(String sender) {
		this.sender = sender;
	}
	public _FAX_TELEX_CODE getFaxTelexCode() {
		return faxTelexCode;
	}
	public void setFaxTelexCode(_FAX_TELEX_CODE faxTelexCode) {
		this.faxTelexCode = faxTelexCode;
	}
	public String getFaxTelexNumber() {
		return faxTelexNumber;
	}
	public void setFaxTelexNumber(String faxTelexNumber) {
		this.faxTelexNumber = faxTelexNumber;
	}
	public _DOC_TYPE getDocType() {
		return docType;
	}
	public void setDocType(_DOC_TYPE docType) {
		this.docType = docType;
	}
	public String getDocTypeRef() {
		return docTypeRef;
	}
	public void setDocTypeRef(String docTypeRef) {
		this.docTypeRef = docTypeRef;
	}
	public long getAssociatedDocId() {
		return associatedDocId;
	}
	public void setAssociatedDocId(long associatedDocId) {
		this.associatedDocId = associatedDocId;
	}
	
	
}
