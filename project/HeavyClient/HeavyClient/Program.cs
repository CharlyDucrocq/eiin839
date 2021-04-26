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
            Position start = new Position();
            Position end = new Position();
            //4.909121,45.735397
            start.Latitude = 45.735397;
            start.Longitude = 4.909121;
            //4.855487,45.746808
            end.Latitude = 45.746808;
            end.Longitude = 4.855487;

            ////4.909121,45.735397
            //Console.WriteLine("Startpoint : Type a longitude coordonate, and then press Enter");
            //start.Longitude = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("Startpoint : Type a latitude coordonate, and then press Enter");
            //start.Latitude = Convert.ToDouble(Console.ReadLine());
            ////4.855487,45.746808
            //Console.WriteLine("Endpoint : Type a longitude coordonate, and then press Enter");
            //end.Longitude = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("Endpoint : Type a latitude coordonate, and then press Enter");
            //end.Latitude = Convert.ToDouble(Console.ReadLine());

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
    }
}
