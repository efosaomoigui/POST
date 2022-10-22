using GIGL.POST.Core.Domain;
using POST.CORE.Domain;
using POST.Infrastructure.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace POST.Infrastructure.IdentityInfrastrure
{
    public class GiglsUserStore<TUser> : UserStore<TUser, AppRole, string, IdentityUserLogin, IdentityUserRole, AppUserClaim>, IUserStore<TUser>, IUserStore<TUser, string>, IDisposable where TUser : User
    {
            public GiglsUserStore(GIGLSContext context) : base(context) { }
    }
}
