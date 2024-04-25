using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryToDoList
{
    //internal class TEstSQLITE
    //{
    //    static SQLiteConnection connection;
    //    static SQLiteCommand command;

    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            connection = new SQLiteConnection("Data Source=D:\\DBS\\students.db;Version=3; FailIfMissing=False");
    //            connection.Open();
    //            Console.WriteLine("Connected!");
    //            command = new SQLiteCommand(connection)
    //            {
    //                CommandText = "SELECT * FROM \"студенты\";"
    //            };
    //            Console.WriteLine("Результат запроса:");
    //            DataTable data = new DataTable();
    //            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
    //            adapter.Fill(data);
    //            Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
    //            foreach (DataRow row in data.Rows)
    //            {
    //                Console.WriteLine($"id = {row.Field<Int64>("id")} name = {row.Field<string>("name")} group = {row.Field<Int64>("group")}");
    //            }

    //        }
    //        catch (SQLiteException ex)
    //        {
    //            Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
    //        }
    //        Console.Read();
    //    }
    //}
}
