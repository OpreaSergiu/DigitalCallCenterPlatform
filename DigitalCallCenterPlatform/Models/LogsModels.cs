using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class LogsModels
    {
        [Key]
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Action { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime? Date { get; set; }
    }
}