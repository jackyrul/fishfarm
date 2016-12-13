﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sklad;
using Farm.Web.Models;

namespace Farm.Web.Controllers
{
    //[Route("api/[controller]")]
    //public class UsersController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    public UsersController(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }
    //    [HttpGet]
    //    public IEnumerable<User> Get()
    //    {
    //        using (var ctx = _context)
    //        {
    //            return ctx.Users.ToArray();
    //        }
    //    }

    //    [HttpGet("{id}")]
    //    public User Get(int id)
    //    {
    //        using (var ctx = _context)
    //        {
    //            return ctx.Users.FirstOrDefault(x => x.Id == id);
    //        }
    //    }

    //    [HttpPost]
    //    public void Post([FromBody]User value)
    //    {
    //        using (var ctx = _context)
    //        {
    //            value.Id = 0; // Making the Id to be set by the Database
    //            value.CreatedAt = DateTime.UtcNow;
    //            value.CreatedBy = this.User.Identity.Name;
    //            ctx.Users.Add(value);
    //            ctx.SaveChanges();
    //        }
    //    }

    //    [HttpPut("{id}")]
    //    public void Put(int id, [FromBody]User value)
    //    {
    //        using (var ctx = _context)
    //        {
    //            value.Id = id;
    //            var val = ctx.Users.FirstOrDefault(x => x.Id == id);
    //            var insert = false;
    //            if (val == null)
    //            {
    //                val = value;
    //                insert = true;
    //            }
    //            val.Name = value.Name;
    //            val.CreatedAt = DateTime.UtcNow;
    //            val.CreatedBy = this.User.Identity.Name;
    //            if (insert)
    //            {
    //                ctx.Users.Add(value);
    //            }
    //            ctx.SaveChanges();
    //        }
    //    }

    //    [HttpDelete("{id}")]
    //    public void Delete(int id)
    //    {
    //        using (var ctx = _context)
    //        {
    //            var val = ctx.Users.FirstOrDefault(x => x.Id == id);
    //            if (val != null)
    //            {
    //                ctx.Users.Remove(val);
    //                ctx.SaveChanges();
    //            }
    //        }
    //    }
    //}
}
