using EpilepsySite.Web.Models;
using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace EpilepsySite.Web.Controllers
{
    public class UserApiController : UmbracoApiController
    {
        
        public string AuthenticateUser([FromBody]LoginModel request)
        {

            if (Membership.ValidateUser(request.Username, request.Password))
            {
                MembershipUser member = Membership.GetUser(request.Username);

                return Newtonsoft.Json.JsonConvert.SerializeObject(member).ToString();
            }
            else
            {
                return "Username/Password incorrect";
            }
           
        }

        public string RegisterUser([FromBody]RegisterModel request)
        {
            try
            {

                int found;
                Services.MemberService.FindByEmail(request.EmailAddress,0,1000, out found);

                if (found > 1)
                    throw new Exception("User already exists");

                IMember newMember = Services.MemberService.CreateMember(request.EmailAddress, request.EmailAddress, string.Format("{0} {1}", request.FirstName, request.LastName), "Member");
                Services.MemberService.Save(newMember);
                Services.MemberService.SavePassword(newMember, request.Password);

                return Newtonsoft.Json.JsonConvert.SerializeObject(newMember).ToString();

            }
            catch (Exception ex)
            {
                return string.Format("Error Adding user {0}", ex.Message);
            }
        }

        public string SyncData([FromBody]SyncDataModel data)
        {

            List<HeartRateItem> heartRatePackets = new List<HeartRateItem>();
            List<MotionSensorItem> motionSensorPackets = new List<MotionSensorItem>();
            List<string> errors = new List<string>();

            if (data.HeartRatePackets != null && data.HeartRatePackets.Any())
            {
                heartRatePackets = data.HeartRatePackets;
            }

            if (data.MotionSensorPackets != null && data.MotionSensorPackets.Any())
            {
                motionSensorPackets = data.MotionSensorPackets;
            }

           SyncItem syncItem = new SyncItem
           {
               Accuracy = data.Accuracy,
               Alt = data.Alt,
               DateTime = data.DateTime,
               HeartRatePackets = heartRatePackets,
               MotionSensorPackets = motionSensorPackets,
               Lat = data.Lat,
               Long = data.Long,
               Status = "test",
               UserId = data.UserId
           };

           if (Data.Sync.InsertSyncItem(syncItem))
           {

               foreach (HeartRateItem heartRateItem in heartRatePackets)
               {
                   try 
                   { 
                        heartRateItem.SyncId = syncItem.Id;
                        if (!Data.HeartRate.InsertHeartRateItem(heartRateItem))
                        {
                            throw new Exception("Insert failed");
                        }
                   }
                   catch (Exception ex)
                   {
                       errors.Add("Error adding heart rate item: " + ex.Message);
                   }
               }

               foreach (MotionSensorItem motionSensorItem in motionSensorPackets)
               {
                   try
                   {
                       motionSensorItem.SyncId = syncItem.Id;
                       if (!Data.MotionSensor.InsertMotionSensorItem(motionSensorItem))
                       {
                           throw new Exception("Insert failed");
                       }
                   }
                   catch (Exception ex)
                   {
                       errors.Add("Error adding motion sensor item: " + ex.Message);
                   }
               }

               if (!errors.Any())
                    return "Success";
           }

           return "Completed with errors: " + string.Join(", ", errors);

        }

    }
}