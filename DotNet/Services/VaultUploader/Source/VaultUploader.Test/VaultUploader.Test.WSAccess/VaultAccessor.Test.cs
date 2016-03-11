using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaultUploader.WSAccess;
using VaultUploader.WSAccess.VaultServiceReference;

namespace VaultUploader.Test.WSAccess
{
    [TestClass]
    public class VaultAccessorTest
    {
        public VaultAccessorTest()
        {
            InitializeCommon();
        }     
               
        private static void InitializeCommon()
        {
            string serviceURL = System.Configuration.ConfigurationManager.AppSettings["RouterServiceURL"];
            ServiceContext.Instance(serviceURL);
        }     

        [TestMethod]
        public void GetDocInfoTest()
        {
            try
            {
                ContractInfo[] results =
                    ServiceContext.Instance().VaultSvcAccessor.GetDocInfo("Manual", "123", "ICTS", new System.Collections.Generic.Dictionary<string, string>());
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
                string result =
                    ServiceContext.Instance().VaultSvcAccessor.UploadDocument("123file", "PDF", "123", "ICTS","PDF", null);
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
                byte[] result =
                    ServiceContext.Instance().VaultSvcAccessor.GetDocumentForURL("Manual", "123", "ICTS", "123filename", "docs\\manual");
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
                ContractInfo[] results =
                    ServiceContext.Instance().VaultSvcAccessor.GetVersionInfo("Manual", "123", "ICTS", "123doc", "docs\\manual");
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

