using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasicServerHTTPlistener
{
    internal class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                switch (args.Length){
                    case 0 :
                            HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/contracts?apiKey=f885f76c599180bf47a6cb6a88c3c1ee83d9ed66");
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Object[] contracts = JsonSerializer.Deserialize<Contract[]>(responseBody);

                            // Above three lines can be replaced with new helper method below
                            // string responseBody = await client.GetStringAsync(uri);
                            foreach (Contract contract in contracts)
                            {
                                Console.WriteLine(contract);
                            }
                        break;
                    case 1:
                        HttpResponseMessage response2 = await client.GetAsync("https://api.jcdecaux.com/vls/v1/stations?contract=" + args[0] + "&apiKey=f885f76c599180bf47a6cb6a88c3c1ee83d9ed66");
                        response2.EnsureSuccessStatusCode();
                        string responseBody2 = await response2.Content.ReadAsStringAsync();
                        Object[] stations = JsonSerializer.Deserialize<Contract[]>(responseBody2);

                        // Above three lines can be replaced with new helper method below
                        // string responseBody = await client.GetStringAsync(uri);
                        foreach (Contract contract in stations)
                        {
                            Console.WriteLine(contract);
                        }
                        break;
                    case 2:
                        HttpResponseMessage response3 = await client.GetAsync("https://api.jcdecaux.com/vls/v1/stations/"+args[1]+"?contract=" + args[0] + "&apiKey=f885f76c599180bf47a6cb6a88c3c1ee83d9ed66");
                        response3.EnsureSuccessStatusCode();
                        string responseBody3 = await response3.Content.ReadAsStringAsync();

                        Console.WriteLine(responseBody3);
                        break;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
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