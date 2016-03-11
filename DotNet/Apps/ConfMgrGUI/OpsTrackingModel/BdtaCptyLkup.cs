using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class BdtaCptyLkup:IOpsDataObj
    {
        private string _cptySn = null;

        public string CptySn
        {
            get { return _cptySn; }
            set { _cptySn = value; }
        }

        public string ShortName
        {
            set { CptySn = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.BdtaCptyLkup;
        }
    }
}
