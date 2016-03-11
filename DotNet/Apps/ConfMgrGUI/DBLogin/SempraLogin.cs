using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Controls;

namespace Sempra
{
	/// <summary>
	/// Facilitates logging into an Oracle database, prompting the user for logon information
	/// </summary>
	public class Login : System.Windows.Forms.Form
	{
		private enum LoginResult
		{
			Successful,
			Unsuccessful,
			Cancelled
		}

		private IDbConnection connection;
		private Sempra.Ops.DBMSType dbms = Sempra.Ops.DBMSType.Oracle;
		private LoginResult loginResult;
		private LoginHistory history;
		private string userListXmlFile;
		private int maxLoginAttempts = 5; //default value, can be overwritten
		private int failedLoginAttempts;

		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.GroupBox groupBox1;
		private DevExpress.XtraEditors.ButtonEdit userName;
		private DevExpress.XtraEditors.TextEdit password;
		private DevExpress.XtraEditors.ComboBoxEdit server;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <param name="ShowLoginHistory">Allow user to select from list of previous usernames and databases</param>
		/// <param name="HistoryLength">Number of items to store in the history</param>
		/// <param name="SettingsDir">Settings directory (location to store list of logins)</param>
		/// <param name="SchemaPath">Full path to directory where XML Schemas are stored</param>
		/// <param name="Databases">List of databases to allow the user to choose from</param>
		public Login(
			bool ShowLoginHistory,
			int HistoryLength,
			string SettingsDir,
			string SchemaPath,
			StringCollection Servers )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			//Clears text boxes
			UserName = "";
			Password = "";
			Server   = "";

			//In case they didn't pass a trailing backslash
			if( SettingsDir[SettingsDir.Length-1] != '\\' )
				SettingsDir += "\\";

			if( SchemaPath[SchemaPath.Length-1] != '\\' )
				SchemaPath += "\\";

			//Create the settings directory, in case it doesn't exist
			if( !System.IO.Directory.Exists( SettingsDir ) )
				System.IO.Directory.CreateDirectory( SettingsDir );

			userListXmlFile = SettingsDir + "DBLoginHistory.xml";

			//Create the login history list
			history = new LoginHistory( SchemaPath, userListXmlFile );
			//Just in case the HistoryLength passed is negative
			history.HistoryLength = Math.Abs( HistoryLength ); //safeguard against negative input

			//This is how you disable the Login History, if ShowLoginHistory is false
			userName.Properties.Buttons[0].Visible = ShowLoginHistory;
				
			//Load databases into the combo box's dropdown
			DBList.Clear();
			foreach( string db in Servers )
				DBList.Add( db );

