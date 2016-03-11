using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceRouter.Config
{
    public class WebServiceRouterConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebServiceRouterRuleConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var config = (WebServiceRouterRuleConfig)element;
            return String.Format("{0}|{1}", config.soapAction, config.tradingSystemCode); 
        }

    }
}
