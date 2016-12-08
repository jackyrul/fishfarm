﻿//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Data.Entity;
using Sklad.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;


namespace Sklad.Web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public Microsoft.EntityFrameworkCore.DbSet<ApplicationUser> ApplicationUser { get; set; }
    }

}
