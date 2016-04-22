using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using NSRMCommon;

namespace NSRiskMgrCtrls {
    public partial class AmphoraFieldSelector : XtraUserControl {
        
        #region constants
        public const string DATA_SEP = ";";

        const string DEFAULT_KEY = WrappedWinDef.VALUE_NONE;

        const int HEIGHT_OFFSET = 4;
        const int WIDTH_OFFSET = 4;
        const int LABEL_WIDTH_OFFSET = 2;
        const int LABEL_HEIGHT_OFFSET = 10;
        const int LABEL_OFFSET_BETWEEN_CATEGORIES = 10;
        const int CB_OFFSET = 10;
        #endregion

        #region fields
        static readonly IDictionary<string,List<string>> map = new Dictionary<string,List<string>>();
        public static readonly IDictionary<string,string> fieldMapping = new Dictionary<string,string>();
        static readonly Dictionary<Type,List<PivotGridField>> typeFieldMap = new Dictionary<Type,List<PivotGridField>>();
        static string pgFieldToFind;
        Type _activeType;
        Color _clr;
        bool ignoreCheckChange;

        #endregion

        #region cctor/ctor
        static AmphoraFieldSelector() {
           
        }

        public AmphoraFieldSelector() {
            InitializeComponent();
        }
        #endregion

        #region properties
        [RefreshProperties(RefreshProperties.All)]
        public Color aColor { get { return _clr; } set { _clr = value; Invalidate(); } }

        #endregion

        #region overrides
        protected override void OnPaint(PaintEventArgs e) 
        {
            Rectangle r;

            base.OnPaint(e);

            r = new Rectangle(Point.Empty,new Size(Bounds.Width - 1,Bounds.Height - 1));

            using (Pen p = new Pen(aColor)) 
            {
                e.Graphics.DrawRectangle(p,r);
            }
        }

        protected override void OnCreateControl() 
        {
            base.OnCreateControl();
            this.SuspendLayout();
            addFields(true,map);
            this.ResumeLayout();
        }

        
        static void makeMap(string vector,IDictionary<string,List<string>> map) {
            string[] items;
            int pos;
            string key,value;

            if (!string.IsNullOrEmpty(vector)) 
            {
                if ((items = vector.Split(DATA_SEP[0])) != null && items.Length > 0) 
                {
                    map.Add(DEFAULT_KEY,new List<string>());

                    foreach (string aString in items) 
                    {
                        if ((pos = aString.IndexOf('.')) >= 0) {
                            if (!map.ContainsKey(key = aString.Substring(0,pos)))
                                map.Add(key,new List<string>());

                            if (map[key].Contains(value = aString.Substring(pos + 1)))
                                Trace.WriteLine("duplicate: " + value);
                            else
                                map[key].Add(value);

                            if (fieldMapping.ContainsKey(value))
                                MessageBox.Show("duplicate value: '" + value + "'!");
                            else
                                fieldMapping.Add(value,aString);
                        } 
                        else 
                        {
                            if (!string.IsNullOrEmpty(aString)) 
                            {
                                if (map[DEFAULT_KEY].Contains(aString))
                                    Trace.WriteLine("duplicate: " + aString);
                                else
                                    map[DEFAULT_KEY].Add(aString);
                            }
                        }
                    }
                }
            }
        }
         


        #endregion

        #region classes
        class LabelAndCheckboxDimesions {
            public int x;
            public int y;
            public int maxx;
            public int maxy;
            public int nlabel;
            public int ncheck;

            public LabelAndCheckboxDimesions() {
                x = y = 2;
                maxx = maxy = 0;
                nlabel = ncheck = 0;
            }
        }
        #endregion

        #region properties
        protected override Size DefaultSize {
            get {
                Size ret = base.DefaultSize;
                if (this.mySize != Size.Empty) {
                    ret = mySize;

                }

                return ret;
            }
        }

        Size mySize { get; set; }

        internal PivotGridControl activePivotGrid { get; set; }
        
        

        internal Type activePivotGridType 
        {
            get { return _activeType; }
            set {
                _activeType = value;
                if (activePivotGridType != null)
                    addTypeMapIfNeeded(this.activePivotGridType);
            }
        }


        #endregion

        void addTypeMapIfNeeded(Type type) {
            if (type == null)
                throw new ArgumentNullException("type","Type is null!");
            if (!typeFieldMap.ContainsKey(type))
                typeFieldMap.Add(type,DXUtil.findPivotFields(type));
        }

