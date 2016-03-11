using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeContractInfoDal
    {
        List<TradeContractInfoDto> GetContractListStub();
        List<TradeContractInfoDto> GetContractList(string pTrdSysCode, string pTrdSysTicket);
    }
}
