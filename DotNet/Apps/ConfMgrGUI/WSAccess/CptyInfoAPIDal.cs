using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace WSAccess
{
    public class CptyInfoAPIDal : ICptyInfoAPIDal
    {

        //private string sqlConnStr = "";
        private string cptyInfoAPIServiceUrlStr = "";
        private const string GET_AGREEMENT_LIST = "GetAgreementList";
        private const string GET_CONFIRM_SENDTO = "GetConfirmSendTo";

        public CptyInfoAPIDal(string pCptyInfoAPIUrl)
        {
            cptyInfoAPIServiceUrlStr = pCptyInfoAPIUrl;
        }

        #region Stub Data

        //public CptyInfo GetInfoStub()
        //{
        //    var result = new CptyInfo();
        //    result.CptyShortName = "TESTOTC";
        //    result.CptyLegalName = "TEST OTC COUNTERPARTY";
        //    result.CptyAddress1 = "555 WEST STREET";
        //    result.CptyAddress2 = "SUITE 400";
        //    result.CptyAddress3 = "";
        //    result.CptyCity = "JERSEY CITY";
        //    result.CptyState = "NJ";
        //    result.CptyZipcode = "07087";
        //    result.CptyCountry = "US";
        //    result.CptyMainFaxCntryCode = "";
        //    result.CptyMainFaxAreaCode = "";
        //    result.CptyMainFax = "";
        //    result.CptyMainPhoneCntryCode = "";
        //    result.CptyMainPhoneAreaCode = "201";
        //    result.CptyMainPhone = "918-4758";
        //    result.CptyAgreements = new List<CptyAgreement>();
        //    result.ContractFaxNumbers = new List<ContractFaxNo>();
        //    return result;
        //}

        public List<CptyAgreement> GetAgreementListStub()
        {
            var result = new List<CptyAgreement>();
            result.Add(new CptyAgreement
            {
                AgrmntTypeCode = "ISDA",
                StatusInd = "E",
                DateSigned = "03-SEP-2013",
                TerminationDt = "",
                SeAgrmntContactName = "TURNER, CHRISTOPHER",
                Cmt = "",
                AgreementId = 15465,
                SeCptyId = 42767,
                CptyId = 42725,
                CptyShortName = "TESTOTC",
                SeCptyShortName = "BTGPC US"
            });
            result.Add(new CptyAgreement
            {
                AgrmntTypeCode = "BRENT",
                StatusInd = "E",
                DateSigned = "15-AUG-2014",
                TerminationDt = "",
                SeAgrmntContactName = "Public, John",
                Cmt = "",
                AgreementId = 15867,
                SeCptyId = 42767,
                CptyId = 42725,
                CptyShortName = "TESTOTC",
                SeCptyShortName = "BTGPC US"
            });
            return result;
        }

        public string GetAgreementDisplayStub()
        {
            string result = "BTGPC US(ISDA:N):03-SEP-2013,BTGPC US(BRENT):15-AUG-2014";
            return result;
        }

        //public List<ContractFaxNo> GetFaxNoListStub()
        //{
        //    var result = new List<ContractFaxNo>();
        //    result.Add(new ContractFaxNo
        //    {
        //        ActiveFlag = "A",
        //        AreaCode = "203",
        //        CountryPhoneCode = "",
        //        CptyId = 42725,
        //        Description = "default fax/telex: all contracts",
        //        DesignationCode = "CF",
        //        DsgActiveFlag = "A",
        //        LocalNumber = "349-7523",
        //        PhoneId = 52876,
        //        PhoneTypeCode = "PHONE",
        //        ShortName = "TESTOTC"
        //    });

        //    return result;
        //}

        //public CptyFaxNoDto GetFaxNoStub()
        //{
        //    CptyFaxNoDto result = new CptyFaxNoDto();
        //    result.PhoneTypeCode = "PHONE";
        //    result.FaxTelexInd = "P";
        //    result.CountryPhoneCode = "";
        //    result.AreaCode = "203";
        //    result.LocalNumber = "349-7523";
        //    return result;
        //}

        public List<BdtaCptyLkup> GetOpenConfirmLookupStub()
        {
            //string dateStr = "";
            var result = new List<BdtaCptyLkup>();
            result.Add(new BdtaCptyLkup { CptySn = "DBLONDON" });
            result.Add(new BdtaCptyLkup { CptySn = "JPM SECURI" });
            result.Add(new BdtaCptyLkup { CptySn = "MIZUHO" });
            result.Add(new BdtaCptyLkup { CptySn = "MMGS INC" });
            result.Add(new BdtaCptyLkup { CptySn = "PJM" });
            result.Add(new BdtaCptyLkup { CptySn = "TESTOTC" });
            return result;
        }


        #endregion

        //Called from: frmEditContracts.ViewCptyInfo
        //Returned from: ConfirmProcessor.getCptyInfo
        //Data source: 
        //Cpty Info:
        //"select short_name,legal_name,str_addr_1,str_addr_2, " +
        //"str_addr_3,city,state_prov_code, country_code,postal_code  " +
        //" from cpty.v_cpty_address where short_name=?";
        //Cpty Phone no:
        //"select phone_type_code,country_phone_code,area_code,local_number " +
        //" from cpty.v_cpty_phone_fax where short_name = ?";						
        //public CptyInfo GetInfo(string pShortName)
        //{
        //    var result = new CptyInfo();
        //    result = GetInfoStub();

        //    return result;
        //}

        //Called from frmEditContract.ViewCptyInfo
        //Returned from: confirmationService.getCptyAgreementList(cptyAgreements);
        //Data source: 
        //"select agrmnt_type_code,status_ind,date_signed,termination_dt,se_agrmnt_contact_name," +
        //"cmt,id,se_cpty_id,cpty_id,se_cpty_sn,cpty_sn " +
        //"from cpty.v_cpty_agreements " +
        //"where cpty_sn = ?";
        public List<CptyAgreement> GetAgreementList(string pCptySN, string pBookingCoSN)
        {
            var result = new List<CptyAgreement>();
            string xmlResult = String.Empty;
            string[] parms = new string[] { pCptySN, pBookingCoSN };
            string urlStr = WSUtils.getUrlStr(cptyInfoAPIServiceUrlStr, GET_AGREEMENT_LIST, parms);
            xmlResult = WSUtils.getWebServiceUrlResult(urlStr);

            XmlDocument xDoc = new XmlDocument();
            if (xmlResult.Length > 1)
            {
                xDoc.LoadXml(xmlResult);
                //string filename = @"C:\Users\ifrankel\AppDev\VS2013Projects\CptyInfoAPIServices\CptyInfoAPIService\XML\Error\CptyInfoResponse.xml";
                //xDoc.Load(filename);

                XmlNodeList agreements = xDoc.GetElementsByTagName("Agreement");
                if (agreements.Count > 1)
                    foreach (XmlNode node in agreements)
                    {
                        CptyAgreement cptyAgrData = new CptyAgreement();
                        cptyAgrData.AgrmntTypeCode = node["AgrmntType"].InnerText;
                        cptyAgrData.StatusInd = node["StatusInd"].InnerText;
                        cptyAgrData.DateSigned = node["DateSigned"].InnerText;
                        cptyAgrData.TerminationDt = node["TerminationDt"].InnerText;
                        cptyAgrData.SeCptyShortName = node["BookingCoSn"].InnerText;
                        cptyAgrData.CptyShortName = node["CptySn"].InnerText;
                        result.Add(cptyAgrData);
                    }
            }

            return result;
        }

        //Called from: frmMain.GetAgreementInfo [line: 9045]
        //Returned from: ConfirmProcessor.getCptyAgreements
        //Data source: cpty.pkg_contracts.f_get_cpty_agreement(pCptySN, pTradeDt)}
        public string GetAgreementDisplay(string pCptySN, string pBookingCoSN, string pTradeDt)
        {
            const string DATE_FORMAT = "dd-MMM-yyyy";
            const string CNFMGR_DATE_FORMAT = "MM/dd/yyyy";
            string result = String.Empty;
            DateTime tradeDt = new DateTime();
            if (!DateTime.TryParseExact(pTradeDt, CNFMGR_DATE_FORMAT,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out tradeDt))
                return "**Invalid Trade Date: " + pTradeDt;

            DateTime dateSigned = new DateTime();
            DateTime terminationDate = new DateTime();
            bool isDateSignedOk;
            bool isTerminationDateOk;
            bool isSecondRow = false;
            List<CptyAgreement> agreementList = new List<CptyAgreement>();
            agreementList = GetAgreementList(pCptySN, pBookingCoSN);
            foreach (CptyAgreement agreementData in agreementList)
            {
                dateSigned = DateTime.MinValue;
                if (agreementData.DateSigned.Length > 1)
                    isDateSignedOk = DateTime.TryParseExact(agreementData.DateSigned, DATE_FORMAT,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateSigned);

                terminationDate = DateTime.MaxValue;
                if (agreementData.TerminationDt.Length > 1)
                    isTerminationDateOk = DateTime.TryParseExact(agreementData.TerminationDt, DATE_FORMAT,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out terminationDate);

                if (agreementData.SeCptyShortName == pBookingCoSN &&
                    agreementData.StatusInd == "E" &&
                    tradeDt >= dateSigned &&
                    tradeDt < terminationDate)
                {
                    if (isSecondRow)
                        result += "; ";
                    result += agreementData.AgrmntTypeCode + "(" + agreementData.SeCptyShortName + "):" + dateSigned.ToString(DATE_FORMAT);
                    isSecondRow = true;
                }

            }

            return result;
        }

        //Called from: frmEditContracts.ViewCptyInfo
        //Returned from: ConfirmProcessor.getContractFaxList
        //Data source: 
        //"select cpty_id,short_name,phone_id,phone_type_code,active_flag, " +
        //"country_phone_code,area_code,local_number,dsgntn_code,dsgactive_flag,descr " +
        //"from cpty.v_cpty_contract_fax " +
        //"where short_name = ?";
        //public List<ContractFaxNo> GetFaxNoList(string pShortName)
        //{
        //    var result = new List<ContractFaxNo>();
        //    result = GetFaxNoListStub();
        //    return result;
        //}

        //Called from: frmMain.GetCptyFaxNo line: 9092
        //Returned from: ConfirmProcessor.getCptyFax
        //Data source: 
        //select phone_type_code,
        //decode(phone_type_code,'FAX','F','TELEX','T','EMAIL','E','PHONE','P','?') fax_telex_ind,
        //country_phone_code, area_code, local_number 
        //from cpty.phone_number 
        //where id IN 
        //cpty.pkg_contracts.f_get_contract_phone_number_id('TESTOTC','NGAS','PHYS')

        //public CptyFaxNoDto GetFaxNo(string pCptySn, string pCdtyCode, string pInstType)
        //{
        //    CptyFaxNoDto cptyFaxNoData = new CptyFaxNoDto();
        //    cptyFaxNoData.PhoneTypeCode = "EMAIL";
        //    cptyFaxNoData.LocalNumber = "ifrankel@amphorainc.com";

        //    //string xmlResult = String.Empty;
        //    //string[] parms = new string[] { pCptySn, pCdtyCode, pInstType };
        //    //string urlStr = DBUtils.getUrlStr(cptyInfoAPIServiceUrlStr, GET_CONFIRM_SENDTO, parms);
        //    //xmlResult = DBUtils.getWebServiceUrlResult(urlStr);

        //    //XmlDocument xDoc = new XmlDocument();
        //    //if (xmlResult.Length > 1)
        //    //{
        //    //    xDoc.LoadXml(xmlResult);
        //    //    //string filename = @"C:\Users\ifrankel\AppDev\VS2013Projects\CptyInfoAPIServices\CptyInfoAPIService\XML\Error\CptyInfoResponse.xml";
        //    //    //xDoc.Load(filename);

        //    //    //XmlNodeList sendTos = xDoc.GetElementsByTagName("CptyInfoResponse");
        //    //    XmlNodeList nodeList = xDoc.SelectNodes("/CptyInfoResponse/SendTo");
        //    //    //if (sendTos.Count > 1)
        //    //    foreach (XmlNode node in nodeList)
        //    //    {
        //    //        cptyFaxNoData.PhoneTypeCode = node["SendToMethod"].InnerText;
        //    //        cptyFaxNoData.CountryPhoneCode = node["CountryPhoneCode"].InnerText;
        //    //        cptyFaxNoData.AreaCode = node["AreaCode"].InnerText;

        //    //        if (cptyFaxNoData.PhoneTypeCode == "EMAIL")
        //    //            cptyFaxNoData.LocalNumber = node["EmailAddress"].InnerText;
        //    //        else
        //    //            cptyFaxNoData.LocalNumber = node["LocalNumber"].InnerText;
        //    //    }
        //    //}
        //    return cptyFaxNoData;
        //}

    }
}
