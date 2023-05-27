using System.ComponentModel.DataAnnotations;

namespace CargoRate.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Ваше имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Ваша фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Название вашей компании")]
        public string? CompanyName { get; set; }
    }
}
