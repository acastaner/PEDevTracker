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
        public ActionResult Index(int? page)
        {
            var fluentConfig = HibernateModule.GetConfiguration();
            var s = HibernateModule.CreateSession();

            var request = s.QueryOver<Post>()
                            .OrderBy(x => x.Date).Desc;

            int postCount = request.RowCount();

            var actualPosts = request
                                .Skip(page.GetValueOrDefault() * 10)
                                .Take(10)
                                .List();
            
            ViewBag.Pagination = new Pagination(postCount, 10, page, "Index", null);
            ViewBag.Message = "Tracking the developers of Project: Eternity.";
            return View(actualPosts);  
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
