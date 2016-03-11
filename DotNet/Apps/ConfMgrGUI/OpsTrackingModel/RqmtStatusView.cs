using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class RqmtStatusView:IOpsDataObj
    {
        private const System.Int32 _nullNumberValue = -1;

        private String _rqmtCode = null;
        private String _displayText = null;
        private String _initialStatus = null;
        private String _statusCode = null;
        private String _terminalFlag = null;
        private String _problemFlag = null;
        private String _colorCode = null;
        private String _descr = null;
        private System.Int32 _ord;

        public RqmtStatusView()
        {
            this._rqmtCode = null;
            this._displayText = null;
            this._initialStatus = null;
            this._statusCode = null;
            this._terminalFlag = null;
            this._problemFlag = null;
            this._colorCode = null;
            this._descr = null;
            this._ord = RqmtStatusView._nullNumberValue;
        }

        public System.Int32 Ord
        {
            get { return _ord; }
            set { _ord = value; }
        }


        public String Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }

        public String ColorCode
        {
            get { return _colorCode; }
            set { _colorCode = value; }
        }

        public String ProblemFlag
        {
            get { return _problemFlag; }
            set { _problemFlag = value; }
        }

        public String TerminalFlag
        {
            get { return _terminalFlag; }
            set { _terminalFlag = value; }
        }

        public String StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        public String InitialStatus
        {
            get { return _initialStatus; }
            set { _initialStatus = value; }
        }

        public String DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }


        public String RqmtCode
        {
            get { return _rqmtCode; }
            set { _rqmtCode = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.RqmtStatusView;
        }
    }
}
