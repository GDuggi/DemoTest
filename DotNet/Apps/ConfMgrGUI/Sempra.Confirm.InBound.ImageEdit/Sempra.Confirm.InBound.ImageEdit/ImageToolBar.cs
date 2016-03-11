using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Collections;
using System.Drawing.Printing;
using Leadtools.WinForms;
using Leadtools;
using Leadtools.Annotations;




namespace Sempra.Confirm.InBound.ImageEdit
{
    public enum FlipDirection{ Left = -1,Right = 1};

     class ImageToolBar
    {

       
        private static string[] allowedEditButtons = new string[] { "Select","Text Pointer", "Line","Pointer","Text", "Rectangle", "Ellipse","Curve",  "Stamp","Check Mark"};

         private ExitButtonDelegate exitDelegate;
         private TransmitDelegate transDelegate;

         

        private ToolStrip viewToolStrip;
        private ToolStrip imgProcToolStrip;
        private ToolStrip statusToolStrip;
        private ToolBar _automationToolbar;
        private ToolStrip imgExitToolStrip;
        private ToolStrip transmitToolStrip;
        
 

        private TifEditor tifEditor;

        public TifEditor TifEditor
        {
            get { return tifEditor; }
            set { tifEditor = value; }
        }
         
        internal ToolBar GetExtendedToolBar(ToolBar toolbar)
        {
            // filtet the images that are applicable.

            _automationToolbar = toolbar;
            toolbar.ButtonSize = new Size(5, 5);
            int buttonLength = toolbar.Buttons.Count;
            for (int i = 0; i < buttonLength; ++i)
            {
                ToolBarButton button = toolbar.Buttons[i];
                string toolText = button.ToolTipText;
                if (Array.IndexOf(allowedEditButtons, toolText) < 0)
                {
                    button.Visible = false;
              //      button.M
                }
                if (button.ToolTipText.Equals("Stamp", StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    button.ToolTipText = "Signature";
                }
                

            }
            
            return toolbar;

        }
         public ToolStripContainer AddExitButton(ToolStripContainer toolbarContainer,ExitButtonDelegate myDelegate)
         {
             if (myDelegate != null) {
                imgExitToolStrip = new ToolStrip();
               // imgExitToolStrip.ImageScalingSize = new Size(18, 18);

                imgExitToolStrip.ImageList = new ImageList();
                imgExitToolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.toolBar_Click);
                imgExitToolStrip.ImageList.Images.Add("Exit", Image.FromHbitmap(ImageEdit.EXIT.GetHbitmap()));

                ToolStripItem button;

                button = new ToolStripButton();
                button.Text = "";
                button.ImageKey = "Exit";
                button.Name = "Exit";
                button.ToolTipText = "Exit";
                imgExitToolStrip.Items.Add(button);
    
                toolbarContainer.TopToolStripPanel.Controls.Add(imgExitToolStrip);
                this.exitDelegate = myDelegate;
             }
             else {
                 if (imgExitToolStrip != null) {
                    toolbarContainer.TopToolStripPanel.Controls.Remove(imgExitToolStrip);
                 }
                 imgExitToolStrip = null;
             }
             return toolbarContainer;

         }
         public ToolStripContainer AddCustomToolBar(ToolStripContainer toolbarContainer)
         {

             toolbarContainer.TopToolStripPanel.Controls.Add(GetTransmitToolBar());
             toolbarContainer.TopToolStripPanel.Controls.Add(GetStatusToolBar());
             toolbarContainer.TopToolStripPanel.Controls.Add(GetViewToolBar());
             toolbarContainer.TopToolStripPanel.Controls.Add(GetProcToolbar());
             

             return toolbarContainer;
             
         }

