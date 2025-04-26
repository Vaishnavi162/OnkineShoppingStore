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

        [Required(ErrorMessage = "Card Number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card Number must be exactly 16 digits.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card Number must be exactly 16 digits.")]
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