﻿using DigitalCallCenterPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DigitalCallCenterPlatform.Controllers
{
    [Authorize(Roles = "Backoffice")]
    public class BackofficeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewBusiness()
        {
            var model = new NewBusinessViewModel
            {
                Files = Directory.EnumerateFiles("C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\NewBusiness\\")
            };

            return View(model);
        }

        public ActionResult Trust()
        {
            return View();
        }

        public ActionResult PaymentRequest()
        {
            return View(db.PaymentsModels.ToList().Where(f => f.PostedFlag == false).OrderByDescending(d => d.PaymentDate));
        }

        public ActionResult PaymentPost(int id)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Backoffice Operator post payment " + id.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var PayModel = db.PaymentsModels.Find(id); ;
            var AccModel = db.WorkPlatformModels.Find(PayModel.AccountNumber);

            string NewInvoiceStatus = "OPEN";

            float invouce_due = db.InvoiceModels.Where(m => m.AccountNumber == PayModel.AccountNumber).Where(m => m.Invoice == PayModel.Invoice).FirstOrDefault().Due;

            if (invouce_due == PayModel.Amount)
            {
                NewInvoiceStatus = "PAID";
            }

            float NewInvoiceDue = invouce_due - PayModel.Amount;

            string NewAccountStatus = AccModel.Status;

            if (AccModel.TotalDue == PayModel.Amount)
            {
                NewAccountStatus = "CLOSED";
            }

            float NewTotalDue = AccModel.TotalDue - PayModel.Amount;
            float NewTotalReceived = AccModel.TotalReceived + PayModel.Amount;

            var invoice = db.InvoiceModels.Where(m => m.AccountNumber == PayModel.AccountNumber).Where(m => m.Invoice == PayModel.Invoice).FirstOrDefault();
            if (invoice != null)
            {
                invoice.Due = NewInvoiceDue;
                invoice.Status = NewInvoiceStatus;
                invoice.PostedFlag = true;
                db.SaveChanges();
            }

            var account = db.WorkPlatformModels.Where(m => m.Id == PayModel.AccountNumber).FirstOrDefault();
            if (account != null)
            {
                account.TotalReceived = NewTotalReceived;
                account.TotalDue = NewTotalDue;
                account.Status = NewAccountStatus;
                db.SaveChanges();
            }

            var payment = db.PaymentsModels.Where(m => m.Id == id).FirstOrDefault();
            if (payment != null)
            {
                payment.PostedFlag = true;
                db.SaveChanges();
            }

            return RedirectToAction("PaymentRequest");
        }

        public ActionResult Desks()
        {

            var users = db.Users.ToList();

            var model_list = new List<BackofficeDesksModel>
            { };

            foreach (var item in users)
            {
                string desks = "";

                var deskUserList = db.UserDeskModels.Where(u => u.UserEmail == item.Email);

                foreach (var desk in deskUserList)
                {
                    desks = desks + desk.Desk + " ";
                }

                string clientid_list = "";

                var clientUserList = db.UserClientidModels.Where(u => u.UserEmail == item.Email);

                foreach (var client in clientUserList)
                {
                    clientid_list = clientid_list + client.ClientId + " ";
                }

                var user_clients = new BackofficeDesksModel()
                {
                    Id = item.Id,
                    Username = item.Email,
                    Desk = desks,
                    Client = clientid_list
                };

                model_list.Add(user_clients);
            }

            var model = new BackofficeDesksViewModel()
            {
                BackofficeDesksList = model_list
            };

            return View(model);
        }


        public ActionResult ChangeDesks(string id = "empty")
        {
            // Check for null
            if (id == "empty")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Backoffice operator change desks.";
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            var models = new BackofficeDesksModel()
            {
                Id = id,
                Username = db.Users.Find(id).Email,
                Desk = "",
                Client = ""
            };

            return View(models);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeDesksAsync(string id, string desk, string type)
        {
            var Username = db.Users.Find(id).Email;
            var clientDeskList = db.UserDeskModels.Where(u => u.UserEmail == Username).Where(c => c.Desk == desk);

            if (type == "Add")
            {
                if (clientDeskList.Count() == 0)
                {
                    var useDeskId = new UserDeskModels
                    {
                        UserEmail = Username,
                        Desk = desk
                    };

                    db.UserDeskModels.Add(useDeskId);
                    db.SaveChanges();
                }
            }
            else
            {
                if (clientDeskList.Count() > 0)
                {
                    var DeskUserRemove = db.UserDeskModels.Where(u => u.UserEmail == Username).Where(c => c.Desk == desk).SingleOrDefault();
                    db.UserDeskModels.Remove(DeskUserRemove);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Desks");
        }

        private string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Sergiu\AppData\Local\Programs\Python\Python36-32\python.exe";
            start.CreateNoWindow = true;
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    //Console.Write(result);
                    process.WaitForExit();
                    return result;
                }
            }
        }

        [HttpPost]
        public ActionResult Trust(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                var logs = new LogsModels();
                string user_name = User.Identity.GetUserName();
                var currentDate = DateTime.Now;
                logs.Action = "Backoffice operator process trust file: " + postedFile.ToString();
                logs.UserEmail = user_name;
                logs.Date = currentDate;

                db.LogsModels.Add(logs);
                db.SaveChanges();

                string path = Server.MapPath("~/TrustFiles/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.FileMessage = "File uploaded successfully.";

                string fullFilePath = path + Path.GetFileName(postedFile.FileName);
                string fullScriptPath = Server.MapPath("~/TrustScrips/") + "\\trust.py";

                var textResult = run_cmd(fullScriptPath, fullFilePath);

                string[] results = textResult.Split(null);

                ViewBag.InvoiceCount = results[0];
                ViewBag.PaymentAmoun = results[1];

                //if (System.IO.File.Exists(path + Path.GetFileName(postedFile.FileName)))
                //{
                //    System.IO.File.Delete(path + Path.GetFileName(postedFile.FileName));
                //}
            }

            return View("Trust");
        }

        public ActionResult PostNewBusiness(string Name)
        {

            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Backoffice operator process new business file: " + Name.ToString();
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            string path = Server.MapPath("~/NewBusinessFiles/");

            string fullFilePath = path + Path.GetFileName(Name) + " \tN" ;
            string fullScriptPath = Server.MapPath("~/NewBusinessScripts/") + "\\newbusiness.py";

            var textResult = run_cmd(fullScriptPath, fullFilePath);

            string[] results = textResult.Split(null);

            ViewBag.filename = Name;
            ViewBag.accounts = results[0];
            ViewBag.invoices = results[1];
            ViewBag.balance = results[2];

            var model = new NewBusinessViewModel
            {
                Files = Directory.EnumerateFiles("C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\NewBusiness\\")
            };

            return View("NewBusiness",model);
        }

        public ActionResult CommitNewBusinessFile(string Name)
        {

            if (Name != null)
            {
                var logs = new LogsModels();
                string user_name = User.Identity.GetUserName();
                var currentDate = DateTime.Now;
                logs.Action = "Backoffice operator commit new business file: " + Name.ToString();
                logs.UserEmail = user_name;
                logs.Date = currentDate;

                db.LogsModels.Add(logs);
                db.SaveChanges();

                string path = Server.MapPath("~/NewBusinessFiles/");
                string destPath = Server.MapPath("~/NewBusinessProcessed/");

                string fullFilePath = path + Path.GetFileName(Name) + " \tY";
                string fullScriptPath = Server.MapPath("~/NewBusinessScripts/") + "\\newbusiness.py";

                var textResult = run_cmd(fullScriptPath, fullFilePath);

                string[] results = textResult.Split(null);

                ViewBag.filename = Name;
                ViewBag.accounts = results[0];
                ViewBag.invoices = results[1];
                ViewBag.balance = results[2];

                if (System.IO.File.Exists(path + Path.GetFileName(Name)))
                {
                    System.IO.File.Move(path + Path.GetFileName(Name), destPath + Path.GetFileName(Name));
                }

                var model = new NewBusinessViewModel
                {
                    Files = Directory.EnumerateFiles("C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\NewBusiness\\")
                };

                return View("NewBusiness", model);
            }
            else
            {
                string redirectUrl = "/Backoffice/NewBusiness";
                return Redirect(redirectUrl);
            }
        }

        public ActionResult IgnoreResults(string Name)
        {
            string redirectUrl = "/Backoffice/NewBusiness";
            return Redirect(redirectUrl);
        }
    }
}