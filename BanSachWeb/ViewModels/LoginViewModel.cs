using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or phone number is required.")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}