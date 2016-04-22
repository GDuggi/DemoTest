using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using com.amphora.cayenne.entity;
using DevExpress.Data;
using DevExpress.Data.PivotGrid;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using DevExpress.XtraTab;
using NSRMCommon;
using NSRMLogging;
using org.apache.cayenne;
using System.Linq;
using com.amphora.entities;
using NSRiskManager.Constants;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.Export;
using DevExpress.Xpf.Printing	;
using DevExpress.Data.Filtering;
using com.amphora.cayenne.entity.support;
using System.Threading;
using System.Threading.Tasks;


namespace NSRiskMgrCtrls {


    public partial class TabPageWithUomPivot : XtraTabPage {

        #region delegates
        delegate bool XmlDataGenerator(XmlDocument doc);
        #endregion delegates

        #region constants
        const string DEFAULT_DOUBLE_FMT = "0.00;0.00;(0.00)";
        const string DEFAULT_DISPLAY_UNITS = "BARREL";
        const string BASE_PREFIX = "#,##0";
        const string monthNameString =
            "JanFebMarApr" +
            "MayJunJulAug" +
            "SepOctNovDec";
        #endregion

        #region events
        public event EventHandler SelectedCellChanged;
        public event EventHandler resetDistributionDatasource;

        public event UpdatePivotDef updatePivotDef;
        public delegate void UpdatePivotDef(object sender, string name,bool showProgressIcon);

        #endregion
      
        #region fields
        static readonly NumberFormatInfo nfi = new NumberFormatInfo();
        bool initialUpdate = true;
        public bool IsEqivChecked = false;
        public bool IsZeroChecked = false;
        IUomRequester uomRequester;
        static int numDecimals = 2;
        private bool firstTimeUnitsSet = false;
        private bool firstTimeDecimalsSet = false;

        private bool firstTimeTabLoading  = false;
   
        #endregion

        #region cctor/ctors
        static TabPageWithUomPivot() {
            nfi.NumberDecimalDigits = 2;
            nfi.NegativeInfinitySymbol = "N/A";
            nfi.PositiveInfinitySymbol = "N/A";
            

        }

        public TabPageWithUomPivot() : this(string.Empty,null) 
        {
          
        }

        GraphicsCache graphicsCache;

        public TabPageWithUomPivot(string aName,IUomRequester iurequester) {
            currentDisplayUnits = DEFAULT_DISPLAY_UNITS;
            InitializeComponent();
            this.Text = aName;
            this.uomRequester = iurequester;

            IsEqivChecked = (this.isEquivCheckbox.Checked == true);
            IsZeroChecked = (this.ZeroCheckbox.Checked == true);

            graphicsCache = new DevExpress.Utils.Drawing.GraphicsCache(this.pivotGrid.CreateGraphics());
           
        }
        #endregion

        #region properties
        public PivotGridControl pivotGrid { get { return this.pgcPositions; } }
        public Type pivotDataType { get; set; }

        public bool isMarkedForReload { get; set; }
        public bool isMarkedForRedraw { get; set; }

        //public string selectedPortfolioType { get; set; }
        public java.lang.Integer[] realPortfoliosList { get; set; }

        public string currentDisplayUnits { get; private set; }
        #endregion

        #region action methods
        public bool isTabAlreadyOpened = false;

        void pivotGridControl1_FieldValueDisplayText(object sender,PivotFieldDisplayTextEventArgs e) {
            DateTime dt;
            string tmp,strikeFmt;
            double dval;

            if (e.Field != null) {
                if (string.Compare(e.Field.FieldName,"tradingPeriod",true) == 0) {
                    if (e.Value == null)
                        e.DisplayText = "UGH";
                    else {
                        if (e.Value is string)
                            dt = DateTime.ParseExact(e.Value.ToString(),"MMM-yy",null).Date;
                        else
                            dt = (DateTime) e.Value;
                        e.DisplayText = dt.ToString("MMM-yy");
                    }
                } else if (string.Compare(e.Field.FieldName,"monthOfPeriod",true) == 0) {
                    if (e.Value == null) {
                        e.DisplayText = "ERROR";
                    } else {
                        e.DisplayText = monthDescriptionFromLongDate(Convert.ToInt32(e.Value));
                    }
                } else if (string.Compare(e.Field.FieldName,"dayOfPeriod",true) == 0) {
                    e.DisplayText = dayDescriptionFromLongDate(Convert.ToInt32(e.Value));
                } else if (string.Compare(e.Field.FieldName,"strike") == 0) {
                    if ((dval = Convert.ToDouble(e.Value)) == 0.0)
                        e.DisplayText = "";//-NotOption-
                    else {
                        strikeFmt = "$#,##0." + new string('0',Convert.ToInt32(DecimalsInputControl.Value));
                        e.DisplayText = Convert.ToDouble(e.Value).ToString(strikeFmt);
                    }
                } else if (string.Compare(e.Field.FieldName,"putcall",true) == 0) {
                    if (e.Value == null || string.IsNullOrEmpty(tmp = e.Value.ToString())) {
                        e.DisplayText = "";//-NotOption-
                    } else {
                        if (string.Compare(tmp,"P",true) == 0) e.DisplayText = "Put";
                        else if (string.Compare(tmp,"C",true) == 0) e.DisplayText = "Call";
                        else e.DisplayText = "";//-NeitherPNorC-
                    }
                } 
              
            }
        }


        public bool isRightRiskMode(RiskGroupAtom atom, string fieldName)
        {
            bool riskQuantityView = false;
            bool physicalQuantityView = false;
            bool discountQuantityView = false;
 
             switch (fieldName)
             {
  
                 case  "riskQty": case "asOfRiskQty":
                     riskQuantityView = true;
                     break;
 
                 case "physQty": case "asOfPhysicalQty":
                     physicalQuantityView = true;
                     break;
                 case "discountQty":
                     discountQuantityView = true;
                     break;
             }
 
             return SharedContext.isRightRiskMode(atom.posType,riskQuantityView, physicalQuantityView, discountQuantityView);
            
        }

