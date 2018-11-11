using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class ActionsModels
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
    }
}