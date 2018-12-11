using DigitalCallCenterPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DigitalCallCenterPlatform.Controllers
{
    public class ClientPortalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        // Audit based on id, each id represent a specific customer account
        public ActionResult Audit(int? id = 0)
        {
            // Check for null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Audit Account " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var user_clients = db.UserClientidModels.Where(b => b.UserEmail == user_name);

            List<string> clients = new List<string>();

            foreach (var item in user_clients)
            {
                clients.Add(item.ClientId);
            }

            clients.ToArray();

            // An empty model that will be use to avoit id errors and diplay blank pages
            var emptyModel = new WorkPlatformAccountViewModels()
            {
                Account = db.WorkPlatformModels.Where(m => m.Id == -1).SingleOrDefault(),

                Phones = db.PhoneModels.Where(m => m.AccountNumber == -1),

                Address = db.AddressModels.Where(m => m.AccountNumber == -1).SingleOrDefault(),

                Invoices = db.InvoiceModels.Where(m => m.AccountNumber == -1),

                Notes = db.NotesModels.Where(m => m.AccountNumber == -1).OrderByDescending(s => s.SeqNumber),

                Inventory = db.WorkPlatformModels.Where(a => clients.Any(s => a.ClientID.Contains(s))).OrderBy(d => d.LastWorkDate),

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

                Inventory = db.WorkPlatformModels.Where(a => clients.Any(s => a.ClientID.Contains(s))).OrderBy(d => d.LastWorkDate),

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
                if (clients.Contains(model.Account.ClientID))
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
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;

            var logs = new LogsModels();
            logs.Action = "Client add note on account " + id.ToString();
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

            string redirectUrl = "/ClientPortal/Audit/" + id;
            return Redirect(redirectUrl);
        }

        public ActionResult ProcessPaymentRequest(int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Client make payment request on account " + id.ToString();
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

            string redirectUrl = "/ClientPortal/Audit/" + AccModel.Id;

            return Redirect(redirectUrl);
        }

        public ActionResult Payments()
        {
            string user_name = User.Identity.GetUserName();
            var user_client = db.UserClientidModels.SingleOrDefault(b => b.UserEmail == user_name);

            return View(db.PaymentsModels.Where(m => m.ClientID == user_client.ClientId).OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult ApprovePayment(int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Client approve payment " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var result = db.PaymentsModels.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                result.Approve = true;
                db.SaveChanges();
            }

            return RedirectToAction("Payments");
        }

        public ActionResult DeletePayment(int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Client delete payment " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var result = db.PaymentsModels.SingleOrDefault(b => b.Id == id);
            var invoice = db.InvoiceModels.SingleOrDefault(i => i.Invoice == result.Invoice);

            if (result != null)
            {
                invoice.PaymentRequestFlag = false;
                db.PaymentsModels.Remove(result);
                db.SaveChanges();
            }

            return RedirectToAction("Payments");
        }

        public ActionResult Inventory()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult NewBusiness(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/NewBusiness/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.FileMessage = "File uploaded successfully. You will be contacted if there are any problems with your file.";

                var logs = new LogsModels();
                string user_name = User.Identity.GetUserName();
                var currentDate = DateTime.Now;
                logs.Action = "Client load inventory file " + postedFile.FileName.ToString();
                logs.UserEmail = user_name;
                logs.Date = currentDate;

                db.LogsModels.Add(logs);
                db.SaveChanges();
            }

            return View("Inventory");
        }
    }
}