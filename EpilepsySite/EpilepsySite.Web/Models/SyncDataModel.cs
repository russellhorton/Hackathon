using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Models
{
    public class SyncDataModel
    {
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public float Alt { get; set; }
        public float Accuracy { get; set; }
        public List<HeartRateItem> HeartRatePackets { get; set; }
        public List<MotionSensorItem> MotionSensorPackets { get; set; }
    }
}