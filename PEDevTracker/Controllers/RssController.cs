using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;

namespace PEDevTracker.Controllers
{
    public class RssController : Controller
    {
        //
        // GET: /Rss/

        public ActionResult Index()
        {
            return View();
        }

        public RssActionResult Feed()
        {
            var s = HibernateModule.CurrentSession;
            var t = s.BeginTransaction();

            var query = s.QueryOver<Post>()
                        .OrderBy(a => a.Date).Desc
                        .Take(10)
                        .List();

            List<SyndicationItem> items = new List<SyndicationItem>();
            
            string hostName = "http://pedevtracker.azurewebsites.net";
            foreach (Post a in query)
            {
                string url = hostName + "/Post/Details/"+ a.Id;
                SyndicationItem item = new SyndicationItem("Dev post by " + a.Author.DisplayName, a.Content, new Uri(url));
                item.PublishDate = a.Date;
                item.LastUpdatedTime = a.RetrieveDate;
                item.Id = a.Id.ToString();
                items.Add(item);
            }
            var feed = new SyndicationFeed("Project Eternity Dev Tracker", "Awkwardly tracking devs since 2013", new Uri(hostName), items);
            return new RssActionResult(new Rss20FeedFormatter(feed));
        }

        //
        // GET: /Rss/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Rss/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Rss/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Rss/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Rss/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Rss/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Rss/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
