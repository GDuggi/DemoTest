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
    public partial class OpenFormChooser : DevExpress.XtraEditors.XtraForm
    {

        public static bool formCurrentlyOpen = false;

        private const int ID_COLUMN = 0;
        private const int NAME_COLUMN = 1;
        private const int DESCRIPTION_COLUMN = 2;
        private List<int> alreadyOpenedWindows;

        public OpenFormChooser(List<int> alreadyOpenedWindows)
        {
            this.alreadyOpenedWindows = alreadyOpenedWindows;
            InitializeComponent();
        }



        public WrappedWinDef getSelectedWindow()
        {

            int[] selectedRows = this.ForManagerGridView.GetSelectedRows();

            //multiselect is disabled now, so return only the first row.

            if (selectedRows.Length > 0)
                return ForManagerGridView.GetRow(selectedRows[0]) as WrappedWinDef;

            return null;
        }

       
        void formLoad(object sender, EventArgs ea)
        {

            formCurrentlyOpen = true;

            FormManagerGridControl.BeginUpdate();
            FormManagerGridControl.DataSource = WinDefContext.createOwnerSpecificWinDefDatasource(alreadyOpenedWindows);

            ForManagerGridView.Columns[ID_COLUMN].Visible = false;
            ForManagerGridView.Columns[DESCRIPTION_COLUMN].Visible = false;

            ForManagerGridView.BestFitColumns();
            FormManagerGridControl.EndUpdate();

        }

    
    }
}