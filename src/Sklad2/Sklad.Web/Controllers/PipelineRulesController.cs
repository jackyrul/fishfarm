using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sklad.Web.Models;

namespace Sklad.Web.Controllers
{
    [Route("api/[controller]")]
    public class PipelineRuleController : Controller
    {
        [HttpGet]
        public IEnumerable<PipelineRule> Get()
        {
            using (var ctx = new OrdersContext())
            {
                return ctx.PipelineRules;
            }
        }

        [HttpGet("{id}")]
        public PipelineRule Get(int id)
        {
            using (var ctx = new OrdersContext())
            {
                return ctx.PipelineRules.FirstOrDefault(x => x.Id == id);
            }
        }

        [HttpPost]
        public void Post([FromBody]PipelineRuleSave value)
        {
            using (var ctx = new OrdersContext())
            {
                var val = new PipelineRule
                {
                    Id = 0,
                    FromId = value.FromId,
                    ToId = value.ToId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = this.User.Identity.Name
                };
                ctx.PipelineRules.Add(val);
                ctx.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]PipelineRuleSave value)
        {
            using (var ctx = new OrdersContext())
            {
                var val = ctx.PipelineRules.FirstOrDefault(x => x.Id == id);
                var insert = false;
                if (val == null)
                {
                    val = new PipelineRule
                    {
                        Id = id,
                    };
                }
                val.FromId = value.FromId;
                val.ToId = value.ToId;
                val.CreatedAt = DateTime.UtcNow;
                val.CreatedBy = this.User.Identity.Name;
                if (insert)
                {
                    ctx.PipelineRules.Add(val);
                }
                ctx.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = new OrdersContext())
            {
                var val = ctx.PipelineRules.FirstOrDefault(x => x.Id == id);
                if (val != null)
                {
                    ctx.PipelineRules.Remove(val);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