         private ToolStrip GetProcToolbar()
         {


             imgProcToolStrip = new ToolStrip();
             
             imgProcToolStrip.ImageList = new ImageList();
             imgProcToolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.toolBar_Click);
             ToolStripItem button;

           
             imgProcToolStrip.ImageList.Images.Add("Save", Image.FromHbitmap(ImageEdit.SAVE.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Save";
             button.Name = "Save";
             button.ToolTipText = "Save Annotation";
             imgProcToolStrip.Items.Add(button);

           
             imgProcToolStrip.ImageList.Images.Add("Print", Image.FromHbitmap(ImageEdit.PRINT.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Print";
             button.Name = "Print";
             button.ToolTipText = "Print";
             imgProcToolStrip.Items.Add(button);


             imgProcToolStrip.ImageList.Images.Add("Cancel", Image.FromHbitmap(ImageEdit.DELETE.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Cancel";
             button.Name = "Cancel";
             button.ToolTipText = "Cancel";
             imgProcToolStrip.Items.Add(button);

             imgProcToolStrip.ImageList.Images.Add("Zoom In", Image.FromHbitmap(ImageEdit.ZoomIn.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Zoom In";
             button.Name = "ZoomIn";
             button.ToolTipText = "Zoom In";
             imgProcToolStrip.Items.Add(button);

             imgProcToolStrip.ImageList.Images.Add("Zoom Out", Image.FromHbitmap(ImageEdit.ZoomOut.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Zoom Out";
             button.Name = "ZoomOut";
             button.ToolTipText = "Zoom Out";
             imgProcToolStrip.Items.Add(button);

             imgProcToolStrip.ImageList.Images.Add("Rotate Right", Image.FromHbitmap(ImageEdit.rotateright.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Rotate Right";
             button.Name = "RotateRight";
             button.ToolTipText = "Rotate Right";
             imgProcToolStrip.Items.Add(button);

             imgProcToolStrip.ImageList.Images.Add("Rotate Left", Image.FromHbitmap(ImageEdit.rotateleft.GetHbitmap()));
             button = new ToolStripButton();
             button.Text = "";
             button.ImageKey = "Rotate Left";
             button.Name = "RotateLeft";
             button.ToolTipText = "Rotate Left";
             imgProcToolStrip.Items.Add(button);

             return imgProcToolStrip;

         }
         private ToolStrip GetViewToolBar()
         {

             viewToolStrip = new ToolStrip();
             
             viewToolStrip.ImageList = new ImageList();

             viewToolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.ViewBar_Click);

             viewToolStrip.ImageList.Images.Add("First", Image.FromHbitmap(ImageEdit.TOP.GetHbitmap()));
             ToolStripItem button1 = new ToolStripButton();
             button1.Text = "";
             button1.ImageKey = "First";
             button1.Name = "First";
             button1.ToolTipText = "First Page";
             viewToolStrip.Items.Add(button1);

             viewToolStrip.ImageList.Images.Add("Previous", Image.FromHbitmap(ImageEdit.PREV.GetHbitmap()));
             button1 = new ToolStripButton();
             button1.Text = "";
             button1.ImageKey = "Previous";
             button1.Name = "Previous";
             button1.ToolTipText = "Previous Page";
             viewToolStrip.Items.Add(button1);

             ToolStripDropDownButton dropDownButton = new ToolStripDropDownButton();
             dropDownButton.Text = "Goto Page:";
             ToolStripDropDown dropDown = new ToolStripDropDown();
             dropDown.ItemClicked += new ToolStripItemClickedEventHandler(this.PageItem_Clicked);
             dropDownButton.Name = "_page";
             dropDownButton.DropDown = dropDown;
             viewToolStrip.Items.Add(dropDownButton);

             viewToolStrip.ImageList.Images.Add("Next", Image.FromHbitmap(ImageEdit.NEXT.GetHbitmap()));
             button1 = new ToolStripButton();
             button1.Text = "";
             button1.ImageKey = "Next";
             button1.Name = "Next";
             button1.ToolTipText = "Next Page";
             viewToolStrip.Items.Add(button1);

             viewToolStrip.ImageList.Images.Add("Last", Image.FromHbitmap(ImageEdit.BOT.GetHbitmap()));
             button1 = new ToolStripButton();
             button1.Text = "";
             button1.ImageKey = "Last";
             button1.Name = "Last";
             button1.ToolTipText = "Last Page";
             viewToolStrip.Items.Add(button1);

             return viewToolStrip;
         }
         public void EnableDisbleNavButton()
         {
             int currentPage = this.tifEditor.Viewer.Image.Page;
             int TotalPage = this.tifEditor.Viewer.Image.PageCount;

             viewToolStrip.Items["First"].Enabled = (currentPage  > 1 && TotalPage > 1);
             viewToolStrip.Items["Previous"].Enabled = (currentPage > 1 && TotalPage > 1);
             viewToolStrip.Items["Last"].Enabled = (currentPage < TotalPage );
             viewToolStrip.Items["Next"].Enabled = (currentPage < TotalPage) ;
             

         }
         private ToolStrip GetStatusToolBar()
         {
             this.statusToolStrip = new ToolStrip();

             ToolStripLabel statusText = new ToolStripLabel();
             statusText.TextAlign = ContentAlignment.MiddleCenter;
             statusText.Padding = new Padding(10, 10, 10, 10);
             
             statusText.Text = "Current Page : ";
             statusText.Name = "PageNum";
             statusText.ImageKey = "PageNum";
             statusToolStrip.Items.Add(statusText);

             ToolStripSeparator sep = new ToolStripSeparator();
             statusToolStrip.Items.Add(sep);

             statusText = new ToolStripLabel();
             statusText.TextAlign = ContentAlignment.MiddleCenter;
             statusText.ForeColor = Color.Blue;
             statusText.Text = "";
             statusText.Name = "ImageStatus";
             statusText.ImageKey = "ImageStatus";
             
             statusToolStrip.Items.Add(statusText);

             return statusToolStrip;


         }

         public void PopulatePageNumber(int totPages)
         {
             ToolStripItem item = viewToolStrip.Items["_page"];
             ToolStripDropDownButton dropDownButton = item as ToolStripDropDownButton;
             if (dropDownButton != null)
             {
                 ToolStripDropDown dropDown = dropDownButton.DropDown;
                 dropDown.Items.Clear();
                 for (int i = 1; i <= totPages; ++i)
                 {
                     dropDown.Items.Add("Page #" + i.ToString());
                     dropDown.Items[i-1].ImageKey = i.ToString();
                 }
             }

             
         }
         private void ViewBar_Click(Object obj, ToolStripItemClickedEventArgs args)
         {
             string imgKey = args.ClickedItem.ImageKey;
             if ("First".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 FirstPage();
             }
             else if ("Previous".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 PrevPage();
             }
             else if ("Next".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 NextPage();
             }
             else if ("Last".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 LastPage();
             }
             

         }

         private void SaveToFile()
         {
             tifEditor.SaveToFile();
         }
         private void toolBar_Click(Object obj, ToolStripItemClickedEventArgs args)
         {
             string imgKey = args.ClickedItem.ImageKey;

             if ("Zoom In".Equals(imgKey,StringComparison.CurrentCultureIgnoreCase)) 
             {
                 ImageZoom(ZoomSize.ZoomIn);
             }
             else if ("Zoom Out".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 ImageZoom(ZoomSize.ZoomOut);
             }
             else if ("Rotate Left".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase)){
                 ImageFlip(FlipDirection.Left);
             }
             else if ("Rotate Right".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase)){
                 ImageFlip(FlipDirection.Right);
             }
             else if ("Save".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 SaveToFile();
             }
             else if("Publish".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase)){
                 Publish();
             }
             else if ("Print".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 PrintImage();
             }
             else if ("Cancel".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 CancelOperation();
             }
             else if ("Exit".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 if (exitDelegate != null)
                 {
                     exitDelegate.Invoke();
                 }
             }
             else if ("Transmit".Equals(imgKey, StringComparison.CurrentCultureIgnoreCase))
             {
                 if (transDelegate != null)
                 {
                     transDelegate.Invoke();
                 }
             }
             
         }

         private void CancelOperation()
         {
             if (MessageBox.Show("Do you want to cancel the current changes and reload the image", "Tif Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                 tifEditor.ReloadAnnotation();
             }
         }

         public void PrintImage()
         {
             if (PrinterSettings.InstalledPrinters != null && PrinterSettings.InstalledPrinters.Count > 0)
             {
                 tifEditor.ScaleFactor = 1;
                 SetPageNumber(1);
                 PrintDocument print = new PrintDocument();
                 print.PrintPage += new PrintPageEventHandler(this.Image_Print);
                 
                 print.Print();
             }
             else
             {
                 MessageBox.Show("No printer is installed.", "Tif Editor");
             }
         }
         private void Image_Print(object sender, PrintPageEventArgs e)
         {


             RasterImage image = tifEditor.Viewer.Image;

             /*
             Rectangle destRectangle = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top,
                                                (int)((float)image.ImageWidth / (float)image.XResolution * 100.0F),
                                                (int)((float)image.ImageWidth / (float)image.YResolution * 100.0F));
             RasterPaintProperties props = RasterPaintProperties.Default;
             props.PaintEngine = RasterPaintEngine.GdiPlus;
             image.Paint(e.Graphics, Rectangle.Empty, Rectangle.Empty, destRectangle, e.MarginBounds, props);
             */


             using (Image printImage = Leadtools.Drawing.RasterImageConverter.ConvertToImage(image, Leadtools.Drawing.ConvertToImageOptions.None))
             {
                 e.Graphics.DrawImage(printImage, 0, 0);
             }

             int currentPage = image.Page;
             int totalCount = image.PageCount;

             ++currentPage;
             e.HasMorePages = currentPage <= image.PageCount;
             if (currentPage > image.PageCount)
             {
                 currentPage = 1;
             }
             SetPageNumber(currentPage);

         }

         private void Publish()
         {
             if (MessageBox.Show("The notes will be saved permanently, Do you want to continue", "Tif Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                 tifEditor.Publish();
             }
         }

         private void ImageZoom(ZoomSize zoomSize)
         {

             ImageSizing imageSize = new ImageSizing(tifEditor.Viewer);
             imageSize.Zoom(zoomSize);
             tifEditor.ScaleFactor = imageSize.getScaleFactor();
             
         }

         private void ImageFlip(FlipDirection direction)
         {
             ImageSizing imageSize = new ImageSizing(tifEditor.Viewer);
             int degree = (direction == FlipDirection.Left) ? -1:1;

             imageSize.RotateImageAndAnnotations(degree * 90, tifEditor.AnnAutomation.Container);

         }

         
         public void UpdateToolBar()
         {
          
             int totalPages = 0;
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 totalPages = tifEditor.Viewer.Image.PageCount;
                 tifEditor.Viewer.Image.Page = 1;
                 PopulatePageNumber(totalPages);
                 viewToolStrip.Enabled = true;
                 imgProcToolStrip.Enabled = true;
                 _automationToolbar.Enabled = true;
                 ShowPageNum();
                 EnableDisbleNavButton();
                 UpdateModeToolBar();
             }
             else
             {
                 viewToolStrip.Enabled = false;
                 imgProcToolStrip.Enabled = false;
                 _automationToolbar.Enabled = false;
                 UpdateModeToolBar();
             }
         }
         public void UpdateModeToolBar()
         {

             imgProcToolStrip.Items["Save"].Enabled = this.tifEditor.AnnAutomation.Manager.UserMode == AnnUserMode.Design;
             imgProcToolStrip.Items["Cancel"].Enabled = this.tifEditor.AnnAutomation.Manager.UserMode == AnnUserMode.Design;
           //  imgProcToolStrip.Items["Publish"].Enabled = this.tifEditor.AnnAutomation.Manager.UserMode == AnnUserMode.Design;
             statusToolStrip.Items["ImageStatus"].Text = ((this.tifEditor.AnnAutomation.Manager.UserMode == AnnUserMode.Design) ? "Design" : "View");

         }
         private void ShowPageNum()
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int currentPage = tifEditor.Viewer.Image.Page;
                 int totalPage = tifEditor.Viewer.Image.PageCount;
                 statusToolStrip.Items["PageNum"].Text = "Current Page : " + currentPage.ToString() + " of " + totalPage.ToString();
                 

             }
         }
         private void NextPage()
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int currentPage = tifEditor.Viewer.Image.Page;
                 SetPageNumber(currentPage + 1);
             }
         }
         private void PrevPage()
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int currentPage = tifEditor.Viewer.Image.Page;
                 SetPageNumber(currentPage - 1);
             }
         }
         private void LastPage()
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int lastPage = tifEditor.Viewer.Image.PageCount;
                 SetPageNumber(lastPage);
             }
         }
         private void FirstPage()
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int lastPage = tifEditor.Viewer.Image.PageCount;
                 SetPageNumber(1);
             }
         }

         public  void SetPageNumber(int pageNum)
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 int currentPage = tifEditor.Viewer.Image.Page;
                 tifEditor.SaveAnnotation();
                 int totalPages = tifEditor.Viewer.Image.PageCount;
                 if (pageNum > totalPages)
                 {
                     pageNum = totalPages;
                 }

                 else if (pageNum < 1)
                 {
                     pageNum = 1;
                 }
                 tifEditor.Viewer.Image.Page = pageNum;
                 tifEditor.LoadAnnotation();
                 ShowPageNum();
                 EnableDisbleNavButton();
             }
         }
         private void PageItem_Clicked(object sender, ToolStripItemClickedEventArgs arg)
         {
             ToolStripItem item = arg.ClickedItem;
             int pageNum = Convert.ToInt32(item.ImageKey);
             SetPageNumber(pageNum);
         }
         public void setFileName(String name)
         {
             if (tifEditor.Viewer.IsImageAvailable)
             {
                 if (name != null)
                 {
                     int count = name.LastIndexOf(@"\");
                     if (count > 0)
                     {
                         name = name.Substring(count+1);
                     }
                 }
                 transmitToolStrip.Items["FileName"].Text = name;
                 transmitToolStrip.Items["FileName"].Visible = true;
             }
             else
             {
                 transmitToolStrip.Items["FileName"].Text = "";
             }
         }

         internal void AddTransmitButton(ToolStripContainer topToolBar, TransmitDelegate transmitDelegate)
         {

             transDelegate = transmitDelegate;

             if (transDelegate != null)
             {
                 if (transmitToolStrip.Items.Count > 0)
                 {
                     transmitToolStrip.Items.RemoveAt(0);
                 }

                 ToolStripItem button;
                 button = new ToolStripButton();
                 button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                 button.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                 //button.

                 button.Text = "Transmit";
                 button.ImageKey = "Transmit";
                 button.Name = "Transmit";
                 button.ToolTipText = "Transmit to FaxGateway";
                 transmitToolStrip.Items.Add(button);
             }
             else
             {
                 if (transmitToolStrip.Items.Count > 0)
                 {
                     transmitToolStrip.Items.RemoveAt(0);
                 }
             }
             ToolStripLabel statusText = new ToolStripLabel();
             statusText.AutoSize = true;
             statusText.TextAlign = ContentAlignment.MiddleCenter;
             statusText.Padding = new Padding(10, 10, 10, 10);
             statusText.Name = "FileName";
            // statusText.Text = "Image File name";
            // statusText.Font = new Font("Arial", 10);
             transmitToolStrip.Items.Add(statusText);
             return;


         }
         private ToolStrip GetTransmitToolBar()
         {
             transmitToolStrip = new ToolStrip();
             transmitToolStrip.ImageScalingSize = new Size(25, 20);
             transmitToolStrip.ImageList = new ImageList();
             transmitToolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.toolBar_Click);
             transmitToolStrip.ImageList.Images.Add("Transmit", Image.FromHbitmap(ImageEdit.transmit.GetHbitmap()));

             
             return transmitToolStrip;
         }
     }
}
