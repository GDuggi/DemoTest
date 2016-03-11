using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class BdtaCdtyLkup:IOpsDataObj
    {
        string _cdtyCode = null;

        public string CdtyCode
        {
            get { return _cdtyCode; }
            set { _cdtyCode = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.BdtaCdtyLkup;
        }
    }
}
