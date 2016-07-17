using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using fishfarm.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using fishfarm.Models;
using Microsoft.AspNet.Mvc.Rendering;

namespace fishfarm.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class WebApiController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/WebApi
        //[HttpGet]
        //public IEnumerable<ApplicationUser> GetApplicationUser()
        //{
        //    return _context.ApplicationUser;
        //}

        // GET: api/WebApi/5
        [HttpGet]//("/GetRestiredUser", Name = "GetRestiredUser")]
        public IActionResult GetRestiredUser()
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }
            var applicationUser = GetCurrentUserAsync();
            //id = "19cb6cb4-3ae1-4df8-b258-52dc6cca3fdd";
            //ApplicationUser applicationUser = new ApplicationUser()
            //{ Email = "jackyrul@mail.ru",Id = "1",Roles = { new IdentityUserRole<string>() {RoleId = "1"} },UserName = "Jackyrul"};
            //ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);

            if (applicationUser.Result == null)
            {
                return HttpNotFound();
            }

            return Ok(applicationUser);
        }

        // GET: api/WebApi/5
        [Authorize]
        [HttpGet("{id}", Name = "GetApplicationUser")]
        public IActionResult GetApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }
            id = "19cb6cb4-3ae1-4df8-b258-52dc6cca3fdd";
            //ApplicationUser applicationUser = new ApplicationUser()
            //{ Email = "jackyrul@mail.ru",Id = "1",Roles = { new IdentityUserRole<string>() {RoleId = "1"} },UserName = "Jackyrul"};
            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return Ok(applicationUser);
        }
        [Route("CreateRole")]
        //[HttpGet(Name = "CreateRole")]
        public IActionResult CreateRole()
        {
            List<string> rolesList = new List<string>();
            rolesList.Add("Admin");
            try
            {
                _context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = rolesList[0]//collection["RoleName"]
                });
                _context.SaveChanges();
                //ViewBag.ResultMessage = "Role created successfully !";
                var roles = _context.Roles.ToList();
                return Ok(roles);
            }
            catch
            {
                return null;
            }
        }

        [Route("AddRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult RoleAddToUser()
        {
            var id = "19cb6cb4-3ae1-4df8-b258-52dc6cca3fdd";
            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            var count = _userManager.GetRolesAsync(applicationUser).Result.Select(r => r.Contains("Admin")).FirstOrDefault();
            if (!count)
            {
                var tt = AddRoleToUser(applicationUser, "Admin");

                //ViewBag.ResultMessage = "Role created successfully !";

                // prepopulat roles for the view dropdown
                var list = _context.UserRoles.ToList();
                ViewBag.Roles = list;

                //applicationUser.Roles.Add(new IdentityUserRole { RoleId = role.Id });

                return Ok(list);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        //[Route("GetUserRoles")]
        [HttpGet("GetUserRoles/{username}")]
        public IActionResult GetUserRoles([FromRoute] string UserName)
        {
            object roles = null;
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                roles = _userManager.GetRolesAsync(user);
            }
            else
            {
                return HttpNotFound();
            }

            return Ok(roles);
        }

        public async Task AddRoleToUser(ApplicationUser applicationUser,string role)
        {
            await _userManager.AddToRoleAsync(applicationUser, "Admin");
        }


        // DELETE: api/WebApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteApplicationUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            _context.ApplicationUser.Remove(applicationUser);
            _context.SaveChanges();

            return Ok(applicationUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Count(e => e.Id == id) > 0;
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
    }
}