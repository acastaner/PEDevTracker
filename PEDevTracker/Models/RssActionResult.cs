﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PEDevTracker.Models
{
    public class RssActionResult : ActionResult
        {
            public Encoding ContentEncoding { get; set; }
            public string ContentType { get; set; }

            private readonly SyndicationFeedFormatter feed;
            public SyndicationFeedFormatter Feed
            {
                get { return feed; }
            }

            public RssActionResult(SyndicationFeedFormatter feed)
            {
                this.feed = feed;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/rss+xml";

                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;

                if (feed != null)
                    using (var xmlWriter = new XmlTextWriter(response.Output))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        feed.WriteTo(xmlWriter);
                    }
            }
        }
}