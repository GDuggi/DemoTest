using System;

namespace NSRMLogging {
    /// <summary>An attribute to be used to identify the location, caption and format of fields be
    /// be used in a pivot-grid.</summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class DesiredPivotGridFieldAttribute : Attribute {

        #region ctors
        /// <summary>default ctor.</summary>
        public DesiredPivotGridFieldAttribute() { }

        /// <summary>overrloaded ctor.</summary>
        /// <param name="aCaption">a <see cref="string"/> containing the caption associated with this item.</param>
        public DesiredPivotGridFieldAttribute(string aCaption) : this(aCaption,PivotGridAvail.NONE) { }

        /// <summary>overloaded ctor.</summary>
        /// <param name="aCaption">a <see cref="string"/> containing the caption associated with this item.</param>
        /// <param name="pga">a <see cref="PivotGridAvail"/> indicating the permissible location of this field.</param>
        public DesiredPivotGridFieldAttribute(string aCaption,PivotGridAvail pga)
            : this() {
            Caption = aCaption;
            pivotGridAvailability = pga;
        }
        #endregion

        #region properties
        /// <summary>The caption to be displayed for this field.</summary>
        public string Caption { get; set; }

        /// <summary>If numeric, use this format for display.</summary>
        public string NumericFormat { get; set; }

        /// <summary></summary>
        public string DisplayFolder { get; set; }

        /// <summary>Indicate when <b>NumericFormat</b> is a date, rather than a number.</summary>
        public bool IsDate { get; set; }

        /// <summary>Permissible location of this field.</summary>
        public PivotGridAvail pivotGridAvailability { get; set; }
        #endregion
    }

    /// <summary>Enumeration describing where a give field is allowed, within a pivot-grid.</summary>
    public enum PivotGridAvail {
        NONE = -1,
        /// <summary>Allowed in row-section.</summary>
        Row,
        /// <summary>Allowed in column-section.</summary>
        Column,
        /// <summary>Allowed in filter-section.</summary>
        Filter,
        /// <summary>Allowed in data-section.</summary>
        Data,
    }
}