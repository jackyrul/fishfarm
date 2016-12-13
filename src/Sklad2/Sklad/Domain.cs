//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sklad
{
    public class DbEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime CreatedAt { get; set; }
    }

    public class NamedDbEntity<T> : DbEntity<T>
    {
        [Required]
        public string Name { get; set; }
    }

    public class Material : NamedDbEntity<int>
    {
    }

    public class User : NamedDbEntity<int>
    {
    }

    public class Worker : NamedDbEntity<int>
    {
    }

    public class Stage : NamedDbEntity<int>
    {
    }

    public class ApplicationUser : IdentityUser
    {

    }

    public class PipelineRule : DbEntity<int>
    {
        [ForeignKey("FromId")]
        [Required]
        public virtual Stage From { get; set; }
        [ForeignKey("ToId")]
        [Required]
        public virtual Stage To { get; set; }

        public int? FromId { get; set; }
        public int? ToId { get; set; }
    }

    public class Order : DbEntity<Guid>
    {
        [Required]
        public virtual Material Material { get; set; }
        [Required]
        public int Kgs { get; set; }
        [Required]
        public int Bags { get; set; }
        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime ActionedAt { get; set; }
        [Required]
        public virtual Worker ActionedBy { get; set; }
        [Required]
        public virtual Stage From { get; set; }
        [Required]
        public virtual Stage To { get; set; }
    }

    public class PerStage
    {
        public Stage Stage { get; set; }
        public DateTime ActionedAt { get; set; }
        public IEnumerable<Order> FromOrders { get; set; }
        public IEnumerable<Order> ToOrders { get; set; }
    }

    public class OrdersContext : DbContext
    {
        public OrdersContext() : base("Sklad")
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<PipelineRule> PipelineRules { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasRequired(c => c.From)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Order>()
                .HasRequired(c => c.To)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineRule>()
                .HasRequired(c => c.From)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PipelineRule>()
                .HasRequired(c => c.To)
                .WithMany()
                .WillCascadeOnDelete(false);
            //modelBuilder.Entity<ApplicationUser>();

            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            //modelBuilder.Entity<ApplicationUser>().HasKey<string>(a => a.Id).ToTable("AspNetUsers");
            //modelBuilder.Entity<IdentityUser>().Property(p => p.Id).HasColumnName("Id");
            //modelBuilder.Entity<IdentityUser>().ToTable("NetUsers");
            //modelBuilder.Entity<IdentityUserRole<string>>();

            //modelBuilder.Entity<IdentityUserRole<string>>()
            //    .Property(p =>  p.UserId);
            //modelBuilder.Entity<IdentityUserRole<string>>().Property(p => p.UserId);
            //    modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("AspNetUserRoles");
            //Map(a =>
            //{
            //    //a.MapInheritedProperties();
            //    //a.Properties(p => new { p.RoleId, p.UserId });
            //    a.ToTable("AspNetUserRoles");
            //    //a.HasTableAnnotation("Relational:TableName", "AspNetUserRoles");
            //    //a.HasKey(r => new { r.RoleId, r.UserId });
            //});
            //modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey }).ToTable("AspNetUserLogins");


            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");

            //modelBuilder.Entity<IdentityUserLogin<>>().HasKey(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id).Property(r => r.Name).HasColumnAnnotation("MaxLength", 256);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            ////////
            //modelBuilder
            //   .HasAnnotation("ProductVersion", "7.0.0-beta8")
            //   .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            //modelBuilder.Entity<IdentityRole>().Property(r => r.Id);
            //modelBuilder.Entity<IdentityRole>().Property(r => r.Name).HasColumnAnnotation("MaxLength", 256);
            //modelBuilder.Entity<IdentityRole>().Property(r => r.ConcurrencyStamp);
            //modelBuilder.Entity<IdentityRole>().Property(r => r.NormalizedName).HasColumnAnnotation("MaxLength", 256);
            //modelBuilder.Entity<IdentityRole>().Property(r => r.NormalizedName).HasColumnAnnotation("Relational:Name", "RoleNameIndex");
            //modelBuilder.Entity<IdentityRole>().Property(r => r.).HasColumnAnnotation("Relational:Name", "RoleNameIndex");


            //HasTableAnnotation("MaxLength", 256)           
            //.ToTable("AspNetRoles");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles");
            //modelBuilder.Entity<IdentityRole>().Map(a => a.Property<string>("Id"));

        }

        private IEnumerable<PerStage> GetPerStage(Expression<Func<Order, bool>> prefilter, Func<PerStage, bool> postfilter)
        {
            using (var ctx = new OrdersContext())
            {
                var empty = new Order[0];
                var ret =
                    ctx.Orders.
                        Where(prefilter).
                        GroupBy(o => new { o.ActionedAt, o.From, o.To }).
                        ToList().
                        SelectMany(g => new[] { new PerStage { Stage = g.Key.From, ActionedAt = g.Key.ActionedAt, FromOrders = g, ToOrders = empty },
                                                new PerStage { Stage = g.Key.To, ActionedAt = g.Key.ActionedAt, FromOrders = empty, ToOrders = g } }).
                        GroupBy(g => new { g.Stage, g.ActionedAt },
                            (k, g) => g.Aggregate((state, e) => new PerStage { Stage = state.Stage, ActionedAt = state.ActionedAt, FromOrders = state.FromOrders.Concat(e.FromOrders), ToOrders = state.ToOrders.Concat(e.ToOrders) })).
                        Select(oo => new PerStage { Stage = oo.Stage, ActionedAt = oo.ActionedAt, FromOrders = oo.FromOrders.ToArray(), ToOrders = oo.ToOrders.ToArray() }).
                        Where(postfilter).
                        ToList();

                return ret;
            }
        }

        public IEnumerable<PerStage> GetForStage(int stageid) => GetPerStage(o => o.From.Id == stageid || o.To.Id == stageid, ps => ps.Stage.Id == stageid);
        public IEnumerable<PerStage> GetForAllStages() => GetPerStage(o => true, ps => true);

    }
}
