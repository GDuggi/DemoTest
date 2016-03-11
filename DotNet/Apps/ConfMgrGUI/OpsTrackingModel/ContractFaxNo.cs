using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    public class ContractFaxNo
    {
        private const Int32 _nullNumberValue = -1;

        private String activeFlag;
        private String areaCode;
        private String countryPhoneCode;
        private Int32 cptyId;
        private String description;
        private String designationCode;
        private String dsgActiveFlag;
        private String localNumber;
        private Int32 phoneId;
        private String phoneTypeCode;
        private String shortName;

        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        public String PhoneTypeCode
        {
            get { return phoneTypeCode; }
            set { phoneTypeCode = value; }
        }

        public Int32 PhoneId
        {
            get { return phoneId; }
            set { phoneId = value; }
        }

        public String LocalNumber
        {
            get { return localNumber; }
            set { localNumber = value; }
        }

        public String DsgActiveFlag
        {
            get { return dsgActiveFlag; }
            set { dsgActiveFlag = value; }
        }

        public String DesignationCode
        {
            get { return designationCode; }
            set { designationCode = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public Int32 CptyId
        {
            get { return cptyId; }
            set { cptyId = value; }
        }

        public String CountryPhoneCode
        {
            get { return countryPhoneCode; }
            set { countryPhoneCode = value; }
        }

        public String AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        public String ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; }
        }

        public ContractFaxNo()
        {
            this.activeFlag = null;
            this.areaCode = null;
            this.countryPhoneCode = null;
            this.cptyId = ContractFaxNo._nullNumberValue;
            this.description = null;
            this.designationCode = null;
            this.dsgActiveFlag = null;
            this.localNumber = null;
            this.phoneId = ContractFaxNo._nullNumberValue;
            this.phoneTypeCode = null;
            this.shortName = null;
        }
    }
}
