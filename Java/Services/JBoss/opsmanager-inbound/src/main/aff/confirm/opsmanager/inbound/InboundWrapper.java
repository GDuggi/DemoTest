package aff.confirm.opsmanager.inbound;

import aff.confirm.opsmanager.inbound.data.*;

import javax.ejb.Remote;

@Remote
public interface InboundWrapper {

	DocStatusResponse[] updateDocStatus(DocStatusRequest[] request,String userName);
	GetUserFlagResponse getUserFlags(GetUserFlagRequest request,String userName);
	UserFlagResponse[] updateUserFlags(UserFlagRequest[] request,String userName);
	InboundUpdateResponse[] updateInboundDoc(InboundUpdateRequest[] request,String userName);
	MapCallerReferenceResponse[] mapCallerRef(MapCallerReferenceRequest[] request,String userName);
	MapCallerReferenceResponse[] unMapCallerRef(MapCallerReferenceRequest[] request,String userName);
	AssociatedDocumentResponse[] createAssociateDoc(AssociatedDocumentRequest[] request,String userName);
	AssociatedDocumentResponse[] updateAssociateDoc(AssociatedDocumentRequest[] request,String userName);
	InboundDocCountResponse getInboundDocCount(InboundDocCountRequest request,String userName);
	InboundUnResolvedCountResponse getInboundUnResolvedCount(InboundUnResolvedCountRequest request,String userName);
	UpdateAssocDocResponse updateAssociatedStatus(UpdateAssocDocRequest request,String userName);
	IndexCountResponse getCurrentIndexValue(IndexCountRequest request, String userName);
	InboundFaxNumberResponse getInboundFaxNumbers(String userName);
	AttrMapValueResponse getInbAttrMapValues(AttrMapValueRequest request, String userName);
	AttrMapPhraseResponse getInbAttribValPhrases(AttrMapPhraseRequest request,String userName);
	UpdateMapPhraseResponse updateAttribValPhrases(UpdateMapPhraseRequest request,String userName); 
	InbDocStatusResponse updateInbDocStatus(InbDocStatusRequest request,String userName);
	InbAttrMapValueUpdateResponse updateAttrMapValue(InbAttrMapValueUpdateRequest request,String userName);
	AttrMapPhraseResponse getInbAttribValPhrasesById(AttrMapPhraseRequest request,String userName);
	AttrMapValueResponse getInbAttrMapByMapValue(AttrMapValueRequest request,String userName );
	InboundDocResponse insertInboundDoc(InboundDocRequest request,String userName);
	
	
}
