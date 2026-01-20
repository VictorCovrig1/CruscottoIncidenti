using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class ScenarioConfiguration : EntityTypeConfiguration<Scenario>
    {
        public ScenarioConfiguration() 
        {
            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(100).IsRequired();
            HasIndex(x => x.Name).IsUnique();
        }
    }
}