        void addFields(bool showEmptyHeader,IDictionary<string,List<string>> categoryToFieldMap) {
            LabelAndCheckboxDimesions labelAndCheckBoxDim;

            if (categoryToFieldMap.Count > 0) 
            {
                labelAndCheckBoxDim = new LabelAndCheckboxDimesions();
                foreach (string categoryName in categoryToFieldMap.Keys)
                {
                    addTitleAndCheckboxes(categoryName, categoryToFieldMap[categoryName], ref labelAndCheckBoxDim,
                        string.Compare(categoryName, DEFAULT_KEY, true) == 0 ?
                        (showEmptyHeader && categoryToFieldMap[categoryName].Count > 0) :
                        true);
                }

                mySize = new Size(labelAndCheckBoxDim.maxx + WIDTH_OFFSET,labelAndCheckBoxDim.maxy + HEIGHT_OFFSET);

                Size s = this.DefaultSize;
                foreach (Control c in this.Controls)
                    if (c.Bounds.Width < s.Width - 2 * WIDTH_OFFSET)
                        c.SetBounds(0,0,s.Width - 2 * WIDTH_OFFSET,0,BoundsSpecified.Width);
                if (s.Width > this.Bounds.Width)
                    this.SetBounds(0,0,s.Width,0,BoundsSpecified.Width);
                if (s.Height > this.Bounds.Height)
                    SetBounds(0,0,0,s.Height,BoundsSpecified.Height);
            }
        }

        void addTitleAndCheckboxes(string aName,List<string> list,ref LabelAndCheckboxDimesions labelAndCheckBoxDims,bool p) 
        {
            Label aLabel;

            if (p) 
            {
                this.Controls.Add(aLabel = new Label());

                aLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                aLabel.Text = aName;
                aLabel.Name = "label" + (labelAndCheckBoxDims.nlabel++);
                aLabel.Location = new Point(labelAndCheckBoxDims.x,labelAndCheckBoxDims.y+ LABEL_OFFSET_BETWEEN_CATEGORIES);

                labelAndCheckBoxDims.y += (aLabel.Size.Height + LABEL_HEIGHT_OFFSET);

                if (labelAndCheckBoxDims.maxx < aLabel.Width + LABEL_WIDTH_OFFSET)
                    labelAndCheckBoxDims.maxx = aLabel.Width + LABEL_WIDTH_OFFSET;

                if (labelAndCheckBoxDims.maxy < aLabel.Location.Y + aLabel.Height + LABEL_HEIGHT_OFFSET)
                    labelAndCheckBoxDims.maxy = aLabel.Location.Y + aLabel.Height + LABEL_HEIGHT_OFFSET;
            }
            addCheckboxes(list,ref labelAndCheckBoxDims);
        }

        void addCheckboxes(List<string> list,ref LabelAndCheckboxDimesions labelAndCheckboxDims) 
        {
            CheckBox cb;
            Size prefSize;

            if (list.Count > 0) {
                foreach (string anItem in list) 
                {
                    this.Controls.Add(cb = new CheckBox());
                    cb.CheckedChanged += cb_CheckedChanged;
                    cb.Text = anItem;
                    cb.Name = "label" + (labelAndCheckboxDims.ncheck++);
                    cb.Location = new Point(labelAndCheckboxDims.x,labelAndCheckboxDims.y);

                    prefSize = cb.PreferredSize;

                    labelAndCheckboxDims.y += (prefSize.Height + HEIGHT_OFFSET);


                    if (labelAndCheckboxDims.maxx < prefSize.Width + CB_OFFSET)
                        labelAndCheckboxDims.maxx = prefSize.Width + CB_OFFSET;
                    if (labelAndCheckboxDims.maxy < cb.Location.Y + prefSize.Height + HEIGHT_OFFSET)
                        labelAndCheckboxDims.maxy = cb.Location.Y + prefSize.Height + HEIGHT_OFFSET;
                }
                
            }
        }

        #region action methods

        void cb_CheckedChanged(object sender,EventArgs e) {
            CheckBox cb;
            string fieldName;

            if (ignoreCheckChange)
                return;

            if (sender != null && (cb = sender as CheckBox) != null && activePivotGrid != null) 
            {
                if (!fieldMapping.ContainsKey(fieldName = cb.Text)) {
                    Trace.WriteLine("checkbox named '" + fieldName + "' not found!");
                } 
                else 
                {
                    enableDisablePivotFieldOnPivotGrid(this.activePivotGrid,activePivotGridType,cb.Checked,fieldName);
                }
            }
        }

        static bool findPGField(PivotGridField pgf) {
            return string.Compare(pgFieldToFind,pgf.Caption,true) == 0;
        }

