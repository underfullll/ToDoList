using BD.Properties;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ToDoListLibrary;

namespace BD.Repository
{
    public class UserRepository
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
           "CREATE TABLE \"Users\" (" +
    "\"UserId\"    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
    "\"Username\" TEXT NOT NULL, " +
    "\"Password\" TEXT NOT NULL, PRIMARY KEY(\"UserId\" AUTOINCREMENT)" +
    ")"
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
        public void UpdateUser(ToDoListLibrary.User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Users SET Username = @Username, Password = @Password WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<ToDoListLibrary.User> ReadAllUsers()
        {
            List<ToDoListLibrary.User> users = new List<ToDoListLibrary.User>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ToDoListLibrary.User user = new ToDoListLibrary.User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2)
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public void CreateUser(ToDoListLibrary.User user)
        {
            if (UserExists(user.Username))
            {
                Console.WriteLine("User with the same username already exists.");
                return;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Users WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetMaxUserId()
        {
            List<User> users = GetAllUsersFromDatabase();
            int maxId = users.Count > 0 ? users.Max(u => u.UserId) : 0;
            return maxId;
        }

        private static List<User> GetAllUsersFromDatabase()
        {
            List<User> users = new List<User>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2)
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public bool UserExists(string username)
        {
            bool userExists = false;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    userExists = count > 0;
                }
            }

            return userExists;
        }
    }
}
