using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Domain.Entities;
using CruscottoIncidenti.Domain.Interfaces;
using CruscottoIncidenti.Infrastructure.Persistance.Configurations;

namespace CruscottoIncidenti.Infrastructure.Persistance
{
    public class CruscottoIncidentiDbContext : DbContext, ICruscottoIncidentiDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public CruscottoIncidentiDbContext() : base("name=CruscottoIncidentiDBContext") { }

        public CruscottoIncidentiDbContext(ICurrentUserService currentUserService, IDateTime dateTime) : base("name=CruscottoIncidentiDBContext")
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<IncidentType> IncidentTypes { get; set; }

        public DbSet<Ambit> Ambits { get; set; }

        public DbSet<Origin> Origins { get; set; }

        public DbSet<Scenario> Scenarios {  get; set; }

        public DbSet<Threat> Threats {  get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AmbitConfiguration());
            modelBuilder.Configurations.Add(new IncidentConfiguration());
            modelBuilder.Configurations.Add(new IncidentTypeConfiguration());
            modelBuilder.Configurations.Add(new OriginConfiguration());
            modelBuilder.Configurations.Add(new ScenarioConfiguration());
            modelBuilder.Configurations.Add(new ThreatConfiguration());
        }
    }
}
