using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DataManager
{
    class FireHandlerClass
    {
        public FireHandlerClass(FireAlarm fireAlarm)
        {
            fireAlarm.FireEvent += new FireAlarm.FireEventHandler(FireAlarmRaised);
        }

        void FireAlarmRaised(object sender, FireEventArgs fe)
        {
            MessageBox.Show(fe.fireMsg);
        }
    }
}
