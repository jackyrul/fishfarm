using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Sklad;

namespace Sklad.Test
{
    public class DomainTests
    {
        [Fact]
        public void ReadAllMaterials()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.Materials.ToArray();
                Assert.NotEmpty(all);
            }

        }

        [Fact]
        public void ReadAllUsers()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.Users.ToArray();
                Assert.NotEmpty(all);
            }

        }

        [Fact]
        public void ReadAllWorkers()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.Workers.ToArray();
                Assert.NotEmpty(all);
            }

        }

        [Fact]
        public void ReadAllStages()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.Stages.ToArray();
                Assert.NotEmpty(all);
            }

        }

        [Fact]
        public void ReadAllPipelineRules()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.PipelineRules.ToArray();
                Assert.NotEmpty(all);
            }

        }

        [Fact]
        public void InsertOneOrder()
        {
            using (var ctx = new OrdersContext())
            {
                var all = ctx.Orders.ToArray();
                ctx.Orders.RemoveRange(all);
                ctx.SaveChanges();
                all = ctx.Orders.ToArray();
                Assert.Empty(all);

                var materialName = Names.Materials[0];
                var material = ctx.Materials.FirstOrDefault(x => x.Name == materialName);
                Assert.NotNull(material);

                var userName = Names.Users[0];
                var user = ctx.Users.FirstOrDefault(x => x.Name == userName);
                Assert.NotNull(user);

                var workerName = Names.Workers[0];
                var worker = ctx.Workers.FirstOrDefault(x => x.Name == workerName);
                Assert.NotNull(worker);

                var stageFromName = Names.Stages[0];
                var stageFrom = ctx.Stages.FirstOrDefault(x => x.Name == stageFromName);
                Assert.NotNull(stageFrom);

                var stageToName = Names.Stages[1];
                var stageTo = ctx.Stages.FirstOrDefault(x => x.Name == stageToName);
                Assert.NotNull(stageTo);

                var order = new Order
                {
                    Material = material,
                    From = stageFrom,
                    To = stageTo,
                    Kgs = 100,
                    Bags = 1,
                    ActionedAt = DateTime.Today.AddHours(12),
                    ActionedBy = worker,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Name
                };

                ctx.Orders.Add(order);
                ctx.SaveChanges();

                Assert.Equal(1, ctx.Orders.Count());

                var perStage = ctx.GetForAllStages().ToArray();
                Assert.Equal(2, perStage.Length);
            }
        }
    }
}
