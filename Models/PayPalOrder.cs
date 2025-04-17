using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models
{
    public class PayPalOrder
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string PayerEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}