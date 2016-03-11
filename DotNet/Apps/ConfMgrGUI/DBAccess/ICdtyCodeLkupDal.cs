using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ICdtyCodeLkupDal
    {
        List<BdtaCdtyLkup> GetAllStub();
        List<BdtaCdtyLkup> GetAll();
    }
}
