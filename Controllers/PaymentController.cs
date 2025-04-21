using OnlineShopingStore;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineShopingStore.Models;
using OnlineShopingStore.DAL;
using OnlineShopingStore.Repository; // adjust namespace

namespace YourProject.Controllers
{

    public class PaymentController : Controller
    {
        private GenericUnitOfWork unitOfWork = new GenericUnitOfWork();

        [HttpGet]
        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Payment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Payment payment = new Tbl_Payment
                {
                    UserId = User.Identity.Name, // Or set user ID from session if needed
                    CardHolderName = model.CardHolderName,
                    CardNumber = model.CardNumber,
                    ExpiryDate = model.ExpiryDate,
                    CVV = model.CVV,
                    PaymentDate = DateTime.Now
                };

                unitOfWork.GetRepositoryInstance<Tbl_Payment>().Add(payment);

                TempData["Message"] = "Payment Successful!";
                return RedirectToAction("SuccessView");
            }

            return View(model);
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        //public ActionResult PaymentSuccess()
        //{
        //    return View();
        //}
    }
}
//    private Payment payment;

//    public ActionResult PaymentWithPaypal()
//    {
//        APIContext apiContext = PaypalConfiguration.GetAPIContext();

//        try
//        {
//            string payerId = Request.Params["PayerID"];
//            if (string.IsNullOrEmpty(payerId))
//            {
//                // First call – redirect to PayPal
//                string guid = Convert.ToString((new Random()).Next(100000));
//                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPaypal?guid=" + guid;

//                var createdPayment = CreatePayment(apiContext, baseURI, guid);

//                var links = createdPayment.links.GetEnumerator();
//                while (links.MoveNext())
//                {
//                    Links lnk = links.Current;
//                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//                    {
//                        Session.Add(guid, createdPayment.id); // Store paymentId
//                        return Redirect(lnk.href); // Go to PayPal
//                    }
//                }

//                return View("FailureView");
//            }
//            else
//            {
//                // Return from PayPal
//                var guid = Request.Params["guid"];
//                var paymentId = Session[guid] as string;

//                if (string.IsNullOrEmpty(paymentId))
//                    return View("FailureView");

//                var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

//                if (executedPayment.state.ToLower() != "approved")
//                    return View("FailureView");

//                return View("SuccessView");
//            }
//        }
//        catch (Exception ex)
//        {
//            ViewBag.ErrorMessage = ex.Message;
//            return View("FailureView");
//        }
//    }

//    private Payment CreatePayment(APIContext apiContext, string redirectURL, string guid)
//    {
//        var itemList = new ItemList() { items = new List<Item>() };
//        decimal totalAmountINR = 0;

//        if (Session["cart"] != null)
//        {
//            List<OnlineShopingStore.Models.Home.Item> cart = (List<OnlineShopingStore.Models.Home.Item>)Session["cart"];

//            foreach (var item in cart)
//            {
//                decimal priceINR = (decimal)item.Product.Price;
//                int quantity = item.Quantity;

//                itemList.items.Add(new Item()
//                {
//                    name = item.Product.ProductName,
//                    currency = "USD", // Always USD for PayPal
//                    price = (priceINR / 83).ToString("F2"), // Convert INR to USD
//                    quantity = quantity.ToString(),
//                    sku = "sku"
//                });

//                totalAmountINR += priceINR * quantity;
//            }
//        }

//        decimal totalAmountUSD = totalAmountINR / 83;

//        var payer = new Payer() { payment_method = "paypal" };

//        var redirUrls = new RedirectUrls()
//        {
//            cancel_url = redirectURL + "&cancel=true",
//            return_url = redirectURL
//        };

//        var amount = new Amount()
//        {
//            currency = "USD",
//            total = totalAmountUSD.ToString("F2")
//        };

//        var transactionList = new List<Transaction>
//        {
//            new Transaction
//            {
//                description = "Mobile purchase",
//                invoice_number = Guid.NewGuid().ToString(),
//                amount = amount,
//                item_list = itemList
//            }
//        };

//        var payment = new Payment()
//        {
//            intent = "sale",
//            payer = payer,
//            transactions = transactionList,
//            redirect_urls = redirUrls
//        };

//        return payment.Create(apiContext);
//    }

//    private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
//    {
//        var paymentExecution = new PaymentExecution() { payer_id = payerId };
//        payment = new Payment() { id = paymentId };
//        return payment.Execute(apiContext, paymentExecution);
//    }





//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Newtonsoft.Json;
//using OnlineShopingStore.DAL;
//using OnlineShopingStore.Models;
//using PayPal.Api;

//namespace OnlineShopingStore.Controllers
//{
//    public class PaymentController : Controller
//    {
//        private PayPal.Api.Payment payment;
//        private decimal totalAmount;

//        //public ActionResult PaymentWithPaypal()
//        //{
//        //    APIContext apiContext = PaypalConfiguration.GetAPIContext();
//        //    try
//        //    {
//        //        string payerId = Request.Params["PayerID"];
//        //        if (string.IsNullOrEmpty(payerId))
//        //        {
//        //            // Redirecting to PayPal
//        //            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPaypal?";
//        //            string guid = Convert.ToString((new Random()).Next(100000));
//        //            var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, guid);

//        //            var links = createdPayment.links.GetEnumerator();
//        //            while (links.MoveNext())
//        //            {
//        //                Links lnk = links.Current;
//        //                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//        //                {
//        //                    return Redirect(lnk.href); // Redirect to PayPal login
//        //                }
//        //            }
//        //        }
//        //        else
//        //        {
//        //            var guid = Request.Params["guid"];
//        //            var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

