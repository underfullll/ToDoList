using BD.Properties;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using ToDoListLibrary;

namespace BD
{
    public class TaskRepository
    {

        private static string connectionString = Resources.DB_ConnectionString;

        public static bool IsDatabaseExist()
        {
            return File.Exists(connectionString);
        }

        public static void CreateDataBase()
        {
            SQLiteConnection.CreateFile(connectionString);
        }

        static List<string> migrationList = new List<string>()
        {

    "CREATE TABLE \"ToDoList\" (" +
    "\"ID\"    INTEGER NOT NULL," +
    "\"Title\" TEXT," +
    "\"Description\"   TEXT," +
    "\"IsCompleted\"   INTEGER," +
    "\"UserID\" INTEGER," +
    "FOREIGN KEY(\"UserID\") REFERENCES \"Users\"(\"ID\"))"
        };

        public static void MigrateDataBase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var migration in migrationList)
                {
                    using (var command = new SQLiteCommand(migration, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

            }
        }

        

        public void Create(ToDoListLibrary.Task task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO ToDoList (ID, Title, Description, IsCompleted) VALUES (@Id, @Title, @Description, @IsCompleted)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ToDoListLibrary.Task> ReadAllTasks()
        {
            List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();

            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ToDoList";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ToDoListLibrary.Task task = new ToDoListLibrary.Task
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                IsCompleted = reader.GetBoolean(3)
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public void Update(ToDoListLibrary.Task task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE ToDoList SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int taskId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM ToDoList WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
       
    }
}