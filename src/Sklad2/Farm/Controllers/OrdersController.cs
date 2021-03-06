﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Farm.Web.Models;
using Sklad;

namespace Farm.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        private OrderGet ToGet(Order order) =>
            new OrderGet
            {
                Id = order.Id,
                MaterialId = order.Material.Id,
                MaterialName = order.Material.Name,
                Kgs = order.Kgs,
                Bags = order.Bags,
                WorkerId = order.ActionedBy.Id,
                WorkerName = order.ActionedBy.Name,
                StageFromId = order.From.Id,
                StageFromName = order.From.Name,
                StageToId = order.To.Id,
                StageToName = order.To.Name,
                ActionedAt = order.ActionedAt,
                CreatedOn = order.CreatedAt
            };

        private SumsPerStage ToSum(PerStage ps) =>
            new SumsPerStage
            {
                StageId = ps.Stage.Id,
                StageName = ps.Stage.Name,
                InKgs = ps.ToOrders.Sum(o => o.Kgs),
                InBags = ps.ToOrders.Sum(o => o.Bags),
                OutKgs = ps.FromOrders.Sum(o => o.Kgs),
                OutBags = ps.FromOrders.Sum(o => o.Bags)
            };

        // GET api/values
        [HttpGet]
        public IEnumerable<OrderGet> Get()
        {
            using (var ctx = _context)
            {
                return ctx.Orders.Select(ToGet).ToArray();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //public IActionResult Get(Guid id)
        public OrderGet Get(Guid id)
        {
            using (var ctx = _context)
            {
                return ctx.Orders.Select(ToGet).FirstOrDefault(o => o.Id == id);
            }
        }


        [HttpGet("stage/{stageid}")]
        public SumsPerStage GetPerStage(int stageid)
        {
            using (var ctx = _context)
            {
                return ctx.
                    GetForStage(stageid).
                    Select(ToSum).
                    FirstOrDefault();
            }
        }

        [HttpGet("stage")]
        public IEnumerable<SumsPerStage> GetPerStages()
        {
            using (var ctx = _context)
            {
                return ctx.
                    GetForAllStages().
                    Select(ToSum);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]OrderSave order)
        {
            using (var ctx = _context)
            {
                var material = ctx.Materials.FirstOrDefault(m => m.Id == order.MaterialId);
                var stageFrom = ctx.Stages.FirstOrDefault(s => s.Id == order.StageFromId);
                var stageTo = ctx.Stages.FirstOrDefault(s => s.Id == order.StageToId);
                var rule = ctx.PipelineRules.FirstOrDefault(r => r.FromId == order.StageFromId && r.ToId == order.StageToId);
                var worker = ctx.Workers.FirstOrDefault(w => w.Id == order.WorkerId);
                if (material == null) return BadRequest("Material was not found");
                if (stageFrom == null) return BadRequest("Stage From was not found");
                if (stageTo == null) return BadRequest("Stage To was not found");
                if (rule == null) return BadRequest("No Pipeline Rules found for the combination of From and To Stages");
                if (worker == null) return BadRequest("Worker was not found");
                if (order.Kgs < 0) return BadRequest("Negative KG amount was not accepted"); //TODO: <= 0 ?
                if (order.Bags < 0) return BadRequest("Negative number of BAGS was not accepted");

                var ord = new Order
                {
                    Material = material,
                    Kgs = order.Kgs,
                    Bags = order.Bags,
                    ActionedAt = order.ActionedAt,
                    ActionedBy = worker,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = this.User.Identity.Name ?? "No User Provided!", // TODO: Add proper Authentication to the whole service
                    From = stageFrom,
                    To = stageTo
                };
                ctx.Orders.Add(ord);
                ctx.SaveChanges(); // TODO: Add exception handling
                return Ok();
            }
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        //TODO: Add security restriction
        public IActionResult Delete(Guid id)
        {
            //TODO: Add audit
            using (var ctx = _context)
            {
                var order = ctx.Orders.FirstOrDefault(o => o.Id == id);
                ctx.Orders.Remove(order);
                ctx.SaveChanges(); // TODO: Add exception handling
                return order != null ? (IActionResult) Ok() : NotFound();
            }
        }
    }
}
