using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration() 
        {
            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(13).IsRequired();
            HasIndex(x => x.Name).IsUnique();
        }
    }
}
