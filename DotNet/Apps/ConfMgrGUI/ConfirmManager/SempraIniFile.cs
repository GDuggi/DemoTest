using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Specialized;

namespace Sempra.Ops
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

		public string IniPath
		{
			get
			{
				return iniPath;
			}
		}

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

		public void WriteValue( string Section, string Key, string Value )
		{
			WritePrivateProfileString( Section, Key, Value, iniPath );
		}

		public void WriteValue( string Section, string Key, int Value )
		{
			WritePrivateProfileString( Section, Key, Value.ToString(), iniPath );
		}
        
		public void WriteValue( string Section, string Key, bool Value )
		{
			WritePrivateProfileString( Section, Key, Value.ToString(), iniPath );
		}
        
		public void WriteValue( string Section,  StringCollection Values )
		{
			for( int i = 0; i < Values.Count; i++ )
				WritePrivateProfileString( Section, Values[i], "", iniPath );
		}
        
		public string ReadValue( string Section, string Key, string defaultVal )
		{
			return ReadString( Section, Key, defaultVal );
		}
		
		public int ReadValue( string Section, string Key, int defaultVal )
		{
			return int.Parse( ReadString( Section, Key, defaultVal.ToString() ) );
		}

		public bool ReadValue( string Section, string Key, bool defaultVal )
		{
			return bool.Parse( ReadString( Section, Key, defaultVal.ToString() ) );
		}

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
