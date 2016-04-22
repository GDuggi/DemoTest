using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NSRMCommon {
    /// <summary>Utility-class for window-frame manipulation.</summary>
    public class WindowPositionHelper {
        #region constants
        public static string RegPath = @"Software\MyApp\";
        public const int INVALID_SPLIT_POSITION = -1;
        #endregion

        public static bool extractFrame(RegistryKey registryKey,string KEY,Form f) {
            string value;
            Rectangle r;

            if (!string.IsNullOrEmpty(value = registryKey.GetValue(KEY,string.Empty) as string))
                if (!(r = makeRectangle(value)).Equals(Rectangle.Empty)) {
                    f.DesktopBounds = r;
                    return true;
                }
            return false;
        }

        public static Rectangle makeRectangle(string value) {
            Rectangle ret;

            ret = readRectangleData(readPairsFrom(value));
            return constrainRectangleToScreen(ret);
        }

        static Rectangle constrainRectangleToScreen(Rectangle ret) {
            Screen[] screens = Screen.AllScreens;
            Screen screen0;
            Rectangle r2;

            foreach (var avar in screens)
                if (avar.WorkingArea.Contains(ret.Location))
                    return ret;
            r2 = new Rectangle(ret.Location,ret.Size);
            screen0 = screens[0];
            if (r2.X > screen0.WorkingArea.Width)
                r2.X = ret.X - screens[0].WorkingArea.Width;
            Debug.Print("screen-movement here?");
            return r2;
        }

        static Rectangle readRectangleData(IDictionary<string,string> pairs) {
            Rectangle ret = Rectangle.Empty;
            int n;

            foreach (string key in pairs.Keys) {
                switch (key) {
                    case "X": if (int.TryParse(pairs[key],out n)) ret.X = n; break;
                    case "Y": if (int.TryParse(pairs[key],out n)) ret.Y = n; break;
                    case "Width": if (int.TryParse(pairs[key],out n)) ret.Width = n; break;
                    case "Height": if (int.TryParse(pairs[key],out n)) ret.Height = n; break;
                }
            }
            return ret;
        }

        public static void saveFrame(RegistryKey registryKey,string KEY,Form f) {
            registryKey.SetValue(KEY,f.DesktopBounds);
        }

        public static bool readRegistryBool(RegistryKey rk,string key) {
            bool ret = false;
            string tmp;

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key","registry key is null!");
            if (Array.IndexOf<string>(rk.GetValueNames(),key) >= 0)
                if (rk.GetValueKind(key) == RegistryValueKind.String)
                    if (!string.IsNullOrEmpty(tmp = rk.GetValue(key,"N") as string))
                        if (char.ToUpper(tmp[0]) == 'Y' || tmp[0] == '1')
                            ret = true;
            return ret;
        }

        public static int readRegistryInt(RegistryKey rk,string key) {
            int ival;

            if (Int32.TryParse(rk.GetValue(key,INVALID_SPLIT_POSITION).ToString(),out ival))
                if (ival != INVALID_SPLIT_POSITION)
                    return ival;
            return INVALID_SPLIT_POSITION;
        }

        static IDictionary<string,string> readPairsFrom(string value) {
            IDictionary<string,string> ret = new Dictionary<string,string>();
            string[] pargs = value.Split('{',',','}');
            int pos;

            foreach (string anArg in pargs)
                if (!string.IsNullOrEmpty(anArg))
                    if ((pos = anArg.IndexOf('=')) >= 0)
                        ret.Add(anArg.Substring(0,pos),anArg.Substring(pos + 1));
            return ret;
        }
    }
}