using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var searchClient = new SearchServiceClient("la70532ss",
                new SearchCredentials("E46A135C14120276E1B6D7FF40935D5A"));

            var indexClient = searchClient.Indexes.GetClient("realestate-us-sample");

            var searchParameters = new SearchParameters() { SearchMode = SearchMode.All };

            var docs = indexClient.Documents.Search("32nd Avenue", searchParameters);
            
            return Json(docs.Results, JsonRequestBehavior.AllowGet);
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