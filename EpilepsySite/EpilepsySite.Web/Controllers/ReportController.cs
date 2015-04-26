using EpilepsySite.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace EpilepsySite.Web.Controllers
{
    public class ReportController: SurfaceController
    {
        public ActionResult GetAllData()
        {

            ReportModel model = new ReportModel();

            if (!string.IsNullOrEmpty(Request.QueryString["userid"]))
            {
                int userid = int.Parse(Request.QueryString["userid"]);
                model.heartRateItems = Data.HeartRate.GetAllHeartRateItemsByUserId(userid);
                model.motionSensorItems = Data.MotionSensor.GetAllMotionSensorItemsByUserId(userid);
            }
            else 
            { 
                model.heartRateItems = Data.HeartRate.GetAllHeartRateItems();
                model.motionSensorItems = Data.MotionSensor.GetAllMotionSensorItems();
            }

            return PartialView("Reports/ReportTable", model);

        }
    }
}