using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class UserRoleView
    {
        private string _userId = null;
        private string _roleCode = null;
        private string _descr = null;

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string RoleCode
        {
            get { return _roleCode; }
            set { _roleCode = value; }
        }

        public string Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }
    }
}
