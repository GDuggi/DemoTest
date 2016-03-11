using System;
using System.Collections.Generic;
using System.Text;

namespace VaultUtils
{
    public class AXFieldMapping
    {
        public AXFieldMapping(string fieldName)
        {
            _editorType = "TXT";
            _fieldName = fieldName;
            _displayName = fieldName;
            _visible = "T";
            _width = "100";
            _helpDisplay = fieldName;
        }

        public AXFieldMapping()
        {
        }

        private string _editorType = null;

        public string EditorType
        {
            get { return _editorType; }
            set { _editorType = value; }
        }

        private string _fieldName = null;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        private string _displayName = null;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        private string _visible = null;

        public string Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        private string _width = null;

        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private string _helpDisplay = null;

        public string HelpDisplay
        {
            get { return _helpDisplay; }
            set { _helpDisplay = value; }
        }
        private string _defaultValue = null;

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }
}
