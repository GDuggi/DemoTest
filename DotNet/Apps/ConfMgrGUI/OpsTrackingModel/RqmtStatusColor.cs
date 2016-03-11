using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class RqmtStatusColor:IOpsDataObj
    {
        private string _hashKey = null;
        private string _csColor = null;

        public string HashKey
        {
            get { return _hashKey; }
            set { _hashKey = value; }
        }

        public string Hashkey  // added for DB loading of database objects.  Set field is not recognized.  Correct fix should be to modify the SQL.  Refactor is an issue though.
        {
            set { HashKey = value; }
        }

        public string CsColor
        {
            get { return _csColor; }
            set { _csColor = value; }
        }

        public string getSelect()
        {
            return Properties.Settings.Default.RqmtStatusColor;
        }
    }
}
