namespace ConfirmManager
{
    partial class frmCptyInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCptyInfo));
            this.tbCntrlMain = new DevExpress.XtraTab.XtraTabControl();
            this.tbCptyAddress = new DevExpress.XtraTab.XtraTabPage();
            this.lblDataFax = new DevExpress.XtraEditors.LabelControl();
            this.lblDataPhone = new DevExpress.XtraEditors.LabelControl();
            this.lblDataCountry = new DevExpress.XtraEditors.LabelControl();
            this.lblDataZipcode = new DevExpress.XtraEditors.LabelControl();
            this.lblDataState = new DevExpress.XtraEditors.LabelControl();
            this.lblDataCity = new DevExpress.XtraEditors.LabelControl();
            this.lblDataAddress3 = new DevExpress.XtraEditors.LabelControl();
            this.lblDataAddress2 = new DevExpress.XtraEditors.LabelControl();
            this.lblDataAddress1 = new DevExpress.XtraEditors.LabelControl();
            this.lblMainFax = new DevExpress.XtraEditors.LabelControl();
            this.lblMainPhone = new DevExpress.XtraEditors.LabelControl();
            this.lblCountry = new DevExpress.XtraEditors.LabelControl();
            this.lblCityStateZip = new DevExpress.XtraEditors.LabelControl();
            this.lblAddress3 = new DevExpress.XtraEditors.LabelControl();
            this.lblAddress2 = new DevExpress.XtraEditors.LabelControl();
            this.lblAddress1 = new DevExpress.XtraEditors.LabelControl();
            this.tbCptyAgreements = new DevExpress.XtraTab.XtraTabPage();
            this.gridAgreements = new DevExpress.XtraGrid.GridControl();
            this.gridViewAgreements = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColAgreementType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSeCptyShortName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDateSigned = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColTerminationDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSeAgrmntContactName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColCmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tbContractFaxNos = new DevExpress.XtraTab.XtraTabPage();
            this.gridContractFaxNos = new DevExpress.XtraGrid.GridControl();
            this.gridViewContractFaxNos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDesignationCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColCountryPhoneCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAreaCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColLocalNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColActiveFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDsgnActiveFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            this.pnlButtons = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnDetails = new DevExpress.XtraEditors.SimpleButton();
            this.lblDataCptyLn = new DevExpress.XtraEditors.LabelControl();
            this.lblDataCptySn = new DevExpress.XtraEditors.LabelControl();
            this.lblCptyLegalName = new DevExpress.XtraEditors.LabelControl();
            this.lblCptyShortName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tbCntrlMain)).BeginInit();
            this.tbCntrlMain.SuspendLayout();
            this.tbCptyAddress.SuspendLayout();
            this.tbCptyAgreements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAgreements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAgreements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.tbContractFaxNos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridContractFaxNos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContractFaxNos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbCntrlMain
            // 
            this.tbCntrlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCntrlMain.Location = new System.Drawing.Point(0, 58);
            this.tbCntrlMain.Name = "tbCntrlMain";
            this.tbCntrlMain.SelectedTabPage = this.tbCptyAddress;
            this.tbCntrlMain.Size = new System.Drawing.Size(683, 251);
            this.tbCntrlMain.TabIndex = 0;
            this.tbCntrlMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbCptyAddress,
            this.tbCptyAgreements,
            this.tbContractFaxNos});
            this.tbCntrlMain.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tbCntrlMain_SelectedPageChanged);
            // 
            // tbCptyAddress
            // 
            this.tbCptyAddress.Controls.Add(this.lblDataFax);
            this.tbCptyAddress.Controls.Add(this.lblDataPhone);
            this.tbCptyAddress.Controls.Add(this.lblDataCountry);
            this.tbCptyAddress.Controls.Add(this.lblDataZipcode);
            this.tbCptyAddress.Controls.Add(this.lblDataState);
            this.tbCptyAddress.Controls.Add(this.lblDataCity);
            this.tbCptyAddress.Controls.Add(this.lblDataAddress3);
            this.tbCptyAddress.Controls.Add(this.lblDataAddress2);
            this.tbCptyAddress.Controls.Add(this.lblDataAddress1);
            this.tbCptyAddress.Controls.Add(this.lblMainFax);
            this.tbCptyAddress.Controls.Add(this.lblMainPhone);
            this.tbCptyAddress.Controls.Add(this.lblCountry);
            this.tbCptyAddress.Controls.Add(this.lblCityStateZip);
            this.tbCptyAddress.Controls.Add(this.lblAddress3);
            this.tbCptyAddress.Controls.Add(this.lblAddress2);
            this.tbCptyAddress.Controls.Add(this.lblAddress1);
            this.tbCptyAddress.Name = "tbCptyAddress";
            this.tbCptyAddress.Size = new System.Drawing.Size(677, 223);
            this.tbCptyAddress.Text = "Address Info";
            // 
            // lblDataFax
            // 
            this.lblDataFax.Location = new System.Drawing.Point(109, 152);
            this.lblDataFax.Name = "lblDataFax";
            this.lblDataFax.Size = new System.Drawing.Size(63, 13);
            this.lblDataFax.TabIndex = 16;
            this.lblDataFax.Text = "labelControl1";
            // 
            // lblDataPhone
            // 
            this.lblDataPhone.Location = new System.Drawing.Point(109, 133);
            this.lblDataPhone.Name = "lblDataPhone";
            this.lblDataPhone.Size = new System.Drawing.Size(63, 13);
            this.lblDataPhone.TabIndex = 15;
            this.lblDataPhone.Text = "labelControl1";
            // 
            // lblDataCountry
            // 
            this.lblDataCountry.Location = new System.Drawing.Point(109, 114);
            this.lblDataCountry.Name = "lblDataCountry";
            this.lblDataCountry.Size = new System.Drawing.Size(63, 13);
            this.lblDataCountry.TabIndex = 14;
            this.lblDataCountry.Text = "labelControl1";
            // 
            // lblDataZipcode
            // 
            this.lblDataZipcode.Location = new System.Drawing.Point(334, 95);
            this.lblDataZipcode.Name = "lblDataZipcode";
            this.lblDataZipcode.Size = new System.Drawing.Size(63, 13);
            this.lblDataZipcode.TabIndex = 12;
            this.lblDataZipcode.Text = "labelControl1";
            // 
            // lblDataState
            // 
            this.lblDataState.Location = new System.Drawing.Point(247, 95);
            this.lblDataState.Name = "lblDataState";
            this.lblDataState.Size = new System.Drawing.Size(63, 13);
            this.lblDataState.TabIndex = 11;
            this.lblDataState.Text = "labelControl1";
            // 
            // lblDataCity
            // 
            this.lblDataCity.Location = new System.Drawing.Point(109, 95);
            this.lblDataCity.Name = "lblDataCity";
            this.lblDataCity.Size = new System.Drawing.Size(63, 13);
            this.lblDataCity.TabIndex = 10;
            this.lblDataCity.Text = "labelControl1";
            // 
            // lblDataAddress3
            // 
            this.lblDataAddress3.Location = new System.Drawing.Point(109, 59);
            this.lblDataAddress3.Name = "lblDataAddress3";
            this.lblDataAddress3.Size = new System.Drawing.Size(78, 13);
            this.lblDataAddress3.TabIndex = 9;
            this.lblDataAddress3.Text = "lblDataAddress3";
            // 
            // lblDataAddress2
            // 
            this.lblDataAddress2.Location = new System.Drawing.Point(109, 40);
            this.lblDataAddress2.Name = "lblDataAddress2";
            this.lblDataAddress2.Size = new System.Drawing.Size(78, 13);
            this.lblDataAddress2.TabIndex = 8;
            this.lblDataAddress2.Text = "lblDataAddress2";
            // 
            // lblDataAddress1
            // 
            this.lblDataAddress1.Location = new System.Drawing.Point(109, 21);
            this.lblDataAddress1.Name = "lblDataAddress1";
            this.lblDataAddress1.Size = new System.Drawing.Size(78, 13);
            this.lblDataAddress1.TabIndex = 7;
            this.lblDataAddress1.Text = "lblDataAddress1";
            // 
            // lblMainFax
            // 
            this.lblMainFax.Location = new System.Drawing.Point(53, 152);
            this.lblMainFax.Name = "lblMainFax";
            this.lblMainFax.Size = new System.Drawing.Size(47, 13);
            this.lblMainFax.TabIndex = 6;
            this.lblMainFax.Text = "Main Fax:";
            // 
            // lblMainPhone
            // 
            this.lblMainPhone.Location = new System.Drawing.Point(41, 133);
            this.lblMainPhone.Name = "lblMainPhone";
            this.lblMainPhone.Size = new System.Drawing.Size(59, 13);
            this.lblMainPhone.TabIndex = 5;
            this.lblMainPhone.Text = "Main Phone:";
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(57, 114);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(43, 13);
            this.lblCountry.TabIndex = 4;
            this.lblCountry.Text = "Country:";
            // 
            // lblCityStateZip
            // 
            this.lblCityStateZip.Location = new System.Drawing.Point(6, 95);
            this.lblCityStateZip.Name = "lblCityStateZip";
            this.lblCityStateZip.Size = new System.Drawing.Size(94, 13);
            this.lblCityStateZip.TabIndex = 3;
            this.lblCityStateZip.Text = "City/State/Zipcode:";
            // 
            // lblAddress3
            // 
            this.lblAddress3.Location = new System.Drawing.Point(15, 59);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(85, 13);
            this.lblAddress3.TabIndex = 2;
            this.lblAddress3.Text = "Street Address 3:";
            // 
            // lblAddress2
            // 
            this.lblAddress2.Location = new System.Drawing.Point(15, 40);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(85, 13);
            this.lblAddress2.TabIndex = 1;
            this.lblAddress2.Text = "Street Address 2:";
            // 
            // lblAddress1
            // 
            this.lblAddress1.Location = new System.Drawing.Point(15, 21);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(85, 13);
            this.lblAddress1.TabIndex = 0;
            this.lblAddress1.Text = "Street Address 1:";
            // 
            // tbCptyAgreements
            // 
            this.tbCptyAgreements.Controls.Add(this.gridAgreements);
            this.tbCptyAgreements.Name = "tbCptyAgreements";
            this.tbCptyAgreements.Size = new System.Drawing.Size(677, 223);
            this.tbCptyAgreements.Text = "Agreements";
            // 
            // gridAgreements
            // 
            this.gridAgreements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAgreements.Location = new System.Drawing.Point(0, 0);
            this.gridAgreements.MainView = this.gridViewAgreements;
            this.gridAgreements.Name = "gridAgreements";
            this.gridAgreements.Size = new System.Drawing.Size(677, 223);
            this.gridAgreements.TabIndex = 0;
            this.gridAgreements.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAgreements,
            this.gridView2});
            this.gridAgreements.DoubleClick += new System.EventHandler(this.gridAgreements_DoubleClick);
            // 
            // gridViewAgreements
            // 
            this.gridViewAgreements.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColAgreementType,
            this.gridColStatus,
            this.gridColSeCptyShortName,
            this.gridColDateSigned,
            this.gridColTerminationDt,
            this.gridColSeAgrmntContactName,
            this.gridColCmt});
            this.gridViewAgreements.GridControl = this.gridAgreements;
            this.gridViewAgreements.Name = "gridViewAgreements";
            this.gridViewAgreements.OptionsBehavior.Editable = false;
            this.gridViewAgreements.OptionsSelection.EnableAppearanceFocusedCell = false;
            // 
            // gridColAgreementType
            // 
            this.gridColAgreementType.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColAgreementType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColAgreementType.Caption = "Type";
            this.gridColAgreementType.FieldName = "AgrmntTypeCode";
            this.gridColAgreementType.Name = "gridColAgreementType";
            this.gridColAgreementType.Visible = true;
            this.gridColAgreementType.VisibleIndex = 0;
            // 
            // gridColStatus
            // 
            this.gridColStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColStatus.Caption = "Status";
            this.gridColStatus.FieldName = "StatusInd";
            this.gridColStatus.Name = "gridColStatus";
            this.gridColStatus.Visible = true;
            this.gridColStatus.VisibleIndex = 1;
            // 
            // gridColSeCptyShortName
            // 
            this.gridColSeCptyShortName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColSeCptyShortName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColSeCptyShortName.Caption = "Our Company";
            this.gridColSeCptyShortName.FieldName = "SeCptyShortName";
            this.gridColSeCptyShortName.Name = "gridColSeCptyShortName";
            this.gridColSeCptyShortName.Visible = true;
            this.gridColSeCptyShortName.VisibleIndex = 2;
            // 
            // gridColDateSigned
            // 
            this.gridColDateSigned.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColDateSigned.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColDateSigned.Caption = "Date Executed";
            this.gridColDateSigned.FieldName = "DateSigned";
            this.gridColDateSigned.Name = "gridColDateSigned";
            this.gridColDateSigned.Visible = true;
            this.gridColDateSigned.VisibleIndex = 3;
            // 
            // gridColTerminationDt
            // 
            this.gridColTerminationDt.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColTerminationDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColTerminationDt.Caption = "Date Terminated";
            this.gridColTerminationDt.FieldName = "TerminationDt";
            this.gridColTerminationDt.Name = "gridColTerminationDt";
            this.gridColTerminationDt.Visible = true;
            this.gridColTerminationDt.VisibleIndex = 4;
            // 
            // gridColSeAgrmntContactName
            // 
            this.gridColSeAgrmntContactName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColSeAgrmntContactName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColSeAgrmntContactName.Caption = "Our Agreement Contact";
            this.gridColSeAgrmntContactName.FieldName = "SeAgrmntContactName";
            this.gridColSeAgrmntContactName.Name = "gridColSeAgrmntContactName";
            this.gridColSeAgrmntContactName.Visible = true;
            this.gridColSeAgrmntContactName.VisibleIndex = 5;
            // 
            // gridColCmt
            // 
            this.gridColCmt.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColCmt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColCmt.Caption = "Comment";
            this.gridColCmt.FieldName = "Cmt";
            this.gridColCmt.Name = "gridColCmt";
            this.gridColCmt.Visible = true;
            this.gridColCmt.VisibleIndex = 6;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridAgreements;
            this.gridView2.Name = "gridView2";
            // 
            // tbContractFaxNos
            // 
            this.tbContractFaxNos.Controls.Add(this.gridContractFaxNos);
            this.tbContractFaxNos.Name = "tbContractFaxNos";
            this.tbContractFaxNos.Size = new System.Drawing.Size(677, 223);
            this.tbContractFaxNos.Text = "Contract Fax Nos";
            // 
            // gridContractFaxNos
            // 
            this.gridContractFaxNos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContractFaxNos.Location = new System.Drawing.Point(0, 0);
            this.gridContractFaxNos.MainView = this.gridViewContractFaxNos;
            this.gridContractFaxNos.Name = "gridContractFaxNos";
            this.gridContractFaxNos.Size = new System.Drawing.Size(677, 223);
            this.gridContractFaxNos.TabIndex = 0;
            this.gridContractFaxNos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewContractFaxNos});
            // 
            // gridViewContractFaxNos
            // 
            this.gridViewContractFaxNos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColDescription,
            this.gridColDesignationCode,
            this.gridColCountryPhoneCode,
            this.gridColAreaCode,
            this.gridColLocalNumber,
            this.gridColActiveFlag,
            this.gridColDsgnActiveFlag});
            this.gridViewContractFaxNos.GridControl = this.gridContractFaxNos;
            this.gridViewContractFaxNos.Name = "gridViewContractFaxNos";
            // 
            // gridColDescription
            // 
            this.gridColDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColDescription.Caption = "Designation";
            this.gridColDescription.FieldName = "Description";
            this.gridColDescription.Name = "gridColDescription";
            this.gridColDescription.Visible = true;
            this.gridColDescription.VisibleIndex = 0;
            // 
            // gridColDesignationCode
            // 
            this.gridColDesignationCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColDesignationCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColDesignationCode.Caption = "Type";
            this.gridColDesignationCode.FieldName = "DesignationCode";
            this.gridColDesignationCode.Name = "gridColDesignationCode";
            this.gridColDesignationCode.Visible = true;
            this.gridColDesignationCode.VisibleIndex = 1;
            // 
            // gridColCountryPhoneCode
            // 
            this.gridColCountryPhoneCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColCountryPhoneCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColCountryPhoneCode.Caption = "Country Code";
            this.gridColCountryPhoneCode.FieldName = "CountryPhoneCode";
            this.gridColCountryPhoneCode.Name = "gridColCountryPhoneCode";
            this.gridColCountryPhoneCode.Visible = true;
            this.gridColCountryPhoneCode.VisibleIndex = 2;
            // 
            // gridColAreaCode
            // 
            this.gridColAreaCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColAreaCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColAreaCode.Caption = "Area Code";
            this.gridColAreaCode.FieldName = "AreaCode";
            this.gridColAreaCode.Name = "gridColAreaCode";
            this.gridColAreaCode.Visible = true;
            this.gridColAreaCode.VisibleIndex = 3;
            // 
            // gridColLocalNumber
            // 
            this.gridColLocalNumber.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColLocalNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColLocalNumber.Caption = "Number";
            this.gridColLocalNumber.FieldName = "LocalNumber";
            this.gridColLocalNumber.Name = "gridColLocalNumber";
            this.gridColLocalNumber.Visible = true;
            this.gridColLocalNumber.VisibleIndex = 4;
            // 
            // gridColActiveFlag
            // 
            this.gridColActiveFlag.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColActiveFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColActiveFlag.Caption = "Fax # Active";
            this.gridColActiveFlag.FieldName = "ActiveFlag";
            this.gridColActiveFlag.Name = "gridColActiveFlag";
            this.gridColActiveFlag.Visible = true;
            this.gridColActiveFlag.VisibleIndex = 5;
            // 
            // gridColDsgnActiveFlag
            // 
            this.gridColDsgnActiveFlag.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColDsgnActiveFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColDsgnActiveFlag.Caption = "DSGN Active";
            this.gridColDsgnActiveFlag.FieldName = "DsgActiveFlag";
            this.gridColDsgnActiveFlag.Name = "gridColDsgnActiveFlag";
            this.gridColDsgnActiveFlag.Visible = true;
            this.gridColDsgnActiveFlag.VisibleIndex = 6;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Controls.Add(this.lblDataCptyLn);
            this.pnlMain.Controls.Add(this.lblDataCptySn);
            this.pnlMain.Controls.Add(this.lblCptyLegalName);
            this.pnlMain.Controls.Add(this.lblCptyShortName);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(683, 58);
            this.pnlMain.TabIndex = 10;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnDetails);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(481, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(200, 54);
            this.pnlButtons.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(103, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            // 
            // btnDetails
            // 
            this.btnDetails.Enabled = false;
            this.btnDetails.Location = new System.Drawing.Point(22, 16);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(75, 23);
            this.btnDetails.TabIndex = 16;
            this.btnDetails.Text = "Details";
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // lblDataCptyLn
            // 
            this.lblDataCptyLn.Location = new System.Drawing.Point(112, 34);
            this.lblDataCptyLn.Name = "lblDataCptyLn";
            this.lblDataCptyLn.Size = new System.Drawing.Size(63, 13);
            this.lblDataCptyLn.TabIndex = 15;
            this.lblDataCptyLn.Text = "labelControl1";
            // 
            // lblDataCptySn
            // 
            this.lblDataCptySn.Location = new System.Drawing.Point(112, 15);
            this.lblDataCptySn.Name = "lblDataCptySn";
            this.lblDataCptySn.Size = new System.Drawing.Size(63, 13);
            this.lblDataCptySn.TabIndex = 14;
            this.lblDataCptySn.Text = "labelControl1";
            // 
            // lblCptyLegalName
            // 
            this.lblCptyLegalName.Location = new System.Drawing.Point(18, 34);
            this.lblCptyLegalName.Name = "lblCptyLegalName";
            this.lblCptyLegalName.Size = new System.Drawing.Size(85, 13);
            this.lblCptyLegalName.TabIndex = 13;
            this.lblCptyLegalName.Text = "Cpty Legal Name:";
            // 
            // lblCptyShortName
            // 
            this.lblCptyShortName.Location = new System.Drawing.Point(17, 15);
            this.lblCptyShortName.Name = "lblCptyShortName";
            this.lblCptyShortName.Size = new System.Drawing.Size(86, 13);
            this.lblCptyShortName.TabIndex = 12;
            this.lblCptyShortName.Text = "Cpty Short Name:";
            // 
            // frmCptyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 309);
            this.Controls.Add(this.tbCntrlMain);
            this.Controls.Add(this.pnlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCptyInfo";
            this.Text = "OpsManager - Counterparty Information";
            ((System.ComponentModel.ISupportInitialize)(this.tbCntrlMain)).EndInit();
            this.tbCntrlMain.ResumeLayout(false);
            this.tbCptyAddress.ResumeLayout(false);
            this.tbCptyAddress.PerformLayout();
            this.tbCptyAgreements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAgreements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAgreements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.tbContractFaxNos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridContractFaxNos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContractFaxNos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tbCntrlMain;
        private DevExpress.XtraTab.XtraTabPage tbCptyAddress;
        private DevExpress.XtraTab.XtraTabPage tbCptyAgreements;
        private DevExpress.XtraEditors.LabelControl lblCityStateZip;
        private DevExpress.XtraEditors.LabelControl lblAddress3;
        private DevExpress.XtraEditors.LabelControl lblAddress2;
        private DevExpress.XtraEditors.LabelControl lblAddress1;
        private DevExpress.XtraEditors.LabelControl lblMainFax;
        private DevExpress.XtraEditors.LabelControl lblMainPhone;
        private DevExpress.XtraEditors.LabelControl lblCountry;
        private DevExpress.XtraTab.XtraTabPage tbContractFaxNos;
        private DevExpress.XtraEditors.LabelControl lblDataAddress3;
        private DevExpress.XtraEditors.LabelControl lblDataAddress2;
        private DevExpress.XtraEditors.LabelControl lblDataAddress1;
        private DevExpress.XtraEditors.LabelControl lblDataFax;
        private DevExpress.XtraEditors.LabelControl lblDataPhone;
        private DevExpress.XtraEditors.LabelControl lblDataCountry;
        private DevExpress.XtraEditors.LabelControl lblDataZipcode;
        private DevExpress.XtraEditors.LabelControl lblDataState;
        private DevExpress.XtraEditors.LabelControl lblDataCity;
        private DevExpress.XtraGrid.GridControl gridAgreements;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAgreements;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAgreementType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSeCptyShortName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDateSigned;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTerminationDt;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSeAgrmntContactName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCmt;
        private DevExpress.XtraGrid.GridControl gridContractFaxNos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewContractFaxNos;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDescription;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDesignationCode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCountryPhoneCode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAreaCode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColLocalNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gridColActiveFlag;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDsgnActiveFlag;
        private DevExpress.XtraEditors.PanelControl pnlMain;
        private DevExpress.XtraEditors.PanelControl pnlButtons;
        private DevExpress.XtraEditors.LabelControl lblDataCptyLn;
        private DevExpress.XtraEditors.LabelControl lblDataCptySn;
        private DevExpress.XtraEditors.LabelControl lblCptyLegalName;
        private DevExpress.XtraEditors.LabelControl lblCptyShortName;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnDetails;
    }
}