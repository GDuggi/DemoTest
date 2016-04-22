using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using com.amphora.cayenne.entity;
using com.amphora.cayenne.entity.support;
using com.amphora.cayenne.main;
using com.amphora.model;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NSRMCommon;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.cayenne.exp;
using org.apache.cayenne.query;
using org.apache.log4j;
using RabbitMQ.Client;
using JLIST = java.util.List;
using JMAP = java.util.Map;
using System.Configuration;
namespace NSRiskManager {

    public partial class PortfolioEditor : DevExpress.XtraEditors.XtraForm {
        #region constants
        const string FRAME_KEY = "PortEditorFrame";
        #endregion

        #region fields
        Portfolio portfolio;
        PortfolioEditAction action;
        CannotCloseReason closeError;
        int parentPortNumber;
        string linkIndicator;
        java.util.Set portfolioTags;
        DXErrorProvider errProvider;
        static Dictionary<string,WrappedPortTagDef> cachedPortTagDefs = null;
        string storedSelectedPortfolioPath;
        #endregion fields

        #region ctors

        public PortfolioEditor() : this(PortfolioEditAction.NONE,null,Int32.MinValue,null, "N", null) { }

        internal PortfolioEditor(PortfolioEditAction pea, Portfolio aport, int parentPort, string selPath, string linkIndicator, java.util.Set portfolioTags)
        {
            InitializeComponent();
            portfolio = aport;
            if (aport != null)
                if (aport.getNumHistoryDays() == null)
                    aport.setNumHistoryDays(new java.lang.Integer(0));
            action = pea;
            parentPortNumber = parentPort;
            this.selectedPortfolioPath = selPath;
            storedSelectedPortfolioPath = selPath;
            this.linkIndicator = linkIndicator;
            this.portfolioTags = portfolioTags;
        }
        #endregion

        #region properties
        #region data-binding properties
        public string portFullName { get { return this.portfolio == null ? string.Empty : this.portfolio.getPortFullName(); } set { if (portfolio != null) portfolio.setPortFullName(value); } }
        public string portShortName { get { return this.portfolio == null ? string.Empty : this.portfolio.getPortShortName(); } set { if (portfolio != null) portfolio.setPortShortName(value); } }
        public int daysOfHistory { get { return this.portfolio == null ? 0 : this.portfolio.getNumHistoryDays().intValue(); } set { if (this.portfolio != null) this.portfolio.setNumHistoryDays(new java.lang.Integer(Convert.ToInt32(value))); } }
        public string portfolioType { get { return this.portfolio == null ? string.Empty : this.portfolio.getPortType(); } set { if (this.portfolio != null) this.portfolio.setPortType(value); } }
        public string baseCurrency { get { return this.portfolio == null ? string.Empty : this.portfolio.getDesiredPlCurrCode(); } set { if (this.portfolio != null) this.portfolio.setDesiredPlCurrCode(value); } }
        #endregion
        public string selectedPortfolioPath { get; private set; }
        #endregion

        #region methods
        #region overridden methods
        protected override void OnClosing(CancelEventArgs e) {
            if (this.DialogResult != DialogResult.Cancel) {
                if (closeError != CannotCloseReason.NONE) {
                    e.Cancel = true;
                    MessageBox.Show("Cannot close window: " + closeError + ".");

                }
            }
            base.OnClosing(e);
            if (!e.Cancel)
                if (this.WindowState == FormWindowState.Normal)
                    WindowPositionHelper.saveFrame(Application.UserAppDataRegistry,FRAME_KEY,this);

        }
        #endregion