        void getRollingGrandTotalCustomCellValue(PivotCellValueEventArgs e, PivotGridControl pgc)
        {
            
                double newVal = 0;

                //assuming all rows are sorted in ascending order (requirement for rolling totals), get the 
                //rolling total for the last non-grandtotal cell
                PivotCellEventArgs anObj = pgc.Cells.GetCellInfo(e.ColumnIndex, pgc.Cells.RowCount - 2);

                try
                {
                    newVal = double.Parse(anObj.Value.ToString(), NumberStyles.Any);
                    e.Value = newVal;
                }
                catch (Exception ex)
                {
                    e.Value = "N/A";
                }

        }


        void getGrandTotalSumCustomCellValue(PivotCellValueEventArgs e, PivotGridControl pgc)
        {
            double dval = 0;
            PivotCellEventArgs anObj;

            string fieldName = e.DataField.FieldName;
            if (fieldName == "riskQtyRollingTotal")
                fieldName = "riskQty";


            for (int i = 0; i < pgc.Cells.RowCount - 1; i++)
            {
                anObj = pgc.Cells.GetCellInfo(e.ColumnIndex, i);

                if (anObj != null && fieldName.Equals(((DevExpress.XtraPivotGrid.PivotGridFieldBase)(anObj.DataField)).FieldName))
                {

                    double newVal = 0;
                    try
                    {

                        string valueToParse = anObj.Value.ToString();

                        if (valueToParse == "N/A")
                        {
                            e.Value = "N/A";
                            break;
                        }


                        newVal = double.Parse(valueToParse, NumberStyles.Any);
                    }
                    catch (Exception ex)
                    {
                        newVal = 0;
                    }

                    if (!Double.IsNegativeInfinity(dval) && !Double.IsPositiveInfinity(dval))
                        dval += newVal;


                }
            }
            if (e.Value.ToString() != "N/A")
                e.Value = dval;

        }


        void getColumnGrandTotalSumCustomCellValue(PivotCellValueEventArgs e, PivotGridControl pgc)
        {

            e.Value = 0;
            double dval = 0;
            PivotCellEventArgs anObj;
            for (int i = 0; i < e.ColumnIndex; i++)
            {
                anObj = pgc.Cells.GetCellInfo(i, e.RowIndex);

                if (anObj.ColumnValueType == PivotGridValueType.GrandTotal)
                    continue;

                if (anObj != null && e.DataField.FieldName.Equals(((DevExpress.XtraPivotGrid.PivotGridFieldBase)(anObj.DataField)).FieldName))
                {
                    double newVal = 0;
                    try
                    {
                        string valueToParse = anObj.Value.ToString();

                        if (valueToParse == "N/A")
                            e.Value = "N/A";
                        else
                            newVal = double.Parse(valueToParse, NumberStyles.Any);
                    }
                    catch (Exception ex)
                    {
                        newVal = 0;
                    }

                    if (!Double.IsNegativeInfinity(dval) && !Double.IsPositiveInfinity(dval))
                        dval += newVal;

                }
            }

            if (e.Value.ToString() != "N/A")
                e.Value = dval;
               
        }

        private void getSimpleCustomCellValue(PivotCellValueEventArgs e,PivotGridControl pgc)
        {
            List<RiskGroup> riskGroups2;

            if ((riskGroups2 = selectedRiskGroups(pgc,
                e.CreateDrillDownDataSource())) != null)
            {
                //simply an empty cell in a pivot grid matrix
                if (riskGroups2.Count == 0)
                    e.Value = "";
                
                else
                    e.Value = findRiskGroupValue(riskGroups2, selectedUnits(pgc), e.DataField.FieldName);
                
            }
            else
                e.Value = "";
        }

        void pgcPositions_CustomCellValue(object sender,PivotCellValueEventArgs e) 
        {
            
            PivotGridControl pgc;

            if (sender is PivotGridControl)
            {

                pgc = sender as PivotGridControl;

                if (e.RowValueType == PivotGridValueType.GrandTotal)
                {

                    if (e.DataField.FieldName == "riskQtyRollingTotal" && e.SummaryType == PivotSummaryType.Custom)
                        getRollingGrandTotalCustomCellValue(e,pgc);

                    else if (e.SummaryType == PivotSummaryType.Sum)
                        getGrandTotalSumCustomCellValue(e, pgc);
                   
                }

                else if (e.ColumnValueType == PivotGridValueType.GrandTotal)
                {

                    if (e.SummaryType == PivotSummaryType.Sum)
                        getColumnGrandTotalSumCustomCellValue(e, pgc);

                }

                else
                    getSimpleCustomCellValue(e,pgc);


                e.Value = findDisplayValue(e.Value, e.DataField, pgc);
            }
          
        
        }



        public List<RiskGroup> getRollingTotalGroups(PivotDrillDownDataSource selectedCellDrillDownDataSource)
        {
            List<RiskGroup> mainDataSource = this.pgcPositions.DataSource as List<RiskGroup>;
            PivotDrillDownDataSource drilldownAllVisibleDataSource = (this.pgcPositions.CreateDrillDownDataSource());

            List<RiskGroup> allselectedRiskGroups = new List<RiskGroup>();

            DateTime thisTradingPeriod = new DateTime();

            foreach (PivotDrillDownDataRow row2 in selectedCellDrillDownDataSource)
            {
                RiskGroup selectedRiskGroup2 = mainDataSource[row2.ListSourceRowIndex] as RiskGroup;
                thisTradingPeriod = selectedRiskGroup2.tradingPeriod;
                break; //trading period should be the same for all riskGroups on given row
            }

            //determine all potential visible periods - they must be sorted
            foreach (PivotDrillDownDataRow row in drilldownAllVisibleDataSource)
            {
                RiskGroup selectedRiskGroup = mainDataSource[row.ListSourceRowIndex] as RiskGroup;

                DateTime tradingPeriod = selectedRiskGroup.tradingPeriod;

                if (tradingPeriod <= thisTradingPeriod)
                {
                    allselectedRiskGroups.Add(selectedRiskGroup);
                }

            }

            return allselectedRiskGroups;
        }

