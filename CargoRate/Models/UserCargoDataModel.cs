
namespace CargoRate.Models
{
    public class UserCargoDataModel
    {
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public string TrailerType { get; set; }
        public decimal? Price { get; set; }
    }
}