        #region action methods
        void PortfolioEditor_Load(object sender,EventArgs e) {
            List<PortfolioType> ds;

            if (!WindowPositionHelper.extractFrame(Application.UserAppDataRegistry,FRAME_KEY,this))
                Debug.Print("extract-frame failed");
            try {
                errProvider = new DXErrorProvider(this);
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }

           
            ds = new List<PortfolioType>(new PortfolioType[] {
                new PortfolioType("IW","World"),
                new PortfolioType("IP","Profit Center"),
                new PortfolioType("ID","Trading Desk"),
                new PortfolioType("IL","Office Location"),
                new PortfolioType("IX","Trading Desk/Location"),
                new PortfolioType("IT","Trader"),
                new PortfolioType("R","Real"),
            });
            this.luePortfolioTypes.Properties.DataSource = ds;
            this.luePortfolioTypes.Properties.DisplayMember = "description";
            this.luePortfolioTypes.Properties.ValueMember = "key";
            this.luePortfolioTypes.Properties.PopulateColumns();
            this.luePortfolioTypes.Properties.Columns["description"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

            this.lueBaseCurrency.Properties.DataSource = SharedContext.readCurrencies();
            this.lueBaseCurrency.Properties.DisplayMember = "shortName";
            this.lueBaseCurrency.Properties.ValueMember = "code";
            this.lueBaseCurrency.Properties.PopulateColumns();
            this.lueBaseCurrency.Properties.Columns["code"].Visible = false;
            this.lueBaseCurrency.Properties.Columns["shortName"].Visible = false;
            this.lueBaseCurrency.Properties.Columns["fullName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

            this.luePortfolioTypes.Enabled = this.action == PortfolioEditAction.NEW;
            this.tePath.Enabled = false;
            gcPortClasses.Enabled = false;
            gcOtherSysAliases.Enabled = false;
            switch (this.action) {
                case PortfolioEditAction.NEW: gcPortfolio.Text = "New portfolio"; break;
                case PortfolioEditAction.NEW_AS_COPY_OF: gcPortfolio.Text = "Duplicating portfolio"; break;
                case PortfolioEditAction.UPDATE: gcPortfolio.Text = "Update portfolio"; break;
                default: gcPortfolio.Text = "unhandled '" + this.action + "'."; break;
            }
            Util.show(MethodBase.GetCurrentMethod(),
                "\r\n\t\t" +
                "SEL-PATH=" + this.selectedPortfolioPath + "\r\n\t\t" +
                "Action=" + this.action + "\r\n\t\t" +
                "Parent=" + this.parentPortNumber);
         

            try {
                teFullName.DataBindings.Add("Text",this,"portFullName");
                teShortName.DataBindings.Add("Text",this,"portShortName");
                teHistoryDays.DataBindings.Add("Text",this,"daysOfHistory");
                luePortfolioTypes.DataBindings.Add("EditValue",this,"portfolioType");
                lueBaseCurrency.DataBindings.Add("Text",this,"baseCurrency");
                this.tePath.DataBindings.Add("Text",this,"selectedPortfolioPath");

                var isPortLocked = this.portfolio.getPortLocked();
                ceLockPort.Checked = (isPortLocked.shortValue() == (short) 1) ? true : false;
              
                setupGridView(this.gvPortTags);


                List<WrappedPortTag> dataSource; ;

                if (this.portfolio == null || portfolioTags == null)
                {
                   dataSource= createTagDatasource(null);
                }
                else
                {
                 
                  dataSource=  createTagDatasource( this.portfolioTags);
                }

                gcPortTags.DataSource = dataSource;
                gcPortTags.RefreshDataSource();
                 
            } 
            catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

     

        static void setupGridView(GridView gvPortTags) {
            GridColumn gc1,gc2;
            RepositoryItemGridLookUpEdit lue1;

            if (gvPortTags.Columns != null) {
                while (gvPortTags.Columns.Count > 0)
                    gvPortTags.Columns.RemoveAt(0);

                lue1 = new RepositoryItemGridLookUpEdit();
                lue1.Name = "lueTagDescriptionSetup";
                lue1.DataSource = SharedContext.readPortTagDefs();
                lue1.ValueMember = "tagName";
                lue1.DisplayMember = "tagDescription";
                gvPortTags.GridControl.RepositoryItems.Add(lue1);

                gc1 = gvPortTags.Columns.AddVisible("tagName");
                gc1.Name = "colTagName";
                gc1.ColumnEdit = lue1;
                gc1.OptionsColumn.AllowEdit = false;
                gc1.OptionsColumn.AllowGroup = DefaultBoolean.False;
                gc1.OptionsColumn.AllowFocus = false;
                gc1.OptionsColumn.AllowSort = DefaultBoolean.True;
                gc1.SortMode = ColumnSortMode.Custom;
                gc1.SortOrder = ColumnSortOrder.Ascending;


                gc2 = gvPortTags.Columns.AddVisible("Unbound","Tag Value");
                gc2.Name = "colTagValue";
                gc2.UnboundType = UnboundColumnType.String;
                gc2.OptionsColumn.AllowSort = DefaultBoolean.False;
                gvPortTags.CustomUnboundColumnData += gv_CustomUnboundColumnData;

                gvPortTags.OptionsView.ShowGroupPanel = false;
            }


        }

        static void gv_CustomUnboundColumnData(object sender,CustomColumnDataEventArgs e) {
            GridView gv = sender as GridView;
            WrappedPortTag wpt;

            if ((wpt = e.Row as WrappedPortTag) != null) {
                if (e.IsGetData) {
                    e.Value = wpt.tagValue;
                } else if (e.IsSetData) {
                    wpt.tagValue = e.Value.ToString();
                }
            } else
                Debug.Print("*** row is " + e.Row.GetType().FullName);
        }


        private PortfolioDTO populateDTO() {
            PortfolioDTO pdto;
            pdto = new PortfolioDTO();
       
         
            pdto.setCmntNum(portfolio.getCmntNum());
            pdto.setDesiredPlCurrCode(portfolio.getDesiredPlCurrCode());
            pdto.setLinkInd(this.linkIndicator); 
            pdto.setNumHistoryDays(portfolio.getNumHistoryDays());
            pdto.setOwnerInit(portfolio.getOwnerInit());
            pdto.setParentPortId(new java.lang.Integer(parentPortNumber));
            
           
            //pdto.setPortClass(portfolio.getPortClass());
            // for now, always send the portfolio class as "P"
            pdto.setPortClass("P");

            pdto.setPortFullName(portfolio.getPortFullName());


            pdto.setPortNum(portfolio.getPortNum());
            pdto.setPortRefKey(portfolio.getPortRefKey());
            pdto.setPortShortName(portfolio.getPortShortName());
            pdto.setPortType(portfolio.getPortType());
            pdto.setTradingEntityNum(portfolio.getTradingEntityNum());
            pdto.setTransId(portfolio.getTransId());

              
            PortfolioDTO.PortfolioTags dtoTags = new PortfolioDTO.PortfolioTags();

            GridView view = gcPortTags.DefaultView as GridView; ;
            for (int i = 0 ;i < view.RowCount ;i++) {
                GridColumn descColumn = view.Columns[0];
                GridColumn valueColumn = view.Columns[1];


                string tagName = view.GetRowCellValue(i,descColumn) as string;
                string tagValue = view.GetRowCellValue(i,valueColumn) as string;

                if (tagValue != null)
                {
                    PortfolioDTO.PortfolioTagDTO dtoTag = new PortfolioDTO.PortfolioTagDTO();
                    dtoTag.tagName = tagName;
                    dtoTag.tagValue = tagValue;
                    dtoTag.transId = pdto.getTransId();
                    dtoTag.portNum = pdto.getPortNum();

                    dtoTags.tags.add(dtoTag);
                }
                  
            }


            short isLocked = this.ceLockPort.Checked == true ? (short) 1 : (short) 0;
            pdto.setPortLocked(new java.lang.Short(isLocked));

            pdto.setPortfolioTags(dtoTags);

            return pdto;

            
        }

        private void triggerAdditionalGridValidation(CancelEventArgs e)
        { 
            //when tags grid is empty, validation is not triggered on close. therefore, we need to trigger it ourselves.

            GridView gridView = this.gvPortTags;

            gridView.ClearColumnErrors();
            this.validateAdditionalFieldsGrid(e , gridView);

            
        }


        void btnSave_Click(object sender,EventArgs e) {

            closeError = CannotCloseReason.NONE;

            PortfolioDTO portfolioDTO;

            var portfolioSupport = PortfolioEntityDTOSupportImpl.Builder.portfolioSupport();

            CancelEventArgs cancelArgs = new CancelEventArgs();

            triggerAdditionalGridValidation(cancelArgs);

            if (cancelArgs.Cancel == true)
            {
              
                closeError = CannotCloseReason.ValidationFailed;
                return;
            }

            switch (action) {
                case PortfolioEditAction.UPDATE:

                    portfolioDTO = populateDTO();

                    portfolioSupport.sendUpdate(portfolioDTO);

                    break;
                case PortfolioEditAction.NEW:
                case PortfolioEditAction.NEW_AS_COPY_OF:

                   
                    portfolioDTO = populateDTO();

                    portfolioSupport.sendCreate(portfolioDTO);

                    break;
                default:
                    closeError = CannotCloseReason.UnhandledAction;
                    break;
            }

        }

        List<PortfolioTag> extractPortfolioTags(List<WrappedPortTag> list) {
            List<PortfolioTag> ret = new List<PortfolioTag>();

            foreach (WrappedPortTag wpt in list)
                ret.Add(new PortfolioTag(wpt.tagName,string.IsNullOrEmpty(wpt.tagValue) ? string.Empty : wpt.tagValue));
            return ret;
        }
        #endregion

        #region utility methods

        /*
        List<WrappedPortTag> createTagDatasource(JLIST list) {
            if (list != null && (list.size()) > 0)
                return createTagDatasourceFromList(list);


            else
                return createTagDatasourceFromScratch();

        }*/


        List<WrappedPortTag> createTagDatasource(java.util.Set set)
        {
            if (set != null && (set.size()) > 0)
                return createTagDatasourceFromSet(set);

            else
                return createTagDatasourceFromScratch();

        }



        List<WrappedPortTag> createTagDatasourceFromSet(java.util.Set set)
        {
            Dictionary<string, WrappedPortTag> existingTagDictionary = new Dictionary<string, WrappedPortTag>();

            List<WrappedPortTag> finalTagList = new List<WrappedPortTag>();


            foreach(PortfolioDTO.PortfolioTagDTO tag in set.toArray())
            { 
                PortfolioTag portTag = new PortfolioTag(tag.tagName, tag.tagValue);
                WrappedPortTag wrappedTag = new WrappedPortTag(portTag);
                wrappedTag.portNum = tag.portNum.intValue();
                wrappedTag.transId = tag.transId.intValue();

                existingTagDictionary.Add(tag.tagName, wrappedTag);

            }

            Dictionary<string, WrappedPortTagDef> allCachedTagsDef = getCachedWrappedPortTags();

            foreach (var tagDefitionKey in allCachedTagsDef.Keys)
            {
                // merge existing tags with the full list from the database.
                // those added from the database should have "empty" values
                if (existingTagDictionary.ContainsKey(tagDefitionKey))
                {
                    finalTagList.Add(existingTagDictionary[tagDefitionKey]);
                }
                else
                {

                    var tagFromDictionary = allCachedTagsDef[tagDefitionKey];
                    var portfolio = new PortfolioTag();
                    portfolio.setTagName(tagFromDictionary.tagName);
                    WrappedPortTag tagToAdd = new WrappedPortTag(portfolio);

                    finalTagList.Add(tagToAdd);

                }
            }

            return finalTagList;
        }

        /*

        List<WrappedPortTag> createTagDatasourceFromList(JLIST list) {
            Dictionary<string,WrappedPortTag> existingTagDictionary = new Dictionary<string,WrappedPortTag>();
            List<WrappedPortTag> finalTagList = new List<WrappedPortTag>();
            for (int i = 0 ;i < list.size() ;i++) {
                WrappedPortTag tag = new WrappedPortTag(list.get(i) as PortfolioTag);
                existingTagDictionary.Add(tag.tagName,tag);

            }

            Dictionary<string,WrappedPortTagDef> allCachedTagsDef = getCachedWrappedPortTags();


            foreach (var tagDefitionKey in allCachedTagsDef.Keys) {
                // merge existing tags with the full list from the database.
                // those added from the database should have "empty" values
                if (existingTagDictionary.ContainsKey(tagDefitionKey)) {
                    finalTagList.Add(existingTagDictionary[tagDefitionKey]);
                } else {

                    var tagFromDictionary = allCachedTagsDef[tagDefitionKey];
                    var portfolio = new PortfolioTag();
                    portfolio.setTagName(tagFromDictionary.tagName);
                    WrappedPortTag tagToAdd = new WrappedPortTag(portfolio);

                    finalTagList.Add(tagToAdd);

                }
            }

            return finalTagList;
        }
         */

        List<WrappedPortTag> createTagDatasourceFromScratch() {
            List<WrappedPortTag> ret = new List<WrappedPortTag>();
            PortfolioTag pt;
            WrappedPortTag wpt;

            foreach (var avar2 in SharedContext.readPortTagDefs()) {
                pt = new PortfolioTag();
                pt.setTagName(avar2.tagName);
                ret.Add(wpt = new WrappedPortTag(pt));

                var avar3 = SharedContext.addPortTagDef(avar2);
                if (avar2 != null) {
                    if (avar3 is List<WrappedAccount>) {
                        wpt.tagValue = ((List<WrappedAccount>) avar3)[0].acctNum.ToString();
                    } else {
                        if (avar3 is List<WrappedEntityTagOption>) {
                            Debug.Print("found one");
                            wpt.tagValue = ((List<WrappedEntityTagOption>) avar3)[0].tagOption;
                        }
                    }
                }
            }
            return ret;
        }


        #endregion

        void gvPortTags_RowCellStyle(object sender,RowCellStyleEventArgs e) {
            GridView gv;
            WrappedPortTag wpt;
            string tagName;


            if ((gv = sender as GridView) != null &&
                e.RowHandle != GridControl.InvalidRowHandle && string.Compare(e.Column.FieldName,"tagName",true) == 0) {
                if ((wpt = gv.GetRow(e.RowHandle) as WrappedPortTag) != null) {
                    tagName = wpt.tagName;

                    bool required = getRequiredForTag(tagName);

                    if (required)
                        e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font.FontFamily,e.Appearance.Font.SizeInPoints,FontStyle.Bold);
                    else
                        e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font.FontFamily,e.Appearance.Font.SizeInPoints,FontStyle.Regular);

                }
            }
        }

        #endregion

        /// <summary>Provide a row-specific set of data, allowing the user to choose from the proper data-source.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gvPortTags_CustomRowCellEdit(object sender,CustomRowCellEditEventArgs e) {
            GridView gv;
            WrappedPortTag wpt;
            RepositoryItem ri;
            string itemName;

            if ((gv = sender as GridView) != null && e.RowHandle != GridControl.InvalidRowHandle) {
                wpt = gv.GetRow(e.RowHandle) as WrappedPortTag;
                itemName = "lue_" + wpt.tagName;

                if (string.Compare(e.Column.FieldName,"tagName",true) == 0) {
                    itemName = "lueSharedItem";
                    if ((ri = findRepositoryItem(itemName,gv.GridControl)) == null)
                        ri = createLookupEditor(gv,wpt,itemName);
                    e.RepositoryItem = ri;
                } else if ((string.Compare(e.Column.Caption,"Tag Value",true) == 0 ||
                      string.Compare(e.Column.FieldName,"tagValue",true) == 0)) {
                    if ((ri = findRepositoryItem(itemName,gv.GridControl)) == null)
                        ri = generateItem(gv,wpt,itemName);

                    DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit ri2 = ri as DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit;
                    if (ri2.View.Columns.Count < 1) {
                        ri2.PopulateViewColumns();
                        setDefaultDropdownOptions(ri2.View);
                    }


                    e.RepositoryItem = ri;
                } else
                    Debug.Print("found CAPTION='" + e.Column.Caption + "' with FieldName='" + e.Column.Caption + "' FLD='" + e.Column.FieldName + "'.");
            }
        }

