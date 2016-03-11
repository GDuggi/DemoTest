using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using DevExpress.XtraEditors;
//using ConfirmManager.InboundWebServices;
using DBAccess;
using DBAccess.SqlServer;

namespace ConfirmManager
{
   public partial class frmMapPhrase : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmMapPhrase";
      private const string FORM_ERROR_CAPTION = "Attribute Phrase Mapping Form Error";
      private string sqlConnectionString = String.Empty;
      public string settingsDir;
      public delegate void SaveAttributeMappingPhrase(int mapValId, string phrase);
      private DataTable tblPhrases = null;

      public frmMapPhrase(string pSqlConnection, string pAttribCode, DataRow pDefaultVal)
      {
         DataTable dt = null;
         InitializeComponent();
         sqlConnectionString = pSqlConnection;
         AllowDrop = true;
         dt = GetInbAttribMapVals(pAttribCode);
         InitForm(dt, pDefaultVal);
      }

      private void ReadUserSettings()
      {
         try
         {
            //Now read user settings, ReadAppSettings() must be called first
             Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

             this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
             this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-297 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void WriteUserSettings()
      {
         try
         {
             Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
             iniFile.WriteValue(FORM_NAME, "Top", this.Top);
             iniFile.WriteValue(FORM_NAME, "Left", this.Left);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                   "Error CNF-298 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmMapPhrase_Load(object sender, EventArgs e)
      {
         this.ReadUserSettings();
      }

      private void frmMapPhrase_FormClosing(object sender, FormClosingEventArgs e)
      {
         this.WriteUserSettings();
      }

      public void InitForm(DataTable mappingValTbl, DataRow defaultVal)
      {
         try
         {
            mappingValTbl.DefaultView.Sort = "Mapping Value";
            lkupMappedValue.Properties.DataSource = mappingValTbl;
            lkupMappedValue.Properties.DisplayMember = "Mapping Value";
            lkupMappedValue.Properties.ForceInitialize();

            if (defaultVal != null)
            {
                lkupMappedValue.Text = defaultVal["Mapping Value"].ToString();
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                   "Error CNF-299 in " + FORM_NAME + ".InitForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void ResetForm()
      {
         try
         {
            tedPhrase.Text = "";
            lkupMappedValue.ItemIndex = -1;
            lkupMappedValue.EditValue = "";
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while resetting data entry field values for the form." + Environment.NewLine +
                   "Error CNF-300 in " + FORM_NAME + ".ResetForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmMapPhrase_Activated(object sender, EventArgs e)
      {
         lkupMappedValue.Focus();
      }

       private void frmMapPhrase_DragOver(object sender, DragEventArgs e)
       {
           if (e.Data.GetDataPresent(typeof(string)))
               e.Effect = DragDropEffects.Move;
           else e.Effect = DragDropEffects.None;
       }

       private void frmMapPhrase_DragDrop(object sender, DragEventArgs e)
       {
           if (!e.Data.GetDataPresent(typeof(string))) return;
           tedPhrase.Text = (string)e.Data.GetData(typeof(string));
       }

       private void btnMapPhraseCancel_Click(object sender, EventArgs e)
       {
           this.Close();
           this.Dispose();
       }

       private void btnMapPhraseSave_Click(object sender, EventArgs e)
       {
           if (lkupMappedValue.Text.Equals("")) return;
           DataRow row = ((System.Data.DataRowView)(lkupMappedValue.EditValue)).Row;
           string phrase = tedPhrase.Text.Trim();

           InbAttribMapPhraseDto mapPhraseData = new InbAttribMapPhraseDto();
           mapPhraseData.ActiveFlag = "Y";
           mapPhraseData.InbAttribMapValId = Convert.ToInt32(row["ID"].ToString());
           mapPhraseData.Phrase = phrase;
           mapPhraseData.Id = 0;

           if (row == null || phrase.Trim().Equals("")) return;
           UpdateMappedPhrase(mapPhraseData);
       }

       private void UpdateMappedPhrase(InbAttribMapPhraseDto pData)
       {
           bool isUpdate = pData.Id > 0;
           DataRow row = null;
           try
           {
               InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionString);
               int methodResult = 0;
               if (isUpdate)
                   methodResult = inbAttribMapPhraseDal.Update(pData);
               else if (pData.ActiveFlag.Equals("N"))
                   methodResult = inbAttribMapPhraseDal.Delete(pData.Id);
               else
                   methodResult = inbAttribMapPhraseDal.Insert(pData);

               if (methodResult > 0)
               {
                   // update grid view with new row.
                   if (tblPhrases == null)
                   {
                       InitPhraseTable();
                   }
                   if (isUpdate)
                   {
                       row = tblPhrases.Rows.Find(pData.Id);
                       if (row != null)
                       {
                           tblPhrases.Rows[tblPhrases.Rows.IndexOf(row)].Delete();
                       }
                   }
                   else
                   {
                       row = tblPhrases.NewRow();
                       row["ID"] = methodResult;
                       row["Mapped Attribute ID"] = pData.InbAttribMapValId;
                       row["Phrase"] = pData.Phrase;
                       row["Active Flag"] = pData.ActiveFlag;
                       tblPhrases.Rows.Add(row);
                   }
               }
               else
               {
                   throw new Exception("No Attribute changes were made to the database.");
               }
           }
           catch (Exception err)
           {
               XtraMessageBox.Show("An error occurred while attempting to perform the requested update." + Environment.NewLine +
                      "Error CNF-301 in " + FORM_NAME + ".UpdateMappedPhrase(): " + err.Message,
                    FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }

       private DataTable GetInbAttribMapVals(string pAttribCode)
       {
           DataTable dt = null;
           try
           {
               List<InbAttribMapValDto> inbAttribMapValList = new List<InbAttribMapValDto>();
               InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionString);
               inbAttribMapValList = inbAttribMapValDal.GetMapValues(pAttribCode);

               if (inbAttribMapValList.Count > 0)
               {
                    dt = new DataTable();
                    dt.Columns.Add("Mapping Value");
                    dt.Columns.Add("Descr");
                    dt.Columns.Add("ID");

                    foreach(InbAttribMapValDto data in inbAttribMapValList)
                       {
                        DataRow dr = dt.NewRow();
                        dr[0] = data.MappedValue;
                        dr[1] = data.Descr;
                        dr[2] = data.Id;
                        dt.Rows.Add(dr);
                       }
               }
               else
               {
                   throw new Exception("Error CNF-302: The mapped value update did not occur for Attribute Code: " + pAttribCode + ".");
               }
           }
           catch (Exception err)
           {
               XtraMessageBox.Show("An error occurred while attempting to perform the requested update." + Environment.NewLine +
                     "Error CNF-535 in " + FORM_NAME + ".GetInbAttribMapVals(): " + err.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           return dt;
       }

       private void lkupMappedValue_TextChanged(object sender, EventArgs e)
       {
           try
           {
               if (lkupMappedValue.Text.Equals("")) return;
               DataRow row = ((System.Data.DataRowView)(lkupMappedValue.EditValue)).Row;
               if (row != null)
               {
                   string mappedValue = row["CptySn"].ToString().Trim();
                   List<InbAttribMapComboDto> inbAttribMapComboList = new List<InbAttribMapComboDto>();
                   InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionString);
                   inbAttribMapComboList = inbAttribMapPhraseDal.GetPhrases(mappedValue);

                   if (inbAttribMapComboList.Count == 0)
                       return;
                   if (inbAttribMapComboList.Count > 0)
                   {
                       if (tblPhrases == null)
                       {
                           InitPhraseTable();
                       }
                       else
                       {
                           tblPhrases.Clear();
                       }

                       foreach (InbAttribMapComboDto data in inbAttribMapComboList)
                       {
                           DataRow dr = tblPhrases.NewRow();
                           dr[0] = data.PhraseId;
                           dr[1] = data.MappedValId;
                           dr[2] = data.Phrase;
                           dr[3] = "Y";

                           tblPhrases.Rows.Add(dr);
                       }
                   }
                   else
                   {
                       throw new Exception("Error CNF-536: No attribute mapped phrases were found for Mapped Value: " + mappedValue + ".");
                   }
               }
           }
           catch (Exception err)
           {
               XtraMessageBox.Show("An error occurred while attempting to retrieve the requested data." + Environment.NewLine +
                     "Error CNF-303 in " + FORM_NAME + ".lkupMappedValue_TextChanged(): " + err.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }

       private void InitPhraseTable()
       {
           tblPhrases = new DataTable();
           tblPhrases.Columns.Add("ID");
           tblPhrases.Columns.Add("Mapped Attribute ID");
           tblPhrases.Columns.Add("Phrase");
           tblPhrases.Columns.Add("Active Flag");
           tblPhrases.PrimaryKey = new DataColumn[] { tblPhrases.Columns["ID"] };

           gridControlMappedPhrases.DataSource = tblPhrases;
           gridControlMappedPhrases.ForceInitialize();
       }

       private void barDeleteMapping_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
       {
           DataRow dr = null;
           try
           {
               if (gridViewMappedPhrases.IsValidRowHandle(gridViewMappedPhrases.FocusedRowHandle))
               {
                   dr = gridViewMappedPhrases.GetDataRow(gridViewMappedPhrases.FocusedRowHandle);

                   InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionString);
                   Int32 id = Convert.ToInt32(dr["ID"].ToString());
                   inbAttribMapPhraseDal.Delete(id);

               }
           }
           catch (Exception ex)
           {
               XtraMessageBox.Show("An error occurred while attempting to delete the requested mapping." + Environment.NewLine +
                     "Error CNF-304 in " + FORM_NAME + ".barDeleteMapping_ItemClick(): " + ex.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }

       private void gridView1_ShowGridMenu(object sender, DevExpress.XtraGrid.Views.Grid.GridMenuEventArgs e)
       {
           popupMappedPhrases.ShowPopup(gridControlMappedPhrases.PointToScreen(e.Point));
       }
   }
}