using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Application.Interfaces
{
    public interface ICruscottoIncidentiDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<UserToRole> UsersToRoles { get; set; }

        DbSet<Incident> Incidents { get; set; }

        DbSet<IncidentType> IncidentTypes { get; set; }

        DbSet<Ambit> Ambits { get; set; }

        DbSet<AmbitToType> AmbitsToTypes { get; set; }

        DbSet<Origin> Origins { get; set; }

        DbSet<OriginToAmbit> OriginsToAmbits { get; set; }

        DbSet<Scenario> Scenarios { get; set; }

        DbSet<Threat> Threats { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        //DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