        RepositoryItem findRepositoryItem(string itemName,GridControl gridControl) {
            foreach (RepositoryItem ri in gridControl.RepositoryItems)
                if (string.Compare(ri.Name,itemName) == 0)
                    return ri;
            return null;
        }

        static RepositoryItem createLookupEditor(GridView gv,WrappedPortTag wpt,string itemName) {
            return createLookupEditor(gv,wpt.tagName,itemName,SharedContext.convertToErrorClass(SharedContext.readPortTagDefs()));
        }

        static RepositoryItemGridLookUpEdit createLookupEditor(GridView gv,string key,string itemName,object datasource) {
            RepositoryItemGridLookUpEdit ret;

            ret = new RepositoryItemGridLookUpEdit();
            ret.ValueMember = "keyValue";
            ret.DisplayMember = "displayValue";
            ret.Tag = ret.View.Tag = key;
            ret.Name = itemName;

            ret.NullText = string.Empty;
            ret.DataSource = datasource;

            gv.GridControl.RepositoryItems.Add(ret);
            return ret;
        }

        static RepositoryItem generateItem(GridView gv,WrappedPortTag wpt,string itemName) {
            string key = wpt.tagName;
            return createLookupEditor(gv,key,itemName,SharedContext.portfolioDataForEntityTag(key));
        }

