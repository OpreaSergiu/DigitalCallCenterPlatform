using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class InvoiceModels
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string Invoice { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
        public float Due { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InvoiceDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DueDate { get; set; }
        public bool PaymentRequestFlag { get; set; }
        public bool PostedFlag { get; set; }
    }
}