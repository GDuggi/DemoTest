using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class InterOpTester
    {
        private const System.Int32 _nullNumberValue = -1;

        
        private System.Int32 _id;
        private System.DateTime? _myDate;

        public System.DateTime? MyDate
        {
            get { return _myDate; }
            set { _myDate = value; }
        }

        public System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public InterOpTester()
        {
            this._id = _nullNumberValue;
            this._myDate = null;
        }
    }
}
