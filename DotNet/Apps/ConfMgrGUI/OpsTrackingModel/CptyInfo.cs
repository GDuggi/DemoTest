using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    public class CptyInfo
    {
        private String cptyShortName;
        private String cptyLegalName;
        private String cptyAddress1;
        private String cptyAddress2;
        private String cptyAddress3;
        private String cptyCity;
        private String cptyState;
        private String cptyZipcode;
        private String cptyCountry;
        private String cptyMainFaxCntryCode;
        private String cptyMainFaxAreaCode;
        private String cptyMainFax;
        private String cptyMainPhoneCntryCode;
        private String cptyMainPhoneAreaCode;
        private String cptyMainPhone;
        private IList<CptyAgreement> cptyAgreements;
        private IList<ContractFaxNo> contractFaxNumbers;

        public String CptyShortName
        {
            get { return cptyShortName; }
            set { cptyShortName = value; }
        }

        public String CptyLegalName
        {
            get { return cptyLegalName; }
            set { cptyLegalName = value; }
        }

        public String CptyAddress1
        {
            get { return cptyAddress1; }
            set { cptyAddress1 = value; }
        }

        public String CptyAddress2
        {
            get { return cptyAddress2; }
            set { cptyAddress2 = value; }
        }

        public String CptyAddress3
        {
            get { return cptyAddress3; }
            set { cptyAddress3 = value; }
        }

        public String CptyCity
        {
            get { return cptyCity; }
            set { cptyCity = value; }
        }

        public String CptyState
        {
            get { return cptyState; }
            set { cptyState = value; }
        }

        public String CptyZipcode
        {
            get { return cptyZipcode; }
            set { cptyZipcode = value; }
        }

        public String CptyCountry
        {
            get { return cptyCountry; }
            set { cptyCountry = value; }
        }

        public String CptyMainFaxCntryCode
        {
            get { return cptyMainFaxCntryCode; }
            set { cptyMainFaxCntryCode = value; }
        }

        public String CptyMainFaxAreaCode
        {
            get { return cptyMainFaxAreaCode; }
            set { cptyMainFaxAreaCode = value; }
        }

        public String CptyMainFax
        {
            get { return cptyMainFax; }
            set { cptyMainFax = value; }
        }

        public String CptyMainPhoneCntryCode
        {
            get { return cptyMainPhoneCntryCode; }
            set { cptyMainPhoneCntryCode = value; }
        }

        public String CptyMainPhoneAreaCode
        {
            get { return cptyMainPhoneAreaCode; }
            set { cptyMainPhoneAreaCode = value; }
        }

        public String CptyMainPhone
        {
            get { return cptyMainPhone; }
            set { cptyMainPhone = value; }
        }

        public IList<CptyAgreement> CptyAgreements
        {
            get { return cptyAgreements; }
            set { cptyAgreements = value; }
        }

        public IList<ContractFaxNo> ContractFaxNumbers
        {
            get { return contractFaxNumbers; }
            set { contractFaxNumbers = value; }
        }

        public CptyInfo()
        {
            this.cptyAddress1 = null;
            this.cptyAddress2 = null;
            this.cptyAddress3 = null;
            this.cptyCity = null;
            this.cptyCountry = null;
            this.cptyLegalName = null;
            this.cptyMainFax = null;
            this.cptyMainFaxAreaCode = null;
            this.cptyMainFaxCntryCode = null;
            this.cptyMainPhone = null;
            this.cptyMainPhoneAreaCode = null;
            this.cptyMainPhoneCntryCode = null;
            this.cptyShortName = null;
            this.cptyState = null;
            this.CptyZipcode = null;
            this.cptyAgreements = new List<CptyAgreement>();
            this.contractFaxNumbers = new List<ContractFaxNo>();
        }
    }
}
