using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Data.Entity;
//using Microsoft.Data.Entity.Infrastructure;
//using Microsoft.Data.Entity.Metadata;
//using Microsoft.Data.Entity.Migrations;
using Sklad.Web.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sklad.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
               .HasAnnotation("ProductVersion", "7.0.0-beta8")
               .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
            {
                b.Property<string>("Id");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Name")
                    .HasAnnotation("MaxLength", 256);

                b.Property<string>("NormalizedName")
                    .HasAnnotation("MaxLength", 256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .HasAnnotation("Relational:Name", "RoleNameIndex");

                b.HasAnnotation("Relational:TableName", "AspNetRoles");
            });


            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId");

                b.Property<string>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
            });

            modelBuilder.Entity("Farm.Web.Models.ApplicationUser", b =>
            {
                b.Property<string>("Id");

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Email")
                    .HasAnnotation("MaxLength", 256);

                b.Property<bool>("EmailConfirmed");

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

                b.Property<string>("NormalizedEmail")
                    .HasAnnotation("MaxLength", 256);

                b.Property<string>("NormalizedUserName")
                    .HasAnnotation("MaxLength", 256);

                b.Property<string>("PasswordHash");

                b.Property<string>("PhoneNumber");

                b.Property<bool>("PhoneNumberConfirmed");

                b.Property<string>("SecurityStamp");

                b.Property<bool>("TwoFactorEnabled");

                b.Property<string>("UserName")
                    .HasAnnotation("MaxLength", 256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasAnnotation("Relational:Name", "EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .HasAnnotation("Relational:Name", "UserNameIndex");

                b.HasAnnotation("Relational:TableName", "AspNetUsers");
            });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                    .WithMany()
                    .HasForeignKey("RoleId");

                b.HasOne("Farm.Web.Models.ApplicationUser")
                    .WithMany()
                    .HasForeignKey("UserId");
            });
        }
    }
}
