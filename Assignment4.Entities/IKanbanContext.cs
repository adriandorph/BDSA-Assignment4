using System;
using Microsoft.EntityFrameworkCore;
namespace Assignment4.Entities
{
    public interface IKanbanContext : IDisposable
    {
        DbSet<User> Users { get; }
        DbSet<Task> Tasks { get; }
        DbSet<Tag> Tags { get; }
        int SaveChanges();
    }
}