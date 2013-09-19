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
            List<Developer> devs = new List<Developer>();
            devs.Add(new Developer("1444-adam-brennecke", "Adam Brennecke", "Adam Brennecke"));
            devs.Add(new Developer("18301-badler", "BAdler", ""));
            devs.Add(new Developer("6-darren-monahan", "Darren Monahan", "Darren Monahan"));
            devs.Add(new Developer("1-the-guildmaster", "The Guildmaster", ""));
            devs.Add(new Developer("24-je-sawyer", "J.E. Sawyer", "Joshua Eric Sawyer"));
            devs.Add(new Developer("43480-kaz", "Kaz", "Kazunori Aruga"));
            devs.Add(new Developer("51406-rob-nesler", "Rob Nesler", "Rob Nesler"));
            devs.Add(new Developer("30529-dimitri-berman", "Dimitri Berman", "Dimitri Berman"));
            devs.Add(new Developer("54039-polina", "Polina", "Polina Hristova"));

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
