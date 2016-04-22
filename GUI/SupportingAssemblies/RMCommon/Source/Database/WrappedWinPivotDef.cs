using System.ComponentModel;
using com.amphora.cayenne.entity;
using org.apache.cayenne;

namespace NSRMCommon {
    public class WrappedWinPivotDef  {

        #region constants
        public const string NULL_PIVOT_NAME = "Untitled 1";
        public const string NULL_LAYOUT_NAME = "test";
        #endregion

        #region ctors
        public WrappedWinPivotDef() : this(new RiskmgrWinPivotDef()) { }

        public WrappedWinPivotDef(RiskmgrWinPivotDef aDef) {
            pivotDefinition = aDef;
        }


        #endregion

        #region properties
        [Browsable(false)]
        public RiskmgrWinPivotDef pivotDefinition { get; private set; }
        [Browsable(false)]
        public bool deleted { get; set; }

        public int tabIndex { get { return pivotDefinition.getTabIndex().intValue(); } set { pivotDefinition.setTabIndex(new java.lang.Integer(value)); } }
        public string pivotLayout { get { return pivotDefinition.getPivotLayout(); } set { pivotDefinition.setPivotLayout(string.IsNullOrEmpty(value) ? NULL_PIVOT_NAME : value); } }
        public string tabName { get { return pivotDefinition.getTabName(); } set { pivotDefinition.setTabName(string.IsNullOrEmpty(value) ? NULL_LAYOUT_NAME : value); } }
        public int numberOfDecimals { get { return pivotDefinition.getNumOfDecimals().intValue(); } set { pivotDefinition.setNumOfDecimals(new java.lang.Short(System.Convert.ToInt16(value))); } }
        public bool showFuturesEquiv {
            get {
                string val = pivotDefinition.getShowFutureEquiv();

                if (!string.IsNullOrEmpty(val))
                    return val.Substring(0,1).CompareTo("Y") == 0;
                return false;
            }
            set {
                pivotDefinition.setShowFutureEquiv(value ? "Y" : "N");
            }
        }

        public bool ShowZero
        {
            get
            {
                string val = pivotDefinition.getShowZero();


                return (val == "Y");
            }
            set
            {
                pivotDefinition.setShowZero(value ? "Y" : "N");
            }
        }

        #endregion

        #region methods
        public WrappedWinPivotDef createDefault(string aName,ObjectContext ctx) {
            WrappedWinPivotDef ret;

            if (ctx == null)
                ctx = LocalCayenneService.sharedInstance.newObjectContext();
            this.pivotDefinition = ctx.newObject(typeof(RiskmgrWinPivotDef)) as RiskmgrWinPivotDef;
            ret = new WrappedWinPivotDef(this.pivotDefinition);
            return ret.createDefault(aName);
        }
        #endregion

        internal WrappedWinPivotDef createDefault(string p) {
            this.pivotLayout = p;
            this.tabIndex = 0;
            this.tabName = WrappedWinPivotDef.NULL_PIVOT_NAME;
            this.numberOfDecimals = 2;
            this.showFuturesEquiv = false;
            this.ShowZero = false;
            this.pivotDefinition.setUomRel((Uom)
                this.pivotDefinition.getObjectContext().localObject(SharedContext.findUom("BBL")));
            return this;
        }

        internal static WrappedWinPivotDef createDefault2(string p,ObjectContext objectContext) {
            return new WrappedWinPivotDef().createDefault(p,objectContext);
        }
    }
}