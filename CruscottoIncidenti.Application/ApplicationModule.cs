using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace CruscottoIncidenti.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddMediatR(typeof(ApplicationModule).Assembly);
        }
    }
}
