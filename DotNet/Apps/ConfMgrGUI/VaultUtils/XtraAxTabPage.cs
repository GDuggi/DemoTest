using System;
using System.Collections.Generic;
using System.Text;
using Sempra.Confirm.InBound.ImageEdit;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraTab;
using DevExpress.XtraEditors;
using System.Net;

namespace VaultUtils
{
    public class XtraAxTabPage : XtraTabPage, IAXTab
    {
        enum DocType
        {
            TIF, RTF, HTML
        };
        private const string PROJ_FILE_NAME = "XtraAxTabPage";
        private string resourceId = null;
        private string fileName = null;
        private PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private int checkPrint = 0;
        private VaultRTFEditor richTextBoxPrintCtrl1 = null;


        public string ResourceId
        {
            get { return resourceId; }
            set { resourceId = value; }
        }

        private AxFolder axFolder = null;

        public XtraAxTabPage(AxFolder axFolder, string resourceId, ref SempraDocWs ws)
        {
            InitializeComponent();

            this.axFolder = axFolder;
            string fileType = "";

            byte[] docStream = new byte[32768];

            this.resourceId = resourceId;
            this.Name = "XtraAxTabPage_" + resourceId;

            var result = ws.GetLatestDocStream(axFolder.FolderName, resourceId, axFolder.DslName, out docStream, out fileType);
            if (docStream.Length > 0)
            {
                AddDocViewer(docStream);
            }
        }

        private void AddDocViewer(byte[] docStream)
        {
            DocType fileDocType = GetDocumentType(docStream);

            switch (fileDocType)
            {
                case DocType.TIF:
                    {
                        try
                        {
                            TifEditor editor = new TifEditor();
                            editor.Dock = System.Windows.Forms.DockStyle.Fill;
                            editor.Edit = false;
                            editor.ExitDelegate = null;
                            editor.ImageFileName = null;
                            editor.Location = new System.Drawing.Point(0, 0);
                            editor.SaveAsFileName = null;
                            editor.ScaleFactor = 1;
                            editor.Size = new System.Drawing.Size(1148, 422);
                            editor.TabIndex = 0;
                            editor.TransDelegate = null;
                            editor.UserName = null;
                            editor.LoadImage(new MemoryStream(docStream));
                            this.Controls.Add(editor);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("An error occurred while loading file: " + fileName + "." + Environment.NewLine +
                                "Error CNF-547 in " + PROJ_FILE_NAME + ".AddDocViewer(): " + ex.Message,
                                 "Inbound Tab Page", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                case DocType.HTML:
                    {
                        WebBrowser webBrowser = new WebBrowser();
                        webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
                        webBrowser.TabIndex = 0;
                        this.Controls.Add(webBrowser);
                        StringBuilder contents = new StringBuilder();
                        char[] str = new char[docStream.Length];
                        for (int i = 0; i < docStream.Length; i++)
                        {
                            str[i] = (char)docStream[i];
                        }
                        contents.Append(str);

                        webBrowser.Navigate("about:blank");

                        webBrowser.Document.Write(contents.ToString());
                        webBrowser.Refresh();

                        break;
                    }
                case DocType.RTF:
                    {
                        
                        VaultRTFEditor richTextBox = new VaultRTFEditor();
                        richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
                        richTextBox.LoadFile(new MemoryStream(docStream), RichTextBoxStreamType.RichText);
                        this.Controls.Add(richTextBox);
                        break;
                    }
            }
        }

        private void CreateImageFile(byte[] docStream, string fileName)
        {
            using (BinaryWriter binWriter =
            new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                binWriter.Write(docStream);
                binWriter.Close();
            }
        }

        private DocType GetDocumentType(byte[] docStream)
        {
            StringBuilder headerInfo = new StringBuilder();
            if (docStream.Length >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    headerInfo.Append((char)docStream[i]);
                }
            }

            if ((headerInfo != null) && (headerInfo.ToString().Equals("II*")))
            {
                return DocType.TIF;
            }
            else return ((DocType)Enum.Parse(typeof(DocType), axFolder.ViewerType, true));
        }

        #region IAXTab Members

        public void TransmitDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-417 in " + PROJ_FILE_NAME + ".TransmitDocument().");
        }

        public void PrintDocument()
        {
            foreach (Control control in this.Controls)
            {
                if ((control is TifEditor))
                {
                    TifEditor editor = (TifEditor)control;
                    editor.PrintImage();
                }
                else if (control is VaultRTFEditor)
                {
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        richTextBoxPrintCtrl1.Rtf = ((VaultRTFEditor)control).Rtf;
                        checkPrint = 0;
                        printDocument1.Print();
                    }
                }
                else if (control is WebBrowser)
                {
                    ((WebBrowser)control).ShowPrintDialog();
                }
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.ResumeLayout(false);

            richTextBoxPrintCtrl1 = new VaultRTFEditor();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            checkPrint = richTextBoxPrintCtrl1.Print(checkPrint, richTextBoxPrintCtrl1.TextLength, e);

            // Check for more pages
            if (checkPrint < richTextBoxPrintCtrl1.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
            return;
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            checkPrint = 0;
        }

        #region IAXTab Members


        public void SaveDocument(string filePath, string fileName)
        {
            fileName = this.Text + "_" + fileName + ".rtf";
            foreach (Control control in this.Controls)
            {
                if (control is VaultRTFEditor)
                {
                    richTextBoxPrintCtrl1.Rtf = ((VaultRTFEditor)control).Rtf;
                    if (richTextBoxPrintCtrl1.Rtf.Length > 0)
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                            richTextBoxPrintCtrl1.SaveFile(filePath + @"\" + fileName);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
