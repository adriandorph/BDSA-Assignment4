using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Xml.Schema;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private static SqlConnection _connection;

        public TaskRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public IReadOnlyCollection<TaskDTO> All()
        {
            List<TaskDTO> AllTasks = new List<TaskDTO>();

            var cmdText = @"SELECT *
                            FROM Tasks
                            JOIN TagTask TT
                            ON T.Id = TT.TasksId
                            JOIN Tags Tg
                            ON TT.TagsId = Tg.Id";

            using var command = new SqlCommand(cmdText, _connection);

            OpenConnection();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AllTasks.Add(new TaskDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    AssignedToId = reader.GetInt32("AssignedToId"),
                    Tags = reader.GetFieldValue<IReadOnlyCollection<string>>("Tags"),
                    State = reader.GetFieldValue<State>("State")
                });
            }

            CloseConnection();
            return AllTasks;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="task"></param>
        /// <returns>The id of the newly created task</returns>
        public int Create(TaskDTO task)
        {
            var cmdText = @"INSERT INTO Tasks (Title, AssignedToId, Description, State)
                            VALUES (@Title, @AssignedToId, @Description, @State);
                            SELECT SCOPE_IDENTITY()";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);
            //command.Parameters.AddWithValue("@Tags", task.Tags);
            command.Parameters.AddWithValue("@State", task.State);

            OpenConnection();

            var id = command.ExecuteScalar();

            CloseConnection();

            return Convert.ToInt32(id);
        }

        public void Delete(int taskId)
        {
            var cmdText = @"DELETE FROM Tasks WHERE Id=@Id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", taskId);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

        public TaskDetailsDTO FindById(int TaskId)
        {
            var cmdText = @"SELECT *
                            FROM Tasks T
                            JOIN Users U
                            ON T.AssignedToId = U.Id
                            JOIN TagTask TT
                            ON T.Id = TT.TasksId
                            JOIN Tags Tg
                            ON TT.TagsId = Tg.Id                            
                            WHERE T.Id=@Id";
            
            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", TaskId);

            OpenConnection();

            using var reader = command.ExecuteReader();

            /*while(reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
                Console.WriteLine(reader.GetValue(1));
                Console.WriteLine(reader.GetValue(2));
                Console.WriteLine(reader.GetValue(3));
                Console.WriteLine(reader.GetValue(4));
            }*/
            
            
            for (int col = 0; col < reader.FieldCount; col++)
            {
                Console.Write(reader.GetName(col).ToString());         // Gets the column name
                Console.Write(reader.GetFieldType(col).ToString());    // Gets the column type
            }
            
            var taskDetailsDTO = reader.Read()
                ? new TaskDetailsDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"), 
                    AssignedToId = reader.GetInt32("AssignedToId"),
                    AssignedToName = reader.GetString("Name"),
                    AssignedToEmail = reader.GetString("Email"),
                    Tags = reader.GetFieldValue<IEnumerable<string>>("Name"),
                    State = reader.GetFieldValue<State>("State")
                }
                : null;
                

            CloseConnection();

            return taskDetailsDTO;
        }

        private IEnumerable<string> yieldTags(SqlDataReader reader)
        {
            while(reader.Read())
            {
                yield return reader.GetFieldValue<string>("Name");
            }
        }

        public void Update(TaskDTO task)
        {
            var cmdText = @"UPDATE Tasks 
                            Title = @Title, AssignedToId = @AssignedToId, Description = @Description, 
                            State = @State, Tags = @Tags
                            WHERE Id = @Id;";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@State", task.State);
            command.Parameters.AddWithValue("@Tags", task.Tags);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
