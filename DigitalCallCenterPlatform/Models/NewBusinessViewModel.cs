using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace DigitalCallCenterPlatform.Models
{
    public class NewBusinessViewModel
    {
        public IEnumerable<string> Files { get; set; }
    }
}