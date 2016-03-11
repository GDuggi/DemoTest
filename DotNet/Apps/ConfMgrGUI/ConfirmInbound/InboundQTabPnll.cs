using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using OpsTrackingModel;
using DataManager;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using CommonUtils;
using DevExpress.XtraEditors;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Columns;
using DBAccess;
using DBAccess.SqlServer;
using DevExpress.XtraBars;
using log4net;
using Sempra.Confirm.InBound.ImageEdit;

namespace ConfirmInbound
{    
    public delegate void PrintDocumentDelegate();
    public delegate bool SplitDocumentDelegate(InboundDocsView ibDoc, AssociatedDoc assDoc);
    public delegate string GetLoadedDocFilename();
    public delegate void ViewAssDocsByInbDodId(Int32 inbDocId);
    public delegate string SubmitEditRqmtDelegate(long ATradeId, long ARqmtId, string ARqmtCode, string AStatusCode,
        DateTime AStatusDate, string AReference, string AComment, bool AUpdateLocalTableNow, bool isUnAssociated);
    public delegate void MergeInboundQTabFilesDelegate(string parrentFile, string assFile);

    [Flags]
    public enum ViewProperties : int
    {
        DefaultView = 1,
        DiscardedView = 2,        
        IgnoredView = 6
    }

    public partial class InboundQTabPnll : UserControl, IInbound
    {
        private const string FORM_NAME = "InboundQTabPnll";
        private const string FORM_ERROR_CAPTION = "Inbound Queue Tab Panel Form Error";
        private static ILog Logger = LogManager.GetLogger(typeof(InboundQTabPnll));
        private const string USER_FLAG_UPDATE = "U";
        private const string USER_FLAG_DELETE = "D";
        
        private GridHitInfo downHitInfo = null;
        //private InboundWebServices inboundWebServices = null;
        private ViewProperties viewProperties;
        private volatile bool isCacheUpdating;

        private InboundDocsView focusedInbDocBeforeUpdates = null;
        private IImagesDal imagesDal;
        private IXmitRequestDal xmitRequestDal;
        private string sqlConnectionString = String.Empty;
        public string p_UserId = "";
        private InboundDocsDal inboundDocsDal;        
        private readonly IVaulter vaulter;

        public IsDocumentModifiedDelegate IsDocumentEdited { get; set; }                
        public SubmitEditRqmtDelegate SubmitEditRqmtDelegate { get; set; }
        public ViewAssDocsByInbDodId ViewAssDocsByInbDocIdDelegate { get; set; }                
        public PrintDocumentDelegate PrintDocumentDelegate { get; set; }
        public DataTable CptyLkupTable { get; set; }
        public DataView DefaultView { get; set; }
        public string TabFilter { get; set; }
        
        public InboundQTabPnll(string pSqlConnectionString, string pUserId)
        {
            TabFilter = "";
            DefaultView = null;
            CptyLkupTable = null;
            InitializeComponent();
            sqlConnectionString = pSqlConnectionString;
            imagesDal = new ImagesDal(sqlConnectionString);
            this.Dock = DockStyle.Fill;
            p_UserId = pUserId;
            inboundDocsDal = new InboundDocsDal(sqlConnectionString);
            vaulter = new Vaulter(sqlConnectionString);            
            xmitRequestDal = new XmitRequestDal(sqlConnectionString);
        }

        public void SubmitEditRqmtFromInbound(long ATradeId, long ARqmtId, string ARqmtCode, string AStatusCode,
            DateTime AStatusDate, string AReference, string AComment, bool AUpdateLocalTableNow, bool isUnAssociated)
        {
            SubmitEditRqmtDelegate.Invoke(ATradeId, ARqmtId, ARqmtCode, AStatusCode, AStatusDate, AReference, AComment, AUpdateLocalTableNow, isUnAssociated);
        }       

        public void InitViewProperties(ViewProperties props)
        {
            this.viewProperties = props;

            if ((((viewProperties & ViewProperties.DefaultView) == ViewProperties.DefaultView) || ((viewProperties & ViewProperties.DiscardedView) == ViewProperties.DiscardedView) ))
            {
                barBtnDiscard.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                barBtnDiscard.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }


            if (((viewProperties & ViewProperties.DefaultView) == ViewProperties.DefaultView))
            {
                barBtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barBtnMatchDoc.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barBtnIgnore.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                barBtnBookmark.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barBtnDocCmt.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barBtnUserCmts.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                barBtnRedirectFax.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                splitButton.Visibility = BarItemVisibility.Always;
                btnMergeDocument.Visibility = BarItemVisibility.Always;
            }
            else
            {
                barBtnMatchDoc.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barBtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barBtnIgnore.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                barBtnBookmark.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barBtnDocCmt.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barBtnUserCmts.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barBtnRedirectFax.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            if (viewProperties == ViewProperties.DiscardedView)
            {
                barBtnDiscard.ImageIndex = 18;
                barBtnDiscard.Caption = "Un-Discard";                
                splitButton.Visibility = BarItemVisibility.Never;
                btnMergeDocument.Visibility = BarItemVisibility.Never;
            }

            if (viewProperties == ViewProperties.IgnoredView)
            {
                barBtnDiscard.Visibility = BarItemVisibility.Never;                
                barBtnIgnore.Visibility = BarItemVisibility.Always;
                barBtnIgnore.Caption = "Un-Ignore";
                barBtnIgnore.ImageIndex = 31;
                splitButton.Visibility = BarItemVisibility.Never;
                btnMergeDocument.Visibility = BarItemVisibility.Never;
            }
        }

        public void SetGridDataSource(ref DataView dataTable)
        {            
            gridControlInboundDocs.DataSource = dataTable;
            gridControlInboundDocs.ForceInitialize();
            gridControlInboundDocs.LevelTree.Nodes.Add("InboundAssociated", gridViewAssociatedDocs);            
        }

        public void LoadImageInEditor(InboundDocsView view)
        {
            if (IsDocumentEdited != null && IsDocumentEdited() && !isCacheUpdating)
            {
                if (MessageBox.Show(@"Save image changes?", @"Save Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    ImagesEventManager.Instance.Raise(new ImagesSavingEventArgs());
                }
            }

            if (view != null)
            {
                var imagesDto = imagesDal.GetByDocId(view.Id, ImagesDtoType.Inbound);
                if (imagesDto == null)
                {
                    Logger.WarnFormat("Attempt to display non-existant inbound image for id = {0}", view.Id);
                }
                ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(imagesDto, (imagesDto != null)));
            }
            else
            {
                ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(null, false));
            }                        
        }

        public GridControl MainDataGrid
        {
            get{ return gridControlInboundDocs; }
        }

        private void gridViewInboundDocs_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DisplayInboundDocument();
        }

