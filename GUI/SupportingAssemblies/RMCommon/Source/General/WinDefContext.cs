using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using com.amphora.cayenne.entity;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.cayenne.exp;
using org.apache.cayenne.query;
using org.apache.log4j;
using java.util;


namespace NSRMCommon {

    public class WinDefContext {
        #region fields
        static ObjectContext sharedWinDefContext;
        public static bool logVerbose = false;
        static readonly WinDefContext defaultContext = new WinDefContext(true);
        bool isDefault;
        #endregion

        #region ctors
        WinDefContext() : this(false) { }

        WinDefContext(bool bDefCtx) { isDefault = bDefCtx; }

        public WinDefContext(WrappedWinDef wwd)
            : this(false) {
            if (wwd == null)
                throw new ArgumentNullException("wwd",typeof(WrappedWinDef).FullName + " is null!");
            wrappedDef = wwd;
        }
        #endregion

        #region properties
        public RiskmgrWinDef windowDefinition { get { return wrappedDef.WindowDefinition; } }
        public WrappedWinDef wrappedDef { get; private set; }
        #endregion

        #region methods
        void addPivotDefinition(RiskmgrWinPivotDef pdef) {
            throw new InvalidOperationException("fix this");
        }

        public static List<WrappedWinDef> findAllWindowDefinitions() 
        {
            List<WrappedWinDef> ret = new List<WrappedWinDef>();
            SelectQuery query;
            java.util.List defs;
            int n;

            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();
            query = new SelectQuery(typeof(RiskmgrWinDef));
            defs = sharedWinDefContext.select(query);
            if (defs != null && (n = defs.size()) > 0)
                for (int i = 0 ;i < n ;i++)
                    ret.Add(new WrappedWinDef(defs.get(i) as RiskmgrWinDef));
            return ret;
        }


        public static List<WrappedWinDef> createOwnerSpecificWinDefDatasourceExcludePublic()
        {
            List<WrappedWinDef> allWindows = WinDefContext.findAllWindowDefinitions();
            List<WrappedWinDef> windowsToOpen = new List<WrappedWinDef>();

            foreach (WrappedWinDef window in allWindows)
            {
                if (window.ownerName == WinDefContext.retrieveCurrentUserName())
                    windowsToOpen.Add(window);
            }

            return windowsToOpen;
        }


        public static List<WrappedWinDef> createOwnerSpecificWinDefDatasource(List<int> alreadyOpenedWindows)
        {
            List<WrappedWinDef> allWindows = WinDefContext.findAllWindowDefinitions();
            List<WrappedWinDef> windowsToOpen = new List<WrappedWinDef>();

            foreach (WrappedWinDef window in allWindows)
            {
                
                if (alreadyOpenedWindows.Contains(window.windowId))
                {
                        continue;
                }

                if (window.isPublic)
                    windowsToOpen.Add(window);
                else if (window.ownerName == WinDefContext.retrieveCurrentUserName())
                    windowsToOpen.Add(window);
            }

            return windowsToOpen;
        }

        public static string LoggedInUserInit = null;

        public static string retrieveCurrentUserName()
        {
            java.util.List loggedInIctsUserList;

            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

            loggedInIctsUserList = sharedWinDefContext.select(
                  new SelectQuery(
                      typeof(IctsUser),
                      ExpressionFactory.matchExp("userLogonId", Environment.UserName)));

            if (loggedInIctsUserList != null && loggedInIctsUserList.size() > 0)
            {
                LoggedInUserInit = (loggedInIctsUserList.get(0) as IctsUser).getUser_init();
            }

            if (LoggedInUserInit == null)
                LoggedInUserInit = Environment.UserName;

            return LoggedInUserInit;

        }


        public static WinDefContext findWindowById(int windowId)
        {
            try
            {

                retrieveCurrentUserName();

                java.util.List alist;

                if (sharedWinDefContext == null)
                    sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

                Map map = new HashMap();
                map.put("winId", new java.lang.Integer(windowId));

                alist = sharedWinDefContext.select(
                      new SelectQuery(
                          typeof(RiskmgrWinDef),
                          ExpressionFactory.matchAllExp(map, Expression.EQUAL_TO)));

                if (alist != null && alist.size() > 0 && LoggedInUserInit != null)
                {
                    (alist.get(0) as RiskmgrWinDef).setOwnerName(LoggedInUserInit);
                    return new WinDefContext(new WrappedWinDef(alist.get(0) as RiskmgrWinDef));
                }

            }
            catch (Exception ex)
            { 
            }
            return null;
        }