			SetWindowText();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.Cancel = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.userName = new DevExpress.XtraEditors.ButtonEdit();
			this.server = new DevExpress.XtraEditors.ComboBoxEdit();
			this.password = new DevExpress.XtraEditors.TextEdit();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.userName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.server.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.password.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Cancel.Location = new System.Drawing.Point(159, 160);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 5;
			this.Cancel.Text = "Cancel";
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// OK
			// 
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.OK.Location = new System.Drawing.Point(71, 160);
			this.OK.Name = "OK";
			this.OK.TabIndex = 4;
			this.OK.Text = "OK";
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.userName);
			this.groupBox1.Controls.Add(this.server);
			this.groupBox1.Controls.Add(this.password);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.groupBox1.Location = new System.Drawing.Point(15, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 128);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// userName
			// 
			this.userName.EditValue = "buttonEdit1";
			this.userName.Location = new System.Drawing.Point(80, 24);
			this.userName.Name = "userName";
			// 
			// userName.Properties
			// 
			this.userName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
			this.userName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
																											 new DevExpress.XtraEditors.Controls.EditorButton()});
			this.userName.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
			this.userName.Size = new System.Drawing.Size(184, 22);
			this.userName.TabIndex = 0;
			this.userName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.userName_ButtonClick);
			// 
			// server
			// 
			this.server.EditValue = "comboBoxEdit1";
			this.server.Location = new System.Drawing.Point(80, 88);
			this.server.Name = "server";
			// 
			// server.Properties
			// 
			this.server.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
			this.server.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
																										   new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.server.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
			this.server.Size = new System.Drawing.Size(184, 22);
			this.server.TabIndex = 2;
			// 
			// password
			// 
			this.password.EditValue = "textEdit2";
			this.password.Location = new System.Drawing.Point(80, 56);
			this.password.Name = "password";
			// 
			// password.Properties
			// 
			this.password.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
			this.password.Properties.PasswordChar = '*';
			this.password.Size = new System.Drawing.Size(184, 22);
			this.password.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Server:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Password:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "Username:";
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItem1,
																						this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Login1, Database1";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Login2, Database2";
			// 
			// Login
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(310, 198);
			this.ControlBox = false;
			this.Controls.Add(this.OK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Login";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Oracle Logon";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.userName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.server.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.password.Properties)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the database connection of the form
		/// </summary>
		public IDbConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				this.connection = value;
			}
		}

		/// <summary>
		/// Sets the username displayed in the dialog
		/// </summary>
		public string UserName
		{
			get
			{
				return this.userName.Text;
			}

			set
			{
				this.userName.Text = value;
			}
		}

		/// <summary>
		/// Sets the password displayed in the dialog
		/// </summary>
		public string Password
		{
			set
			{
				this.password.Text = value;
			}
		}

		/// <summary>
		/// Sets the database displayed in the dialog
		/// </summary>
		public string Server
		{
			get
			{
				return this.server.Text;
			}

			set
			{
				this.server.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets list of items in the Database combo box's dropdown
		/// </summary>
		public ComboBoxItemCollection DBList
		{
			get
			{
				return this.server.Properties.Items;
			}

			set
			{
				this.server.Properties.Items.Assign( value );
			}
		}

		/// <summary>
		/// Gets or sets the location and file name where the login history is stored
		/// </summary>
		public string UserListXMLFile
		{
			get
			{
				return this.userListXmlFile;
			}

			set
			{
				this.userListXmlFile = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of logins stored in the history
		/// </summary>
		public int HistoryLength
		{
			get
			{
				return this.history.HistoryLength;
			}

			set
			{
				this.history.HistoryLength = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of times a user can fail logging in before quitting
		/// </summary>
		public int MaxLoginAttempts
		{
			get
			{
				return this.maxLoginAttempts;
			}

			set
			{
				this.maxLoginAttempts = value;
			}
		}

		/// <summary>
		/// Gets the number of failed login attempts
		/// </summary>
		public int FailedLoginAttempts
		{
			get
			{
				return this.failedLoginAttempts;
			}
		}

		/// <summary>
		/// The type of database connecting to
		/// </summary>
		public Sempra.Ops.DBMSType DBMS
		{
			get
			{
				return this.dbms;
			}

			set
			{
				this.dbms = value;
				SetWindowText();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Executes the login form, returning whether the login was successful
		/// </summary>
		public bool Execute( IDbConnection Connection )
		{
			//Initialize the database connection
			this.connection = Connection;

			//get the login history
			StringCollection loginList;
			loginList = history.GetLogins();

			contextMenu.MenuItems.Clear();

			userName.Properties.Buttons[0].Enabled = loginList.Count > 0;

			//fill the context menu that contains the list of logins
			if( loginList.Count > 0 )
			{
				MenuItem menuItem;
				for( int i = 0; i < loginList.Count; i++ )
				{
					menuItem = new MenuItem( loginList[i],
						new System.EventHandler(HistoryItemClick) );
					contextMenu.MenuItems.Add( menuItem );
				}
			}

			//initialize variables, since they're loop conditions
			loginResult = LoginResult.Unsuccessful;
			failedLoginAttempts = 0;

			//Attempt logging in until successful, cancelled, or maxed out on attempts
			while(  (loginResult == LoginResult.Unsuccessful) &&
				(failedLoginAttempts < maxLoginAttempts) )
				this.ShowDialog();
			
			//return whether result was successful
			return loginResult == LoginResult.Successful;
		}

		private void SetWindowText()
		{
			switch( dbms )
			{
				case Sempra.Ops.DBMSType.Oracle :
					this.Text = "Oracle Logon";
					break;

				case Sempra.Ops.DBMSType.SQLServer :
					this.Text = "SQL Server Logon";
					break;
			}
		}

		#endregion

		#region Event Handlers

		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				//make sure connection is closed
				connection.Close();

				//Generate connection string
				connection.ConnectionString = Sempra.Ops.GetConnectionStr(
					dbms,
					userName.Text,
					password.Text,
					server.Text );

				//Open the connection - if unsuccessful, raises exception, caught below
				connection.Open();

				//If successful, add login information to history
				history.AddLogin( userName.Text, server.Text ); //only if login is successful

				//result returned to Execute is "successful"
				loginResult = LoginResult.Successful;
			}
			catch ( Exception error )
			{
				failedLoginAttempts++;
				loginResult = LoginResult.Unsuccessful;

				string errorText =
					"Login attempt failed:\n" +
					error.Message +
					"\nAttempt " + failedLoginAttempts.ToString() + "/" + maxLoginAttempts.ToString();

				MessageBox.Show(
					errorText,					
					"Login failed",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error );
			}

		}

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			loginResult = LoginResult.Cancelled;
		}

		private void userName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			int cursorX = userName.Width - 20; //left edge of button
			int cursorY = userName.Height; //below button
			
			contextMenu.Show( userName, new System.Drawing.Point( cursorX, cursorY ) );
		}

		/// <summary>
		/// Fills the username and database text boxes, and focuses the password box
		/// </summary>
		private void HistoryItemClick(object sender, System.EventArgs e)
		{
			string user = sender.ToString();
			user = user.Substring( user.IndexOf("Text:") + 6 ); //separate Text from all properties
			string data = user;
			int comma = user.IndexOf( ',' );
			user = user.Substring( 0, comma );
			data = data.Substring( comma + 2 );

			userName.Text = user;
			server.Text = data;
			password.Focus();
		}

		#endregion
	}
}
