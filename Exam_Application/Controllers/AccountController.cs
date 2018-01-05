using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Exam_Application.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using DataLayer.ExamModel;
using itechDll;

namespace Exam_Application.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        Exam_MasterEntities db = new Exam_MasterEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult AdminLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminLogin(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Model State Is Invalid.");
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                string UserName = "";
                try
                {                   
                    UserName = db.tbl_User_Profile.Where(x => x.EmailAddress == model.Email || x.MobileNumber == model.Email).FirstOrDefault().UserName;
                }
                catch
                {

                }
                var result = await SignInManager.PasswordSignInAsync(UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return RedirectToAction("AdminLogin", "Account");
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult AdminUserRegister(string Exceptionmsg)
        {
            Session["Menu"] = 7.ToString();
            ViewBag.Exception = Exceptionmsg;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Adduser()
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            ViewBag.rolelist = new SelectList(roleManager.Roles.ToList(), "Name", "Name");
            return PartialView();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Adduser(RegisterViewModel model)
        {
            string exception = "";
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            ViewBag.rolelist = new SelectList(roleManager.Roles.ToList(), "Name", "Name");
            if (!roleManager.RoleExists("AdminUser"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "AdminUser";
                model.UserRoles = "AdminUser";
                roleManager.Create(role);
            }
            if (model.Email != null || model.MobileNumber != null)
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        int mobdt = db.tbl_User_Profile.Where(x => x.EmailAddress == model.Email).Count();
                        if (mobdt > 0)
                        {
                            exception = "User Email Already Taken.";
                            return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
                        }
                    }
                    if (!string.IsNullOrEmpty(model.MobileNumber))
                    {
                        int mobdt = db.tbl_User_Profile.Where(x => x.MobileNumber == model.MobileNumber).Count();
                        if (mobdt > 0)
                        {
                            exception = "User Mobile Number Already Taken.";
                            return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
                        }
                    }
                    string UUNAME = "";
                    if (string.IsNullOrEmpty(model.Email))
                    {
                        UUNAME = model.MobileNumber;
                    }
                    else
                    {
                        UUNAME = model.Email;
                    }
                    var user = new ApplicationUser { UserName = UUNAME, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                        var currentUser = UserManager.FindByName(user.UserName);

                        var roleresult = UserManager.AddToRole(currentUser.Id, model.UserRoles);
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                        TempData["id"] = currentUser.Id;
                        TempData["role"] = model.UserRoles;
                        TempData["username"] = user.UserName;
                        TempData["ema"] = model.Email;
                        TempData["passwo"] = model.Password;
                        TempData["fname"] = model.FullName;
                        TempData["mobi"] = model.MobileNumber;
                        return RedirectToAction("SaveCustomer", "Admin_Task");
                    }
                    foreach (var error in result.Errors)
                    {
                        if (exception == "")
                        {
                            exception = error;
                        }
                    }
                }
            }
            else
            {
                exception = "Mobile Number Or Email is Required";
                return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
            }
          
            return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
        }

        [AllowAnonymous]
        public ActionResult AddRole()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddRole(IdentityRole model)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists(model.Name))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = model.Name;
                if (model.Id == null)
                {
                    roleManager.Create(role);
                }
                else
                {
                    //  roleManager.Update(role);
                }
                return RedirectToAction("AdminUserRegister", "Account");
            }
            else
            {
                string ex = "Role Already Exist?";
                return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = ex });
            }
        }


        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword(string result)
        {
            ViewBag.Exception = result;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var username = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                string res = "User Not Available";
                return RedirectToAction("ChangePassword", "Account", new { result = res });
            }

            string userid = User.Identity.GetUserId();
            var token = await UserManager.GeneratePasswordResetTokenAsync(userid);
            var result = await UserManager.ResetPasswordAsync(userid, token, model.NewPassword);
            if (result.Succeeded)
            {
                string res = "Password changed.";
                return RedirectToAction("ChangePassword", "Account", new { result = res });
            }
            AddErrors(result);
            string ress = result.Errors.FirstOrDefault();
            return RedirectToAction("ChangePassword", "Account", new { result = ress });

        }

        public async Task<ActionResult> CreateStudent(string email, string pass)
        {
            string response = "";
            try
            {
                if (email != "" && pass != "")
                {
                    var user = new ApplicationUser { UserName = email,Email=email };
                    var result = await UserManager.CreateAsync(user, pass);
                    if (result.Succeeded)
                    {
                        //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                        var currentUser = UserManager.FindByName(user.UserName);
                        var roleresult = UserManager.AddToRole(currentUser.Id, "Student");                        
                        return Json(currentUser.Id, JsonRequestBehavior.AllowGet);
                    }
                    response = "failed "+result.Errors.FirstOrDefault();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {               
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("AdminLogin", "Account");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string UserName = "";
                    try
                    {
                        UserName = db.tbl_User_Profile.Where(x => x.EmailAddress == model.Email || x.MobileNumber == model.Email).FirstOrDefault().UserName;
                    }
                    catch
                    {

                    }
                    var user = await UserManager.FindByNameAsync(UserName);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                    var result = await UserManager.ResetPasswordAsync(user.Id, token, "123456");
                    // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("AdminLogin", "Account");
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View(model);
            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult LogOff()
        {
            try
            {
                Roles.DeleteCookie();
            }
            catch (Exception e)
            {

            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            return RedirectToAction("FirstPage", "Admin_Task");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}