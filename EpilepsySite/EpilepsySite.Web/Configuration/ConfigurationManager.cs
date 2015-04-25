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

    }
}