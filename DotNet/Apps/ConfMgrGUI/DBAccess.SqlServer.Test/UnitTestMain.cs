using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using OpsTrackingModel;
using System.Globalization;
using System.IO;
using WSAccess;
using DevExpress.XtraRichEdit;
using Sempra.Ops;

namespace DBAccess.SqlServer.Test
{
    [TestClass]
    public class UnitTestMain
    {
        /* 
         * 
         * Current test unit creation status. 
         *  Key: Waiting for DB work=!  Needs data=D  done=Ok  InProgress=[*]
         *  AssociatedDocsDal:      [!]PKG_INBOUND$p_update_asso_status
         *  CdtyCodeLkupDal:        Ok
         *  CdtyGroupCodesDal:      Ok
         *  ColorTranslateDal:      Ok
         *  ConfirmDocsAPIDal
         *  ConfirmMgrAPIDal
         *  CptyInfo                **In Progress**
         *  DealsheetAPIDal
         *  FaxLogSentDal:          [!]PKG_RQMT_CONFIRM$p_insert_fax_log_sent
         *  FaxLogStatusDal:        Ok
         *  InbAttribMapValDal      Ok
         *  InbAttribMapPhraseDal   Ok
         *  InboundDocCallerRefDal  [!]PKG_RQMT_CONFIRM$p_map_caller_ref, [!]PKG_RQMT_CONFIRM$p_ummap_caller_ref
         *  InboundDocsDal          [!]PKG_INBOUND$p_update_inbound_doc
         *  InboundDocUserFlagDal   [!]PKG_INBOUND_EXT$p_update_user_flag
         *  InboundFaxNosDal        Ok
         *  ImagesDal        **In Progress**
         *  RqmtDal                 Ok
         *  RqmtStatusColorsDal     Ok
         *  RqmtStatusDal           Ok
         *  TradeAuditDal           Ok
         *  TradeContractInfoDal    DEPRECATED
         *  TradeDataChgDal         DEPRECATED
         *  TradeGroupDal           Ok
         *  TradeRqmtConfirmDal     [!]PKG_RQMT_CONFIRM$p_insert_rqmt_confirm, [!]PKG_RQMT_CONFIRM$p_update_rqmt_confirm, [!]PKG_TRADE_RQMT$p_update_trade_confirm_creator
         *  TradeRqmtConfirmBlobDal
         *  TradeRqmtDal            [!]PKG_TRADE_RQMT$p_add_trade_rqmt, PKG_TRADE_RQMT$p_update_trade_rqmt
         *  TradeSummaryDal         [!]pkg_trade_summary$p_update_trade_summary_cmt, [!]pkg_trade_summary$p_update_determine_actions, [!]pkg_trade_summary$p_update_final_approval
         *  UserCompanyDal          Ok
         *  UserFiltersOpsmgrDal    Ok
         *  UserRoleDal             Ok
         *  VPcTradeRqmtDal         Ok
         *  VPcTradeSummaryDal      [!]v_pc_trade_summary
         *  
         */

        private const string PRODUCT_COMPANY = "Amphora";
        private const string PRODUCT_BRAND = "Affinity";
        private const string PRODUCT_GROUP = "Confirms";
        private const string PRODUCT_NAME = "ConfirmManager";
        private const string PRODUCT_SETTINGS = "Settings";
        private const string PRODUCT_TEMP = "Temp";
        private string appTempDir;

        private string sqlConnectionString = 
            @"Server=CNF01INFDBS01\SQLSVR11;UID=ifrankel;Password=Oracle11;Database=cnf_mgr;MultipleActiveResultSets=True;";
        private string sqlConnectionIntegratedSecurityString =
            @"Server=CNF01INFDBS01\SQLSVR11;Database=cnf_mgr;integrated security=sspi;";
        private string cptyInfoAPIUrlString = @"http://cnf01inf01:55083/CptyInfoAPI.svc";
        private int expectedCount = 0;
        private int actualCount = 0;
        private int messageSeqNo = 0;
        private string expectedValue = String.Empty;
        private string actualValue = String.Empty;

        private string getMessage(string pMessage) 
        {
            string result = String.Empty;
            messageSeqNo++;
            result = messageSeqNo.ToString() + ": " + pMessage;
            return result;
        }

        //public void UnitTestMain()
        //{
        //    string roamingFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //        PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);
        //    appTempDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_TEMP);
        //}

