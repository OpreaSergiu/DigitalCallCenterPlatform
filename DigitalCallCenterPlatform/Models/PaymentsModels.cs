using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class PaymentsModels
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string ClientID { get; set; }
        public string ClientReference { get; set; }
        public string Invoice { get; set; }
        public float Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PaymentDate { get; set; }
        public bool Approve { get; set; }
        public bool PostedFlag { get; set; }
    }
}