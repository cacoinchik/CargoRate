
namespace CargoRate.Models
{
    public class Cargo
    {
        public int Id { get; set; }
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public string TrailerType { get; set; }
        public decimal Price { get; set; }
    }
}
