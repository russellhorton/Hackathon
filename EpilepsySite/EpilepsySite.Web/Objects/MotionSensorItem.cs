using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Objects
{
    public class MotionSensorItem
    {
        public int Id { get; set; }
        public float XValue { get; set; }
        public float YValue { get; set; }
        public float ZValue { get; set; }
        public float Gravity { get; set; }
        public int UserId { get; set; }        
        public DateTime DateTime { get; set; }
        public int SyncId { get; set; }
    }
}