using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopingStore.Models
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

    }
}