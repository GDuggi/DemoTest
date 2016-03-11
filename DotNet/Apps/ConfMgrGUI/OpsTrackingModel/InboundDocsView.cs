using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [System.Serializable]
    public class InboundDocsView:IOpsDataObj
    {
        private const Int32 _nullNumberValue = -1;
        private const string _nullDateTime = "1900-01-01T12:00:00";

        private System.Int32 _id;

        private System.Int32 _unresolvedCount;

        private string _callerRef;
        private string _sentTo;

        private System.DateTime _rcvdTs;

        private string _fileName;
        private string _sender;
        private string _cmt;
        private string _docStatusCode;
        private string _hasAutoAsctedFlag;
        private string _tradeIds;
        private string _bookmarkFlag;
        private string _bookmarkUser;
        private string _ignoreFlag;
        private string _ignoredUser;
        private string _commentFlag;
        private string _commentUser;
        private string _userComments;
        private string _mappedCptySn;
        private string _procFlag;

        public string ProcFlag
        {
            get { return _procFlag; }
            set { _procFlag = value; }
        }

        public string MappedCptySn
        {
            get { return _mappedCptySn; }
            set { _mappedCptySn = value; }
        }

        public string UserComments
        {
            get { return _userComments; }
            set { _userComments = value; }
        }

        public string CommentUser
        {
            get { return _commentUser; }
            set { _commentUser = value; }
        }

        public string CommentFlag
        {
            get { return _commentFlag; }
            set { _commentFlag = value; }
        }

        public string IgnoredUser
        {
            get { return _ignoredUser; }
            set { _ignoredUser = value; }
        }

        public string IgnoreFlag
        {
            get { return _ignoreFlag; }
            set { _ignoreFlag = value; }
        }

        public string BookmarkUser
        {
            get { return _bookmarkUser; }
            set { _bookmarkUser = value; }
        }

        public string BookmarkFlag
        {
            get { return _bookmarkFlag; }
            set { _bookmarkFlag = value; }
        }

        public string TradeIds
        {
            get { return _tradeIds; }
            set { _tradeIds = value; }
        }

        public string Tradeids // added for DB loading of database objects.  Set field is not recognized.  Correct fix should be to modify the SQL.  Refactor is an issue though.
        {
            set { TradeIds = value; }
        }

        public string HasAutoAsctedFlag
        {
            get { return _hasAutoAsctedFlag; }
            set { _hasAutoAsctedFlag = value; }
        }

        public string DocStatusCode
        {
            get { return _docStatusCode; }
            set { _docStatusCode = value; }
        }

        public string Cmt
        {
            get { return _cmt; }
            set { _cmt = value; }
        }

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public System.DateTime RcvdTs
        {
            get { return _rcvdTs; }
            set { _rcvdTs = value; }
        }

        public string SentTo
        {
            get { return _sentTo; }
            set { _sentTo = value; }
        }

        public string CallerRef
        {
            get { return _callerRef; }
            set { _callerRef = value; }
        }

        public System.Int32 UnresolvedCount
        {
            get { return _unresolvedCount; }
            set { _unresolvedCount = value; }
        }

        public System.Int32 Unresolvedcount // added for DB loading of database objects.  Set field is not recognized.  Correct fix should be to modify the SQL.  Refactor is an issue though.
        {
            set { UnresolvedCount = value; }
        }

        public System.Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public InboundDocsView()
        {
            this._id = InboundDocsView._nullNumberValue;
            this._unresolvedCount = InboundDocsView._nullNumberValue;
            this._callerRef = null;
            this._sentTo = null;
            this._rcvdTs = System.DateTime.Parse(InboundDocsView._nullDateTime);
            this._fileName = null;
            this._sender = null;
            this._cmt = null;
            this._docStatusCode = null;
            this._hasAutoAsctedFlag = null;
            this._tradeIds = null;
            this._bookmarkFlag = null;
            this._bookmarkUser = null;
            this._ignoreFlag = null;
            this._ignoredUser = null;
            this._commentFlag = null;
            this._commentUser = null;
            this._userComments = null;
            this._mappedCptySn = null;
            this._procFlag = "Y";
        }

        public string getSelect()
        {
            return Properties.Settings.Default.InboundDocsView;
        }
    }
}
