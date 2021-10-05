using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Xunit;
using Assignment4.Core;
using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System.Data;
using System.Data.SqlClient;

namespace Assignment4.Entities.Tests
{
    [TestClass]
    public class TaskRepositoryTests
    {
        TaskRepository taskRepository;
        
        [TestInitialize]
        private void SetupTestData()
        {
            var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=dea8e906-b888-42a5-9e7a-39a1a4cde709";
            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);
            
            seed(context);
            
            taskRepository = new TaskRepository(new SqlConnection(connectionString));
        }

        [Fact]
        public void Create_And_Get_Task()
        {
            //Arrange
            SetupTestData();
        }   

        [Fact]
        public void Find_By_Id() 
        {
            SetupTestData();

        }

        [Fact]
        public void Delete() 
        {
            SetupTestData();
            
        }

        [Fact]
        public void Dispose()
        {
            SetupTestData();
        }

        // This method really should be in another class like a Factory Class for KanbanContext
        // We are leaving it here since we do not have a Factory Class
        private static void seed(KanbanContext context)
        {

            context.Database.ExecuteSqlRaw("DELETE dbo.Tasks");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
            context.Database.ExecuteSqlRaw("DELETE dbo.Users");
            //context.Database.ExecuteSqlRaw("DELETE dbo.TagTask");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)");
            //context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.TagTask', RESEED, 0)");

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
    }
}
