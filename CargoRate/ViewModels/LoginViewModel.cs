﻿using System.ComponentModel.DataAnnotations;

namespace CargoRate.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }

        public string? returnUrl { get; set; }
    }
}
