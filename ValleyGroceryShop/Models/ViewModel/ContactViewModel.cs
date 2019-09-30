using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models.ViewModel
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject Required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Phone Required")]
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}