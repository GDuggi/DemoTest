using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInbAttribMapValDal
    {
        List<InbAttribMapValDto> GetMapValues(string pInbAttribCode);
        Int32 Insert(InbAttribMapValDto pData);
        Int32 Update(InbAttribMapValDto pData);
        Int32 Delete(Int32 pId);
        //int Delete(string pMappedValue);
    }
}