        void pgcPositions_CustomSummary(object sender,PivotGridCustomSummaryEventArgs e )
        {

            try
            {
                // this logic is applicable only to rolling totals
                if (e.FieldName != "riskQtyRollingTotal")
                    return;


                PivotGridFieldBase periodField = this.pgcPositions.Fields.GetFieldByName("pgfPeriod");

               //if period field is not visible, can't apply rolling totals
               if (periodField == null || periodField.Visible == false || periodField.Area != PivotArea.RowArea || periodField.SortOrder!=PivotSortOrder.Ascending)
               {
                   e.CustomValue = "N/A";
                  
                   return;
               }

             
                PivotDrillDownDataSource selectedCellDrillDownDataSource = e.CreateDrillDownDataSource();

                List<RiskGroup> allselectedRiskGroups = getRollingTotalGroups(selectedCellDrillDownDataSource);
             

                e.CustomValue = findRiskGroupValue(allselectedRiskGroups, selectedUnits(this.pgcPositions), "riskQty");

                var x = e.CustomValue;

                e.CustomValue = findDisplayValue(e.CustomValue, e.DataField as PivotGridField, this.pgcPositions);

                if (e.CustomValue.ToString() != "N/A")
                { 
                }
            }
            catch (Exception ex)
            {
                e.CustomValue = "N/A";
            }
        }
      
     
        void pivotGrid_CustomDrawCell(object sender,PivotCustomDrawCellEventArgs e) {
            PivotGridControl pgc;
            
            PivotDrillDownDataSource v;
            string disp;

            try
            {

                if (sender is PivotGridControl)
                {
                    pgc = sender as PivotGridControl;
                    if (!pgc.IsAsyncInProgress)
                    {
                    
                        List<RiskGroup> riskGroups = selectedRiskGroups(pgc, v = e.CreateDrillDownDataSource());

                        string tabName = this.Text;
                        string fieldName = e.DataField.FieldName;
                        string fieldKey = tabName + fieldName;

                        bool isCellDirty=false;
                        foreach (RiskGroup riskGroup in riskGroups)
                        {
                            isCellDirty = riskGroup.getTabDirtyStatus(fieldKey);
                            if (isCellDirty)
                                break;
                        }
    
                        if (isCellDirty)
                        {
                            drawCell(e,  isCellDirty);
                            isMarkedForRedraw = true;
                        }
                        else
                        {
                            drawCell(e,  false);

                        }
                           
                       isMarkedForReload = false;
                    }
                }
            }
            catch (Exception e2)
            { 
            }
        }
      

   
        void drawCell(PivotCustomDrawCellEventArgs e,  bool isDirty)
        {
            drawCell(e, CommonSkins.GetSkin(UserLookAndFeel.Default), isDirty);
        }
 

      
        void drawCell(PivotCustomDrawCellEventArgs e,  Skin currentSkin, bool isDirty)
        {
            Rectangle r,r2;
            SkinElement element;

            r = new Rectangle(
                new Point(e.Bounds.X,e.Bounds.Y),
                new Size(e.Bounds.Width - 1,e.Bounds.Height - 1));
            r2 = r;
            r2.Width -= 2;
            if (e.Focused || e.Selected) 
            {
                element = currentSkin[CommonSkins.SkinLabel];
                e.GraphicsCache.FillRectangle((e.Selected || e.Focused) ? element.Color.BackColor : element.Color.ForeColor,e.Bounds);
            } 
            else
                e.GraphicsCache.FillRectangle(e.Appearance.GetBackBrush(e.GraphicsCache),e.Bounds);
            
            if (e.Focused || e.Selected)
            {
                e.GraphicsCache.Paint.DrawFocusRectangle(e.Graphics,r2,
                    e.Appearance.GetForeColor(),
                    e.Appearance.GetBackColor());
            }

            if (isDirty )
            {
                e.GraphicsCache.DrawRectangle(e.Appearance.GetForePen(e.GraphicsCache), r);
            }
          
            e.Appearance.DrawString(e.GraphicsCache,e.Value.ToString(),r2,e.Appearance.GetStringFormat());
            e.Handled = true;
        }




        string findDisplayValue(object threadSafevalue, PivotGridField pivotGridField, PivotGridControl pivotGridControl)
        {
            double dval;
            string dispFmt = DEFAULT_DOUBLE_FMT, disp = "";


            if (pivotGridControl != null)
            {

                if (pivotGridField != null)
                {
                    dval = 0;

                    if (threadSafevalue.GetType() == typeof(string))
                    {
                        disp = Convert.ToString(threadSafevalue);
                    }


                    else if (threadSafevalue.GetType() == typeof(double))
                    {
                        Type valueType = threadSafevalue.GetType();

                        var val = threadSafevalue.ToString();


                        //if it is a zero and type of int, not double, we got invalid value - 
                        //val1 would be equal to "0". If it's a double, it's a proper value
                        //and val1 would be "0.0" and we should display it as a double
                        if (val == "0" & valueType.Name == "Int32")
                        {
                            disp = "";
                        }

                        else
                        {

                            try
                            {

                                dval = double.Parse(threadSafevalue.ToString(), NumberStyles.Any);
                                 
                            }
                            catch (Exception ex)
                            {
                                dval = double.MinValue;
                            }


                            if (dval == double.MinValue)
                            {
                                disp = "N/A";
                            }

                            else if (dval == 0.0)
                            {
                                // this hack is necessary because ValueFormat.FormatString gets messed up
                                //and starts displaying --- for zeros - it looks like somehow it reverts to
                                //default formatting. eventually, calling setDecimalFormattingForPivotFields
                                //once should be sufficient to set the formatting (if formatting never gets reset)
                                dispFmt = SharedContext.getCustomFormatForDoubles(numDecimals);
                                disp = dval.ToString(dispFmt, nfi);
                            }
                            else
                            {

                                if (pivotGridField.ValueFormat != null && !string.IsNullOrEmpty(pivotGridField.ValueFormat.FormatString))
                                    dispFmt = pivotGridField.ValueFormat.FormatString;


                                disp = dval.ToString(dispFmt, nfi);
                            }
                        }
                    }
                }
            }

            if (disp == "---")
                disp = "N/A";

            return disp;
        }
     
         

