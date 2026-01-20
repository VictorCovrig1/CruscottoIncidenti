using Autofac;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Infrastructure.Persistance;
using CruscottoIncidenti.Infrastructure.Services;

namespace CruscottoIncidenti.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CruscottoIncidentiDbContext>()
                .As<ICruscottoIncidentiDbContext>().InstancePerRequest();
            builder.RegisterType<DateTimeService>().As<IDateTime>().InstancePerDependency();
            builder.RegisterType<CurrentUserService>().As<ICurrentUserService>().InstancePerRequest();
        }
    }
}
