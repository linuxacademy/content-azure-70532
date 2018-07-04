using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Redis2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sessionKey = Session["myData"].ToString();  
            return Content(sessionKey);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}