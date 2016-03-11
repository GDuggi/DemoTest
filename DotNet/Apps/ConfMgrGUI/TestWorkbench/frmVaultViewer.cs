using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWorkbench
{
    public partial class frmVaultViewer : Form
    {
        private List<string> docTipList = new List<string>();
        private List<string> docList = new List<string>();
        private List<string> docType = new List<string>();
        private List<string> docViewerExtList = new List<string>();
        private List<frmViewerRtf> viewerRtfList = new List<frmViewerRtf>();
        private List<frmViewerPdf> viewerPdfList = new List<frmViewerPdf>();
        private List<frmViewerTif> viewerTifList = new List<frmViewerTif>();

        public enum DocViewerType
        {
            None = 0, Rtf, Pdf, Tif
        };

        public frmVaultViewer()
        {
            InitializeComponent();
        }

        private void frmVaultViewer_Load(object sender, EventArgs e)
        {
            this.Top = 200;
            this.Left = 800;
            InitDocList();

            docViewerExtList.Add("DOC");
            docViewerExtList.Add("DOCX");
            docViewerExtList.Add("RTF");
            docViewerExtList.Add("HTML");
            docViewerExtList.Add("TXT");

            xtraTabPageDoc.PageVisible = false;
            xtraTabPagePdf.PageVisible = false;
            xtraTabPageTif.PageVisible = false;
        }

        private void InitDocList()
        {
            string docLoc = "";
            listBoxDocuments.Items.Add("Contract");
            docTipList.Add("First contract : PDF 1 Page - Meta Data a:01, b:02");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\contract_pdf.pdf";
            docList.Add(docLoc);
            docType.Add("PDF");

            listBoxDocuments.Items.Add("Contract");
            docTipList.Add("Second contract : PDF - 4 pages - Meta Data c:03, d:04");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\20130204_1028292971_4pg_pdf.pdf";
            docList.Add(docLoc);
            docType.Add("PDF");

            listBoxDocuments.Items.Add("Contract Template");
            docTipList.Add("Second contract : docx e:05, f:06");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\0037_OIL.SWAP.FLO.FLO.ISDA.PARTY_docx.docx";
            docList.Add(docLoc);
            docType.Add("DOCX");

            listBoxDocuments.Items.Add("Dealsheet");
            docTipList.Add("Dealsheet : HTML g:07, h:08");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\7080-20121031-132958-INS_html.html";
            docList.Add(docLoc);
            docType.Add("HTML");

            listBoxDocuments.Items.Add("Misc Doc");
            docTipList.Add("Misc doc [template] : txt  i:09, j:10");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\0009_NGAS.PHYS.LONG.FORM_txt.txt";
            docList.Add(docLoc);
            docType.Add("TXT");

            listBoxDocuments.Items.Add("Inbound Doc");
            docTipList.Add("Broker data sheet : tif k:11, l:12");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\Test_Doc_02.tif";
            docList.Add(docLoc);
            docType.Add("TIF");

            listBoxDocuments.Items.Add("Inbound Doc");
            docTipList.Add("Broker Invoice - Tif file m:13, n:14");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\test_sempra.tif";
            docList.Add(docLoc);
            docType.Add("TIF");

            listBoxDocuments.Items.Add("Bill Of Lading");
            docTipList.Add("Transit status : tif  o:015, p:16");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\20140929_075918_112_1_tif.tif";
            docList.Add(docLoc);
            docType.Add("TIF");

            listBoxDocuments.Items.Add("Bill Of Lading");
            docTipList.Add("16-Aug-2015 : rtf q:17, r:18");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\0037_OIL.SWAP.FLO.FLO.ISDA.PARTY.A_rtf.rtf";
            docList.Add(docLoc);
            docType.Add("RTF");

            listBoxDocuments.Items.Add("Bill Of Lading");
            docTipList.Add("17-Aug-2015 : doc s:19, t:20");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\0000_OIL.NAPHTHA.PHYSICAL.ASIA_doc.doc";
            docList.Add(docLoc);
            docType.Add("DOC");

            listBoxDocuments.Items.Add("Cpty Paper");
            docTipList.Add("Cpty confirmation doc : html u:21, v:22");
            docLoc = @"\\cnf01file01\cnf01\Apps\VaultViewer_Demo\Sample Docs\3-20130828-131107-INS_html.html";
            docList.Add(docLoc);
            docType.Add("HTML");
        }

        private void listBoxDocuments_MouseMove(object sender, MouseEventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int index = listBoxControl.IndexFromPoint(new Point(e.X, e.Y));
            if (index != -1)
            {
                //string item = listBoxControl.GetItem(index) as string;
                string item = docTipList[index];
                toolTipController.ShowHint(item, listBoxControl.PointToScreen(new Point(e.X, e.Y)));
            }
            else
            {
                toolTipController.HideHint();
            }
        }

        private void listBoxDocuments_MouseLeave(object sender, EventArgs e)
        {
            toolTipController.HideHint();
        }

        private void listBoxDocuments_Click(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int index = listBoxControl.SelectedIndex;
            if (index != -1)
            {
                string docFileExt = docType[index];
                DocViewerType fileViewerType = GetViewerType(docFileExt);
                SetTabPageDisplay(fileViewerType, docTipList[index]);

                switch (fileViewerType)
                {
                    case DocViewerType.Rtf:
                        DisplayViewerRtf(index);
                        break;
                    case DocViewerType.Pdf:
                        DisplayViewerPdf(index);
                        break;
                    case DocViewerType.Tif:
                        DisplayViewerTif(index);
                        break;
                    default:
                        throw new Exception("Internal Error: " + fileViewerType + " not found");
                }
            }
        }

        private void listBoxDocuments_DoubleClick(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int index = listBoxControl.SelectedIndex;
            if (index != -1)
            {
                string docFileExt = docType[index];
                DocViewerType fileViewerType = GetViewerType(docFileExt);

                switch (fileViewerType)
                {
                    case DocViewerType.Rtf:
                        PopupViewerRtf(index);
                        break;
                    case DocViewerType.Pdf:
                        PopupViewerPdf(index);
                        break;
                    case DocViewerType.Tif:
                        PopupViewerTif(index);
                        break;
                    default:
                        throw new Exception("Internal Error: " + fileViewerType + " not found");
                }
            }
        }

        #region Display in TabControl

        private void DisplayViewerRtf(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            xtraTabControl.SelectedTabPage = xtraTabPageDoc;
            DocumentFormat docFormat = GetDocumentFormat(docType[pIndex]);
            this.richEditControl.LoadDocument(fileName, docFormat);
        }

        private void DisplayViewerPdf(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            xtraTabControl.SelectedTabPage = xtraTabPagePdf;
            this.pdfViewer.LoadDocument(fileName);
        }

        private void DisplayViewerTif(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            xtraTabControl.SelectedTabPage = xtraTabPageTif;
            Image img = Image.FromFile(fileName);
            pictureEdit.Image = img;
        }

        #endregion

        #region PopUp Forms

        private void PopupViewerRtf(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            bool isDocExists = false;
            frmViewerRtf currentViewForm;

            //for (int i = 0; i < viewerRtfList.Count - 1; i++ )
            foreach (frmViewerRtf viewDoc in viewerRtfList)
            {
                if (viewDoc.FileName == fileName)
                {
                    if (!viewDoc.Visible)
                    {
                        //viewerRtfList[i].FileName = String.Empty;
                        //this.RemoveOwnedForm(viewerRtfList[i]);
                        //viewerRtfList.RemoveAt(i);
                        viewDoc.Show();
                    }
                    else
                    {
                        viewDoc.Focus();
                    }
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerRtf rtfViewer = new frmViewerRtf();
                rtfViewer.FileName = fileName;
                rtfViewer.Text = docTipList[pIndex];

                DocumentFormat docFormat = GetDocumentFormat(docType[pIndex]);
                rtfViewer.richEditControl.LoadDocument(fileName, docFormat);
                viewerRtfList.Add(rtfViewer);

                rtfViewer.Show();
            }
        }

        private void PopupViewerPdf(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            bool isDocExists = false;

            foreach (frmViewerPdf viewPdf in viewerPdfList)
            {
                if (viewPdf.FileName == fileName)
                {
                    if (!viewPdf.Visible)
                    {
                        viewPdf.Show();
                    }
                    else
                    {
                        viewPdf.Focus();
                    }
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerPdf pdfViewerForm = new frmViewerPdf();
                pdfViewerForm.FileName = fileName;
                pdfViewerForm.Text = docTipList[pIndex];

                DocumentFormat docFormat = GetDocumentFormat(docType[pIndex]);
                pdfViewerForm.pdfViewer.LoadDocument(fileName);
                viewerPdfList.Add(pdfViewerForm);

                pdfViewerForm.Show();
            }
        }

        private void PopupViewerTif(Int32 pIndex)
        {
            string fileName = docList[pIndex];
            bool isDocExists = false;

            foreach (frmViewerTif viewTif in viewerTifList)
            {
                if (viewTif.FileName == fileName)
                {
                    if (!viewTif.Visible)
                    {
                        viewTif.Show();
                    }
                    else
                    {
                        viewTif.Focus();
                    }
                    viewTif.Show();
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerTif tifViewerForm = new frmViewerTif();
                tifViewerForm.FileName = fileName;
                tifViewerForm.Text = docTipList[pIndex];

                Image img = Image.FromFile(fileName);
                tifViewerForm.pictureEdit.Image = img;

                viewerTifList.Add(tifViewerForm);

                tifViewerForm.Show();
            }
        }

        #endregion 

        private DocViewerType GetViewerType(string pFileExt)
        {
            DocViewerType resultType = DocViewerType.None;

            if (docViewerExtList.IndexOf(pFileExt) > -1)
                resultType = DocViewerType.Rtf;
            else if (pFileExt.ToUpper() == "PDF")
                resultType = DocViewerType.Pdf;
            else if (pFileExt.ToUpper() == "TIF")
                resultType = DocViewerType.Tif;

            return resultType;
        }

        private DocumentFormat GetDocumentFormat(string pFileExtension)
        {
            DocumentFormat docFormat = DocumentFormat.Undefined;
            switch (pFileExtension.ToUpper())
            {
                case "DOC":
                    docFormat = DocumentFormat.Doc;
                    break;
                case "DOCX":
                    docFormat = DocumentFormat.OpenXml;
                    break;
                case "RTF":
                    docFormat = DocumentFormat.Rtf;
                    break;
                case "HTML":
                    docFormat = DocumentFormat.Html;
                    break;
                case "TXT":
                    docFormat = DocumentFormat.PlainText;
                    break;
            }//switch DBMS

            return docFormat;
        }

        private string GetDocFormatFileExt(DocumentFormat pDocFormat)
        {
            string fileExt = "";
            if (pDocFormat == DocumentFormat.Doc)
                fileExt = "DOC";
            else if (pDocFormat == DocumentFormat.OpenXml)
                fileExt = "DOCX";
            else if (pDocFormat == DocumentFormat.Rtf)
                fileExt = "RTF";
            else if (pDocFormat == DocumentFormat.Html)
                fileExt = "HTML";
            else if (pDocFormat == DocumentFormat.PlainText)
                fileExt = "TXT";
            return fileExt;
        }

        private void SetTabPageDisplay(DocViewerType pViewerType, string pText)
        {
            switch (pViewerType)
            {
                case DocViewerType.Rtf:
                    xtraTabPageDoc.Text = pText;
                    xtraTabPageDoc.PageVisible = true;
                    xtraTabPagePdf.PageVisible = false;
                    xtraTabPageTif.PageVisible = false;
                    break;
                case DocViewerType.Pdf:
                    xtraTabPagePdf.Text = pText;
                    xtraTabPageDoc.PageVisible = false;
                    xtraTabPagePdf.PageVisible = true;
                    xtraTabPageTif.PageVisible = false;
                    break;
                case DocViewerType.Tif:
                    xtraTabPageTif.Text = pText;
                    xtraTabPageDoc.PageVisible = false;
                    xtraTabPagePdf.PageVisible = false;
                    xtraTabPageTif.PageVisible = true;
                    break;
                default:
                    throw new Exception("Internal Error: " + pViewerType + " not found");
            }
        }



    }
}
