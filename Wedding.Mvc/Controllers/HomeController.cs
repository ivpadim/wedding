using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;
using Wedding.Mvc.Services;

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

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult GetPhotosOfSlider()
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var container = account.CreateCloudBlobClient().GetContainerReference("photo-slider");

            var blobsUrl = container.ListBlobs().Select(b => new { Url = b.Uri.ToString() }).ToList();

            return Json(new { blobs = blobsUrl });
        }

        public ActionResult Link()
        {
            return View();
        }
    }
}
