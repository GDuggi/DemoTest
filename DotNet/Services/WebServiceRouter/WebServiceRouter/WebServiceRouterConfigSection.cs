using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceRouter.Config
{
    public class WebServiceRouterConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ruleConfigurations", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WebServiceRouterConfigCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public WebServiceRouterConfigCollection RuleConfigurations
        {
            get { return (WebServiceRouterConfigCollection)base["ruleConfigurations"]; }
        }

        public IEnumerable<WebServiceRouterConfigCollection> ResponseConfigs
        {
            get { return RuleConfigurations.Cast<WebServiceRouterConfigCollection>(); }
        }

        [ConfigurationProperty("listenMask", IsRequired = true)]
        public String listenMask
        {
            get { return Convert.ToString(this["listenMask"]); }
            set { this["listenMask"] = value; }
        }
    }
}
