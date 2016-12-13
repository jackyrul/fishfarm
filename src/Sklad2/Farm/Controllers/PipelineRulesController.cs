using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Farm.Web.Models;
using Sklad;

namespace Farm.Web.Controllers
{
    [Route("api/[controller]")]
    public class PipelineRuleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PipelineRuleController(ApplicationDbContext context)
        {
            _context = context;
        }
        private PipelineRuleGet ToGet(PipelineRule rule) =>
            new PipelineRuleGet
            {
                Id = rule.Id,
                StageFromId = rule.From.Id,
                StageFromName = rule.From.Name,
                StageToId = rule.To.Id,
                StageToName = rule.To.Name,
                CreatedOn = rule.CreatedAt
            };

        [HttpGet]
        public IEnumerable<PipelineRuleGet> Get()
        {
            using (var ctx = _context)
            {
                return ctx.PipelineRules.Select(ToGet).ToArray();
            }
        }

        [HttpGet("{id}")]
        public PipelineRuleGet Get(int id)
        {
            using (var ctx = _context)
            {
                return ctx.PipelineRules.Select(ToGet).FirstOrDefault(x => x.Id == id);
            }
        }

        [HttpPost]
        public void Post([FromBody]PipelineRuleSave value)
        {
            using (var ctx = _context)
            {
                var val = new PipelineRule
                {
                    Id = 0,
                    FromId = value.StageFromId,
                    ToId = value.StageToId,
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
            using (var ctx = _context)
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
                val.FromId = value.StageFromId;
                val.ToId = value.StageToId;
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
            using (var ctx = _context)
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
