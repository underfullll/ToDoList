using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD
{
    class ClassInterface : Interface
    {
        private string ConnectionString = "Data Source = C:\\Users\\anton\\Desktop\\projects\\ToDoList\\Todolist_in_WPF\\BD\\BD\\DB_ToDoList.sqbpro";
        public ClassInterface()
        {
            string relativePath = @"BD\DB_ToDoList.db";
            string parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
            string tmp = parentDir.Remove(parentDir.Length - 16, 16);
            string absolutePath = Path.Combine(tmp, relativePath);
            ConnectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath, true);
        }

        public void create(string title, string description)
        {
            try
            {
                string sql = "INSERT INTO ToDoList (Title, Description, IsCompleted) VALUES (@Title, @Description, 0)"; // Параметризованный запрос
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при создании записи. Исключение: {ex.Message}");
            }
        }
        public void read()
        {
            try
            {
                string sql = "SELECT * FROM ToDoList";
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int id = rdr.GetInt32(0);
                                string title = rdr.GetString(1);
                                string description = rdr.GetString(2);
                                int isCompleted = rdr.GetInt32(3);
                                Console.WriteLine("{0} \t{1} \t{2} \t{3}", id, title, description, isCompleted);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
        }

        public void Update(int id, string title)
        {
            try
            {
                string sql = "UPDATE ToDoList SET Title = @Title WHERE ID = @ID";
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {

                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при обновлении записи. Исключение: {ex.Message}");
            }
        }

        public void delete(int id)
        {
            try
            {
                string sql = "DELETE FROM ToDoList WHERE ID = @ID";
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при удалении записи. Исключение: {ex.Message}");
            }
        }
    }
}
