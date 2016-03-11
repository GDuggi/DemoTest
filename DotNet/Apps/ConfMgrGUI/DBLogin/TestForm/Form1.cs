using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TestForm
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;

		private System.Data.OracleClient.OracleConnection oracleConnection;
		private DevExpress.XtraGrid.GridControl gridData;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
		private System.Data.OracleClient.OracleDataAdapter oracleDataAdapter;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			oracleConnection  = new System.Data.OracleClient.OracleConnection();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.gridData = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(96, 48);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "Login";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(64, 120);
			this.label1.Name = "label1";
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// gridData
			// 
			// 
			// gridControl1.EmbeddedNavigator
			// 
			this.gridData.EmbeddedNavigator.Name = "";
			this.gridData.Location = new System.Drawing.Point(168, 120);
			this.gridData.MainView = this.gridView1;
			this.gridData.Name = "gridData";
			this.gridData.Size = new System.Drawing.Size(440, 280);
			this.gridData.TabIndex = 2;
			this.gridData.Text = "gridControl1";
			// 
			// gridView1
			// 
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
																							 this.gridColumn1,
																							 this.gridColumn2});
			this.gridView1.GridControl = this.gridData;
			this.gridView1.Name = "gridView1";
			// 
			// gridColumn1
			// 
			this.gridColumn1.Caption = "ID";
			this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.gridColumn1.FieldName = "ID";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.VisibleIndex = 0;
			// 
			// gridColumn2
			// 
			this.gridColumn2.Caption = "Name";
			this.gridColumn2.FieldName = "NAME";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.VisibleIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 461);
			this.Controls.Add(this.gridData);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Name = "Form1";
			this.Text = "Oracle Logon";
			this.Closed += new System.EventHandler(this.Form1_Closed);
			((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Collections.Specialized.StringCollection databases = new System.Collections.Specialized.StringCollection();
				databases.Add( "SEMPRA.DEV" );
				databases.Add( "SEMPRA.STAGE" );

				Sempra.Login oracleLogin = new Sempra.Login(
					true,
					5,
					"H:\\VS2003Projects\\SempraOpsDevLib\\Login",
					"\\\\stdev2\\dev\\OpsDev\\XMLSchemas\\",
					databases );

				if ( oracleLogin.Execute( oracleConnection ) )
				{
					label1.Text = "Login Successful";

					oracleDataAdapter = new System.Data.OracleClient.OracleDataAdapter(
						"select * from dept", oracleConnection );

					DataSet dataSet = new DataSet();
					oracleDataAdapter.Fill( dataSet );
					gridData.BeginUpdate();
					gridData.DataSource = dataSet;
					gridData.DataMember = "Table";
					gridData.EndUpdate();
				}
				else
					label1.Text = "Login Failed";
			}
			catch( Exception error )
			{
				MessageBox.Show( error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			oracleConnection.Close();
		}
	}
}
