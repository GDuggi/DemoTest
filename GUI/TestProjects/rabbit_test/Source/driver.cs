/*
 * [3/23/2015 2:02 PM] Melvin  Ramos: 
RabbitMQ is running[3/23/2015 2:02 PM] Melvin  Ramos: 
http://172.16.143.199:8305

user: guest
pwd: guest
 *                 Console.Error.WriteLine("  <uri> = \"amqp://user:pass@host:port/vhost\"");
amqp://guest:guest@172.16.143.199:5672/
 * 
 * ***************************************************************
 * [3/23/2015 2:06 PM] Melvin  Ramos: 
so you can use it to publish data[3/23/2015 2:07 PM] Melvin  Ramos: 
and your connector will just listen[3/23/2015 2:07 PM] Melvin  Ramos: 
http://topicServerIP:1234/inject

{
posNum = 5432;
quantity = 100;
….
}
 * [3/23/2015 2:07 PM] Melvin  Ramos: 
this is RESTful.[3/23/2015 2:08 PM] Melvin  Ramos: 
the HTTP request is doing a POST
 * ***************************************************************
passing JSON as body[3/23/2015 2:08 PM] Melvin  Ramos: 
here is some background http://www.drdobbs.com/web-development/restful-web-services-a-tutorial/240169069
 * 
*
*/

using System;
using System.Windows.Forms;
namespace NSRabbit_test {
    public static class driver {
        [STAThread]
        public static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new rabbit_testForm());
        }
    }
}
