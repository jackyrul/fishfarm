namespace Sklad.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<OrdersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OrdersContext context)
        {
            //  This method will be called after migrating to the latest version.

            var dt = DateTime.UtcNow;

            context.Users.AddOrUpdate(
                u => u.Name,
                Names.Users.Select(u => new User { Name = u, CreatedBy = "System", CreatedAt = dt }).ToArray());

            context.Workers.AddOrUpdate(
                w => w.Name,
                Names.Users.Select(u => new Worker { Name = u, CreatedBy = "System", CreatedAt = dt }).ToArray());

            context.Materials.AddOrUpdate(
                m => m.Name,
                Names.Users.Select(u => new Material { Name = u, CreatedBy = "System", CreatedAt = dt }).ToArray());

            var stages = Names.Stages.Select(s => new Stage { Name = s, CreatedBy = "System", CreatedAt = dt }).ToArray();

            context.Stages.AddOrUpdate(
                s => s.Name,
                stages
            );

            //context.ApplicationUsers.AddOrUpdate(
            //    a => a.Name,
            //    Names.Users.Select(u => new ApplicationUser { Name = u, CreatedBy = "System", CreatedAt = dt }).ToArray());

            context.SaveChanges();

            var savedStages = context.Stages.Where(s => Names.Stages.Contains(s.Name)).ToArray();

            var rulesPairs =
                Names.Stages.
                Zip(Names.Stages.Skip(1), 
                    (s1, s2) => new PipelineRule { From = savedStages.First(s => s.Name == s1), To = savedStages.First(s => s.Name == s2), CreatedBy = "System", CreatedAt = dt }).
                ToArray();

            context.PipelineRules.AddOrUpdate(
                r => new { r.FromId, r.ToId },
                rulesPairs
            );

        }
    }

    //partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    //{
    //    protected override void BuildModel(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder
    //           .HasAnnotation("ProductVersion", "7.0.0-beta8")
    //           .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

    //        modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
    //        {
    //            b.Property<string>("Id");

    //            b.Property<string>("ConcurrencyStamp")
    //                .IsConcurrencyToken();

    //            b.Property<string>("Name")
    //                .HasAnnotation("MaxLength", 256);

    //            b.Property<string>("NormalizedName")
    //                .HasAnnotation("MaxLength", 256);

    //            b.HasKey("Id");

    //            b.HasIndex("NormalizedName")
    //                .HasAnnotation("Relational:Name", "RoleNameIndex");

    //            b.HasAnnotation("Relational:TableName", "AspNetRoles");
    //        });


    //        modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
    //        {
    //            b.Property<string>("UserId");

    //            b.Property<string>("RoleId");

    //            b.HasKey("UserId", "RoleId");

    //            b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
    //        });

    //        modelBuilder.Entity("Farm.Web.Models.ApplicationUser", b =>
    //        {
    //            b.Property<string>("Id");

    //            b.Property<int>("AccessFailedCount");

    //            b.Property<string>("ConcurrencyStamp")
    //                .IsConcurrencyToken();

    //            b.Property<string>("Email")
    //                .HasAnnotation("MaxLength", 256);

    //            b.Property<bool>("EmailConfirmed");

    //            b.Property<bool>("LockoutEnabled");

    //            b.Property<DateTimeOffset?>("LockoutEnd");

    //            b.Property<string>("NormalizedEmail")
    //                .HasAnnotation("MaxLength", 256);

    //            b.Property<string>("NormalizedUserName")
    //                .HasAnnotation("MaxLength", 256);

    //            b.Property<string>("PasswordHash");

    //            b.Property<string>("PhoneNumber");

    //            b.Property<bool>("PhoneNumberConfirmed");

    //            b.Property<string>("SecurityStamp");

    //            b.Property<bool>("TwoFactorEnabled");

    //            b.Property<string>("UserName")
    //                .HasAnnotation("MaxLength", 256);

    //            b.HasKey("Id");

    //            b.HasIndex("NormalizedEmail")
    //                .HasAnnotation("Relational:Name", "EmailIndex");

    //            b.HasIndex("NormalizedUserName")
    //                .HasAnnotation("Relational:Name", "UserNameIndex");

    //            b.HasAnnotation("Relational:TableName", "AspNetUsers");
    //        });

    //        modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
    //        {
    //            b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
    //                .WithMany()
    //                .HasForeignKey("RoleId");

    //            b.HasOne("Farm.Web.Models.ApplicationUser")
    //                .WithMany()
    //                .HasForeignKey("UserId");
    //        });
    //    }
    //}

}
