using FluentNHibernate.Mapping;
using HtmlAgilityPack;
using PEDevTracker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PEDevTracker.Models
{
    #region Developer class
    public class Developer
    {
        #region Attributes
        public virtual int Id { get; set; }
        public virtual string ProfileId { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string RealName { get; set; }
        #endregion
        #region Constructors
        public Developer()
        {

        }
        public Developer(string profileId, string displayName, string realName)
        {
            this.ProfileId = profileId;
            this.DisplayName = displayName;
            this.RealName = realName;
        }

        #endregion
        #region Methods
        /// <summary>
        /// Returns the URI of the developer's profile.
        /// </summary>
        /// <returns></returns>
        public virtual Uri GetProfileUri()
        {
            return new Uri("http://forums.obsidian.net/user/" + ProfileId + "/");

        }
        /// <summary>
        /// Returns the "Posts" tab of the developer's profile page.
        /// </summary>
        /// <returns></returns>
        public virtual Uri GetPostsUri()
        {
            return new Uri(GetProfileUri() + "?tab=posts");
        }
        /// <summary>
        /// Returns the "Topics" tab of the developer's profile page.
        /// </summary>
        /// <returns></returns>
        public virtual Uri GetTopicsUri()
        {
            return new Uri(GetProfileUri() + "?tab=topics");
        }
        /// <summary>
        /// Fetches all the posts by this user from the local database (ie: not the official forums)
        /// </summary>
        /// <returns></returns>
        public virtual IList<Post> FetchPostsFromLocal(int amount)
        {
            var s = HibernateModule.CurrentSession;
            IList<Post> posts = s.QueryOver<Post>()
                                    .Where(x => x.Author == this)
                                    .OrderBy(x => x.Date).Desc
                                    .Take(amount)
                                    .List<Post>();

            return posts;
        }
        /// <summary>
        /// Fetches all the posts by this developer from remote (ie: from the official forums)
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Post> FetchPostsFromRemote()
        {
            List<Post> devPosts = new List<Post>();
            devPosts.AddRange(ParseTabContent(GetPostsUri()));
            devPosts.AddRange(ParseTabContent(GetTopicsUri()));            
            return devPosts;
        }

        private IEnumerable<Post> ParseTabContent(Uri tabUri)
        {
            string content;
            // Retrieve web page
            // TODO: Not hard-coded authentication because this is ugly
            // TODO: Also, error handling
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Cookie: rteStatus=rte; member_id=54540; pass_hash=f19af4a426127e8d98cd94efd310ea0a; ipsconnect_5ba0915b63128c11858535d880db7be6=1; coppa=0; session_id=2af087b0700e12f676ea244448fbf5e4");
                content = wc.DownloadString(tabUri);
            }

            // Save only the HTML code we are interested in
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode mainNode = doc.GetElementbyId("profile_panes_wrap");

            List<Post> devPosts = new List<Post>();

            // If we found some data, store each post as a different node (we will need to parse the nodes some more)
            if (mainNode != null)
            {
                IEnumerable<HtmlNode> allPosts = null;
                allPosts = mainNode.SelectNodes("//*[@class='post_block no_sidebar']");


                if (allPosts != null)
                {
                    // Further parse each post to extract thread link, post time, etc..
                    foreach (HtmlNode postNode in allPosts)
                    {
                        Post newPost = new Post();
                        newPost.Author = this;
                        newPost.ImportPost(postNode);
                        devPosts.Add(newPost);
                    }
                }
            }
            return devPosts;
        }
        public virtual int GetPostCount()
        {
            var s = HibernateModule.CurrentSession;
            return s.QueryOver<Post>()
                            .Where(x => x.Author == this)
                            .RowCount();
        }
        #endregion
    }
    #endregion
    #region Fluent NHibernate Mappings
    public class AuthorMap : ClassMap<Developer>
    {
        public AuthorMap()
        {
            Table("pedt_Developer"); 
            Id(x => x.Id);
            Map(x => x.ProfileId);
            Map(x => x.DisplayName);
            Map(x => x.RealName);
        }
    }
    #endregion
}