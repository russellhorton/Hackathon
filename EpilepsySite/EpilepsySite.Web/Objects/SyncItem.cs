using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Objects
{
    public class SyncItem
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public float Alt { get; set; }
        public float Accuracy { get; set; }
        public List<HeartRateItem> HeartRatePackets { get; set; }
        public List<MotionSensorItem> MotionSensorPackets { get; set; }
    }
}