using System;
using System.Collections.Generic;
using System.Linq;
using Farm.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Sklad;

namespace Farm.Web.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WorkerController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Worker> Get()
        {
            using (var ctx = _context)
            {
                return ctx.Workers.ToArray();
            }
        }

        [HttpGet("{id}")]
        public Worker Get(int id)
        {
            using (var ctx = _context)
            {
                return ctx.Workers.FirstOrDefault(x => x.Id == id);
            }
        }

        [HttpPost]
        public void Post([FromBody]Worker value)
        {
            using (var ctx = _context)
            {
                value.Id = 0; // Making the Id to be set by the Database
                value.CreatedAt = DateTime.UtcNow;
                value.CreatedBy = this.User.Identity.Name;
                ctx.Workers.Add(value);
                ctx.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Worker value)
        {
            using (var ctx = _context)
            {
                value.Id = id;
                var val = ctx.Workers.FirstOrDefault(x => x.Id == id);
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
                    ctx.Workers.Add(value);
                }
                ctx.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = _context)
            {
                var val = ctx.Workers.FirstOrDefault(x => x.Id == id);
                if (val != null)
                {
                    ctx.Workers.Remove(val);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
