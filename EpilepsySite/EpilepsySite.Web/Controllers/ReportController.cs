﻿using EpilepsySite.Web.Data;
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

            if (string.IsNullOrEmpty(Request.QueryString["showraw"]))
            {
                model.heartRateItems = model.heartRateItems.Take(30);
                model.motionSensorItems = model.motionSensorItems.Take(30);
            }

            return PartialView("Reports/ReportTable", model);

        }

        public ActionResult GetMapData()
        {

            int userId;
            if (string.IsNullOrEmpty(Request.QueryString["userid"]))
            {
                var user = Membership.GetUser();
                userId = (int)user.ProviderUserKey;
            }
            else
            {
               userId = int.Parse(Request.QueryString["userid"].ToString());
            }
           

            //IMember member = Services.MemberService.GetById(userId);

            List<SyncItem> syncItems = Sync.GetAllSyncHistory(userId);

            MapModel model = new MapModel();

            model.syncItems = syncItems;

            return PartialView("Reports/MapView", model);

        }
    }
}