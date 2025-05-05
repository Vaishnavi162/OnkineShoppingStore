using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models
{
    public class UserOrderDetailViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string OrderStatus { get; set; }

        public List<ProductInfo> Products { get; set; }

        public class ProductInfo
        {
            public string ProductName { get; set; }
            public string ProductImage { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}