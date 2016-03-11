using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Leadtools;
using Leadtools.Codecs;
using Leadtools.Drawing;
using Leadtools.WinForms;

namespace InboundDocuments
{
    public partial class PageSelection : Form
    {
        private String fileName;
        private String pageNumList = "";
        private RasterImage image;
        
        
        public String PageNumList
        {
            get { return pageNumList; }
        }

        public String FileName
        {
            get { return fileName; }
            set { 
                fileName = value;
                LoadImageList();
            }
        }

        private void LoadImage()
        {
           
        }

        private void LoadPageNumbers()
        {
            

        }

        

        public PageSelection()
        {
            InitializeComponent();
            
        }

        private void LoadImageList()
        {
            if (fileName == null || "".Equals(fileName))
            {
                return;
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }
            RasterCodecs codec = new RasterCodecs();

            CodecsImageInfo info = codec.GetInformation(fileName,true);

            RasterPaintProperties paintProp = imageList.PaintProperties;
            paintProp.PaintDisplayMode = RasterPaintDisplayModeFlags.ScaleToGray;

            int lastPage = info.TotalPages;
            image =codec.Load(fileName,0,CodecsLoadByteOrder.BgrOrGray,1,lastPage);
            imageList.Items.Clear();
            int totalCount = image.PageCount;
            for (int i = 1; i <= totalCount; ++i)
            {
                RasterImageListItem imageItem = new RasterImageListItem(image, i, "Page " + i.ToString());
                imageList.Items.Add(imageItem);

            }

            LoadCheckBox(totalCount);
        }

        private void LoadCheckBox(int totalCount)
        {
            for (int i = 1; i <= totalCount; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "check" + i.ToString();
                checkBox.Size = new Size(50, 24);
                checkBox.Text = i.ToString();
                checkBox.CheckStateChanged += new EventHandler(checkBox_CheckStateChanged);
                flowPanel.Controls.Add(checkBox);
                
            }
        }

        void checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            int pageNum = Convert.ToInt32(chk.Text);
            imageList.Items[pageNum - 1].Selected = chk.Checked;
            imageList.Refresh();

        }
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            pageNumList = "";
        }

        private void ZoomInPercent(int percent)
        {

            
            Size newSize = new Size (percent/10 *100,percent/10 * 100);
            imageList.ItemImageSize = newSize;
            Size itemSize = new Size(newSize.Width, newSize.Height + 25);
            imageList.ItemSize = itemSize;
            
            
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = toolStripComboBox1.SelectedIndex + 3;
            ZoomInPercent( selectIndex * 10);

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            pageNumList = "";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!isPagesSelected())
            {
                MessageBox.Show("Please select the pages you want to transmit.");
                this.DialogResult = DialogResult.None;
            }
        }

        private bool isPagesSelected()
        {
            int totalPages = flowPanel.Controls.Count;
            if (totalPages == 0) { return false; }
            pageNumList = "";
            bool isSelected = false;
            for (int i = 0; i < totalPages; ++i)
            {
                CheckBox chk = (CheckBox)flowPanel.Controls[i];
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        isSelected = true;
                        if (!"".Equals(pageNumList))
                        {
                            pageNumList += ",";
                        }
                        pageNumList += chk.Text;
                    }
                }
            }
            return isSelected;
        }


    }
}