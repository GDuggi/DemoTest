using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager
{

    public class FireEventArgs: EventArgs
    {
        public string fireMsg;
        public FireEventArgs(string fireMsg)
        {
            this.fireMsg = fireMsg;
        }
    }
}