        public static PivotGridField findFieldNamed(PivotGridControl pgc,string fldName) {
            if (pgc == null)
                throw new ArgumentNullException("pgc",typeof(PivotGridControl).FullName + " is null.");
            if (string.IsNullOrEmpty(fldName))
                throw new ArgumentNullException("fldName","field-name is null!");
            foreach (PivotGridField pgf0 in pgc.Fields)
                if (string.Compare(pgf0.Name,fldName,true) == 0)
                    return pgf0;
            return null;
        }

        void pivotGridControl1_FocusedCellChanged(object sender,EventArgs e) {
            if (SelectedCellChanged != null)
                SelectedCellChanged(sender,EventArgs.Empty);
        }

        void seDecimals_EditValueChanging(object sender,ChangingEventArgs e) {
            int aVal;

            try
            {
                if (!Int32.TryParse(e.NewValue.ToString(),out aVal))
                    aVal = 0;
            } 
            catch (FormatException) {
                aVal = 0;
            }
            if (aVal < 0 || aVal > 10)
                e.Cancel = true;
        }

        public void setDecimalFormattingForPivotFields(int numDecimals)
        {
            string fmt, tmp;
          

            foreach (PivotGridField pgf in this.pgcPositions.Fields)
            {
                if (pgf.ValueFormat != null && pgf.ValueFormat.FormatType == FormatType.Numeric)
                {
                   
                        tmp = new string('0', numDecimals);
                        fmt = BASE_PREFIX + "." + tmp + " ;(" + BASE_PREFIX + "." + tmp + ");";

                    }
                    else
                    {
                        fmt = BASE_PREFIX + " ;(" + BASE_PREFIX + ");";
                    }

                    pgf.ValueFormat.FormatString = fmt;
                    pgf.CellFormat.FormatString = fmt;
                }
            
        }

       

        [SuppressMessage("CodeRush","String.Format can be used")]
        void seDecimals_EditValueChanged(object sender,EventArgs e) {
            SpinEdit spinEdit;

            if ((spinEdit = sender as SpinEdit) != null) 
            {
                if ((numDecimals = Convert.ToInt32(spinEdit.EditValue) )>=0)
                    {
                        setDecimalFormattingForPivotFields(numDecimals);

                        if (!firstTimeDecimalsSet)
                        {
                            updatePivotDef(this, this.Text,false);

                        }
                        firstTimeDecimalsSet = false;
                    }
            }
        }
        static bool enableDisableAsOfDateEditor = false;
        public static void enableDisablesOfDateEditor(bool enable)
        {
            enableDisableAsOfDateEditor = enable;
        }

        void UOMInputControl_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            LookUpEdit lue;

            if ((lue = sender as LookUpEdit) != null && lue.EditValue != null)
            {
                this.currentDisplayUnits = Convert.ToString(UOMInputControl.Properties.GetDisplayValueByKeyValue(lue.EditValue.ToString()));
                resetDatasourceRecalc(this.pivotGrid);
                if (this.pivotGrid.OptionsBehavior.UseAsyncMode)
                {
                    if (!this.pivotGrid.IsAsyncInProgress)
                        this.pivotGrid.RefreshDataAsync();
                }
                else
                    this.pivotGrid.RefreshData();

                if (!firstTimeUnitsSet)
                {
                    updatePivotDef(this, this.Text, false);
                }

                firstTimeUnitsSet = false;

            }
            else
                Debug.Print("null here");
            Util.show(MethodBase.GetCurrentMethod());
        }

        void pivotGridControl1_CellSelectionChanged(object sender,EventArgs e) {
            PivotGridControl pgc;

            if ((pgc = sender as PivotGridControl) != null)
                Util.show(MethodBase.GetCurrentMethod());
        }
        void pgcPositions_FieldValueCollapsing(object sender,PivotFieldValueCancelEventArgs e) {
            if (resetDistributionDatasource != null)
                resetDistributionDatasource(this,EventArgs.Empty);
        }
        void pgcPositions_FieldValueExpanding(object sender,PivotFieldValueCancelEventArgs e) {
            if (resetDistributionDatasource != null)
                resetDistributionDatasource(this,EventArgs.Empty);
        }


        void pgcPositions_FieldFilterChanged(object sender,
                                        PivotFieldEventArgs e)
        {
            updatePivotDef(this, this.Text,false);
        }
        
        void pgcPositions_fieldAreaChanged(object sender,
                                        PivotFieldEventArgs e)
        {
            this.asOfDateEditor.Enabled = enableDisableAsOfDateEditor;
            if (AmphoraFieldSelector.dictCheckBoxesMap != null && AmphoraFieldSelector.dictCheckBoxesMap.Count > 0)
            {
                if (AmphoraFieldSelector.dictCheckBoxesMap["As Of Risk Qty"].Checked == false && AmphoraFieldSelector.dictCheckBoxesMap["As Of Physical Qty"].Checked == false)
                {
                    this.asOfDateEditor.Enabled = false;
                    asOfDateEditor.EditValue = "";
                }
                else
                    this.asOfDateEditor.Enabled = true;
            }
            updatePivotDef(this, this.Text,false);
        }
        
        
     

        #endregion action methods


