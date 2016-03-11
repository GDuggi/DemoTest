using System;
using System.IO;
using Aff.Sif.MessageBusClient;
using CommandLine;

namespace TradePublisherExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new CommandLineOptions();
                var parser = new Parser(new ParserSettings(Console.Error));
                if (parser.ParseArguments(args, options))
                {
                    string fileContents = File.ReadAllText(options.FilePath);
                    Console.WriteLine("Connecting to message bus at {0} as {1}", options.Server, options.UserName);
                    // Connecting to the message bus is fairly expensive so I would recommend doing it once 
                    // at the beginning of your program. It is configured to reconnect if the connection drops by default.
                    using(var messageBus = MessageBusFactory.Instance.GetMessageBus(
                            options.Server,
                            options.UserName,
                            options.Password
                            ))
                    {
                        Console.WriteLine("Sending contents of {0}...", options.FilePath);

                        // Here is where we send the payload to HornetQ.
                        // The message recievr 
                        messageBus.Publish(options.Destination, fileContents);
                        Console.WriteLine("Sent.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured:" + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }        
    }
}
