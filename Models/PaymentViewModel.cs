using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopingStore.Models
{
    public class PaymentViewModel
    {

        [Required]
        public string CardHolderName { get; set; }


        [Required(ErrorMessage = "Card number is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number must be 16 digits")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be numeric and 16 digits long")]
        public string CardNumber { get; set; }


        [Required]
        public string Status { get; set; } = "Pending";


        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string CVV { get; set; }

        //public string Status { get; set; }

        //public List<SelectListItem> StatusList { get; set; }
    }

}