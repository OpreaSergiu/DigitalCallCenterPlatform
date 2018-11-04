using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class WorkPlatformModels
    {
        public int Id { get; set; }
        public string ClientReference { get; set; }
        public string ClientID { get; set; }
        public string Name { get; set; }
        public float AssignAmount { get; set; }
        public float TotalReceived { get; set; }
        public float OtherDue { get; set; }
        public float TotalDue { get; set; }
        public string Desk { get; set; }
        public string Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PalacementDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastWorkDate { get; set; }
    }
}