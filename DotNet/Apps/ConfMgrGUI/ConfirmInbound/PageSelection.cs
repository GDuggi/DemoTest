using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Leadtools;
using Leadtools.Codecs;
using Leadtools.Drawing;
using Leadtools.WinForms;
using Sempra.Confirm.InBound.ImageEdit;

namespace ConfirmInbound
{
    public partial class PageSelection : Form
    {
        private String fileName;
        private String pageNumList = "";
        private RasterImage image;
        private const string FORM_NAME = "PageSelection";


        public String PageNumList
        {
            get { return pageNumList; }
        }

        public List<int> Pages
        {
            get
            {
                if (string.IsNullOrWhiteSpace(pageNumList))
                {
                    return new List<int>();
                }
                return pageNumList.Split(',').Select(s => Convert.ToInt32(s.Trim())).ToList();
            }
        }

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                LoadImageList();
            }
        }

        public PageSelection()
        {
            InitializeComponent();
        }

        public PageSelection(TifImage image)
        {
            InitializeComponent();
            LoadPages(image);
        }

        private void LoadPages(TifImage tifImage)
        {
            if (tifImage == null)
            {
                return;
            }
                        
            RasterPaintProperties paintProp = imageList.PaintProperties;
            paintProp.PaintDisplayMode = RasterPaintDisplayModeFlags.ScaleToGray;
            
            image = tifImage.ImageData;
            imageList.Items.Clear();
            for (var i = 1; i <= image.PageCount; ++i)
            {
                RasterImageListItem imageItem = new RasterImageListItem(image, i, "Page " + i);
                imageList.Items.Add(imageItem);
            }

            LoadCheckBox(image.PageCount);
        }

        private void LoadImageList()
        {
            if (fileName == null || "".Equals(fileName))
            {
                return;
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("An error occurred while loading the image list." + Environment.NewLine +
                    "Error CNF-543 in " + FORM_NAME + ".LoadImageList()");
            }

            RasterCodecs codec = new RasterCodecs();

            CodecsImageInfo info = codec.GetInformation(fileName, true);

            RasterPaintProperties paintProp = imageList.PaintProperties;
            paintProp.PaintDisplayMode = RasterPaintDisplayModeFlags.ScaleToGray;

            int lastPage = info.TotalPages;
            image = codec.Load(fileName, 0, CodecsLoadByteOrder.BgrOrGray, 1, lastPage);
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

            // Add select all check box
            CheckBox selectAllcheckBox = new CheckBox();
            selectAllcheckBox.Name = "selectAllcheckBox";
            selectAllcheckBox.Size = new Size(200, 24);
            selectAllcheckBox.Text = "Select All";
            selectAllcheckBox.CheckStateChanged += new EventHandler(selectAllcheckBox_CheckStateChanged);
            flowPanel.Controls.Add(selectAllcheckBox);
        }

        void selectAllcheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            for (int i = 0; i <= flowPanel.Controls.Count - 1; i++)
            {
                if (flowPanel.Controls[i] is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)flowPanel.Controls[i];
                    if (checkBox != chk)
                    {
                        checkBox.Checked = chk.Checked;
                    }
                }
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


            Size newSize = new Size(percent / 10 * 100, percent / 10 * 100);
            imageList.ItemImageSize = newSize;
            Size itemSize = new Size(newSize.Width, newSize.Height + 25);
            imageList.ItemSize = itemSize;


        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = toolStripComboBox1.SelectedIndex + 3;
            ZoomInPercent(selectIndex * 10);

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
                    if ((chk.Checked) && (chk.Name != "selectAllcheckBox"))
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