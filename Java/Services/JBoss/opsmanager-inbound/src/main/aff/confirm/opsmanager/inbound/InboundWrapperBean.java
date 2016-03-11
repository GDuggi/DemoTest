package aff.confirm.opsmanager.inbound;

import aff.confirm.opsmanager.inbound.data.*;
import org.jboss.ws.api.annotation.WebContext;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebService;

@Stateless(name="InboundWrapperBean",mappedName="InboundWrapperBean")
@WebService(serviceName="InboundService",name="InboundService",targetNamespace="http://rbssempra.com/confirm")
@WebContext(contextRoot="InBound")
public class InboundWrapperBean implements InboundWrapper {

	@EJB private InboundDoc inbDoc;
	
	

	@WebMethod(operationName="updateDocStatus")
	public DocStatusResponse[] updateDocStatus(
			@WebParam(name="documentList")  DocStatusRequest[] request,
			@WebParam(name="userName",header=true) String userName) {
		return inbDoc.updateDocStatus(request, userName);
	}

	@WebMethod(operationName="getUserFlags")
	public GetUserFlagResponse getUserFlags(
			@WebParam(name="userFlagRequest") GetUserFlagRequest request,
			@WebParam(name="userName",header=true) String userName) {
		return inbDoc.getUserFlags(request, userName);
	}

	@WebMethod(operationName="updateUserFlags")
	public UserFlagResponse[] updateUserFlags(
			@WebParam(name="flagUpdateRequest")  UserFlagRequest[] request,
			@WebParam(name="userName",header=true) String userName) {
		return inbDoc.updateUserFlags(request, userName);
	}

	@WebMethod(operationName="updateInboundDoc")
	public InboundUpdateResponse[] updateInboundDoc(
			@WebParam(name="updateRequest") InboundUpdateRequest[] request, 
			@WebParam(name="userName", header=true)  String userName) {
		return inbDoc.updateInboundDoc(request, userName);
	}

	@WebMethod(operationName="mapCallerRef")
	public MapCallerReferenceResponse[] mapCallerRef(
			@WebParam(name="callerRefRequest") MapCallerReferenceRequest[] request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.mapCallerRef(request, userName);
	}

	@WebMethod(operationName="unMapCallerRef")
	public MapCallerReferenceResponse[] unMapCallerRef(
			@WebParam(name="callerRefRequest") MapCallerReferenceRequest[] request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.unMapCallerRef(request, userName);
	}

	@WebMethod(operationName="createAssociateDoc")
	public AssociatedDocumentResponse[] createAssociateDoc(
			@WebParam(name="assocDocRequest") AssociatedDocumentRequest[] request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.createAssociateDoc(request, userName);
	}

	@WebMethod(operationName="updateAssociateDoc")
	public AssociatedDocumentResponse[] updateAssociateDoc(
			@WebParam(name="assocDocRequest") AssociatedDocumentRequest[] request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.updateAssociateDoc(request, userName);
	}

	@WebMethod(operationName="getInboundDocCount")
	public InboundDocCountResponse getInboundDocCount(
			@WebParam(name="inboundCountRequest") InboundDocCountRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.getInboundDocCount(request, userName);
		
	}

	@WebMethod(operationName="getInboundUnResolvedCount")
	public InboundUnResolvedCountResponse getInboundUnResolvedCount(
			@WebParam(name="inboundCountRequest")  InboundUnResolvedCountRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.getInboundUnResolvedCount(request, userName);
		
	}

	@WebMethod(operationName="updateAssociatedStatus")
	public UpdateAssocDocResponse updateAssociatedStatus(
			@WebParam(name="docRequest") UpdateAssocDocRequest request, 
			@WebParam(name="userName", header=true)  String userName) {
		
		return inbDoc.updateAssociatedStatus(request, userName);
	}

	@WebMethod(operationName="getCurrentIndexValue")
	public IndexCountResponse getCurrentIndexValue(
			@WebParam(name="indexRequest")  IndexCountRequest request, 
			@WebParam(name="userName", header=true)  String userName) {

		return inbDoc.getCurrentIndexValue(request, userName);
	}

	@WebMethod(operationName="getInboundFaxNumbers")
	public InboundFaxNumberResponse getInboundFaxNumbers(
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.getInboundFaxNumbers(userName);
	}

	@WebMethod(operationName="getInbAttrMapValues")
	public AttrMapValueResponse getInbAttrMapValues(
			@WebParam(name="attrMapValueRequest")  AttrMapValueRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.getInbAttrMapValues(request, userName);
	}

	@WebMethod(operationName="getInbAttribValPhrases")
	public AttrMapPhraseResponse getInbAttribValPhrases(
			@WebParam(name="attrMapPhraseRequest")  AttrMapPhraseRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		
		return inbDoc.getInbAttribValPhrases(request, userName);
	}

	@WebMethod(operationName="updateAttribValPhrases")
	public UpdateMapPhraseResponse updateAttribValPhrases(
			@WebParam(name="updateAttrMapPhraseRequest")   UpdateMapPhraseRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.updateAttribValPhrases(request, userName);
	}

	@WebMethod(operationName="updateInbDocStatus")
	public InbDocStatusResponse updateInbDocStatus(
			@WebParam(name="updateInbStatusRequest") InbDocStatusRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.updateInbDocStatus(request, userName);
	}

	@WebMethod(operationName="updateAttrMapValue")
	public InbAttrMapValueUpdateResponse updateAttrMapValue(
			@WebParam(name="updateInbAttrMapValueRequest") InbAttrMapValueUpdateRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		
		return inbDoc.updateAttrMapValue(request, userName);
	}

	@WebMethod(operationName="getInbAttribValPhrasesById")
	public AttrMapPhraseResponse getInbAttribValPhrasesById(
			@WebParam(name="attrMapPhraseRequest") AttrMapPhraseRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.getInbAttribValPhrasesById(request, userName);
	}

	@WebMethod(operationName="getInbAttrMapByMapValue")
	public AttrMapValueResponse getInbAttrMapByMapValue(
			@WebParam(name="attrMapValueRequest") AttrMapValueRequest request, 
			@WebParam(name="userName", header=true) String userName) {

		return inbDoc.getInbAttrMapByMapValue(request, userName);
	}

	@WebMethod(operationName="insertInboundDoc")
	public InboundDocResponse insertInboundDoc(
			@WebParam(name="inboundDocRequest") InboundDocRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return inbDoc.insertInboundDoc(request, userName);
	}

	
}