        public List<RiskGroup> MainDataSource = new List<RiskGroup>();
        public List<RiskGroup> DataSourceWithUnderlyingNoZeros = new List<RiskGroup>();
        public List<RiskGroup> DataSourceWithUnderlyingShowZeros = new List<RiskGroup>();
        public List<RiskGroup> DataSourceWithOptionsNoZeros = new List<RiskGroup>();
        public List<RiskGroup> DataSourceWithOptionsShowZeros = new List<RiskGroup>(); 

        public void clearAllDataSources()
        {
            MainDataSource.Clear();
            DataSourceWithUnderlyingNoZeros.Clear();
            DataSourceWithUnderlyingShowZeros.Clear();
            DataSourceWithOptionsNoZeros.Clear();
            DataSourceWithOptionsShowZeros.Clear();
        }




        
        List<RiskGroup> getMainDataSource()
        {
            return MainDataSource;
        }


        List<RiskGroup> getDataSourceWithOptionsShowZeros()
        {
            if (DataSourceWithOptionsShowZeros.Count > 0)
                return DataSourceWithOptionsShowZeros;

            else
            {
                foreach (RiskGroup riskGroup in MainDataSource)
                {
                    // add both option and all others other than underlying
                    if ((riskGroup.EquivalentPositions == "Option" || riskGroup.EquivalentPositions != "Underlying"))
                        DataSourceWithOptionsShowZeros.Add(riskGroup);
                }
            }

            return DataSourceWithOptionsShowZeros;
        }


        List<RiskGroup> getDataSourceWithOptionsNoZeros()
        {
            if (DataSourceWithOptionsNoZeros.Count > 0)
                return DataSourceWithOptionsNoZeros;

            else
            {
                foreach (RiskGroup riskGroup in MainDataSource)
                {
                    // add both option and all others other than underlying
                    if ( (riskGroup.EquivalentPositions == "Option" || riskGroup.EquivalentPositions != "Underlying") && (riskGroup.isZero==false ))
                        DataSourceWithOptionsNoZeros.Add(riskGroup);
                }
            }

            return DataSourceWithOptionsNoZeros;
        }


      

        List<RiskGroup> getDataSourceWithUnderlyingShowZeros()
        {
            if (DataSourceWithUnderlyingShowZeros.Count > 0)
                return DataSourceWithUnderlyingShowZeros;

            else
            {
                foreach (RiskGroup riskGroup in MainDataSource)
                {
                    // add both option and all others other than underlying
                    if ((riskGroup.EquivalentPositions == "Underlying" || riskGroup.EquivalentPositions != "Option" ) )
                        DataSourceWithUnderlyingShowZeros.Add(riskGroup);
                }
            }

            return DataSourceWithUnderlyingShowZeros;
        }


        List<RiskGroup> getDataSourceWithUnderlyingNoZeros()
        {
            if (DataSourceWithUnderlyingNoZeros.Count > 0)
                return DataSourceWithUnderlyingNoZeros;

            else
            {
                foreach (RiskGroup riskGroup in MainDataSource)
                {
                    // add both option and all others other than underlying
                    if ((riskGroup.EquivalentPositions == "Underlying" || riskGroup.EquivalentPositions != "Option") && (riskGroup.isZero==false))
                        DataSourceWithUnderlyingNoZeros.Add(riskGroup);
                }
            }

            return DataSourceWithUnderlyingNoZeros;
        }

        public void setCurrentDataSource()
        {
            TabPageWithUomPivot tabPage = this;
            List<RiskGroup> finalSource;

            if (tabPage.IsZeroChecked == false && tabPage.IsEqivChecked == false)
                finalSource = getDataSourceWithOptionsNoZeros();
            else if (tabPage.IsZeroChecked == true && tabPage.IsEqivChecked == false)
                finalSource = getDataSourceWithOptionsShowZeros();
            else if (tabPage.IsZeroChecked == false && tabPage.IsEqivChecked == true)
                finalSource = getDataSourceWithUnderlyingNoZeros();
            else if (tabPage.IsZeroChecked == true && tabPage.IsEqivChecked == true)
                finalSource = getDataSourceWithUnderlyingShowZeros();

            //we should never really get here
            else
                finalSource = getMainDataSource();

           tabPage.pivotGrid.DataSource = finalSource;   

        }

        void ceIsEquiv_CheckedChanged(object sender,EventArgs e) 
        {

            IsEqivChecked = (this.isEquivCheckbox.Checked == true);
          
            setCurrentDataSource();
            this.pivotGrid.RefreshData();

            if (!firstTimeTabLoading)
                 updatePivotDef(this, this.Text,true);
        }




        void ceIsZero_CheckedChanged(object sender, EventArgs e)  
        {

          IsZeroChecked = (this.ZeroCheckbox.Checked == true);

          setCurrentDataSource();
          this.pivotGrid.RefreshData();

          if(!firstTimeTabLoading)
          {
              
            updatePivotDef(this, this.Text,true);
              
          }
            
        }


        #region public methods
        public void setUpUOM()
        {
            if (string.IsNullOrEmpty(currentDisplayUnits))
                currentDisplayUnits = DEFAULT_DISPLAY_UNITS;

            firstTimeUnitsSet = true;
            firstTimeDecimalsSet = true;

            UOMInputControl.EditValue = UOMInputControl.Properties.GetKeyValueByDisplayValue(currentDisplayUnits);
        }

        public void setNumberRealPortfolios(int numRealPortfolios)
        {
            this.RealPortfolioCountLabel.Text = "Real Portfolio Count: " + numRealPortfolios;
        }

        public void setNumberPositions(int numPositions)
        {
            this.UpdatedPositionCountLabel.Text = "Updated Position Count: " + numPositions;
        }

        #endregion public methods

        #region internal methods
        
        internal void RefreshDataForTab() {
           
            if (!pgcPositions.IsAsyncInProgress) 
            {
                this.pgcPositions.RefreshData();

            }

        }

        #endregion internal methods

