using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS_NCAS.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }  

        [Required]
        public string LastName { get; set; }  

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits and contain no special characters.")]
        public string ContactNumber { get; set; }
    }
}