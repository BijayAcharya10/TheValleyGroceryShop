using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
     
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Fullname { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public string Usertype { get; set; }

    }
}