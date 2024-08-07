﻿using BD.Properties;
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
    "\"UserId\" INTEGER," +
    "PRIMARY KEY(\"ID\" AUTOINCREMENT)"
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

                string query = "INSERT INTO ToDoList (Title, Description, IsCompleted, UserId) VALUES (@Title, @Description, @IsCompleted, @UserId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@UserId", task.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ToDoListLibrary.Task> ReadAllTasks(int userId)
        {
            List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM ToDoList WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ToDoListLibrary.Task task = new ToDoListLibrary.Task
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                IsCompleted = reader.GetBoolean(3),
                                UserId = reader.GetInt32(0)
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

                string query = "UPDATE ToDoList SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE ID = @Id";
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

                string query = "DELETE FROM ToDoList WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}