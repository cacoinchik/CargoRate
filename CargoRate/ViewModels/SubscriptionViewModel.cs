
using System.ComponentModel.DataAnnotations;

namespace CargoRate.ViewModels
{
    public class SubscriptionViewModel
    {
        [Required]
        public string RateName { get; set; }

    }
}
