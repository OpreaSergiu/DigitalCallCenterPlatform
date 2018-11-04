using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class AddressModels
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
    }
}