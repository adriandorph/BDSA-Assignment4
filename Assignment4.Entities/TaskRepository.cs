using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System;
using Assignment4.Entities;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IKanbanContext _context;

        public TaskRepository(IKanbanContext context)
        {
            _context = context;
        }
        
        public IReadOnlyCollection<TaskDTO> All()
        {
            

            return _context.Tasks.SqlQuery("SELECT *", new string[]);
            //throw new NotImplementedException();

            /*var characters = from c in _context.Characters
                             where c.Id == characterId
                             select new CharacterDetailsDTO(
                                 c.Id,
                                 c.GivenName,
                                 c.Surname,
                                 c.AlterEgo,
                                 c.City.Name,
                                 c.FirstAppearance,
                                 c.Occupation,
                                 c.Powers.Select(c => c.Name).ToHashSet()
                             );*/
            
        }

        public int Create(TaskDTO task)
        {
            throw new NotImplementedException();
        }

        public void Delete(int taskId)
        {
            var taskToRemove = _context.Tasks.Find(taskId);
            _context.Tasks.Remove(taskToRemove);
        }

        public TaskDetailsDTO FindById(int TaskId)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<string> yieldTags(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public void Update(TaskDTO task)
        {
           
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
