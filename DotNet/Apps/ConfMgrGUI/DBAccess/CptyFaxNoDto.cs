using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class CptyFaxNoDto
    {
        public CptyFaxNoDto()
        {
            this.PhoneTypeCode = null;
            this.FaxTelexInd = null;
            this.CountryPhoneCode = null;
            this.AreaCode = null;
            this.LocalNumber = null;
        }

        public string PhoneTypeCode { get; set; }
        public string FaxTelexInd { get; set; }
        public string CountryPhoneCode { get; set; }
        public string AreaCode { get; set; }
        public string LocalNumber { get; set; }
    }

    
}
