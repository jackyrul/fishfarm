using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sklad.Web.Controllers
{
    [Route("api/[controller]")]
    public class MaterialsController : Controller
    {
        [HttpGet]
        public IEnumerable<Material> Get()
        {
            using (var ctx = new OrdersContext())
            {
                return ctx.Materials;
            }
        }

        [HttpGet("{id}")]
        public Material Get(int id)
        {
            using (var ctx = new OrdersContext())
            {
                return ctx.Materials.FirstOrDefault(x => x.Id == id);
            }
        }

        [HttpPost]
        public void Post([FromBody]Material value)
        {
            using (var ctx = new OrdersContext())
            {
                value.Id = 0; // Making the Id to be set by the Database
                value.CreatedAt = DateTime.UtcNow;
                value.CreatedBy = this.User.Identity.Name;
                ctx.Materials.Add(value);
                ctx.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Material value)
        {
            using (var ctx = new OrdersContext())
            {
                value.Id = id;
                var val = ctx.Materials.FirstOrDefault(x => x.Id == id);
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
                    ctx.Materials.Add(value);
                }
                ctx.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = new OrdersContext())
            {
                var val = ctx.Materials.FirstOrDefault(x => x.Id == id);
                if (val != null)
                {
                    ctx.Materials.Remove(val);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
