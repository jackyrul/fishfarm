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

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var dt = DateTime.UtcNow;

            context.Users.AddOrUpdate(
                u => u.Name,
                new User { Name = "System", CreatedBy = "System", CreatedAt = dt },
                new User { Name = "Admin", CreatedBy = "System", CreatedAt = dt },
                new User { Name = "Katya", CreatedBy = "System", CreatedAt = dt }
            );

            context.Workers.AddOrUpdate(
                w => w.Name,
                new Worker { Name = "KIYEMBA", CreatedBy = "System", CreatedAt = dt },
                new Worker { Name = "OPYENE", CreatedBy = "System", CreatedAt = dt },
                new Worker { Name = "YASIN", CreatedBy = "System", CreatedAt = dt },
                new Worker { Name = "MICHEAL", CreatedBy = "System", CreatedAt = dt }
            );

            context.Materials.AddOrUpdate(
                m => m.Name,
                new Material { Name = "BONES", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "SEASHELLS", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "BLOOD", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "MUKENE", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "YEAST", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "SUNFLOWER CAKE", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "BARLEY ROOTLETS", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "PREMIX", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "CALCIUM MONOPHOSPHATE", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "SALT", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "RECYCLE OF FEED 30%", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "RECYCLE OF FEED 35%", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "SOYA CAKE", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "WHEAT", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "MAIZE", CreatedBy = "System", CreatedAt = dt },
                new Material { Name = "SOYA OIL", CreatedBy = "System", CreatedAt = dt }
            );

            string[] stageNames = {
                "Supply",
                "Raw",
                "Milling",
                "Milled",
                "Extruder",
                "Finish",
                "Inventory"
            };
            var stages = stageNames.Select(s => new Stage { Name = s, CreatedBy = "System", CreatedAt = dt }).ToArray();

            context.Stages.AddOrUpdate(
                s => s.Name,
                stages
            );

            context.SaveChanges();

            var savedStages = context.Stages.Where(s => stageNames.Contains(s.Name)).ToArray();

            var rulesPairs = 
                stageNames.
                Zip(stageNames.Skip(1), 
                    (s1, s2) => new PipelineRule { From = savedStages.First(s => s.Name == s1), To = savedStages.First(s => s.Name == s2), CreatedBy = "System", CreatedAt = dt }).
                ToArray();

            context.PipelineRules.AddOrUpdate(
                r => new { r.FromId, r.ToId },
                rulesPairs
            );

        }
    }
}
