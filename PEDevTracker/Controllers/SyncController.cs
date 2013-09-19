using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PEDevTracker.Controllers
{
    public class SyncController : Controller
    {
        //
        // GET: /Sync/

        public ActionResult Index()
        {
            return View();
        }

        public object All()
        {
            bool success = false;
            try
            {
                var s = HibernateModule.CreateSession();
                IList<Developer> devs = s.QueryOver<Developer>()
                                        .List<Developer>();

                foreach (Developer dev in devs)
                {
                    dev.FetchPostsFromRemote();
                }
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't fetch posts from remote: " + ex.Message);
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

    }
}
