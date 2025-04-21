using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopingStore.DAL;
using OnlineShopingStore.Models.Home;
using PagedList;

namespace OnlineShopingStore.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntitiess ctx = new dbMyOnlineShoppingEntitiess();
        //public ActionResult Index(string search, int? page)
        //{
        //    HomeIndexViewModel model = new HomeIndexViewModel();
        //    model = model.CreateModel(search, 4, page);
        //    return View(model);
        //}
        //public ActionResult Index(string search, int? genreId, int? page)
        //{
        //    HomeIndexViewModel model = new HomeIndexViewModel();
        //    model = model.CreateModel(search, 4, page, genreId);

        //    var genres = ctx.Tbl_Genre.ToList();
        //    ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName", genreId);


        //    ViewBag.SelectedGenre = genreId;

        //    return View(model);
        //}

        public ActionResult Index(string search, string genre, int? page)
        {
            using (var db = new dbMyOnlineShoppingEntitiess())
            {
                var genres = db.Tbl_Category.Select(c => c.CategoryName).Distinct().ToList();

                // Join products and categories manually
                var productsQuery = from p in db.Tbl_Product
                                    join c in db.Tbl_Category on p.CategoryId equals c.CategoryId
                                    select new
                                    {
                                        Product = p,
                                        CategoryName = c.CategoryName
                                    };

                // Filter by genre
                if (!string.IsNullOrEmpty(genre))
                {
                    productsQuery = productsQuery.Where(x => x.CategoryName == genre);
                }

                // Filter by search term (on ProductName or Description)
                if (!string.IsNullOrEmpty(search))
                {
                    productsQuery = productsQuery.Where(x =>
                        x.Product.ProductName.Contains(search) ||
                        x.Product.Description.Contains(search));
                }

                // Extract products
                var productList = productsQuery.Select(x => x.Product).ToList();

                // Pagination
                int pageSize = 8;
                int pageNumber = page ?? 1;
                var pagedProducts = productList.ToPagedList(pageNumber, pageSize);

                // Create view model
                var viewModel = new HomeIndexViewModel
                {
                    ListofProducts = pagedProducts,
                    Genres = genres,
                   
                };

                return View(viewModel);
            }
        }



        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            // Pull the cart from session (or make it empty if null)
            var cart = Session["cart"] as List<Item> ?? new List<Item>();

            // Compute grand total
            decimal grandTotal = (decimal)cart.Sum(i => i.Quantity * i.Product.Price);

            // Either pass via ViewBag:
            ViewBag.GrandTotal = grandTotal;

            // Or use a simple view‑model:
            // var vm = new CartViewModel { Items = cart, Total = grandTotal };
            // return View(vm);

            return View(cart);
        }

        public ActionResult CheckOutDetails()
        {
            // 1) Get cart from session
            var cart = Session["cart"] as List<Item> ?? new List<Item>();

            // 2) Compute grand total
            decimal total = (decimal)cart.Sum(i => i.Quantity * i.Product.Price);

            // 3) Fetch logged‑in user
            Tbl_User user = null;
            if (Session["UserId"] != null)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                user = ctx.Tbl_User.SingleOrDefault(u => u.UserID == userId);
            }

            // 4) Pack into view‑model
            var vm = new CheckOutDetailsViewModel
            {
                User = user,
                CartItems = cart,
                GrandTotal = total
            };

            return View(vm);
        }
        //public ActionResult DecreaseQty(int productId)
        //{
        //    if (Session["cart"] != null)
        //    {
        //    List<Item> cart = (List<Item>)Session["cart"];
        //    var product = ctx.Tbl_Product.Find(productId);
        //    foreach (var item in cart)
        //    {
        //        if (item.Product.ProductId == productId)
        //        {
        //            int prevQty = item.Quantity;
        //                if (prevQty > 0)
        //                {
        //                    cart.Remove(item);
        //                    cart.Add(new Item()
        //                    {
        //                        Product = product,
        //                        Quantity = prevQty - 1
        //                    });
        //                }
        //                break;
        //            }
        //        }
        //        Session["cart"] = cart;
        //    }

        //    return Redirect("CheckOut");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DecreaseQty(int productId, string returnUrl)
        {
            var cart = Session["cart"] as List<Item>;
            if (cart != null)
            {
                // Find the item
                var item = cart.FirstOrDefault(x => x.Product.ProductId == productId);
                if (item != null)
                {
                    item.Quantity--;               // decrement
                    if (item.Quantity <= 0)        // if 0 or less
                        cart.Remove(item);         // remove from list
                }

                // Update session
                Session["cart"] = cart;
            }

            // Redirect back
            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("CheckOut");
        }


        [HttpPost]
        public ActionResult AddToCart(int productId, int? quantity)
        {
            int qty = quantity ?? 1;

            if (Session["Fullname"] == null)
            {
                TempData["LoginRequired"] = "Please login to add items to your cart.";
                return RedirectToAction("Login", "Account");
            }

            List<Item> cart = Session["cart"] as List<Item> ?? new List<Item>();
            var product = ctx.Tbl_Product.Find(productId);

            var existingItem = cart.FirstOrDefault(item => item.Product.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += qty;
                return RedirectToAction("CheckOut");
            }
            else
            {
                cart.Add(new Item
                {
                    Product = product,
                    Quantity = qty
                });
            }

            Session["cart"] = cart;
            TempData["CartSuccess"] = "Your product has been added to the cart.";

            return RedirectToAction("Index");
        }


        



        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach(var item in cart)
            {
                if(item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }
        public ActionResult Details(int productId)
        {
            // Get product from database
            var product = ctx.Tbl_Product.Find(productId);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
      
    }
}