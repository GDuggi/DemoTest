using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Configuration;

namespace ConfirmInbound
{
    public partial class DocumentEditor : Form
    {
        private const string FORM_NAME = "DocumentEditor";
        private const string FORM_ERROR_CAPTION = "Document Editor Form Error";
        private String fileName;
        private bool modified = false;
        private string tradeID = "";
        private string tradeSys = "";
        private bool isAttached = false;


        public virtual string TradeID
        {
            set { tradeID = value; }
        }

        public virtual bool IsAttached
        {
            get { return isAttached; }
        }

        public virtual string TradeSys
        {
            set { tradeSys = value; }
        }

        public bool Modified
        {
            get { return modified; }
        }

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                this.Text = fileName;
            }
        }

        public DocumentEditor()
        {
            InitializeComponent();
            LoadValues();
        }

        private void LoadValues()
        {
            FontFamily[] fonts = FontFamily.Families;
            string docFontName = txtDoc.SelectionFont.Name;
            for (int i = 0; i < fonts.Length; ++i)
            {
                String fontName = fonts[i].Name;
                fontNameList.Items.Add(fontName);
                if (fontName.Equals(docFontName, StringComparison.CurrentCultureIgnoreCase))
                {
                    fontNameList.SelectedIndex = i;
                }
            }
            float[] fontSize = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24 };
            float docFontSize = txtDoc.SelectionFont.Size;
            for (int i = 0; i < fontSize.Length; i++)
            {
                fontSizeList.Items.Add(fontSize[i]);
                if (docFontSize == fontSize[i])
                {
                    fontSizeList.SelectedIndex = i;
                }
            }
        }
        private void LoadFile()
        {
            while (true)
            {
                try
                {
                    this.txtDoc.LoadFile(fileName);
                    break;
                }
                catch (IOException e)
                {
                    if (MessageBox.Show("Error occurred while loading: " + e.Message + "\nDo you want to try again?", "File Error", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        //throw e;
                        throw new Exception("An error occurred while loading: " + fileName + "." + Environment.NewLine +
                             "Error CNF-419 in " + FORM_NAME + ".LoadFile(): " + e.Message);
                    }
                }

            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveToFile();
            this.Close();
        }
        private void SaveToFile()
        {
            while (true)
            {
                try
                {
                    if (fileName != null && !"".Equals(fileName))
                    {
                        this.txtDoc.SaveFile(fileName);
                        modified = false;
                        isAttached = true;
                    }
                    break;

                }
                catch (IOException err)
                {
                    if (MessageBox.Show("Error occurred while saving: " + err.Message + "\nDo you want to try again?", "File Error", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        //throw err;
                        throw new Exception("An error occurred while saving: " + fileName + "." + Environment.NewLine +
                             "Error CNF-420 in " + FORM_NAME + ".SaveToFile(): " + err.Message);
                    }
                }
            }
        }
        private void txtDoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            modified = true;
            //  SetFormatToolBar();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }
        private void SetFormatToolBar()
        {
            string fontName = txtDoc.SelectionFont.Name;
            float fontSize = txtDoc.SelectionFont.Size;
            bool fontItalic = txtDoc.SelectionFont.Italic;
            bool fontBold = txtDoc.SelectionFont.Bold;
            bool fontUnderLine = txtDoc.SelectionFont.Underline;

            boldButton.Checked = fontBold;
            italicButton.Checked = fontItalic;
            underButton.Checked = fontUnderLine;
            int count = this.fontNameList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                string itemText = (string)fontNameList.Items[i];
                if (fontName.Equals(itemText, StringComparison.CurrentCultureIgnoreCase))
                {
                    fontNameList.SelectedIndex = i;
                    break;
                }

            }
            count = this.fontSizeList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                float itemText = (float)fontSizeList.Items[i];
                float itemFontSize = Convert.ToSingle(itemText);
                if (itemFontSize == fontSize)
                {
                    fontSizeList.SelectedIndex = i;
                    break;
                }

            }

        }
        private void setTextFont()
        {
            string fontName = this.fontNameList.Text;
            string fontSize = this.fontSizeList.Text;
            bool fontItalic = italicButton.Checked;
            bool fontBold = boldButton.Checked;
            bool fontUnderLine = underButton.Checked;

            if (fontName == null || "".Equals(fontName))
            {
                return;
            }
            if (fontSize == null || "".Equals(fontSize))
            {
                return;
            }
            FontStyle fontStyle = FontStyle.Regular;

            if (fontBold)
            {
                fontStyle = fontStyle | FontStyle.Bold;
            }
            if (fontItalic)
            {
                fontStyle = fontStyle | FontStyle.Italic;
            }
            if (fontUnderLine)
            {
                fontStyle = fontStyle | FontStyle.Underline;
            }
            //  Font font = txtDoc.SelectionFont;
            Font font = new Font(fontName, Convert.ToSingle(fontSize), fontStyle);


            txtDoc.SelectionFont = font;
            this.txtDoc.Focus();
        }

        private void txtDoc_Enter(object sender, EventArgs e)
        {
            //SetFormatToolBar();
            //   setTextFont();
        }

        private void fontNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTextFont();
        }

        private void fontSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTextFont();
        }



        private void txtDoc_Click(object sender, EventArgs e)
        {
            SetFormatToolBar();
        }

        private void boldButton_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;
            if (button.CheckState == CheckState.Checked)
            {
                button.CheckState = CheckState.Unchecked;
            }
            else
            {
                button.CheckState = CheckState.Checked;
            }

        }

        private void boldButton_CheckStateChanged(object sender, EventArgs e)
        {
            setTextFont();
        }

        private void italicButton_CheckStateChanged(object sender, EventArgs e)
        {
            setTextFont();
        }

        private void underButton_CheckStateChanged(object sender, EventArgs e)
        {
            setTextFont();
        }

        private void boldButton_CheckedChanged(object sender, EventArgs e)
        {
            FontStyle fontStyle = FontStyle.Regular;

            if (this.boldButton.Checked)
            {
                fontStyle = fontStyle | FontStyle.Bold;
            }
            if (this.italicButton.Checked)
            {
                fontStyle = fontStyle | FontStyle.Italic;
            }
            if (this.underButton.Checked)
            {
                fontStyle = fontStyle | FontStyle.Underline;
            }
            this.txtDoc.SelectionFont = new Font(this.txtDoc.SelectionFont, fontStyle);

        }

        private void txtDoc_KeyDown(object sender, KeyEventArgs e)
        {
            SetFormatToolBar();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            isAttached = false;
            if (File.Exists(this.fileName))
            {
                File.Delete(this.fileName);
            }
            this.Close();
        }

        private void DocumentEditor_Load(object sender, EventArgs e)
        {

            cmbbxServer.Items.Add(ContractServer);

            cmbbxServer.Text = ContractServer;
            cmbbxServer.SelectedIndex = 0;
            cmbbxServer.Enabled = false;



            cmbbxPort.Items.Add(InboundSettings.ContractServerPort);
            cmbbxPort.Text = InboundSettings.ContractServerPort;
            cmbbxPort.SelectedIndex = 0;
            cmbbxPort.Enabled = false;

            this.txtDoc.Focus();
            SetFormatToolBar();

        }

        public virtual string ContractServer
        {
            //get { return InboundPnl.settingsReader.GetSettingValue("ContractServer"); }
            get { return InboundPnl.appTempDir; }
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            StringBuilder sbParams = new StringBuilder();
            sbParams.Append("&SERVER_METHOD=1");
            sbParams.Append("&TRADING_SYSTEM=" + this.tradeSys);
            sbParams.Append("&TICKET_NO=" + this.tradeID);
            sbParams.Append("&CONTRACT_TYPE=NEW");
            sbParams.Append("&TEMPLATE_NAME=" + cmbbxTemplateName.Text);

            string urlRequest = "http://" + cmbbxServer.Text + ":" + cmbbxPort.Text + "/request?" + sbParams.ToString();



            WebRequest request = WebRequest.Create(urlRequest);
            WebResponse response = request.GetResponse();

            StreamReader sr = new StreamReader(response.GetResponseStream());
            string contractBody = sr.ReadToEnd();

            contractBody = contractBody.Remove(0, (contractBody.IndexOf("CONTRACT_BODY") + 14));

            if (File.Exists(this.fileName))
            {
                File.Delete(this.fileName);
            }

            FileStream fs = new FileStream(this.FileName, FileMode.CreateNew);

            StreamWriter sw = new StreamWriter(fs);

            sw.Write(contractBody);

            sw.Close();

            this.LoadFile();

        }

        private void txtDoc_Protected(object sender, EventArgs e)
        {
            this.txtDoc.SelectionProtected = false;
        }

    }
}