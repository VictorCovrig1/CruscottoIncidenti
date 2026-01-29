using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class AmbitToTypeConfiguration : EntityTypeConfiguration<AmbitToType>
    {
        public AmbitToTypeConfiguration() 
        {
            HasKey(x => new { x.AmbitId, x.TypeId });

            HasRequired(x => x.Ambit)
                .WithMany(x => x.AmbitToTypes)
                .HasForeignKey(x => x.AmbitId);

            HasRequired(x => x.Type)
                .WithMany(x => x.TypeToAmbits)
                .HasForeignKey(x => x.TypeId);

            ToTable("AmbitsToTypes");
        }
    }
}
