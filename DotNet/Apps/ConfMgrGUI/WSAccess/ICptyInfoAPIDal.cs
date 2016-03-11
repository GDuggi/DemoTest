using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAccess
{
    public interface ICptyInfoAPIDal
    {
        List<CptyAgreement> GetAgreementListStub();
        string GetAgreementDisplayStub();
        List<BdtaCptyLkup> GetOpenConfirmLookupStub();
        List<CptyAgreement> GetAgreementList(string pCptySN, string pBookingCoSN);
        string GetAgreementDisplay(string pCptySN, string pBookingCoSN, string pTradeDt);
    }
}
