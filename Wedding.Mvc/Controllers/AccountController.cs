using System.Web.Mvc;
using System.Web.Routing;
using Wedding.Mvc.Models;
using Wedding.Mvc.Services;
using System.Web.Security;

namespace Wedding.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public AccountMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (this.MembershipService.AuthenticateUser(model.Account, model.Password))
                {
                    this.FormsService.SignIn(model.Account, model.RememberMe, this.MembershipService.User.ToString());

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "the user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateUser(UserData userData)
        {
            if (ModelState.IsValid)
            {
                var createStatus = this.MembershipService.CreateUser(userData);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            return View(userData);
        }

    }
}
