using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PEDevTracker.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/

        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Home");
        }

        public ActionResult View(int id)
        {
            var s = HibernateModule.CurrentSession;
            Post post = new Post();

            try
            {
                post = s.QueryOver<Post>()
                               .Where(x => x.Id == id)
                               .Take(1)
                               .List<Post>().First();
            }
            catch
            {
                post = null;
            }            
            return View(post);
        }

    }
}
