using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using EpilepsySite.Web.Models;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Umbraco.Core.Models;
using EpilepsySite.Web.Objects;

namespace EpilepsySite.Web.Controllers
{
    public class RegisterController: SurfaceController
    {

        [HttpGet]
        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();

            return PartialView("Register", model);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            IMember newMember = Services.MemberService.CreateMember(model.EmailAddress, model.EmailAddress, string.Format("{0} {1}", model.FirstName, model.LastName), model.UserType);
            Services.MemberService.Save(newMember);
            Services.MemberService.SavePassword(newMember, model.Password);

            model.UserId = newMember.Id;

            return PartialView("RegisterSuccess", model);
        }

        

    }
}