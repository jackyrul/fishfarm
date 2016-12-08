using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Sklad.Web.Models;
using Sklad.Web.ViewModels.Account;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Sklad.Web.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class WebApiController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<Sklad.Web.Models.ApplicationUser> _userManager;
        private readonly SignInManager<Sklad.Web.Models.ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public WebApiController(ApplicationDbContext context, UserManager<Sklad.Web.Models.ApplicationUser> userManager,
            SignInManager<Sklad.Web.Models.ApplicationUser> signInManager, ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //[HttpGet("GetCurrentUser")]
        //public IActionResult GetCurrentUser()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    Sklad.Web.Models.ApplicationUser applicationUser = GetCurrentUserAsync();
        //    if (applicationUser.Result == null)
        //    {
        //        return NotFound();
        //    }
        //    var restrictUser = RestrictUserData(applicationUser.Result);

        //    return Ok(restrictUser);
        //}

        // GET: api/WebApi/5
        [Authorize]
        [HttpGet("GetApplicationUser/{id}", Name = "{id}")]
        public IActionResult GetApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            id = "19cb6cb4-3ae1-4df8-b258-52dc6cca3fdd";
            //ApplicationUser applicationUser = new ApplicationUser()
            //{ Email = "jackyrul@mail.ru",Id = "1",Roles = { new IdentityUserRole<string>() {RoleId = "1"} },UserName = "Jackyrul"};
            Sklad.Web.Models.ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("GetUsers")]
        //public IActionResult GetUsers()
        //{
        //    //var restrictUsers = new List<string>();
        //    var users = _context.Users.ToList().Select(RestrictUserData).ToList();//.SelectMany(user=> RestrictUserData(user)).ToList();

        //    return Ok(users);
        //}

        public Dictionary<string, object> RestrictUserData(ApplicationUser applicationUser)
        {
            var role = GetUserRoles(applicationUser.Email);
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("Name", applicationUser.UserName);
            dict.Add("Id", applicationUser.Id);
            dict.Add("Roles", role);

            return dict;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            //var restrictUsers = new List<string>();
            var roles = _context.Roles.ToList().ToList();//.SelectMany(user=> RestrictUserData(user)).ToList();

            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [Route("CreateRole")]
        [HttpGet("CreateRole/{roleName}")]
        public IActionResult CreateRole([FromRoute] string roleName)
        {
            try
            {
                var roles = _context.Roles.Where(r=>r.Name==roleName).ToList();
                if (roles.Count > 0)
                {
                    return BadRequest("Role already exist");
                }
                _context.Roles.Add(new IdentityRole
                {
                    Name = roleName
                });
                _context.SaveChanges();
                return Ok("Role added");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("AddRoleToUser")]
        [Authorize(Roles = "Admin")]
        [HttpGet("AddRoleToUser/{userId}&{roleId}")]
        public async Task<IActionResult> AddRoleToUser([FromRoute] string userId, [FromRoute] string roleId)
        {
            Sklad.Web.Models.ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == userId);

            var role = _context.Roles.Where(r => r.Id == roleId).Select(r => r.Name).FirstOrDefault();
            if (role == null)
                return NotFound();

            var count = _userManager.GetRolesAsync(applicationUser).Result.Select(r => r.Contains(role)).FirstOrDefault();
            if (!count)
            {
                //var tt = AddRoleToUser(applicationUser, "Admin");
                var rr = await _userManager.AddToRoleAsync(applicationUser, role);
                return Ok("Success");
            }
            else
            {
                return Ok("Already Added");
            }

        }

        [Route("ChangeUserRole")]
        [Authorize(Roles = "Admin")]
        [HttpGet("ChangeUserRole/{userId}&{roleId}")]
        public async Task<IActionResult> ChangeUserRole([FromRoute] string userId, [FromRoute] string roleId)
        {
            Sklad.Web.Models.ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == userId);

            var roles = _context.Roles;

            var role = roles.Where(r => r.Id == roleId).Select(r => r.Name).FirstOrDefault();
            if (string.IsNullOrEmpty(role))
                return NotFound("Role doesn't exist");

            var count = _userManager.GetRolesAsync(applicationUser).Result.Select(r => r.Contains(role)).FirstOrDefault();
            if (!count)
            {
                try
                {
                    var userRoles = GetUserRoles(applicationUser.UserName);
                    var remove = await _userManager.RemoveFromRolesAsync(applicationUser, userRoles);//AddToRoleAsync(applicationUser, role);

                    var addrole = await _userManager.AddToRoleAsync(applicationUser, role);
                    return Ok("Success");

                }
                catch (Exception ex)
                {
                    return BadRequest("Error");
                }
            }
            return BadRequest("Already Added");
        }
        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isExist = ApplicationUserExists(model.Email);
                if (isExist)
                {
                    return BadRequest("User already exist");
                }
                var user = new Sklad.Web.Models.ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var thUser = _context.Users.FirstOrDefault(usr => usr.Email == user.Email);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    if (thUser != null)
                    {
                        await AddRoleToUser(thUser.Id, model.Role);
                        _logger.LogInformation(3, "User created a new account with password.");
                        return Ok("Success");
                    }
                    return NotFound("Error");
                }
            }

            // If we got this far, something failed, redisplay form
            return NotFound("Error");
        }

        [HttpPost("EditUser")]
        [Authorize(Roles = "Admin")]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] RegisterViewModel model)
        {
            //TODO:Create Edit User
            //if (ModelState.IsValid)
            //{
            //    var isExist = ApplicationUserExists(model.Email);
            //    if (isExist)
            //    {
            //        return HttpBadRequest("User already exist");
            //    }
            //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //    var result = await _userManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        var thUser = _context.Users.FirstOrDefault(usr => usr.Email == user.Email);
            //        //await _signInManager.SignInAsync(user, isPersistent: false);
            //        if (thUser != null)
            //        {
            //            await AddRoleToUser(thUser.Id, model.Role);
            //            _logger.LogInformation(3, "User created a new account with password.");
            //            return Ok("Success");
            //        }
            //        return HttpNotFound("Error");
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return NotFound("Error");
        }

        //[Route("GetUserRoles")]
        [HttpGet("GetUserRoles/{username}")]
        public IList<string> GetUserRoles([FromRoute] string UserName)
        {
            IList<string> roles = new List<string>();
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Sklad.Web.Models.ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
                roles = _userManager.GetRolesAsync(user).Result;
            }
            //else
            //{
            //    return roles;
            //}

            return roles;
        }

        [HttpPost("Login/{model}")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return Ok("User logged in.");
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return Ok("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok("Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Ok("Something failed. Try Again"); ;
        }
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //await _signInManager.SignOutAsync();
            //_logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // DELETE: api/WebApi/5
        [HttpGet("DeleteUser/{id}")]
        public IActionResult DeleteUser(string email)
        {
            var currentUser = GetCurrentUserAsync();
            if (!ModelState.IsValid || ApplicationUserExists(email) || currentUser.Result.Email != email)
            {
                return BadRequest(ModelState);
            }

            Sklad.Web.Models.ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Email == email);
            if (applicationUser == null)
            {
                return NotFound();
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

        private bool ApplicationUserExists(string email)
        {
            return _context.ApplicationUser.Count(e => e.Email == email) > 0;
        }
        private async Task<Sklad.Web.Models.ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);// _userManager.GetUserIdAsync(ApplicationUser.User);//HttpContext.User.GetUserId()
        }
    }
}