using System;

using Xunit;

using Assignment4.Core;

using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly TaskRepository _taskRepository;
        private readonly KanbanContext _context;
        
        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            //connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();

            //var kanbanContextFactory = new KanbanContextFactory();

            //_context = kanbanContextFactory.CreateDbContext(new string[] {});

            _taskRepository = new TaskRepository(_context);
            
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
            _context = context;
        }

        [Fact]
        public void All_returns_four_Tasks()
        {
            /*//var expected = new List<Task> {new Task()}
            Assert.Collection(_taskRepository.All(), 
                c => Assert.Equal(new TaskDTO(1, "Draw the chair", 
                                "A chair has to be drawn. Use Millimeter paper", 1,
                                new List<string> { "Drawing", "Chair" },
                                State.New), c),
                c => Assert.Equal(new TaskDTO(2, "Draw a window", 
                                "Draw a window behind the chair", 2, 
                                new List<string> {"Drawing"} ,
                                State.New), c),
                c => Assert.Equal(new TaskDTO(3, "Build the chair",  
                                "UserID 1 needs to build a chair", 1, 
                                new List<string>{"Building", "Chair"}, 
                                State.Active), c),
                c => Assert.Equal(new TaskDTO(4, "Order coffeeMachine", 
                                "We need a new coffe machine, the budget is 200,-", 2, 
                                new List<string>{"Kitchen Appliance"}, 
                                State.Active), c)
            );
            
            
            
            /*
            Assert.Collection(characters,
                c => Assert.Equal(new CharacterDTO(1, "Clark", "Kent", "Superman"), c),
                c => Assert.Equal(new CharacterDTO(2, "Bruce", "Wayne", "Batman"), c),
                c => Assert.Equal(new CharacterDTO(3, "Diana", "Prince", "Wonder Woman"), c),
                c => Assert.Equal(new CharacterDTO(4, "Selina", "Kyle", "Catwoman"), c)
            );
            */
            throw new NotImplementedException();
        }

        [Fact]
        public void Create_And_Get_Task()
        {
            /*var expected = new TaskDetailsDTO { Id = 5,
                                                Title = "Draw the Table",
                                                AssignedToId = 1,
                                                AssignedToName = "Adrian",
                                                AssignedToEmail = "email1",
                                                Description = "A table has to be drawn. Use Millimeter paper",
                                                Tags = new List<string> { "Drawing" },
                                                State = State.Active };
                                                
            var NewTask = new TaskDTO { Id = 5,
                                        Title = "Draw the Table",
                                        AssignedToId = 1,
                                        Description = "A table has to be drawn. Use Millimeter paper",
                                        State = State.Active,
                                        Tags = new List<string> { "Drawing" } };

            //_taskRepository.Create(NewTask);

            var actual = _taskRepository.FindById(1);

            Assert.Equal(expected, actual);*/
            throw new NotImplementedException();
        } 

        [Theory]
        [InlineData(1, "Drawing")]
        [InlineData(2, "Chair")]
        [InlineData(4, "Building")]
        public void Find_By_Id(int id, string expectedTitle) 
        {
            //Assert.Equal(expectedTitle, _taskRepository.FindById(id).Title);
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Delete(int id) 
        {
            //_taskRepository.Delete(id);
            //Assert.Null(_taskRepository.FindById(id));
            throw new NotImplementedException();
        }

        [Fact]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
