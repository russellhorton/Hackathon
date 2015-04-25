using EpilepsySite.Web.Models;
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
    public class SecurityController: SurfaceController
    {

        [HttpGet]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();

            return PartialView("Login", model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {

            if (Membership.ValidateUser(model.Username, model.Password))
            {
                MembershipUser member = Membership.GetUser(model.Username);
                FormsAuthentication.SetAuthCookie(model.Username,false);
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                model.Message = "Bad username and password";
                return PartialView("login", model);
            }
            
        }

    }
}