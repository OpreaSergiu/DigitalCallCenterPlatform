﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DigitalCallCenterPlatform.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<WorkPlatformModels> WorkPlatformModels { get; set; }
        public DbSet<PhoneModels> PhoneModels { get; set; }
        public DbSet<NotesModels> NotesModels { get; set; }
        public DbSet<InvoiceModels> InvoiceModels { get; set; }
        public DbSet<AddressModels> AddressModels { get; set; }
        public DbSet<ActionsModels> ActionsModels { get; set; }
        public DbSet<StatusesModels> StatusesModels { get; set; }
        public DbSet<PaymentsModels> PaymentsModels { get; set; }
        public DbSet<UserClientidModels> UserClientidModels { get; set; }
        public DbSet<UserDeskModels> UserDeskModels { get; set; }
        public DbSet<LogsModels> LogsModels { get; set; }
    }
}