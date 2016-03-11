using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Xml;
using CommonUtils;
using DevExpress.XtraGrid;

namespace ConfirmManager
{
   public partial class frmTemplateList : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmTemplateList";
      private const string FORM_ERROR_CAPTION = "Template List Form Error";
      private const string TEMPLATE_LIST_GRID_SETTINGS = "TemplateListGrid.xml";
      public string settingsDir;
      public bool isDataBeenLoaded = false;
      private DataTable templateListData;
      public string selectedTemplate;
      public EventHandler RefreshFired;

      public frmTemplateList()
      {
         InitializeComponent();
      }

      private void frmTemplateList_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmTemplateList_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      private void ReadUserSettings()
      {
         try
         {
            if (System.IO.File.Exists(settingsDir + "\\" + TEMPLATE_LIST_GRID_SETTINGS))
                gridViewTemplateList.RestoreLayoutFromXml(settingsDir + "\\" + TEMPLATE_LIST_GRID_SETTINGS);
            
            //Now read user settings, ReadAppSettings() must be called first
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

            this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
            this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
            this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-308 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void WriteUserSettings()
      {
         try
         {
            gridViewTemplateList.SaveLayoutToXml(settingsDir + "\\" + TEMPLATE_LIST_GRID_SETTINGS);
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
            iniFile.WriteValue(FORM_NAME, "Top", this.Top);
            iniFile.WriteValue(FORM_NAME, "Left", this.Left);
            iniFile.WriteValue(FORM_NAME, "Width", this.Width);
            iniFile.WriteValue(FORM_NAME, "Height", this.Height);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                   "Error CNF-309 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void CreateTemplateListData()
      {
         try
         {
            templateListData = new DataTable();
            //Israel 9/30/2015
            //templateListData.Columns.Add(new DataColumn("Category", typeof(string)));
            //templateListData.Columns.Add(new DataColumn("Group", typeof(string)));
            templateListData.Columns.Add(new DataColumn("TemplateName", typeof(string)));
            templateListData.PrimaryKey = new DataColumn[] { templateListData.Columns["TemplateName"] };
            templateListData.CaseSensitive = false;
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while constructing the Template List internal data storage table." + Environment.NewLine +
                   "Error CNF-310 in " + FORM_NAME + ".CreateTemplateListData(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void LoadDataFromXml(string AXmlData,bool fromCache)
      {
          try
          {
              //StringReader strReader = new StringReader(AXmlData);
              DataSet ds = new DataSet();
              ds.ReadXml(new XmlTextReader(new StringReader(AXmlData)));
              DataTable templateListData = ds.Tables[0];

              gridTemplateList.DataSource = null;
              gridViewTemplateList.Columns.Clear();
              gridTemplateList.DataSource = templateListData;
              gridViewTemplateList.PopulateColumns();
              gridTemplateList.ForceInitialize();
              gridTemplateList.RefreshDataSource();
              gridViewTemplateList.BestFitColumns();
              gridViewTemplateList.OptionsView.ShowGroupPanel = true;
              gridViewTemplateList.OptionsView.ShowAutoFilterRow = true;
              //gridTemplateList.Refresh();
              //gridTemplateList.FocusedView.RefreshData();
              //gridTemplateList.MainView.RefreshData();
              isDataBeenLoaded = true;
              //if (fromCache)
              //    barStaticFiller.Caption = "* The data loaded from previous state";
              //else
              //    barStaticFiller.Caption = String.Empty;

          }
          catch (Exception ex)
          {
              XtraMessageBox.Show("An error occurred while populating the Template List using the xml data." + Environment.NewLine +
                     "Error CNF-311 in " + FORM_NAME + ".LoadDataFromXml(): " + ex.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }


      public void LoadData(string ATemplateList)
      {
         try
         {
            //Prepare the input string for processing:
            //  1. Strip off the " " around the category
            //  2. Get rid of the CR/LF
            //  3. Add each row into an arraylist
            ArrayList list = new ArrayList();
            string inputListRow = "";
            for (int i = 0; i < ATemplateList.Length; i++)
               switch (ATemplateList[i].ToString())
               {
                  case "\"":
                     inputListRow += "";
                     break;
                  case "\r":
                     list.Add(inputListRow);
                     inputListRow = "";
                     break;
                  case "\n":
                     inputListRow += "";
                     break;
                  default:
                     inputListRow += ATemplateList[i].ToString();
                     break;
               }

            // 7/29/14 Israel - last line was getting excluded because there was no '\r'
            list.Add(inputListRow);

            //Break out each row into 3 separate fields.
            //Add each row into the data table.
            string category = "";
            string group = "";
            string templateName = "";
            string tempStr = "";
            list.Insert(0, (string)category + ",<DEFAULT>");

            CreateTemplateListData();
            DataTable table = templateListData;
            table.Clear();
            for (int i = 0; i < list.Count; i++)
            {
               tempStr = list[i].ToString();
               category = tempStr.Substring(0, tempStr.IndexOf(","));
               //if (tempStr.IndexOf(".") > -1)
                 // group = tempStr.Substring(tempStr.IndexOf(",") + 1,
                   //           tempStr.IndexOf(".") - tempStr.IndexOf(",") - 1);
               templateName = tempStr.Substring(tempStr.IndexOf(",") + 1,
                           tempStr.Length - tempStr.IndexOf(",") - 1);
            
               DataRow row = table.NewRow();
               row["Category"] = category;
               //row["Group"] = group;
               row["TemplateName"] = templateName;
               table.Rows.Add(row);
            }

            gridTemplateList.DataSource = templateListData;
            gridTemplateList.ForceInitialize();
            isDataBeenLoaded = true;
         }
         catch (Exception ex)
         {
             //Israel 12/09/2015 -- Skipped sequence to use the intravening number elsewhere.
             throw new Exception("An error occurred while populating the Template List internal data storage table." + Environment.NewLine +
                 "Error CNF-313 in " + FORM_NAME + ".LoadData(): " + ex.Message);
         }
      }

      public void SetGroupFilter(bool AFilterByGroup)
      {
         try
         {
            if (AFilterByGroup)
            {
               templateListData.DefaultView.RowFilter = "Group = 'REPLY'";
            }
            else
            {
               templateListData.DefaultView.RowFilter = "";
            }
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while applying/removing the Group Filter." + Environment.NewLine +
                   "Error CNF-314 in " + FORM_NAME + ".SetGroupFilter(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnSelect_Click(object sender, EventArgs e)
      {
         try
         {
            if (gridViewTemplateList.SelectedRowsCount == 0)
               throw new Exception("Please select a row.");
            else
            {
               GridView view = gridViewTemplateList;
               selectedTemplate = view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateName");
            }
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while attempting to select a row." + Environment.NewLine +
                   "Error CNF-315 in " + FORM_NAME + ".btnSelect_Click(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void gridViewTemplateList_DoubleClick(object sender, EventArgs e)
      {
         btnSelect.PerformClick();
      }

      private void btnRefresh_Click(object sender, EventArgs e)
      {
          if(RefreshFired!=null)
          {
              RefreshFired(sender, e);
          }
      }

      private void panelControl2_Paint(object sender, PaintEventArgs e)
      {

      }

   }
}