using HtmlAgilityPack;
using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace PEDevTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fluentConfig = HibernateModule.GetConfiguration();
            var s = HibernateModule.CreateSession();
            IList<Post> latestDevPosts = s.QueryOver<Post>()
                                .OrderBy(x => x.Date).Desc
                                .Take(100)
                                .List<Post>();

            ViewBag.Message = "Tracking the developers of Project: Eternity.";
            return View(latestDevPosts);  
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
