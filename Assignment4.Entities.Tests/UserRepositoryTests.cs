using System;
using Xunit;
using System.Collections.Generic;

using Assignment4.Core;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using static Assignment4.Core.Response;

namespace Assignment4.Entities.Tests
{
    public class UserRepositoryTests
    {
        private readonly IKanbanContext _context;
        private readonly UserRepository _repo;
        public UserRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();

            _context = context;
            _repo = new UserRepository(_context);
        }

        [Fact]
        public void Create_User()
        {
            // Expected
            var ExpectedResponse = (Created, 1);

            // Actual
            var UserToCreate = new UserCreateDTO { Name = "Mai", Email = "mai@mail.com" };
            var ActualResponse = _repo.Create(UserToCreate);

            // Test
            Assert.Equal(ExpectedResponse, ActualResponse);
        }

        [Fact]
        public void Find_User_By_Id()
        {
            // Arrange 
            _context.Users.Add(new User { Name = "Mai", Email = "mai@mail.com"} );
            _context.SaveChanges();
            
            // Expected
            var ExpectedUser = new UserDTO(1, "Mai", "mai@mail.com");

            // Actual
            var ActualUser = _repo.Read(1);

            // Test
            Assert.Equal(ExpectedUser, ActualUser);
        }

        [Fact]
        public void Delete_By_Id()
        {
            // Arrange
            _context.Users.Add(new User { Name = "Mai", Email = "mai@mail.com"} );
            _context.SaveChanges();

            // Test
            Assert.Equal(Deleted, _repo.Delete(1));
        }

        [Fact]
        public void Delete_By_Id_Not_Found()
        {
            // Test
            Assert.Equal(NotFound, _repo.Delete(1));
        }

        [Fact]
        public void ReadAll_success()
        {
            // Arrange
            _context.Users.Add(new User { Name = "Mai", Email = "mai@mail.com"} );
            _context.Users.Add(new User { Name = "Sofia", Email = "sofia@mail.com"} );
            _context.SaveChanges();

            // Expected
            var ExpectedUsers = new List<UserDTO> { 
                                    new UserDTO(1, "Mai", "mai@mail.com"),
                                    new UserDTO(2, "Sofia", "sofia@mail.com")
                                    };

            // Actual
            var ActualUsers = _repo.ReadAll();

            // Test
            Assert.Equal(ExpectedUsers, ActualUsers);
        }
    }
}