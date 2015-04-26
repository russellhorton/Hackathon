using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Objects
{
    public class PatientGuardianLink
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int GuardianId { get; set; }
        public GuardianStatus Status { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateConfirmed { get; set; }
    }
}