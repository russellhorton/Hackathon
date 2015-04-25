using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Objects
{
    public class HeartRateItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HeartRate { get; set; }
        public DateTime DateTime { get; set; }
        public int SyncId { get; set; }
    }
}