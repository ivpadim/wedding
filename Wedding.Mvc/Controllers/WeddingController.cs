using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace Wedding.Mvc.Controllers
{
    public class WeddingController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Wedding page";
            return View();
        }

        [Authorize]
        public ActionResult YourWishes()
        {
            ViewBag.Message = "Your Wishes";
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var query = context.CreateQuery<Wish>("Wishes")
                                .Where(w => w.PartitionKey == "wedding");

            return View(query);
        }


        [HttpPost]
        public ActionResult AddWish(string comment)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var userPrincipal = HttpContext.User as UserPrincipal;
            var userIdentity = userPrincipal.Identity as UserIdentity;
            var userData = UserData.FromString(userIdentity.Ticket.UserData);

            var wish = new Wish();
            wish.Account = userData.Email;
            wish.From = userData.FirstName + " " + userData.LastName;
            wish.Message = comment;
            wish.PublishDate = DateTime.Now;

            context.AddObject("Wishes", wish);
            context.SaveChangesWithRetries();

            return RedirectToAction("YourWishes");
        }


        [Authorize]
        public ActionResult PhotoAlbum()
        {
            ViewBag.Message = "Photo Album";

            return View();
        }
    }
}
