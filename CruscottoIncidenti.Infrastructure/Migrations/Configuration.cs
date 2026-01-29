using System;
using System.Collections.Generic;
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
                UserName = "CRME001",
                IsEnabled = true,
                Password = "f1490065b81c7f1e406793b5c89994d5c21123ebba42a81a8dfcbfdf77a57d6e",
            };

            if(!adminUser.UserRoles.Any(x => 
                x.UserId == adminUser.Id && x.RoleId == adminRole.Id))
            {
                adminUser.UserRoles.Add(new UserToRole
                {
                    User = adminUser,
                    UserId = adminUser.Id,
                    Role = adminRole,
                    RoleId = adminRole.Id
                });
            }

            if (!adminUser.UserRoles.Any(x => 
                x.UserId == adminUser.Id && x.RoleId == operatorRole.Id))
            {
                adminUser.UserRoles.Add(new UserToRole
                {
                    User = adminUser,
                    UserId = adminUser.Id,
                    Role = operatorRole,
                    RoleId = operatorRole.Id
                });
            }

            if (!adminUser.UserRoles.Any(x => 
                x.UserId == adminUser.Id && x.RoleId == userRole.Id))
            {
                adminUser.UserRoles.Add(new UserToRole
                {
                    User = adminUser,
                    UserId = adminUser.Id,
                    Role = userRole,
                    RoleId = userRole.Id
                });
            }

            if (!context.Users.Any(x => x.Email == adminUser.Email))
                context.Users.AddOrUpdate(adminUser);

            var origin = new Origin()
            {
                Name = "Applicativa"
            };

            var ambit = new Ambit()
            {
                Name = "Software"
            };

            var incidentType = new IncidentType()
            {
                Name = "Saturazione risorse"
            };

            if(!origin.OriginToAmbits.Any(x => x.AmbitId == ambit.Id && x.OriginId == origin.Id))
            {
                origin.OriginToAmbits.Add(new OriginToAmbit()
                {
                    Origin = origin,
                    OriginId = origin.Id,
                    Ambit = ambit,
                    AmbitId = ambit.Id,
                });
            }

            if (!ambit.AmbitToTypes.Any(x => x.AmbitId == ambit.Id && x.TypeId == incidentType.Id))
            {
                ambit.AmbitToTypes.Add(new AmbitToType()
                {
                    Type = incidentType,
                    TypeId = incidentType.Id,
                    Ambit = ambit,
                    AmbitId = ambit.Id,
                });
            }

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
                CreatedBy = adminUser.Id,
                RequestNr = "HOST0000000000001",
                Subsystem = "AA",
                OpenDate = DateTime.Now,
                Type = "Incident",
                ApplicationType = "Incident",
                Urgency = 2,
                SubCause = "Problem with login",
                ProblemSumary = "Cannot login into application",
                ProblemDescription = "Some description",
                IncidentTypeId = incidentType.Id,
                IncidentType = incidentType,
                AmbitId = ambit.Id,
                Ambit = ambit,
                OriginId = origin.Id,
                Origin = origin,
                ThreatId = threat.Id,
                Threat = threat,
                ScenarioId = scenario.Id,
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
