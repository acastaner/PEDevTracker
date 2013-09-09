using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using FluentNHibernate.Mapping;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace PEDevTracker.Models
{
    #region DevPost class
    public class DevPost
    {
        #region Attributes
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime RetrieveDate { get; set; }
        public virtual Developer Author { get; set; }
        public virtual string Content { get; set; }
        public virtual Uri Uri { get; set; }
        public virtual string ContentHash { get; set; }
        #endregion
        #region Constructors
        public DevPost()
        {
            this.RetrieveDate = DateTime.Now;
        }
        #endregion
        #region Methods
        /// <summary>
        ///  Parses the provided HtmlNode containing the developer's post and sets the properties of the object appropriately.
        ///  This function also checks for duplicate and won't import them.
        /// </summary>
        /// <param name="postNode"></param>
        public virtual void ImportPost(HtmlNode postNode)
        {
            // First we parse the content, and check if we already have that post
            SetContent(postNode);
            SetContentHash();

            var s = HibernateModule.CreateSession();
            // If the previous query returned nothing, we don't have that post yet
            // so continue parsing and save into db
            if (!this.PostExists())
            {
                SetOriginalPostUri(postNode);
                SetTime(postNode);
                var f = this.Content.Length;
                var t = s.BeginTransaction();
                var length = this.Content.Length;
                s.Save(this);
                t.Commit();
            }
            else
            {
                Console.WriteLine(DateTime.Now + " Did not import this post.");
            }
        }

        public virtual bool PostExists()
        {
            var s = HibernateModule.CreateSession();
            var matchingHash = s.QueryOver<DevPost>()
                                .Where(x => x.ContentHash == this.ContentHash)
                                .OrderBy(x => x.RetrieveDate).Desc
                                .SingleOrDefault<DevPost>();

            bool exists = (matchingHash == null) ? false : true;
            return exists;
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
            postTime = postTime.Replace(",", "");

            this.Date = ParseObsidianTime(postTime);
            this.RetrieveDate = DateTime.Now;            

        }
        /// <summary>
        /// Parses the non-standard way the datetime is displayed on the page
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime ParseObsidianTime(string time)
        {
            // Ex: 21 August 2013 - 07:48 AM
            // But can also be "Yesterday 07:48 AM"
            string[] values = time.Split(' ');
            string[] hourAndMinutes = null;
            string amOrPm = null;
            string day = null;
            string month = null;
            string year = null;

            // If string is "Yesterday 07:48 AM"
            if (values.Length == 3)
            {
                hourAndMinutes = values[1].Split(':');
                amOrPm = values[2];
                DateTime yesterday = DateTime.Now;
                yesterday = yesterday.AddDays(-1);
                day = yesterday.ToString("dd", CultureInfo.InvariantCulture);
                month = yesterday.ToString("MMMM", CultureInfo.InvariantCulture);
                year = yesterday.ToString("yyyy", CultureInfo.InvariantCulture);
            }
            else if (values.Length == 6)
            {
                hourAndMinutes = values[4].Split(':');
                day = values[0];
                month = values[1];
                year = values[2];
                amOrPm = values[5];                
            }

            int hour = Int16.Parse(hourAndMinutes[0]);
            int minutes = Int16.Parse(hourAndMinutes[1]);

            if (amOrPm == "PM")
            {
                hour = hour + 12;
            }

            // August 21 2013 07:48 (or 19:48 if PM)
            // which is :
            // MMMM dd yyyy H:mm
            string convertedDateFormat = month + " " + day + " " + year + " " + hour + ":" + minutes;
            DateTime finalTime = DateTime.ParseExact(convertedDateFormat, "MMMM dd yyyy H:m", CultureInfo.InvariantCulture);

            return finalTime;
        }

        /// <summary>
        /// Extracts original post content (including quotes etc...) from provided HTML node and set the object attribute value appropriately.
        /// </summary>
        /// <param name="postNode"></param>
        private void SetContent(HtmlNode postNode)
        {
            this.Content = postNode.SelectSingleNode(".//div[@class='post']").InnerHtml;
        }
        /// <summary>
        /// Sets the ContentHash value based on the Content value.
        /// Used to check for duplicates.
        /// </summary>
        private void SetContentHash()
        {
            if (!String.IsNullOrEmpty(this.Content))
            {
                this.ContentHash = CalculateMD5Hash(this.Content);
            }
            else
            {
                throw new Exception("Cannot parse a null or empty string.");
            }            
        }

        private string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion
    }
    #endregion
    #region Fluent NHibernate Mappings
    public class DevPostMap : ClassMap<DevPost>
    {
        public DevPostMap()
        {
            Table("pedt_DevPost"); 
            Id(x => x.Id);
            Map(x => x.Date);
            Map(x => x.RetrieveDate);
            References(x => x.Author);
            Map(x => x.Content).Length(10000);
            Map(x => x.Uri);
            Map(x => x.ContentHash);
        }
    }
    #endregion
}