using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace GIGLS.CORE.Domain
{

    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AppUserClaim : IdentityUserClaim<string>
    {
        public AppUserClaim() : base() { }
        public string SystemRoleId { get; set; }     
    }
}
