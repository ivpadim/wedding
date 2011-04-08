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

        


        [Authorize]
        public ActionResult PhotoAlbum()
        {
            ViewBag.Message = "Photo Album";
            
            return View();
        }
    }
}
