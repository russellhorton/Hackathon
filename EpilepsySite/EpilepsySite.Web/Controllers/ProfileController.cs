using EpilepsySite.Web.Data;
using EpilepsySite.Web.Helpers;
using EpilepsySite.Web.Models;
using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace EpilepsySite.Web.Controllers
{
    [MemberAuthorize]
    public class ProfileController: SurfaceController
    {
       
        public ActionResult GetProfile()
        {

            ProfileModel model = new ProfileModel();            

            MembershipUser user = Membership.GetUser();
            int userId = (int)user.ProviderUserKey;

            IMember member = Services.MemberService.GetById(userId);

            string typeOfUser = member.ContentTypeAlias;
            model.ConfirmedConnections = new List<IMember>();
            model.PendingConnections = new List<IMember>();
            model.UserId = userId;

            if (typeOfUser == "Patient")
            {
                List<PatientGuardianLink> links = Data.PatientGuardian.GetAllGuardiansForPatient(member.Id);
                List<PatientGuardianLink> confirmedLinks = links.Where(l => l.Status == GuardianStatus.Confirmed).ToList();
                List<PatientGuardianLink> pendingLinks = links.Where(l => l.Status == GuardianStatus.Pending).ToList();
                model.ConfirmedConnections = LoadLinkedGuardians(confirmedLinks);
                model.PendingConnections = LoadLinkedGuardians(pendingLinks);
            }
            else if (typeOfUser == "Guardian")
            {
                List<PatientGuardianLink> links = Data.PatientGuardian.GetAllPatientsForGuardian(member.Id);
                List<PatientGuardianLink> confirmedLinks = links.Where(l => l.Status == GuardianStatus.Confirmed).ToList();
                List<PatientGuardianLink> pendingLinks = links.Where(l => l.Status == GuardianStatus.Pending).ToList();
                model.ConfirmedConnections = LoadLinkedPatients(confirmedLinks);
                model.PendingConnections = LoadLinkedPatients(pendingLinks);
            }

            model.FirstName = member.Name.Split(' ')[0];
            model.LastName = member.Name.Split(' ')[1];
            model.EmailAddress = member.Email;
            model.UserType = typeOfUser;

            return PartialView("Profile", model);

        }

        public ActionResult UpdateProfile(ProfileModel profile)
        {

            IMember member = Services.MemberService.GetById(profile.UserId);

            member.Email = profile.EmailAddress;
            member.Name = string.Format("{0} {1}", profile.FirstName, profile.LastName);

            if (!string.IsNullOrEmpty(profile.Password)) 
            {
                Services.MemberService.SavePassword(member, profile.Password);
            }

            if (!string.IsNullOrEmpty(profile.NewConnections))
            {
                foreach (string email in profile.NewConnections.Split(','))
                {

                    IMember guardian = Services.MemberService.CreateMember(email, email, "anon anon", "Guardian");
                    Services.MemberService.Save(guardian);

                    PatientGuardianLink link = new PatientGuardianLink{
                        PatientId = member.Id,
                        GuardianId = guardian.Id,
                        DateRequested = DateTime.Now,
                        Status = GuardianStatus.Pending
                    };

                    if (PatientGuardian.InsertPatientGuardianLink(link)){

                    EmailHelper.SendEmail(
                        email.Trim(), 
                        member.Email, 
                        "Request to be a guardian", 
                        string.Format("Go here to confirm your connection to this patient: http://epilexy.azurewebsites.net/confirmconnection/?linkid={0}", link.Id));                        
                    }
                }

            }


            return Redirect("/profile/");
        }

        private List<Umbraco.Core.Models.IMember> LoadLinkedGuardians(List<PatientGuardianLink> links)
        {
            List<Umbraco.Core.Models.IMember> members = new List<Umbraco.Core.Models.IMember>();

            foreach(PatientGuardianLink link in links)
            {
                Umbraco.Core.Models.IMember member = Services.MemberService.GetById(link.GuardianId);
                members.Add(member);
            }

            return members;
        }

        private List<Umbraco.Core.Models.IMember> LoadLinkedPatients(List<PatientGuardianLink> links)
        {
            List<Umbraco.Core.Models.IMember> members = new List<Umbraco.Core.Models.IMember>();

            foreach(PatientGuardianLink link in links)
            {
                Umbraco.Core.Models.IMember member = Services.MemberService.GetById(link.PatientId);
                members.Add(member);
            }

            return members;
        }
    }
}