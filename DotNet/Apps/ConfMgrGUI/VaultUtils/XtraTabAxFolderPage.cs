using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraTab;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace VaultUtils
{
    public class XtraTabAxFolderPage : XtraTabPage
    {
        private AXFolderPnl axFolderPanel = null;
        private SempraDocWs sempraDocWs = null;
        private AxFolder axFolder = null;
        private DataTable axFolderMapping = null;
        private string defaultTradeId = null;

        public string DefaultTradeId
        {
            set 
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    defaultTradeId = value;
                    foreach (Control control in this.Controls)
                    {
                        if (control is AXFolderPnl)
                        {
                            ((AXFolderPnl)control).DefaultTradeId = value;
                        }
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        public void SetADefaultValue(string fieldName, string value)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control is AXFolderPnl)
                    {
                        ((AXFolderPnl)control).SetADefaultValue(fieldName, value);
                    }
                }
            }
            finally
            {
            }
        }

        public void GetDocuments()
        {
            foreach (Control control in this.Controls)
            {
                if (control is AXFolderPnl)
                {
                    ((AXFolderPnl)control).GetDocuments();
                }
            }
        }

        public void SaveDocuments(string path, string fileName)
        {
            foreach (Control control in this.Controls)
            {
                if (control is AXFolderPnl)
                {
                    ((AXFolderPnl)control).SaveDocuments(path, fileName);
                }
            }
        }
        

        public AXFolderPnl AxFolderPanel
        {
            get { return axFolderPanel; }
            set { axFolderPanel = value; }
        }

        public XtraTabAxFolderPage(AxFolder axFolder, DataTable axFolderMapping, ref SempraDocWs sempraDocWs)
        {
            this.axFolder = axFolder;
            this.axFolderMapping = axFolderMapping;
            this.Text = axFolder.DisplayName;
            this.sempraDocWs = sempraDocWs;
            axFolderPanel = new AXFolderPnl(axFolder, axFolderMapping, ref sempraDocWs);
            axFolderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(axFolderPanel);
        }
    }
}
