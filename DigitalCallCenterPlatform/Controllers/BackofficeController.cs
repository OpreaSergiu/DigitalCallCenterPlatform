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
    public class BackofficeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewBusiness()
        {
            return View();
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
    }
}