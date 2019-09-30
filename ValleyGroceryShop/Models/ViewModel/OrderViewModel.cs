using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Firstname Required")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname Required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Required")]
        public string Phone { get; set; }

        public string Total { get; set; }
        public string OrderDate { get; set; }
        public string DeliveredStatus { get; set; }
    }
}