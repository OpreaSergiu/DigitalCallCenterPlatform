using DigitalCallCenterPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DigitalCallCenterPlatform.Controllers
{
    public class WorkPlatformController : Controller
    {
        // Database Connection
        private ApplicationDbContext db = new ApplicationDbContext();

        // Index based on id, each id represent a specific customer account
        public ActionResult Index(int? id = 0)
        {
            // Check for null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the data that will be provided to the view
            var model = new WorkPlatformAccountViewModels()
            {
                Account = db.WorkPlatformModels.Find(id),

                Phones = db.PhoneModels.Where(m => m.AccountNumber == id),

                Address = db.AddressModels.Where(m => m.AccountNumber == id).SingleOrDefault(),

                Invoices = db.InvoiceModels.Where(m => m.AccountNumber == id),

                Notes = db.NotesModels.Where(m => m.AccountNumber == id).OrderByDescending(s => s.SeqNumber),

                Check = true
            };

            // Check id that account id exist and if not make the check false
            if (model.Account is null)
            {
                model.Check = false;
            }

            // Return the data to the view
            return View(model);
        }
    }
}