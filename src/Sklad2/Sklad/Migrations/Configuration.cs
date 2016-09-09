namespace Sklad.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<Sklad.OrdersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sklad.OrdersContext context)
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
}
