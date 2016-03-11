using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSAccess.VaultSvcClient;

namespace WSAccessTest
{
    [TestClass]
    public class VaultAccessorTest
    {
        string URL = System.Configuration.ConfigurationManager.AppSettings["ExtSvcAPIBaseUrl"];//"http://hou-121.mercuria.met:9001";      

        [TestMethod]
        public void GetDocInfoTest()
        {
            try
            {
                WSAccess.VaultSvcAccessor accessor = WSAccess.VaultSvcAccessor.Instance(URL);
                ContractInfo[] results =
                    accessor.GetDocInfo("Manual", "123", "ICTS", new System.Collections.Generic.Dictionary<string, string>());
                if (results == null)
                    Assert.Fail("Failed empty messgae");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed empty messgae "+ex.Message);
            }
        }

        [TestMethod]
        public void UploadDocumentTest()
        {
            try
            {
                WSAccess.VaultSvcAccessor accessor = WSAccess.VaultSvcAccessor.Instance(URL);
                string result =
                    accessor.UploadDocument("123file","PDF","123","ICTS",null);
                if (result == null)
                    Assert.Fail("Failed empty messgae");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed empty messgae " + ex.Message);
            }
        }

        [TestMethod]
        public void GetDocumentForURLTest()
        {
            try
            {
                WSAccess.VaultSvcAccessor accessor = WSAccess.VaultSvcAccessor.Instance(URL);
                byte[] result =
                    accessor.GetDocumentForURL("Manual","123","ICTS","123filename","docs\\manual");
                if (result == null)
                    Assert.Fail("Failed empty messgae");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed empty messgae " + ex.Message);
            }
        }

        [TestMethod]
        public void GetVersionInfoTest()
        {
            try
            {
                WSAccess.VaultSvcAccessor accessor = WSAccess.VaultSvcAccessor.Instance(URL);
                ContractInfo[] results =
                    accessor.GetVersionInfo("Manual","123","ICTS","123doc","docs\\manual");
                if (results == null)
                    Assert.Fail("Failed empty messgae");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed empty messgae " + ex.Message);
            }
        }
    }
}

