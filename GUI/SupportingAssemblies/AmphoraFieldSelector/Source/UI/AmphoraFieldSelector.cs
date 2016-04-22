
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NSFS {
    public partial class AmphoraFieldSelector : XtraUserControl {
        #region fields
        Color _clr;
        #endregion

        #region properties
        [RefreshProperties(RefreshProperties.All)]
        public Color aColor { get { return _clr; } set { _clr = value; Invalidate(); } }

        #endregion

        #region ctor
        public AmphoraFieldSelector() {
            InitializeComponent();
        }
        #endregion

        #region overrides
        protected override void OnPaint(PaintEventArgs e) {
            Rectangle r;

            base.OnPaint(e);

            r = new Rectangle(Point.Empty,new Size(Bounds.Width - 1,Bounds.Height - 1));
            using (Pen p = new Pen(aColor)) {

                e.Graphics.DrawRectangle(p,r);
#if false
                if (DesignMode) {
                    e.Graphics.DrawLine(
                        p,
                        0,0,
                        Bounds.Width,
                        Bounds.Height);
                    e.Graphics.DrawLine(
                        p,
                        0,Bounds.Height,
                        Bounds.Width,
                        0);
                } 
#endif
            }
        }
        protected override void OnCreateControl() {
            base.OnCreateControl();
            IDictionary<string,List<string>> map = new Dictionary<string,List<string>>();
            int pos;
            string[] items;
            string key,value;

            if (!string.IsNullOrEmpty(ITEMS)) {
                if ((items = ITEMS.Split(';')) != null && items.Length > 0) {

                    map.Add(DEFAULT_KEY,new List<string>());
                    foreach (string aString in items) {
                        if ((pos = aString.IndexOf('.')) >= 0) {
                            if (!map.ContainsKey(key = aString.Substring(0,pos)))
                                map.Add(key,new List<string>());
                            if (map[key].Contains(value = aString.Substring(pos + 1)))
                                Debug.Print("duplicate: " + value);
                            else
                                map[key].Add(value);

                        } else {
                            if (!string.IsNullOrEmpty(aString)) {
                                if (map[DEFAULT_KEY].Contains(aString))
                                    Debug.Print("duplicate: " + aString);
                                else
                                    map[DEFAULT_KEY].Add(aString);
                            }
                        }
                    }
                }
            }

            int maxx,maxy;

            addFields(true,map,out maxx,out maxy);


        }
        #endregion

        #region utility methods
        string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name + "." + mb.Name;
        }
        #endregion utility methods

        #region constants
        const string ITEMS = "Quantity.Risk Quantity;" +
    "Quantity.Physical Quantity;" +
    "Quantity.Est Quantity;" +
    "Quantity.Discount Quantity;" +

    "Attributes.Commodity;" +
    "Attributes.Market;" +

    "Time.Period;" +
    "Time.Month of Period;" +
    "Time.Expiration Date;"+

    "Other.Location;"+
    "Other.Don's field"
    ;

        const string DEFAULT_KEY = "NONE";

        const int HEIGHT_OFFSET = 4;
        const int WIDTH_OFFSET = 4;
        const int LABEL_WIDTH_OFFSET = 2;
        const int CB_OFFSET = 3;
        #endregion

        #region classes
        class blah {
            public int x;
            public int y;
            public int maxx;
            public int maxy;
            public int nlabel;
            public int ncheck;

            public blah() {
                x = y = 2;
                maxx = maxy = 0;
                nlabel = ncheck = 0;
            }
        }
        #endregion

        void addFields(bool showEmptyHeader,IDictionary<string,List<string>> map,out int maxx,out int maxy) {
            blah ablah;

            maxx = maxy = -1;
            if (map.Count > 0) {
                ablah = new blah();
                foreach (string aName in map.Keys)
                    addTitleAndCheckboxes(aName,map[aName],ref ablah,
                        string.Compare(aName,DEFAULT_KEY,true) == 0 ?
                        (showEmptyHeader && map[aName].Count > 0) :
                        true);
                if (this.Size.Height < ablah.y + HEIGHT_OFFSET)
                    this.SetBounds(0,0,0,ablah.y + HEIGHT_OFFSET,BoundsSpecified.Height);

                if (this.Size.Width < ablah.maxx + WIDTH_OFFSET)
                    SetBounds(0,0,ablah.maxx + WIDTH_OFFSET,0,BoundsSpecified.Width);
                if (this.Size.Width > ablah.maxx)
                    SetBounds(0,0,ablah.maxx,0,BoundsSpecified.Width);
                Debug.Print("sizing ends-2");
            }
        }

        void addTitleAndCheckboxes(string aName,List<string> list,ref blah ablah,bool p) {
            Label aLabel;

            if (p) {
                this.Controls.Add(aLabel = new Label());
                aLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                aLabel.Text = aName;
                aLabel.Name = "label" + (ablah.nlabel++);
                aLabel.Location = new Point(ablah.x,ablah.y);
                ablah.y += (aLabel.Size.Height + LABEL_WIDTH_OFFSET);
                if (ablah.maxx < aLabel.Width + LABEL_WIDTH_OFFSET)
                    ablah.maxx = aLabel.Width + LABEL_WIDTH_OFFSET;
            }
            addCheckboxes(list,ref ablah);
        }

        void addCheckboxes(List<string> list,ref blah ablah) {
            CheckBox cb;
            if (list.Count > 0) {
                foreach (string anItem in list) {
                    this.Controls.Add(cb = new CheckBox());
                    cb.Text = anItem;
                    cb.Name = "label" + (ablah.ncheck++);
                    cb.Location = new Point(ablah.x,ablah.y);
                    ablah.y += (cb.Size.Height + CB_OFFSET);
                    if (ablah.maxx < cb.Width + CB_OFFSET)
                        ablah.maxx = cb.Width + CB_OFFSET;
                }
            }
        }
    }
}