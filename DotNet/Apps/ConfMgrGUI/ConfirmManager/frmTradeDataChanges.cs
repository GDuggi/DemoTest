using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DBAccess;
using System.Reflection;
using CommonUtils;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace ConfirmManager
{
   public partial class frmTradeDataChanges : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmTradeDataChanges";
      private const string FORM_ERROR_CAPTION = "Trade Data Changes Form Error";
      private string TRADE_DATA_CHANGES_GRID_SETTINGS = "TradeDataChangesGrid.xml";
      public string settingsDir;
      public string saveToExcelDirectory;
      public List<TradeDataChgDto> tradeDataChangeList;
      List<Dictionary<string, bool>> fieldChangeList;

      public frmTradeDataChanges()
      {
         InitializeComponent();
         tradeDataChangeList = new List<TradeDataChgDto>();
         fieldChangeList = new List<Dictionary<string, bool>>();
      }

            //foreach (var docInfo in response.QueryResult)
            //{
            //    string url = docInfo.URL;
            //    Type docInfoType = docInfo.GetType();
            //    PropertyInfo[] propsForDocInfo = docInfoType.GetProperties();

            //    Dictionary<string, string> dicItem = new Dictionary<string, string>();
            //    foreach (PropertyInfo prop in propsForDocInfo)
            //    {
            //        var propType = prop.PropertyType;
            //        bool getValueOk = false;
            //        if (propType.IsPrimitive || propType == typeof(Decimal) || propType == typeof(String))
            //        {
            //            string propValue = String.Empty;
            //            if (prop.GetValue(docInfo, null) != null)
            //                propValue = prop.GetValue(docInfo, null).ToString();
            //            if (VVUtils.IsNumericPropertyType(propType.Name))
            //            {
            //                propValue = prop.GetValue(docInfo, null).ToString();
            //                if (propValue != "0")
            //                    getValueOk = true;
            //            }
            //            else if (!String.IsNullOrEmpty(propValue))
            //            {
            //                propValue = prop.GetValue(docInfo, null).ToString();
            //                getValueOk = true;
            //            }
                         
            //            if (getValueOk)
            //            {
            //                string propName = prop.Name;
            //                dicItem.Add(propName, propValue);
            //            }
            //        }
            //    }
            //    dicList.Add(dicItem);
            //}

      public void PopulateRowChangesList()
      {
          fieldChangeList.Clear();
          Dictionary<string, bool> rowChangesDict;

          //Skip the first row since we are testing for changes
          if (tradeDataChangeList.Count > 1)
          {
              for (int i = 0; i < tradeDataChangeList.Count -1; i++)
              {
                  rowChangesDict = new Dictionary<string, bool>();

                  #region Better way to determine changed fields -- but has problems

                  //Type rowDataType1 = tradeDataChangeList[i].GetType();
                  //Type rowDataType2 = tradeDataChangeList[i = 1].GetType();

                  //PropertyInfo[] propsForDataRow1 = rowDataType1.GetProperties();
                  //PropertyInfo[] propsForDataRow2 = rowDataType2.GetProperties();

                  //foreach (PropertyInfo prop in propsForDataRow1)
                  //{
                  //    foreach (PropertyInfo prop2 in propsForDataRow2)
                  //    {
                  //        if (prop.Name == prop2.Name)
                  //        {
                  //            if (prop.GetValue(tradeDataChangeList[i], null) == prop2.GetValue(tradeDataChangeList[i + 1], null))
                  //                rowChangesDict.Add(prop.Name, true);
                  //        }
                  //    }
                  //}

                  #endregion

                  if (tradeDataChangeList[i].BookingCoSn != tradeDataChangeList[i + 1].BookingCoSn)
                      rowChangesDict.Add("BookingCoSn", true);
                  if (tradeDataChangeList[i].Book != tradeDataChangeList[i + 1].Book)
                      rowChangesDict.Add("Book", true);
                  if (tradeDataChangeList[i].BrokerLegalName != tradeDataChangeList[i + 1].BrokerLegalName)
                      rowChangesDict.Add("BrokerLegalName", true);
                  if (tradeDataChangeList[i].BrokerPrice != tradeDataChangeList[i + 1].BrokerPrice)
                      rowChangesDict.Add("BrokerPrice", true);
                  if (tradeDataChangeList[i].BrokerSn != tradeDataChangeList[i + 1].BrokerSn)
                      rowChangesDict.Add("BrokerSn", true);
                  if (tradeDataChangeList[i].BuySellInd != tradeDataChangeList[i + 1].BuySellInd)
                      rowChangesDict.Add("BuySellInd", true);
                  if (tradeDataChangeList[i].CdtyCode != tradeDataChangeList[i + 1].CdtyCode)
                      rowChangesDict.Add("CdtyCode", true);
                  if (tradeDataChangeList[i].CdtyGrpCode != tradeDataChangeList[i + 1].CdtyGrpCode)
                      rowChangesDict.Add("CdtyGrpCode", true);
                  if (tradeDataChangeList[i].CptyLegalName != tradeDataChangeList[i + 1].CptyLegalName)
                      rowChangesDict.Add("CptyLegalName", true);
                  if (tradeDataChangeList[i].CptySn != tradeDataChangeList[i + 1].CptySn)
                      rowChangesDict.Add("CptySn", true);
                  if (tradeDataChangeList[i].EndDt != tradeDataChangeList[i + 1].EndDt)
                      rowChangesDict.Add("EndDt", true);
                  if (tradeDataChangeList[i].InceptionDt != tradeDataChangeList[i + 1].InceptionDt)
                      rowChangesDict.Add("InceptionDt", true);
                  if (tradeDataChangeList[i].LocationSn != tradeDataChangeList[i + 1].LocationSn)
                      rowChangesDict.Add("LocationSn", true);
                  if (tradeDataChangeList[i].OptnPremPrice != tradeDataChangeList[i + 1].OptnPremPrice)
                      rowChangesDict.Add("OptnPremPrice", true);
                  if (tradeDataChangeList[i].OptnPutCallInd != tradeDataChangeList[i + 1].OptnPutCallInd)
                      rowChangesDict.Add("OptnPutCallInd", true);
                  if (tradeDataChangeList[i].OptnStrikePrice != tradeDataChangeList[i + 1].OptnStrikePrice)
                      rowChangesDict.Add("OptnStrikePrice", true);
                  if (tradeDataChangeList[i].PermissionKey != tradeDataChangeList[i + 1].PermissionKey)
                      rowChangesDict.Add("PermissionKey", true);
                  if (tradeDataChangeList[i].PriceDesc != tradeDataChangeList[i + 1].PriceDesc)
                      rowChangesDict.Add("PriceDesc", true);
                  if (tradeDataChangeList[i].ProfitCenter != tradeDataChangeList[i + 1].ProfitCenter)
                      rowChangesDict.Add("ProfitCenter", true);
                  if (tradeDataChangeList[i].QtyDesc != tradeDataChangeList[i + 1].QtyDesc)
                      rowChangesDict.Add("QtyDesc", true);
                  if (tradeDataChangeList[i].QtyTot != tradeDataChangeList[i + 1].QtyTot)
                      rowChangesDict.Add("QtyTot", true);
                  if (tradeDataChangeList[i].RefSn != tradeDataChangeList[i + 1].RefSn)
                      rowChangesDict.Add("RefSn", true);
                  if (tradeDataChangeList[i].StartDt != tradeDataChangeList[i + 1].StartDt)
                      rowChangesDict.Add("StartDt", true);
                  if (tradeDataChangeList[i].SttlType != tradeDataChangeList[i + 1].SttlType)
                      rowChangesDict.Add("SttlType", true);
                  if (tradeDataChangeList[i].Trader != tradeDataChangeList[i + 1].Trader)
                      rowChangesDict.Add("Trader", true);
                  if (tradeDataChangeList[i].TradeDesc != tradeDataChangeList[i + 1].TradeDesc)
                      rowChangesDict.Add("TradeDesc", true);
                  if (tradeDataChangeList[i].TradeDt != tradeDataChangeList[i + 1].TradeDt)
                      rowChangesDict.Add("TradeDt", true);
                  if (tradeDataChangeList[i].TradeStatCode != tradeDataChangeList[i + 1].TradeStatCode)
                      rowChangesDict.Add("TradeStatCode", true);
                  if (tradeDataChangeList[i].TradeTypeCode != tradeDataChangeList[i + 1].TradeTypeCode)
                      rowChangesDict.Add("TradeTypeCode", true);
                  if (tradeDataChangeList[i].TransportDesc != tradeDataChangeList[i + 1].TransportDesc)
                      rowChangesDict.Add("TransportDesc", true);
                  if (tradeDataChangeList[i].Xref != tradeDataChangeList[i + 1].Xref)
                      rowChangesDict.Add("Xref", true);

                  //Always add a row, even if it is empty. Of course, the 2nd row in the grid will be the first in the fieldChangeList
                  fieldChangeList.Add(rowChangesDict);
              }
          }
      }

      private void frmTradeDataChanges_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmTradeDataChanges_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      private void ReadUserSettings()
      {
         try
         {
            if (System.IO.File.Exists(settingsDir + TRADE_DATA_CHANGES_GRID_SETTINGS))
               gridViewTradeDataChanges.RestoreLayoutFromXml(settingsDir + TRADE_DATA_CHANGES_GRID_SETTINGS);

            //Now read user settings, ReadAppSettings() must be called first
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

            this.StartPosition = FormStartPosition.Manual;
            this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
            this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
            this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-316 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void WriteUserSettings()
      {
         try
         {
            gridViewTradeDataChanges.SaveLayoutToXml(Path.Combine(settingsDir, TRADE_DATA_CHANGES_GRID_SETTINGS));

            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
            iniFile.WriteValue(FORM_NAME, "Top", this.Top);
            iniFile.WriteValue(FORM_NAME, "Left", this.Left);
            iniFile.WriteValue(FORM_NAME, "Width", this.Width);
            iniFile.WriteValue(FORM_NAME, "Height", this.Height);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                   "Error CNF-317 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnClose_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void btnExcel_Click(object sender, EventArgs e)
      {
         try
         {
            saveFileDialog.Title = "Save Grid to Excel";
            saveFileDialog.InitialDirectory = saveToExcelDirectory;
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
               saveToExcelDirectory = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
               DevExpress.XtraPrinting.XlsExportOptions xlOptions =
                   new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value, true);
               gridViewTradeDataChanges.ExportToXls(saveFileDialog.FileName, xlOptions);
            }
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while exporting the grid data to an MS Excel file." + Environment.NewLine +
                   "Error CNF-318 in " + FORM_NAME + ".btnExcel_Click(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void gridViewTradeDataChanges_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
      {
          //Test for both no row selected (-1) and the first row (0). If first row, nothing to compare so don't paint cell.
          if (e.Column == null || e.RowHandle < 1) return;

          //There are always 1 less row int he fields change list because the first row is skipped.
          int changeListIdx = e.RowHandle - 1;

          //Never change the cell value color based on whether it's selected.
          Boolean isRowSelected = false;
          try
          {
              GridView view = sender as GridView;
              //isRowSelected = view.IsRowSelected(e.RowHandle);

              //For the current change list record, see if the column has a rowChangesDict entry.
              if (fieldChangeList[changeListIdx].ContainsKey(e.Column.FieldName))
                  SetColumnColor(isRowSelected, e, Color.FromName("Gold"));
          }
          catch (Exception ex)
          {
              XtraMessageBox.Show("An error occurred while applying highlight colors to the changed data fields." + Environment.NewLine +
                     "Error CNF-319 in " + FORM_NAME + ".gridViewSummary_CustomDrawCell(): " + ex.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }

      private static void SetColumnColor(Boolean isRowSelected,
         DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs eArgs, Color color)
      {
          if (eArgs.RowHandle < 0) return;
          try
          {
              if (isRowSelected)
                  eArgs.Appearance.ForeColor = color;
              else
              {
                  eArgs.Appearance.BackColor = color;
                  eArgs.Appearance.BackColor2 = color;
              }
          }
          catch (Exception ex)
          {
              XtraMessageBox.Show("An error occurred while determining changed field highlight color based on row selection state." + Environment.NewLine +
                     "Error CNF-320 in SetColumnColor(): " + ex.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }

   }
}