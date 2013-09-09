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

            InitiateData();

            string url = "http://forums.obsidian.net/user/18301-badler/?tab=posts";

            string content;
            // Retrieve web page
            // TODO: Not hard-coded authentication because this is ugly
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Cookie: \"session_id=323a49c3240f5b3a1c61978cf5ebe3a5; modtids=,; member_id=43009; pass_hash=f98e8579016c508c07c0c2fd4e9ec91d; ipsconnect_5ba0915b63128c11858535d880db7be6=1; coppa=0; rteStatus=rte\"");
                content = wc.DownloadString(url);
                
            }

            // Save only the HTML code we are interested in
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode mainNode = doc.GetElementbyId("profile_panes_wrap");

            
            List<DevPost> devPosts = new List<DevPost>();
            
            // If we found some data, store each post as a different node (we will need to parse the nodes some more)
            if (mainNode != null)
            {
                IEnumerable<HtmlNode> allPosts = null;
                allPosts = mainNode.SelectNodes("//*[@class='post_block no_sidebar']");
                
                // Further parse each post to extract thread link, post time, etc..
                foreach (HtmlNode postNode in allPosts)
                {
                    DevPost newPost = new DevPost();
                    newPost.Author = new Developer("18301-badler", "BAddler", "Some Dude"); 
                    newPost.ImportPost(postNode);                    
                    devPosts.Add(newPost);
                }                
            }

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View(devPosts);                       

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
            Developer sawyer = new Developer("24-je-sawyer", "J.E. Sawyer", "J.E. Sawyer");
            s.Save(adam);
            s.Save(darren);
            s.Save(badler);
            s.Save(sawyer);

            t.Commit();
        }
    }
}
