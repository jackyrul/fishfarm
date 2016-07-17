using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.IUser;

namespace fishfarm.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }
    public class Admin : IdentityUser
    {

    }
}
