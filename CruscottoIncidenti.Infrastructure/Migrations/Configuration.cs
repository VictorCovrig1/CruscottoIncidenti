using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CruscottoIncidenti.Domain.Entities;
using CruscottoIncidenti.Infrastructure.Persistance;

namespace CruscottoIncidenti.Infrastructure.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CruscottoIncidentiDbContext>
    {
        public Configuration() => AutomaticMigrationsEnabled = false;

        protected override void Seed(CruscottoIncidentiDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var adminRole = new Role()
            {
                Id = 1,
                Name = "Administrator",
                Description = "Manages program users and edits data manually."
            };

            var operatorRole = new Role()
            {
                Id = 2,
                Name = "Operator",
                Description = "Imports data using CSV files."
            };

            var userRole = new Role()
            {
                Id = 3,
                Name = "User",
                Description = "Views incident information."
            };

            context.Roles.AddOrUpdate(adminRole, operatorRole, userRole);

            var adminUser = new User()
            {
                FullName = "Covrig Victor",
                Email = "admin@admin.com",
                UserName = "admin",
                IsEnabled = true,
                Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
            };

            adminUser.UserRoles.Add(new UserToRole
            {
                User = adminUser,
                Role = adminRole,
            });

            adminUser.UserRoles.Add(new UserToRole
            {
                User = adminUser,
                Role = operatorRole,
            });

            adminUser.UserRoles.Add(new UserToRole
            {
                User = adminUser,
                Role = userRole,
            });

            if (!context.Users.Any(x => x.Email == adminUser.Email))
                context.Users.AddOrUpdate(adminUser);

            var origin = new Origin()
            {
                Name = "Esterna"
            };

            var ambit = new Ambit()
            {
                Name = "Funzionalità"
            };

            var incidentType = new IncidentType()
            {
                Name = "Terze Parti"
            };

            origin.OriginToAmbits.Add(new OriginToAmbit()
            {
                Origin = origin,
                Ambit = ambit,
            });

            ambit.AmbitToTypes.Add(new AmbitToType()
            {
                Type = incidentType,
                Ambit = ambit
            });

            var threat = new Threat()
            {
                Name = "New threat"
            };

            var scenario = new Scenario()
            {
                Name = "New scenario"
            };

            var incident = new Incident()
            {
                Created = DateTime.Now,
                CreatedBy = 1,
                RequestNr = "HOST0000000000001",
                Subsystem = "AA",
                OpenDate = DateTime.Now,
                Type = "Incident",
                ApplicationType = "Incident",
                Urgency = "Low",
                SubCause = "Problem with login",
                ProblemSumary = "Cannot login into application",
                ProblemDescription = "Some description",
                IncidentType = incidentType,
                Ambit = ambit,
                Origin = origin,
                Threat = threat,
                Scenario = scenario,
                ThirdParty = "Microsoft",
                IsDeleted = false
            };

            ambit.Incidents.Add(incident);
            origin.Incidents.Add(incident);
            scenario.Incidents.Add(incident);
            threat.Incidents.Add(incident);
            scenario.Incidents.Add(incident);

            if (!context.Origins.Any(x => x.Name == origin.Name))
                context.Origins.AddOrUpdate(origin);

            if (!context.Ambits.Any(x => x.Name == ambit.Name))
                context.Ambits.AddOrUpdate(ambit);

            if (!context.IncidentTypes.Any(x => x.Name == incidentType.Name))
                context.IncidentTypes.AddOrUpdate(incidentType);

            if (!context.Threats.Any(x => x.Name == threat.Name))
                context.Threats.AddOrUpdate(threat);

            if (!context.Scenarios.Any(x => x.Name == scenario.Name))
                context.Scenarios.AddOrUpdate(scenario);

            if (!context.Incidents.Any(x => x.RequestNr == incident.RequestNr))
                context.Incidents.AddOrUpdate(incident);

            base.Seed(context);
        }
    }
}
