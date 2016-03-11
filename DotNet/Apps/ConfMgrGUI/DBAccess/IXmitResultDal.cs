using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IXmitResultDal
    {
        List<XmitResultDto> GetByTradeRqmtConfirmId(Int32 pTradeRqmtConfirmId);
        List<XmitResultDto> GetByAssociatedDocsId(Int32 pAssociatedDocsId);
    }
}