        #region static methods
        static void fixupPivotFields(Type type,PivotGridControl pgc) {
            List<PivotGridField> pivotfields;
            PivotGridField pgfNew;

            if (type == null)
                throw new ArgumentNullException("type","inspected-type is null!");
            if (type == null)
                throw new ArgumentNullException("pgc",typeof(PivotGridControl).Name + " is null!");

            if ((pivotfields = DXUtil.findPivotFields(type)) != null)
                foreach (PivotGridField pgf in pivotfields)
                    if (pgf.Appearance.Value.HAlignment != HorzAlignment.Default)
                        if ((pgfNew = pgc.Fields.GetFieldByName(DXUtil.PGF_PREFIX + pgf.Caption)) != null)
                            if (pgfNew.Appearance.Value.HAlignment != pgf.Appearance.Value.HAlignment) {
                                pgfNew.Appearance.Value.TextOptions.HAlignment = pgf.Appearance.Value.HAlignment;
                                pgfNew.Appearance.Value.Options.UseTextOptions = true;
                                
                            }
        }
        #endregion static methods

        #region overridden methods
        protected override void OnCreateControl() {
            base.OnCreateControl();
            this.UOMInputControl.Properties.DataSource = this.uomRequester.desiredUoms();
            this.UOMInputControl.Properties.DisplayMember = "uomShortName";
            this.UOMInputControl.Properties.ValueMember = "uomCode";
            this.UOMInputControl.Properties.PopulateColumns();
            this.UOMInputControl.Properties.Columns["uomCode"].Visible = false;
            this.UOMInputControl.Properties.Columns["uomShortName"].SortOrder = ColumnSortOrder.Ascending;
        }

        #endregion overridden methods

        #region private methods

        void zzz(AsyncOperationResult result) {
            if (initialUpdate) {
                initialUpdate = false;
                if (this.pivotDataType == null)
                    throw new InvalidOperationException("pivotDataType is not set!");
               fixupPivotFields(this.pivotDataType,this.pgcPositions);
            }
        }

        double findRiskGroupValue(List<RiskGroup> riskGroups,string toUnits, string fieldName) 
        {
            double summaryValue = 0;

            if (enableDisableAsOfDateEditor && (string.Compare("asOfRiskQty", fieldName, true) == 0 || string.Compare("asOfPhysicalQty", fieldName) == 0))
                summaryValue = asOfQuantityFieldsValues(fieldName);
            else
            {
               
                foreach (RiskGroup riskGroup in riskGroups)
                {
                    foreach (RiskGroupAtom atom in riskGroup.atoms)
                    {

                        if (isRightRiskMode(atom, fieldName))
                        {
                            double newValue = (double)riskGroup.calculateValueOfField(fieldName, toUnits, false, null);

                            if (!Double.IsNegativeInfinity(newValue) && !Double.IsPositiveInfinity(newValue))
                                summaryValue += newValue;
                            else
                            {
                                Console.WriteLine("infinitity found");
                            }
                        }
                    }
                }
            }
            return summaryValue;
        }

