using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApothecaricApi.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "{0} character limit of {1} exceeded.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
