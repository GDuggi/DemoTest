using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using Microsoft.Win32;

namespace TradePublisherExample
{
    internal class CommandLineOptions
    {

        [Option('d', "destination", 
                 Required = false, 
                 DefaultValue = "mercuria.confirmsMgr.tradeNotification",
                 HelpText = "Destination to send the trade message to.")]
        public string Destination { get; set; }

        [Option('s', "server",
            Required = false,
            DefaultValue = "cnf02inf01:61613",
            HelpText = "Name:port of the HornetQ message server"
            )]
        public string Server { get; set; }

        [Option('u', "userName",
            Required = false,
            DefaultValue = "cnf.mgr.trade.submitter",
            HelpText = "Username used to log into the hornet message bus")]
        public String UserName { get; set; }

        [Option('p', "password",
            Required = false,
            DefaultValue = "Amphora-123",
            HelpText = "Password used to log into the hornet message bus")]
        public String Password { get; set; }

        [Option('f', "path", 
                Required = true, HelpText = "Path to the file to send to the message bus.")]
        public string FilePath { get; set; }

        

       [HelpOption]
        public
        string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine(
                "TradePublisherExample - an example of how to publish trades to the HornetQ to be processed by Confirmations.");
            usage.Append(HelpText.AutoBuild(this));
            return usage.ToString();
        }


    }
}
