using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class AmbitConfiguration : EntityTypeConfiguration<Ambit>
    {
        public AmbitConfiguration() 
        {
            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(100).IsRequired();
            HasIndex(x => x.Name).IsUnique();

            HasMany(x => x.IncidentTypes).
            WithMany(x => x.Ambits).
            Map(cs =>
            {
                cs.MapLeftKey("AmbitId");
                cs.MapRightKey("TypeId");
                cs.ToTable("AmbitsToTypes");
            });

            HasMany(x => x.Origins).
            WithMany(x => x.Ambits).
            Map(cs =>
            {
                cs.MapLeftKey("AmbitId");
                cs.MapRightKey("OriginId");
                cs.ToTable("OriginsToAmbit");
            });
        }
    }
}
