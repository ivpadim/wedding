using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.Mvc.Models;
using Wedding.Mvc.Services;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Wedding.Mvc.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Home page";
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Contact(string comment)
        {

            var userPrincipal = HttpContext.User as UserPrincipal;
            var userIdentity = userPrincipal.Identity as UserIdentity;
            var userData = UserData.FromString(userIdentity.Ticket.UserData);

            comment = comment.Trim();
            if (!comment.EndsWith("."))
                comment = comment + ".";

            string adminEmail = RoleEnvironment.GetConfigurationSettingValue("AdminEmail");
            string adminName = RoleEnvironment.GetConfigurationSettingValue("AdminFirstName") + ' ' + RoleEnvironment.GetConfigurationSettingValue("AdminLastName");

            GmailService.Send(adminEmail, adminName,
                        userData.FirstName + "  " + userData.LastName + "<" + userData.Email + ">" + " te envio un comentario",
                        comment);

            return new EmptyResult();
        }

        [Authorize]
        public ActionResult Family()
        {
            ViewBag.Message = "Our family page";
            return View();
        }

    }
}