        private void gridViewInboundDocs_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfo = null;
            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;
        }

        private void gridViewInboundDocs_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfo.RowHandle);
                    InboundDocsView doc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(row);
                    view.GridControl.DoDragDrop(doc, DragDropEffects.Move);
                    downHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        public void PrintDocument()
        {
            try
            {
                PrintDocumentDelegate.Invoke();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while while printing the document." + Environment.NewLine +
                       "Error CNF-482 in " + FORM_NAME + ".ReadUserSettings(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DiscardDocument()
        {
            Dictionary<int, string> inbDocsInputList = new Dictionary<int, string>();
            IList<InboundDocsView> inbDocViewList = new List<InboundDocsView>();
            bool isDiscard = true;
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                    {
                        var dr = gridViewInboundDocs.GetDataRow(rowHandle);
                        var inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                        inbDocViewList.Add(inbDocView);
                     
                        var inbDocId = Convert.ToInt32(inbDocView.Id);

                        var inbDocStatusCode = inbDocView.DocStatusCode.Equals(AssociatedDoc.DISCARDED) ? AssociatedDoc.OPEN : AssociatedDoc.DISCARDED;                        
                        
                        inbDocsInputList.Add(inbDocId, inbDocStatusCode);                        
                    }
                    SubmitDiscardDocumentsRequest(inbDocsInputList);        
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while discarding the document." + Environment.NewLine +
                       "Error CNF-483 in " + FORM_NAME + ".DiscardDocument(): ", ex);
            }
            finally
            {
                DisplayInboundDocument();
            }
        }

        private void SubmitDiscardDocumentsRequest(Dictionary<int, string> pInbDocsList)
        {
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();

                Dictionary<int, int> rowsUpdatedList = new Dictionary<int, int>();
                rowsUpdatedList = inboundDocsDal.UpdateStatus(pInbDocsList);
                
                foreach (var data in pInbDocsList)
                {
                    DataRow drFind = null;
                    int id = data.Key; 
                    int rowsUpdated = 0;
                    if (rowsUpdatedList.TryGetValue(id, out rowsUpdated))
                    {                        
                        if (rowsUpdated > 0)
                        {                            
                            drFind = InboundDocsViewTable.Rows.Find(id);
                            if (drFind != null)
                            {                         
                                drFind["DocStatusCode"] = data.Value;
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("An error occurred while updating the database for the discarded document." + Environment.NewLine +
                               "Error CNF-484 in " + FORM_NAME + ".SubmitDiscardDocumentsRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the database for the discarded document." + Environment.NewLine +
                               "Error CNF-485 in " + FORM_NAME + ".SubmitDiscardDocumentsRequest(): ", ex);                
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        private void inputBox_Validating(object sender, InputBoxValidatingArgs e)
        {
            if (e.Text.Trim().Length > 0)
            {
                try
                {
                    Convert.ToInt32((e.Text.Trim()));
                }
                catch (Exception err)
                {
                    e.Cancel = true;
                    e.Message = err.Message;
                }
            }
            else
            {
                e.Cancel = true;
                e.Message = "Required / Invalid Number";
            }
        }
        
        public void CopyDocument()
        {
            try
            {
                InputBoxResult result = InputBox.Show("Copies:", "Inbound Document Copies", "1", inputBox_Validating);
                if (result.OK)
                {
                    using (new CursorBlock(Cursors.WaitCursor))
                    {
                        int copies = Convert.ToInt32(result.Text);
                        if (copies > 0)
                        {
                            var dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
                            var inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                            var originalImagesDto = imagesDal.GetByDocId(inbDocView.Id, ImagesDtoType.Inbound);
                            string copyFileName = GenerateCopyFileName(inbDocView);
                            for (int i = 1; i <= copies; i++)
                            {                                
                                var inboundDocsData = DtoViewConverter.CreateInboundDocsDto(inbDocView, copyFileName);
                                int newId = inboundDocsDal.Insert(inboundDocsData);

                                if (newId <= 0)
                                {
                                    throw new Exception("Error CNF-537: The documeent was not inserted into the Database.");
                                }
                                
                                imagesDal.Insert(
                                    new ImagesDto(newId,
                                        originalImagesDto.MarkupImage,
                                        originalImagesDto.MarkupImageFileExt,
                                        originalImagesDto.OriginalImage,
                                        originalImagesDto.OriginalImageFileExt,
                                        originalImagesDto.Type)
                                    );

                                var newView = DtoViewConverter.CreateInboundDocsView(inboundDocsData);
                                newView.Id = newId;
                                newView.FileName = copyFileName;
                                ApplyNewInboundDocToView(newView);
                                copyFileName = GenerateCopyFileName(newView);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while copying the document." + Environment.NewLine +
                               "Error CNF-486 in " + FORM_NAME + ".CopyDocument(): ", ex);
            }
            finally
            {
                DisplayInboundDocument();             
            }
        }

        private string GenerateCopyFileName(InboundDocsView inbDocView, string fileNameMiddle = "_copy_")
        {
            var fileName = inbDocView.FileName;
            var lastDot = fileName.LastIndexOf('.');
            var ext = (lastDot < 0) ? "" : fileName.Substring(lastDot, fileName.Length-lastDot);

            fileName = fileName.Replace(ext, "");
            var pattern = string.Format("(.*){0}([0-9]+)$",fileNameMiddle);
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(fileName);
            if (!match.Success)
            {
                return string.Format("{0}{1}1{2}", fileName, fileNameMiddle, ext);
            }

            var filePrefix = match.Groups[1].Value;
            var numCapture = match.Groups[2].Value;
            var next = Convert.ToInt32(numCapture) + 1;                
            return string.Format("{0}{1}{2}{3}", filePrefix, fileNameMiddle, next, ext);
        }

        public void CloseOutDocument()
        {
            Dictionary<int, string> inbDocsInputList = new Dictionary<int, string>();

            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                    {
                        var dr = gridViewInboundDocs.GetDataRow(rowHandle);
                        var inbDocsView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);                        
                        var inbDocId = Convert.ToInt32(inbDocsView.Id);
                        var inbDocStatusCode = inbDocsView.DocStatusCode.Equals(AssociatedDoc.CLOSED) ? AssociatedDoc.OPEN : AssociatedDoc.CLOSED;                        
                        inbDocsInputList.Add(inbDocId, inbDocStatusCode);
                    }
                    SubmitCloseDocumentsRequest(inbDocsInputList);                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error CNF-487 in QTabPnl Closeout Document: " + ex.Message, ex);
                XtraMessageBox.Show("An error occurred while closing out the document." + Environment.NewLine +
                       "Error CNF-487 in " + FORM_NAME + ".CloseOutDocument(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DisplayInboundDocument();               
            }
        }
        
        private void SubmitCloseDocumentsRequest(Dictionary<int, string> pInbDocsList)
        {           
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();
                
                var rowsUpdatedList = inboundDocsDal.UpdateStatus(pInbDocsList);
                
                foreach (var data in pInbDocsList)
                {
                    int id = data.Key;
                    int rowsUpdated = 0;
                    if (rowsUpdatedList.TryGetValue(id, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {
                            var drFind = InboundDocsViewTable.Rows.Find(id);
                            if (drFind != null)
                            {                         
                                drFind["DocStatusCode"] = data.Value;
                            }
                        }
                    }
                    else
                    {                            
                        XtraMessageBox.Show("An error occurred while updating the database with the closed document." + Environment.NewLine +
                               "Error CNF-488 in " + FORM_NAME + ".SubmitCloseDocumentsRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while closing the Inbound Document." + Environment.NewLine +
                       "Error CNF-489 in " + FORM_NAME + ".SubmitCloseDocumentsRequest().", ex);
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();
            }
        }

        private void FinishApplyingInboundGridViewChanges()
        {
            try
            {                
                InboundDocsViewTable.EndLoadData();
                gridViewInboundDocs.EndDataUpdate();
                gridViewInboundDocs.ClearSelection();
                gridViewInboundDocs.SelectRow(gridViewInboundDocs.FocusedRowHandle);
            }
            catch (Exception e)
            {
                Logger.Error("Error applying grid view changes:" + e.Message, e);
            }
        }

        private static void LogAndDisplayException(string errorMessagePrefix, Exception ex)
        {
            Logger.Error("Error CNF-490: " + errorMessagePrefix + ex.Message, ex);
            XtraMessageBox.Show("An error occurred while performing the following process: " + errorMessagePrefix + "." + Environment.NewLine +
                   "Error CNF-490 in " + FORM_NAME + ".LogAndDisplayException(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void MapCallerReference()
        {
            DataRow dr = null;
            InboundDocsView inbDoc = null;
            RefEditForm refEdit = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                {
                    dr = gridViewInboundDocs.GetDataRow(rowHandle);
                    inbDoc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    refEdit = new RefEditForm();

                    refEdit.FaxType = RefEditForm.FaxDataType.CounterPartyFax;
                    refEdit.CallerReference = inbDoc.CallerRef;
                    refEdit.InitLookup(CptyLkupTable);

                    refEdit.ShowDialog();
                    if (refEdit.DialogResult == DialogResult.OK)
                    {
                        //Israel 7/16/15 -- This structure seems unncessary--we are creating an arraylist to submit one item at a time.
                        //However, in the interest of not needlessly breaking the code due to some unforeseen circumstances, 
                        //we have elected to support the original structure.
                        //mapCallerReferenceRequest[] requestArray = new mapCallerReferenceRequest[1];
                        //mapCallerReferenceRequest request = new mapCallerReferenceRequest();

                        List<InboundDocCallerRefDto> callerRefList = new List<InboundDocCallerRefDto>();
                        InboundDocCallerRefDto callerRefData = new InboundDocCallerRefDto();

                        if (refEdit.CallerReference == null || refEdit.CallerReference == "")
                        {
                            XtraMessageBox.Show("Please Map a valid Caller Reference Value.");
                        }
                        else
                        {
                            //request.callerRef = refEdit.CallerReference;
                            //request.cptySn = refEdit.CptyMapping;
                            //request.refType = "CPTY";
                            //requestArray[0] = request;
                            callerRefData.CallerRef = refEdit.CallerReference;
                            callerRefData.CptyShortCode = refEdit.CptyMapping;
                            callerRefData.RefType = "CPTY";
                            callerRefList.Add(callerRefData);
                            
                            //SubmitCallerReferenceRequest(requestArray, false)//;
                            SubmitCallerReferenceRequest(callerRefList, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while mapping the caller reference." + Environment.NewLine +
                       "Error CNF-491 in " + FORM_NAME + ".MapCallerReference(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void SubmitCallerReferenceRequest(List<InboundDocCallerRefDto> pCallerRefList, bool unMap)
        {            
            InboundDocCallerRefDal inboundDocsCallerRefDal = new InboundDocCallerRefDal(sqlConnectionString);

            Dictionary<string, int> updatedRowsList = new Dictionary<string, int>();
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();
                if (unMap)
                {
                    //ufr = inboundWebServices.unMapCallerRef(requestArray);
                    updatedRowsList = inboundDocsCallerRefDal.UnmapCallerRef(pCallerRefList);
                }
                else
                {
                    //ufr = inboundWebServices.mapCallerRef(requestArray);
                    updatedRowsList = inboundDocsCallerRefDal.MapCallerRef(pCallerRefList);
                }

                //foreach (mapCallerReferenceResponse resp in ufr)
                foreach (InboundDocCallerRefDto data in pCallerRefList)
                {
                    int rowsUpdated = 0;
                    //if (resp.responseStatus.Equals("OK"))
                    if (updatedRowsList.TryGetValue(data.CallerRef, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {
                            foreach (DataRow dr in InboundDocsViewTable.Rows)
                            {                               
                                if (data.CallerRef.Equals(dr["CallerRef"]))
                                {
                                    dr["MappedCptySn"] = data.CptyShortCode;                             
                                }
                            }
                        }
                    }
                    else
                    {                        
                        XtraMessageBox.Show("An error occurred while updating the database with the caller reference." + Environment.NewLine +
                               "Error CNF-492 in " + FORM_NAME + ".SubmitCallerReferenceRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the database with the caller reference." + Environment.NewLine +
                       "Error CNF-493 in " + FORM_NAME + ".SubmitCallerReferenceRequest(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        public void BookmarkDocument()
        {
            DataRow dr = null;
            bool isBookMark = false;
            //userFlagRequest[] requestArray = new userFlagRequest[gridViewInboundDocs.GetSelectedRows().Length];
            //int counter = 0;
            List<InboundDocUserFlagDto> userFlagList = new List<InboundDocUserFlagDto>();
            InboundDocsView inbDocView = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                {
                    //userFlagRequest request = new userFlagRequest();
                    InboundDocUserFlagDto userFlagData = new InboundDocUserFlagDto();
                    dr = gridViewInboundDocs.GetDataRow(rowHandle);
                    inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    isBookMark = "BOOKMARK".Equals(inbDocView.BookmarkFlag);

                    //request.inboundDocId = inbDocView.Id;
                    //request.flagType = "BOOKMARK";
                    //request.userName = (inboundWebServices.userName.Text[0]).ToUpper();
                    //if (isBookMark)  
                    //{
                    //    request.updateDeleteFlag = updateUserFlag.Delete;
                    //}
                    //else  
                    //{
                    //    request.updateDeleteFlag = updateUserFlag.Update;
                    //}
                    //request.updateDeleteFlagSpecified = true;
                    //requestArray[counter] = request;
                    //counter++;

                    userFlagData.InboundDocId = Convert.ToInt32(inbDocView.Id);
                    userFlagData.FlagType = "BOOKMARK";
                    userFlagData.InboundUser = p_UserId;
                    if (isBookMark)
                    {
                        userFlagData.UpdateDeleteInd = USER_FLAG_DELETE;
                    }
                    else
                    {
                        userFlagData.UpdateDeleteInd = USER_FLAG_UPDATE;
                    }
                    userFlagList.Add(userFlagData);
                }

                //SubmitBookmarkDocumentsRequest(requestArray);
                SubmitBookmarkDocumentsRequest(userFlagList);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the grid with the bookmark." + Environment.NewLine +
                       "Error CNF-494 in " + FORM_NAME + ".BookmarkDocument(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void SubmitBookmarkDocumentsRequest(List<InboundDocUserFlagDto> pUserFlagList)
        {
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();

                InboundDocUserFlagDal inboundDocUserFlagDal = new InboundDocUserFlagDal(sqlConnectionString);
                Dictionary<int, int> rowsUpdatedList = new Dictionary<int, int>();
                rowsUpdatedList = inboundDocUserFlagDal.UpdateFlags(pUserFlagList);

                foreach (InboundDocUserFlagDto data in pUserFlagList)
                {                    
                    int rowsUpdated = 0;
                    if (rowsUpdatedList.TryGetValue(data.InboundDocId, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {
                            var drFind = InboundDocsViewTable.Rows.Find(data.InboundDocId);
                            if (drFind != null)
                            {                                
                                if (data.UpdateDeleteInd.Equals(USER_FLAG_DELETE))
                                {
                                    drFind["BookmarkFlag"] = System.DBNull.Value;
                                }
                                else
                                {
                                    drFind["BookmarkFlag"] = "BOOKMARK";
                                }
                            }
                        }
                    }
                    else
                    {                        
                        XtraMessageBox.Show("An error occurred while updating the database with the bookmark." + Environment.NewLine +
                               "Error CNF-495 in " + FORM_NAME + ".SubmitBookmarkDocumentsRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the database with the bookmark." + Environment.NewLine +
                               "Error CNF-496 in " + FORM_NAME + ".SubmitBookmarkDocumentsRequest().", ex);
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        public void CreateUserComment()
        {
            DataRow dr = null;
            bool deleteUserComment = false;
            //userFlagRequest[] requestArray = new userFlagRequest[gridViewInboundDocs.GetSelectedRows().Length];
            //int counter = 0;
            List<InboundDocUserFlagDto> userFlagList = new List<InboundDocUserFlagDto>();
            InboundDocsView inbDocView = null;
            string userComment = "";
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                {
                    //userFlagRequest request = new userFlagRequest();
                    InboundDocUserFlagDto userFlagData = new InboundDocUserFlagDto();
                    dr = gridViewInboundDocs.GetDataRow(rowHandle);
                    inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    userComment = inbDocView.CommentUser;
                    CommentForm commentForm = new CommentForm();
                    commentForm.Comment = userComment;
                    commentForm.ShowDialog();

                    if (commentForm.DialogResult == DialogResult.OK)
                    {
                        //Israel 9/17/2015
                        userComment = commentForm.Comment;     
                    }

                    deleteUserComment = ((userComment == null) || (userComment.Trim() == ""));

                    //request.inboundDocId = inbDocView.Id;
                    //request.flagType = "COMMENT";
                    //request.userName = (inboundWebServices.userName.Text[0]).ToUpper();
                    //request.comment = userComment;
                    //if (deleteUserComment)
                    //{
                    //    request.updateDeleteFlag = updateUserFlag.Delete;
                    //}
                    //else
                    //{
                    //    request.updateDeleteFlag = updateUserFlag.Update;
                    //}
                    //request.updateDeleteFlagSpecified = true;
                    //requestArray[counter] = request;
                    //counter++;

                    userFlagData.InboundDocId = Convert.ToInt32(inbDocView.Id);
                    userFlagData.FlagType = "COMMENT";
                    userFlagData.InboundUser = p_UserId;
                    userFlagData.Comments = userComment;
                    if (deleteUserComment)
                    {
                        userFlagData.UpdateDeleteInd = USER_FLAG_DELETE;
                    }
                    else
                    {
                        userFlagData.UpdateDeleteInd = USER_FLAG_UPDATE;
                    }
                    userFlagList.Add(userFlagData);
                }
                //SubmitUserCommentRequest(requestArray);
                SubmitUserCommentRequest(userFlagList);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the grid with the User Comment." + Environment.NewLine +
                       "Error CNF-497 in " + FORM_NAME + ".CreateUserComment(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
                
        private void SubmitUserCommentRequest(List<InboundDocUserFlagDto> pUserFlagList)
        {
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();

               
                InboundDocUserFlagDal inboundDocUserFlagDal = new InboundDocUserFlagDal(sqlConnectionString);
                Dictionary<int, int> rowsUpdatedList = new Dictionary<int, int>();
                rowsUpdatedList = inboundDocUserFlagDal.UpdateFlags(pUserFlagList);
               
                foreach (InboundDocUserFlagDto data in pUserFlagList)
                {
                    int rowsUpdated = 0;
                    if (rowsUpdatedList.TryGetValue(data.InboundDocId, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {                            
                            DataRow drFind = InboundDocsViewTable.Rows.Find(data.InboundDocId);
                            if (drFind != null)
                            {                                
                                if (data.UpdateDeleteInd.Equals(USER_FLAG_DELETE))
                                {
                                    drFind["CommentFlag"] = System.DBNull.Value;
                                    drFind["CommentUser"] = System.DBNull.Value;
                                }
                                else
                                {
                                    drFind["CommentFlag"] = "COMMENT";                             
                                    drFind["CommentUser"] = data.Comments;
                                }
                            }
                        }
                    }
                    else
                    {                        
                        XtraMessageBox.Show("An error occurred while updating the database with the User Comment." + Environment.NewLine +
                               "Error CNF-498 in " + FORM_NAME + ".SubmitUserCommentRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the database with the User Comment." + Environment.NewLine +
                               "Error CNF-499 in " + FORM_NAME + ".SubmitUserCommentRequest().", ex);                
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        public void IgnoreDocument()
        {            
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    List<InboundDocUserFlagDto> userFlagList = new List<InboundDocUserFlagDto>();
                    foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                    {
                        InboundDocUserFlagDto userFlagData = new InboundDocUserFlagDto();
                        var dr = gridViewInboundDocs.GetDataRow(rowHandle);
                        var inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                        var isIgnored = "IGNORE".Equals(inbDocView.IgnoreFlag);

                        userFlagData.InboundDocId = Convert.ToInt32(inbDocView.Id);
                        userFlagData.FlagType = "IGNORE";
                        userFlagData.InboundUser = p_UserId;
                        userFlagData.UpdateDeleteInd = isIgnored ? USER_FLAG_DELETE : USER_FLAG_UPDATE;
                        userFlagList.Add(userFlagData);
                    }

                    SubmitIgnoreDocumentsRequest(userFlagList);
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the grid with the Ignore setting." + Environment.NewLine +
                               "Error CNF-500 in " + FORM_NAME + ".IgnoreDocument().", ex);
            }
            finally
            {
                DisplayInboundDocument();                
            }
        }
        
        private void SubmitIgnoreDocumentsRequest(List<InboundDocUserFlagDto> pUserFlagList)
        {            
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();
                
                InboundDocUserFlagDal inboundDocUserFlagDal = new InboundDocUserFlagDal(sqlConnectionString);
                Dictionary<int, int> rowsUpdatedList = new Dictionary<int, int>();
                rowsUpdatedList = inboundDocUserFlagDal.UpdateFlags(pUserFlagList);

                foreach (InboundDocUserFlagDto data in pUserFlagList)
                {                    
                    int rowsUpdated = 0;
                    if (rowsUpdatedList.TryGetValue(data.InboundDocId, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {                            
                            var drFind = InboundDocsViewTable.Rows.Find(data.InboundDocId);
                            if (drFind != null)
                            {                         
                                if (data.UpdateDeleteInd.Equals(USER_FLAG_DELETE))
                                {
                                    drFind["IgnoreFlag"] = System.DBNull.Value;
                                }
                                else
                                {
                                    drFind["IgnoreFlag"] = "IGNORE";
                                }
                            }
                        }
                    }
                    else
                    {                        
                        XtraMessageBox.Show("An error occurred while updating the database with the Ignore setting." + Environment.NewLine +
                               "Error CNF-501 in " + FORM_NAME + ".SubmitIgnoreDocumentsRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the database with the Ignore setting." + Environment.NewLine +
                               "Error CNF-502 in " + FORM_NAME + ".SubmitIgnoreDocumentsRequest().", ex);
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        public void CreateDocumentComment()
        {
            List<InboundDocsDto> inboundDocsDataList = new List<InboundDocsDto>();
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                    {
                        InboundDocsDto inboundDocsData = new InboundDocsDto();
                        var dr = gridViewInboundDocs.GetDataRow(rowHandle);
                        var inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);

                        inboundDocsData.CallerRef = inbDocView.CallerRef;
                        inboundDocsData.Cmt = GetInboundDocComment(inbDocView.Cmt);
                        if (inboundDocsData.Cmt == inbDocView.Cmt)
                            return; // no comment changes
                        inboundDocsData.DocStatusCode = inbDocView.DocStatusCode;
                        inboundDocsData.FileName = inbDocView.FileName;
                        inboundDocsData.Id = inbDocView.Id;
                        inboundDocsData.Sender = inbDocView.Sender;
                        inboundDocsData.SentTo = inbDocView.SentTo;

                        inboundDocsDataList.Add(inboundDocsData);
                    }

                    SubmitUpdateInboundDocRequest(inboundDocsDataList);
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the grid with the document comment." + Environment.NewLine +
                               "Error CNF-503 in " + FORM_NAME + ".CreateDocumentComment().", ex);
            }            
        }

        private string GetInboundDocComment(string currentComment)
        {
            try
            {
                CommentForm commentForm = new CommentForm();
                commentForm.Comment = currentComment;
                commentForm.ShowDialog();

                if (commentForm.DialogResult == DialogResult.OK)
                {
                    //Israel 9/17/2015
                    return commentForm.Comment;
                }
                else return currentComment;
            }
            catch(Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while retrieving the document comment with the following value:" + Environment.NewLine +
                    "Comment: " + currentComment + Environment.NewLine +
                     "Error CNF-504 in " + FORM_NAME + ".GetInboundDocComment(): " + ex.Message);
            }
        }
        
        private void SubmitUpdateInboundDocRequest(List<InboundDocsDto> pInboundDocsDataList)
        {
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundDocsViewTable.BeginLoadData();

                Dictionary<int, int> rowsUpdatedList = new Dictionary<int, int>();
                rowsUpdatedList = inboundDocsDal.Update(pInboundDocsDataList);
                
                foreach (InboundDocsDto data in pInboundDocsDataList)
                {
                    int rowsUpdated = 0;
                    int id = Convert.ToInt32(data.Id);
                    if (rowsUpdatedList.TryGetValue(id, out rowsUpdated))
                    {
                        if (rowsUpdated > 0)
                        {
                            var drFind = InboundDocsViewTable.Rows.Find(data.Id);
                            if (drFind != null)
                            {
                                drFind["CallerRef"] = data.CallerRef;
                                drFind["Cmt"] = data.Cmt;
                                drFind["DocStatusCode"] = data.DocStatusCode;
                                drFind["FileName"] = data.FileName;
                                drFind["Sender"] = data.Sender;
                                drFind["SentTo"] = data.SentTo;
                            }                        
                        }
                    }
                    else
                    {                        
                        XtraMessageBox.Show("An error occurred while updating the database with the Inbound Documents data." + Environment.NewLine +
                               "Error CNF-505 in " + FORM_NAME + ".SubmitUpdateInboundDocRequest().",
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the database with the Inbound Documents data." + Environment.NewLine +
                               "Error CNF-506 in " + FORM_NAME + ".SubmitUpdateInboundDocRequest().", ex);                
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();                
            }
        }

        public void UnMapCallerReference()
        {
            DataRow dr = null;
            InboundDocsView inbDocView = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                {
                    dr = gridViewInboundDocs.GetDataRow(rowHandle);
                    inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    if (inbDocView.CallerRef == null || inbDocView.CallerRef == "")
                    {
                        XtraMessageBox.Show("Please select a valid Caller Reference to un-map.");
                    }
                    else
                    {
                        //Israel 7/16/15 -- This structure seems unncessary--we are creating an arraylist to submit one item at a time.
                        //However, in the interest of not needlessly breaking the code due to some unforeseen circumstances, 
                        //we have elected to support the original structure.
                        //mapCallerReferenceRequest[] requestArray = new mapCallerReferenceRequest[1];
                        //mapCallerReferenceRequest request = new mapCallerReferenceRequest();

                        List<InboundDocCallerRefDto> callerRefList = new List<InboundDocCallerRefDto>();
                        InboundDocCallerRefDto callerRefData = new InboundDocCallerRefDto();
                        
                        //request.callerRef = inbDocView.CallerRef;
                        //requestArray[0] = request;
                        callerRefData.CallerRef = inbDocView.CallerRef;
                        callerRefList.Add(callerRefData);

                        //SubmitCallerReferenceRequest(requestArray, true);
                        SubmitCallerReferenceRequest(callerRefList, true);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the database with the Unmap Caller Reference data." + Environment.NewLine +
                       "Error CNF-505 in " + FORM_NAME + ".UnMapCallerReference(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void barBtnIgnore_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IgnoreDocument();
        }

        private void barBtnBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BookmarkDocument();
        }

        private void barBtnUserCmts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                XtraMessageBox.Show("Cannot perform this operation on multiple documents.");
                return;
            }
            CreateUserComment();
        }

        private void barBtnDocCmt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                XtraMessageBox.Show("Cannot perform this operation on multiple documents.");
                return;
            }
            CreateDocumentComment();
        }

        private void barBtnDiscard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string message = "Discard currently selected Inbound Document(s)?";
            string caption = "Discard Document(s)";
            if (viewProperties == ViewProperties.DiscardedView)
            {
                message = "Un-Discard currently selected Inbound Document(s)?";
                caption = "Un-Discard Document(s)";
            }

            DialogResult result = XtraMessageBox.Show(message, caption,
   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                DiscardDocument();
            }
        }

        private void gridViewInboundDocs_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            bool indicatorIcon = true;

            GridView view = (GridView)sender;
            try
            {
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle > 0)
                {
                    string bookMark = view.GetRowCellValue(e.RowHandle, "BookmarkFlag").ToString();
                    string commentMark = view.GetRowCellValue(e.RowHandle, "CommentFlag").ToString();
                    string displayText = "";

                    if ((bookMark == null) || (bookMark == ""))
                    {
                        displayText = "";
                    }
                    else displayText = "B";

                    if ((commentMark == null) || (commentMark.Trim() == ""))
                    {
                    }
                    else
                    {
                        if (displayText != "")
                            displayText = "B,C";
                        else displayText = "C";
                    }

                    e.Info.DisplayText = displayText;
                    if (!indicatorIcon)
                        e.Info.ImageIndex = -1;
                }
            }
            catch (Exception err)
            {
                //Israel 12/14/2015 - appears out of sequence because CNF-506 was used elsewhere.
                XtraMessageBox.Show("An error occurred while setting grid with the bookmark and comment flags." + Environment.NewLine +
                       "Error CNF-507 in " + FORM_NAME + ".gridViewInboundDocs_CustomDrawRowIndicator(): " + err.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintDocument();
        }

        private void barBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                XtraMessageBox.Show("Cannot perform this operation on multiple documents.");
                return;
            }
            CopyDocument();
        }


        private void mapCallerReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                XtraMessageBox.Show("Cannot perform this operation on multiple documents.");
                return;
            }
            MapCallerReference();
        }

        private void unMapCallerReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                XtraMessageBox.Show("Cannot perform this operation on multiple documents.");
                return;
            }
            if (MessageBox.Show("UnMap current Caller Reference?", "UnMap Caller Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                UnMapCallerReference();
            }
        }

        private void barBtnMatchDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AssociateDocument(false);
        }

        private void ValidateRqmt()
        {
            bool validRqmt = false;
            if (InboundPnl.ActiveTradeRqmt.Rqmt == AssociatedDoc.SEMPRA_PAPER)
            {
                foreach (string statusCode in AssociatedDoc.sempraPaperCodes)
                {
                    if (InboundPnl.ActiveTradeRqmt.Status.Equals(statusCode))
                    {
                        throw new Exception("Unable to perform this Document Match due to Rqmt status code state." + Environment.NewLine +
                             "Error CNF-508 in " + FORM_NAME + ".ValidateRqmt().");
                    }
                }
            }
            // we are doing away with the paper check.  requested by London Users.
            validRqmt = true;
            //validRqmt = (
            //    (InboundPnl.ActiveTradeRqmt.Rqmt == AssociatedDoc.BROKER_PAPER) ||
            //    (InboundPnl.ActiveTradeRqmt.Rqmt == AssociatedDoc.CPTY_PAPER) ||
            //    (InboundPnl.ActiveTradeRqmt.Rqmt == AssociatedDoc.SEMPRA_PAPER)
            //);

            if (! validRqmt) 
            {
                throw new Exception("Rqmt to associate is NOT a valid paper requirement." + Environment.NewLine +
                        "Error CNF-509 in " + FORM_NAME + ".ValidateRqmt().");
            }
        }
        
        public void AssociateDocument(bool xmitDocument)
        {
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    var sumData = InboundPnl.ActiveSummaryData;
                    var inbDocView = GetInboundDocsViewForFocusedRow();
                    ValidateAssociation(inbDocView);

                    var assDoc = CreateAssociatedDoc(inbDocView, InboundPnl.ActiveTradeRqmt, InboundPnl.ActiveSummaryData);
                    var assocDocsData = DtoViewConverter.CreateAssociatedDocsDto(assDoc);
                    var rqmtStatus = DetermineRqmtStatusForAssociation(assocDocsData);
                    if (rqmtStatus == null)
                    {
                        LoadImageInEditor(inbDocView);
                        return;
                    }

                    assocDocsData.RqmtStatus = rqmtStatus;
                    
                    assDoc.SecondValidateReqFlag = assDoc.SecondValidateReqFlag ?? "N";                    
                    assocDocsData.SecValidateReqFlag = assDoc.SecondValidateReqFlag;                
                    assocDocsData.CptySn = assDoc.CptyShortName;
                    assocDocsData.BrokerSn = assDoc.BrokerShortName;
                    assocDocsData.CdtyGroupCode = assDoc.CdtyGroupCode;
                    assocDocsData.DocStatusCode = InboundPnl.DetermineDocStatusCode(assocDocsData);
                    assDoc.DocStatusCode = assocDocsData.DocStatusCode;
                                                           
                    SubmitAssociatedDocumentStatusRequest(assocDocsData);
                    assDoc.Id = assocDocsData.Id;
                    var imagesDto = imagesDal.GetByDocId(inbDocView.Id, ImagesDtoType.Inbound);
                    imagesDal.SwitchImagesDtoType(imagesDto, assocDocsData.Id);
                    InboundPnl.ApplyAssoicatedDocChangeToView(assDoc);

                    var canXmitdocument = assocDocsData.DocStatusCode == AssociatedDoc.APPROVED;
                    if (xmitDocument && canXmitdocument)
                    {
                        TransmitDocument(sumData, assDoc);
                    }                    
                    
                    CloseOutDocument();                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error CNF-510: Error associating document:" + ex.Message, ex);
                XtraMessageBox.Show("An error occurred while associating the document." + Environment.NewLine +
                       "Error CNF-510 in " + FORM_NAME + ".AssociateDocument(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private string DetermineRqmtStatusForAssociation(AssociatedDocsDto assocDocsData)
        {            
            var validRqmtStatuses = new HashSet<string>
            {
                AssociatedDoc.RS_APPROVED,
                AssociatedDoc.RS_DISPUTED,
                AssociatedDoc.RS_RECEIVED
            };

            while(true)
            {
                var rqmtStatus = SubmitEditRqmtDelegate.Invoke(
                    assocDocsData.TradeId,
                    assocDocsData.TradeRqmtId,
                    assocDocsData.DocTypeCode,
                    assocDocsData.DocStatusCode,
                    DateTime.Now, "", "", true, false);

                if (rqmtStatus.Equals("CANCELLED"))
                {
                    return null;
                }

                if (validRqmtStatuses.Contains(rqmtStatus))
                {
                    return rqmtStatus;
                }

                XtraMessageBox.Show(
                    String.Format(
                        "For associating a document the trade requirement status must be one of the following:\n{0}",
                        string.Join(",", validRqmtStatuses)
                        )
                    );
            }             
        }


        private AssociatedDoc CreateAssociatedDoc(InboundDocsView inbDocView, RqmtData activeTradeRqmt,
            SummaryData activeSummaryData)
        {
            var assDoc = new AssociatedDoc();
            assDoc.AssociatedBy = p_UserId;
            assDoc.InboundDocsId = inbDocView.Id;
            assDoc.MultipleAssociatedDocs = false;
            assDoc.SecondValidateReqFlag = activeTradeRqmt.SecondCheckFlag ?? "N";
            assDoc.TradeId = activeTradeRqmt.TradeId;
            assDoc.TradeRqmtId = activeTradeRqmt.Id;
            assDoc.TradeId = activeTradeRqmt.TradeId;

            //Israel 12/02/2015 -- Changed rqmt code to readable display
            //assDoc.DocTypeCode = activeTradeRqmt.Rqmt;
            assDoc.DocTypeCode = activeTradeRqmt.DisplayText;

            // Trade Summary Data..
            assDoc.CptyShortName = activeSummaryData.CptySn;
            assDoc.BrokerShortName = activeSummaryData.BrokerSn;
            assDoc.CdtyGroupCode = activeSummaryData.CdtyGrpCode;
            assDoc.SetDocStatus();
            SetDocIndexValue(ref assDoc);
            assDoc.FileName = inbDocView.FileName;
            return assDoc;
        }

        private InboundDocsView GetInboundDocsViewForFocusedRow()
        {
            var dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
            var inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
            return inbDocView;
        }

        private void ValidateAssociation(InboundDocsView inbDoc)
        {            
            var currentSelected = ImagesEventManager.Instance.CurrentSelected;
            if (currentSelected == null)
            {
                throw new Exception("No document is currently selected." + Environment.NewLine +
                        "Error CNF-538 in " + FORM_NAME + ".ValidateAssociation().");
            }

            if (currentSelected.Type != ImagesDtoType.Inbound &&
                currentSelected.DocsId != inbDoc.Id)
            {
                throw new Exception("The display document does not match current document selected.  Please reload selected document." + Environment.NewLine +
                        "Error CNF-539 in " + FORM_NAME + ".ValidateAssociation().");
            }
            
            if (gridViewInboundDocs.GetSelectedRows().Length > 1)
            {
                throw new Exception("Cannot perform this operation on multiple documents." + Environment.NewLine +
                        "Error CNF-540 in " + FORM_NAME + ".ValidateAssociation().");
            }

            if ((!gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle)) || inbDoc == null)
            {
                throw new Exception("No Inbound Document is currently selected." + Environment.NewLine +
                        "Error CNF-541 in " + FORM_NAME + ".ValidateAssociation().");
            }
            ValidateRqmt();
            ValidateInboundDocument(inbDoc);
        }

        private void ValidateInboundDocument(InboundDocsView inbDoc)
        {
            ImagesDto byDocId = imagesDal.GetByDocId(inbDoc.Id, ImagesDtoType.Inbound);
            if (byDocId == null || byDocId.OriginalImage == null || byDocId.MarkupImage == null)
            {
                throw new Exception(string.Format(
                    "Cannot Find selected Inbound Document for Id: {0}, FileName: {1}", inbDoc.Id, inbDoc.FileName) + Environment.NewLine +
                        "Error CNF-511 in " + FORM_NAME + ".ValidateInboundDocument().");
            }
        }
        
        private void SubmitAssociatedDocumentStatusRequest(AssociatedDocsDto pAssocDocsData)
        {
            AssociatedDocsDal assocDocsDal = new AssociatedDocsDal(sqlConnectionString);
            int associatedDocId = assocDocsDal.UpdateStatus(pAssocDocsData);            

            if (associatedDocId <= 0)
            {
                throw new Exception("An Id was not returned from creating the associated document." + Environment.NewLine +
                        "Error CNF-512 in " + FORM_NAME + ".SubmitAssociatedDocumentStatusRequest().");
            }
            pAssocDocsData.Id = associatedDocId;            
        }

        private void SetDocIndexValue(ref AssociatedDoc assDoc)
        {            
            AssociatedDocsDal assocDocsDal = new AssociatedDocsDal(sqlConnectionString);
            int inboundDocsId = Convert.ToInt32(assDoc.InboundDocsId);
            int indexVal = assocDocsDal.GetCurrentIndexValue(inboundDocsId);

            if (indexVal >= 0)
            {
                assDoc.IndexVal = indexVal + 1;
            }
            else
            {
                throw new Exception("An error occurred while setting the document index value." + Environment.NewLine +
                        "Error CNF-513 in " + FORM_NAME + ".SetDocIndexValue().");
            }            
        }

        public void LocateAndDisplayTradeRqmtDocument()
        {
            throw new Exception("LocateAndDisplayTradeRqmtDocument has not been implemented." + Environment.NewLine +
                    "Error CNF-514 in " + FORM_NAME + ".LocateAndDisplayTradeRqmtDocument().");
        }

        public void DisplayInboundDocument()
        {
            using (new CursorBlock(Cursors.WaitCursor))
            {
                try
                {
                    InboundDocsView doc = null;
                    if (gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle))
                    {
                        var dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
                        if (InboundPnl.DsManager == null)
                            throw new Exception("The DsManager internal object was not instantiated." + Environment.NewLine +
                                "Error CNF-515 in " + FORM_NAME + ".DisplayInboundDocument().");
                        if (dr != null)
                        {
                            doc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                        }
                    }
                    LoadImageInEditor(doc);         
                }
                catch (Exception err)
                {
                    Logger.Error("Error CNF-516: Error on DisplayInboundDocument:" + err.Message, err);
                    XtraMessageBox.Show("An error occurred while displaying the inbound document." + Environment.NewLine +
                           "Error CNF-516 in " + FORM_NAME + ".DisplayInboundDocument(): " + err.Message,
                         FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }    

        public void BeginDataGridUpdates()
        {
            isCacheUpdating = true;
            DataRow dr = null;
            focusedInbDocBeforeUpdates = null;
            gridViewInboundDocs.BeginDataUpdate();
            if (gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle) && (gridViewInboundDocs.FocusedRowHandle != DevExpress.XtraGrid.GridControl.AutoFilterRowHandle))
            {
                dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
                if (dr != null)
                {
                    focusedInbDocBeforeUpdates = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                }
            }
            this.gridViewInboundDocs.FocusedRowChanged -= this.gridViewInboundDocs_FocusedRowChanged;
        }

        public void EndGridDataUpdates()
        {
            try
            {
                gridViewInboundDocs.EndDataUpdate();               

                if (focusedInbDocBeforeUpdates != null)
                {
                    try
                    {
                        DataRow[] found = InboundDocsViewTable.Select("Id = " + focusedInbDocBeforeUpdates.Id);
                        if (found.Length > 0)
                        {
                            GridColumn col = gridViewInboundDocs.Columns["Id"];
                            int rowHandle = gridViewInboundDocs.LocateByValue(0, col, focusedInbDocBeforeUpdates.Id);
                            if ((rowHandle >= 0))
                            {
                                gridViewInboundDocs.ClearSelection();
                                gridViewInboundDocs.SelectRow(rowHandle);
                                gridViewInboundDocs.FocusedRowHandle = rowHandle;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogAndDisplayException("An error occurred while processing updates for Id: " +
                                            focusedInbDocBeforeUpdates.Id + "." + Environment.NewLine +
                               "Error CNF-517 in " + FORM_NAME + ".EndGridDataUpdates().", ex);
                    }
                }

                focusedInbDocBeforeUpdates = null;
                gridViewInboundDocs.FocusedRowChanged += gridViewInboundDocs_FocusedRowChanged;
            }
            finally
            {
                isCacheUpdating = false;
            }
        }       

        public string GetActiveDocumentFileName()
        {
            InboundDocsView inbDoc = null;
            DataRow dr = null;
            string fileName = "";

            try
            {
                if (gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle))
                {
                    dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
                    inbDoc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    fileName = inbDoc.FileName;
                }
                return fileName;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving the document file name." + Environment.NewLine +
                       "Error CNF-518 in " + FORM_NAME + ".GetActiveDocumentFileName(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return fileName;
            }
        }

        internal void LoadGridSettings(string p)
        {        
            string gridSettingFileName = Path.Combine(InboundPnl.appSettingsDir, p + ".xml");         
            if (File.Exists(gridSettingFileName))
            {
                gridViewInboundDocs.RestoreLayoutFromXml(gridSettingFileName);
            }
        }

        internal void SaveGridSettings(string p)
        {
            string gridSettingFileName = Path.Combine(InboundPnl.appSettingsDir, p + ".xml");            
            gridViewInboundDocs.SaveLayoutToXml(gridSettingFileName);
        }

        private void barBtnRedirectFax_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dtFaxNums = null;
            try
            {
                dtFaxNums = getFaxNumbers();

                RedirectDocument redirect = new RedirectDocument(dtFaxNums);
                redirect.ShowDialog();

                if (redirect.NewFaxNum != "")
                {
                    UpdateSentToFaxNo(redirect.NewFaxNum);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error CNF-519: Exception in Redirect fax:" + ex.Message, ex);
                XtraMessageBox.Show("An error occurred while retrieving the transmission." + Environment.NewLine +
                       "Error CNF-519 in " + FORM_NAME + ".barBtnRedirectFax_ItemClick(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSentToFaxNo(string newSentToVal)
        {
            DataRow dr = null;
            //inboundUpdateRequest[] requestArray = new inboundUpdateRequest[gridViewInboundDocs.GetSelectedRows().Length];
            //int counter = 0;
            List<InboundDocsDto> inboundDocsDataList = new List<InboundDocsDto>();
            InboundDocsView inbDocView = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int rowHandle in gridViewInboundDocs.GetSelectedRows())
                {
                    //inboundUpdateRequest request = new inboundUpdateRequest();
                    InboundDocsDto inboundDocsData = new InboundDocsDto();
                    dr = gridViewInboundDocs.GetDataRow(rowHandle);
                    inbDocView = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);

                    //request.CallerRef = inbDocView.CallerRef;
                    //request.Cmt = inbDocView.Cmt;
                    //request.DocStatusCode = inbDocView.DocStatusCode;
                    //request.FileName = inbDocView.FileName;
                    //request.Id = inbDocView.Id;
                    //request.Sender = inbDocView.Sender;
                    //request.SentTo = newSentToVal;
                    //requestArray[counter] = request;
                    //counter++;

                    inboundDocsData.CallerRef = inbDocView.CallerRef;
                    inboundDocsData.Cmt = inbDocView.Cmt;
                    inboundDocsData.DocStatusCode = inbDocView.DocStatusCode;
                    inboundDocsData.FileName = inbDocView.FileName;
                    inboundDocsData.Id = inbDocView.Id;
                    inboundDocsData.Sender = inbDocView.Sender;
                    inboundDocsData.SentTo = newSentToVal;
                    inboundDocsDataList.Add(inboundDocsData);
                }
                //SubmitUpdateInboundDocRequest(requestArray);
                SubmitUpdateInboundDocRequest(inboundDocsDataList);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating Sent To Transmission Address for: " + newSentToVal + "." + Environment.NewLine +
                       "Error CNF-520 in " + FORM_NAME + ".UpdateSentToFaxNo(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private DataTable getFaxNumbers()
        {
            DataTable dt = null;
            try
            {
                //getInboundFaxNumbers request = new getInboundFaxNumbers();
                //getInboundFaxNumbersResponse res = new getInboundFaxNumbersResponse();
                //res = inboundWebServices.getInboundFaxNumbers(request);

                InboundFaxNosDal inboundFaxNoDal = new InboundFaxNosDal(sqlConnectionString);
                List<InboundFaxNosDto> faxNoList = new List<InboundFaxNosDto>();
                faxNoList = inboundFaxNoDal.GetAll();

                //if (res.@return.responseStatus.Equals("OK"))
                if (faxNoList.Count > 0)
                {
                    dt = new DataTable();
                    //string[] faxNumbers = res.@return.faxNumbers;
                    dt.Columns.Add("FAXNO");

                    //foreach (string faxNumber in faxNumbers)
                    foreach (InboundFaxNosDto data in faxNoList)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = data.Faxno;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    //throw new Exception("Error getting Inbound Fax Number list: " + res.@return.responseText);
                    throw new Exception("An error occurred while retrieving the Transmission Address list." + Environment.NewLine +
                            "Error CNF-521 in " + FORM_NAME + ".getFaxNumbers().");
                }
            }
            catch (Exception err)
            {
                XtraMessageBox.Show("An error occurred while retrieving the Transmission Address list." + Environment.NewLine +
                       "Error CNF-522 in " + FORM_NAME + ".getFaxNumbers(): " + err.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        private void gridControlInboundDocs_DoubleClick(object sender, EventArgs e)
        {
            DisplayInboundDocument();
        }

        private void gridViewInboundDocs_RowStyle(object sender, RowStyleEventArgs e)
        {
            bool hasUnResolvedChildren = false;

            GridView activeGridView = null;

            activeGridView = (GridView)gridControlInboundDocs.DefaultView;

            if (e.RowHandle >= 0)
            {
                string autoAssociatedFlag = activeGridView.GetRowCellValue(e.RowHandle, "HasAutoAsctedFlag").ToString();
                hasUnResolvedChildren = (activeGridView.GetRowCellValue(e.RowHandle, "UnresolvedCount").ToString() != "0");

                if (hasUnResolvedChildren)
                {
                    if (autoAssociatedFlag.Equals("Y"))
                    {
                        e.Appearance.BackColor = Color.AliceBlue;
                        e.Appearance.BackColor2 = Color.CadetBlue;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.YellowGreen;
                        e.Appearance.BackColor2 = Color.Yellow;
                    }
                }
            }
        }
        
        public void TransmitDocument(SummaryData sumData, AssociatedDoc assDoc)
        {            
            string pageList = null;
            string fileName = null;            

            try
            {
                if (assDoc == null)
                {
                    throw new NotSupportedException("Error CNF-523: Unable to transmit without an associated document data.");
                }

                if (sumData == null)
                {
                    throw new NotSupportedException("Error CNF-524: Unable to transmit a document without trade summary data.");
                }

                var transmitter = new DocumentTransmitter(imagesDal, vaulter, xmitRequestDal);
                transmitter.SendToGateway(assDoc, sumData);                
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while transmitting the document." + Environment.NewLine +
                       "Error CNF-525 in " + FORM_NAME + ".TransmitDocument(): ", ex);
            }            
        }

        private void gridViewInboundDocs_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (gridViewInboundDocs.FocusedRowHandle <= 0)
            {
                DisplayInboundDocument();
            }
        }

        private void gridViewInboundDocs_DataSourceChanged(object sender, EventArgs e)
        {
            DisplayInboundDocument();
        }

        private void gridViewInboundDocs_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            try
            {
                //Israel --Adds the fixed options to column customization
                if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
                {
                    DevExpress.XtraGrid.Menu.GridViewColumnMenu menu = e.Menu as DevExpress.XtraGrid.Menu.GridViewColumnMenu;
                    if (menu.Column != null)
                    {
                        menu.Items.Add(CreateCheckItem("Not Fixed", menu.Column, FixedStyle.None, imageList2.Images[0]));
                        menu.Items.Add(CreateCheckItem("Fixed Left", menu.Column, FixedStyle.Left, imageList2.Images[1]));
                        menu.Items.Add(CreateCheckItem("Fixed Right", menu.Column, FixedStyle.Right, imageList2.Images[2]));
                    }
                }

                if (e.Menu == null || e.Menu.GetType() == typeof(DevExpress.XtraGrid.Menu.GridViewMenu))
                {
                    popupMain.ShowPopup(gridControlInboundDocs.PointToScreen(e.Point));
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while displaying the popup menu." + Environment.NewLine +
                       "Error CNF-526 in " + FORM_NAME + ".gridViewInboundDocs_ShowGridMenu(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, FixedStyle style, Image image)
        {
            DXMenuCheckItem item = new DXMenuCheckItem(caption, column.Fixed == style, image, new EventHandler(OnFixedClick));
            item.Tag = new MenuInfo(column, style);
            if (caption == "Not Fixed")
                item.BeginGroup = true;
            return item;
        }
        void OnFixedClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            MenuInfo info = item.Tag as MenuInfo;
            if (info == null) return;
            info.Column.Fixed = info.Style;
        }
        class MenuInfo
        {
            public MenuInfo(GridColumn column, FixedStyle style)
            {
                this.Column = column;
                this.Style = style;
            }
            public FixedStyle Style;
            public GridColumn Column;
        }

        private void barBtnViewMatchedDocuments_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = null;
            InboundDocsView inbDoc = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle))
                {
                    dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
                    inbDoc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
                    ViewAssDocsByInbDocIdDelegate.Invoke(inbDoc.Id);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while displaying associated documents." + Environment.NewLine +
                       "Error CNF-527 in " + FORM_NAME + ".barBtnViewMatchedDocuments_ItemClick(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void barBtnApproveAndSend_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AssociateDocument(true);
        }

        public void ApplyUserUpdateAcces(bool updateAcces)
        {
            barBtnCopy.Enabled = updateAcces;            
            barBtnMatchDoc.Enabled = updateAcces;

            barBtnUtils.Enabled = updateAcces;
            barBtnUserCmts.Enabled = updateAcces;
            barBtnDocCmt.Enabled = updateAcces;
            btnGroupMain.Enabled = updateAcces;
            barBtnGrpDocs.Enabled = updateAcces;
            barBtnGrpUser.Enabled = updateAcces;
            barBtnGrpUtils.Enabled = updateAcces;
            barBtnRedirectFax.Enabled = updateAcces;
            barBtnApproveAndSend.Enabled = updateAcces;            
        }

        private void ribbonControl1_ShowCustomizationMenu(object sender, DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventArgs e)
        {
            e.CustomizationMenu.ItemLinks.Clear();
            e.ShowCustomizationMenu = false;
        }

        private InboundDocsView GetCurrentSelectedRow()
        {
            if (!gridViewInboundDocs.IsValidRowHandle(gridViewInboundDocs.FocusedRowHandle)) return null;
            
            var dr = gridViewInboundDocs.GetDataRow(gridViewInboundDocs.FocusedRowHandle);
            return CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(dr);
        }

        private List<InboundDocsView> GetSelectedInboundDocViews()
        {
            var selectedRows = gridViewInboundDocs.GetSelectedRows();
            var rowViews = selectedRows.Select(n => gridViewInboundDocs.GetRow(n) as DataRowView);

            return
                rowViews.Select(
                    rv => 
                        CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(rv.Row)
                        ).ToList();
        }

        private void PersistDocumentSplit(InboundDocsView current, DocumentSplitResult splitResult, ImagesDto originalDocDto)
        {            
            using (var ts = new TransactionScope())
            {                                
                string guidFileName = GenerateCopyFileName(current, "_split_");
                var newInboundDocDto = DtoViewConverter.CreateInboundDocsDto(current,guidFileName);
                newInboundDocDto.Id = inboundDocsDal.Insert(newInboundDocDto);
                Logger.DebugFormat("Persisted document split, new document Id = {0}", newInboundDocDto.Id);

                var splitImage = splitResult.SplitPages.GetImageBytes();
                imagesDal.Insert(
                    new ImagesDto(
                        newInboundDocDto.Id,
                        splitImage,
                        originalDocDto.MarkupImageFileExt,
                        splitImage,
                        originalDocDto.MarkupImageFileExt,
                        originalDocDto.Type)
                );

                originalDocDto.MarkupImage = splitResult.Remainder.GetImageBytes();
                imagesDal.Update(originalDocDto);
                var newInboundDocView = DtoViewConverter.CreateInboundDocsView(newInboundDocDto);
                ApplyNewInboundDocToView(newInboundDocView);
                ts.Complete();                                
            }           
        }

        private void OnSplitDocumentItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                InboundDocsView current = GetCurrentSelectedRow();
                if (current == null)
                {
                    XtraMessageBox.Show("Please select a document to split.");
                    return;
                }

                var originalDocDto = imagesDal.GetByDocId(current.Id, ImagesDtoType.Inbound);
                if (originalDocDto == null)
                {
                    XtraMessageBox.Show("Unable to find an inbound document for Id: " + current.Id);
                    return;
                }

                var markupTifImage = originalDocDto.GetMarkupTifImage();
                if (markupTifImage.TotalPages <= 1)
                {
                    XtraMessageBox.Show("Unable to split documents that are less than 2 pages.");
                    return;
                }

                PageSelection pageSelect = new PageSelection(markupTifImage);
                pageSelect.ShowDialog();
                var pages = pageSelect.Pages;
                if (pages == null || !pages.Any() || pages.Count >= markupTifImage.TotalPages)
                {
                    Logger.Debug("Nothing to split because there are either no pages selected or all pages selected.");
                    return;
                }

                DocumentSplitter splitter = new DocumentSplitter();
                StatusForm statusForm = new StatusForm();
                statusForm.Show();
                statusForm.Refresh();
                statusForm.LoadFormData("", "", markupTifImage.TotalPages);
                var splitResult = splitter.Split(markupTifImage, pages, i => { statusForm.ShowPageInfo(i); });
                if (splitResult == null) return;

                PersistDocumentSplit(current, splitResult, originalDocDto);                
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while splitting the document." + Environment.NewLine +
                       "Error CNF-528 in " + FORM_NAME + ".OnSplitDocumentItemClick().", ex);

                //Israel 12/15/2015 -- I assume the following are here by mistake, since they perform the same function as the preceding.
                //Logger.Error("Error splitting document:" + ex.Message, ex);
                //XtraMessageBox.Show("Error splitting document:" + ex.Message);
            }
        }

        private void ApplyNewInboundDocToView(InboundDocsView newInboundDoc)
        {
            var table = InboundDocsViewTable;
            try
            {
                gridViewInboundDocs.BeginDataUpdate();                
                table.BeginLoadData();
                var newRow = table.NewRow();
                table.Rows.Add(CollectionHelper.CreateDataRowFromObject<InboundDocsView>(newInboundDoc, newRow));                
            }            
            finally
            {
                FinishApplyingInboundGridViewChanges();
            }
            
        }

        private DataTable InboundDocsViewTable
        {
            get { return ((DataView) gridViewInboundDocs.DataSource).Table; }
        }

        private void OnMergeDocumentsClicked(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedViews = GetSelectedInboundDocViews();
                if (selectedViews.Count() != 2)
                {
                    XtraMessageBox.Show("Please select two inbound documents to merge.");
                    return;
                }

                var imagesDtos = selectedViews.Select(v => imagesDal.GetByDocId(v.Id, ImagesDtoType.Inbound)).ToArray();
                var originalImage = imagesDtos[0].GetMarkupTifImage();
                var appendImage = imagesDtos[1].GetMarkupTifImage();
                var mergedImage = originalImage.Merge(appendImage);

                PersistDocumentMerge(selectedViews, imagesDtos, mergedImage);                 
            }
            catch (Exception ex)
            {
                Logger.Error("Error CNF-528: attempting to merge documents:" + ex.Message, ex);
                XtraMessageBox.Show("An error occurred while merging the documents." + Environment.NewLine +
                       "Error CNF-528 in " + FORM_NAME + ".OnMergeDocumentsClicked(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void PersistDocumentMerge(List<InboundDocsView> selectedViews, ImagesDto[] imagesDto, TifImage mergedImage)
        {
            using (var ts = new TransactionScope())
            {
                var mergedBytes = mergedImage.GetImageBytes();

                imagesDto[0].MarkupImage = mergedBytes;
                imagesDal.Update(imagesDto[0]);
                var viewToDiscard = selectedViews[1];
                inboundDocsDal.UpdateStatus((int)viewToDiscard.Id, InboundDoc.DISCARDED);
                viewToDiscard.DocStatusCode = InboundDoc.DISCARDED;
                ApplyMergeResultsToView(viewToDiscard);
                ts.Complete();
            }           
        }

        private void ApplyMergeResultsToView(InboundDocsView viewToDiscard)
        {
            try
            {
                gridViewInboundDocs.BeginDataUpdate();
                InboundPnl.MasterInboundDocsTable.BeginLoadData();
                var rowFound = InboundPnl.MasterInboundDocsTable.Rows.Find(viewToDiscard.Id);
                if (rowFound != null)
                {
                    CollectionHelper.UpdateDataRowFromObject<InboundDocsView>(viewToDiscard, ref rowFound);
                }                
            }
            finally
            {
                FinishApplyingInboundGridViewChanges();
            }            
        }


    }
}
