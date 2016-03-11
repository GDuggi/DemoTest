using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInboundFaxNosDal
    {
        List<InboundFaxNosDto> GetAllStub();
        List<InboundFaxNosDto> GetAll();
    }
}
