using System;
using System.Collections.Generic;
using System.Text;

namespace ConfirmInbound
{
    class AutoMatchVerifyException :Exception
    {
        public AutoMatchVerifyException(string msg) : base(msg)
        {
        }
    }
}