        [TestMethod]
        public void Test_Misc()
        {
            string testStr = @"ASH\ifrankel";
            int delimPos = testStr.IndexOf(@"\");
            string testDomain = testStr.Substring(0, delimPos);
            string actualValue = Utils.GetUserNameWithoutDomain(testStr);
            Assert.AreEqual("ifrankel", actualValue, getMessage("Domain not stripped."));
            actualValue = Utils.GetUserNameWithoutDomain(testStr);
            Assert.AreEqual("ifrankel", actualValue, getMessage("Domain not stripped."));
        }

        [TestMethod]
        public void Test_AssociatedDocsDal()
        {
            messageSeqNo = 0;
            //AssociatedDocsDal assocDocsDal = new AssociatedDocsDal(sqlConnectionString);
            AssociatedDocsDal assocDocsDal = new AssociatedDocsDal(sqlConnectionIntegratedSecurityString);
            List<AssociatedDoc> assocDocsList = new List<AssociatedDoc>();
            assocDocsList = assocDocsDal.GetAllStub();
            expectedValue = "MIZUHO";
            actualValue = assocDocsList[0].CptyShortName;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: Cpty Short name found."));
            expectedValue = "20150423_135943_199_1.tif";
            actualValue = assocDocsList[0].FileName;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: File name found."));

            expectedCount = 0;
            actualCount = assocDocsDal.GetCount(500);
            Assert.AreEqual(expectedCount, actualCount, getMessage("Actual Data: Rows found - Id only."));

            assocDocsList = assocDocsDal.GetAll();
            expectedCount = 0;
            actualCount = assocDocsList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Actual Data: Rows found - All."));

            expectedValue = "20150508_123005_580.tif";
            foreach(AssociatedDoc assocDocData in assocDocsList)
            {
                if (assocDocData.Id == 8002)
                {
                    actualValue = assocDocData.FileName;
                    break;
                }
            }
            //Assert.AreEqual(expectedValue, actualValue, getMessage("Actual Data: File name found."));
        }

        [TestMethod]
        public void Test_CdtyCodeLkupDal()
        {
            messageSeqNo = 0;
            CdtyCodeLkupDal cdtyCodeLkupDal = new CdtyCodeLkupDal(sqlConnectionString);
            List<BdtaCdtyLkup> cdtyCodeLookupList = new List<BdtaCdtyLkup>();
            cdtyCodeLookupList = cdtyCodeLkupDal.GetAllStub();
            expectedCount = 5;
            actualCount = cdtyCodeLookupList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "OIL";
            actualValue = cdtyCodeLookupList[4].CdtyCode;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: CdtyCode found."));

            expectedValue = "NGAS";
            actualValue = "";
            cdtyCodeLookupList = cdtyCodeLkupDal.GetAll();
            foreach (BdtaCdtyLkup dataValue in cdtyCodeLookupList)
            {
                if (dataValue.CdtyCode.Equals("NGAS"))
                {
                    actualValue = dataValue.CdtyCode;
                    break;
                }
            }

            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: CdtyCode found."));
        }

        [TestMethod]
        public void Test_CdtyGroupCodesDal()
        {
            messageSeqNo = 0;
            CdtyGroupCodesDal cdtyGroupCodesDal = new CdtyGroupCodesDal(sqlConnectionString);
            List<GetCdtyGroupCodesDto> cdtyGroupCodeList = new List<GetCdtyGroupCodesDto>();
            cdtyGroupCodeList = cdtyGroupCodesDal.GetAllStub();
            expectedCount = 4;
            actualCount = cdtyGroupCodeList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "OIL";
            actualValue = cdtyGroupCodeList[3].CdtyGroupCode;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: CdtyGroupCode found."));

            cdtyGroupCodeList = cdtyGroupCodesDal.GetAll();
            actualValue = "";
            foreach(GetCdtyGroupCodesDto cdtyGroupCodeData in cdtyGroupCodeList)
            {
                if (cdtyGroupCodeData.CdtyGroupCode.Equals("NGAS"))
                {
                    actualValue = cdtyGroupCodeData.CdtyGroupCode;
                }
            }
            expectedValue = "NGAS";
            Assert.AreEqual(expectedValue, actualValue, getMessage("Trade Data: CdtyGroupCode found."));
        }

        [TestMethod]
        public void Test_ColorTranslateDal()
        {
            messageSeqNo = 0;
            ColorTranslateDal colorTranslateDal = new ColorTranslateDal(sqlConnectionString);
            List<ColorTranslate> colorTranslateList = new List<ColorTranslate>();
            colorTranslateList = colorTranslateDal.GetAllStub();
            expectedCount = 6;
            actualCount = colorTranslateList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "Tomato";
            actualValue = colorTranslateList[2].CsColor;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: CsColor found."));

            colorTranslateList = colorTranslateDal.GetAll();
            Assert.AreEqual(expectedCount, colorTranslateList.Count, getMessage("Get all data count."));
            //foreach (ColorTranslate dataList in colorTranslateList) 
            //{ 
            //    if (dataList.Code.)
            //}
        }

        //WSAccess
        [TestMethod]
        public void Test_ConfirmDocsAPIDal()
        {
            //messageSeqNo = 0;
            //ConfirmDocsAPIDal confirmDocsDal = new ConfirmDocsAPIDal();
            //string inputFilename = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0009_NGAS.PHYS.LONG.FORM.docx";
            //byte[] bytes = File.ReadAllBytes(inputFilename);

            //DocumentFormat docFormat = DocumentFormat.OpenXml;
            //string outputFilename = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0009_NGAS.PHYS.LONG.FORM.pdf";
            //bool isConversionOk = false;
            //isConversionOk = WSUtils.SaveByteArrayAsPdfFile(bytes, docFormat, outputFilename);
            //Assert.IsTrue(isConversionOk, getMessage("PDF Conversion ok."));
        }

        [TestMethod]
        public void Test_ConfirmMgrAPIDal()
        {
            string baseUrl = @"http://cnf01inf01:11111";
            string userId = "affsvcprd";
            string password = "78ui&*UI";

            ConfirmMgrAPIDal confirmMgrApiDal = new WSAccess.ConfirmMgrAPIDal(baseUrl, userId, password);
            List<string> permKeyList = new List<string>();
            permKeyList.Add("AMPH 01");
            permKeyList.Add("MERC 01");
            string permKeyInClause = confirmMgrApiDal.GetPermissionKeyDBInClause(permKeyList);

        }

        [TestMethod]
        public void Test_DealsheetAPIDal()
        {
            string dealsheetUrl = @"http://cnf01inf01:11111/GetDocument.svc";
            DealsheetAPIDal dealsheetAPIdal = new DealsheetAPIDal(dealsheetUrl, "", "");
            string docFormatStr = "";
            dealsheetAPIdal.GetDealsheet("AFF", "1", out docFormatStr);
        }

        [TestMethod]
        public void Test_CptyInfoDal()
        {
            messageSeqNo = 0;
            CptyInfoDal cptyInfoDal = new CptyInfoDal(cptyInfoAPIUrlString);
            List<BdtaCptyLkup> cptySnList = new List<BdtaCptyLkup>();
            cptySnList = cptyInfoDal.GetOpenConfirmLookupStub();
            expectedCount = 6;
            actualCount = cptySnList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "MIZUHO";
            actualValue = cptySnList[2].CptySn;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: CptySn found."));

            //cptySnList = cptyInfoDal.GetOpenConfirmLookup(sqlConnectionString);
            expectedValue = "MIZUHO";
            actualValue = cptySnList[0].CptySn;
            //Assert.AreEqual(expectedValue, actualValue, getMessage("Live Data: CptySn found."));

            var cptyAgreementDataList = new List<CptyAgreement>();
            string testCpty = "MERC EN TR";
            string testBookingCo = "AMPH SWIT";
            //cptyAgreementDataList = cptyInfoDal.GetAgreementList(testCpty, testBookingCo);
            //Assert.IsTrue(cptyAgreementDataList.Count > 0, getMessage("Http call: GetAgreementList"));

            string testTradeDtStr = "08/30/2015";
            string testResult;
            //testResult = cptyInfoDal.GetAgreementDisplay(testCpty, testBookingCo, testTradeDtStr);
            //Assert.IsTrue(testResult.Length > 10, getMessage("Http call: GetAgreementDisplay"));

            CptyFaxNoDto cptyFaxNoData = new CptyFaxNoDto();
            testCpty = "TESTOTC";
            string cdtyCode = "NGAS";
            string sttlType = "PHYS";
            //cptyFaxNoData = cptyInfoDal.GetFaxNo(testCpty, cdtyCode, sttlType);
            //Assert.IsTrue(cptyFaxNoData.PhoneTypeCode.Length > 2, getMessage("Http call: GetFaxNo"));
        }

        [TestMethod]
        public void Test_FaxLogSentDal()
        {
            //Waiting for PKG_RQMT_CONFIRM$p_insert_fax_log_sent
        }

        [TestMethod]
        public void Test_FaxLogStatusDal()
        {
            messageSeqNo = 0;
            FaxLogStatusDal faxLogStatusDal = new FaxLogStatusDal(sqlConnectionString);
            List<FaxLogStatusDto> faxLogStatusList = faxLogStatusDal.GetStub();
            expectedCount = 2;
            actualCount = faxLogStatusList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "CONFIRMATIONS@MITSUI-EP.COM";
            actualValue = faxLogStatusList[0].FaxTelexNumber;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: FaxTelexNumber found."));

            faxLogStatusList = faxLogStatusDal.Get(3);
            expectedCount = 2;
            actualCount = faxLogStatusList.Count;
            actualValue = "";
            expectedValue = "S";
            foreach(FaxLogStatusDto faxLogStatusData in faxLogStatusList)
            {
                if (faxLogStatusData.Id == 9005)
                {
                    actualValue = faxLogStatusData.FaxStatus;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live Data: FaxStatus found."));
        }

        [TestMethod]
        public void Test_InbAttribMapValDal()
        {
            // Pre-Requisite -- INB_ATTRIB row, CODE = "CPTY_SN"
            //Setup
            const string INPUT_A = "TEST_IF_01";
            const string INPUT_B = "TEST_IF_02";
            const string INPUT_C = "TEST_IF_03";
            const string INPUT_D = "TEST_IF_04";
            const string ATTRIB_CODE = "CPTY_SN";
            messageSeqNo = 0;
            InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionIntegratedSecurityString);
            List<InbAttribMapValDto> resultDataList = new List<InbAttribMapValDto>();

            //Make sure the data we are about to insert doesn't already exist.
            resultDataList = inbAttribMapValDal.GetMapValues(ATTRIB_CODE);
            foreach (InbAttribMapValDto mapVal in resultDataList)
            {
                if (mapVal.MappedValue.Equals(INPUT_A) ||
                    mapVal.MappedValue.Equals(INPUT_B) ||
                    mapVal.MappedValue.Equals(INPUT_C) ||
                    mapVal.MappedValue.Equals(INPUT_D))
                inbAttribMapValDal.Delete(mapVal.Id);
            }

            resultDataList = inbAttribMapValDal.GetMapValues(ATTRIB_CODE);
            bool oldTestDataExists = false;
            foreach (InbAttribMapValDto mapVal in resultDataList)
            {
                if (mapVal.MappedValue.Equals(INPUT_A) ||
                    mapVal.MappedValue.Equals(INPUT_B) ||
                    mapVal.MappedValue.Equals(INPUT_C) ||
                    mapVal.MappedValue.Equals(INPUT_D))
                {
                    oldTestDataExists = true;
                    break;
                }
            }

            Assert.IsFalse(oldTestDataExists, getMessage("Old test data exists and was not deleted."));

            Int32 testIdInsert1 = 0;
            Int32 testIdInsert2 = 0;
            InbAttribMapValDto parmData = new InbAttribMapValDto();
            parmData.InbAttribCode = ATTRIB_CODE;
            resultDataList = inbAttribMapValDal.GetMapValues(parmData.InbAttribCode);

            //Insert -- test null value parms
            parmData.MappedValue = INPUT_A;
            //data.Descr = "DESCR";
            parmData.ActiveFlag = "Y";
            testIdInsert1 = inbAttribMapValDal.Insert(parmData);
            Assert.AreNotEqual(0, testIdInsert1, getMessage("Row inserted - non-Zero Id returned."));

            expectedValue = INPUT_A;
            resultDataList = inbAttribMapValDal.GetMapValues("CPTY_SN");
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after insert."));
            bool foundRow = false;
            foreach (InbAttribMapValDto mapVal in resultDataList) 
            {
                if (mapVal.Id.Equals(testIdInsert1))
                {
                    Assert.AreEqual(INPUT_A, mapVal.MappedValue, getMessage("MappedValue not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after insert."));

            //Insert -- test non-null value parms
            parmData.MappedValue = INPUT_B;
            parmData.Descr = INPUT_B + "- DESCR";
            parmData.ActiveFlag = "Y";
            testIdInsert2 = inbAttribMapValDal.Insert(parmData);
            Assert.AreNotEqual(0, testIdInsert2, getMessage("Row inserted - non-Zero Id returned."));

            expectedValue = INPUT_B;
            resultDataList = inbAttribMapValDal.GetMapValues("CPTY_SN");
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after insert."));
            foundRow = false;
            foreach (InbAttribMapValDto mapVal in resultDataList)
            {
                if (mapVal.Id.Equals(testIdInsert2))
                {
                    Assert.AreEqual(INPUT_B, mapVal.MappedValue, getMessage("MappedValue not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after insert."));

            //Update
            parmData.Id = testIdInsert1;
            parmData.MappedValue = INPUT_C;
            parmData.Descr = null;
            parmData.ActiveFlag = "Y";
            int rowsUpdated = inbAttribMapValDal.Update(parmData);
            resultDataList = inbAttribMapValDal.GetMapValues("CPTY_SN");
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after update."));
            foundRow = false;
            foreach (InbAttribMapValDto mapVal in resultDataList)
            {
                if (mapVal.Id.Equals(testIdInsert1))
                {
                    Assert.AreEqual(INPUT_C, mapVal.MappedValue, getMessage("MappedValue not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after update."));


            //Update
            parmData.Id = testIdInsert2;
            parmData.MappedValue = INPUT_D;
            parmData.Descr = "Test data: " + INPUT_D;
            parmData.ActiveFlag = "Y";
            rowsUpdated = inbAttribMapValDal.Update(parmData);
            resultDataList = inbAttribMapValDal.GetMapValues("CPTY_SN");
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after update."));
            foundRow = false;
            foreach (InbAttribMapValDto mapVal in resultDataList)
            {
                if (mapVal.Id.Equals(testIdInsert2))
                {
                    Assert.AreEqual(INPUT_D, mapVal.MappedValue, getMessage("MappedValue not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after update."));
            
            //Delete
            rowsUpdated = inbAttribMapValDal.Delete(testIdInsert1);
            Assert.IsTrue(rowsUpdated == 1, getMessage("Row not deleted."));
            rowsUpdated = inbAttribMapValDal.Delete(testIdInsert2);
            Assert.IsTrue(rowsUpdated == 1, getMessage("Row not deleted."));
        }

        [TestMethod]
        public void Test_InbAttribMapPhraseDal()
        {
            const string BASE_MAPPED_VALUE = "TEST_IF";
            messageSeqNo = 0;
            // Pre-Requisite -- INB_ATTRIB row, CODE = "CPTY_SN"
            // INB_ATTRIB_MAP_VAL: ID = 1, MAPPED_VALUE = "TEST_IF"

            InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionString);
            List<InbAttribMapValDto> resultMapValList = new List<InbAttribMapValDto>();
            InbAttribMapValDto mapValData = new InbAttribMapValDto();
            mapValData.InbAttribCode = "CPTY_SN";
            mapValData.MappedValue = BASE_MAPPED_VALUE;
            mapValData.Descr = BASE_MAPPED_VALUE + "- DESCR";
            mapValData.ActiveFlag = "Y";

            //Make sure TEST_IF exists in INB_ATTRB table
            resultMapValList = inbAttribMapValDal.GetMapValues(mapValData.InbAttribCode);
            bool foundRow = false;
            foreach (InbAttribMapValDto mapVal in resultMapValList)
            {
                if (mapVal.MappedValue.Equals(BASE_MAPPED_VALUE))
                {
                    mapValData.Id = mapVal.Id;
                    foundRow = true;
                    break;
                }
            }
            if (!foundRow)
            {
                mapValData.Id = inbAttribMapValDal.Insert(mapValData);
            }
            Assert.IsTrue(mapValData.Id > 0, getMessage("Attrib Map Val row not found."));

            InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionString);
            List<InbAttribMapPhraseDto> resultDataList = new List<InbAttribMapPhraseDto>();
            List<InbAttribMapComboDto> resultComboList = new List<InbAttribMapComboDto>();
            InbAttribMapPhraseDto parmData = new InbAttribMapPhraseDto();

            //Make sure the data we are about to insert doesn't already exist.
            resultDataList = inbAttribMapPhraseDal.GetPhrases(mapValData.Id);
            int oldTestDataRowsDeleted = 0;
            foreach (InbAttribMapPhraseDto mapPhrase in resultDataList)
            {
                inbAttribMapPhraseDal.Delete(mapPhrase.Id);
                oldTestDataRowsDeleted++;
            }
            bool oldTestDataExists = resultDataList.Count != oldTestDataRowsDeleted;
            Assert.IsFalse(oldTestDataExists, getMessage("Old test data exists and was not deleted."));

            //Main test routine
            const string INPUT_A = "TEST_IF_01";
            const string INPUT_B = "TEST_IF_02";
            const string INPUT_C = "TEST_IF_03";
            const string INPUT_D = "TEST_IF_04";
            Int32 testIdInsert1 = 0;
            Int32 testIdInsert2 = 0;

            //Insert -- test null value parms
            parmData.InbAttribMapValId = mapValData.Id;
            parmData.Phrase = INPUT_A;
            parmData.ActiveFlag = "Y";

            testIdInsert1 = inbAttribMapPhraseDal.Insert(parmData);
            Assert.AreNotEqual(0, testIdInsert1, getMessage("Row inserted - non-Zero Id returned."));

            expectedValue = INPUT_A;
            resultDataList = inbAttribMapPhraseDal.GetPhrases(parmData.InbAttribMapValId);
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after insert."));
            foundRow = false;
            foreach (InbAttribMapPhraseDto mapPhrase in resultDataList)
            {
                if (mapPhrase.Id.Equals(testIdInsert1))
                {
                    Assert.AreEqual(INPUT_A, mapPhrase.Phrase, getMessage("MappedPhrase not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after insert."));

            //Insert -- test non-null value parms
            parmData.InbAttribMapValId = mapValData.Id;
            parmData.Phrase = INPUT_B;
            parmData.ActiveFlag = "Y";
            testIdInsert2 = inbAttribMapPhraseDal.Insert(parmData);
            Assert.AreNotEqual(0, testIdInsert2, getMessage("Row inserted - non-Zero Id returned."));

            expectedValue = INPUT_B;
            resultComboList = inbAttribMapPhraseDal.GetPhrases("TEST_IF");
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after insert."));
            foundRow = false;
            foreach (InbAttribMapComboDto mapPhrase in resultComboList)
            {
                if (mapPhrase.PhraseId.Equals(testIdInsert2))
                {
                    Assert.AreEqual(INPUT_B, mapPhrase.Phrase, getMessage("MappedPhrase not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after insert."));

            //Update
            parmData.InbAttribMapValId = mapValData.Id;
            parmData.Id = testIdInsert1;
            parmData.Phrase = INPUT_C;
            parmData.ActiveFlag = "Y";
            int rowsUpdated = inbAttribMapPhraseDal.Update(parmData);
            resultDataList = inbAttribMapPhraseDal.GetPhrases(parmData.InbAttribMapValId);
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after update."));
            foundRow = false;
            foreach (InbAttribMapPhraseDto mapPhrase in resultDataList)
            {
                if (mapPhrase.Id.Equals(testIdInsert1))
                {
                    Assert.AreEqual(INPUT_C, mapPhrase.Phrase, getMessage("MappedPhrase not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after update."));

            //Update
            parmData.InbAttribMapValId = mapValData.Id;
            parmData.Id = testIdInsert2;
            parmData.Phrase = INPUT_D;
            parmData.ActiveFlag = "Y";
            rowsUpdated = inbAttribMapPhraseDal.Update(parmData);
            resultDataList = inbAttribMapPhraseDal.GetPhrases(parmData.InbAttribMapValId);
            Assert.IsTrue(resultDataList.Count > 0, getMessage("No rows returned after update."));
            foundRow = false;
            foreach (InbAttribMapPhraseDto mapPhrase in resultDataList)
            {
                if (mapPhrase.Id.Equals(testIdInsert2))
                {
                    Assert.AreEqual(INPUT_D, mapPhrase.Phrase, getMessage("MappedPhrase not found."));
                    foundRow = true;
                    break;
                }
            }
            Assert.IsTrue(foundRow, getMessage("Row not found after update."));

            //Delete
            rowsUpdated = inbAttribMapPhraseDal.Delete(testIdInsert1);
            Assert.IsTrue(rowsUpdated == 1, getMessage("Row not deleted."));
            rowsUpdated = inbAttribMapPhraseDal.Delete(testIdInsert2);
            Assert.IsTrue(rowsUpdated == 1, getMessage("Row not deleted."));
        }

        [TestMethod]
        public void Test_InboundDocCallerRefDal()
        {

        }

        [TestMethod]
        public void Test_InboundDocsDal()
        {
            messageSeqNo = 0;
            InboundDocsDal inboundDocsDal = new InboundDocsDal(sqlConnectionString);
            List<InboundDocsView> inboundDocsViewList = new List<InboundDocsView>();
            inboundDocsViewList = inboundDocsDal.GetAllStub();
            expectedValue = @"email\US\15-04-23_02_contract.pdf";
            actualValue = inboundDocsViewList[2].CallerRef;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: CallerRef found."));
            expectedValue = "15";
            actualValue = inboundDocsViewList[4].Id.ToString();
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stubbed: Id found."));

            inboundDocsViewList = inboundDocsDal.GetAll();
            expectedValue = "20150521_095410_430.tif";
            foreach (InboundDocsView inboundDocsData in inboundDocsViewList)
            {
                if (inboundDocsData.Id == 9004)
                {
                    actualValue = inboundDocsData.FileName;

                }
            }
            Assert.AreEqual(expectedValue, actualValue, "Selected Inbound filename.");

            InboundDocsDto inbDocsDto = new InboundDocsDto();
            inbDocsDto.Id = 0;
            inbDocsDto.CallerRef = @"scan\SampleInbound_01-15.tif";
            inbDocsDto.SentTo = "scan";
            inbDocsDto.RcvdTs = DateTime.ParseExact("08-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture);
            inbDocsDto.FileName = "20150814_115410_520.tif";
            inbDocsDto.Sender = null;
            inbDocsDto.Cmt = null;
            inbDocsDto.DocStatusCode = "OPEN";
            inbDocsDto.HasAutoAsctedFlag = "N";
            inbDocsDto.ProcFlag = "Y";
            inbDocsDto.MappedCptySn = null;
            inbDocsDto.MappedBrkrSn = null;
            inbDocsDto.MappedCdtyCode = null;
            inbDocsDto.JobRef = null;

            Int32 idVal = 0;
            idVal = inboundDocsDal.Insert(inbDocsDto);

            //idVal = 20;
            Assert.AreNotEqual(0, idVal, "Row was inserted");
            inboundDocsViewList = inboundDocsDal.GetAll();
            expectedValue = inbDocsDto.FileName;
            foreach (InboundDocsView inboundDocsData in inboundDocsViewList)
            {
                if (inboundDocsData.Id == idVal)
                {
                    actualValue = inboundDocsData.FileName;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, "Selected Inbound filename.");

            int rowsUpdated = 0;
            rowsUpdated = inboundDocsDal.UpdateStatus(idVal, "CLOSED");
            expectedCount = 1;
            Assert.AreEqual(expectedCount, rowsUpdated, "Updated single row.");

            rowsUpdated = 0;
            rowsUpdated = inboundDocsDal.UpdateStatus(idVal, "OPEN");
            Assert.AreEqual(expectedCount, rowsUpdated, "Updated single row again.");

            Dictionary<int, string> inbDict = new Dictionary<int, string>();
            string docStatus = "CLOSED";
            inbDict.Add(idVal, docStatus);
            idVal = inboundDocsDal.Insert(inbDocsDto);
            //idVal = 21;
            inbDict.Add(idVal, docStatus);
            idVal = inboundDocsDal.Insert(inbDocsDto);
            //idVal = 22;
            inbDict.Add(idVal, docStatus);

            Dictionary<int, int> rowsUpdatedDict = new Dictionary<int, int>();
            rowsUpdatedDict = inboundDocsDal.UpdateStatus(inbDict);

            expectedCount = 3;
            actualCount = rowsUpdatedDict.Count;
            Assert.AreEqual(expectedCount, actualCount, "Update status updated correct number of rows.");

            int rowsDeleted = 0;
            actualCount = 0;
            foreach (var dictRow in inbDict)
            {
                rowsDeleted = inboundDocsDal.Delete(dictRow.Key);
                actualCount = actualCount + rowsDeleted;
            }
            Assert.AreEqual(inbDict.Count, actualCount, "Deleted correct number of rows.");
        }

        [TestMethod]
        public void Test_InboundDocUserFlagDal()
        {

        }

        [TestMethod]
        public void Test_InboundFaxNosDal()
        {
            messageSeqNo = 0;
            InboundFaxNosDal inboundFaxNosDal = new InboundFaxNosDal(sqlConnectionString);
            List<InboundFaxNosDto> inboundFaxNosList = new List<InboundFaxNosDto>();
            inboundFaxNosList = inboundFaxNosDal.GetAllStub();
            expectedCount = 4;
            actualCount = inboundFaxNosList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "Stamford";
            actualValue = "";
            inboundFaxNosList = inboundFaxNosDal.GetAll();
            foreach (InboundFaxNosDto dataValue in inboundFaxNosList)
            {
                if (dataValue.Faxno.Equals("Stamford"))
                {
                    actualValue = dataValue.Faxno;
                    break;
                }
            }

            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: FaxNo found."));
        }

        [TestMethod]
        public void Test_ImagesDal()
        {
            messageSeqNo = 0;
            const string TEST_DOC_01 = @"C:\Users\ifrankel\AppDev\VS2013Projects\DocFlow\DBAccess.SqlServer.Test\TestData\Test_Image_01.tif";
            const string TEST_DOC_02 = @"C:\Users\ifrankel\AppDev\VS2013Projects\DocFlow\DBAccess.SqlServer.Test\TestData\Test_Image_02.tif";
            const string TEST_DOC_03 = @"C:\Users\ifrankel\AppDev\VS2013Projects\DocFlow\DBAccess.SqlServer.Test\TestData\Test_Image_03.tif";

            ImagesDal inbImgsDal = new ImagesDal(sqlConnectionIntegratedSecurityString);            
            ImagesDto inbImgsData = new ImagesDto(9001, null, "TIF",  null, "TIF", ImagesDtoType.Inbound );            
            Int32 imageId = inbImgsDal.Insert(TEST_DOC_01, TEST_DOC_01, inbImgsData);
            Assert.IsTrue(imageId > 0, getMessage("Insert Test Image."));
        }

        [TestMethod]
        public void Test_RqmtDal()
        {
            messageSeqNo = 0;
            RqmtDal rqmtDal = new RqmtDal(sqlConnectionString);
            List<RqmtView> rqmtList = new List<RqmtView>();
            rqmtList = rqmtDal.GetAllStub();
            expectedCount = 9;
            actualCount = rqmtList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "Cpty Paper";
            actualValue = "";
            rqmtList = rqmtDal.GetAll();
            foreach (RqmtView dataValue in rqmtList)
            {
                if (dataValue.Code.Equals("XQCCP"))
                {
                    actualValue = dataValue.DisplayText;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Rqmt found."));

            expectedValue = "No Cpty Confirm";
            actualValue = "";
            rqmtList = rqmtDal.GetAll();
            foreach (RqmtView dataValue in rqmtList)
            {
                if (dataValue.Code.Equals("NOCNF"))
                {
                    actualValue = dataValue.DisplayText;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Rqmt found."));
        }

        [TestMethod]
        public void Test_RqmtStatusColorsDal()
        {
            messageSeqNo = 0;
            RqmtStatusColorsDal rqmtStatusColorsDal = new RqmtStatusColorsDal(sqlConnectionString);
            List<RqmtStatusColor> rqmtStatusColorList = new List<RqmtStatusColor>();
            rqmtStatusColorList = rqmtStatusColorsDal.GetAllStub();
            expectedCount = 21;
            actualCount = rqmtStatusColorList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            expectedValue = "Gold";
            actualValue = "";
            rqmtStatusColorList = rqmtStatusColorsDal.GetAll();
            foreach (RqmtStatusColor dataValue in rqmtStatusColorList)
            {
                if (dataValue.HashKey.Equals("XQCSPOK_TO_SEND"))
                {
                    actualValue = dataValue.CsColor;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Color found."));

            expectedValue = "LightGreen";
            actualValue = "";
            rqmtStatusColorList = rqmtStatusColorsDal.GetAll();
            foreach (RqmtStatusColor dataValue in rqmtStatusColorList)
            {
                if (dataValue.HashKey.Equals("XQCSPAPPR"))
                {
                    actualValue = dataValue.CsColor;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Color found."));
        }

        [TestMethod]
        public void Test_RqmtStatusDal()
        {
            messageSeqNo = 0;
            RqmtStatusDal rqmtStatusDal = new RqmtStatusDal(sqlConnectionString);
            List<RqmtStatusView> rqmtStatusList = new List<RqmtStatusView>();
            rqmtStatusList = rqmtStatusDal.GetAllStub();
            expectedCount = 21;
            actualCount = rqmtStatusList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all stub count."));

            rqmtStatusList = rqmtStatusDal.GetAll();
            expectedCount = 40;
            actualCount = rqmtStatusList.Count;
            Assert.AreEqual(expectedCount, actualCount, getMessage("Get all live count."));

            expectedValue = "LightGray";
            actualValue = "";
            foreach (RqmtStatusView dataValue in rqmtStatusList)
            {
                if (dataValue.RqmtCode.Equals("XQCCP") && dataValue.StatusCode.Equals("CXL"))
                {
                    actualValue = dataValue.ColorCode;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Rqmt Status found."));

            expectedValue = "Gold";
            actualValue = "";
            rqmtStatusList = rqmtStatusDal.GetAll();
            foreach (RqmtStatusView dataValue in rqmtStatusList)
            {
                if (dataValue.RqmtCode.Equals("XQCSP") && dataValue.StatusCode.Equals("OK_TO_SEND"))
                {
                    actualValue = dataValue.ColorCode;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Rqmt Status found."));
        }

        [TestMethod]
        public void Test_TradeAuditDal()
        {
            messageSeqNo = 0;
            TradeAuditDal tradeAuditDal = new TradeAuditDal(sqlConnectionString);
            List<TradeAuditDto> tradeAuditList = new List<TradeAuditDto>();
            int tradeId = 1;
            tradeAuditList = tradeAuditDal.GetTradeAudit(tradeId);
            expectedCount = 19;
            actualCount = tradeAuditList.Count;
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Trade audit rows found."));

        }

        [TestMethod]
        public void Test_TradeContractInfoDal()
        {

        }

        [TestMethod]
        public void Test_TradeDataChgDal()
        {

        }

        [TestMethod]
        public void Test_TradeGroupDal()
        {
            const string TEST_XREF = "TEST_GROUP_01";
            messageSeqNo = 0;
            TradeGroupDal tradeGroupDal = new TradeGroupDal(sqlConnectionString);
            tradeGroupDal.CleanTestData(TEST_XREF);

            List<TradeGroupDto> tradeGroupList = new List<TradeGroupDto>();
            TradeGroupDto tradeGroupData = new TradeGroupDto();
            tradeGroupData.TradeId = 10001;
            tradeGroupData.Xref = TEST_XREF;
            tradeGroupList.Add(tradeGroupData);

            tradeGroupData = new TradeGroupDto();
            tradeGroupData.TradeId = 10002;
            tradeGroupData.Xref = TEST_XREF;
            tradeGroupList.Add(tradeGroupData);

            tradeGroupData = new TradeGroupDto();
            tradeGroupData.TradeId = 10003;
            tradeGroupData.Xref = TEST_XREF;
            tradeGroupList.Add(tradeGroupData);

            expectedCount = 3;
            actualCount = tradeGroupDal.Group(tradeGroupList);
            Assert.AreEqual(expectedCount, actualCount, "Group count.");

            List<Int32> tradeIdList = new List<int>();
            tradeIdList.Add(10001);
            tradeIdList.Add(10002);
            tradeIdList.Add(10003);
            actualCount = tradeGroupDal.Ungroup(tradeIdList);
            Assert.AreEqual(expectedCount, actualCount, "Ungroup count.");
        }

        [TestMethod]
        public void Test_TradeRqmtConfirmDal()
        {
            messageSeqNo = 0;
            TradeRqmtConfirmDal tradeRqmtConfirmDal = new TradeRqmtConfirmDal(sqlConnectionString);
            List<TradeRqmtConfirm> rqmtConfirmDataList = new List<TradeRqmtConfirm>();
            rqmtConfirmDataList = tradeRqmtConfirmDal.GetAll();
            expectedCount = 2;
            actualCount = rqmtConfirmDataList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            TradeRqmtConfirm trcData = new TradeRqmtConfirm();
            trcData.Id = 9;
            trcData.RqmtId = 10;
            trcData.TradeId = 15;
            trcData.TemplateName = "";
            trcData.ConfirmCmt = "Test...";
            trcData.FaxTelexInd = "";
            trcData.FaxTelexNumber = "";
            trcData.ConfirmLabel = "CONFIRM";
            trcData.NextStatusCode = "";
            trcData.ActiveFlag = "Y";


            tradeRqmtConfirmDal.Update(trcData);
        }

        [TestMethod]
        public void Test_TradeRqmtConfirmBlobDal()
        {
            string testImagesSqlConnStr =
                    @"Server=CNF01INFDBS01\SQLSVR11;Database=TestImage;integrated security=sspi;";
            messageSeqNo = 0;
            TradeRqmtConfirmBlobDal tradeRqmtConfirmBlobDal = new TradeRqmtConfirmBlobDal(testImagesSqlConnStr);
            //List<TradeRqmtConfirm> rqmtConfirmDataList = new List<TradeRqmtConfirm>();
            //rqmtConfirmDataList = tradeRqmtConfirmDal.GetAll();
            //expectedCount = 3;
            //actualCount = rqmtConfirmDataList.Count;
            //Assert.AreEqual(expectedCount, actualCount, "Selected list count.");
            Int32 imageId = 19;
            byte[] imageBytes;
            string imageFileName = @"C:\Users\ifrankel\DevTools\Sample Docs\20140929_055012_077_1_tif.tif";
            string DOCX_TESTDOC_2 = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0039_OIL.SWAP.FLO.FLO.ISDA.PARTY.B.docx";

            imageBytes = File.ReadAllBytes(DOCX_TESTDOC_2);
            //tradeRqmtConfirmBlobDal.TestOverwriteFilestream(imageId, imageBytes);

            string descr = "Test byte stream insert -- TestUpdateInboundImages- docx";
            string imageExt = "DOCX";
            //tradeRqmtConfirmBlobDal.TestUpdateInboundImages(imageId, descr, imageExt, imageBytes);
            tradeRqmtConfirmBlobDal.TestInsertDocImageByteArray(imageId, descr, imageBytes, imageExt);
        }


        [TestMethod]
        public void Test_TradeRqmtDal()
        {
            messageSeqNo = 0;
            TradeRqmtDal tradeRqmtDal = new TradeRqmtDal(sqlConnectionString);
            List<TradeRqmtDto> dataList = new List<TradeRqmtDto>();

            //Development test to test date handling routines.
            int tradeId = 1;
            dataList = tradeRqmtDal.GetTradeRqmts(tradeId);
            TradeRqmtDto resultRow = new TradeRqmtDto();
            foreach (TradeRqmtDto data in dataList)
            {
                resultRow.Id = data.Id;
                resultRow.TradeId = data.TradeId;
                resultRow.RqmtTradeNotifyId = data.RqmtTradeNotifyId;
                resultRow.RqmtCode = data.RqmtCode; 
                resultRow.StatusCode = data.StatusCode;
                resultRow.CompletedDt = data.CompletedDt;
                resultRow.CompletedTimestampGmt = data.CompletedTimestampGmt;
                resultRow.Reference = data.Reference;
                resultRow.CancelTradeNotifyId = data.CancelTradeNotifyId;
                resultRow.Cmt = data.Cmt;
                resultRow.SecondCheckFlag = data.SecondCheckFlag;
            }
        }

        [TestMethod]
        public void Test_TradeSummaryDal()
        {

        }

        [TestMethod]
        public void Test_UserCompanyDal()
        {
            messageSeqNo = 0;
            UserCompanyDal userCompanyDal = new UserCompanyDal(sqlConnectionString);
            List<UserCompanyDto> userCompanyList = new List<UserCompanyDto>();
            userCompanyList = userCompanyDal.GetAllStub();
            expectedCount = 4;
            actualCount = userCompanyList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "AMPH US";
            actualValue = userCompanyList[0].CompanySn;
            Assert.AreEqual(expectedValue, actualValue, "Selected company.");

            userCompanyList = null;
            userCompanyList = userCompanyDal.GetAll("IFRANKEL");
            expectedCount = 1;
            actualCount = userCompanyList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "AMPH US";
            actualValue = userCompanyList[0].CompanySn;
            Assert.AreEqual(expectedValue, actualValue, "Selected company.");
        }
        
        [TestMethod]
        public void Test_UserFiltersOpsmgrDal()
        {
            messageSeqNo = 0;
            UserFiltersOpsmgrDal userFiltersDal = new UserFiltersOpsmgrDal(sqlConnectionString);
            List<UserFiltersOpsmgrDto> userFiltersList = new List<UserFiltersOpsmgrDto>();
            userFiltersList = userFiltersDal.GetAll("IFRANKEL");
            //expectedCount = 6;
            //actualCount = userFiltersList.Count;
 
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");
            expectedValue = "Need Contracts Assigned";
            foreach (UserFiltersOpsmgrDto filterData in userFiltersList)
            {
                if (filterData.Id == 9002)
                {
                    actualValue = filterData.Descr;

                }
            }
            Assert.AreEqual(expectedValue, actualValue, "Selected Filter Description.");

            UserFiltersOpsmgrDto userFiltersData = new UserFiltersOpsmgrDto();
            userFiltersData.UserId = "IFRANKEL";
            userFiltersData.Descr = "TEST UNIT 01";
            userFiltersData.FilterExpr = "[SetcMeth] = 'Our Paper' And [SetcStatus] = 'OK_TO_SEND'";
            Int32 filterId = 0;
            filterId = userFiltersDal.Insert(userFiltersData);
            Assert.AreNotEqual(0, filterId, "Row was inserted");

            userFiltersData = userFiltersDal.Get(filterId);
            expectedValue = "TEST UNIT 01";
            actualValue = userFiltersData.Descr;
            Assert.AreEqual(expectedValue, actualValue, "Inserted Filter Row.");

            userFiltersData.Id = filterId;
            userFiltersData.UserId = "IFRANKEL";
            userFiltersData.Descr = "TEST UNIT 99";
            userFiltersData.FilterExpr = "[BkrMeth] = 'Broker Paper'";
            int rowsUpdated = 0;
            rowsUpdated = userFiltersDal.Update(userFiltersData);
            Assert.AreEqual(1, rowsUpdated, "User Filter Row Updated.");

            userFiltersData = userFiltersDal.Get(filterId);
            expectedValue = "TEST UNIT 99";
            actualValue = userFiltersData.Descr;
            Assert.AreEqual(expectedValue, actualValue, "User Filter Descr Updated.");

            expectedValue = "[BkrMeth] = 'Broker Paper'";
            actualValue = userFiltersData.FilterExpr;
            Assert.AreEqual(expectedValue, actualValue, "User Filter FilterExpr Updated.");

            int rowsDeleted = 0;
            rowsDeleted = userFiltersDal.Delete(filterId);
            Assert.AreEqual(1, rowsDeleted, "User Filter Row Deleted.");

            userFiltersData = userFiltersDal.Get(10);
            Assert.AreEqual(0, userFiltersData.Id, "User Filter Row Successfully Deleted.");
        }

        [TestMethod]
        public void Test_UserRoleDal()
        {
            messageSeqNo = 0;
            UserRoleDal userRoleDal = new UserRoleDal(sqlConnectionString);
            List<UserRoleView> userRoleList = new List<UserRoleView>();
            userRoleList = userRoleDal.GetAllStub();
            expectedCount = 8;
            actualCount = userRoleList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "CNTRCT-APP";
            actualValue = userRoleList[1].RoleCode;
            Assert.AreEqual(expectedValue, actualValue, "Selected Role.");

            userRoleList = null;
            userRoleList = userRoleDal.GetAll("JVEGA");
            expectedCount = 5;
            actualCount = userRoleList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "FNAPP";
            actualValue = userRoleList[2].RoleCode;
            Assert.AreEqual(expectedValue, actualValue, "Selected Role.");
        }

        [TestMethod]
        public void Test_VPcTradeRqmtDal()
        {
            messageSeqNo = 0;
            VPcTradeRqmtDal vpcTradeRqmtDal = new VPcTradeRqmtDal(sqlConnectionString);
            List<RqmtData> rqmtDataList = new List<RqmtData>();
            rqmtDataList = vpcTradeRqmtDal.GetAllStub();
            expectedCount = 3;
            actualCount = rqmtDataList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "Our Paper";
            foreach(RqmtData rqmtData in rqmtDataList)
            {
                if (rqmtData.Id == 279970)
                {
                    actualValue = rqmtData.DisplayText;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stub data: Rqmt Display Text found."));

            rqmtDataList = vpcTradeRqmtDal.GetAll();
            expectedValue = "XQCCP";
            foreach (RqmtData rqmtData in rqmtDataList)
            {
                if (rqmtData.Id == 281761)
                {
                    actualValue = rqmtData.Rqmt;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Rqmt Display Text found."));
        }

        [TestMethod]
        public void Test_VPcTradeSummaryDal()
        {
            messageSeqNo = 0;
            VPcTradeSummaryDal vpcTradeSummaryDal = new VPcTradeSummaryDal(sqlConnectionString);
            List<SummaryData> tradeSummaryList = new List<SummaryData>();
            tradeSummaryList = vpcTradeSummaryDal.GetAllStub();
            expectedCount = 3;
            actualCount = tradeSummaryList.Count;
            Assert.AreEqual(expectedCount, actualCount, "Selected list count.");

            expectedValue = "COLUMBIA-TCO";
            foreach (SummaryData tradeSummaryData in tradeSummaryList)
            {
                if (tradeSummaryData.Id == 155037)
                {
                    actualValue = tradeSummaryData.LocationSn;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Stub data: Trade Summary Text found."));

            tradeSummaryList = vpcTradeSummaryDal.GetAll("THIS DEFINATLEY WILL FAIL BUT AT LEAST IT WILL COMPILE");
            expectedValue = "ELEC";
            foreach (SummaryData tradeSummaryData in tradeSummaryList)
            {
                if (tradeSummaryData.Id == 2)
                {
                    actualValue = tradeSummaryData.CdtyCode;
                    break;
                }
            }
            Assert.AreEqual(expectedValue, actualValue, getMessage("Live data: Trade Summary Text found."));

        }


    }
}
