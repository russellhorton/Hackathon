using EpilepsySite.Web.Models;
using EpilepsySite.Web.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace EpilepsySite.Web.Controllers
{
    public class ReportApiController : UmbracoApiController
    {
        public string ReturnLatest([FromBody]ReportRequest r)
        {

            ReportModel model = new ReportModel();
            model.heartRateItems = new List<HeartRateItem>();
            model.motionSensorItems = new List<MotionSensorItem>();

            if (r.UserId > 0 && r.TimeSince != default(DateTime))
            {

                model.heartRateItems = Data.HeartRate.GetAllHeartRateItemsByUserIdSinceTime(r.UserId, r.TimeSince);
                model.motionSensorItems = Data.MotionSensor.GetAllMotionSensorItemsByUserIdSinceTime(r.UserId, r.TimeSince);
            }

            return JsonConvert.SerializeObject(model).ToString();

        }

        public string ReturnLast20([FromBody]ReportRequest r)
        {
            ReportModel model = new ReportModel();
            model.heartRateItems = new List<HeartRateItem>();
            model.motionSensorItems = new List<MotionSensorItem>();

            if (r.UserId > 0)
            {
                model.heartRateItems = Data.HeartRate.GetAllHeartRateItemsByUserId(r.UserId);
                model.motionSensorItems = Data.MotionSensor.GetAllMotionSensorItemsByUserId(r.UserId);
            }

            return JsonConvert.SerializeObject(model).ToString();

        }
    }
}