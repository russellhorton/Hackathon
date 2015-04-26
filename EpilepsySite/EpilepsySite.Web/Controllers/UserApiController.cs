using EpilepsySite.Web.Configuration;
using EpilepsySite.Web.Data;
using EpilepsySite.Web.Helpers;
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
                FormsAuthentication.SetAuthCookie(request.Username, false);
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

                IMember newMember = Services.MemberService.CreateMember(request.EmailAddress, request.EmailAddress, string.Format("{0} {1}", request.FirstName, request.LastName), request.UserType);
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
               Lng = data.Long,
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

        public void SendAlert([FromBody]AlertModel data)
        {
            List<PatientGuardianLink> links = PatientGuardian.GetAllGuardiansForPatient(data.UserId);
            IMember patient = Services.MemberService.GetById(data.UserId);

            List<SyncItem> syncHistory = Sync.GetAllSyncHistory(data.UserId);
            string staticMapLink = string.Empty;

            if (syncHistory.Any())
            {
                SyncItem latestSync = syncHistory[0];
                staticMapLink = string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom=15&size=600x300&key={2}&markers=color:blue%7Clabel:S%7C{0},{1}", latestSync.Lat, latestSync.Lng, ConfigurationManager.GoogleStaticMapsAPIKey);

            }

            
            foreach (PatientGuardianLink link in links)
            {
                IMember guardian = Services.MemberService.GetById(link.GuardianId);
                
                if (guardian != null)
                {
                    try { 
                        EmailHelper.SendEmail(guardian.Email,
                        patient.Email,
                        string.Format("Siezure warning for {0}", patient.Name),
                        string.Format("Please come quick! <br/><img src='{0}' alt='location of {1}'/>", staticMapLink, patient.Name));
                    }
                    catch (Exception ex)
                    {
                        //guess no help is coming.
                    }
                }

            }

        }

    }
}