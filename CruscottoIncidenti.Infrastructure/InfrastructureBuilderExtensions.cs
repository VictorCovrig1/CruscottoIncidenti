using Autofac;

namespace CruscottoIncidenti.Infrastructure
{
    public static class InfrastructureBuilderExtensions
    {
        public static ContainerBuilder AddInfrastructure(this ContainerBuilder builder)
        {
            builder.RegisterModule(new InfrastructureModule());

            return builder;
        }
    }
}