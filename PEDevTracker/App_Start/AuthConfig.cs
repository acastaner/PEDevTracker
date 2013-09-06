using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using PEDevTracker.Models;

namespace PEDevTracker
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "VCr3LxJ3QONRW0kijvUWog",
                consumerSecret: "Mm5sXg9rRpdtBDKH9qR2e97rYwNrcXe09LrOomNdhbQ");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "238740206274804",
                appSecret: "577a3ebe61875475f4e951ec080b9cd3");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
