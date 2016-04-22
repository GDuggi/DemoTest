using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NSRiskMgrCtrls;
using NSRMCommon;
using com.amphora.cayenne.entity.support;

namespace NSRiskManager
{
    public class RiskManagerWindowManager:ApplicationContext
    {
        public static readonly IDictionary<int, RiskManagerForm> windows = new Dictionary<int, RiskManagerForm>();
        static readonly List<int> newWindows = new List<int>();
        public static List<int> openWindowsToKeep = new List<int>();
        static IPortfolioEntityDTOSupport portEntSupport;

        public RiskManagerWindowManager()
        {
          
            initApplication();
        }

        //to be called at the first start of the program
        private void initApplication()
        {
         
            AmphoraFieldSelector.buildMapFrom(typeof(RiskGroup));
            SharedContext.setup();
            RabbitRouter.shared.startup();

            PositionEntityCacheImpl.Builder.positionCache().init(LocalCayenneService.sharedInstance, RabbitRouter.shared);

            portEntSupport = PortfolioEntityDTOSupportImpl.Builder.portfolioSupport();
            portEntSupport.init(LocalCayenneService.sharedInstance, RabbitRouter.shared);

            UOMEntityCacheImpl.Builder.uomCache().init(LocalCayenneService.sharedInstance, RabbitRouter.shared);

            reopenPreviousWindows();
           
        
        }


        public static bool inProcessOfQuittingApplication = false;
      

        public static void quitApplicationCompletely()
        {
            List<int> keys  = windows.Keys.ToList();

            inProcessOfQuittingApplication = true;

            foreach (int windowId in keys)
            {
                try
                {
                    object o = new object();
                    lock(o)
                    {
                        closeWindowForm(windowId);
                    }

                }
                 
                catch (Exception e)
                { 
                }
            }

         
        }

       
        public static void restoreOpenWindowSettings()
        {
            try
            {
                NSRiskManager.Properties.Settings.Default.Reload();
                string commaSeparatedList = NSRiskManager.Properties.Settings.Default.open_windows;

                List<string> parsedWindowIds = NSRiskManager.Properties.Settings.Default.open_windows.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                openWindowsToKeep = parsedWindowIds.Select(int.Parse).ToList();
            }
            catch (Exception ex)
            { 
            }
           
        }

        public static void shutdownApplication()
        {
            saveOpenWindowSettings();

            RabbitRouter.shared.shutdown();
            ChannelUtil.shutdown();
            System.Environment.Exit(0);
        }

        public static void closeWindowForm(int windowId)
        {
            if (windows.ContainsKey(windowId))
            {
                RiskManagerForm formToClose = windows[windowId];
                formToClose.stopDisplayTimer();
                formToClose.Close();
                
            }
           
        }


        public static void saveWindow(int windowId)
        {
            if (windows.ContainsKey(windowId))
            {

                RiskManagerForm formToSave = windows[windowId];

                if (newWindows.Contains(windowId))
                    saveWindowAs(formToSave.windowDef.windowTitle, formToSave.windowDef.windowId); //user must be prompted if he wants to rename the window

                 else 
                    formToSave.saveLayoutToDatabaseWithoutPrompting();
            }
        }

        public static bool saveWindowAs(string oldWindowName, int windowId)
        {

            bool saved = false;

            if (windows.ContainsKey(windowId))
            {

                RiskManagerForm formToSave = windows[windowId];
 
                FormNamer formNamer;
                WinDefContext winDefContext;


                formNamer = new FormNamer();
                formNamer.validateName += ValidateWindowName;
                formNamer.formName = oldWindowName;
                formNamer.windowId = windowId;
                formNamer.oldWindowName = oldWindowName;

                if (formNamer.ShowDialog() == DialogResult.OK)
                {
                    string newWindowName = formNamer.formName;

                    try
                    {
                        winDefContext = WinDefContext.findWindowNamed(oldWindowName);
                        if (winDefContext == null)
                        {
                            //window was never saved before - save as new window  
                            addWindowWithName(new Tuple < string, string> (newWindowName, winDefContext.windowDefinition.getOwnerName()));
                            formToSave.saveLayoutToDatabaseWithoutPrompting();
                        }
                        else
                        {
                            //rename the window
                            winDefContext.windowDefinition.setWindowTitle(newWindowName);
                            formToSave.formName = newWindowName;
                            formToSave.windowDef.windowTitle = newWindowName;
                            formToSave.saveLayoutToDatabaseWithoutPrompting();
                        }

                        if (newWindows.Contains(windowId))
                            newWindows.Remove(windowId);

                        openWindowsToKeep.Remove(windowId);
                        addWindowToOpenWindows(windowId);
                        saveOpenWindowSettings();

                        saved = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error occurred");
                    }

                }

                formNamer.validateName -= ValidateWindowName;
            }

            return saved;
        }

