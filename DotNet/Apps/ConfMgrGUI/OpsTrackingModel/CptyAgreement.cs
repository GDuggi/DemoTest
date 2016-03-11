using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    public class CptyAgreement
    {
        private const Int32 _nullNumberValue = -1;

        private String agrmntTypeCode;
        private String statusInd;
        private String dateSigned;
        private String terminationDt;
        private String seAgrmntContactName;
        private String cmt;
        private Int32 agreementId;
        private Int32 seCptyId;
        private Int32 cptyId;
        private String cptyShortName;
        private String seCptyShortName;

        public String AgrmntTypeCode
        {
            get { return agrmntTypeCode; }
            set { agrmntTypeCode = value; }
        }

        public String StatusInd
        {
            get { return statusInd; }
            set { statusInd = value; }
        }

        public String DateSigned
        {
            get { return dateSigned; }
            set { dateSigned = value; }
        }

        public String TerminationDt
        {
            get { return terminationDt; }
            set { terminationDt = value; }
        }

        public String SeAgrmntContactName
        {
            get { return seAgrmntContactName; }
            set { seAgrmntContactName = value; }
        }

        public String Cmt
        {
            get { return cmt; }
            set { cmt = value; }
        }

        public Int32 AgreementId
        {
            get { return agreementId; }
            set { agreementId = value; }
        }

        public Int32 SeCptyId
        {
            get { return seCptyId; }
            set { seCptyId = value; }
        }

        public Int32 CptyId
        {
            get { return cptyId; }
            set { cptyId = value; }
        }

        public String CptyShortName
        {
            get { return cptyShortName; }
            set { cptyShortName = value; }
        }

        public String SeCptyShortName
        {
            get { return seCptyShortName; }
            set { seCptyShortName = value; }
        }


        public CptyAgreement()
        {
            this.agreementId = CptyAgreement._nullNumberValue;
            this.agrmntTypeCode = null;
            this.cmt = null;
            this.cptyId = CptyAgreement._nullNumberValue;
            this.cptyShortName = null;
            this.dateSigned = null;
            this.seCptyId = CptyAgreement._nullNumberValue;
            this.statusInd = null;
            this.terminationDt = null;
            this.seCptyShortName = null;

        }
    }
}
