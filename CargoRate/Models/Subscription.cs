
namespace CargoRate.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int RateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
