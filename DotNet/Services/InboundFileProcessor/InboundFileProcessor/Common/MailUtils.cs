using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace InboundFileProcessor.Common
{
    public static class MailUtils
    {
        public static void SendEmail(string emailType, string emailSubject, string emailBody,string emailAttachFileName)
        {
            if (Properties.Settings.Default.EmailEnabled)
            {
                System.Net.Mail.MailMessage message = null;
                try
                {
                    string emailFrom = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "_" + System.Environment.MachineName + "@" + Properties.Settings.Default.EmailSenderDomain;
                    string emailTo = Properties.Settings.Default.EmailNotifyAddresses;
                    emailSubject = emailType + " - " + emailSubject;
                    emailBody = "Application:  " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name +Environment.NewLine + Environment.NewLine +
                                 emailType+" Message: "+ Environment.NewLine + emailBody;
                    message = new System.Net.Mail.MailMessage(emailFrom,emailTo, emailSubject, emailBody);
                    if (emailAttachFileName != null && emailAttachFileName.Trim() != "")
                    {
                        message.Attachments.Add(new Attachment(emailAttachFileName));
                    }

                    SmtpClient client = new SmtpClient();
                    client.Host = Properties.Settings.Default.EmailHost;
                    client.Port = Properties.Settings.Default.EmailPort;
                    client.Send(message);
                }
                finally
                {
                    message.Dispose();
                }
            }
        }
    }
}
