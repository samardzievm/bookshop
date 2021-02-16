using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        // purpose of this model is to modify the Identity Tables (for example, add the user Name to form)
        public string FullName { get; set; }
    }
}
