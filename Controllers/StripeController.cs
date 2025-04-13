using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Web.Mvc;

public class StripeController : Controller
{
    private readonly StripeClient stripeClient;

    public StripeController()
    {
        var secretKey = System.Configuration.ConfigurationManager.AppSettings["StripeSecretKey"];
        stripeClient = new StripeClient(secretKey);
    }

    public ActionResult Checkout()
    {
        var domain = "https://localhost:44300"; // adjust this for your domain

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 2000, // $20.00
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Product"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = domain + "/Stripe/Success",
            CancelUrl = domain + "/Stripe/Cancel"
        };

        var service = new SessionService(stripeClient); // use injected client here
        Session session = service.Create(options);

        return Redirect(session.Url);
    }

    public ActionResult Success()
    {
        return View();
    }

    public ActionResult Cancel()
    {
        return View();
    }
}
