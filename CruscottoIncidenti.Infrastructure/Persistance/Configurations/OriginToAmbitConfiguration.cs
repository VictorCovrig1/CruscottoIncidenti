using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class OriginToAmbitConfiguration : EntityTypeConfiguration<OriginToAmbit>
    {
        public OriginToAmbitConfiguration()
        {
            HasKey(x => new { x.AmbitId, x.OriginId });

            HasRequired(x => x.Ambit)
                .WithMany(x => x.AmbitToOrigins)
                .HasForeignKey(x => x.AmbitId);

            HasRequired(x => x.Origin)
                .WithMany(x => x.OriginToAmbits)
                .HasForeignKey(x => x.OriginId);

            ToTable("OriginsToAmbit");
        }
    }
}
