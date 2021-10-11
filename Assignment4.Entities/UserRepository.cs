using Assignment4.Core;
using static Assignment4.Core.Response;
using System.Data;
using System.Linq;
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
            if (ReadByEmail(user.Email) != null)
            {
                return (Conflict, 0);
            }

            var entity = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            _context.Users.Add(entity);
            _context.SaveChanges();

            return (Response.Created, entity.Id);
        }
        public IReadOnlyCollection<UserDTO> ReadAll() =>
            _context.Users
                    .Select(u => new UserDTO(u.Id, u.Name, u.Email))
                    .ToList().AsReadOnly();
        public UserDTO Read(int userId)
        {
            var entities = from u in _context.Users
                                where u.Id == userId
                                select new UserDTO(
                                    u.Id,
                                    u.Name,
                                    u.Email
                                );
            return entities.FirstOrDefault();
        }

        public UserDTO ReadByEmail(string email)
        {
            var entities = from u in _context.Users
                                where u.Email == email
                                select new UserDTO(
                                    u.Id,
                                    u.Name,
                                    u.Email
                                );
            return entities.FirstOrDefault();
        }

        public Response Update(UserUpdateDTO user)
        {
            var entity = _context.Users.Find(user.Id);

            if (user == null)
            {
                return NotFound;
            }

            if (ReadByEmail(user.Email) != null)
            {
                return Conflict;
            }


            entity.Name = user.Name;
            entity.Email = user.Email;

            return Updated;
        }
        public Response Delete(int userId, bool force = false)
        {
            var entity = _context.Users.Find(userId);

            if (entity == null)
            {
                return NotFound;
            }

            if (entity.Tasks != null && force)
            {
                return Conflict;
            }

            _context.Users.Remove(entity);
            _context.SaveChanges();

            return Deleted;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}