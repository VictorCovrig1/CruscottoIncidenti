using System.Data.Entity.ModelConfiguration;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Infrastructure.Persistance.Configurations
{
    public class UserToRoleConfiguration : EntityTypeConfiguration<UserToRole>
    {
        public UserToRoleConfiguration()
        {
            HasKey(x => new { x.UserId, x.RoleId });

            HasRequired(x => x.Role)
                .WithMany(x => x.RoleUsers)
                .HasForeignKey(x => x.RoleId);

            HasRequired(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);

            ToTable("UserRoles");
        }
    }
}
