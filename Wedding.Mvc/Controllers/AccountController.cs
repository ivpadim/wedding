using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;
using Wedding.Mvc.Services;

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
                    string body = string.Format("{0} buen dia!!, <br/> Tu cuenta para accesar al sitio de Martha & Ivan Wedding " +
                                            "<a ref='http://marthaeivan.cloudapp.net'>http://marthaeivan.cloudapp.net</a> ha sido creada" +
                                            "<br/><br/>usuario:{1}<br/>password:{2}<br/><br/>", userData.FirstName, userData.Email, userData.Password);
                    GmailService.Send(userData.Email, userData.Email, "Se ha creado tu cuenta :)", body);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            return View(userData);
        }

        [Authorize]
        public ActionResult EditUser()
        {
            var userPrincipal = HttpContext.User as UserPrincipal;
            var userIdentity = userPrincipal.Identity as UserIdentity;
            var userData = UserData.FromString(userIdentity.Ticket.UserData);
            return View(userData);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateUser(string email)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var userData = context.CreateQuery<UserData>("Users")
                        .Where(user => user.PartitionKey == "wedding" &&
                                                     user.Email == email)
                        .FirstOrDefault();

            return View("EditUser", userData);
        }


        [HttpPost]
        [Authorize]
        public ActionResult UpdateUser(UserData userData)
        {
            return EditUser(userData);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUser(UserData userData)
        {
            if (ModelState.IsValid)
            {
                var createStatus = this.MembershipService.EditUser(userData);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
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
