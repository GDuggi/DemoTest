using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager
{
    public class FireAlarm
    {
        public delegate void FireEventHandler(object sender, FireEventArgs fe);
        public event FireEventHandler FireEvent;

        public void ActivateFireAlarm(string msg)
        {
            FireEventArgs fireArgs = new FireEventArgs(msg);

            FireEvent(this.FireEvent, fireArgs);
        }
    }
}
