//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PayPal.Api;

//namespace OnlineShopingStore.Controllers
//{
//    public class PaymentController : Controller
//    {
//        // GET: Payment
//        public ActionResult PaymentWithPaypal()
//        {
//            APIContext apicontext = PaypalConfiguration.GetAPIContext();
//            try
//            {
//                string PayerId = Request.Params["PayerID"];
//                if (string.IsNullOrEmpty(PayerId))
//                {
//                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
//                        "/PaymentWithPaypal/PaymentWithPaypal?";
//                    var Guid = Convert.ToString((new Random()).Next(10000000));
//                    var createdPayment = this.CreatePayment(apicontext, baseURi + "guid" + Guid);
//                    var links = createdPayment.links.GetEnumerator();
//                    string paypalRedirectUrl = null;
//                    while (links.MoveNext())
//                    {
//                        Links lnk = links.Current;
//                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//                        {
//                            paypalRedirectUrl = lnk.href;
//                        }
//                    }
//                    //Session.Add(guid, createdPayment.id);
//                    //return Redirect(paypalRedirectUrl);
//                }
//                else
//                {
//                    var guid = Request.Params["guid"];
//                    var executedPayment = ExecutePayment(apicontext, PayerId, Session[guid] as string);
//                    if (executedPayment.ToString().ToLower() != "approved")
//                    {
//                        return View("FailureView");
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                return View("FailureView");
//            }
//            return View("SuccessView"); // Ensure a return value in all code paths
//        }

//        private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
//        {
//            var paymentExecution = new PaymentExecution() { payer_id = payerId };
//            this.payment = new Payment() { id = PaymentId };
//            return this.payment.Execute(apicontext, paymentExecution);
//        }

//        private PayPal.Api.Payment payment;
//        //private object CreatePayment(APIContext apicontext, string redirectURL)
//        private PayPal.Api.Payment CreatePayment(APIContext apicontext, string redirectURL)

//        {
//            var ItemList = new ItemList() { items = new List<Item>() };
//            if (Session["cart"] != "")
//            {
//                List<Models.Home.Item> cart = (List<Models.Home.Item>) (Session["cart"]);
//                foreach (var item in cart)
//                {
//                    ItemList.items.Add(new Item()
//                    {
//                        name = item.Product.ProductName.ToString(),
//                        currency = "TK",
//                        price = item.Product.Price.ToString(),
//                        quantity = item.Quantity.ToString(),
//                        sku = "sku"
//                    });
//                }

//                var payer = new Payer() { payment_method = "paypal" };
//                var redirUrl = new RedirectUrls()
//                {
//                    cancel_url = redirectURL + "&Cancle=true",
//                    return_url = redirectURL

//                };
//                var details = new Details()
//                {
//                    tax = "1",
//                    shipping = "1",
//                    subtotal = "1"
//                };
//                var amount = new Amount()
//                {
//                    currency = "USD",
//                    total = Session["SesTotal"].ToString(),
//                    details = details
//                };
//                var transactionList = new List<Transaction>();
//                transactionList.Add(new Transaction()
//                {
//                    description = "Transaction description",
//                    invoice_number = "#100000",
//                    amount = amount,
//                    item_list = ItemList

//                });
//                this.payment = new Payment()
//                {
//                    intent = "sale",
//                    payer = payer,
//                    transactions = transactionList,
//                    redirect_urls = redirUrl
//                };
//            }
//            return this.payment.Create(apicontext);


//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace OnlineShopingStore.Controllers
{
    public class PaymentController : Controller
    {
        private PayPal.Api.Payment payment;

        public ActionResult PaymentWithPaypal()
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // Redirecting to PayPal
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPaypal?";
                    string guid = Convert.ToString((new Random()).Next(10000000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            return Redirect(lnk.href); // Redirect to PayPal login
                        }
                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception)
            {
                return View("FailureView");
            }
            return View("SuccessView");
        }

        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectURL)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            if (Session["cart"] != null)
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)Session["cart"];
                decimal totalAmount = 0;

                foreach (var item in cart)
                {
                    itemList.items.Add(new Item()
                    {
                        name = item.Product.ProductName,
                        currency = "USD",
                        price = item.Product.Price.ToString(),
                        quantity = item.Quantity.ToString(),
                        sku = "sku"
                    });

                    totalAmount += (decimal)(item.Product.Price * item.Quantity);
                }

                Session["SesTotal"] = totalAmount.ToString();

                var payer = new Payer() { payment_method = "paypal" };
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectURL + "&Cancel=true",
                    return_url = redirectURL
                };

                var details = new Details()
                {
                    tax = "2",
                    shipping = "5",
                    subtotal = totalAmount.ToString()
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = (totalAmount + 2 + 5).ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Online Shopping Payment",
                    invoice_number = new Random().Next(100000).ToString(),
                    amount = amount,
                    item_list = itemList
                });

                this.payment = new PayPal.Api.Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }

            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
    }
}


//using PayPal.Api;
//using System.Collections.Generic;
//using System.Web.Mvc;

//public class PaymentController : Controller
//{
//    public ActionResult PaymentWithPayPal()
//    {
//        // Set up the payer
//        var payer = new Payer() { payment_method = "paypal" };

//        // Set redirect URLs
//        var redirectUrls = new RedirectUrls()
//        {
//            cancel_url = "https://localhost:44300/Payment/Cancel",
//            return_url = "https://localhost:44300/Payment/Success"
//        };

//        // Set item list
//        var itemList = new ItemList() { items = new List<Item>() };

//        itemList.items.Add(new Item()
//        {
//            name = "Product 1",
//            currency = "USD",
//            price = "10",
//            quantity = "1",
//            sku = "sku"
//        });

//        // Set payment details
//        var details = new Details()
//        {
//            tax = "1",
//            shipping = "2",
//            subtotal = "10"
//        };

//        // Set amount
//        var amount = new Amount()
//        {
//            currency = "USD",
//            total = "13", // Total = tax + shipping + subtotal
//            details = details
//        };

//        // Create transaction
//        var transactionList = new List<Transaction>();
//        transactionList.Add(new Transaction()
//        {
//            description = "Payment description",
//            invoice_number = "123456", // You should generate this
//            amount = amount,
//            item_list = itemList
//        });

//        var payment = new Payment()
//        {
//            intent = "sale",
//            payer = payer,
//            transactions = transactionList,
//            redirect_urls = redirectUrls
//        };

//        // Create the payment
//        var createdPayment = payment.Create(PaypalConfiguration.GetAPIContext());

//        // Get PayPal redirect URL and redirect user
//        var links = createdPayment.links.GetEnumerator();

//        string paypalRedirectUrl = null;
//        while (links.MoveNext())
//        {
//            Links lnk = links.Current;
//            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//            {
//                paypalRedirectUrl = lnk.href;
//            }
//        }

//        return Redirect(paypalRedirectUrl);
//    }

//    public ActionResult Success(string paymentId, string token, string PayerID)
//    {
//        var apiContext = PaypalConfiguration.GetAPIContext();

//        var paymentExecution = new PaymentExecution() { payer_id = PayerID };
//        var payment = new Payment() { id = paymentId };

//        var executedPayment = payment.Execute(apiContext, paymentExecution);

//        if (executedPayment.state.ToLower() != "approved")
//        {
//            return View("Failure");
//        }

//        return View("Success");
//    }

//    public ActionResult Cancel()
//    {
//        return View();
//    }
//}