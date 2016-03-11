using DBAccess.SqlServer;
using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRolesUI_Admin
{
    public static class RoleUtil
    {
        
        private static List<RoleView>  _appRoles;
        //one time setting
        public static List<RoleView> appRoles
        {
            get
            {
                return _appRoles;
            }
            set
            {
                _appRoles = value;
            }
        }
        public static List<string> AllUsers
        {
            get;
            set;
        }
        public static string connString
        {
            get;
            set;
        }

        public static List<string> appRolesCodes
        {
            get;
            set;
        }
        public static List<string> userAssignedRoles
        {
            get;
            set;
        }
        public static List<string> userUnassignedRoles
        {
            get;
            set;
        }

        private static List<string> _addedRoles = new List<string>();
        public static List<string> addedRoles
        {
            get
            {
                return _addedRoles;
            }
            set
            {
                _addedRoles = value;
            }
        }

        private static List<string> _deletedRoles = new List<string>();
        public static List<string> deletedRoles
        {
            get
            {
                return _deletedRoles;
            }
            set
            {
                _deletedRoles = value;
            }
        }
        public static void GetUserRoles(string userId)
        {
            UserRoleDal userRolsDal = new UserRoleDal(connString);
            List<UserRoleView> userRoles = userRolsDal.GetAll(userId);

            if(userRoles == null || userRoles.Count ==0 )
            {
                userUnassignedRoles = new List<string>(appRolesCodes);
                userAssignedRoles = new List<string>();
            }
            else if( appRolesCodes != null && appRolesCodes.Count == userRoles.Count)
            {
                userUnassignedRoles = new List<string>();
                userAssignedRoles = new List<string>(appRolesCodes);
            }
            else
            {
                userAssignedRoles = userRoles.Select(r => r.RoleCode).ToList();
                userUnassignedRoles = new List<string>(appRolesCodes);
                foreach(string role in userAssignedRoles)
                {
                    if(userUnassignedRoles.Contains(role))
                    {
                        userUnassignedRoles.Remove(role);
                    }
                }
            }
        }
        public static void ClearAll()
        {
            if (userUnassignedRoles != null && userAssignedRoles != null)
            {
                userUnassignedRoles.Clear();
                userAssignedRoles.Clear();
                _deletedRoles.Clear();
                _addedRoles.Clear();
                addedRoles.Clear();
                deletedRoles.Clear();
            }
        }
        public static string GetDesrForRoleCode(string roleCode)
        {
            string retVal = "";
            if(appRoles != null)
            {
               List<RoleView> role  =  (from item in appRoles where item.RoleCode == roleCode select item).ToList();
               if (role != null && role.Count > 0)
                   retVal = role[0].Descr;
            }
            return retVal;
        }
    }
}
