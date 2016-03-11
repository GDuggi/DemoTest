using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSAccess.SvcRef_ConfirmationsManager;

namespace WSAccess
{
    public class ConfirmMgrAPIDal : IConfirmMgrAPIDal
    {
        private const string CONFIRMATIONS_MGR_ENDPOINT = "BasicHttpBinding_ConfirmationsManager";
        private const string CONFIRMATIONS_MGR_URL_EXT = "ConfirmationsManager";
        private const string APPLICATION_NAME = "ConfirmManager";

        private string urlStr = ""; 
        private string svcUserName = ""; 
        private string svcPassword = "";

        public ConfirmMgrAPIDal(string pBaseUrl, string pSvcUserName, string pSvcPassword)
        {
            urlStr = pBaseUrl + @"/" + CONFIRMATIONS_MGR_URL_EXT;
            svcUserName = pSvcUserName;
            svcPassword = pSvcPassword;
        }

        public List<string> GetPermissionKeys(string pUserId, out bool pSuperUserFlag)
        {
            List<string> permKeyList = new List<string>();

            ConfirmationsManagerClient client = new ConfirmationsManagerClient(CONFIRMATIONS_MGR_ENDPOINT, urlStr);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetPermissionKeysRequest request = new GetPermissionKeysRequest();
            GetPermissionKeysResponse response = new GetPermissionKeysResponse();
            request.applicationName = APPLICATION_NAME;
            request.userId = pUserId;

            response = client.getPermissionKeys(request);
            pSuperUserFlag = response.superUserFlag;
            
            foreach(string permKey in response.permissionKeyCodes){
                permKeyList.Add(permKey);
            }

            return permKeyList;
        }

        public string GetPermissionKeyDBInClause(List<string> pPermissionKeyList)
        {
            string permKeyInClause = "PERMISSION_KEY IN (";
            bool addComma = false;

            foreach (string permKey in pPermissionKeyList)
            {
                if (addComma)
                    permKeyInClause += ",";    
                permKeyInClause += "'" + permKey + "'";
                addComma = true;
            }

            permKeyInClause += ")";
            return permKeyInClause;
        }


    }
}
