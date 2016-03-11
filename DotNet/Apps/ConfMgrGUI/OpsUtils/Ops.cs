using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace Sempra
{
	/// <summary>
	/// List of items obtained by GetVersionInfo()
	/// </summary>
	public struct VersionInfo
	{
		/// <summary></summary>
		public string CompanyName;
		/// <summary></summary>
		public string FileDescription;
		/// <summary></summary>
		public string FileVersion;
		/// <summary></summary>
		public string InternalName;
		/// <summary></summary>
		public string LegalCopyright;
		/// <summary></summary>
		public string LegalTradeMarks;
		/// <summary></summary>
		public string OriginalFileName;
		/// <summary></summary>
		public string ProductName;
		/// <summary></summary>
		public string ProductVersion;
		/// <summary></summary>
		public string Comments;
	}
		
	/// <summary>
	/// Catch-all class containing types and static methods
	/// </summary>
	public class Ops
	{
        private const string PROJ_FILE_NAME = "Ops";
        
        /// <summary>
		/// The type of connection to the database
		/// </summary>
		public enum DBMSType
		{
			/// <summary>
			/// Value for an Oracle table
			/// </summary>
			Oracle,

			/// <summary>
			/// Value for a SQL Server table
			/// </summary>
			SQLServer
		}

		/// <summary>
		/// Retrieves a value from a string[] based on parameter syntax
		/// </summary>
		/// <param name="ParamName">Name of parameter after "-" ("-u" should be passed as "u")</param>
		/// <param name="args">Arguments passed into constructor from Main()</param>
		public static string GetParamValue( string ParamName, string[] args )
		{
			int i;
			for( i = 0; i != args.Length; i++ )
			{
				if( args[i].IndexOf( "-" + ParamName ) >= 0 )
					break;
			}

			if( i == args.Length )
				return "";
			
			if( args[i] == "-" + ParamName )
			{
				//is the next line another parameter name? If not, it's the value
				if( args[i+1].IndexOf( "-" ) < 0 )
					return args[i+1];
				//it is another parameter name, parameter has null value
				else
					return "";
			}

			string result = args[i].Substring( args[i].IndexOf( "-" + ParamName ) + 1 + ParamName.Length );
			int nextParamIndex = result.IndexOf( "-" );
			if( nextParamIndex >= 0 )
				result = result.Substring( 0, nextParamIndex );

			return result.Trim();
		}

		/// <summary>
		/// Methods imported from the Win32 API
		/// </summary>
		//some code from MSDN (http://msdn.microsoft.com/library/default.asp?url=/library/en-us/csref/html/vcwlkUnsafeCodeTutorial.asp)
		public class Win32Imports 
		{
			/// <summary>
			/// Gets version info for a file
			/// </summary>
			[DllImport("version.dll")]
			public static extern bool GetFileVersionInfo (string sFileName,
				int handle, int size, byte[] infoBuffer);

			/// <summary>
			/// Gets the size of the version info for a file
			/// </summary>
			[DllImport("version.dll")]
			public static extern int GetFileVersionInfoSize (string sFileName,
				out int handle);

			/// <summary>
			/// Used in getting version info
			/// </summary>
			// The third parameter - "out string pValue" - is automatically
			// marshaled from ANSI to Unicode:
			[DllImport("version.dll")]
			unsafe public static extern bool VerQueryValue (byte[] pBlock,
				string pSubBlock, out string pValue, out uint len);

			/// <summary>
			/// Used in getting version info
			/// </summary>
			// This VerQueryValue overload is marked with 'unsafe' because 
			// it uses a short*:
			[DllImport("version.dll")]
			unsafe public static extern bool VerQueryValue (byte[] pBlock,
				string pSubBlock, out short *pValue, out uint len);
		
			/// <summary>
			/// Gets the name of localhost on the computer it's run from
			/// </summary>
			[DllImport("Kernel32")]
			unsafe public static extern bool GetComputerName(byte* lpBuffer,long* nSize);

			/// <summary>
			/// Gets the login of the user whose machine calls the function
			/// </summary>
			[DllImport("Advapi32.dll", EntryPoint="GetUserName", ExactSpelling=false,
				 SetLastError=true)]
			public static extern bool GetUserName(
				[MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
				[MarshalAs(UnmanagedType.LPArray)] Int32[] nSize );
		
			/// <summary>
			/// Provides a beep used for void input
			/// </summary>
			[DllImport("user32.dll", SetLastError=true)]
			public static extern bool MessageBeep(int code);

			/// <summary>
			/// Runs a file from the shell
			/// </summary>
			[DllImport("Shell32.dll",CharSet=CharSet.Auto)]
			public static extern IntPtr ShellExecute(
				IntPtr hwnd,
				string lpVerb,
				string lpFile,
				string lpParameters,
				string lpDirectory,
				int nShowCmd );
		}
 
		/// <summary>
		/// Gets the version info for a given application
		/// </summary>
		/// <param name="AppName">Full path to the application to get version info</param>
		// marked with 'unsafe' because it uses pointers:
		unsafe public static VersionInfo GetVersionInfo( string AppName ) 
		{
			VersionInfo result = new VersionInfo();

			try 
			{
				int handle = 0;
				// Figure out how much version info there is:
				int size =
					Win32Imports.GetFileVersionInfoSize(AppName, out handle);

				if (size == 0)
                    throw new Exception("Couldn't find Application: " + AppName + "." + Environment.NewLine +
                        "Error CNF-402 in " + PROJ_FILE_NAME + ".validateData().");
					
				byte[] buffer = new byte[size];

				if (!Win32Imports.GetFileVersionInfo( AppName, handle, size, buffer ))
                    throw new Exception("Failed to query file version information." + Environment.NewLine +
                        "Error CNF-403 in " + PROJ_FILE_NAME + ".validateData().");

				const int INFONUM = 10;
				string[] InfoStr = new string[INFONUM]
				{
					"CompanyName",
					"FileDescription",
					"FileVersion",
					"InternalName",
					"LegalCopyright",
					"LegalTradeMarks",
					"OriginalFileName",
					"ProductName",
					"ProductVersion",
					"Comments"
				};

				short *subBlock = null;
				uint len = 0;
				string val;

				if (!Win32Imports.VerQueryValue (buffer, @"\VarFileInfo\Translation", out subBlock, out len))
                    throw new Exception("Failed to query version information." + Environment.NewLine +
                        "Error CNF-404 in " + PROJ_FILE_NAME + ".validateData().");
				
				for( int i = 0; i < INFONUM; i++ )
				{
					string spv = "\\StringFileInfo\\" + subBlock[0].ToString("X4") + subBlock[1].ToString("X4") + "\\" + InfoStr[i];

					if (Win32Imports.VerQueryValue (buffer, spv, out val, out len))
					{
						switch( i )
						{
							case 0 : result.CompanyName			= val; break;
							case 1 : result.FileDescription		= val; break;
							case 2 : result.FileVersion			= val; break;
							case 3 : result.InternalName		= val; break;
							case 4 : result.LegalCopyright		= val; break;
							case 5 : result.LegalTradeMarks		= val; break;
							case 6 : result.OriginalFileName	= val; break;
							case 7 : result.ProductName			= val; break;
							case 8 : result.ProductVersion		= val; break;
							case 9 : result.Comments			= val; break;
						} // switch i 
					} // if Win32Imports.VerQueryValue
				}
			}
			catch (Exception e) 
			{
                throw new Exception("Error retrieving file info." + Environment.NewLine +
                    "Error CNF-405 in " + PROJ_FILE_NAME + ".validateData(): " + e.Message);
            }
    
			return result;
		}

		/// <summary>
		/// Used internally to convert an Ascii-Z string to a string
		/// </summary>
		public static string GetStringFromByteArray( byte[] source )
		{
			System.Text.Encoding textEnc = new System.Text.ASCIIEncoding();
			string result = textEnc.GetString(source);
			System.Globalization.UnicodeCategory uniCat;
			for( int i = 0; i < result.Length; i++ )
			{
				uniCat = char.GetUnicodeCategory( result, i );
				if( uniCat == System.Globalization.UnicodeCategory.Control )
					result = result.Substring( 0, i ); //quits loop also
			}
			return result;
		}

		/// <summary>
		/// Gets the local machine's name
		/// </summary>
		unsafe public static string GetComputerName()
		{
			byte[] buffer = new byte[512];
			long size = buffer.Length;
			
			long* pSize = &size;
			fixed (byte* pBuffer = buffer)
			{
				Win32Imports.GetComputerName(pBuffer,pSize);
			}

			return GetStringFromByteArray( buffer );
		}	

		/// <summary>
		/// Gets the current user's login name
		/// </summary>
		public static string GetUserName()
		{
			byte[] str=new byte[20];
			Int32[] len=new Int32[1];
			len[0]=20;
			Win32Imports.GetUserName(str,len);
			return GetStringFromByteArray( str );
		}

		/// <summary>
		/// Gets what the name of the application's INI file for user settings should be
		/// </summary>
		/// <param name="SettingsDir">Path to location for storing settings (usually retrieved from a config file)</param>
		public static string GetUserIniFileName( string SettingsDir )
		{
			string appName = Application.ExecutablePath;
			//start at end, but after ".exe"
			for( int i = appName.Length - 5; i > 0; i-- )
			{
				if( appName[i] == '\\' )
				{
					appName = appName.Substring( i + 1 );
					break;
				}
			}

			string result = appName.Substring( 0, appName.Length - 4 ) + ".ini";

			if( SettingsDir[SettingsDir.Length-1] != '\\' )
				SettingsDir += "\\";

			result = SettingsDir + result;
			return result;
		}

		/// <summary>
		/// Makes an Oracle date out of a DateTime object
		/// </summary>
		public static string OracleDate( DateTime Date )
		{
			return "TO_DATE( '" + Date.ToString( "MM/dd/yyyy" ) + "', 'MM/DD/YYYY' )";
		}

		/// <summary>
		/// Returns a StringCollection from a list of comma separated values
		/// </summary>
		public static StringCollection ListFromString( string StringList )
		{
			StringCollection result = new StringCollection();

			if( StringList == null )
				return result;

			while( StringList != "" )
			{
				int i = StringList.IndexOf( "," );

				if( i >= 0 )
				{
					result.Add( StringList.Substring( 0, i ).Trim() );
					StringList = StringList.Substring( i + 1 );
				}
				else
				{
					result.Add( StringList.Trim() );
					StringList = "";
				}
			}

			return result;
		}

		/// <summary>
		/// Formats the text of an error message
		/// </summary>
		/// <param name="Procedure">Name of the method calling this function (without any parentheses)</param>
		/// <param name="Error">The Exception.Message that triggered the error</param>
		public static string ErrorMessage( string Procedure, string Error )
		{
			//Adds "()" to end of procedure name, followed by ":" and a carriage return
			//   The carriage return is first so that this can be called recursively
			return "\n" + Procedure + "(): " + Error;
		}

		/// <summary>
		/// Calls a standard error message box with the Message's text
		/// </summary>
		public static void ShowErrorMessage( string Message )
		{
			MessageBox.Show(
				Message,
				"Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error );
		}

		/// <summary>
		/// Gets a standard connection string from a list of Username, Password, and Server
		/// </summary>
		/// <param name="DBMS">Type of database to format the ConnectionStr for</param>
		/// <param name="UserName">Username in connection string</param>
		/// <param name="Password">Password in connection string</param>
		/// <param name="Server">Server in connection string</param>
		public static string GetConnectionStr(
			DBMSType DBMS,
			string UserName,
			string Password,
			string Server )
		{
            string strResult = "";
			switch( DBMS )
			{
				case Sempra.Ops.DBMSType.Oracle :
                    //DATA SOURCE=SEMPRA.PROD;PERSIST SECURITY INFO=True;USER ID=OPS_TRACKING
					strResult =
                        "user id=" + UserName +
                        ";password=" + Password +
                        ";data source=" + Server;
                    break;
                case Sempra.Ops.DBMSType.SQLServer:
					strResult =
						"uid=\"" + UserName +
						"\";pwd=\"" + Password +
						"\";server=\"" + Server + "\";";
                    break;				
				default :
					strResult = "";
                    break;
			}//switch DBMS
            return strResult;
		}
	}
}
