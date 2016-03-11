using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Specialized;

namespace Sempra
{
	/// <summary>
	/// Create a New INI file to store or load data
	/// based on a class at http://www.codeproject.com/csharp/cs_ini.asp
	/// </summary>
	public class IniFile
	{
		private string iniPath;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,
			string key, string val, string filePath );
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,
			string key, string def, StringBuilder retVal, int size, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileSection(
			string section, byte[] retVal, int size, string filePame );

		/// <summary>
		/// Path to the INI file
		/// </summary>
		public string IniPath
		{
			get
			{
				return iniPath;
			}
		}

		/// <summary>
		/// Creates an object for accessing an INI file
		/// </summary>
		/// <param name="INIPath">Path to the INI file</param>
		public IniFile(string INIPath)
		{
			iniPath = INIPath;
		}

		private string ReadString( string Section, string Key, string defaultVal )
		{
			StringBuilder temp = new StringBuilder(255);
			GetPrivateProfileString( Section, Key, defaultVal, temp, 255, iniPath );
			
			return temp.ToString();
		}

		/// <summary>
		/// Write a string to the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Key to write, "Key="</param>
		/// <param name="Value">Value to write, "=Value"</param>
		public void WriteValue( string Section, string Key, string Value )
		{
			WritePrivateProfileString( Section, Key, Value, iniPath );
		}

		/// <summary>
		/// Write an integer to the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Key to write, "Key="</param>
		/// <param name="Value">Value to write, "=Value"</param>
		public void WriteValue( string Section, string Key, int Value )
		{
			WritePrivateProfileString( Section, Key, Value.ToString(), iniPath );
		}
        
		/// <summary>
		/// Write a boolean value to the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Key to write, "Key="</param>
		/// <param name="Value">Value to write, "=Value"</param>
		public void WriteValue( string Section, string Key, bool Value )
		{
			WritePrivateProfileString( Section, Key, Value.ToString(), iniPath );
		}
        
		/// <summary>
		/// Write a list of values to the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Values">Values to write, "Value1= Value2=..."</param>
		public void WriteValue( string Section,  StringCollection Values )
		{
			for( int i = 0; i < Values.Count; i++ )
				WritePrivateProfileString( Section, Values[i], "", iniPath );
		}
        
		/// <summary>
		/// Reads a string from the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Name of key to read</param>
		/// <param name="defaultVal">Value to return if Key not found</param>
		public string ReadValue( string Section, string Key, string defaultVal )
		{
			return ReadString( Section, Key, defaultVal );
		}
		
		/// <summary>
		/// Reads an integer from the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Name of key to read</param>
		/// <param name="defaultVal">Value to return if Key not found</param>
		public int ReadValue( string Section, string Key, int defaultVal )
		{
			return int.Parse( ReadString( Section, Key, defaultVal.ToString() ) );
		}

		/// <summary>
		/// Reads a boolean value from the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		/// <param name="Key">Name of key to read</param>
		/// <param name="defaultVal">Value to return if Key not found</param>
		public bool ReadValue( string Section, string Key, bool defaultVal )
		{
			return bool.Parse( ReadString( Section, Key, defaultVal.ToString() ) );
		}

		/// <summary>
		/// Reads a list of values from a section in the INI file
		/// </summary>
		/// <param name="Section">Section of INI file, "[Section]"</param>
		public StringCollection ReadValue( string Section )
		{
			const int bufSize = 255;
			byte[] temp = new byte[bufSize];
			StringCollection result = new StringCollection();
			
			int i = GetPrivateProfileSection( Section, temp, bufSize, iniPath);
			
			StringBuilder curItem = new StringBuilder("", temp.Length);
			char c=' ';
			
			for (i = 0; i < temp.Length - 1; i++)
			{
				if( temp[i] == '=' )								//end of entry
				{
					result.Add( curItem.ToString() );
					i++;											//skip over null character
					curItem = new StringBuilder("", temp.Length);
					c=';';
				}
				else
				{
					c = (char)temp[i];
					curItem.Append( c );
				}
			}

			return result;
		}
	}
}