        public static WinDefContext findWindowNamed(Tuple<string, string> windowName)
        {
            java.util.List alist;

            Level prevLevel = Level.OFF;

            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

            if (logVerbose)
            {
                prevLevel = Logger.getRootLogger().getLevel();
                Logger.getRootLogger().setLevel(Level.ALL);
            }
            try
            {
                retrieveCurrentUserName();


                Map map = new HashMap();
                map.put("windowTitle", windowName.Item1);
                map.put("ownerName", windowName.Item2);

                alist = sharedWinDefContext.select(
                   new SelectQuery(
                       typeof(RiskmgrWinDef),
                       ExpressionFactory.matchAllExp(map, Expression.EQUAL_TO)));

                if (alist != null && alist.size() > 0 && LoggedInUserInit != null)
                {
                    (alist.get(0) as RiskmgrWinDef).setOwnerName(LoggedInUserInit);
                    return new WinDefContext(new WrappedWinDef(alist.get(0) as RiskmgrWinDef));
                }

            }
            catch (Exception ex)
            {
                Util.show(MethodBase.GetCurrentMethod(), ex);
            }
            finally
            {
                if (logVerbose)
                    Logger.getRootLogger().setLevel(prevLevel);
            }
            return null;
        }



        public static WinDefContext findWindowNamed(string windowName)
        {
            java.util.List alist; 
    
            Level prevLevel = Level.OFF;

            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

            if (logVerbose) 
            {
                prevLevel = Logger.getRootLogger().getLevel();
                Logger.getRootLogger().setLevel(Level.ALL);
            }
            try 
            {
                retrieveCurrentUserName();


                Map map = new HashMap();
                map.put("windowTitle", windowName);
                map.put("ownerName", LoggedInUserInit);


                alist = sharedWinDefContext.select(
                   new SelectQuery(
                       typeof(RiskmgrWinDef),
                       ExpressionFactory.matchAllExp(map, Expression.EQUAL_TO)));

                if (alist != null && alist.size() > 0 && LoggedInUserInit != null)
                {
                    (alist.get(0) as RiskmgrWinDef).setOwnerName(LoggedInUserInit);
                    return new WinDefContext(new WrappedWinDef(alist.get(0) as RiskmgrWinDef));
                }
                
            } 
            catch (Exception ex) 
            {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            } 
            finally 
            {
                if (logVerbose)
                    Logger.getRootLogger().setLevel(prevLevel);
            }
            return null;
        }



        public static WinDefContext createWindowDefinition(Tuple<string, string> winName)
        {

            WinDefContext ret;


            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

            if ((ret = findWindowNamed(winName)) == null)
            {
                ret = createDefault();
                ret.wrappedDef.windowTitle = winName.Item1;
                ret.wrappedDef.ownerName = winName.Item2;
            }

            return ret;
        }


        public static WinDefContext createWindowDefinition(int windowId)
        {

            WinDefContext ret;


            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();

            if ((ret = findWindowById(windowId)) == null)
            {
                ret = createDefault();
                //ret.wrappedDef.windowTitle = winName.Item1;
                //ret.wrappedDef.ownerName = winName.Item2;
            }

            return ret;
        }


        public static WinDefContext createWindowDefinition(string winName) {
            
            WinDefContext ret;
        

            if (sharedWinDefContext == null)
                sharedWinDefContext = LocalCayenneService.sharedInstance.newObjectContext();
       
            if ((ret = findWindowNamed(winName)) == null) 
            {
                ret = createDefault();
                ret.wrappedDef.windowTitle = winName;
            }
       
            return ret;
        }

        public static WinDefContext createDefault() {
            return new WinDefContext().createDefaultPrivate();
        }

        WinDefContext createDefaultPrivate() {
            WrappedWinPivotDef wwpd;

            wrappedDef = new WrappedWinDef(WrappedWinDef.WINDOW_NAME );
            wrappedDef.populateDefaultValues();
            if (LoggedInUserInit == null)
                LoggedInUserInit = Environment.UserName;
            wrappedDef.ownerName = LoggedInUserInit;
            wrappedDef.addPivotDefinition(
                wwpd = WrappedWinPivotDef.createDefault2(
                    wrappedDef.windowTitle,
                    wrappedDef.WindowDefinition.getObjectContext()));
            return this;
        }
        #endregion
    }
}