using EpilepsySite.Web.Data;
using EpilepsySite.Web.Models;
using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace EpilepsySite.Web.Controllers
{
    [AuthorizeAttribute]
    public class ReportController: SurfaceController
    {
        [HttpGet]
        public ActionResult GetAllData()
        {
            ReportModel model = new ReportModel();
            int userId = (int)Membership.GetUser().ProviderUserKey;

            model.heartRateItems = Data.HeartRate.GetAllHeartRateItemsByUserId(userId);
            model.motionSensorItems = Data.MotionSensor.GetAllMotionSensorItemsByUserId(userId);
           
            if (string.IsNullOrEmpty(Request.QueryString["showraw"]))
            {
                model.heartRateItems = model.heartRateItems;
                model.motionSensorItems = model.motionSensorItems;
            }

            return PartialView("Reports/ReportTable", model);

        }

        [HttpGet]
        public ActionResult GetMapData()
        {            
            int userId = (int)Membership.GetUser().ProviderUserKey;
          
            List<SyncItem> syncItems = Sync.GetAllSyncHistory(userId);

            MapModel model = new MapModel();

            model.syncItems = syncItems;

            return PartialView("Reports/MapView", model);

        }
    }
}