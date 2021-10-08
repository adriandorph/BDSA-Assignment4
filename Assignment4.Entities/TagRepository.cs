using Assignment4.Core;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Xml.Schema;


namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly IKanbanContext _context;

        public TagRepository(IKanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var entity = new Tag
            {
                Id = tag.Id,
                Name = tag.Name
            };

            _context.Tags.Add(entity);

            _context.SaveChanges();

            return (Response.Created, entity.Id);
        }
        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            throw new NotImplementedException();
        }
        public TagDTO Read(int tagId)
        {
            throw new NotImplementedException();
        }
        public Response Update(TagUpdateDTO tag)
        {
            throw new NotImplementedException();
        }
        public Response Delete(int tagId, bool force = false)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}