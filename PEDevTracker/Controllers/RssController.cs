using System;
using System.Collections.Generic;
using System.Linq;
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
