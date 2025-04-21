using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models
{
    public class PaymentViewModel
    {
        [Required]
        public string CardHolderName { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string CVV { get; set; }
    }

}