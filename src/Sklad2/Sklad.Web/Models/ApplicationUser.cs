using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.IUser;

namespace Sklad.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }
    public class Admin : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser
    {

    }
}
