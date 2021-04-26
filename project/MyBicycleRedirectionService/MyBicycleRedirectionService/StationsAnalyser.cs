using System;
using System.Collections.Generic;
using System.Linq;
using System.Device.Location;
using Newtonsoft.Json;

namespace MyBicycleRedirectionService
{
    public class StationsAnalyser
    {
        ServiceReference1.CacheServiceClient client = new ServiceReference1.CacheServiceClient();
        List<Station> stations;

        public StationsAnalyser()
        {
            stations = JsonConvert.DeserializeObject<List<Station>>(client.getResultFromRequest("https://api.jcdecaux.com/vls/v1/stations?apiKey=f885f76c599180bf47a6cb6a88c3c1ee83d9ed66"));
        }

        public Station getClosestTo(Position position)
        {
            return getClosestToIf(position, (p) => true);
        }

        public Station getClosestToIf(Position position, Predicate<Station> pred)
        {
            var toCompare = new GeoCoordinate(position.Latitude, position.Longitude);
            var sorted = stations.OrderBy((s) => toCompare.GetDistanceTo(new GeoCoordinate(s.position.lat, s.position.lng)));
            for(int i=0;i<stations.Count;i++)
            {
                Station s = sorted.ElementAt(i);
                if (pred(s))
                    return s;
            }
            return null;
        }

        public Station update(Station station)
        {
            return JsonConvert.DeserializeObject<Station>(client.getResultFromRequest("https://api.jcdecaux.com/vls/v3/stations/"+station.number+"?contract="+station.contract_name+"&apiKey=f885f76c599180bf47a6cb6a88c3c1ee83d9ed66"));
        }
    }

    public class StationPosition
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Station
    {
        public string number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public StationPosition position { get; set; }
        public string status { get; set; }
        public int available_bikes { get; set; }
        public int available_bike_stands { get; set; }
    }

    public class Stand
    {
        public int capacity { get; set; }
        public Availabilities availabilities { get; set; }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
    }
}
