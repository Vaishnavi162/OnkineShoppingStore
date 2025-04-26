using OnlineShopingStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models
{
    public class PaymentOrderViewModel
    {
        public int PaymentID { get; set; }
        public int? UserID { get; set; }
        public string ProductName { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }

        // Optional: Include user info
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string Status { get; set; }
        public string OrderStatus { get; set; }


    }
}