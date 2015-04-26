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
    public class GuardianController: SurfaceController
    {
        [HttpGet]
        public ActionResult ConfirmConnection()
        {
            try { 
                FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {
                //3am decision
            }

            int linkId = int.Parse(Request.QueryString["linkid"]);

            PatientGuardianLink link = PatientGuardian.GetPatientsPatientGuardianLink(linkId);

            ConfirmConnectionModel model = new ConfirmConnectionModel();

            IMember member = Services.MemberService.GetById(link.PatientId);

            model.LinkId = linkId;
            model.PatientEmail = member.Email;
            model.PatientName = member.Name;

            return PartialView("ConfirmConnection", model);
        }

        [HttpPost]
        public ActionResult ConfirmConnection(ConfirmConnectionModel model)
        {
            PatientGuardian.UpdatePatientGuardianLink(model.LinkId, GuardianStatus.Confirmed);

            return Redirect("/confirmconnection/Thankyou");
        }

        [HttpPost]
        public ActionResult RejectConnection(ConfirmConnectionModel model)
        {
            PatientGuardian.UpdatePatientGuardianLink(model.LinkId, GuardianStatus.Rejected);

            return Redirect("/confirmconnection/Thankyou");
        }

    }
}