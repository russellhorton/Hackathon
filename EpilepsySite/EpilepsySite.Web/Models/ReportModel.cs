﻿using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Models
{
    public class ReportModel
    {
        public List<HeartRateItem> heartRateItems { get; set; }
        public List<MotionSensorItem> motionSensorItems { get; set; }
    }
}