        static void setDefaultDropdownOptions(GridView gv) {
            gv.Columns["originalClassName"].Visible = false;
            gv.Columns["keyValue"].Visible = false;
            gv.Columns["displayValue"].Visible = true;
            gv.Columns["displayValue"].SortOrder = ColumnSortOrder.Ascending;
        }

        void luePortfolioTypes_Validating(object sender,CancelEventArgs e) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void portfolioTypeChanged_handler(object sender, EventArgs e)
        {
            var portfolioDropdown = this.luePortfolioTypes;

            string porfolioSelected = portfolioDropdown.EditValue.ToString();

            //reset path to the top level
            if (porfolioSelected == "IW")
            {
                selectedPortfolioPath = "/";

            }
            else
            {
                selectedPortfolioPath = storedSelectedPortfolioPath;
            }

        }


        void teShortName_Validating(object sender,CancelEventArgs e) {
            this.errProvider.SetError(this.teShortName,string.Empty);
            if (string.IsNullOrEmpty(this.teShortName.Text)) {
                this.errProvider.SetError(this.teShortName,"Short Name is empty");
                e.Cancel = true;
            }
        }

        void teFullName_Validating(object sender,CancelEventArgs e) {
            this.errProvider.SetError(this.teFullName,string.Empty);
            if (string.IsNullOrEmpty(this.teFullName.Text)) {
                this.errProvider.SetError(this.teFullName,"Full Name is empty");
                e.Cancel = true;
            }
        }

