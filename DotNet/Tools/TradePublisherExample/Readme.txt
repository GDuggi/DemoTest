
Overview:
This program is a simple example illustrating how to connect to the message bus and publish a trade so it can be picked up by confirms.
THe program leverages a wrapper library which Amphora has written to make connecting to the message bus fairly painless. 

The majority of the arguments have been given reasonable default values with the exception of --path which is the test file that you are trying to send.

Here are the command line parameters:
-d, --destination    (Default: mercuria.confirmsMgr.tradeNotification)
                       Destination to send the trade message to.
-s, --server         (Default: cnf01inf02:61613) Name:port of the HornetQ message bus. 
-u, --userName       (Default: cnf.mgr.trade.submitter) Username used to log into the HornetQ message bus
-p, --password       (Default: Amphora-123) Password used to log into the HornetQ message bus
-f, --path           Required. Path to the file to send to the message bus.

How to Use:
1. Modify or make a copy of the test XML in ExampleTrades. Note - Confirmations Manager requires that the combination of the tags TradingSystemCode, TradeSystemTicket be a unique value.
2. Run the program passing the path to the file you want use to as the body of the message.
3. Open Confirmations Manager and verify that the trade shows up.
