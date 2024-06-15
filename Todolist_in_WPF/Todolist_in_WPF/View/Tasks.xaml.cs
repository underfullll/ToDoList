using BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Markup;
using Todolist_in_WPF.Utilities;
using Todolist_in_WPF.View;
using Todolist_in_WPF.ViewModel;

namespace Todolist_in_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        public Tasks()
        {
            InitializeComponent();
            EnsureDataBase();
            UpdateTasksFromDatabase();
        }
        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();
        private ToDoListLibrary.Task selectedTaskForEdit;
        private int nextId = 1;

        private void EnsureDataBase()
        {
            if (!TaskRepository.IsDatabaseExist())
            {
                TaskRepository.CreateDataBase();
                TaskRepository.MigrateDataBase();
            }
        }

        private void UpdateTasksFromDatabase()
        {
            TaskRepository taskRepository = new TaskRepository();
            List<ToDoListLibrary.Task> tasksFromDatabase = taskRepository.ReadAllTasks();


            tasks.Clear();
            foreach (var task in tasksFromDatabase)
            {
                tasks.Add(task);
            }

            dataGridTasks.ItemsSource = tasks;
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (tasks == null)
            {
                tasks = new List<ToDoListLibrary.Task>();
            }

            int maxId = tasks.Any() ? tasks.Max(task => task.Id) : 0;
            nextId = maxId + 1;

            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
            {
                Id = nextId,
                Title = txtFilter3.Text,
                Description = txtFilter2.Text,
                IsCompleted = false,
            };

            TaskRepository taskRepository = new TaskRepository();
            taskRepository.Create(newTask);

            tasks = taskRepository.ReadAllTasks();

            dataGridTasks.ItemsSource = tasks;

            txtFilter3.Text = "Name Tasks";
            txtFilter2.Text = "Description Tasks";

            dataGridTasks.Items.Refresh();
        }

        private void Button_Click_DeliteOll(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source = DB_ToDoList.sqlite";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("DELETE FROM ToDoList", connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            TaskRepository taskRepository = new TaskRepository();
            tasks = taskRepository.ReadAllTasks();
            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh();
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            selectedTaskForEdit = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            txtFilter3.Text = selectedTaskForEdit.Title;
            txtFilter2.Text = selectedTaskForEdit.Description;
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoListLibrary.Task selectedTask = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            tasks.Remove(selectedTask);

            TaskRepository taskRepository = new TaskRepository();
            taskRepository.Delete(selectedTask.Id);

            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh();
        }

        private void Button_Click_Done(object sender, RoutedEventArgs e)
        {
            if (selectedTaskForEdit != null)
            {
                selectedTaskForEdit.Title = txtFilter3.Text;
                selectedTaskForEdit.Description = txtFilter2.Text;

                TaskRepository taskRepository = new TaskRepository();
                taskRepository.Update(selectedTaskForEdit);

                tasks = taskRepository.ReadAllTasks();
                dataGridTasks.ItemsSource = tasks;
                dataGridTasks.Items.Refresh();
            }
        }
    }
}
