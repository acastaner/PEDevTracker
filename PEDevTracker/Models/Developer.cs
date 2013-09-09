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
        public virtual Uri GetProfileUri()
        {
            return new Uri("http://forums.obsidian.net/user/" + ProfileId + "/");

        }
        public virtual Uri GetPostsUri()
        {
            return new Uri(GetProfileUri() + "?tab=posts");
        }
        /// <summary>
        /// Fetches all the posts by this user from the local database (ie: not the official forums)
        /// </summary>
        /// <returns></returns>
        public virtual IList<DevPost> FetchPostsFromLocal(int amount)
        {
            var s = HibernateModule.CreateSession();
            IList<DevPost> posts = s.QueryOver<DevPost>()
                                    .Where(x => x.Author == this)
                                    .OrderBy(x => x.Date).Desc
                                    .Take(amount)
                                    .List<DevPost>();

            return posts;
        }
        /// <summary>
        /// Fetches all the posts by this developer from remote (ie: from the official forums)
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<DevPost> FetchPostsFromRemote()
        {
            string content;
            // Retrieve web page
            // TODO: Not hard-coded authentication because this is ugly
            // TODO: Also, error handling
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Cookie: \"session_id=323a49c3240f5b3a1c61978cf5ebe3a5; modtids=,; member_id=43009; pass_hash=f98e8579016c508c07c0c2fd4e9ec91d; ipsconnect_5ba0915b63128c11858535d880db7be6=1; coppa=0; rteStatus=rte\"");
                content = wc.DownloadString(GetPostsUri());
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
                    newPost.Author = this;
                    newPost.ImportPost(postNode);
                    devPosts.Add(newPost);
                }
            }
            return devPosts;
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