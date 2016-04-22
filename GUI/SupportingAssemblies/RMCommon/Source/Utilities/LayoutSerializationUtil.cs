using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NSRMCommon
{
    [Obsolete ("This class is no longer needed",true)]
    public static class LayoutSerializationUtil {
        #region constants
        const string WINDOW_DIR = "Windows";
        #endregion

        #region fields
        static string _serializationPath;
        #endregion

        #region properties
        public static string serializationPath {
            get {
                if (string.IsNullOrEmpty(_serializationPath)) {
                    _serializationPath = Path.Combine(
                        Environment.GetEnvironmentVariable("APPDATA"),
                        Regex.Replace(Application.CompanyName,"[\\.]*",""),
                        Application.ProductName,
                        Regex.Replace(Application.ProductVersion,"[\\.]?","_"));
                }
                return _serializationPath;
            }
        }
        #endregion

        #region methods
        public static string makeSerializationPath(string other) {
            return Path.Combine(serializationPath,other);
        }
        internal static void removePGFile(string tabName) {
            string tmp;

            if (string.IsNullOrEmpty(tabName))
                throw new ArgumentNullException("tabName","Tab-name is null!");
            if (Directory.Exists(serializationPath))
                if (File.Exists(tmp = makeSerializationPath(tabName + ".xml")))
                    File.Delete(tmp);
        }
        public static string windowPath(string aWinName) {
            if (string.IsNullOrEmpty(aWinName))
                return Path.Combine(serializationPath,WINDOW_DIR);
            return Path.Combine(serializationPath,WINDOW_DIR,aWinName);
        }
        #endregion

    }
}