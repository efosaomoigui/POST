using GIGL.GIGLS.Core.Domain;
using GIGLS.CORE.Domain;
using GIGLS.Infrastructure.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace GIGLS.Infrastructure.IdentityInfrastrure
{
    public class GiglsUserStore<TUser> : UserStore<TUser, AppRole, string, IdentityUserLogin, IdentityUserRole, AppUserClaim>, IUserStore<TUser>, IUserStore<TUser, string>, IDisposable where TUser : User
    {

            public GiglsUserStore(GIGLSContext context) : base(context) { }
    }
}
