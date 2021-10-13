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

            var Task1 = new Task {  Id = 1,
                                    Title = "Draw the chair", 
                                    AssignedTo = user1, 
                                    Description = "A chair has to be drawn. Use millimeter paper", 
                                    State = State.New, 
                                    Tags = new List<Tag> {tag1, tag2},
                                    Created = DateTime.UtcNow,
                                    StateUpdated = DateTime.UtcNow };
            var Task2 = new Task {  Id = 2,
                                    Title = "Draw a window", 
                                    AssignedTo = user2, 
                                    Description = "Draw a window behind the chair", 
                                    State = State.New, 
                                    Tags = new List<Tag>{tag1},
                                    Created = DateTime.UtcNow,
                                    StateUpdated = DateTime.UtcNow };
            var Task3 = new Task {  Id = 3,
                                    Title = "Build the chair", 
                                    AssignedTo = user1, 
                                    Description = "UserID 1 needs to build a chair", 
                                    State = State.Active,
                                    Tags = new List<Tag> {tag4, tag2},
                                    Created = DateTime.UtcNow,
                                    StateUpdated = DateTime.UtcNow };
            var Task4 = new Task {  Id = 4,
                                    Title = "Order coffeemachine", 
                                    AssignedTo = user2, 
                                    Description = "We need a new coffeemachine, the budget is 200,-", 
                                    State = State.Closed, 
                                    Tags = new List<Tag> {tag3},
                                    Created = DateTime.UtcNow,
                                    StateUpdated = DateTime.UtcNow };
            var Task5 = new Task {  Id = 5,
                                    Title = "Destroy coffeemachine", 
                                    AssignedTo = user3, 
                                    Description = "This coffeemachine is bad. We should destroy so we can get a new one", 
                                    State = State.Removed, 
                                    Tags = new List<Tag> {tag3},
                                    Created = DateTime.UtcNow,
                                    StateUpdated = DateTime.UtcNow };

            context.Tags.Add(tag1);
            context.Tags.Add(tag2);
            context.Tags.Add(tag3);
            context.Tags.Add(tag4);
            context.Users.Add(user1);
            context.Users.Add(user2);
            context.Users.Add(user3);
            context.Tasks.Add(Task1);
            context.Tasks.Add(Task2);
            context.Tasks.Add(Task3);
            context.Tasks.Add(Task4);
            context.Tasks.Add(Task5);

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

        [Fact]
        public void ReadAll_returns_five_Tasks()
        {
            // Expected
            var ExpectedTaskDTO1 = new TaskDTO(1, "Draw the chair", 
                                "Adrian", 
                                new List<string> { "Drawing", "Chair" },
                                State.New);
            var ExpectedTaskDTO2 = new TaskDTO(2, "Draw a window", 
                                "Mai",
                                new List<string> {"Drawing"} ,
                                State.New);
            var ExpectedTaskDTO3 = new TaskDTO(3, "Build the chair",  
                                "Adrian", 
                                new List<string>{"Chair", "Building"}, 
                                State.Active);
            var ExpectedTaskDTO4 = new TaskDTO(4, "Order coffeemachine", 
                                "Mai", 
                                new List<string>{"Kitchen Appliance"}, 
                                State.Closed);
            var ExpectedTaskDTO5 = new TaskDTO(5, "Destroy coffeemachine", 
                                "Sofia",
                                new List<string> {"Kitchen Appliance"},
                                State.Removed);

            // Actual
            var ExpectedTasks = _taskRepository.ReadAll();

            Assert.Collection(ExpectedTasks, 
                t => {
                    Assert.Equal(ExpectedTaskDTO1.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO1.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO1.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO1.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO1.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO2.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO2.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO2.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO2.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO2.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO3.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO3.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO3.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO3.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO3.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO4.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO4.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO4.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO4.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO4.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO5.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO5.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO5.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO5.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO5.State, t.State);
                }
            );
        }

        [Fact]
        public void ReadAllRemoved_returns_task5()
        {
            // Expected
            var ExpectedTaskDTO = new TaskDTO(5, "Destroy coffeemachine", "Sofia",
                                    new List<string> {"Kitchen Appliance"}, State.Removed);

            // Actual
            var removed = _taskRepository.ReadAllRemoved();

            Assert.Collection(removed,
                t => {
                    Assert.Equal(ExpectedTaskDTO.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO.State, t.State);
                }
            );
        }

        [Fact]
        public void ReadAllByTag_given_drawing_returns_2_tasks()
        {   
            // Expected
            var ExpectedTaskDTO1 = new TaskDTO(1, "Draw the chair", "Adrian",
                                    new List<string> {"Drawing", "Chair"}, State.New);
            var ExpectedTaskDTO2 = new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New);

            // Actual
            var tagsByDrawing = _taskRepository.ReadAllByTag("Drawing");

            // Test
            Assert.Collection(tagsByDrawing,
                t => {
                    Assert.Equal(ExpectedTaskDTO1.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO1.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO1.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO1.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO1.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO2.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO2.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO2.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO2.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO2.State, t.State);
                }
            );
        }

        [Fact]
        public void ReadAllByUser_given_2_returns_Task_4_and_2()
        {
            // Expected
            var ExpectedTaskDTO1 = new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New);
            var ExpectedTaskDTO2 = new TaskDTO(4, "Order coffeemachine", "Mai", 
                                    new List<string> {"Kitchen Appliance"}, State.Closed);

            // Actual
            var tasksBy2 = _taskRepository.ReadAllByUser(2);

            // Test
            Assert.Collection(tasksBy2,
                t => {
                    Assert.Equal(ExpectedTaskDTO1.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO1.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO1.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO1.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO1.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO2.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO2.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO2.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO2.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO2.State, t.State);
                }
            );
        }

        [Fact]
        public void ReadAllByState_given_new_returns_2_tasks()
        {
            // Expected
            var ExpectedTaskDTO1 = new TaskDTO(1, "Draw the chair", "Adrian", 
                                    new List<string> {"Drawing", "Chair"}, State.New);
            var ExpectedTaskDTO2 = new TaskDTO(2, "Draw a window", "Mai", 
                                    new List<string> {"Drawing"}, State.New);

            // Actual
            var newTasks = _taskRepository.ReadAllByState(State.New);

            // Test
            Assert.Collection(newTasks,
                t => {
                    Assert.Equal(ExpectedTaskDTO1.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO1.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO1.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO1.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO1.State, t.State);
                },
                t => {
                    Assert.Equal(ExpectedTaskDTO2.Id, t.Id);
                    Assert.Equal(ExpectedTaskDTO2.Title, t.Title);
                    Assert.Equal(ExpectedTaskDTO2.AssignedToName, t.AssignedToName);
                    Assert.Equal(ExpectedTaskDTO2.Tags, t.Tags);
                    Assert.Equal(ExpectedTaskDTO2.State, t.State);
                }
            );
        }

        [Fact]
        public void Read_given_1_returns_task_1()
        {
            var expectedTask = new TaskDetailsDTO(1, "Draw the chair", 
                    "A chair has to be drawn. Use millimeter paper", DateTime.UtcNow, "Adrian", 
                    new List<string> {"Drawing", "Chair"}, State.New, DateTime.UtcNow);

            var actualTask = _taskRepository.Read(1);

            Assert.Equal(expectedTask.Id, actualTask.Id);
            Assert.Equal(expectedTask.Title, actualTask.Title);
            Assert.Equal(expectedTask.Description, actualTask.Description);
            Assert.Equal(expectedTask.AssignedToName, actualTask.AssignedToName);
            Assert.Equal(expectedTask.State, actualTask.State);
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
        [InlineData(3, Response.Updated)]
        [InlineData(5, Response.Conflict)]
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
