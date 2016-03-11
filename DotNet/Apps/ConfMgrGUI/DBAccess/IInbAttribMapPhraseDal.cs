using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInbAttribMapPhraseDal
    {
        List<InbAttribMapComboDto> GetPhrases(string pMappedValue);
        List<InbAttribMapPhraseDto> GetPhrases(Int32 pMappedValueId);
        Int32 Insert(InbAttribMapPhraseDto pData);
        Int32 Update(InbAttribMapPhraseDto pData);
        Int32 Delete(int pId);
    }
}