        //returns windowId
        public static int createNewWindow(int defaultPortId=0)
        {
            string newWindowName =findNextAvailableNewWindowName();
            WinDefContext context = addWindowWithName(new Tuple <string, string> (newWindowName, WinDefContext.retrieveCurrentUserName()), defaultPortId);
            newWindows.Add(context.windowDefinition.getWinId().intValue());

            return context.windowDefinition.getWinId().intValue();
        }

        public static void openExistingWindowWithParams(WrappedWinDef windowDef)
        {
            if(!windows.ContainsKey(windowDef.windowId))
                 addWindow(new RiskManagerForm(new WinDefContext(windowDef), false, 0));
        }

        public static void saveExistingWindow(WrappedWinDef windowDef)
         {
             int windowId = windowDef.windowId;

             if (windows.ContainsKey(windowId))
             {
                 RiskManagerForm formToUpdate = windows[windowId];
                 formToUpdate.windowDef.windowTitle = windowDef.windowTitle;
                 formToUpdate.windowDef.saveChangesWithoutPrompting();
             }

             windowDef.saveChangesWithoutPrompting();
         }
        
        

        public static void deleteExistingWindow(WrappedWinDef windowDef)
        {
            int windowId = windowDef.windowId;

            if (openWindowsToKeep.Contains(windowId))
                openWindowsToKeep.Remove(windowId);

            if (newWindows.Contains(windowId))
                newWindows.Remove(windowId);

            //window is open, close it first
            if (windows.ContainsKey(windowId))
            {
                RiskManagerForm formToDelete = windows[windowId];

  
                formToDelete.Close();
            }


            windowDef.deleteWindow();
        }



        public static void openNewWindowByPortfolioNum()
        {

            try
            {
                if (PortfolioIDSelector.formCurrentlyOpen)
                {
                    MessageBox.Show("The open by portfolio id window is already opened. Please close the previous window first to open a new one");
                    return;
                }

                PortfolioIDSelector idSelector = new PortfolioIDSelector();
                

                if (idSelector.ShowDialog() == DialogResult.OK)
                {
                    int windowId = createNewWindow(idSelector.PortfolioId);
                }

                idSelector.Dispose();
            }

            catch (Exception ex)
            { 
            }
        }

        public static void openExistingWindow()
        {
            if (OpenFormChooser.formCurrentlyOpen)
            {
                MessageBox.Show("The open form window is already opened. Please close the previous window first to open a new one");
                return;
            }

            OpenFormChooser openFormChooser = new OpenFormChooser(openWindowsToKeep);

            if (openFormChooser.ShowDialog() == DialogResult.OK)
            {
                WrappedWinDef selectedWindow = openFormChooser.getSelectedWindow();

                if (selectedWindow != null)
                    addWindow(new RiskManagerForm(new WinDefContext(selectedWindow), false, 0));
            }

            openFormChooser.Dispose();

        }
        public static void manageExistingWindows()
        {
            if (FormManager.formCurrentlyOpen)
            {
                MessageBox.Show("Form manager window is already opened. Please close the previous window first to open a new one");
                return;
            }

            FormManager formManager = new FormManager();

            formManager.ShowDialog();

            formManager.Dispose();

        

        }

        // save after every operation where open windows might change
        // this might help restore the windows if the application crashes
        private static void saveOpenWindowSettings()
        {
        
            NSRiskManager.Properties.Settings.Default.open_windows = string.Join(",", openWindowsToKeep);
            NSRiskManager.Properties.Settings.Default.Save();
        }


        private static string findNextAvailableNewWindowName()
        {
             string baseName = "Untitled";
             int count = 0;
             string fullName;

            do
            {
                count++;

                fullName = baseName + count;

                bool foundName = false;
                foreach(RiskManagerForm form in windows.Values)
                {
                    if (form.windowDef.windowTitle == fullName)
                    {
                        foundName = true;
                        break;
                    }
                }
                
                //don't need to check windefcontext for name uniqueness,
                //as we don't allow to save with the name "Untitled*" anyway
                if (!foundName) 
                    return fullName;

            }
            while(true);
        }


