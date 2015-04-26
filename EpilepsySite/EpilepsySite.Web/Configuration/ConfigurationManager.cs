using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Configuration
{
    public class ConfigurationManager
    {

        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.
                    ConnectionStrings["umbracoDbDSN"].ConnectionString;
            }
        }
        public static string SendGridPassword { get { return "LJp52w4wTZMEhGh";  } }
        public static string SmtpServer { get { return "smtp.sendgrid.net"; } }
        public static string SendGridUserName { get { return "azure_bba846e22c21c5a9d2392a1724d799dd@azure.com";  } }
        public static string GoogleStaticMapsAPIKey { get { return "AIzaSyAIcHANQpZyNXiFtq0vYrie1XXgiabsEoo";  } }
    }
}