using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ConfirmInbound
{
    public enum TransmitCallbackAction
    {
        Success, 
        Queued,
        Failed
    }

    public class TransmitCallbackGenerator
    {
        public int XmitRequestId { get; private set; }

        public TransmitCallbackGenerator(int xmitRequestId)
        {
            XmitRequestId = xmitRequestId;
        }

        public string GenerateUrl(TransmitCallbackAction action)
        {
            var queryStringParts = HttpUtility.ParseQueryString(string.Empty);
            queryStringParts["xmitRequestId"] = Convert.ToString(XmitRequestId);
            queryStringParts["action"] = Convert.ToString(action);
            // ToDo - add tokens for the rest of the parameters which the Transmission Gateway will fill in for the response.

            return String.Format("{0}?{1}", InboundSettings.TransmissionGatewayCallback,
                queryStringParts);

        }
    }
}
