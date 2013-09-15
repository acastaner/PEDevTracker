using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PEDevTracker.Controllers
{
    public class DeveloperController : Controller
    {
        //
        // GET: /Developer/

        public ActionResult Index()
        {
            var s = HibernateModule.CreateSession();
            IList<Developer> devs = s.QueryOver<Developer>()
                                    .OrderBy(x => x.DisplayName).Asc
                                    .List<Developer>();                              
            return View(devs);
        }

        public ActionResult View(int id)
        {
            var s = HibernateModule.CreateSession();
            Developer dev = new Developer();

            try
            {
                dev = s.QueryOver<Developer>()
                                .Where(x => x.Id == id)
                                .Take(1)
                                .List<Developer>().First();
            }
            catch
            {
                throw new Exception("Could not find that developer. You either found a bug or trying to catch land sharks.");
            }

            return View(dev);
        }

        /// <summary>
        /// Checks if accounts are present in the database, and if not, create them
        /// </summary>
        public static void InitiateAccounts()
        {
            var s = HibernateModule.CreateSession();
            var t = s.BeginTransaction();

            Developer badler = new Developer("18301-badler", "BAdler", "");
            Developer adam = new Developer("1444-adam-brennecke", "Adam Brennecke", "Adam Brennecke");
            Developer darren = new Developer("6-darren-monahan", "Darren Monahan", "Darren Monahan");
            Developer sawyer = new Developer("24-je-sawyer", "J.E. Sawyer", "Joshua Eric Sawyer");
            Developer guildmaster = new Developer("1-the-guildmaster", "The Guildmaster", "");

            List<Developer> devs = new List<Developer>();
            devs.Add(adam);
            devs.Add(badler);
            devs.Add(darren);
            devs.Add(guildmaster);
            devs.Add(sawyer);

            foreach (Developer dev in devs)
            {
                Developer foundDev = s.QueryOver<Developer>()
                                        .Where(x => x.ProfileId == dev.ProfileId)
                                        .Take(1)
                                        .List<Developer>().FirstOrDefault();
                if (foundDev == null)
                {
                    s.Save(dev);
                }
                                        
            }
            t.Commit();
        }

    }
}
