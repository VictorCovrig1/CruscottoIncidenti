using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.UserName).HasMaxLength(7).IsRequired();
            HasIndex(x => x.UserName).IsUnique();

            Property(x => x.FullName).HasMaxLength(150).IsRequired();

            Property(x => x.Email).HasMaxLength(254).IsRequired();
            HasIndex(x => x.Email).IsUnique();

            Property(x => x.Password).IsRequired();
        }
    }
}
