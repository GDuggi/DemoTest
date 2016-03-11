using System;
using System.Configuration;

namespace WebServiceRouter.Config
{
    public class WebServiceRouterRuleConfig : ConfigurationElement
    {
        // optional : a SoapAction that is compared to the incoming request 
        [ConfigurationProperty("soapAction", IsRequired = false)]
        public String soapAction
        {
            get { return Convert.ToString(this["soapAction"]); }
            set { this["soapAction"] = value; }
        }

        // optional : a tradingSystemCode that is compared to the incoming request 
        [ConfigurationProperty("tradingSystemCode", IsRequired = false)]
        public String tradingSystemCode
        {
            get { return Convert.ToString(this["tradingSystemCode"]); }
            set { this["tradingSystemCode"] = value; }
        }

        // required
        [ConfigurationProperty("url", IsRequired = true)]
        public String url
        {
            get { return Convert.ToString(this["url"]); }
            set { this["url"] = value; }
        }

        override public String ToString()
        {
            return "Rule( " +
                "soapAction=" + (soapAction == null ? "null" : "'" + soapAction + "'") +
                ", tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'") +
                 ", url=" + (url == null ? "null" : "'" + url + "'") +
                ")";
        }

     
    }

}
