using System.Web.Mvc;
using Wedding.Mvc.Models;

namespace Wedding.Mvc.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
