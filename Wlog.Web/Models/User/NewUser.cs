using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.User
{
    public class NewUser
    {
         [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        public NewUser()
        {
            this.IsAdmin = false;
        }
    }
}