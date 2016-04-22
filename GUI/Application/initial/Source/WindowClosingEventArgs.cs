using System;
namespace NSRiskManager {
    public class WindowClosingEventArgs : EventArgs {
        #region ctor
        public WindowClosingEventArgs(string aWinName, int  windowId) 
        {
            windowName = aWinName;
            windowID = windowId;
        }
        #endregion

        #region properties
        public string windowName { get; private set; }
        public int windowID { get; private set; }
        #endregion
    }

    public delegate void WindowClosingHandler(object sender,WindowClosingEventArgs wcea);
}