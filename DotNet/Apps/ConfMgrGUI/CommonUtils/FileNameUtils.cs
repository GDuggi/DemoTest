using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils
{
    public class FileNameUtils
    {
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
    }
}
