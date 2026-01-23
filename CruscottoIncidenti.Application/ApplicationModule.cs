using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace CruscottoIncidenti.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddMediatR(typeof(ApplicationModule).Assembly);
            builder.AddAutoMapper(typeof(ApplicationModule).Assembly);
        }
    }
}
