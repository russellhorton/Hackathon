using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace EpilepsySite.Web.Models
{
    public class ProfileModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        public int UserId { get; set; }

        public string NewConnections { get; set; }

        public List<Umbraco.Core.Models.IMember> ConfirmedConnections { get; set; }
        public List<Umbraco.Core.Models.IMember> PendingConnections { get; set; }


        public IEnumerable<SelectListItem> UserTypes
        {
            get
            {
                List<SelectListItem> userTypes = new List<SelectListItem>();
                
                userTypes.Add(new SelectListItem { Selected = false, Text = "Patient", Value = "Patient"});
                userTypes.Add(new SelectListItem { Selected = false, Text = "Gardian", Value = "Guardian" });
                
               return userTypes;
            }
        }
    }
}