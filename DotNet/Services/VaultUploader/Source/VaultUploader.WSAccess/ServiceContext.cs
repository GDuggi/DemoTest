using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultUploader.WSAccess
{
    public class ServiceContext
    {
        private string _URL;
       
        private ServiceContext(string URL)
        {
            _URL = URL;
        }

        private static ServiceContext _serviceContext;
        public static ServiceContext Instance(string URL = null)
        {
            if (_serviceContext == null)
            {
                _serviceContext = new ServiceContext(URL);
            }
            return _serviceContext;
        }
               
        public VaultSvcAccessor VaultSvcAccessor
        {
            get
            {
                return VaultSvcAccessor.Instance(_URL);
            }
        }
    }
}
