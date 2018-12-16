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

            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now; ;

            if (id != 0)
            {
                var logs = new LogsModels();
                logs.Action = "Agent access account " + id.ToString();
                logs.UserEmail = user_name;
                logs.Date = currentDate;

                db.LogsModels.Add(logs);
                db.SaveChanges();
            }

            var user_desk = db.UserDeskModels.SingleOrDefault(b => b.UserEmail == user_name);

            // An empty model that will be use to avoit id errors and diplay blank pages
            var emptyModel = new WorkPlatformAccountViewModels()
            {           
                Account = db.WorkPlatformModels.Where(m => m.Id == -1).SingleOrDefault(),

                Phones = db.PhoneModels.Where(m => m.AccountNumber == -1),

                Address = db.AddressModels.Where(m => m.AccountNumber == -1).SingleOrDefault(),

                Invoices = db.InvoiceModels.Where(m => m.AccountNumber == -1),

                Notes = db.NotesModels.Where(m => m.AccountNumber == -1).OrderByDescending(s => s.SeqNumber),

                Inventory = db.WorkPlatformModels.Where(m => m.Desk == user_desk.Desk).Where(m => m.Status != "CLOSED"),

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

                Inventory = db.WorkPlatformModels.Where(m => m.Desk == user_desk.Desk).Where(m => m.Status != "CLOSED"),

                Actions = db.ActionsModels.OrderBy(a => a.Action),

                Statuses = db.StatusesModels.OrderBy(s => s.Status),

                Check = true
            };

            // Check id that account id exist and if not make the check false
            if (model.Account is null)
            {
                model.Check = false;
            }
            else
            {
                if ((model.Account.Desk == user_desk.Desk) | (User.IsInRole("Admin")))
                {
                    return View(model);
                }
                else
                {
                    return View(emptyModel);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNote(string actioncode, string status, string note, int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Agent post note on account " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            int maxAge = db.NotesModels.Where(m => m.AccountNumber == id).Max(p => p.SeqNumber);

            var user_desk = db.UserDeskModels.SingleOrDefault(b => b.UserEmail == user_name);

            var result = db.WorkPlatformModels.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                result.LastWorkDate = currentDate;
                result.Desk = user_desk.Desk;
                result.Status = status;
                db.SaveChanges();
            }

            var newNote = new NotesModels();

            newNote.AccountNumber = id;
            newNote.ActionCode = actioncode;
            newNote.Status = status;
            newNote.Note = note;
            newNote.SeqNumber = maxAge + 1;
            newNote.NoteDate = currentDate;
            newNote.UserCode = user_desk.Desk;
            newNote.Desk = user_desk.Desk;

            db.NotesModels.Add(newNote);
            db.SaveChanges();

            string redirectUrl = "/WorkPlatform/Index/" + id;
            return Redirect(redirectUrl);
        }

        public ActionResult ProcessPaymentRequest(int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Agent post payment request for " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var result = db.InvoiceModels.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                result.PaymentRequestFlag = true;
                db.SaveChanges();
            }

            var newPayment = new PaymentsModels();
            var InvModel = db.InvoiceModels.Find(id);
            var AccModel = db.WorkPlatformModels.Find(InvModel.AccountNumber);

            newPayment.AccountNumber = AccModel.Id;
            newPayment.ClientID = AccModel.ClientID;
            newPayment.ClientReference = AccModel.ClientReference;
            newPayment.Invoice = InvModel.Invoice;
            newPayment.Amount = InvModel.Due;
            newPayment.PaymentDate = currentDate;
            newPayment.Approve = false;
            newPayment.PostedFlag = false;

            db.PaymentsModels.Add(newPayment);
            db.SaveChanges();

            string redirectUrl = "/WorkPlatform/Index/" + AccModel.Id;

            return Redirect(redirectUrl);
        }


    }
}