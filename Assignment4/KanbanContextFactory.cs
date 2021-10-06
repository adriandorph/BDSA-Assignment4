using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment4.Entities;
using Assignment4.Core;

using System.Collections.Generic;

namespace Assignment4
{
    public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        string connectionString;
        public KanbanContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Kanban");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>()
                .UseSqlServer(connectionString);

            var context = new KanbanContext(optionsBuilder.Options);
            Seed(context);
            
            return context;
        }

        public static void Seed(KanbanContext context)
        {
            context.Database.ExecuteSqlRaw("DELETE dbo.TagTask");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tasks");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
            context.Database.ExecuteSqlRaw("DELETE dbo.Users");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)");

            var tag1 = new Tag { Name = "Drawing" };
            var tag2 = new Tag { Name = "Chair" };
            var tag3 = new Tag { Name = "Kitchen Appliance" };
            var tag4 = new Tag { Name = "Building" };

            var user1 = new User { Name = "Adrian", Email = "email1" };
            var user2 = new User { Name = "Mai", Email = "email2" };
            var user3 = new User { Name = "Sofia", Email = "email@3.dk" };

            var Task1 = new Task { 
                                    Title = "Draw the Chair", 
                                    AssignedTo = user1, 
                                    Description = "A chair has to be drawn. Use Millimeter paper", 
                                    State = State.New, 
                                    Tags = new List<Tag> {tag1, tag2} };
            var Task2 = new Task {
                                    Title = "Draw a window", 
                                    AssignedTo = user2, 
                                    Description = "Draw a window behind the chair", 
                                    State = State.New, 
                                    Tags = new List<Tag>{tag1} };
            var Task3 = new Task {
                                    Title = "Build the chair", 
                                    AssignedTo = user1, 
                                    Description = "UserID 1 needs to build a chair", 
                                    State = State.Active,
                                    Tags = new List<Tag> {tag4, tag2} };
            var Task4 = new Task {
                                    Title = "Order coffeMachine", 
                                    AssignedTo = user2, 
                                    Description = "We need a new coffe machine, the budget is 200,-", 
                                    State = State.Closed, 
                                    Tags = new List<Tag> {tag3} };

            context.Tasks.AddRange(Task1, Task2, Task3, Task4);
            context.Tags.AddRange(tag1, tag2, tag3, tag4);
            context.Users.AddRange(user1, user2, user3);

            context.SaveChanges();
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}