using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farm.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Sklad;

namespace Farm.Web.Controllers
{
    [Route("api/[controller]")]
    public class StagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Stage> Get()
        {
            using (var ctx = _context)
            {
                return ctx.Stages.ToArray();
            }
        }

        [HttpGet("{id}")]
        public Stage Get(int id)
        {
            using (var ctx = _context)
            {
                return ctx.Stages.FirstOrDefault(x => x.Id == id);
            }
        }

        [HttpPost]
        public void Post([FromBody]Stage value)
        {
            using (var ctx = _context)
            {
                value.Id = 0; // Making the Id to be set by the Database
                value.CreatedAt = DateTime.UtcNow;
                value.CreatedBy = this.User.Identity.Name;
                ctx.Stages.Add(value);
                ctx.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Stage value)
        {
            using (var ctx = _context)
            {
                value.Id = id;
                var val = ctx.Stages.FirstOrDefault(x => x.Id == id);
                var insert = false;
                if (val == null)
                {
                    val = value;
                    insert = true;
                }
                val.Name = value.Name;
                val.CreatedAt = DateTime.UtcNow;
                val.CreatedBy = this.User.Identity.Name;
                if (insert)
                {
                    ctx.Stages.Add(value);
                }
                ctx.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = _context)
            {
                var val = ctx.Stages.FirstOrDefault(x => x.Id == id);
                if (val != null)
                {
                    ctx.Stages.Remove(val);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
