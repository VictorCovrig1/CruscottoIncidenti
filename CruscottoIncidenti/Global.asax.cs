using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacSerilogIntegration;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using CruscottoIncidenti.Application;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Infrastructure.Persistance;
using CruscottoIncidenti.Infrastructure.Services;
using MediatR.Extensions.Autofac.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace CruscottoIncidenti
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable propFerty injection into action filters.
            builder.RegisterFilterProvider();

            // Register controllers all at once using assembly scanning...
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register Application services.
            builder.AddMediatR(typeof(ApplicationModule).Assembly);
            builder.AddAutoMapper(typeof(ApplicationModule).Assembly);

            // Register Infrastructure services.
            builder.RegisterType<CruscottoIncidentiDbContext>()
                .As<ICruscottoIncidentiDbContext>().InstancePerRequest();
            builder.RegisterType<DateTimeService>().As<IDateTime>().InstancePerDependency();
            builder.RegisterType<CurrentUserService>().As<ICurrentUserService>().InstancePerRequest();

            // Register logging
            string logFileAbsolutePath = Path.Combine(Server.MapPath("~"), "Logs", "Log-.json");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(a =>
                {
                    a.File(new CompactJsonFormatter(), logFileAbsolutePath,
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Error,
                        shared: true);
                    a.Debug();
                })
                .CreateLogger();

            builder.RegisterLogger();

            // Add the Configuration service that uses the data from web.config.
            //builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
