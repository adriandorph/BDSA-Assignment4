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
        private readonly ITaskRepository _taskRepository;
        private readonly KanbanContext _context;
        
        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();

            //var kanbanContextFactory = new KanbanContextFactory();

            //_context = kanbanContextFactory.CreateDbContext(new string[] {});
            
            /*context.Database.ExecuteSqlRaw("DELETE dbo.TagTask");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tasks");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
            context.Database.ExecuteSqlRaw("DELETE dbo.Users");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)");*/

            var tag1 = new Tag { Name = "Drawing" };
            var tag2 = new Tag { Name = "Chair" };
            var tag3 = new Tag { Name = "Kitchen Appliance" };
            var tag4 = new Tag { Name = "Building" };

            var user1 = new User { Name = "Adrian", Email = "email1" };
            var user2 = new User { Name = "Mai", Email = "email2" };
            var user3 = new User { Name = "Sofia", Email = "email@3.dk" };

            var Task1 = new Task { 
                                    Title = "Draw the chair", 
                                    AssignedTo = user1, 
                                    Description = "A chair has to be drawn. Use millimeter paper", 
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
                                    Title = "Order coffeemachine", 
                                    AssignedTo = user2, 
                                    Description = "We need a new coffeemachine, the budget is 200,-", 
                                    State = State.Closed, 
                                    Tags = new List<Tag> {tag3} };
            var Task5 = new Task {
                                    Title = "Destroy coffeemachine", 
                                    AssignedTo = user3, 
                                    Description = "This coffeemachine is bad. We should destroy so we can get a new one", 
                                    State = State.Removed, 
                                    Tags = new List<Tag> {tag3} };

            context.Tasks.Add(Task1);
            context.Tasks.Add(Task2);
            context.Tasks.Add(Task3);
            context.Tasks.Add(Task4);
            context.Tasks.Add(Task5);
            context.Tags.Add(tag1);
            context.Tags.Add(tag2);
            context.Tags.Add(tag3);
            context.Tags.Add(tag4);
            context.Users.Add(user1);
            context.Users.Add(user2);
            context.Users.Add(user3);

            context.SaveChanges();
            _context = context;
            _taskRepository = new TaskRepository(_context);
        }

        [Fact]
        public void Create_creates_new_task()
        {
            var taskCreate = new TaskCreateDTO
            {
                Title = "Color Chair", 
                AssignedToId = 2, 
                Description = "The chair is looking so boring, give it some color", 
                Tags = new List<string> {"drawing"}
            };
            
            // Expected
            var ExpectedResponse = (Response.Created, 6);

            // Actual
            var ActualResponse = _taskRepository.Create(taskCreate);
            
            // Test
            Assert.Equal(ExpectedResponse, ActualResponse);
        }

        // Shows exactly the same as Expected and Actual, I have no idea what to do here - Mai
        [Fact]
        public void ReadAll_returns_five_Tasks()
        {
            
            Assert.Collection(_taskRepository.ReadAll(), 
                t => Assert.Equal(new TaskDTO(1, "Draw the chair", 
                                "Adrian", 
                                new List<string> { "Drawing", "Chair" },
                                State.New), t),
                t => Assert.Equal(new TaskDTO(2, "Draw a window", 
                                "Mai",
                                new List<string> {"Drawing"} ,
                                State.New), t),
                t => Assert.Equal(new TaskDTO(3, "Build the chair",  
                                "Adrian", 
                                new List<string>{"Building", "Chair"}, 
                                State.Active), t),
                t => Assert.Equal(new TaskDTO(4, "Order coffeemachine", 
                                "Mai", 
                                new List<string>{"Kitchen Appliance"}, 
                                State.Active), t),
                t => Assert.Equal(new TaskDTO(5, "Destroy coffeemachine", 
                                "Sofia",
                                new List<string> {"Kitchen Appliance"},
                                State.Removed), t)
            );
        }

        // Shows exactly the same as Expected and Actual, I have no idea what to do here - Mai
        [Fact]
        public void ReadAllRemoved_returns_task5()
        {
            var removed = _taskRepository.ReadAllRemoved();

            Assert.Collection(removed,
                t => Assert.Equal(new TaskDTO(5, "Destroy coffeemachine", "Sofia", 
                                    new List<string> {"Kitchen Appliance"}, State.Removed), t)
                                    
            );
        }

        // Shows exactly the same as Expected and Actual, I have no idea what to do here - Mai
        [Fact]
        public void ReadAllByTag_given_drawing_returns_2_tasks()
        {   
            var tagsByDrawing = _taskRepository.ReadAllByTag("Drawing");

            Assert.Collection(tagsByDrawing,
                t => Assert.Equal(new TaskDTO(1, "Draw the chair", "Adrian",
                                    new List<string> {"Drawing", "Chair"}, State.New), t),
                t => Assert.Equal(new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New), t)
            );
        }

        // Draw window task's ID is off by one
        [Fact]
        public void ReadAllByUser_given_2_returns_Task_4_and_2()
        {
            var tasksBy2 = _taskRepository.ReadAllByUser(2);

            Assert.Collection(tasksBy2,
            t => Assert.Equal(new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New), t),
                t => Assert.Equal(new TaskDTO(4, "Order coffeemachine", "Mai", 
                                    new List<string> {"Kitchen Appliance"}, State.Closed), t)
            );
        }

        // Shows exactly the same as Expected and Actual, I have no idea what to do here - Mai
        [Fact]
        public void ReadAllByState_given_new_returns_2_tasks()
        {
            var newTasks = _taskRepository.ReadAllByState(State.New);

            Assert.Collection(newTasks,
                t => Assert.Equal(new TaskDTO(1, "Draw the chair", "Adrian", 
                                    new List<string> {}, State.New), t),
                t => Assert.Equal(new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New), t)
            );
        }

        // TODO:
        // Issues with time not being set correctly
        [Fact]
        public void Read_given_1_returns_task_1()
        {
            var expectedTask = new TaskDetailsDTO(1, "Draw the chair", 
                    "A chair has to be drawn. Use millimeter paper", DateTime.UtcNow, "Adrian", 
                    new List<string> {"Drawing", "Chair"}, State.New, DateTime.UtcNow);

            var actualTask = _taskRepository.Read(1);

            Assert.Equal(expectedTask, actualTask); 
            Assert.Equal(expectedTask.Created, actualTask.Created, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(expectedTask.StateUpdated, actualTask.StateUpdated, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Update()
        {
            //Act
            _taskRepository.Update(new TaskUpdateDTO{
                Id = 2,
                Title = "The Task", 
                AssignedToId = 1, 
                Description = "UserID 1 needs to build a chair", 
                State = State.Active,
                Tags = new List<string> {"Building", "Chair"}
            });

            //Assert
            Assert.Equal("The Task", _taskRepository.Read(2).Title);
        }

        
        // Since the first one is getting removed (Task ID = 1), 
        // then you have to move then Id one down to get the expected Tasks.
        [Theory]
        [InlineData(1, Response.Deleted)]
        [InlineData(2, Response.Updated)]
        [InlineData(4, Response.Conflict)]
        public void Delete_Task_with_status_returns_correct_response(int taskId, Response response)
        {
            Assert.Equal(response, _taskRepository.Delete(taskId));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
