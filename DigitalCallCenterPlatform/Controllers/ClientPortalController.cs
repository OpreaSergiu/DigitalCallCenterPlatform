using DigitalCallCenterPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Payments()
        {
            string user_name = User.Identity.GetUserName();
            var user_client = db.UserClientidModels.SingleOrDefault(b => b.UserEmail == user_name);

            return View(db.PaymentsModels.Where(m => m.ClientID == user_client.ClientId).OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult ApprovePayment(int id)
        {
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

        public ActionResult Audit(int? id = 0)
        {
            return View();
        }
    }
}