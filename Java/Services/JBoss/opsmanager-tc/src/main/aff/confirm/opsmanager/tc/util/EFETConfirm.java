package aff.confirm.opsmanager.tc.util;

import java.util.ArrayList;

import javax.ejb.Remote;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitRequest;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitResponse;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmResponse;

@Remote
public interface EFETConfirm {
	BaseResponse resubmit(int tradeId, String status, String userName);
	ArrayList<EFETConfirmResponse> batchSubmit(ArrayList<EFETConfirmRequest> tradeList, String userName);
	ECMBoxSubmitResponse submitToECMBox(ECMBoxSubmitRequest request,String userName); 
}
