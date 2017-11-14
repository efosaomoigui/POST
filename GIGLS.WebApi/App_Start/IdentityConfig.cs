using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using GIGLS.Infrastructure.Persistence;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Services.Implementation.Messaging;
using System;
using GIGLS.CORE.Domain;
using GIGLS.Infrastructure.IdentityInfrastrure;

namespace GIGLS.WebApi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            GIGLSContext db = new GIGLSContext();
            ApplicationUserManager manager = new ApplicationUserManager(new GiglsUserStore<User>(db));

            manager.EmailService = new EmailService();

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            //manager.SmsService = new Smsservice(); //Not created yet
            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return manager;
        }

    }

    public class ApplicationRoleManager : RoleManager<AppRole>
    {
        public ApplicationRoleManager(IRoleStore<AppRole, string> roleStore)
            : base(roleStore) 
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<AppRole>(context.Get<GIGLSContext>()));

            return appRoleManager;
        }
    }
}
