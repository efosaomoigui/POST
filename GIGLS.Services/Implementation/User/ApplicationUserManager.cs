using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using GIGLS.CORE.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Messaging.MessageService;

namespace GIGLS.Services.Implementation.User
{
    public class ApplicationUserManager : UserManager<GIGL.GIGLS.CORE.Domain.User>
    {
        public ApplicationUserManager(IUserStore<GIGL.GIGLS.CORE.Domain.User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<GIGLSContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<GIGL.GIGLS.CORE.Domain.User>(appDbContext));

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<GIGL.GIGLS.CORE.Domain.User>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            //appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<GIGL.GIGLS.CORE.Domain.User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return appUserManager;
        }
    }
}