        void lueBaseCurrency_Validating(object sender,CancelEventArgs e) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void additionalFields_CustomSort(object sender,CustomColumnSortEventArgs e) {
            GridView view = sender as GridView;

            int row1Index = e.ListSourceRowIndex1;
            int row2Index = e.ListSourceRowIndex2;

            GridColumn descColumn = view.Columns[0];

            string description1 = view.GetRowCellDisplayText(row1Index,descColumn) as string;
            string tagName1 = view.GetRowCellValue(row1Index,descColumn) as string;
            string description2 = view.GetRowCellDisplayText(row2Index,descColumn) as string;
            string tagName2 = view.GetRowCellValue(row2Index,descColumn) as string;

            var required1 = getRequiredForTag(tagName1);
            var required2 = getRequiredForTag(tagName2);

            // both required's are true or false
            if (required1 == required2) {
                e.Result = string.Compare(description1,description2);
            } else if (required1 == true) {
                e.Result = -1;
            } else if (required2 == true) {
                e.Result = 1;
            } else
                e.Result = 0;

            e.Handled = true;

        }

        void additionalFielsRow_exceptionHandler(object sender,DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e) {
            //Suppress displaying the error message box
            //    ?        e.ExceptionMode = ExceptionMode.NoAction;
            Util.show(MethodBase.GetCurrentMethod());
        }

