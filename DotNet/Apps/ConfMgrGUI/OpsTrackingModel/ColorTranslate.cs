using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class ColorTranslate:IOpsDataObj
    {
        private String _code = null;
        private String _csColor = null;

        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public String CsColor
        {
            get { return _csColor; }
            set { _csColor = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.ColorTranslate;
        }
    }
}
