using System;

namespace NSRMCommon {
    /// <summary>An attribute to be used to identify the location, caption and format of fields be
    /// be used in a pivot-grid.</summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class DesiredPivotGridFieldAttribute : Attribute {

        #region ctors
        /// <summary>default ctor.</summary>
        public DesiredPivotGridFieldAttribute() { }

        /// <summary>overrloaded ctor.</summary>
        /// <param name="aCaption">a <see cref="string"/> containing the caption associated with this item.</param>
        public DesiredPivotGridFieldAttribute(string aCaption) : this(aCaption,false,string.Empty) { }

        /// <summary>overloaded ctor.</summary>
        /// <param name="aCaption">a <see cref="string"/> containing the caption associated with this item.</param>
        /// <param name="bIsDataField"> a <see cref="bool"/> indicating whether this is a data-only field.</param>
        /// <param name="aDispFolder">a <see cref="string"/> containing the display-folder for this item.</param>
        public DesiredPivotGridFieldAttribute(string aCaption,bool bIsDataField,string aDispFolder)
            : this() {
            Caption = aCaption;
            //            pivotGridAvailability = pga;
            IsDataField = bIsDataField;
            DisplayFolder = aDispFolder;
        }
        #endregion

        #region properties
        /// <summary>The caption to be displayed for this field.</summary>
        public string Caption { get; set; }

        /// <summary>If numeric, use this format for display.</summary>
        public string NumericFormat { get; set; }

        /// <summary></summary>
        public string DisplayFolder { get; set; }

        public PivotFieldAlignment Alignment { get; set; }

        /// <summary>Indicate when <b>NumericFormat</b> is a date, rather than a number.</summary>
        public bool IsDate { get; set; }

        /// <summary>Indicate whether this field is permissible in the data-area of the grid, or not.</summary>
        public bool IsDataField { get; set; }

        public bool IsFilterField { get; set; }

        public bool IsRunningTotal { get; set; }


        public string FieldName { get; set; }
        #endregion
    }
    public enum PivotFieldAlignment {
        LEFT,
        CENTER,
        RIGHT
    }
}