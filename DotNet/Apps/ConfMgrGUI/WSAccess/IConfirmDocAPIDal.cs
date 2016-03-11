using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAccess
{
    public interface IConfirmDocAPIDal
    {
        byte[] GetStubDocxAuto();
        byte[] GetStubDocxManual();
        string GetStubTemplateList();            
    }
}
