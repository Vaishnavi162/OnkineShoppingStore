using OnlineShopingStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models.Home
{
    public class CheckOutDetailsViewModel
    {
        public Tbl_User User { get; set; }
        public List<Item> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}