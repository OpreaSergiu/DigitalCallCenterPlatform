using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class WorkPlatformAccountViewModels
    {
        public IEnumerable<PhoneModels> Phones { get; set; }
        public IEnumerable<NotesModels> Notes { get; set; }
        public IEnumerable<InvoiceModels> Invoices { get; set; }
        public AddressModels Address { get; set; }
        public WorkPlatformModels Account { get; set; }
        public bool Check { get; set; }
        public IEnumerable<WorkPlatformModels> Inventory { get; set; }
    }
}