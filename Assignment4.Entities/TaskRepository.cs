using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using static Assignment4.Core.Response;
using static Assignment4.Core.State;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository, IDisposable
    {
        private readonly IKanbanContext _context;

        public TaskRepository(IKanbanContext context)
        {
            _context = context;
        }
        
        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            if (_context.Users.Find(task.AssignedToId) == null)
            {
                return (Response.BadRequest, 0);
            }

            var entity = new Task
            {
                Title = task.Title,
                Description = task.Description,
                AssignedTo = _context.Users.Find(task.AssignedToId),
                Tags = GetTags(task.Tags),
                State = State.New,
                Created = DateTime.UtcNow,
                StateUpdated = DateTime.UtcNow
            };

            _context.Tasks.Add(entity);
            _context.SaveChanges();
            
            return (Response.Created, entity.Id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll() =>
            _context.Tasks
                    .Select(t => 
                    new TaskDTO(t.Id,
                                t.Title,
                                t.AssignedTo.Name,
                                GetTagNames(t.Tags).ToList(),
                                t.State))
                    .ToList().AsReadOnly();
                    
        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            return ReadAllByState(State.Removed);
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            var entities = from t in _context.Tasks
                                where t.Tags.Contains(GetTag(tag))
                                select new TaskDTO(
                                    t.Id,
                                    t.Title,
                                    t.AssignedTo.Name,
                                    GetTagNames(t.Tags).ToList(),
                                    t.State
                                );
                                
            return entities.ToList().AsReadOnly();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            var entities = from t in _context.Tasks
                                where t.AssignedTo.Id == userId
                                select new TaskDTO(
                                    t.Id,
                                    t.Title,
                                    t.AssignedTo.Name,
                                    GetTagNames(t.Tags).ToList(),
                                    t.State
                                );
            return entities.ToList().AsReadOnly();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            var entities = from t in _context.Tasks
                                where t.State == state
                                select new TaskDTO(
                                    t.Id,
                                    t.Title,
                                    t.AssignedTo.Name,
                                    GetTagNames(t.Tags).ToList(),
                                    t.State
                                );
            return entities.ToList().AsReadOnly();

        }
        public TaskDetailsDTO Read(int taskId)
        {
            var entities = from t in _context.Tasks
                                where t.Id == taskId
                                select new TaskDetailsDTO(
                                    t.Id,
                                    t.Title,
                                    t.Description,
                                    t.Created,
                                    t.AssignedTo.Name,
                                    GetTagNames(t.Tags).ToList(),
                                    t.State,
                                    t.StateUpdated
                                );
            return entities.FirstOrDefault();
        }
        
        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Tasks.Find(task.Id);

            if (entity == null)
            {
                return NotFound;
            }

            if (_context.Users.Find(task.AssignedToId) == null)
            {
                return Response.BadRequest;
            }
            
            if (task.State != entity.State)
            {
                entity.StateUpdated = DateTime.UtcNow;
            }

            entity.AssignedTo = _context.Users.Find(task.AssignedToId);
            entity.State = task.State;
            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.Tags = GetTags(task.Tags);

            _context.Tasks.Update(entity);
            _context.SaveChanges();

            return Response.Updated;
        }
        
        public Response Delete(int taskId)
        {
            var entity = _context.Tasks.Find(taskId);

            if (entity == null)
            {
                return NotFound;
            } 
            else if (entity.State == Active)
            {
                var toBeUpdated = new TaskUpdateDTO
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description,
                    AssignedToId = entity.AssignedTo.Id,
                    Tags = GetTagNames(entity.Tags).ToList(),
                    State = State.Removed
                };
                return Update(toBeUpdated);
            }
            else if (entity.State == Removed || entity.State == Closed || entity.State == Resolved)
            {
                return Conflict;
            } 
            else 
            {
                _context.Tasks.Remove(entity);
                _context.SaveChanges();

                return Response.Deleted;
            } 
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private static Tag GetTag(string tagName)
        {
            var entities = 
            return entities.FirstOrDefault();
        }

        private static ICollection<Tag> GetTags(ICollection<string> tags)
        {
            var ReturnTags = new List<Tag>();

            for (int i = 0; i < tags.Count; i++)
            {
                var entity = from t in _context.Tags
                            where t.Name == tags.ElementAt(i)
                            select new Tag {
                                Id = t.Id,
                                Name = t.Name,
                                Tasks = t.Tasks
                                };
                Tag tag = entity.FirstOrDefault();
                ReturnTags.Add(tag);
            }
            
            return ReturnTags;
        }

        private static IEnumerable<string> GetTagNames(ICollection<Tag> tags)
        {
            foreach (var tag in tags)
            {
                yield return tag.Name;
            }
        }
    }
}
