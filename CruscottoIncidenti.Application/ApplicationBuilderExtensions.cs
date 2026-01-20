using Autofac;

namespace CruscottoIncidenti.Application
{
    public static class ApplicationBuilderExtensions
    {
        public static ContainerBuilder AddApplication(this ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());

            return builder;
        }
    }
}
