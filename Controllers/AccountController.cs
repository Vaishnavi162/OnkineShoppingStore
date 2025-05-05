using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineShopingStore.DAL;
using OnlineShopingStore.Models;
using OnlineShopingStore.Repository;

namespace OnlineShopingStore.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private dbMyOnlineShoppingEntitiess db = new dbMyOnlineShoppingEntitiess();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }



        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Login(Tbl_User model)
        //{
        //    using (dbMyOnlineShoppingEntitiess db = new dbMyOnlineShoppingEntitiess())
        //    {
        //        var user = db.Tbl_User.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
        //        if (user != null)
        //        {
        //            // Store user info in session
        //            Session["UserId"] = user.UserID;
        //            Session["Fullname"] = user.Fullname;

        //            // Set success message in TempData to show it in the next request
        //            TempData["SuccessMessage"] = "Login successful!";

        //            // Redirect to Home page or any other page
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            // Show error message if login fails
        //            ModelState.AddModelError("", "Invalid email or password");
        //            return View();
        //        }
        //    }
        //}
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Tbl_User model)
        {
            using (dbMyOnlineShoppingEntitiess db = new dbMyOnlineShoppingEntitiess())
            {
                var user = db.Tbl_User.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    // Store user info in session
                    Session["UserId"] = user.UserID;
                    Session["Fullname"] = user.Fullname;

                    // Set success message in TempData
                    TempData["SuccessMessage"] = "Login successful!";

                    // Check if the user is admin
                    if (user.Email == "admin@gmail.com" && user.Password == "admin123")
                    {
                        TempData["SuccessMessage"] = "Redirecting to dashboard...";
                        return RedirectToAction("Dashboard", "Admin");

                    }

                    // Redirect to Index for regular users
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Show error message if login fails
                    ModelState.AddModelError("", "Invalid email or password");
                    return View();
                }
            }
        }



        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Tbl_User model)
        {
            if (ModelState.IsValid)
            {
                using (dbMyOnlineShoppingEntitiess db = new dbMyOnlineShoppingEntitiess())
                {
                    var existingUser = db.Tbl_User.FirstOrDefault(x => x.Email == model.Email);
                    if (existingUser != null)
                    {
                        // ✅ This line adds the error message
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(model);
                    }

                    db.Tbl_User.Add(model);
                    db.SaveChanges();
                    TempData["Success"] = "Registration successful! Please log in.";
                    return RedirectToAction("Login");
                }
            }

            return View(model);
        }

        //public ActionResult Profile()
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        TempData["LoginRequired"] = "Please login to view your profile.";
        //        return RedirectToAction("Login", "Account");
        //    }

        //    int userId = Convert.ToInt32(Session["UserId"]);

        //    using (var context = new dbMyOnlineShoppingEntitiess())
        //    {
        //        var user = context.Tbl_User.SingleOrDefault(x => x.UserID == userId);
        //        if (user != null)
        //        {
        //            return View(user);
        //        }
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        public ActionResult Profile()
        {
            if (Session["UserId"] == null)
            {
                TempData["LoginRequired"] = "Please login to view your profile.";
                return RedirectToAction("Login", "Account");
            }


            int userId = Convert.ToInt32(Session["UserId"]);

            using (var context = new dbMyOnlineShoppingEntitiess())
            {
                var user = context.Tbl_User.SingleOrDefault(x => x.UserID == userId);
                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        //public ActionResult MyOrders()
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    int userId = Convert.ToInt32(Session["UserId"]);

        //    var orders = (from order in db.Tbl_Order
        //                  where order.UserID == userId
        //                  select new UserOrderDetailViewModel
        //                  {
        //                      OrderID = order.OrderID,
        //                      OrderDate = (DateTime)order.OrderDate,
        //                      PaymentAmount = order.PaymentAmount ?? 0,
        //                      OrderStatus = order.OrderStatus,
        //                      Products = (from detail in db.Tbl_OrderDetail
        //                                  join product in db.Tbl_Product
        //                                  on detail.ProductID equals product.ProductId
        //                                  where detail.OrderID == order.OrderID
        //                                  select new UserOrderDetailViewModel.ProductInfo
        //                                  {
        //                                      ProductName = product.ProductName,
        //                                      ProductImage = product.ProductImage,
        //                                      Price = detail.Price,
        //                                      Quantity = detail.Quantity
        //                                  }).ToList()
        //                  }).ToList();

        //    return View(orders);
        //}
        public ActionResult MyOrders()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            var orders = db.Tbl_Order
                .Where(order => order.UserID == userId)
                .Select(order => new UserOrderDetailViewModel
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate ?? DateTime.MinValue,
                    PaymentAmount = order.PaymentAmount ?? 0,
                    OrderStatus = order.OrderStatus, // this is admin-updated value
                    Products = db.Tbl_OrderDetail
                        .Where(detail => detail.OrderID == order.OrderID)
                        .Join(db.Tbl_Product,
                              detail => detail.ProductID,
                              product => product.ProductId,
                              (detail, product) => new UserOrderDetailViewModel.ProductInfo
                              {
                                  ProductName = product.ProductName,
                                  ProductImage = product.ProductImage,
                                  Price = detail.Price,
                                  Quantity = detail.Quantity
                              })
                        .ToList()
                })
                .ToList();

            return View(orders);
        }



        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            using (var context = new dbMyOnlineShoppingEntitiess())
            {
                var user = context.Tbl_User.FirstOrDefault(x => x.UserID == userId);
                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(Tbl_User updatedUser)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            using (var context = new dbMyOnlineShoppingEntitiess())
            {
                var user = context.Tbl_User.FirstOrDefault(x => x.UserID == userId);
                if (user != null)
                {
                    user.Fullname = updatedUser.Fullname;
                    user.Email = updatedUser.Email;
                    user.Password = updatedUser.Password;
                    user.Contact = updatedUser.Contact;
                    user.Address = updatedUser.Address;
                    // Add other fields as needed

                    context.SaveChanges();
                    TempData["Message"] = "Profile updated successfully!";
                    Session["Fullname"] = user.Fullname; // Update session if needed
                }
            }

            return RedirectToAction("Profile");
        }
        private GenericUnitOfWork unitOfWork = new GenericUnitOfWork();

        public ActionResult Users()
        {
            var users = unitOfWork.GetRepositoryInstance<Tbl_User>().GetAllRecords().ToList();
            return View(users);
        }



        public ActionResult Logout()
        {
            // Clear session or authentication info
            Session.Clear(); // or use Session.Abandon();

            // Set TempData success message
            TempData["SuccessMessage"] = "Logout successful!";

            // Redirect to login or home page
            return RedirectToAction("Index", "Home"); // or RedirectToAction("Login", "Account");
        }





        //    //
        //    // GET: /Account/ConfirmEmail
        //    [AllowAnonymous]
        //    public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //    {
        //        if (userId == null || code == null)
        //        {
        //            return View("Error");
        //        }
        //        var result = await UserManager.ConfirmEmailAsync(userId, code);
        //        return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //    }

        //

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData["Message"] = "Please enter a valid email address.";
                return View();
            }

            // Here, you would implement the logic to generate a reset token, send email, etc.
            TempData["Message"] = "If the email is registered, a reset link will be sent.";
            return RedirectToAction("Login");
        }



        //
        // POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //    //
        //    // GET: /Account/ForgotPasswordConfirmation
        //    [AllowAnonymous]
        //    public ActionResult ForgotPasswordConfirmation()
        //    {
        //        return View();
        //    }

        //    //
        //    // GET: /Account/ResetPassword
        //    [AllowAnonymous]
        //    public ActionResult ResetPassword(string code)
        //    {
        //        return code == null ? View("Error") : View();
        //    }

        //    //
        //    // POST: /Account/ResetPassword
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(model);
        //        }
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null)
        //        {
        //            // Don't reveal that the user does not exist
        //            return RedirectToAction("ResetPasswordConfirmation", "Account");
        //        }
        //        var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("ResetPasswordConfirmation", "Account");
        //        }
        //        AddErrors(result);
        //        return View();
        //    }

        //    //
        //    // GET: /Account/ResetPasswordConfirmation
        //    [AllowAnonymous]
        //    public ActionResult ResetPasswordConfirmation()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Account/ExternalLogin
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult ExternalLogin(string provider, string returnUrl)
        //    {
        //        // Request a redirect to the external login provider
        //        return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //    }

        //    //
        //    // GET: /Account/SendCode
        //    [AllowAnonymous]
        //    public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //    {
        //        var userId = await SignInManager.GetVerifiedUserIdAsync();
        //        if (userId == null)
        //        {
        //            return View("Error");
        //        }
        //        var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //        return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //    }

        //    //
        //    // POST: /Account/SendCode
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View();
        //        }

        //        // Generate the token and send it
        //        if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //        {
        //            return View("Error");
        //        }
        //        return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //    }

        //    //
        //    // GET: /Account/ExternalLoginCallback
        //    [AllowAnonymous]
        //    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //    {
        //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (loginInfo == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        // Sign in the user with this external login provider if the user already has a login
        //        var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //        switch (result)
        //        {
        //            case SignInStatus.Success:
        //                return RedirectToLocal(returnUrl);
        //            case SignInStatus.LockedOut:
        //                return View("Lockout");
        //            case SignInStatus.RequiresVerification:
        //                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //            case SignInStatus.Failure:
        //            default:
        //                // If the user does not have an account, then prompt the user to create an account
        //                ViewBag.ReturnUrl = returnUrl;
        //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //        }
        //    }

        //    //
        //    // POST: /Account/ExternalLoginConfirmation
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            return RedirectToAction("Index", "Manage");
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // Get the information about the user from the external login provider
        //            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //            if (info == null)
        //            {
        //                return View("ExternalLoginFailure");
        //            }
        //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //            var result = await UserManager.CreateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //                if (result.Succeeded)
        //                {
        //                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                    return RedirectToLocal(returnUrl);
        //                }
        //            }
        //            AddErrors(result);
        //        }

        //        ViewBag.ReturnUrl = returnUrl;
        //        return View(model);
        //    }

        //    //
        //    // POST: /Account/LogOff
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult LogOff()
        //    {
        //        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    //
        //    // GET: /Account/ExternalLoginFailure
        //    [AllowAnonymous]
        //    public ActionResult ExternalLoginFailure()
        //    {
        //        return View();
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            if (_userManager != null)
        //            {
        //                _userManager.Dispose();
        //                _userManager = null;
        //            }

        //            if (_signInManager != null)
        //            {
        //                _signInManager.Dispose();
        //                _signInManager = null;
        //            }
        //        }

        //        base.Dispose(disposing);
        //    }

        //    #region Helpers
        //    // Used for XSRF protection when adding external logins
        //    private const string XsrfKey = "XsrfId";

        //    private IAuthenticationManager AuthenticationManager
        //    {
        //        get
        //        {
        //            return HttpContext.GetOwinContext().Authentication;
        //        }
        //    }

        //    private void AddErrors(IdentityResult result)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error);
        //        }
        //    }

        //    private ActionResult RedirectToLocal(string returnUrl)
        //    {
        //        if (Url.IsLocalUrl(returnUrl))
        //        {
        //            return Redirect(returnUrl);
        //        }
        //        return RedirectToAction("Index", "Home");
        //    }

        //    internal class ChallengeResult : HttpUnauthorizedResult
        //    {
        //        public ChallengeResult(string provider, string redirectUri)
        //            : this(provider, redirectUri, null)
        //        {
        //        }

        //        public ChallengeResult(string provider, string redirectUri, string userId)
        //        {
        //            LoginProvider = provider;
        //            RedirectUri = redirectUri;
        //            UserId = userId;
        //        }

        //        public string LoginProvider { get; set; }
        //        public string RedirectUri { get; set; }
        //        public string UserId { get; set; }

        //        public override void ExecuteResult(ControllerContext context)
        //        {
        //            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //            if (UserId != null)
        //            {
        //                properties.Dictionary[XsrfKey] = UserId;
        //            }
        //            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //        }
        //    }
        //    #endregion
    }
}