        static public void arrangeWindows()
        {
            RiskManagerForm rmf;
            int x = 0, y = 0;

            foreach (int akey in windows.Keys)
            {
                rmf = windows[akey];
                if (rmf.WindowState == FormWindowState.Minimized)
                {
                    rmf.WindowState = FormWindowState.Normal;
                    if (rmf.Location.X == -32000)
                        rmf.SetBounds(0, 0, 0, 0, BoundsSpecified.X);
                    if (rmf.Location.Y == -32000)
                        rmf.SetBounds(0, 0, 0, 0, BoundsSpecified.Y);
                }
                else if (rmf.WindowState == FormWindowState.Normal)
                {
                    rmf.SetBounds(x, y, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                    x += 25;
                    y += 25;
                }
            }
        }


        static public void showAboutBox()
        {
            new AboutBox1().Show();
        }



        static private bool ValidateWindowName(object sender, string oldName, string newName, int windowID)
        {
            //untitled* is a reserved name
            if (newName.StartsWith("Untitled") || newName.StartsWith("untitled"))
                return false;

            else if (oldName == newName)
                return true;

            foreach(RiskManagerForm form in windows.Values)
            {
                if(form.windowDef.windowTitle ==newName )
                    return false;
            }

            if (WinDefContext.findWindowNamed(new Tuple<string, string>(newName, WinDefContext.retrieveCurrentUserName())) != null)
                return false;

            return true;
        }


        private static WinDefContext addWindowWithName(Tuple<string, string> windowName, int defaultPortId=0)
         {
             WinDefContext windDefContext;

             windDefContext = WinDefContext.createWindowDefinition(windowName);

             addWindow(new RiskManagerForm(windDefContext, true, defaultPortId));

             return windDefContext;
         }


        private static WinDefContext addWindowWithId(int windowId)
        {
            WinDefContext windDefContext;

            windDefContext = WinDefContext.createWindowDefinition(windowId);

            addWindow(new RiskManagerForm(windDefContext, true, 0));

            return windDefContext;
        }

        private static void addWindow(RiskManagerForm riskManagerForm)
        {
          

            riskManagerForm.windowClosingEvent += new WindowClosingHandler(someWindowClosingEventHandler);
            riskManagerForm.windowDef.beforeWindowClosingEvent +=  new WrappedWinDef.BeforeWindowClosingHandler(beforeSomeWindowClosingEventHandler);

            PortfolioEntityDTOSupportImpl.Builder.portfolioSupport().registerContainer(riskManagerForm);

            riskManagerForm.Show();

            int key = riskManagerForm.windowDef.windowId;


            if (windows.ContainsKey(key))
            {
                windows[key] = riskManagerForm;
            }
            else
                windows.Add(key, riskManagerForm);

            addWindowToOpenWindows(key);
        }


        private static void addWindowToOpenWindows(int windowID)
        {
            if (!openWindowsToKeep.Contains(windowID))
                openWindowsToKeep.Add(windowID);

            saveOpenWindowSettings();
        }

        static void beforeSomeWindowClosingEventHandler(object sender, string windowName, string owner, int windowID)
        {
            //this is a  window already marked for deletion.
            if (!openWindowsToKeep.Contains(windowID))
                return;

            RiskManagerForm formToSave = windows[windowID];

            //new window, show save as prompt
            if (newWindows.Contains(windowID))
            {
                bool saved = saveWindowAs(windowName, windowID);
               if (!saved)
               {
                   //this is a new window and we didn't save it.
                   //need to delete window from database, so no trace of new window exists in db
                   openWindowsToKeep.Remove(windowID);
                   formToSave.windowDef.deleteWindow();
                   saveOpenWindowSettings();
               }
            }
            else
            {
                
                if (formToSave.windowDef.CheckForContextChanges())
                {

                    if (MessageBox.Show("Save window '" + windowName + "'?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        saveWindow(windowID);
                    }

                }

                //unless the user explicityly quits the application the fact of closing the window means
                // he doesnt' want to see it on next application startup anymore.
                if (!inProcessOfQuittingApplication)
                    openWindowsToKeep.Remove(windowID);
                saveOpenWindowSettings();
            }
        }

        static void someWindowClosingEventHandler(object sender, WindowClosingEventArgs ea)
        {
            int key = ea.windowID;


            if (windows.ContainsKey(key))
            {
                RiskManagerForm formToDelete = windows[key];
                formToDelete.stopDisplayTimer();

                windows.Remove(key);

            }

            if (windows.Count == 0)
            {
                shutdownApplication();
            }
            
        }

        static void reopenPreviousWindows()
        {
            restoreOpenWindowSettings();

            try
            {

                List<int> openWindowsToKeepCopy = openWindowsToKeep;

                if (openWindowsToKeepCopy.Count > 0)
                {
                    foreach (int openWindowId in openWindowsToKeepCopy)
                    {

                        addWindowWithId(openWindowId);
                    }
                }
                else
                {
                    createNewWindow();
                }

            }
            catch (Exception ex)
            { 
            
            }
        }
    }
}
