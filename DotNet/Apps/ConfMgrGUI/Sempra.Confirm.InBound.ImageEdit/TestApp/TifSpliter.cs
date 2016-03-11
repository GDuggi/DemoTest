using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Leadtools.WinForms;
using System.IO;
using Leadtools;
using Leadtools.Codecs;
using Leadtools.Drawing;

namespace InboundDocuments
{
    public partial class TifSpliter : Form
    {
        private string srcFileName;

        public string SrcFileName
        {
            get { return srcFileName; }

            set { 
                srcFileName = value;
                if (srcFileName != null)
                {
                    SetSrcImageList();
                }
                else
                {
                    this.srcImageList.Items.Clear();
                }
            }
        }

        private void SetSrcImageList()
        {
            LoadImageList(this.SrcFileName, this.srcImageList); 
        }

        public TifSpliter()
        {
            InitializeComponent();
        }

        private void LoadImageList(String fileName, RasterImageList rasterImageList) {

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

            RasterPaintProperties paintProp = rasterImageList.PaintProperties;
            paintProp.PaintDisplayMode = RasterPaintDisplayModeFlags.ScaleToGray;

            int lastPage = info.TotalPages;
            RasterImage image =codec.Load(fileName,0,CodecsLoadByteOrder.BgrOrGray,1,lastPage);
            rasterImageList.Items.Clear();
            int totalCount = image.PageCount;
            for (int i = 1; i <= totalCount; ++i)
            {
                RasterImageListItem imageItem = new RasterImageListItem(image, i, "Page " + i.ToString());
                rasterImageList.Items.Add(imageItem);

            }

        }

        private void ZoomInPercent(int percent)
        {

            Size newSize = new Size(percent / 10 * 100, percent / 10 * 100);
            this.srcImageList.ItemImageSize = newSize;
            Size itemSize = new Size(newSize.Width, newSize.Height + 25);
            srcImageList.ItemSize = itemSize;


        }

        private void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = cboZoom.SelectedIndex + 3;
            ZoomInPercent(selectIndex * 10);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RasterImage image = srcImageList.Items[0].Image;
            AddPageToDest(image);
        }

        private void srcImageList_MouseDown(object sender, MouseEventArgs e)
        {
            srcImageList.DoDragDrop(srcImageList.Items[0].Image, DragDropEffects.Copy);
        }

        private void destImageList_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void AddPageToDest(RasterImage image)
        {
            RasterImageListItem item = new RasterImageListItem();
            item.Image = image;
          //  image.v
            image.FlipViewPerspective(true);
            destImageList.Items.Add(item);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveToFile("C:\\temp\\dest.tif");
        }

        private void SaveToFile(string fileName)
        {
            RasterCodecs codec = new RasterCodecs();
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            for (int i=0;i<destImageList.Items.Count;++i){
                codec.Save(destImageList.Items[i].Image, fileName, RasterImageFormat.Tif, 0, i + 1, i + 1, i + 1, CodecsSavePageMode.Append);
            }
        }
    }
}