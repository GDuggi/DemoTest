using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class RqmtView:IOpsDataObj
    {
        private String _code = null;
        private String _descr = null;
        private String _category = null;
        private String _initialStatus = null;
        private String _displayText = null;
        private String _activeFlag = null;
        private String _detActRqmtFlag = null;

        public String DetActRqmtFlag
        {
            get { return _detActRqmtFlag; }
            set { _detActRqmtFlag = value; }
        }

        public String ActiveFlag
        {
            get { return _activeFlag; }
            set { _activeFlag = value; }
        }

        public String DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        public String InitialStatus
        {
            get { return _initialStatus; }
            set { _initialStatus = value; }
        }

        public String Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public String Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }
       
        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.RqmtView;
        }
    }
}
