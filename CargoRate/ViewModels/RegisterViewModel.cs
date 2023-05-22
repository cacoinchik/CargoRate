using System.ComponentModel.DataAnnotations;

namespace CargoRate.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        [Display(Name = "Ваше имя")]
        public string Name { get; set; }

        [Display(Name = "Ваша фамилия")]
        [Required(ErrorMessage = "Введите вашу фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите ваш логин")]
        [Display(Name = "Ваш логин")]
        public string UserName { get; set; }

        [Display(Name="Название компании")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторите ваш пароль")]
        public string ConfirmPassword { get; set; }
    }
}
