package aff.confirm.opsmanager.tc.util;

import java.util.ArrayList;

import javax.ejb.Remote;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchResponse;
import aff.confirm.opsmanager.tc.data.util.EConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmResponse;

@Remote	
public interface EConfirm {


	BaseResponse resubmit(int tradeId, String status, String userName);
	ArrayList<EConfirmResponse> batchSubmit(ArrayList<EConfirmRequest> tradeList,String userName);
	ArrayList<EConfirmMatchResponse> batchMatch(ArrayList<EConfirmMatchRequest> tradeidList, String userName);
}
