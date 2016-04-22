using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using com.amphora.cayenne.entity;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.log4j;
using NSRiskManager;


namespace NSRMCommon {
    public class WrappedWinDef {
        #region delegates
        delegate bool LoggedOperationHandler();

        public event BeforeWindowClosingHandler beforeWindowClosingEvent;
        public delegate void BeforeWindowClosingHandler(object sender, string  windowName, string ownerName, int windowId);
        #endregion

        #region constants
        public const string VALUE_NONE = "NONE";
        #endregion

        #region fields
        static readonly java.lang.Character YES_RESPONSE = new java.lang.Character('Y');
        public const string WINDOW_NAME = "Untitled 1";
        readonly List<WrappedWinPivotDef> PivotWindowDefinitons = new List<WrappedWinPivotDef>();
        #endregion

        #region ctors
        WrappedWinDef() { }
        public WrappedWinDef(RiskmgrWinDef rwd) {
            this.WindowDefinition = rwd;
            buildList();
        }

        public WrappedWinDef(string p) {
            ObjectContext ctx = LocalCayenneService.sharedInstance.newObjectContext();

            WindowDefinition = (RiskmgrWinDef) ctx.newObject(typeof(RiskmgrWinDef));
            this.windowTitle = p;
            //this.windowFrame = "EMPTY";
            //this.portfolioSplitPosition = 0;
            this.description = "description";
            this.portfolioPath = "EMPTY";
            this.selectedIndex = 0;
            if (WindowDefinition != null && WindowDefinition.getOwnerName() != null)
                this.ownerName = WindowDefinition.getOwnerName(); //"NONE"
            else
                this.ownerName = Environment.UserName;
            //this.lowerSplitPosition = 0;
            //this.pnlSplitPosition = 0;
        }
        #endregion

        #region properties

        [Browsable(false)]
        public RiskmgrWinDef WindowDefinition { get; private set; }

        [DisplayName("ID")]
        public int windowId { get { return WindowDefinition.getWinId() == null ? -1 : WindowDefinition.getWinId().intValue(); } }
        [DisplayName("Title")]
        public string windowTitle { get { return WindowDefinition.getWindowTitle(); } set { WindowDefinition.setWindowTitle(string.IsNullOrEmpty(value) ? WrappedWinPivotDef.NULL_PIVOT_NAME : value); } }
        [DisplayName("Description")]
        public string description { get { return WindowDefinition.getDescription(); } set { WindowDefinition.setDescription(string.IsNullOrEmpty(value) ? "default window" : value); } }
        [DisplayName("Owner")]
        public string ownerName { get { return WindowDefinition.getOwnerName(); } set { WindowDefinition.setOwnerName(string.IsNullOrEmpty(value) ? VALUE_NONE : value); } }
        [DisplayName("Public")]
        public bool isPublic { get { return (WindowDefinition.getIsPublic() != null) ? (WindowDefinition.getIsPublic().equals(YES_RESPONSE)) : false; } set { WindowDefinition.setIsPublic(new java.lang.Character(value ? 'Y' : 'N')); } }
        [Browsable(false)]
        public string portfolioPath { get { return WindowDefinition.getPortPath(); } set { WindowDefinition.setPortPath(string.IsNullOrEmpty(value) ? "default window" : value); } }


        [Browsable(false)]
        public int transactionId { get { return WindowDefinition.getTransId() == null ? 0 : WindowDefinition.getTransId().intValue(); } set { WindowDefinition.setTransId(new java.lang.Integer(value)); } }

         
        [Browsable(false)]
        public WinDefContext contextOwner { get; internal set; }

        [Browsable(false)]

        public int selectedIndex { 
            get 
            { 
                    if( WindowDefinition.getSelectedIndex() == null  )
                        WindowDefinition.setSelectedIndex(new java.lang.Integer(0));

                     return WindowDefinition.getSelectedIndex().intValue(); 
            } 
            set 
            {
                WindowDefinition.setSelectedIndex(new java.lang.Integer(value)); 
            } 
        }

        #endregion

        #region methods
        public IEnumerable<WrappedWinPivotDef> allPivotDefinitions() {

            return this.PivotWindowDefinitons;

        }

        public bool CheckForContextChanges()
        {
          
            foreach (WrappedWinPivotDef pivotDefinition in allPivotDefinitions())
            {
                if (pivotDefinition.pivotDefinition.getObjectContext().hasChanges())
                {
                    Console.WriteLine("**** pivot has changes" +  WindowDefinition.getWindowTitle());
                    return true;
                }
            }

            ObjectContext context = WindowDefinition.getObjectContext();

            if (context == null)
            {
                return false;
            }
            if (context.hasChanges())
            {

                return true;
            }
            
           
            return false;
        }

