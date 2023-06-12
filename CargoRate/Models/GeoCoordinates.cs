
namespace CargoRate.Models
{
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GeoCoordinatesParameter
    {
        public double minLat { get; set; }
        public double maxLat { get; set; }
        public double minLon { get; set; }
        public double maxLon { get; set; }
    }
}
