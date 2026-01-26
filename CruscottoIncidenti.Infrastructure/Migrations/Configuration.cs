using System;
using System.Data.Entity.Migrations;
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
                Id = 1,
                FullName = "Covrig Victor",
                Email = "admin@admin.com",
                UserName = "CRME001",
                IsEnabled = true,
                Password = "f1490065b81c7f1e406793b5c89994d5c21123ebba42a81a8dfcbfdf77a57d6e",
            };

            if(!adminUser.Roles.Contains(adminRole))
                adminUser.Roles.Add(adminRole);

            if(!adminUser.Roles.Contains(operatorRole))
            adminUser.Roles.Add(operatorRole);

            if (!adminUser.Roles.Contains(userRole))
                adminUser.Roles.Add(userRole);

            context.Users.AddOrUpdate(adminUser);

            var origin = new Origin()
            {
                Id = 1,
                Name = "Applicativa"
            };

            var ambit = new Ambit()
            {
                Id = 1,
                Name = "Software"
            };

            var incidentType = new IncidentType()
            {
                Id = 1,
                Name = "Saturazione risorse"
            };

            origin.Ambits.Add(ambit);
            ambit.Origins.Add(origin);
            ambit.IncidentTypes.Add(incidentType);
            incidentType.Ambits.Add(ambit);

            var threat = new Threat()
            {
                Id = 1,
                Name = "New threat"
            };

            var scenario = new Scenario()
            {
                Id = 1,
                Name = "New scenario"
            };

            var incident = new Incident()
            { 
                Id = 1,
                Created = DateTime.Now,
                CreatedBy = adminUser.Id,
                RequestNr = "2as3fgs3fggs23fs5",
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

            context.Origins.AddOrUpdate(origin);
            context.Ambits.AddOrUpdate(ambit);
            context.IncidentTypes.AddOrUpdate(incidentType);
            context.Threats.AddOrUpdate(threat);
            context.Scenarios.AddOrUpdate(scenario);
            context.Incidents.AddOrUpdate(incident);

            base.Seed(context);
        }
    }
}
