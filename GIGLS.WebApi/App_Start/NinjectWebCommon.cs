[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GIGLS.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GIGLS.WebApi.App_Start.NinjectWebCommon), "Stop")]
namespace GIGLS.WebApi.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;
    //using System.Web.Http;
    using Ninject.Web.WebApi;
    using Core.IServices.Customers;
    using Services.Implementation.Customers;
    using Core;
    using Infrastructure.Persistence;
    using Ninject.Syntax;
    using GIGL.GIGLS.Core.Repositories;
    using Core.IServices;
    using INFRASTRUCTURE.Persistence.Repositories.User;
    using GIGLS.Core.IServices.Shipments;
    using GIGLS.Services.Business.Magaya.Shipment;

    //using Hangfire;
    //using GlobalConfiguration = Hangfire.GlobalConfiguration;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                
                kernel.Bind<GIGLSContext>().ToSelf().InRequestScope();
                kernel.Bind<IUnitOfWork>().To<UnitOfWork<GIGLSContext>>().InRequestScope();
                
                RegisterRepositoriesByConvention(kernel);
                RegisterAuthRepositoriesByConvention(kernel);
                RegisterServicesByConvention(kernel);
                //GlobalConfiguration.Configuration.UseNinjectActivator(kernel);
                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
               
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICompanyService>().To<CompanyService>();
            kernel.Bind<IMagayaService>().To<MagayaService>(); 
            kernel.Bind<IUnitOfWork>().To<UnitOfWork<GIGLSContext>>().InRequestScope();
            kernel.Bind<GIGLSContext>().ToSelf().InRequestScope();
            kernel.Bind<UserRepository>().ToSelf().InRequestScope();
            //kernel.Bind<IMagayaService>().ToSelf().InRequestScope();
            //kernel.Bind<AuthRepository>().ToSelf().InRequestScope();
        }

        private static void RegisterRepositoriesByConvention(IBindingRoot root)
        {
            root.Bind(convention => convention
                .FromAssembliesMatching("*")
                .SelectAllClasses()
                .InheritedFrom(typeof(IRepository<>))
                .BindDefaultInterfaces()
            );
        }


        private static void RegisterAuthRepositoriesByConvention(IBindingRoot root)
        {
            root.Bind(convention => convention
                .FromAssembliesMatching("*")
                .SelectAllClasses()
                .InheritedFrom(typeof(IAuthRepository<>))
                .BindDefaultInterfaces()
            );
        }

        private static void RegisterServicesByConvention(IBindingRoot root)
        {
            root.Bind(convention => convention
                .FromAssembliesMatching("*")
                .SelectAllClasses()
                .InheritedFrom(typeof(IServiceDependencyMarker))
                .BindDefaultInterfaces()
            );
        }
    }
}
