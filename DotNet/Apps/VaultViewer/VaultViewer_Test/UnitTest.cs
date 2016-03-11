using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaultViewer;

namespace VaultViewer_Test
{
    [TestClass]
    public class UnitTest
    {
        private string expectedValue = String.Empty;
        private string actualValue = String.Empty;
        private int messageSeqNo = 0;
        private string vaultSvcUrl = @"http://cnf01inf01:11111";
        private string vaultSvcUserName = "cnfsvcprd";
        private string vaultSvcPassword = @"78ui&*UI";

        private string getMessage(string pMessage)
        {
            string result = String.Empty;
            messageSeqNo++;
            result = messageSeqNo.ToString() + ": " + pMessage;
            return result;
        }

        [TestMethod]
        public void TestVaultServiceAPI()
        {
            VaultServiceDal vaultServiceDal = new VaultServiceDal(vaultSvcUrl, vaultSvcUserName, vaultSvcPassword);

            string tradingSysCode = "SYMPH";
            string ticketNo = "IF-1106-09";
            //Dictionary<string, string> docQueryDic = new Dictionary<string, string>();
            //docQueryDic.Add("key", "value");
            string result = "";
            //result = vaultServiceDal.GetDocumentInfoList(tradingSysCode, ticketNo, null);


        }
    }
}
