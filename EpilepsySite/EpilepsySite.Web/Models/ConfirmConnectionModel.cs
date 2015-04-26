using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Models
{
    public class ConfirmConnectionModel
    {
        public int LinkId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
    }
}