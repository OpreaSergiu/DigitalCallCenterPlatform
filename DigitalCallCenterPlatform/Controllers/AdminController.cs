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
                var logs = new LogsModels();
                string user_name = User.Identity.GetUserName();
                var currentDate = DateTime.Now;
                logs.Action = "Add account.";
                logs.UserEmail = user_name;
                logs.Date = currentDate;

                db.LogsModels.Add(logs);
                db.SaveChanges();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, "Default123!");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);

                    var userClientId = new UserClientidModels
                    {
                        UserEmail = model.Email,
                        ClientId = ""
                    };

                    db.UserClientidModels.Add(userClientId);
                    db.SaveChanges();

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

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
        public async Task<ActionResult> ChangeRoleAsync(string id, string role, string type)
        {
            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Change Roles list.";
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

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

        public ActionResult Clientids()
        {
            var users = db.Users.ToList();

            var model_list = new List<AdminClientModel>
            { };

            foreach (var item in users)
            {
                string clientid_list = "";

                var clientUserList = db.UserClientidModels.Where(u => u.UserEmail == item.Email);

                foreach (var client in clientUserList)
                {
                    clientid_list = clientid_list + client.ClientId + " ";
                }
                 
                var user_clients = new AdminClientModel()
                {
                    Id = item.Id,
                    Username = item.Email,
                    Client = clientid_list
                };

                model_list.Add(user_clients);
            }

            var model = new AdminClientsViewModel()
            {
                AdminClientList = model_list
            };

            return View(model);
        }


        public ActionResult ChangeClientids(string id = "empty")
        {
            // Check for null
            if (id == "empty")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var models = new AdminClientModel()
            {
                Id = id,
                Username = db.Users.Find(id).Email,
                Client = ""
            };

            return View(models);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeClientIdAsync(string id, string client, string type)
        {
            var Username = db.Users.Find(id).Email;
            var clientUserList = db.UserClientidModels.Where(u => u.UserEmail == Username).Where(c => c.ClientId == client);

            var logs = new LogsModels();
            string user_name = User.Identity.GetUserName();
            var currentDate = DateTime.Now;
            logs.Action = "Change Client ID list.";
            logs.UserEmail = user_name;
            logs.Date = currentDate;

            db.LogsModels.Add(logs);
            db.SaveChanges();

            if (type == "Add")
            {
                if (clientUserList.Count() == 0)
                {
                    var userClientId = new UserClientidModels
                    {
                        UserEmail = Username,
                        ClientId = client
                    };

                    db.UserClientidModels.Add(userClientId);
                    db.SaveChanges();
                }
            }
            else
            {
                if (clientUserList.Count() > 0)
                {
                    var clientUserRemove = db.UserClientidModels.Where(u => u.UserEmail == Username).Where(c => c.ClientId == client).SingleOrDefault();
                    db.UserClientidModels.Remove(clientUserRemove);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Clientids");
        }

        public ActionResult Logs()
        {
            return View(db.LogsModels.OrderByDescending(s => s.Id).ToList());
        }
    }
}