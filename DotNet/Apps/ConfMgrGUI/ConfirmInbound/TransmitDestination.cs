using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using DBAccess;

namespace ConfirmInbound
{
    public class TransmitDestination
    {
        private const string FORM_NAME = "TransmitDestination";
        public TransmitDestinationType Type { get; set; }
        public string Value { get; set; }        

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(Value); }
        }

        public TransmitDestination(TransmitDestinationType type, string value)
        {
            Type = type;
            Value = value;
            Validate();
        }

        private void Validate()
        {
            if (Type == TransmitDestinationType.EMAIL && !string.IsNullOrWhiteSpace(Value))
            {
                try
                {
                    var m = new MailAddress(Value);                    
                }
                catch (FormatException e)
                {
                    throw new ApplicationException(String.Format("'{0}' is not a valid email address:{1}", Value, e.Message) + Environment.NewLine +
                         "Error CNF-530 in " + FORM_NAME + ".Validate(): " + e.Message);
                }
            }
        }

        public TransmitDestination(string value)
        {
            Value = value;
            Type = value.Contains("@") ? TransmitDestinationType.EMAIL : TransmitDestinationType.FAX;
        }

        public bool IsValidNonProdSendToAddress()
        {
            switch (Type)
            {
                case TransmitDestinationType.FAX:
                    return IsValidNonProdFaxMumber();
                    
                case TransmitDestinationType.EMAIL:
                    return IsValidNonProdEmailAddress();

                default:
                    throw new ArgumentOutOfRangeException("Unable to determine the non-Production transmission address for type: " + Type  + "." + Environment.NewLine +
                         "Error CNF-531 in " + FORM_NAME + ".IsValidNonProdSendToAddress().");
            }
        }

        private bool IsValidNonProdFaxMumber()
        {
            return InboundSettings.FaxNumbersDevAllowSendTo.Contains(Value);
        }

        private bool IsValidNonProdEmailAddress()
        {
            var destinations = this.Value.Split(';');
            if (destinations.Count() == 1)
            {
                var parts = Value.Split('@');
                var rightHandSide = parts[1];
                var emailDownloadDevsAllowSendTo = InboundSettings.EmailDownloadDevsAllowSendTo;
                return emailDownloadDevsAllowSendTo.Contains(rightHandSide);
            }
            else
            {
                foreach (var destin in destinations)
                {
                    var parts = destin.Split('@');
                    var rightHandSide = parts[1];
                    var emailDownloadDevsAllowSendTo = InboundSettings.EmailDownloadDevsAllowSendTo;
                    if(emailDownloadDevsAllowSendTo.Contains(rightHandSide) ==false)
                        return false;
                }
                return true;
            }
        }
    }
}