        double asOfQuantityFieldsValues(string fieldName)
        {
            double result = double.MinValue;
            try
            {
                 
                if (asOfDateEditor.EditValue == null || asOfDateEditor.EditValue == "" || (DateTime)asOfDateEditor.EditValue > DateTime.Today)
                    return double.MinValue;

                DateTime selectedAsOfDate = (DateTime)asOfDateEditor.EditValue;
                if (selectedAsOfDate != null)
                {
                    List<RiskGroup> riskGroups;
                    java.util.Date dt = new java.util.Date(selectedAsOfDate.ToString("MM/dd/yyyy"));

                    if (this.pivotGrid != null && this.pivotGrid.DataSource != null && (riskGroups = this.pivotGrid.DataSource as List<RiskGroup>) != null)
                    {
                       java.util.Map map = PositionEntityCacheImpl.Builder.positionCache().findPositionHistory(dt, dt, this.realPortfoliosList);
                       if (map.size() == 0)
                           return double.MinValue;

                       Dictionary<int, java.util.ArrayList> dict = new Dictionary<int, java.util.ArrayList>();
                        
                        var iterator = map.keySet().iterator();
                        while (iterator.hasNext())
                        {
                            var key =(java.lang.Number)iterator.next();
                            java.util.Map mapValue = (java.util.Map)(map.get(key));

                            if (mapValue != null)
                            {
                                var iterator2 = mapValue.keySet().iterator();
                                while (iterator2.hasNext())
                                {
                                    Position key2 = (Position)iterator2.next();
                                    java.util.ArrayList alist = (java.util.ArrayList)(mapValue.get(key2));
                                    if (alist != null && alist.size() > 0)
                                        dict.Add(key2.getPosNum().intValue(), alist);
                                }
                            }
                        }
                        PivotGridFieldCollection gridFields = (this.pivotGrid as PivotGridControl).Fields;
                        if (dict != null && dict.Count > 0)
                        {
                            foreach (RiskGroup riskGroup in riskGroups)
                            {
                                if (dict.ContainsKey(riskGroup.positionNumber))
                                {
                                    foreach (RiskGroupAtom atom in riskGroup.atoms)
                                    {
                                        foreach (PivotGridField gfield in gridFields)
                                        {
                                            double summaryValue = 0;
                                            if (string.Compare("asOfRiskQty", gfield.FieldName, true) == 0 || string.Compare("asOfPhysicalQty", gfield.FieldName, true) == 0)
                                            {
                                                double newValue = (double)riskGroup.calculateValueOfField(gfield.FieldName, selectedUnits(this.pivotGrid),  true, dict[riskGroup.positionNumber]);

                                                if (!Double.IsNegativeInfinity(newValue) && !Double.IsPositiveInfinity(newValue))
                                                {
                                                    summaryValue += newValue;

                                                    if (string.Compare("asOfRiskQty", gfield.FieldName, true) == 0)
                                                    {

                                                        atom.asOfRiskQty = summaryValue;
                                                        riskGroup.setAsOfRiskQty(summaryValue);
                                                        if (fieldName != null)
                                                            return result = summaryValue;

                                                    }
                                                    if (string.Compare("asOfPhysicalQty", gfield.FieldName, true) == 0)
                                                    {
                                                        atom.asOfPhyQty = summaryValue;
                                                        riskGroup.setAsOfPhysicalQty(summaryValue);
                                                        if (fieldName != null)
                                                            return result = summaryValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return result;
        }

        #endregion private methods
       
        DateTime? oldDateTime;
        void asOfDateEditor_EditValueChanged(object sender, EventArgs e)
        {
           
            if (asOfDateEditor.EditValue == null || asOfDateEditor.EditValue == "")
                oldDateTime = null;
            else
            {
                if ((DateTime)asOfDateEditor.EditValue > DateTime.Today)
                {
                    asOfDateEditor.EditValue = "";
                    MessageBox.Show("Date cannot be in the future!");
                    return;
                }

                else if (oldDateTime != (DateTime)asOfDateEditor.EditValue)
                {
                    oldDateTime = (DateTime)asOfDateEditor.EditValue;
                    
                    refreshScreenAsynchronously();
                   
                    
                }
            }
            
        }

        private void refreshScreenAsynchronously()
        {
            Task.Factory.StartNew(()=> {
                
                //Console.WriteLine("started refreshing");
                (this.pgcPositions as PivotGridControl).RefreshDataAsync(); 
                //Console.WriteLine("finished refreshing");
                });
        }
      

        #region other methods

        internal void updateFromPivotDef(WrappedWinPivotDef windowDefinition) 
        {
           
            XmlDocument doc;
            doc = new XmlDocument();
            
            try 
            {
                doc.LoadXml(windowDefinition.pivotLayout);
                applyLayoutToPivotGrid(this.pgcPositions,windowDefinition);
            } 
            catch (Exception) 
            {
                Util.show(MethodBase.GetCurrentMethod(),"pivot-xml is not valid.  This may be OK.");
            }


            firstTimeTabLoading = true;
            DecimalsInputControl.Value = windowDefinition.numberOfDecimals;
            UOMInputControl.EditValue = windowDefinition.pivotDefinition.getUomRel().getUomCode();
            isEquivCheckbox.Checked = windowDefinition.showFuturesEquiv;
            ZeroCheckbox.Checked = windowDefinition.ShowZero;
            firstTimeTabLoading = false;
        
       
        }

        void HandlePrintButtonClick(object sender, EventArgs e)
        {

            try
            {

                if (!this.pivotGrid.IsPrintingAvailable)
                {
                    MessageBox.Show("Printing is not available!");
                    return;
                }
                this.pivotGrid.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Printing Failed: " + ex.ToString());
            }
             
           
        }

        void HandleExportToExcelClick(object sender, EventArgs e)
        {
            
            try

            {
                SaveFileDialog dialog = new SaveFileDialog();

                dialog.Filter = "Excel File|*.xlsx";
                dialog.FilterIndex = 0;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ExportSettings.DefaultExportType = ExportType.WYSIWYG;
                    this.pivotGrid.ExportToXlsx(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excel Failed: " + ex.ToString());
            }
             
        }

        void HandlePrintPreviewButtonClick(object sender, EventArgs e)
        {
            try
            {
        
            if (!this.pivotGrid.IsPrintingAvailable)
            {
                MessageBox.Show("Printing is not available!");
                return;
            }

            this.pivotGrid.ShowPrintPreview();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Print Preview Failed: " + ex.ToString());
            }

           
        }
        public string localStorePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Amphora_RiskManager\\";
        void applyFilterValuesOnLayout(PivotGridControl pgc)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, List<string>> filterDictonary = new Dictionary<string, List<string>>();
                string filterKey = null;
                XmlDocument doc = new XmlDocument();
                doc.Load(localStorePath + "tempLayout.xml");
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    if ((node.Attributes["name"]).Value.Equals("Fields") && node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            if ((n.Attributes["name"]).Value.Contains("Item"))
                            {
                                foreach (XmlNode nn in n.ChildNodes)
                                {
                                    if ((nn.Attributes["name"]).Value.Equals("Name"))
                                    {
                                        filterKey = nn.InnerText;
                                    }
                                    if ((nn.Attributes["name"]).Value.Equals("FilterValues"))
                                    {
                                        foreach (XmlNode nnn in nn.ChildNodes)
                                        {
                                            if ((nnn.Attributes["name"]).Value.Equals("ValuesCore"))
                                            {
                                                if (nnn.ChildNodes != null && nnn.ChildNodes.Count > 0)
                                                {
                                                    List<String> fValues = new List<string>();
                                                    foreach (XmlNode nnnn in nnn.ChildNodes)
                                                    {
                                                        if ((nnnn.Attributes["name"]).Value.Contains("Item"))
                                                        {
                                                            fValues.Add(nnnn.InnerText);
                                                        }
                                                    }
                                                    filterDictonary.Add(filterKey, fValues);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (filterDictonary != null && filterDictonary.Count > 0)
                {
                    for (int fldcnt = 0; fldcnt < pgc.Fields.Count - 1; fldcnt++)
                    {
                        if (filterDictonary.Keys.Contains(pgc.Fields[fldcnt].Name))
                        {
                            foreach (string filterTxt in filterDictonary[pgc.Fields[fldcnt].Name])
                            {
                                pgc.Fields[fldcnt].FilterValues.Add(filterTxt);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
            }
        }
        

         void applyLayoutToPivotGrid(PivotGridControl pgc,WrappedWinPivotDef adef) {
            MemoryStream ms = new MemoryStream();
            byte[] data = Encoding.UTF8.GetBytes(adef.pivotLayout);
            try
            {
                string layoutString = adef.pivotLayout;
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(layoutString);
                xdoc.Save(localStorePath + "tempLayout.xml");
            }
            catch (Exception ee)
            {
            }
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms))
            {
                pgc.RestoreLayoutFromStream(ms, null);
                applyFilterValuesOnLayout(pgc);
            }

            var stream = readPivotLayout(this.pgcPositions);
        }

        internal void updateWindowStateDefinition(WrappedWinPivotDef pivotWindowStateDef) {
            ObjectContext ctx;
            Uom auom;
            int dval;
            string UOMValue,pivotLayoutXML;

            //always check if the value has changed before setting it, in order to avoid an extra "dirty" event
            dval = Convert.ToInt32(DecimalsInputControl.Value);
            if (dval != pivotWindowStateDef.numberOfDecimals)
                pivotWindowStateDef.numberOfDecimals = dval;

            if (isEquivCheckbox.Checked != pivotWindowStateDef.showFuturesEquiv)
                pivotWindowStateDef.showFuturesEquiv = isEquivCheckbox.Checked;

            if (ZeroCheckbox.Checked != pivotWindowStateDef.ShowZero)
                 pivotWindowStateDef.ShowZero = ZeroCheckbox.Checked;


            if (UOMInputControl.EditValue != null) 
            {
                UOMValue = UOMInputControl.EditValue.ToString();
                if (string.Compare(UOMValue,pivotWindowStateDef.pivotDefinition.getUomRel().getUomCode()) != 0) 
                {
                    ctx = pivotWindowStateDef.pivotDefinition.getObjectContext();
                    auom = SharedContext.findUom(UOMValue);
                    pivotWindowStateDef.pivotDefinition.setUomRel((Uom) ctx.localObject(auom));
                }
            } 
            else
                Util.show(MethodBase.GetCurrentMethod(),"No edit-value?");

            pivotLayoutXML = readPivotLayout(this.pgcPositions);
           if (pivotWindowStateDef.pivotLayout !=pivotLayoutXML )
                pivotWindowStateDef.pivotLayout = pivotLayoutXML;
        }

        static string readPivotLayout(PivotGridControl pgc) {
            string ret;

            using (MemoryStream ms = new MemoryStream()) {
                using (StreamReader sr = new StreamReader(ms)) {
                    pgc.SaveLayoutToStream(ms,null);
                  

                    ms.Position = 0;
                    ret = sr.ReadToEnd();
                }
            }
            return ret;
        }

        List<RiskGroup> selectedRiskGroups(PivotGridControl pgc, PivotDrillDownDataSource drillDownDataSource)
        {
            List<RiskGroup> allselectedGroups = new List<RiskGroup>();

            foreach (PivotDrillDownDataRow row in drillDownDataSource)
            {
                allselectedGroups.Add(selectedRiskGroup(pgc, row));
            }
            return allselectedGroups;
        }

        public void setAllRiskGroupsInTabDirty(bool status)
        {

            string[] riskFields = { "riskQty", "physQty", "discountQty" };

            List<RiskGroup> allRiskGroups = this.MainDataSource; //this.pivotGrid.DataSource as List<RiskGroup>;

            string tabName = this.Text;

            
            foreach (RiskGroup riskGroup in allRiskGroups )
            {

                foreach (string riskField in riskFields)
                {
                    riskGroup.setTabDirtyStatus(tabName + riskField, status);
                 
                }
            }
            

        }


        RiskGroup selectedRiskGroup(PivotGridControl pgc,PivotDrillDownDataRow row) {
            object pgcDatasource;

            List<RiskGroup> allRiskGroups;

            if (pgc != null && (pgcDatasource = pgc.DataSource) != null && pgcDatasource != null &&
                pgcDatasource is List<RiskGroup> && (allRiskGroups = pgcDatasource as List<RiskGroup>) != null &&
                row.ListSourceRowIndex < allRiskGroups.Count) 
            {
                return allRiskGroups[row.ListSourceRowIndex] as RiskGroup;
            }
            return null;
        }

        string selectedUnits(PivotGridControl pgc) {
            TabPageWithUomPivot parent;
            string ret = "BBL";
            object unitContainer;

            parent = (pgc.Parent == null ? null : pgc.Parent as TabPageWithUomPivot);
            if (parent != null) {
                unitContainer = parent.UOMInputControl.Properties.GetDataSourceRowByDisplayValue(currentDisplayUnits);
                if (unitContainer != null && unitContainer is WrappedUom)
                    ret = ((WrappedUom) unitContainer).uomCode;
            }
            return ret;
        }

        string monthDescriptionFromLongDate(int tmp) {
            int month,day,year;

            partsFromLongDateInt(tmp,out year,out month,out day);
            return monthNameString.Substring((month - 1) * 3,3) + "-" + (year > 2000 ? year % 2000 : year % 1900);
        }

        string dayDescriptionFromLongDate(int tmp) {
            int month,day,year;

            partsFromLongDateInt(tmp,out year,out month,out day);
            return day.ToString("00") + "-" + monthNameString.Substring((month - 1) * 3,3);
        }

        void partsFromLongDateInt(int tmp,out int year,out int month,out int day) {
            int pow;

            day = (tmp / 100) * 100;
            day = tmp - ((tmp / 100) * 100);
            tmp -= day;
            pow = Convert.ToInt32(Math.Pow(10,4));
            year = tmp / pow;
            month = (tmp - (year * pow)) / 100;
        }

        static void showSkinProperties(Skin currentSkin) {
            List<string> names = new List<string>();

            foreach (SkinElement e2 in currentSkin.GetElements())
                names.Add(e2.ElementName);
            if (names.Count > 1)
                names.Sort();
        }

        static void resetDatasourceRecalc(PivotGridControl pgc) {
            List<RiskGroup> blah;

            if (pgc != null && pgc.DataSource != null && (blah = pgc.DataSource as List<RiskGroup>) != null)
                foreach (var anrg in blah)
                    anrg.resetCalc();
        }
        #endregion other methods

     
    }

   
}