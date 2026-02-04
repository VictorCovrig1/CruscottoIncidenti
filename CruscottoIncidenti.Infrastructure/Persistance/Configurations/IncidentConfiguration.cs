using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class IncidentConfiguration : EntityTypeConfiguration<Incident>
    {
        public IncidentConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.RequestNr).HasMaxLength(17).IsRequired();
            HasIndex(x => x.RequestNr).IsUnique();

            Property(x => x.Subsystem).HasMaxLength(2).IsRequired();

            Property(x => x.Type).HasMaxLength(25).IsRequired();

            Property(x => x.Urgency).HasMaxLength(8).IsRequired();

            Property(x => x.ApplicationType).HasMaxLength(50).IsRequired();

            Property(x => x.SubCause).HasMaxLength(100).IsRequired();

            Property(x => x.ProblemSumary).HasMaxLength(500).IsRequired();

            Property(x => x.ProblemDescription).IsRequired();

            Property(x => x.IncidentTypeId).IsRequired();

            Property(x => x.AmbitId).IsRequired();

            Property(x => x.OriginId).IsRequired();

            Property(x => x.ThreatId).IsRequired();

            Property(x => x.ScenarioId).IsRequired();

            Property(x => x.ThirdParty).HasMaxLength(100).IsRequired();
        }
    }
}
