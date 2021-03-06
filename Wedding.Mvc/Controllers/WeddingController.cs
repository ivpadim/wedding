﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;

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
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult GetYourWishes(string token)
        {
            ViewBag.Message = "Your Wishes";
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var count = Request.UrlReferrer.OriginalString.ToLower().Contains("wedding") ? 8 : 6;

            var query = context.CreateQuery<Wish>("Wishes")
                                .Where(w => w.PartitionKey == "wedding")
                                .Take(count).AsTableServiceQuery();

            var continuation = token != null
                   ? (ResultContinuation)new XmlSerializer(typeof(ResultContinuation)).Deserialize(new StringReader(token))
                   : null;
            var response = query.EndExecuteSegmented(query.BeginExecuteSegmented(continuation, null, null));
            var writer = new StringWriter();
            new XmlSerializer(typeof(ResultContinuation)).Serialize(writer, response.ContinuationToken);
            return Json(new
            {
                wishes = response.Results,
                nextToken = writer.ToString(),
                hasMore = response.ContinuationToken != null
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWish(string comment)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var userPrincipal = HttpContext.User as UserPrincipal;
            var userIdentity = userPrincipal.Identity as UserIdentity;
            var userData = UserData.FromString(userIdentity.Ticket.UserData);

            comment = comment.Trim();
            if (!comment.EndsWith("."))
                comment = comment + ".";

            var previousWish = context.CreateQuery<Wish>("Wishes")
                                                .Where(w => w.PartitionKey == "wedding" &&
                                                                    w.Account == userData.Email)
                                                .FirstOrDefault();

            if (previousWish != null)
                context.DeleteObject(previousWish);

            var wish = new Wish();
            wish.Account = userData.Email;
            wish.From = userData.FirstName + " " + userData.LastName;
            wish.Message = comment;
            wish.PublishDate = DateTime.Now;

            context.AddObject("Wishes", wish);
            context.SaveChangesWithRetries();

            return new EmptyResult();
        }

        [Authorize]
        public ActionResult PhotoAlbum()
        {
            ViewBag.Message = "Photo Album";

            return View();
        }

        [Authorize]
        public ActionResult Music()
        {
            ViewBag.Message = "Music page";
            return View();
        }
    }
}
