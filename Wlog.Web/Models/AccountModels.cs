using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Wlog.Web.Models
{
   

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password corrente")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nuova password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma nuova password")]
        [Compare("NewPassword", ErrorMessage = "La nuova password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Memorizza account")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
