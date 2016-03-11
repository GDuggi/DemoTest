using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Leadtools.Annotations;
using Leadtools.Codecs;
using Leadtools;
using Leadtools.WinForms;
using Leadtools.Drawing;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.IO;
using log4net;
using DevExpress.XtraEditors;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public delegate void ExitButtonDelegate();
    public delegate void TransmitDelegate();

    public delegate void SaveImageOverrideDelegate(TifEditor editor);

    public delegate byte[] GetImageDataDelegate();
    
    public partial class TifEditor : UserControl, IImageHolder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (TifEditor));
        public static readonly string _CHECK_MARK_NAME = "CheckMark";
        public static readonly string UNASSIGNED = "UNASSIGNED";
        public static string licenseFile = UNASSIGNED;
        public static string developerKey = UNASSIGNED;
        public static string SignatureLocation = String.Empty;

        //private string MY_LICENSE_FILE = "\\Amphora Inc.-DOCIMG_RASPDFR175.lic";
        //private string MY_DEVELOPER_KEY = "5cxfXs1TnpaXbVXDIdFzk9iRWcBBLjcIoN2W3sVpvDPvoNzYNmOr1zmYg3qhCnFE";
        private string imageFileName;
        private static TifEditor CurrentEditor;

        private const string FORM_NAME = "TifEditor";
        private const string FORM_ERROR_CAPTION = "Image Editor Form Error";
        private AnnAutomationManager _automationManager;
        private AnnAutomation _annAutomation;
        private RasterCodecs _codec;
        private ImageToolBar imageToolBar;
        private EditEventHandler eventHandler;
        private Annotation annotation;
        private string userId;
        private bool edit;
        private string newFileName;
        private double scaleFactor = 1;
        private string saveAsFileName = null;
        private ExitButtonDelegate exitDelegate;
        private TransmitDelegate transDelegate;
        private int pageNumber;
        private ToolBar toolBar;

        public SaveImageOverrideDelegate SaveImageOverrideDelegate { get; set; }
        public GetImageDataDelegate GetImageDataOverride { get; set; }

        public TifEditor()
        {
            // RasterSupport.Unlock(RasterSupportType.Document, "xcRns97c3");
            // RasterSupport.Unlock(RasterSupportType.Document, "vhG42tyuh9");
            try
            {
                SetLicenseFile();
                InitializeComponent();
                IntializeAnnContainer();
                CurrentEditor = this;
            }
            catch (Exception ex)
            {
                Logger.Error("Error constructing tifEditor:" + ex.Message, ex);
                XtraMessageBox.Show("An error occurred while creating the Image Editor." + Environment.NewLine +
                        "Error CNF-388 in " + FORM_NAME + ".TifEditor(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetLicenseFile()
        {            
            string licensePath = Path.Combine(Application.StartupPath, licenseFile);
            RasterSupport.SetLicense(licensePath, developerKey);
        }
        
        public static TifEditor GetCurrentEditor()
        {
            return CurrentEditor;
        }

        public int PageNumber
        {
            set
            {
                imageToolBar.SetPageNumber(value);
            }
        }
        public TransmitDelegate TransDelegate
        {
            get { return transDelegate; }
            set { transDelegate = value;
                imageToolBar.AddTransmitButton(topToolBar, value);
             }
        }

        public ExitButtonDelegate ExitDelegate
        {
            get { return exitDelegate; }
            set { 
                exitDelegate = value;
                topToolBar = imageToolBar.AddExitButton(topToolBar,value);
            }
        }

        public int TotalPages
        {
            get
            {
                if (imageViewer.Image == null)
                { 
                    return 0;
                } 
                else 
                { 
                    return imageViewer.Image.PageCount;
                }
            }
        }


        /// <summary>
        /// Gets whether the annotation is added to the image
        /// </summary>
        public bool ImageModified
        {
            get
            {
                return _annAutomation.ImageDirty || _annAutomation.ObjectsDirty;
            }
        }

        public string SaveAsFileName
        {
            get { return saveAsFileName; }
            set { saveAsFileName = value; }
        }


        public double ScaleFactor
        {
            get { return scaleFactor; }
            set {
                if (value < ImageSizing._minimumScaleFactor || value > ImageSizing._maximumScaleFactor)
                {
                    throw new Exception("Invalid Scale Factor range." + Environment.NewLine +
                         "Error CNF-389 in " + FORM_NAME + ".ScaleFactor().");
                }
                else
                {
                    scaleFactor = value;
                }
            }
        }

        public bool Edit
        {
            get { return edit; }
            set 
            {
                if (value == true)
                {
                    string fileName = ImageFileName;
                    if (fileName != null)
                    {
                        if (fileName.IndexOf(".txt", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            XtraMessageBox.Show("Text file can not be edited." + Environment.NewLine +
                                    "Error CNF-390 in " + FORM_NAME + ".Edit.",
                                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            value = false;
                        }
                    }
                }
                edit = value;
                UserModeChanged();
            }
        }

        

        public string UserName
        {
            get { return userId; }
            set { userId = value; SetStampObjectText(); }
        }

        internal ToolStripContainer TopToolBar
        {
            get { return topToolBar; }
            set { topToolBar = value; }
        }

        internal RasterCodecs Codecs {
            get { return _codec; }
         }
        
    
        internal AnnAutomation AnnAutomation{
            get { return _annAutomation; }
        }
        internal RasterImageViewer Viewer
        {
            get { return imageViewer; }
        }
        public string ImageFileName
        {
            get { return newFileName; }
            set { newFileName = value;}
        }

        /// <summary>
        /// Loads the images in view mode.
        /// </summary>
        public void LoadImage()
        {
            try
            {               
                ImageLoader imgLoader = new ImageLoader();

                imgLoader.FileName = newFileName;
                imgLoader.Load(_codec);
                RasterImage _image = imgLoader.Image;
                this.imageViewer.Image = null;
                imageFileName = newFileName;
                if (_image != null)
                {
                    this.imageViewer.Image = _image;
                    ImageSizing sizing = new ImageSizing(imageViewer);
                    sizing.SetScale(scaleFactor);
                    _codec.Options.Load.XResolution = imageViewer.Image.XResolution;
                    _codec.Options.Load.YResolution = imageViewer.Image.YResolution;
                    _annAutomation.Container.UnitConverter.DpiX = imageViewer.Image.XResolution;
                    _annAutomation.Container.UnitConverter.DpiY = imageViewer.Image.YResolution;
                    
                    if (imageViewer.Image.XResolution == 204 && imageViewer.Image.YResolution == 196)
                    {
                        imageViewer.UseDpi = true;
                    }
                    else
                    {
                        imageViewer.UseDpi = false;
                    }
                    
                    annotation.LoadFromFile(imageFileName);
                    annotation.LoadFromMemory(1);
                    imageToolBar.setFileName(newFileName);
                }

                _annAutomation.ObjectsDirty = false;
                _annAutomation.ImageDirty = false;

                imageToolBar.UpdateToolBar();
                
                saveAsFileName = null; // always user has to set it.
//                this.Edit = false;
            }
            catch (Exception e)
            {
                imageViewer.Image = null;
                _annAutomation.ObjectsDirty = false;
                _annAutomation.ImageDirty = false;
                imageToolBar.UpdateToolBar();
                XtraMessageBox.Show("An error occurred while loading the image for file: " + newFileName  + "." + Environment.NewLine +
                        "Error CNF-391 in " + FORM_NAME + ".LoadImage(): " + e.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadImage(Stream stream)
        {
            try
            {                
                ImageLoader imgLoader = new ImageLoader();
                imgLoader.Stream = stream;
                imgLoader.Load(_codec);
                RasterImage _image = imgLoader.Image;
                imageViewer.Image = null;
                if (_image != null)
                {
                    imageViewer.Image = _image;
                    ImageSizing sizing = new ImageSizing(imageViewer);
                    sizing.SetScale(scaleFactor);
                    _codec.Options.Load.XResolution = imageViewer.Image.XResolution;
                    _codec.Options.Load.YResolution = imageViewer.Image.YResolution;
                    _annAutomation.Container.UnitConverter.DpiX = imageViewer.Image.XResolution;
                    _annAutomation.Container.UnitConverter.DpiY = imageViewer.Image.YResolution;

                    if (imageViewer.Image.XResolution == 204 && imageViewer.Image.YResolution == 196)
                    {
                        imageViewer.UseDpi = true;
                    }
                    else
                    {
                        imageViewer.UseDpi = false;
                    }

                    annotation.LoadFromStream(stream);
                    annotation.LoadFromMemory(1);
                }

                _annAutomation.ObjectsDirty = false;
                _annAutomation.ImageDirty = false;

                imageToolBar.UpdateToolBar();

                saveAsFileName = null; // always user has to set it.
                Edit = false;
            }
            catch (Exception ex)
            {
                imageViewer.Image = null;
                _annAutomation.ObjectsDirty = false;
                _annAutomation.ImageDirty = false;
                imageToolBar.UpdateToolBar();
                throw new Exception("An error occurred while loading the image." + Environment.NewLine +
                     "Error CNF-392 in " + FORM_NAME + ".LoadImage(): " + ex.Message);
            }
        }


        public void PrintImage()
        {
            imageToolBar.PrintImage();
        }
        
        internal void SaveAnnotation()
        {         
            annotation.SaveToMemory(imageViewer.Image.Page);
            
        }
        internal void ReloadAnnotation()
        {
            if (GetImageDataOverride != null)
            {
                byte[] buffer = GetImageDataOverride();
                if (buffer == null)
                { 
                    XtraMessageBox.Show("Unable to Reload annotations. The original image data was not found." + Environment.NewLine +
                            "Error CNF-393 in " + FORM_NAME + ".ReloadAnnotation().",
                         FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var stream = new MemoryStream(buffer))
                {
                    annotation.LoadFromStream(stream);
                }
            }
            else
            {
                annotation.LoadFromFile(imageFileName);
            }            
            imageViewer.Invalidate();
            
            annotation.LoadFromMemory(imageViewer.Image.Page);
        }

        internal void LoadAnnotation()
        {           
            annotation.LoadFromMemory(imageViewer.Image.Page);
        }

        public void SaveToFile()
        {            
            int pageNum = imageViewer.Image.Page;
            annotation.SaveToMemory(pageNum);

            if (SaveImageOverrideDelegate != null)
            {
                SaveImageOverrideDelegate(this);
            }
            else if (!string.IsNullOrWhiteSpace(saveAsFileName))                
            {
                annotation.SaveToFile(saveAsFileName, true);
                ImageFileName = saveAsFileName;
                imageToolBar.setFileName(saveAsFileName);
            }
            else
            {                
                annotation.SaveToFile(imageFileName, true);
            }
            _annAutomation.ObjectsDirty = false;
            _annAutomation.ImageDirty = false;
        }
        /// <summary>
        ///  Writes the annotation onto the image permanently
        /// </summary>
        public void Publish()
        {
            int currentPage = imageViewer.Image.Page;
            try
            {
                // SaveToFile();
                if (saveAsFileName != null && "".Equals(saveAsFileName) == false)
                {
                    annotation.Publish(saveAsFileName);
                    annotation.RemoteAnnotation(saveAsFileName, imageViewer.Image.PageCount);
                }
                else
                {
                    annotation.Publish(imageFileName);
                    annotation.RemoteAnnotation(imageFileName, imageViewer.Image.PageCount);
                }
                _annAutomation.ObjectsDirty = false;
                _annAutomation.ImageDirty = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while publishing the image file." + Environment.NewLine +
                        "Error CNF-394 in " + FORM_NAME + ".Publish(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetPageNumber(currentPage);
            }
        }

        internal void SetPageNumber(int pageNum)
        {
            imageToolBar.SetPageNumber(pageNum);
        }
        private void IntializeAnnContainer()
        {
             SuspendLayout();

            imageViewer = new RasterImageViewer();
             imageViewer.Dock = DockStyle.Fill;
             imageViewer.EnableScrollingInterface = true;
             this.imageViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imageViewer_KeyDown);
             this.imageViewer.DoubleClick += new System.EventHandler(this.imageViewer_Clicked);
            // this.imageViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this._viewer_MouseMove);
             
             this.Controls.Add(imageViewer);

            

            _automationManager = new AnnAutomationManager();
            _annAutomation = new AnnAutomation(_automationManager, this.imageViewer);
            _annAutomation.Active = true;
            this.imageViewer.BringToFront();
            
            //imageViewer.
            
           
      
            _codec = new RasterCodecs();
            CodecsTxtLoadOptions txtOption = _codec.Options.Txt.Load;
            txtOption.Enabled = true;

            _automationManager.RasterCodecs = _codec;
            _automationManager.RedactionRealizePassword = "";
            _automationManager.CreateDefaultObjects();
            CustomizeObjectProperty();

             
            PropertyForm propForm = new PropertyForm();
            
            _automationManager.ObjectPropertiesDialogType = propForm.GetType();
         //   _automationManager.ObjectPropertiesDialogType = null;
            AddCheckMark(_automationManager);

            _automationManager.CreateToolBar();
            _automationManager.ToolBar.BringToFront();
            imageToolBar = new ImageToolBar();
            this.imageToolBar.TifEditor = this;
            toolBar = imageToolBar.GetExtendedToolBar(_automationManager.ToolBar);
            toolBar.Dock = DockStyle.Bottom;

            this.Controls.Add(_automationManager.ToolBar);




            topToolBar = imageToolBar.AddCustomToolBar(topToolBar);

            
            imageToolBar.UpdateToolBar();
            ResumeLayout();

            RasterPaintProperties prop = imageViewer.PaintProperties;
           // prop.PaintDisplayMode |= RasterPaintDisplayModeFlags.ScaleToGray | RasterPaintDisplayModeFlags.Bicubic;
            prop.PaintDisplayMode = RasterPaintDisplayModeFlags.ScaleToGray;
            imageViewer.PaintProperties = prop;
            

            
            InitialzeEdit();


        }

        private void AddCheckMark(AnnAutomationManager _annAutomation)
        {

            AnnAutomationObject annObject = new AnnAutomationObject();

            annObject.Id = AnnAutomationManager.UserObjectId;

            annObject.Name = _CHECK_MARK_NAME;

            PictureObject annStamp = new PictureObject();
            
            annStamp.Pen = new AnnPen(Color.Beige, new AnnLength(2, AnnUnit.Pixel));
            annStamp.Brush = new AnnSolidBrush(Color.Yellow);
            annStamp.Name = _CHECK_MARK_NAME;
            annStamp.NameVisible = false;
            
            annObject.DrawDesignerType = typeof(AnnRectangleDrawDesigner);
            annObject.EditDesignerType = typeof(AnnRectangleEditDesigner);
            annObject.RunDesignerType = typeof(AnnRunDesigner);


            annObject.Object = annStamp;
            
            Image image = Image.FromHbitmap(ImageEdit.check.GetHbitmap());
                
            Bitmap btmp = new Bitmap(image);
            using (Graphics g = Graphics.FromImage(btmp))
            {
                g.FillRectangle(Brushes.Magenta,0,0,16,16);
               // g.DrawImage(image,0,0,16,16);
            }
            
            annObject.ToolBarImage = image;
            annObject.ToolBarToolTipText = "Check Mark";
            _annAutomation.Objects.Add(annObject);
        }

        private void CustomizeObjectProperty()
        {
            foreach (AnnAutomationObject autoObj in _automationManager.Objects)
            {
                if (autoObj.Object != null)
                {

                    if (autoObj.Id == AnnAutomationManager.TextObjectId || autoObj.Id == AnnAutomationManager.TextPointerObjectId || autoObj.Id == AnnAutomationManager.NoteObjectId 
                        || autoObj.Id == AnnAutomationManager.TextRollupObjectId)
                    {
                        ((AnnTextObject)autoObj.Object).Text = "Enter your data";
                    }
                    else
                    {
                        if (autoObj.Id == AnnAutomationManager.StampObjectId && !_CHECK_MARK_NAME.Equals(autoObj.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            ((AnnStampObject)autoObj.Object).Text = "Signature is missing";
                        }
                        ContextMenu contextMenu = autoObj.ContextMenu;
                        ContextMenu newMenu = new ContextMenu();
                        foreach (MenuItem menu1 in contextMenu.MenuItems)
                        {
                            MenuItem m2 = menu1.CloneMenu();
                            newMenu.MenuItems.Add(m2);
                        }
                        newMenu.MenuItems[newMenu.MenuItems.Count - 1].Enabled = false;
                        autoObj.ContextMenu = newMenu;
                      
                    }
                    AnnPen pen = new AnnPen(Color.Red, new AnnLength(4.0f, AnnUnit.Pixel));
                    autoObj.Object.Pen = pen;
                }
            }
        }

        private void imageViewer_Clicked(object sender, EventArgs e)
        {
       //     return;
            MouseEventArgs eventArgs = (MouseEventArgs)e;
            Point p = new Point(eventArgs.X,eventArgs.Y);
            //Point clientP = imageViewer.Con

            Image image = null;
            
            if (toolBar != null && this.edit)
            {
                  //  this.imageViewer.
                if (Form.ModifierKeys == Keys.Control)
                {
                    image = (Image.FromHbitmap(ImageEdit.RedCheckmarkOnly.GetHbitmap()));
                }
                else
                {
                    image = (Image.FromHbitmap(ImageEdit.CheckmarkOnly.GetHbitmap()));
                }

                    AnnPicture pic = new AnnPicture(image);

                    AnnStampObject obj = new AnnStampObject();
                    obj.Picture = pic;
                    obj.Visible = true;
                    //  obj.
                    Transformer transform = new Transformer();
                    transform.Transform = this.imageViewer.Transform;


                    PointF pf = transform.PointToLogical(new PointF(eventArgs.X - 10, eventArgs.Y - 10));

                    obj.Bounds = new AnnRectangle(pf.X, pf.Y, 20, 20, AnnUnit.Pixel);
                    //  obj.Bounds = new AnnRectangle(new Rectangle(, new SizeF(30, 30)));
                    _annAutomation.Container.Objects.Add(obj);
                    _annAutomation.ImageDirty = true;
                    this.imageViewer.Invalidate();
             }
        }

        private void _viewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (_annAutomation.Container != null)
            {
                AnnPoint physical = new AnnPoint(e.X, e.Y, AnnUnit.Pixel);
                AnnTransformer transformer = new AnnTransformer(_annAutomation.Container.UnitConverter, _annAutomation.Container.Transform);
                AnnPoint logical = transformer.PointToLogical(physical);
            }
        }

        private void InitialzeEdit()
        {
            annotation = new Annotation();
            annotation.TifEditor = this;
            eventHandler = new EditEventHandler();
            eventHandler.Editor = this;

            SuspendLayout();
            _annAutomation.Container.Objects.ItemAdded += new EventHandler<RasterCollectionEventArgs<AnnObject>>(eventHandler.Objects_ItemAddedRemoved);
            _annAutomation.Container.BeforeDrawingObjects += new EventHandler<AnnPaintEventArgs>(Container_BeforeDrawingObjects);
         
            ResumeLayout();
        }

        private void Container_BeforeDrawingObjects(object sender, AnnPaintEventArgs e)
        {
            e.Graphics.SmoothingMode =  SmoothingMode.None;
        }
        
        private void CleanUp()
        {
        }

        private void imageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    this.AnnAutomation.Delete();

                }
            }
        }
        private void UserModeChanged()
        {
            if (edit) {
                _automationManager.UserMode = AnnUserMode.Design;
            }
            else {
                _automationManager.UserMode = AnnUserMode.Run;
            }
            imageToolBar.UpdateModeToolBar();
        }

        private void TifEditor_Enter(object sender, EventArgs e)
        {
            CurrentEditor = this;
        }

        private void SetStampObjectText()
        {
            string stampText = "Signature is missing";
            if (userId != null && !"".Equals(userId))
            {
                stampText = userId + " signature";
            }
            foreach (AnnAutomationObject autoObj in _automationManager.Objects)
            {
                if (autoObj.Object != null && autoObj.Id == AnnAutomationManager.StampObjectId &&
                    !_CHECK_MARK_NAME.Equals(autoObj.Name, StringComparison.OrdinalIgnoreCase))
                {
                    ((AnnStampObject) autoObj.Object).Text = stampText;

                }
            }
        }

        public byte[] GetImageBytes()
        {
            return annotation.SaveToBytes();
        }

        public byte[] PublishToBytes()
        {
            return annotation.PublishToBytes();
        }
    }
}