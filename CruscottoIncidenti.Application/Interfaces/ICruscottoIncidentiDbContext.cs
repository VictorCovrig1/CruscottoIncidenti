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

        DbSet<Incident> Incidents { get; set; }

        DbSet<IncidentType> IncidentTypes { get; set; }

        DbSet<Ambit> Ambits { get; set; }

        DbSet<Origin> Origin { get; set; }

        DbSet<Scenario> Scenario { get; set; }

        DbSet<Threat> Threat { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        //DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
