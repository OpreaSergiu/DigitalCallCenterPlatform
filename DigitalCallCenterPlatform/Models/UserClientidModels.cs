using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class UserClientidModels
    {
        [Key]
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string ClientId { get; set; }
    }
}