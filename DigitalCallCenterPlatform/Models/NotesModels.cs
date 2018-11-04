using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class NotesModels
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public int SeqNumber { get; set; }
        public string UserCode { get; set; }
        public string ActionCode { get; set; }
        public string Status { get; set; }
        public string Desk { get; set; }
        public string Note { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime? NoteDate { get; set; }
    }
}