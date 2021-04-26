using System;
using System.Collections.Generic;
using System.Text.Json;
using MyBicycleRedirectionService;
using Newtonsoft.Json;

namespace MyBicycleRedirectionService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.


    public class RedirectionService : IRedirectionService
    {
        ServiceReference1.CacheServiceClient cache = new ServiceReference1.CacheServiceClient();
        StationsAnalyser stations = new StationsAnalyser();

        public List<Edge> GetWayToGoSimple(double startLng, double startLat, double endLng, double endLat)
        {
            Position start = new Position();
            Position end = new Position();
            start.Latitude = startLat;
            start.Longitude = startLng;
            end.Latitude = endLat;
            end.Longitude = endLng;
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return GetWayToGo(start, end);
        }

        public List<Edge> GetWayToGo(Position start, Position end)
        {
            var result = new List<Edge>();

            Predicate<Station> checkIfBikeAvailable = (station) =>
            {
                Station s = stations.update(station);
                return station.available_bikes > 0;
            };
            Predicate<Station> checkIfBikeStandAvailable = (station) =>
            {
                Station s = stations.update(station);
                return station.available_bike_stands > 0;
            };

            Station stationStart = stations.getClosestToIf(start, checkIfBikeAvailable);
            Station stationEnd = stations.getClosestToIf(end, checkIfBikeStandAvailable);
            Edge startToEnd = stringToEdge(requestEdgeInfoAsync(start, end, false));
            if (stationStart != null && stationEnd != null)
            {
                Edge startToStationStart = stringToEdge(requestEdgeInfoAsync(start, stationPosToPos(stationStart), false));
                Edge stationStartToStationEnd = stringToEdge(requestEdgeInfoAsync(stationPosToPos(stationStart), stationPosToPos(stationEnd), true));
                Edge stationEndToEnd = stringToEdge(requestEdgeInfoAsync(stationPosToPos(stationEnd), end, false));

                if (startToEnd.Duration <= startToStationStart.Duration + stationStartToStationEnd.Duration + stationEndToEnd.Duration)
                {
                    startToEnd.Status = MoveType.Default;
                    result.Add(startToEnd);
                }
                else
                {
                    startToStationStart.Status = MoveType.Default;
                    result.Add(startToStationStart);
                    stationStartToStationEnd.Status = MoveType.Bike;
                    result.Add(stationStartToStationEnd);
                    stationEndToEnd.Status = MoveType.Default;
                    result.Add(stationEndToEnd);
                }
            } else
            {
                startToEnd.Status = MoveType.Default;
                result.Add(startToEnd);
            }

            return result;
        }

        private string requestEdgeInfoAsync(Position start, Position end, bool inBike) {
            var profile = inBike ? "cycling-regular" : "foot-walking";
            return cache.getResultFromRequest("https://api.openrouteservice.org/v2/directions/"+ profile +
                "?api_key=5b3ce3597851110001cf624852f73d17a81b40a59c50c7a3c311b95b" +
                "&start=" + start.Longitude.ToString().Replace(",", ".") + "," + start.Latitude.ToString().Replace(",", ".") + 
                "&end=" + end.Longitude.ToString().Replace(",", ".") + "," + end.Latitude.ToString().Replace(",", "."));

        }

        private Edge stringToEdge(string input)
        {
            GeoResponse o = JsonConvert.DeserializeObject<GeoResponse>(input);

            Edge toAdd = new Edge();
            toAdd.Start = new Position();
            toAdd.End = new Position();
            toAdd.Start.Latitude = o.features[0].bbox[0];
            toAdd.Start.Longitude = o.features[0].bbox[1];
            toAdd.End.Latitude = o.features[0].bbox[2];
            toAdd.End.Longitude = o.features[0].bbox[3];
            toAdd.Duration = o.features[0].properties.segments[0].duration;
            toAdd.Lentgh = o.features[0].properties.segments[0].distance;
            toAdd.instructions = new string[o.features[0].properties.segments[0].steps.Length];
            toAdd.coordinates = o.features[0].geometry.coordinates;
            int i = 0;
            foreach(Step step in o.features[0].properties.segments[0].steps)
            {
                toAdd.instructions[i] = step.instruction;
                i++;
            }
            return toAdd;
        }

        private static Position stationPosToPos(Station station)
        {
            Position result = new Position();
            result.Latitude = station.position.lat;
            result.Longitude = station.position.lng;
            return result;
        }
    }

    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public string instruction { get; set; }
    }

    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public Step[] steps { get; set; }
    }

    public class Propeties
    {
        public Segment[] segments { get; set; }
    }
    public class Geometrie
    {
        public double[][] coordinates { get; set; }
    }

    public class Feature
    {
        public Propeties properties { get; set; }
        public double[] bbox { get; set; }
        public Geometrie geometry { get; set; }
    }

    public class GeoResponse
    {
        public Feature[] features { get; set; }
    }
}