        void gcPortTags_ValidateRow(object sender,ValidateRowEventArgs e) {
            GridView view = sender as GridView;


            int rowIndex = e.RowHandle;

            view.ClearColumnErrors();
            this.validateAdditionalFieldsRow(view,rowIndex);
        }

        void gcPortTags_ValueChanging(object sender,CellValueChangedEventArgs e) {
            //when value is changing, clear the previous error. sometimes it's too late do to it on validation
            GridView view = sender as GridView;
            view.ClearColumnErrors();
        }

        void gvPortTags_Validating(object sender,CancelEventArgs e) {

            GridControl gridContol = sender as GridControl;

            GridView additionalGridView = gridContol.DefaultView as GridView;

            additionalGridView.ClearColumnErrors();
            this.validateAdditionalFieldsGrid(e,additionalGridView);

        }

        void validateAdditionalFieldsGrid(CancelEventArgs e,GridView additionalGridView) {
            for (int i = 0 ;i < additionalGridView.RowCount ;i++) {
                bool validationStatus = this.validateAdditionalFieldsRow(additionalGridView,i);

                e.Cancel = !validationStatus;
                if (!validationStatus) {
                    e.Cancel = true;
                    return;
                }
            }

        }

        bool validateAdditionalFieldsRow(GridView view,int rowIndex) {
            var tagToValidate = view.GetRow(rowIndex) as WrappedPortTag;
            GridColumn descColumn = view.Columns[0];
            GridColumn valueColumn = view.Columns[1];

            var description = view.GetRowCellDisplayText(rowIndex,descColumn);
            var value = view.GetRowCellValue(rowIndex,valueColumn);

            var dataSource = SharedContext.portfolioDataForEntityTag(tagToValidate.tagName);

            string errorMessage = "";
            bool validationStatus = true;


            if (dataSource is List<WrappedErrorProvider>)
                validationStatus = validateEntityTagOption(description,tagToValidate,out errorMessage);

            else
                Debug.Print("Uknown tag type!");

            if (validationStatus != true) {
                //set focus on the current row

                view.FocusedRowHandle = rowIndex;
                view.SetColumnError(valueColumn,errorMessage);

            }
            return validationStatus;
        }

