using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalCallCenterPlatform.Controllers
{
    public class ClientPortalController : Controller
    {
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

            return View();
        }

        public ActionResult Audit(int? id = 0)
        {
            return View();
        }
    }
}