//        //            if (executedPayment.state.ToLower() != "approved")
//        //            {
//        //                return View("FailureView");
//        //            }
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return View("FailureView");
//        //    }
//        //    return View("SuccessView");
//        //}
//        public ActionResult PaymentWithPaypal()
//        {
//            APIContext apiContext = PaypalConfiguration.GetAPIContext();

//            try
//            {
//                string payerId = Request.Params["PayerID"];
//                if (string.IsNullOrEmpty(payerId))
//                {
//                    // First request: redirect to PayPal
//                    string guid = Convert.ToString((new Random()).Next(100000));
//                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPaypal?guid=" + guid;

//                    var createdPayment = CreatePayment(apiContext, baseURI, guid);

//                    var links = createdPayment.links.GetEnumerator();
//                    while (links.MoveNext())
//                    {
//                        Links lnk = links.Current;
//                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//                        {
//                            // Save the payment ID using guid
//                            Session.Add(guid, createdPayment.id);
//                            return Redirect(lnk.href);
//                        }
//                    }

//                    return View("FailureView");
//                }
//                else
//                {
//                    // Second request: after PayPal redirects back
//                    var guid = Request.Params["guid"];
//                    var paymentId = Session[guid] as string;

//                    if (string.IsNullOrEmpty(paymentId))
//                    {
//                        return View("FailureView");
//                    }

//                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

//                    if (executedPayment.state.ToLower() != "approved")
//                    {
//                        return View("FailureView");
//                    }

//                    return View("SuccessView");
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Message = ex.Message;
//                return View("FailureView");
//            }
//        }


//        //private Payment CreatePayment(APIContext apiContext, string redirectURL, string guid)

//        //{
//        //    var itemList = new ItemList() { items = new List<Item>() };

//        //    if (Session["cart"] != null)
//        //    {
//        //        List<Models.Home.Item> cart = (List<Models.Home.Item>)Session["cart"];
//        //        // Assuming INR price
//        //        decimal totalAmountINR = totalAmount;

//        //        // For demo, use static conversion rate (e.g., ₹83 = $1)
//        //        decimal conversionRate = 83m;

//        //        // Convert INR to USD
//        //        decimal totalAmountUSD = totalAmountINR / conversionRate;

//        //        foreach (var item in cart)
//        //        {
//        //            itemList.items.Add(new Item()
//        //            {
//        //                name = item.Product.ProductName,
//        //                currency = "USD",
//        //                price = Convert.ToDecimal(item.Product.Price / conversionRate).ToString("F2"),
//        //                quantity = item.Quantity.ToString(),
//        //                sku = "sku"
//        //            });

//        //            totalAmount += (decimal)(item.Product.Price * item.Quantity);
//        //        }

//        //        Session["SesTotal"] = totalAmount.ToString("F2");

//        //        var payer = new Payer() { payment_method = "paypal" };
//        //        var redirUrls = new RedirectUrls()
//        //        {
//        //            cancel_url = redirectURL + "&Cancel=true",
//        //            return_url = redirectURL
//        //        };

//        //        var amount = new Amount()
//        //        {
//        //            currency = "USD",
//        //            total = totalAmountUSD.ToString("F2")
//        //        };

//        //        var transactionList = new List<Transaction>();
//        //        transactionList.Add(new Transaction()
//        //        {
//        //            description = "Mobile purchase",
//        //            invoice_number = Guid.NewGuid().ToString(),
//        //            amount = amount,
//        //            item_list = itemList
//        //        });

//        //        this.payment = new Payment()
//        //        {
//        //            intent = "sale",
//        //            payer = payer,
//        //            transactions = transactionList,
//        //            redirect_urls = redirUrls
//        //        };
//        //        Session.Add(guid, payment.id);
//        //    }

//        //    return this.payment.Create(apiContext);
//        //}

//        private Payment CreatePayment(APIContext apiContext, string redirectURL, string guid)
//        {
//            var itemList = new ItemList() { items = new List<Item>() };
//            decimal totalAmountINR = 0;

//            if (Session["cart"] != null)
//            {
//                List<Models.Home.Item> cart = (List<Models.Home.Item>)Session["cart"];

//                foreach (var item in cart)
//                {
//                    itemList.items.Add(new Item()
//                    {
//                        name = item.Product.ProductName,
//                        currency = "USD",
//                        price = Convert.ToDecimal(item.Product.Price / 83).ToString("F2"), // converting INR → USD
//                        quantity = item.Quantity.ToString(),
//                        sku = "sku"
//                    });

//                    totalAmountINR += (decimal)(item.Product.Price * item.Quantity);
//                }
//            }

//            decimal conversionRate = 83m; // You can replace this with real-time rate
//            decimal totalAmountUSD = totalAmountINR / conversionRate;

//            var payer = new Payer() { payment_method = "paypal" };

//            var redirUrls = new RedirectUrls()
//            {
//                cancel_url = redirectURL + "&cancel=true",
//                return_url = redirectURL
//            };

//            var amount = new Amount()
//            {
//                currency = "USD",
//                total = totalAmountUSD.ToString("F2")
//            };

//            var transactionList = new List<Transaction>();
//            transactionList.Add(new Transaction()
//            {
//                description = "Mobile purchase",
//                invoice_number = Guid.NewGuid().ToString(),
//                amount = amount,
//                item_list = itemList
//            });

//            var payment = new Payment()
//            {
//                intent = "sale",
//                payer = payer,
//                transactions = transactionList,
//                redirect_urls = redirUrls
//            };

//            return payment.Create(apiContext);
//        }



//        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
//        {
//            var paymentExecution = new PaymentExecution() { payer_id = payerId };
//            this.payment = new Payment() { id = paymentId };
//            return this.payment.Execute(apiContext, paymentExecution);
//        }
//    }
//}


