using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyBicycleRedirectionService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRedirectionService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/getRoute", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Edge> GetWayToGo(Position start, Position end);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/way?startLng={startLng}&startLat={startLat}&endLng={endLng}&endLat={endLat}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Edge> GetWayToGoSimple(double startLng, double startLat, double endLng, double endLat);
    }

    [DataContract]
    public class Edge
    {
        [DataMember]
        public Position Start { set; get; }

        [DataMember]
        public Position End { set; get; }

        [DataMember]
        public double Lentgh { set; get; }

        [DataMember]
        public double Duration { set; get; }

        [DataMember]
        public string[] instructions { set; get; }

        [DataMember]
        public double[][] coordinates { set; get; }

        [DataMember]
        public MoveType Status { set; get; }
    }

    [DataContract]
    public class Position
    {
        [DataMember]
        public double Latitude
        {
            get; set;
        }

        [DataMember]
        public double Longitude
        {
            get; set;
        }
    }

    [DataContract]
    public enum MoveType
    {
        [EnumMember]
        Bike,
        [EnumMember]
        Default
    }
}
