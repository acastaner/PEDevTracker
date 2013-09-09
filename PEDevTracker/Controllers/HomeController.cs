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
            InitiateData();

            var s = HibernateModule.CreateSession();
            IList<DevPost> latestDevPosts = s.QueryOver<DevPost>()
                                .OrderBy(x => x.Date).Desc
                                .Take(100)
                                .List<DevPost>();

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

        public void InitiateData()
        {
            var s = HibernateModule.CreateSession();
            var t = s.BeginTransaction();

            Developer badler = new Developer("18301-badler", "BAdler", "");
            Developer adam = new Developer("1444-adam-brennecke", "Adam Brennecke", "Adam Brennecke");
            Developer darren = new Developer("6-darren-monahan", "Darren Monahan", "Darren Monahan");
            Developer sawyer = new Developer("24-je-sawyer", "J.E. Sawyer", "Joshua Eric Sawyer");
            Developer guildmaster = new Developer("1-the-guildmaster", "The Guildmaster", "");
            s.Save(adam);
            s.Save(badler);
            s.Save(darren);
            s.Save(guildmaster);
            s.Save(sawyer);
            t.Commit();

            List<Developer> devs = new List<Developer>();
            devs.Add(adam);
            devs.Add(badler);
            devs.Add(darren);            
            devs.Add(guildmaster);
            devs.Add(sawyer);            
        }
    }
}
