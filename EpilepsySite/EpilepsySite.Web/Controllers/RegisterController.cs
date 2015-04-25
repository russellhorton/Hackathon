﻿using System;
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

            IMember newMember = Services.MemberService.CreateMember(model.EmailAddress, model.EmailAddress, string.Format("{0} {1}", model.FirstName, model.LastName), "Member");
            Services.MemberService.Save(newMember);
            Services.MemberService.SavePassword(newMember, model.Password);

            model.UserId = newMember.Id;

            return PartialView("RegisterSuccess", model);
        }

        [HttpPost]
        public JsonResult Register(string jsonUser)
        {
            try
            {
                User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(jsonUser);
                IMember newMember = Services.MemberService.CreateMember(user.EmailAddress, user.EmailAddress, string.Format("{0} {1}", user.FirstName, user.LastName), "Member");
                Services.MemberService.Save(newMember);
                Services.MemberService.SavePassword(newMember, user.Password);

                user.UserId = newMember.Id;

                return Json(Newtonsoft.Json.JsonConvert.SerializeObject(user));

            } 
            catch(Exception ex)
            {
                return Json("Error " + ex.Message);
            }

            
        }

    }
}