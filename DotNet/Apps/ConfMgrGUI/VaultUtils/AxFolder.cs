using System;
using System.Collections.Generic;
using System.Text;

namespace VaultUtils
{
    public class AxFolder
    {
        private String _groupByIndexField = null;

        public String GroupByIndexField
        {
            get { return _groupByIndexField; }
            set { _groupByIndexField = value; }
        }
        
        private String _displayName = null;

        public String DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private String _folderName = null;

        public String FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }
        private String _fields = null;

        public String Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
        private String _viewerType = null;

        public String ViewerType
        {
            get { return _viewerType; }
            set { _viewerType = value; }
        }
        private String _dslName = null;

        public String DslName
        {
            get { return _dslName; }
            set { _dslName = value; }
        }
    }
}
