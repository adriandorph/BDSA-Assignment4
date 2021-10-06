using System;

using Xunit;

using Assignment4.Core;

using System.Collections.Generic;

using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Data.SqlClient;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        TaskRepository taskRepository;
        
        private void SetupTestData()
        {
            var kanbanContextFactory = new KanbanContextFactory();

            var context = kanbanContextFactory.CreateDbContext(new string[] {});

            taskRepository = new TaskRepository(new SqlConnection(kanbanContextFactory.GetConnectionString()));
        }

        [Fact]
        public void Create_And_Get_Task()
        {
            SetupTestData();

            var expected = new TaskDetailsDTO { Id = 5,
                                                Title = "Draw the Table",
                                                AssignedToId = 1,
                                                AssignedToName = "Adrian",
                                                AssignedToEmail = "email1",
                                                Description = "A table has to be drawn. Use Millimeter paper",
                                                Tags = new List<string> { "Drawing" },
                                                State = State.Active };
                                                
            var NewTask = new TaskDTO { Title = "Draw the Table",
                                        AssignedToId = 1,
                                        Description = "A table has to be drawn. Use Millimeter paper",
                                        State = State.Active,
                                        Tags = new List<string> { "Drawing" } };

            taskRepository.Create(NewTask);

            var actual = taskRepository.FindById(1);

            Assert.Equal(expected, actual);
        } 

        [Theory]
        [InlineData(1, "Drawing")]
        [InlineData(2, "Chair")]
        [InlineData(4, "Building")]
        public void Find_By_Id(int id, string expectedTitle) 
        {
            SetupTestData();
            Assert.Equal(expectedTitle, taskRepository.FindById(id).Title);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Delete(int id) 
        {
            //Arrange
            SetupTestData();
            //Act
            taskRepository.Delete(id);
            //Assert
            Assert.Null(taskRepository.FindById(id));
        }

        [Fact]
        public void Dispose()
        {
            SetupTestData();
        }
    }
}
