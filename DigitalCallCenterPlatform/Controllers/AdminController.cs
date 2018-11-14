using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DigitalCallCenterPlatform.Models;
using System.Net;
using System.Collections.Generic;

namespace DigitalCallCenterPlatform.Controllers
{
    public class AdminController : Controller
    {
        // Database Connection
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public AdminController()
        {
        }


        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAccount(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, "Default123!");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Accounts");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AddAccount()
        {
            var model = new AdminRegisterViewModel()
            {
                roles = new string[] { "Agent", "Client", "Backoffice", "Admin" }
            };

            return View(model);
        }

        public ActionResult Accounts()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Roles()
        {
            var users = db.Users.ToList();

            var model_list = new List<AdminRolesModel>
            { };

            foreach (var item in users)
            {
                string role = "";

                if (UserManager.IsInRole(item.Id, "Agent"))
                    role = "Agent";
                if (UserManager.IsInRole(item.Id, "Client"))
                    role = "Client" + " " + role;
                if (UserManager.IsInRole(item.Id, "Backoffice"))
                    role = "Backoffice" + " " + role;
                if (UserManager.IsInRole(item.Id, "Admin"))
                    role = "Admin" + " " + role;

                var user_role = new AdminRolesModel()
                {
                        Id = item.Id,
                        Username = item.Email,
                        Role = role
                };

                model_list.Add(user_role);
            }

            var model = new AdminRolesViewModel()
            {
                AdminRolesList = model_list
            };

            return View(model);
        }


        public ActionResult ChangeRole(string id = "empty")
        {
            // Check for null
            if (id == "empty")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var models = new AdminRolesModel()
            {
                Id = id,
                Username = db.Users.Find(id).Email,
                Role = ""
            };

            return View(models);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoleAsync(string id, string role, string type)
        {
            if (type == "Add")
            {
                if (role == "Agent")
                    if (!UserManager.IsInRole(id, "Agent"))
                        await UserManager.AddToRoleAsync(id, "Agent");
                if (role == "Client")
                    if (!UserManager.IsInRole(id, "Client"))
                        await UserManager.AddToRoleAsync(id, "Client");
                if (role == "Backoffice")
                    if (!UserManager.IsInRole(id, "Backoffice"))
                        await UserManager.AddToRoleAsync(id, "Backoffice");
                if (role == "Admin")
                    if (!UserManager.IsInRole(id, "Admin"))
                        await UserManager.AddToRoleAsync(id, "Admin");
            }
            else
            {
                if (role == "Agent")
                    if (UserManager.IsInRole(id, "Agent"))
                        await UserManager.RemoveFromRolesAsync(id, "Agent");
                if (role == "Client")
                    if (UserManager.IsInRole(id, "Client"))
                        await UserManager.RemoveFromRolesAsync(id, "Client");
                if (role == "Backoffice")
                    if (UserManager.IsInRole(id, "Backoffice"))
                        await UserManager.RemoveFromRolesAsync(id, "Backoffice");
                if (role == "Admin")
                    if (UserManager.IsInRole(id, "Admin"))
                        await UserManager.RemoveFromRolesAsync(id, "Admin");
            }

            return RedirectToAction("Roles");
        }
    }
}