//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Data.Entity;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sklad;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Farm.Web.Models
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

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<PipelineRule> PipelineRules { get; set; }
        internal DbSet<Order> Orders { get; set; }
        internal DbSet<Stage> Stages { get; set; }
        internal DbSet<Worker> Workers { get; set; }
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
