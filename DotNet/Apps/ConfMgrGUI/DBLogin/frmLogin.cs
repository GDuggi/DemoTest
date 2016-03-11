using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace Sempra
{
	/// <summary>
	/// Facilitates logging into an Oracle database, prompting the user for logon information
	/// </summary>
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
	{
		private enum LoginResult
		{
			Successful,
			Unsuccessful,
			Cancelled
		}

        private const string FORM_NAME = "frmLogin";
        private const string FORM_ERROR_CAPTION = "Login Form Error";
        private SqlConnection connection;
        private Sempra.Ops.DBMSType dbms = Sempra.Ops.DBMSType.SQLServer;
		private LoginResult loginResult;
		private LoginHistory history;
		private string userListXmlFile;
		private int maxLoginAttempts = 5; //default value, can be overwritten
		private int failedLoginAttempts;
        public string settingsDir;

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteUserSettings();
        }

        private void ReadUserSettings()
        {
            try
            {
                //Now read user settings, ReadAppSettings() must be called first
                Sempra.IniFile iniFile = new Sempra.IniFile(Sempra.Ops.GetUserIniFileName(settingsDir));

                this.StartPosition = FormStartPosition.Manual;
                this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
                this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
                this.Width = 384;
                this.Height = 240;
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                       "Error CNF-399 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                Sempra.IniFile iniFile = new Sempra.IniFile(Sempra.Ops.GetUserIniFileName(settingsDir));
                iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                iniFile.WriteValue(FORM_NAME, "Left", this.Left);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                       "Error CNF-400 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


		/// <param name="ShowLoginHistory">Allow user to select from list of previous usernames and databases</param>
		/// <param name="HistoryLength">Number of items to store in the history</param>
		/// <param name="SettingsDir">Settings directory (location to store list of logins)</param>
		/// <param name="SchemaPath">Full path to directory where XML Schemas are stored</param>
		/// <param name="Servers">List of databases to allow the user to choose from</param>
		public frmLogin(
			bool ShowLoginHistory,
			int HistoryLength,
			string SettingsDir,
			string SchemaPath,
			StringCollection Servers )
		{
			InitializeComponent();            

			//Clears text boxes
			UserName = "";
			Password = "";
			Server = "";
            Database = "";

            settingsDir = SettingsDir;

			//In case they didn't pass a trailing backslash
			if( SettingsDir[SettingsDir.Length - 1] != '\\' )
				SettingsDir += "\\";

			if( SchemaPath[SchemaPath.Length - 1] != '\\' )
				SchemaPath += "\\";

			//Create the settings directory, in case it doesn't exist
			if( !System.IO.Directory.Exists( SettingsDir ) )
				System.IO.Directory.CreateDirectory( SettingsDir );

            userListXmlFile = Path.Combine(SettingsDir, "DBLoginHistory.xml");
            string userListXmdFile = Path.Combine(SettingsDir, "LoginHistory.xsd");
            string xsdDistLocation = Path.Combine(System.Environment.CurrentDirectory, "LoginHistory.xsd");

            if (!File.Exists(userListXmdFile))
            {
                File.Copy(xsdDistLocation, userListXmdFile);
            }

            ReadUserSettings();

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
		#region Properties

		/// <summary>
		/// Gets or Sets the database connection of the form
		/// </summary>
        public SqlConnection Connection
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
        /// Sets the database displayed in the dialog
        /// </summary>
        public string Database
        {
            get
            {
                return this.database.Text;
            }

            set
            {
                this.database.Text = value;
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
        public bool Execute(SqlConnection Connection)
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
						new System.EventHandler( HistoryItemClick ) );
					contextMenu.MenuItems.Add( menuItem );
				}
			}

			//initialize variables, since they're loop conditions
			loginResult = LoginResult.Unsuccessful;
			failedLoginAttempts = 0;

			//Attempt logging in until successful, cancelled, or maxed out on attempts
			while( ( loginResult == LoginResult.Unsuccessful ) &&
				( failedLoginAttempts < maxLoginAttempts ) )
				this.ShowDialog();

			//return whether result was successful
			return loginResult == LoginResult.Successful;
		}

        public string GetConnectionStr(
            Sempra.Ops.DBMSType DBMS,
            string UserName,
            string Password,
            string Server,
            string Database)
        {
            switch (DBMS)
            {
                case Sempra.Ops.DBMSType.Oracle:
                    return
                        "user id=" + UserName +
                        ";password=" + Password +
                        ";data source=" + Server;

                case Sempra.Ops.DBMSType.SQLServer:
                    return
                        "server=\"" + Server +
                        "\";uid=\"" + UserName +
                        "\";pwd=\"" + Password +
                        "\";database=\"" + Database + "\";";

                default:
                    return "";
            }
        }//switch DBMS

        public string GetMSSQLConnectionStr()
        {
            string connectionStr = "";
            connectionStr = GetConnectionStr(Sempra.Ops.DBMSType.SQLServer, UserName, password.Text, Server, Database);
            return connectionStr;
        }
        
        private void SetWindowText()
		{
			switch( dbms )
			{
				case Sempra.Ops.DBMSType.Oracle:
					this.Text = "Oracle Logon";
					break;

				case Sempra.Ops.DBMSType.SQLServer:
					this.Text = "SQL Server Logon";
					break;
			}
		}

		#endregion

		#region Event Handlers

		private void OK_Click( object sender, System.EventArgs e )
		{
			try
			{
				//make sure connection is closed
				connection.Close();

				//Generate connection string               
				connection.ConnectionString = GetConnectionStr(
					dbms,
					userName.Text,
					password.Text,
					server.Text,
                    database.Text );

				//Open the connection - if unsuccessful, raises exception, caught below
				connection.Open();

				//If successful, add login information to history
				history.AddLogin( userName.Text, server.Text, database.Text ); //only if login is successful

				//result returned to Execute is "successful"
				loginResult = LoginResult.Successful;
			}
			catch( Exception error )
			{
				failedLoginAttempts++;
				loginResult = LoginResult.Unsuccessful;

                //string errorText =
                //    "Login attempt failed:\n" +
                //    error.Message +
                //    "\nAttempt " + failedLoginAttempts.ToString() + "/" + maxLoginAttempts.ToString();

                //MessageBox.Show(
                //    errorText,
                //    "Login failed",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Error );

                XtraMessageBox.Show("Login failed. Attempt: " + failedLoginAttempts.ToString() + " / " + maxLoginAttempts.ToString() + Environment.NewLine +
                       "Error CNF-401 in " + FORM_NAME + ".OK_Click(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

		}

		private void Cancel_Click( object sender, System.EventArgs e )
		{
			loginResult = LoginResult.Cancelled;
		}

		private void userName_ButtonClick( object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
		{
			int cursorX = userName.Width - 20; //left edge of button
			int cursorY = userName.Height; //below button

			contextMenu.Show( userName, new System.Drawing.Point( cursorX, cursorY ) );
		}

		/// <summary>
		/// Fills the username and database text boxes, and focuses the password box
		/// </summary>
		private void HistoryItemClick( object sender, System.EventArgs e )
		{
            MenuItem menuItem = sender as MenuItem;
            string storedData = menuItem.Text;
            string[] fields = storedData.Split(';');
            string userField = fields[0];
            string serverField = fields[1];
            string databaseField = fields[2];

            userName.Text = userField;
			server.Text = serverField;
            database.Text = databaseField;
			password.Focus();
		}

		#endregion
	}
}