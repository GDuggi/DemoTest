using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class BdtaSeCptyLkup
    {
        private string _cptySn = null;


        public BdtaSeCptyLkup()
        {
            this._cptySn = null;
        }
        
        public string CptySn
        {
            get { return _cptySn; }
            set { _cptySn = value; }
        }
    }
}