        static bool getRequiredForTag(string tagName) {

            var cachedPortTags = getCachedWrappedPortTags();

            var cachedPortTagDef = cachedPortTagDefs[tagName];

            return tagName == null ? false : cachedPortTagDef.required;
        }

        static void cacheWrappedPortTagDef() {
            cachedPortTagDefs = new Dictionary<string,WrappedPortTagDef>();

            var cachedPortTagDefsList = SharedContext.readPortTagDefs();

            foreach (WrappedPortTagDef wpt in cachedPortTagDefsList) {
                cachedPortTagDefs.Add(wpt.tagName,wpt);
            }
        }

        static Dictionary<string,WrappedPortTagDef> getCachedWrappedPortTags() {

            if (cachedPortTagDefs == null) {
                cacheWrappedPortTagDef();
            }

            return cachedPortTagDefs;
        }

        static bool validateEntityTagOption(string description,WrappedPortTag tagToValidate,out string errorMessage) {
            errorMessage = "";

            var required = getRequiredForTag(tagToValidate.tagName);

            if ((tagToValidate.tagValue == null) && (getRequiredForTag(tagToValidate.tagName) == true)) {
                errorMessage = "Tag " + description + " needs to have a value!";
                return false;
            }

            return true;

        }

        void gvPortTags_ValidateRow(object sender,ValidateRowEventArgs e) {
            Util.show(MethodBase.GetCurrentMethod(),"do something here.");
        }

        void PortfolioEditor_Validating(object sender,CancelEventArgs e) {
            Util.show(MethodBase.GetCurrentMethod());
            validateGridView(e,"Unbound",this.gvPortTags);
        }

        static void validateGridView(CancelEventArgs e,string columnName,GridView gvPortTags) {
            GridColumn gc;

            if (gvPortTags.UpdateCurrentRow()) {
                for (int i = 0 ;i < gvPortTags.RowCount ;i++) {
                    if ((gc = gvPortTags.Columns[columnName]) != null) {
                        var avar = gvPortTags.GetRowCellValue(i,gc);
                        if (avar == null) {
                            gvPortTags.SetColumnError(gc,"is null!");
                            e.Cancel = true;
                        } else if (string.IsNullOrEmpty(avar.ToString())) {
                            gvPortTags.SetColumnError(gc,"is null!");
                            e.Cancel = true;
                        }
                    }
                }
            } else
                throw new InvalidOperationException("row-failed");
        }

        public static Portfolio createDefaultPortfolio() {
            Portfolio ret = new Portfolio();

            ret.setPortShortName("New portfolio");
            ret.setPortFullName(ret.getPortShortName());
            ret.setNumHistoryDays(new java.lang.Integer(0));
            ret.setDesiredPlCurrCode("USD");
            ret.setPortType("R");
            ret.setPortLocked(new java.lang.Short(0));
            ret.setPortRefKey(string.Empty);
            addRequiredPortfolioTags(ret);
            return ret;
        }

        public static void addRequiredPortfolioTags(Portfolio newCopy) {
            foreach (PortfolioTag pt in readPortfolioTagDefs())
                newCopy.addPortfolioTag(pt);
        }

        static List<PortfolioTag> readPortfolioTagDefs() {
            return readPortfolioTagDefs(true);
        }

