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

            // An empty model that will be use to avoit id errors and diplay blank pages
            var emptyModel = new WorkPlatformAccountViewModels()
            {           
                Account = db.WorkPlatformModels.Where(m => m.Id == -1).SingleOrDefault(),

                Phones = db.PhoneModels.Where(m => m.AccountNumber == -1),

                Address = db.AddressModels.Where(m => m.AccountNumber == -1).SingleOrDefault(),

                Invoices = db.InvoiceModels.Where(m => m.AccountNumber == -1),

                Notes = db.NotesModels.Where(m => m.AccountNumber == -1).OrderByDescending(s => s.SeqNumber),

                Actions = db.ActionsModels.Where(m => m.Id == -1),

                Statuses = db.StatusesModels.Where(m => m.Id == -1),

                Check = true

            };

            // Get the data that will be provided to the view
            var model = new WorkPlatformAccountViewModels()
            {
                Account = db.WorkPlatformModels.Find(id),

                Phones = db.PhoneModels.Where(m => m.AccountNumber == id),

                Address = db.AddressModels.Where(m => m.AccountNumber == id).SingleOrDefault(),

                Invoices = db.InvoiceModels.Where(m => m.AccountNumber == id),

                Notes = db.NotesModels.Where(m => m.AccountNumber == id).OrderByDescending(s => s.SeqNumber),

                Actions = db.ActionsModels.OrderBy(a => a.Action),

                Statuses = db.StatusesModels.OrderBy(s => s.Status),

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