using System;
using ServiceReference2;

namespace HeavyClient
{
    class Program
    {

        private static ServiceReference2.RedirectionServiceClient service = new ServiceReference2.RedirectionServiceClient();
        static void Main(string[] args)
        {
            Console.WriteLine("----------- START ------------");
            Position start = askForPosition("Please enter start's coordinates : (Example : \"4.909121,45.735397\")");
            Position end = askForPosition("Please enter end's coordinates : (Example : \"4.855487,45.746808\")");

            Edge[] got = service.GetWayToGoAsync(start, end).Result;

            for (int i = 0; i < got.Length; i++)
            {
                if (i % 2 != 0) Console.WriteLine("\nTake a bike a the station.\n");
                string prefix = (i % 2 == 0) ? "At foot : " : "On bike : ";
                foreach (string instruction in got[i].instructions)
                {
                    Console.WriteLine(prefix + instruction);
                }
                if (i % 2 != 0) Console.WriteLine("\nPut back the bike a the station.\n");
            }
            Console.WriteLine("----------- END ------------");
        }

        public static Position askForPosition(string ask)
        {
            Console.WriteLine(ask);
            var result = new Position();

            string input;
            while((input = Console.ReadLine()) != null)
            {
                string[] values = input.Split(",");
                if(values.Length!=2)
                {
                    Console.WriteLine("Error : the input must contains two values like \"1.23,43.21\"... " + input + " doesn't match");
                    continue;
                }
                double value1;
                if (!double.TryParse(values[0].Replace(".", ","), out value1))
                {
                    Console.WriteLine("Error : the input must be two double values like \"1.23,43.21\"..." + values[0] + " is not a double");
                    continue;
                }
                double value2;
                if (!double.TryParse(values[1].Replace(".",","), out value2))
                {
                    Console.WriteLine("Error : the input must be two double values like \"1.23,43.21\"..." + values[1] + " is not a double");
                    continue;
                }
                result.Longitude = value1;
                result.Latitude = value2;
                break;
            }

            return result;
        }
    }
}