        void buildList() {
            PivotWindowDefinitons.Clear();
            int n;
            java.util.List aList;

            if ((aList = WindowDefinition.getRiskManagerWinPivotDefs()) != null && (n = aList.size()) > 0)
                for (int i = 0; i < n; i++)
                {
                    WrappedWinPivotDef newDefinition = new WrappedWinPivotDef(aList.get(i) as RiskmgrWinPivotDef);
                    PivotWindowDefinitons.Add(newDefinition);
                }
            if (PivotWindowDefinitons.Count > 1)
                PivotWindowDefinitons.Sort(sortByTabIndex);
        }

        int sortByTabIndex(WrappedWinPivotDef o1,WrappedWinPivotDef o2) {
            return o1.tabIndex - o2.tabIndex;
        }

        public void addPivotDefinition(WrappedWinPivotDef wwpd) {
           
            if (WindowDefinition.getObjectContext() != wwpd.pivotDefinition.getObjectContext())
                wwpd.pivotDefinition.setObjectContext(WindowDefinition.getObjectContext());
            PivotWindowDefinitons.Add(wwpd);
            PivotWindowDefinitons.Sort(sortByTabIndex);
            WindowDefinition.addToRiskManagerWinPivotDefs(wwpd.pivotDefinition);
        }

        bool doWindowDeletion() {
            ObjectContext context = this.WindowDefinition.getObjectContext();
            context.deleteObject(this.WindowDefinition);
            context.commitChanges();
            return true;
        }

        public bool doWindowUpdate() 
        {

            ObjectContext context = this.WindowDefinition.getObjectContext();

            bool isNew = false;

            if (this.windowId <= 0)
            {
                context.registerNewObject(this.WindowDefinition);
                isNew = true;
            }
            try 
            {
                foreach (WrappedWinPivotDef wwpd in this.PivotWindowDefinitons)
                {
                  
                    ObjectContext localPivotContext = wwpd.pivotDefinition.getObjectContext();
                    localPivotContext.commitChanges();
                    localPivotContext.commitChangesToParent();
                   
                }
                context.commitChangesToParent();
                context.commitChanges();
                
                Util.show(MethodBase.GetCurrentMethod(),(isNew ? "Inserted new" : "Updated ") + " window: " + this.windowTitle + " [ID=" + this.windowId + "]");
              
                return true;
            } 
            catch (Exception ex) 
            {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
            return false;
        }

        bool performLoggedOperation(LoggedOperationHandler loh,bool logVerbose) {
            ObjectContext ctx;
            Logger logger = null;
            Level prevLevel = Level.OFF;
            bool ret;

            if (loh == null)
                throw new ArgumentNullException("loh",typeof(LoggedOperationHandler).FullName + "-instance is null!");
            ctx = this.WindowDefinition.getObjectContext();
            if (logVerbose) {
                logger = Logger.getRootLogger();
                prevLevel = logger.getLevel();
                logger.setLevel(Level.ALL);
            }
            ret = loh();
            if (logVerbose && logger != null)
                logger.setLevel(prevLevel);
            return ret;
        }

        public void deleteWindow() {
            performLoggedOperation(new LoggedOperationHandler(doWindowDeletion), false);
        }

        public void saveChangesWithoutPrompting()
        {
            performLoggedOperation(new LoggedOperationHandler(doWindowUpdate), false);
        }

      

        public void saveChanges()
        {

            if (CheckForContextChanges())
                beforeWindowClosingEvent(this, this.windowTitle, this.ownerName, this.windowId); 
                
        }

        internal void populateDefaultValues() {
            if (WindowDefinition != null && WindowDefinition.getOwnerName() != null)
                this.ownerName = WindowDefinition.getOwnerName(); 
            else
                this.ownerName = Environment.UserName;
            this.description = "description";
            this.isPublic = false;
            //this.lowerSplitPosition = 317;//System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2;//317
            //this.pnlSplitPosition = 541;
            //this.portfolioSplitPosition = 294;
            this.portfolioPath = "";
            //int width = 1180;
            ////if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width > 1450)
            ////    width = ((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) / 2);
            ////else
            ////    width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 60);
            //this.windowFrame = "{X=0,Y=0,Width= " + width
            //    + ",Height= " + (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100).ToString() + "}";//"{X=2299,Y=13,Width=1524,Height=818}";
        }
        #endregion
    }
}