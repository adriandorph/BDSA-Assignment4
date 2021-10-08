using Assignment4.Core;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Assignment4.Entities
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly IKanbanContext _context;

        public UserRepository(IKanbanContext context) 
        {
            _context = context;
        }

        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            var entity = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            _context.Users.Add(entity);

            _context.SaveChanges();

            return (Response.Created, entity.Id);
        }
        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            throw new NotImplementedException();
        }
        public UserDTO Read(int userId)
        {
            throw new NotImplementedException();
        }
        public Response Update(UserUpdateDTO user)
        {
            throw new NotImplementedException();
        }
        public Response Delete(int userId, bool force = false)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}