        static List<PortfolioTag> readPortfolioTagDefs(bool requiredOnly) {
            List<PortfolioTag> ret = new List<PortfolioTag>();
            PortfolioTag pt;

            foreach (var avar2 in SharedContext.readPortTagDefs()) {
                if (requiredOnly && !avar2.required) continue;
                pt = new PortfolioTag();
                pt.setTagName(avar2.tagName);
                ret.Add(pt);
            }
            return ret;
        }

        internal static List<WrappedPortTag> readWrappedPortfolioTags() {
            return readWrappedPortfolioTags(true);
        }

        internal static List<WrappedPortTag> readWrappedPortfolioTags(bool requiredOnly) {
            return wrap(readPortfolioTagDefs(requiredOnly));
        }

        static List<WrappedPortTag> wrap(List<PortfolioTag> list) {
            List<WrappedPortTag> ret = new List<WrappedPortTag>();

            if (list != null)
                foreach (PortfolioTag pt in list)
                    ret.Add(new WrappedPortTag(pt));
            return ret;
        }

     
    }

    internal class WrappedPortTag {
        #region ctor
        public WrappedPortTag(PortfolioTag pt) {
            if (pt == null)
                throw new ArgumentNullException("pt",typeof(PortfolioTag).FullName + " is null!");
            portTag = pt;
        }
        #endregion

        #region properties
        [Browsable(false)]
        public PortfolioTag portTag { get; set; }


        public int portNum { get; set; }
        public int transId { get; set; }

        [DisplayName("RealPortfolio Tag")]
        public string tagName { get { return portTag.getTagName(); } }
        [DisplayName("Tag Value")]
        public string tagValue { get { return portTag.getTagValue(); } set { portTag.setTagValue(value); } }
        #endregion

        #region methods
        public override string ToString() {
            return base.ToString() + ": " + tagName + " = " + tagValue;
        }
        #endregion
    }

    public class PortfolioType {
        #region ctor
        public PortfolioType(string akey,string adesc) {
            key = akey;
            description = adesc;
        }
        #endregion

        #region properties

        [Browsable(false)]
        public string key { get; private set; }

        [DisplayName("PortfolioType")]
        public string description { get; private set; }

        #endregion
    }

    enum CannotCloseReason {
        NONE,
        UnhandledAction,
        UpdateFailed,
        CreationFailed,
        ValidationFailed
    }



    public class ChannelUtil {
        public static ChannelUtil shared = new ChannelUtil();
        public IConnection connection;
        ConnectionFactory factory;
        ChannelContainer cc;

        ChannelUtil() {
            try {
                if (factory == null)
                {
                    string rabbitMQConnection = ConfigurationSettings.AppSettings["rabbitMQConnection"];
                   string[] connectionParams  = rabbitMQConnection.Split(':');

                    
                    factory = new ConnectionFactory
                    {
                        HostName = connectionParams[0],
                        Port = Convert.ToInt32(connectionParams[1]),
                        //HostName = "172.16.143.199",
                        //Port = 5672,
                        //UserName = "guest",
                        //Password = "guest"
                    };
                }
                if (connection == null)
                    connection = factory.CreateConnection();
                if (cc == null) {
                    cc = createContainer(connection,Guid.NewGuid().ToString());
                }
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

        public static bool isConnected { get; private set; }

        internal static ChannelContainer createContainer() {
            return createContainer(shared.connection,Guid.NewGuid().ToString());
        }
        internal static ChannelContainer createContainer(IConnection connection,string aReplyId) {
            ChannelContainer cc = new ChannelContainer(aReplyId);

            cc.setup(connection);
            return cc;
        }

        internal static void shutdown() {
            shared.shutdown2();
        }
        void shutdown2() {
            stopListening(cc);
        }
        internal void stopListening(ChannelContainer cc) {
            Util.show(MethodBase.GetCurrentMethod(),"signaling thread.");
            cc.threadLock.Set();
            cc.threadLock.WaitOne();
            Util.show(MethodBase.GetCurrentMethod(),"thread-lock signalled");
            if (this.connection != null) {
                connection.Close(0,"OK");
                connection.Dispose();
                connection = null;
            }
            if (connection != null) {
                try {
                    connection.Close(0,"OK");
                } catch (Exception ex) {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                }
                connection.Dispose();
                connection = null;
            }
            if (factory != null)
                factory = null;
        }
    }
}