using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        [Authorize]
        public ActionResult PhotoAlbum()
        {
            ViewBag.Message = "Photo Album";
            return View();
        }
    }
}
