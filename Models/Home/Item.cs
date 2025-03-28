using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopingStore.DAL;

namespace OnlineShopingStore.Models.Home
{
    public class Item
    {
        public Tbl_Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Product
    {
        public string ProductName { get; set; }
    }
}