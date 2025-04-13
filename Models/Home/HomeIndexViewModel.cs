using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using OnlineShopingStore.DAL;
using OnlineShopingStore.Repository;
using PagedList;
using PagedList.Mvc;

namespace OnlineShopingStore.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbMyOnlineShoppingEntitiess contex = new dbMyOnlineShoppingEntitiess();
        public IPagedList<Tbl_Product> ListofProducts { get; set; }

        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page, int? genreId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@search",search ?? (object)DBNull.Value)
            };
            IPagedList<Tbl_Product> data = contex.Database.SqlQuery<Tbl_Product>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {
                ListofProducts = data
            };
        }


    }
}
