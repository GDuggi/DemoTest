using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;

namespace ConfirmManager
{
   public partial class frmClauseViewer : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmClauseViewer";
      private const string FORM_ERROR_CAPTION = "Clause Viewer Error";
      public string settingsDir;
      private DataTable clauseHeaderTable;
      private DataTable clauseBodyTable;
      private bool isInitComplete = false;


      public frmClauseViewer()
      {
         InitializeComponent();
      }

      private void frmClauseViewer_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmClauseViewer_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      private void ReadUserSettings()
      {
         try
         {
            //Now read user settings, ReadAppSettings() must be called first
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

            this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
            this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
            this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
            splitContainerClauses.SplitterPosition = iniFile.ReadValue(FORM_NAME, "SplitterPosition", 240);
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-212 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
            iniFile.WriteValue(FORM_NAME, "Width", this.Width);
            iniFile.WriteValue(FORM_NAME, "Height", this.Height);
            iniFile.WriteValue(FORM_NAME, "SplitterPosition", splitContainerClauses.SplitterPosition);
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                    "Error CNF-213 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void InitForm(DataTable AClauseHeader, DataTable AClauseBody)
      {
         try
         {
            if (!isInitComplete)
            {
               //this.dbtnClauses.Enabled = false;
               clauseHeaderTable = new DataTable();
               clauseHeaderTable = AClauseHeader.Copy();
               clauseBodyTable = new DataTable();
               clauseBodyTable = AClauseBody.Copy();

               InitTreeList();
               //treeListClauses.DataSource = clauseHeaderTable.DefaultView;
               isInitComplete = true;
            }
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                    "Error CNF-214 in " + FORM_NAME + ".InitForm(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            //this.dbtnClauses.Enabled = true;
         }
      }


      private void InitTreeList()
      {
         try
         {
            string categoryLabel = "";
            ArrayList categoryList = new ArrayList();
            //Load up an array list for unique, non-blank iterating
            foreach (DataRow row in clauseHeaderTable.Rows)
            {
               categoryLabel = row["Category"].ToString();
               if (categoryList.IndexOf(categoryLabel) == -1 &&
                   categoryLabel.Trim().Length > 0)
                  categoryList.Add(categoryLabel);
            }
            categoryList.Sort();
            
            treeListClauses.BeginUnboundLoad();
            TreeListNode rootNode;
            TreeListNode categoryNode;
            TreeListNode shortNameNode;
            rootNode = treeListClauses.AppendNode(new object[] { "Categories" }, null);

            string shortName = "";
            string filterStr = "";
            //For each item in array list, select all items
            for (int i = 0; i < categoryList.Count; i++)
            {
               categoryLabel = categoryList[i].ToString();
               //node = treeListClauses.AppendNode(new object[] { s, di.Name, "Folder", null }, rootNode);
               categoryNode = treeListClauses.AppendNode(new object[] { categoryLabel }, rootNode);

               filterStr = "Category = '" + categoryLabel + "'";
               foreach (DataRow row in clauseHeaderTable.Select(filterStr))
               {
                  shortName = row["ShortName"].ToString();
                  shortNameNode = treeListClauses.AppendNode(new object[] { shortName }, categoryNode);
                  shortNameNode.Tag = (int)row["PrmntConfirmClauseId"];                  
               }
            }

            treeListClauses.EndUnboundLoad();
            rootNode.Expanded = true;
            //treeListClauses.ExpandAll();
         }
         catch (Exception ex)
         {
             throw new Exception("An error occurred while setting up the category display list." + Environment.NewLine +
                 "Error CNF-215 in " + FORM_NAME + ".InitTreeList(): " + ex.Message);
         }
      }


      private string GetClauseBodyText(int APrmntConfirmClauseId)
      {
         string bodyText = "";
         try
         {
            string filterStr = "PrmntConfirmClauseId = '" + APrmntConfirmClauseId.ToString() + "'";
            foreach (DataRow row in clauseBodyTable.Select(filterStr))
            {
               bodyText += row["Body"].ToString();
            }
            return bodyText;
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while reading the clause." + Environment.NewLine +
                "The error occurred while reading Clause Id:" + APrmntConfirmClauseId.ToString() + Environment.NewLine +
                "Error CNF-216 in " + FORM_NAME + ".GetClauseBodyText(): " + ex.Message);
         }
      }

      private void treeListClauses_FocusedNodeChanged(object sender, 
         DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
      {
         try
         {
            if (e.Node.Level == 2)
            {
               int prmntConfirmClauseId = (int)e.Node.Tag;
               richTextClauses.Text = GetClauseBodyText(prmntConfirmClauseId);
            }
            else
               richTextClauses.Clear();
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while changing the current selected row." + Environment.NewLine +
                   "Error CNF-217 in " + FORM_NAME + ".treeListClauses_FocusedNodeChanged(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }


      private void btnCopyAll_Click(object sender, EventArgs e)
      {
         try
         {
            Clipboard.Clear();
            TextDataFormat tdFormat = TextDataFormat.Rtf;
            Clipboard.SetText(richTextClauses.Rtf, tdFormat);
         }
         catch (Exception ex)
         {
             XtraMessageBox.Show("An error occurred while copying all rows onto the MS Windows clipboard." + Environment.NewLine +
                    "Error CNF-218 in " + FORM_NAME + ".btnCopyAll_Click(): " + ex.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnCopy_Click(object sender, EventArgs e)
      {
         try
         {
            Clipboard.Clear();
            richTextClauses.Copy();
         }
         catch (Exception ex)
         {
             XtraMessageBox.Show("An error occurred while copying the clause onto the MS Windows clipboard." + Environment.NewLine +
                    "Error CNF-219 in " + FORM_NAME + ".btnCopy_Click(): " + ex.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }      
      
      private void btnClose_Click(object sender, EventArgs e)
      {
         this.Visible = false;
      }

   }
}