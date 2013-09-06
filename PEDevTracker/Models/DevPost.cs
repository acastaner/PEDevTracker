using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace PEDevTracker.Models
{
    public class DevPost
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime RetrieveDate { get; set; }
        public virtual string Author { get; set; }
        public virtual string Content { get; set; }
        public virtual Uri Uri { get; set; }

        public DevPost()
        {
            this.RetrieveDate = DateTime.Now;
        }

        /// <summary>
        ///  Parses the provided HtmlNode containing the developer's post and set the properties of the object appropriately.
        /// </summary>
        /// <param name="postNode"></param>
        public void ImportPost(HtmlNode postNode)
        {
            SetOriginalPostUri(postNode);
            SetTime(postNode);
            SetContent(postNode);
        }

        /// <summary>
        /// Extracts original post Uri from provided HTML node and set the object attribute value appropriately.
        /// </summary>
        /// <param name="postNode"></param>
        private void SetOriginalPostUri(HtmlNode postNode)
        {
            this.Uri = new Uri(postNode.SelectSingleNode(".//a").Attributes[0].Value);
        }

        /// <summary>
        /// Extracts original post time from provided HTML node and set the object attribute value appropriately.
        /// </summary>
        /// <param name="postNode"></param>
        private void SetTime(HtmlNode postNode)
        {
            string postTime = postNode.SelectSingleNode(".//p[@class='posted_info']").InnerText;
            postTime = postTime.Replace("\t", "");
            postTime = postTime.Replace("\n", "");
        }

        /// <summary>
        /// Extracts original post content (including quotes etc...) from provided HTML node and set the object attribute value appropriately.
        /// </summary>
        /// <param name="postNode"></param>
        private void SetContent(HtmlNode postNode)
        {
            this.Content = postNode.SelectSingleNode(".//div[@class='post']").InnerHtml;
        }
    }
}