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
        private static DateTime? _LastSyncTime = DateTime.Now.AddMinutes(-11);

        // GET: /sync/all
        public object All()
        {
            bool success = false;
            
            // If the last sync time is null (ie: website just started) we init the variable as -11 minutes, so it'll trigger a sync
            
            // People are so playful, we make sure they can't trigger the update more than once/9.5 min
            if (TimeSinceLastSync() >= 9.5)
            {
                try
                {
                    var s = HibernateModule.CurrentSession;
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
                finally
                {
                    _LastSyncTime = DateTime.Now;
                }
            }            
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Will return the time of the last attempted sync (successful or not).
        /// This is to prevent spamming of the refresh, which could in turn become a DDoS.
        /// This feels rather safe, but not sure about the performance.
        /// </summary>
        /// <returns></returns>
        private static double TimeSinceLastSync()
        {
            TimeSpan diff = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) - new TimeSpan(_LastSyncTime.Value.Hour, _LastSyncTime.Value.Minute, _LastSyncTime.Value.Second);
            return diff.TotalMinutes;
        }

    }
}
