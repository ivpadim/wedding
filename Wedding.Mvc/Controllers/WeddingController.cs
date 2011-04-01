using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wedding.Mvc.Controllers
{
    public class WeddingController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Message = "Wedding page";
            return View();
        }

        public ActionResult YourWishes()
        {
            ViewBag.Message = "Your Wishes";
            return View();
        }
        
        public ActionResult PhotoAlbum()
        {
            return View();
        }
    }
}
