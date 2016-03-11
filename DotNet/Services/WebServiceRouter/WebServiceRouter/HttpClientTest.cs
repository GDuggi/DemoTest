using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebServiceRouter
{
    class HttpClientTest
    {
        public static void xMain(String[] args)
        {
            Run().Wait();

            Console.ReadLine();
        }

        public static async Task Run()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri( "http://localhost:63002/Counterparty.svc" );
            //client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "text/xml; charset=utf-8") );
            String payload = 
@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:int=""http://cnf/Integration"">
   <soapenv:Header/>
   <soapenv:Body>
      <int:getAgreementList>
         <!--Optional:-->
         <int:request>
            <int:bookingCompanyShortName>?</int:bookingCompanyShortName>
            <int:cptyShortName>?</int:cptyShortName>
            <int:tradingSystemCode>?</int:tradingSystemCode>
         </int:request>
      </int:getAgreementList>
   </soapenv:Body>
</soapenv:Envelope>";

            var request = new HttpRequestMessage();
            request.Content = new StringContent(payload,  Encoding.UTF8, "text/xml" );
            request.Method = HttpMethod.Post;
            request.Headers.Add( "SOAPAction", @"""http://cnf/Integration/Counterparty/getAgreementList""");


            HttpResponseMessage response = await client.SendAsync(request);
                        
             // Check that response was successful or throw exception
             response.EnsureSuccessStatusCode();

             // Read response asynchronously and write out top facts for each country
             String s = await response.Content.ReadAsStringAsync();

             Console.WriteLine("Web Service returned " + s);
                                
        }
    }
}
