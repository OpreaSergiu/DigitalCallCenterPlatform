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

        public ActionResult PaymentPost()
        {
            return View();
        }
    }
}