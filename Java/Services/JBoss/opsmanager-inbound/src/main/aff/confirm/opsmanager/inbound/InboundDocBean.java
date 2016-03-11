package aff.confirm.opsmanager.inbound;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.inbound.common.InboundProcessor;
import aff.confirm.opsmanager.inbound.data.*;
import org.jboss.logging.Logger;

import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;
import java.sql.SQLException;

@Stateless(name="InboundDoc",mappedName="InboundDoc")
public class InboundDocBean extends BaseOpsBean implements InboundDoc {

	private InboundProcessor inb;
	
	public InboundDocBean() throws NamingException {
		super();
//		inb = new InboundProcessor();
        //Israel 6/11/14 -- Necessary for EAP 6 functionality.
		inb = new InboundProcessor(this.affinityConnection);
	}

	@Override
	public void prepareForMethodCall() {
		inb.setConnection(this.affinityConnection);
	}

	public DocStatusResponse[] updateDocStatus(DocStatusRequest[] request,
			String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateDocStatus called");
		
		DocStatusResponse[] resp = new DocStatusResponse[]{ new DocStatusResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateDocStatus(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateDocStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateDocStatus returned");
		return resp;

	}

	public GetUserFlagResponse getUserFlags(GetUserFlagRequest request,
			String userName) {
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getUserFlags called");
		
		GetUserFlagResponse resp = new GetUserFlagResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getUserFlags(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getUserFlags error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getUserFlags returned");
		return resp;
	}

	public UserFlagResponse[] updateUserFlags(UserFlagRequest[] request,
			String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateUserFlags called");
		
		UserFlagResponse[] resp = new UserFlagResponse[]{ new UserFlagResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateUserFlag(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateUserFlags error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateUserFlags returned");
		return resp;
	}

	public InboundUpdateResponse[] updateInboundDoc(InboundUpdateRequest[] request,String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateInboundDoc called");
		
		InboundUpdateResponse[] resp = new InboundUpdateResponse[]{ new InboundUpdateResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateInboundDoc(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateInboundDoc error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateInboundDoc returned");
		return resp;
	}

	public MapCallerReferenceResponse[] mapCallerRef(
			MapCallerReferenceRequest[] request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") mapCallerRef called");
		
		MapCallerReferenceResponse[] resp = new MapCallerReferenceResponse[]{ new MapCallerReferenceResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.mapCallerRef(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") mapCallerRef error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") mapCallerRef returned");
		return resp;
	}

	public MapCallerReferenceResponse[] unMapCallerRef(
			MapCallerReferenceRequest[] request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") unMapCallerRef called");
		
		MapCallerReferenceResponse[] resp = new MapCallerReferenceResponse[]{ new MapCallerReferenceResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.unMapCallerRef(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") unMapCallerRef error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") unMapCallerRef returned");
		return resp;
	}

	public AssociatedDocumentResponse[] createAssociateDoc(
			AssociatedDocumentRequest[] request,String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") createAssociateDoc called");
		
		AssociatedDocumentResponse[] resp = new AssociatedDocumentResponse[]{ new AssociatedDocumentResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.createAssociateDocument(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") createAssociateDoc error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") createAssociateDoc returned");
		return resp;
	}

	public AssociatedDocumentResponse[] updateAssociateDoc(
			AssociatedDocumentRequest[] request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAssociateDoc called");
		
		AssociatedDocumentResponse[] resp = new AssociatedDocumentResponse[]{ new AssociatedDocumentResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateAssociateDocument(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateAssociateDoc error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAssociateDoc returned");
		return resp;
	}

	public InboundDocCountResponse getInboundDocCount(
			InboundDocCountRequest request, String userName) {

		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundDocCount called");
		
		InboundDocCountResponse resp = new InboundDocCountResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInboundDocCount(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInboundDocCount error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundDocCount returned");
		return resp;
	}

	public InboundUnResolvedCountResponse getInboundUnResolvedCount(
			InboundUnResolvedCountRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundUnResolvedCount called");
		
		InboundUnResolvedCountResponse resp = new InboundUnResolvedCountResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInboundUnResolvedDocCount(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInboundUnResolvedCount error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundUnResolvedCount returned");
		return resp;	}

	
	public UpdateAssocDocResponse updateAssociatedStatus(
			UpdateAssocDocRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAssociatedStatus called");
		
		UpdateAssocDocResponse resp = new UpdateAssocDocResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateAssocStatus(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateAssociatedStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAssociatedStatus returned");
		return resp;
	}

	public IndexCountResponse getCurrentIndexValue(
			IndexCountRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getCurrentIndexValue called");
		
		IndexCountResponse resp = new IndexCountResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getCurrentIndexValue(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getCurrentIndexValue error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getCurrentIndexValue returned");
		return resp;
	}

	public InboundFaxNumberResponse getInboundFaxNumbers(String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundFaxNumbers called");
		
		InboundFaxNumberResponse resp = new InboundFaxNumberResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getFaxNumber(userName);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInboundFaxNumbers error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInboundFaxNumbers returned");
		return resp;	
	}

	public AttrMapValueResponse getInbAttrMapValues(
			AttrMapValueRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttrMapValues called");
		
		AttrMapValueResponse resp = new AttrMapValueResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInbAttrMapValues(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInbAttrMapValues error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttrMapValues returned");
		return resp;
		
	}

	public AttrMapPhraseResponse getInbAttribValPhrases(
			AttrMapPhraseRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttribValPhrases called");
		
		AttrMapPhraseResponse resp = new AttrMapPhraseResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInbAttribValPhrases(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInbAttribValPhrases error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttribValPhrases returned");
		return resp;
	}

	public UpdateMapPhraseResponse updateAttribValPhrases(
			UpdateMapPhraseRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAttribValPhrases called");
		
		UpdateMapPhraseResponse resp = new UpdateMapPhraseResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateAttribValPhrases(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateAttribValPhrases error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAttribValPhrases returned");
		return resp;
	}

	public InbDocStatusResponse updateInbDocStatus(
			InbDocStatusRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateInbDocStatus called");
		
		InbDocStatusResponse resp = new InbDocStatusResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateInbDocStatus(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateInbDocStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateInbDocStatus returned");
		return resp;
	}

	public InbAttrMapValueUpdateResponse updateAttrMapValue(
			InbAttrMapValueUpdateRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAttrMapValue called");
		
		InbAttrMapValueUpdateResponse resp = new InbAttrMapValueUpdateResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.updateAttrMapValue(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") updateAttrMapValue error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") updateAttrMapValue returned");
		return resp;
	}

	public AttrMapPhraseResponse getInbAttribValPhrasesById(
			AttrMapPhraseRequest request, String userName) {

		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttribValPhrasesById called");
		
		AttrMapPhraseResponse resp = new AttrMapPhraseResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInbAttribValPhrasesById(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInbAttribValPhrasesById error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttribValPhrasesById returned");
		return resp;
	}

	public AttrMapValueResponse getInbAttrMapByMapValue(
			AttrMapValueRequest request, String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttrMapByMapValue called");
		
		AttrMapValueResponse resp = new AttrMapValueResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.getInbAttrMapByMapValue(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") getInbAttrMapByMapValue error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") getInbAttrMapByMapValue returned");
		return resp;
	}

	public InboundDocResponse insertInboundDoc(InboundDocRequest request,
			String userName) {
		
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") insertInboundDoc called");
		
		InboundDocResponse resp = new InboundDocResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =inb.insertInboundDoc(request);
		} catch (SQLException e) {
			Logger.getLogger(InboundDocBean.class).error("User(" + userName + ") insertInboundDoc error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(InboundDocBean.class).info("User(" + userName + ") insertInboundDoc returned");
		return resp;
	}

	
	

}
