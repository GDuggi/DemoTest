using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class TradeRqmtConfirm:IOpsDataObj
    {
        private const Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";
        private System.Int32 _id;
        private System.Int32 _rqmtId;
        private System.Int32 _tradeId;
        private System.Int32 _templateId;
        private string _confirmLabel;
        private string _confirmCmt;
        private string _faxTelexInd;
        private string _faxTelexNumber;
        private string _xmitStatusInd;
        private string _xmitAddr;
        private string _xmitCmt;
        private System.DateTime _xmitTimeStampGmt;
        private string _templateName;
        private string _preparerCanSendFlag;
        private string _templateCategory;
        private string _templateTypeInd;
        private string _finalApprovalFlag;
        private string _nextStatusCode;
        private string _activeFlag;

        public string ActiveFlag
        {
            get { return _activeFlag; }
            set { _activeFlag = value; }
        }

        public string NextStatusCode
        {
            get { return _nextStatusCode; }
            set { _nextStatusCode = value; }
        }

        public string FinalApprovalFlag
        {
            get { return _finalApprovalFlag; }
            set { _finalApprovalFlag = value; }
        }

        public string TemplateTypeInd
        {
            get { return _templateTypeInd; }
            set { _templateTypeInd = value; }
        }

        public string TemplateCategory
        {
            get { return _templateCategory; }
            set { _templateCategory = value; }
        }

        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }


        public string PreparerCanSendFlag
        {
            get { return _preparerCanSendFlag; }
            set { _preparerCanSendFlag = value; }
        }

        public System.DateTime XmitTimeStampGmt
        {
            get { return _xmitTimeStampGmt; }
            set { _xmitTimeStampGmt = value; }
        }

        public System.DateTime XmitTimestampGmt // added for database loading of objects.  Correct fix would be to fix the SQL to match the fieldname.  too many factors though for refactoring.
        {
            set { XmitTimeStampGmt = value; }
        }

        public string XmitCmt
        {
            get { return _xmitCmt; }
            set { _xmitCmt = value; }
        }

        public string XmitAddr
        {
            get { return _xmitAddr; }
            set { _xmitAddr = value; }
        }

        public string XmitStatusInd
        {
            get { return _xmitStatusInd; }
            set { _xmitStatusInd = value; }
        }

        public string FaxTelexNumber
        {
            get { return _faxTelexNumber; }
            set { _faxTelexNumber = value; }
        }

        public string FaxTelexInd
        {
            get { return _faxTelexInd; }
            set { _faxTelexInd = value; }
        }

        public string ConfirmCmt
        {
            get { return _confirmCmt; }
            set { _confirmCmt = value; }
        }

        public string ConfirmLabel
        {
            get { return _confirmLabel; }
            set { _confirmLabel = value; }
        }
        
        public System.Int32 TemplateId
        {
            get { return _templateId; }
            set { _templateId = value; }
        }

        public System.Int32 TradeId
        {
            get { return _tradeId; }
            set { _tradeId = value; }
        }

        public System.Int32 RqmtId
        {
            get { return _rqmtId; }
            set { _rqmtId = value; }
        }

        public System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public TradeRqmtConfirm()
        {
            
            this._id = TradeRqmtConfirm._nullNumberValue;
            this._rqmtId = TradeRqmtConfirm._nullNumberValue;
            this._tradeId = TradeRqmtConfirm._nullNumberValue;
            this._templateId = TradeRqmtConfirm._nullNumberValue;
            this._confirmLabel = null;
            this._confirmCmt = null;
            this._faxTelexInd = null;
            this._faxTelexNumber = null;
            this._xmitStatusInd = null;
            this._xmitAddr = null;
            this._xmitCmt = null;
            this._xmitTimeStampGmt = System.DateTime.Parse(TradeRqmtConfirm._nullDateTime);
            this._templateName = null;
            this._preparerCanSendFlag = null;
            this._templateCategory = null;
            this._templateTypeInd = null;
            this._finalApprovalFlag = null;
            this._nextStatusCode = null;
            this._activeFlag = null;
        }

        public string getSelect()
        {
            return Properties.Settings.Default.TradeRqmtConfirm;
        }
    }
}