        static void enableDisablePivotFieldOnPivotGrid(PivotGridControl pgc,Type currType,bool addField,string fieldName) {
            string tmp;

            if (currType == null)
                throw new ArgumentNullException("currType",typeof(Type).Name + " is null!");
            if (!typeFieldMap.ContainsKey(currType))
                throw new InvalidOperationException("current type (" + currType.FullName + ") not found in field-map!");

            if (addField) 
            {
                if (fieldName != null && fieldName.Equals("As Of Physical Qty") || fieldName.Equals("As Of Risk Qty"))
                {
                    TabPageWithUomPivot.enableDisablesOfDateEditor(true);
                }
                PivotGridField pivotFieldFound = pgc.Fields.GetFieldByName(DXUtil.PGF_PREFIX + fieldName);

                if (pivotFieldFound != null) 
                {
                    pivotFieldFound.Visible = true;
                    
                    if(fieldName == "Is Zero")
                        pivotFieldFound.Options.IsFilterRadioMode = DevExpress.Utils.DefaultBoolean.True;
                } 
                else 
                {
                    List<PivotGridField> pivotFieldsForCurrType = typeFieldMap[currType];
                    pgFieldToFind = fieldName;

                    PivotGridField pivotFieldFound2 = pivotFieldsForCurrType.Find(findPGField);
                    pgFieldToFind = null;

                    if (pivotFieldFound2 != null) 
                    {
                        if (fieldName == "Is Zero")
                            pivotFieldFound2.Options.IsFilterRadioMode = DevExpress.Utils.DefaultBoolean.True;
                        pgc.Fields.Add(pivotFieldFound2);
                        pivotFieldFound2.Visible = true;
                    } 
                    else 
                    {
                        Trace.WriteLine("field '" + fieldName + "' not found!");
                    }
                }
               
            } 
            else 
            {
                PivotGridField x = pgc.Fields.GetFieldByName(tmp = DXUtil.PGF_PREFIX + fieldName);
                if (x != null) 
                {
                    x.Visible = false;
                } 
                else 
                {
                    Trace.WriteLine("not added, and not found!");
                }
                if (fieldName != null && fieldName.Equals("As Of Physical Qty") || fieldName.Equals("As Of Risk Qty"))
                {
                    if (dictCheckBoxesMap != null && dictCheckBoxesMap.Count > 0)
                    {
                        if (dictCheckBoxesMap["As Of Risk Qty"].Checked == false && dictCheckBoxesMap["As Of Physical Qty"].Checked == false)
                        {
                            TabPageWithUomPivot.enableDisablesOfDateEditor(false);
                        }
                    }
                }
            }
        }

        #endregion
        public static Dictionary<string, CheckBox> dictCheckBoxesMap;
        internal void populateSelection(PivotGridFieldCollection fields) {
            CheckBox cb;
            
            string key;

            ignoreCheckChange = true;
            dictCheckBoxesMap = new Dictionary<string, CheckBox>();
            foreach (var avar in Controls)
                if ((cb = avar as CheckBox) != null) {
                    cb.Checked = false;
                    dictCheckBoxesMap.Add(cb.Text, cb);
                }
            if (fields != null)
                foreach (PivotGridField pgf in fields)
                    if (dictCheckBoxesMap.ContainsKey(key = pgf.Caption))
                        if (pgf.Visible)
                            dictCheckBoxesMap[key].Checked = true;
            ignoreCheckChange = false;
        }

        static bool rebuilt = false;

        
        public static void buildMapFrom(Type aType) 
        {
            DesiredPivotGridFieldAttribute attr;
            Type attrType = typeof(DesiredPivotGridFieldAttribute);
            int nitems = 0;
            object[] attrs;
            string dispGrp;
            StringBuilder sb;

            if (rebuilt)
                return;
            sb = new StringBuilder();
            foreach (var avar in aType.GetProperties()) {
                if ((attrs = avar.GetCustomAttributes(attrType,false)) != null &&
                    attrs.Length > 0 &&
                    attrs[0] is DesiredPivotGridFieldAttribute) 
                {
                    attr = attrs[0] as DesiredPivotGridFieldAttribute;

                    if (nitems > 0)
                        sb.Append(AmphoraFieldSelector.DATA_SEP);
                    if (!string.IsNullOrEmpty(dispGrp = attr.DisplayFolder))
                        sb.Append(dispGrp + ".");
                    sb.Append(string.IsNullOrEmpty(attr.FieldName) ? attr.Caption : attr.FieldName);
                    nitems++;
                }
            }
            map.Clear();
            fieldMapping.Clear();
            makeMap(sb.ToString(),map);
            rebuilt = true;
        }
         
    }
}