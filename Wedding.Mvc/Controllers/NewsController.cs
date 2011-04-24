using System;
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
    public class NewsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Noticias";
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult GetLatestNews(string token)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var count = 6;

            var query = context.CreateQuery<News>("News")
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
                news = response.Results,
                nextToken = writer.ToString(),
                hasMore = response.ContinuationToken != null
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddNews(string title, string body)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var userPrincipal = HttpContext.User as UserPrincipal;
            var userIdentity = userPrincipal.Identity as UserIdentity;
            var userData = UserData.FromString(userIdentity.Ticket.UserData);

            body = body.Trim();

            var news = new News();
            news.Title = title;
            news.Body = body;
            news.PublishDate = DateTime.Now.ToLongDateString();

            context.AddObject("News", news);
            context.SaveChangesWithRetries();

            return new EmptyResult();
        }

    }
}
