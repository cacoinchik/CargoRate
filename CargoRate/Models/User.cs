using Microsoft.AspNetCore.Identity;

namespace CargoRate.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
