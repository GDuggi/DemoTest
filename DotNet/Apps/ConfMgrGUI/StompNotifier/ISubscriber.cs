using System;
using System.Collections.Generic;
using System.Text;

namespace OpsManagerNotifier
{
    public interface ISubscriber
    {
        void  Subscribe();
        void UnSubscribe();
    }
}
