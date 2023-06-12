using CargoRate.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CargoRate.Service
{
    public class GeocodingService
    {
        public async Task<GeoCoordinates> Code(string address)
        {
            using(var client = new HttpClient())
            {
                var apiKey = "b4a1c746-72e9-4327-be9e-63360beab195";
                var url = $"https://geocode-maps.yandex.ru/1.x/?apikey={apiKey}&format=json&geocode={Uri.EscapeDataString(address)}";

                var response=await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var json=JObject.Parse(jsonString);

                    var point = json.SelectToken("$.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos")?.ToString()?.Split(' ');
                    if(point!=null && point.Length==2 
                        && double.TryParse(point[1], NumberStyles.Any,CultureInfo.InvariantCulture, out var lat)
                        && double.TryParse(point[0],NumberStyles.Any,CultureInfo.InvariantCulture,out var lng))
                    {
                        GeoCoordinates coordinates = new GeoCoordinates
                        {
                            Latitude = lat,
                            Longitude = lng
                        };
                        return coordinates;
                    }
                }

                return new GeoCoordinates { Latitude=0,Longitude=0};
            }
        }

        public GeoCoordinatesParameter GetGeoCoordinatesParameter(GeoCoordinates coordinates)
        {
            var radius = 100000;
            var earthRadius = 6371;

            var latDistance = (radius / 1000.0) / earthRadius;
            var lngDistance = latDistance / Math.Cos(coordinates.Latitude * Math.PI / 180);

            var minLat = coordinates.Latitude - (latDistance * 180 / Math.PI);
            var maxLat = coordinates.Latitude + (latDistance * 180 / Math.PI);
            var minLon = coordinates.Longitude - (lngDistance * 180 / Math.PI);
            var maxLon = coordinates.Longitude + (lngDistance * 180 / Math.PI);

            return new GeoCoordinatesParameter { maxLat = maxLat, minLat = minLat, maxLon = maxLon, minLon=minLon };
        }
    }
}
