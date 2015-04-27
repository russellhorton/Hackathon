using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Models
{
    public class ReportRequest
    {
        public int UserId { get; set; }
        public DateTime TimeSince { get; set; }
    }
}