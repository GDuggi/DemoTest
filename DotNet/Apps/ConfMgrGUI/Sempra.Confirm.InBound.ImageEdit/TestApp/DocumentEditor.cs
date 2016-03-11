using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace InboundDocuments
{
    public partial class DocumentEditor : Form
    {
        private String fileName;
        private bool modified = false;

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
                LoadFile();
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
                if (fontName.Equals(docFontName,StringComparison.CurrentCultureIgnoreCase)) 
                {
                    fontNameList.SelectedIndex = i;
                }
            }
            float[] fontSize  = {8,9,10,11,12,14,16,18,20,22,24};
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
                        throw e;
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
                    }
                    break;

                }
                catch (IOException err)
                {
                    if (MessageBox.Show("Error occurred while saving: " + err.Message + "\nDo you want to try again?", "File Error", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw err;
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
                string itemText = (string) fontNameList.Items[i];
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
                if (itemFontSize == fontSize) {
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
            if (fontSize == null || "".Equals(fontSize) )
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
            Font font = new Font(fontName, Convert.ToSingle(fontSize),fontStyle);
          

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
            if (this.Modified)
            {
                if (MessageBox.Show("The document is modified. Do you want to save it?", "Document Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveToFile();
                }
            }
            this.Close();
        }

        private void DocumentEditor_Load(object sender, EventArgs e)
        {
            this.txtDoc.Focus();
            SetFormatToolBar();

        }

        private void txtDoc_Protected(object sender, EventArgs e)
        {
            this.txtDoc.SelectionProtected = false;
        }


    }
}