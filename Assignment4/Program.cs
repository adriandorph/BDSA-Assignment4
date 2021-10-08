using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;
using Assignment4.Core;
using System.Collections.Generic;

using System.Data.SqlClient;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            //var kanbanContextFactory = new KanbanContextFactory();

            //var context = kanbanContextFactory.CreateDbContext(new string[] {});

            //var taskRepository = new TaskRepository(new SqlConnection(kanbanContextFactory.GetConnectionString()));

            /*var NewTask = new TaskDTO { Title = "Draw the Table",
                                        AssignedToId = 1,
                                        Description = "A table has to be drawn. Use Millimeter paper",
                                        State = State.Active };*/

            //taskRepository.Create(NewTask);

            //taskRepository.FindById(1);
        }

        /*static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>();

            return builder.Build();
        }*/
    }
}
