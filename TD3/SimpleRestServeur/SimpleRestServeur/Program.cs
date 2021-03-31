using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceReference1;

namespace BasicServerHTTPlistener
{
    internal class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            CalculatorSoapClient calculator = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);

            Console.WriteLine("1+2=" + calculator.AddAsync(1,2).Result);
        }

        public class Contract : Object
        {
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
    }
}