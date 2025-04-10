
using Newtonsoft.Json;
using OnlineShopingStore.DAL;
using OnlineShopingStore.Models;
using OnlineShopingStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopingStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();


        //public ActionResult AdminLogin()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult AdminLogin(string username, string password)
        //{
        //    if (username == "admin" && password == "admin123") // Example logic
        //    {
        //        Session["AdminUsername"] = username;
        //        return RedirectToAction("Dashboard", "Admin");
        //    }

        //    ViewBag.Message = "Invalid credentials!";
        //    return View();
        //}
        //public ActionResult Logout()
        //{
        //    Session.Clear(); // or Session.Abandon();
        //    return RedirectToAction("AdminLogin", "Admin");
        //}

        // GET: Admin/Login
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
            // Dummy check — replace with your DB logic
            if (username == "admin" && password == "admin123")
            {
                Session["Admin"] = username;
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid login details";
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("AdminLogin");

            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("AdminLogin");
        }
        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list= new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            return list;
        }
        
        //public ActionResult Dashboard()
        //{
        //    return View();
        //}

        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
        }
        [HttpPost]
        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }

        public ActionResult UpdateCategory(int? categoryId)
        {
            CategoryDetail cd;
            if (categoryId != null && categoryId != 0)
            {
                cd = JsonConvert.DeserializeObject<CategoryDetail>(
                    JsonConvert.SerializeObject(
                        _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(categoryId.Value)
                    )
                );
            }
            else
            {
                cd = new CategoryDetail();
            }
            return View("UpdateCategory", cd);

        }


        public ActionResult CategoryEdit(int catId)
        {
          
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(catId)); 
        }
        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {
           
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
        }
        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct()); 
        }
        [HttpGet]
        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productId)); 
        }
        [HttpPost]
        public ActionResult ProductEdit(Tbl_Product tbl, HttpPostedFileBase File)
        {
            string pic = null;
            if (File != null)
            {
                pic = System.IO.Path.GetFileName(File.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                File.SaveAs(path);
            }
            tbl.ProductImage = File != null ? pic : tbl.ProductImage;
            tbl.ModifiedData = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Update(tbl);
            return RedirectToAction("Product"); 
        }

        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        //public ActionResult ProductAdd(int? productId)
        //{
        //    if (productId == null)
        //    {
        //        ViewBag.Message = "Product ID is not provided.";
        //    }
        //    else
        //    {
        //        // Logic to handle product addition using productId
        //        ViewBag.Message = $"Product ID: {productId}";
        //    }

        //    return View();
        //}
        [HttpPost]
        public ActionResult ProductAdd(Tbl_Product tbl,HttpPostedFileBase File)
        {
            string pic = null;
            if(File != null)
            {
                pic = System.IO.Path.GetFileName(File.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                File.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Product");
        }
        
    }
}