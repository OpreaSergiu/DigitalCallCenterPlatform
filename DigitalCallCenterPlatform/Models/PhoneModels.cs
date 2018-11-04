using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DigitalCallCenterPlatform.Models
{
    public class PhoneModels
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string Prefix { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
    }
}