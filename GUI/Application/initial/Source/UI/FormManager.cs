using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NSRMCommon;
using NSRMLogging;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace NSRiskManager
{
    public partial class FormManager : DevExpress.XtraEditors.XtraForm
    {
        
        private const int ID_COLUMN = 0;
        private const int NAME_COLUMN = 1;
        private const int DESCRIPTION_COLUMN = 2;
        private const int OWNER_COLUMN = 3;
        private const int PUBLIC_COLUMN = 4;

        public static bool formCurrentlyOpen = false;



        public FormManager()
        {
            InitializeComponent();
        }

        public List<WrappedWinDef> getSelectedWindows()
        {

            int[] selectedRows = this.OpenWindowsMainGridView.GetSelectedRows();

            List<WrappedWinDef> selectedWindows = new List<WrappedWinDef>();

            foreach (int selectedRow in selectedRows)
                selectedWindows.Add( OpenWindowsMainGridView.GetRow(selectedRow) as WrappedWinDef);

            return selectedWindows;
        }

        public List<WrappedWinDef> getAllWindows()
        {
            return this.OpenWindowsMainGridView.DataSource as List<WrappedWinDef>;
        }

        void formLoad(object sender, EventArgs ea)
        {
            formCurrentlyOpen = true;

            refreshData();
      
        }


        private void refreshData()
        {
            OpenWindowsGridControl.BeginUpdate();
            OpenWindowsGridControl.DataSource = WinDefContext.createOwnerSpecificWinDefDatasourceExcludePublic();
            
         
            OpenWindowsMainGridView.Columns[ID_COLUMN].OptionsColumn.AllowEdit = false;
            OpenWindowsMainGridView.Columns[OWNER_COLUMN].Visible = false;
            OpenWindowsMainGridView.BestFitColumns();
            OpenWindowsGridControl.EndUpdate();
        }

        void OpenSelectedButtonClickedHandler(object sender, EventArgs e)
        {
            try
            {
                List<WrappedWinDef> selectedWindows = getSelectedWindows();

                foreach (WrappedWinDef window in selectedWindows)
                {

                    RiskManagerWindowManager.openExistingWindowWithParams(window);
                }

            }
            catch (Exception ex)
            { 
            }

            refreshData();
            this.BringToFront();
    
        }

        void DeleteSelectedButtonClickedHandler(object sender, EventArgs e)
        {
            try
            {
                List<WrappedWinDef> selectedWindows = getSelectedWindows();
                String allWindowNames = null;
                foreach (WrappedWinDef window in selectedWindows)
                {
                    RiskManagerWindowManager.deleteExistingWindow(window);
                    if (allWindowNames != null && allWindowNames.Length > 0)
                        allWindowNames += "," + window.windowTitle;
                    else
                        allWindowNames = window.windowTitle;
                }

                foreach (RiskManagerForm rmForm in RiskManagerWindowManager.windows.Values)
                {
                   rmForm.setStatusBarMessage("Deleted Window(s): " + allWindowNames +" successfully.", true);
                }
                refreshData();
                this.BringToFront();
                
            }
            catch (Exception exp)
            {
            }
        }

        void SaveSelectedButtonClickedHandler(object sender, EventArgs e)
        {
            try
            {
                List<WrappedWinDef> selectedWindows = getSelectedWindows();
                List<WrappedWinDef> allWindows = getAllWindows();

                string error = "";
                bool okToSave = validateSelectedChangesToSave(selectedWindows, allWindows, out  error);
                if (!okToSave)
                {
                    MessageBox.Show(error);
                    return;
                }

                foreach (WrappedWinDef window in selectedWindows)
                {
                    RiskManagerWindowManager.saveExistingWindow(window);
                    RiskManagerForm rmForm = RiskManagerWindowManager.windows[window.windowId];
                    rmForm.setStatusBarMessage("Window saved " + window.windowTitle + " successfully.", true);
                }

                refreshData();
                this.BringToFront();
            }
            catch (Exception exp)
            {
            }
        }

        void SaveAllButtonClickedHandler(object sender, EventArgs e)
        {
            try
            {
                List<WrappedWinDef> allWindows = getAllWindows();

                string error = "";
                bool okToSave = validateSelectedChangesToSave(allWindows, allWindows, out  error);
                if (!okToSave)
                {
                    MessageBox.Show(error);
                    return;
                }

                foreach (WrappedWinDef window in allWindows)
                {
                    RiskManagerWindowManager.saveExistingWindow(window);
                    RiskManagerForm rmForm = RiskManagerWindowManager.windows[window.windowId];
                    rmForm.setStatusBarMessage("Window saved " + window.windowTitle + " successfully.", true);
                }

                refreshData();
                this.BringToFront();
            }
            catch (Exception exp)
            {
            }
        }

        private bool validateSelectedChangesToSave(List<WrappedWinDef> windowsToSave, List<WrappedWinDef> allWindows,  out string error)
        { 
            error = "";

            List<string> windowNames = new List<string>();

            foreach (WrappedWinDef window in allWindows)
            {

                if(windowNames.Contains(window.windowTitle))
                {
                    error = "Didn't save selected windows: some windows have duplicate names";
                    return false;
                }

             

                windowNames.Add(window.windowTitle);


            }

            foreach (WrappedWinDef window in windowsToSave)
            {
                if (window.windowTitle.Contains("untitled") || window.windowTitle.Contains("Untitled"))
                {
                    error = "Didn't save selected windows: some windows that contain reserved word 'Untitled'";
                    return false;
                }
            }

            return true;

        }

    }
}