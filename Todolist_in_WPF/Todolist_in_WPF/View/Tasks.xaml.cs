using BD;
using BD.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Todolist_in_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        private int userId = 0;
        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();
        private ToDoListLibrary.Task selectedTaskForEdit;
        private int nextId = 1;
        private TaskRepository taskRepository;
        private UserRepository UserRepository;

        public Tasks()
        {
            InitializeComponent();
            EnsureDataBase();
            taskRepository = new TaskRepository();
            UserRepository = new UserRepository();
            LoadUserTasks();
        }
        private void LoadUserTasks()
        {
            List<ToDoListLibrary.Task> userTasks = taskRepository.ReadAllTasks(userId);

            tasks.Clear();
            tasks.AddRange(userTasks);

            dataGridTasks.ItemsSource = tasks;
        }
        private void RefreshDataGrid()
        {
            tasks = taskRepository.ReadAllTasks(userId);
            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh();
        }

        private void EnsureDataBase()
        {
            if (!TaskRepository.IsDatabaseExist())
            {
                TaskRepository.CreateDataBase();
                TaskRepository.MigrateDataBase();
            }
            if (!UserRepository.IsDatabaseExist())
            {
                UserRepository.CreateDataBase();
                UserRepository.MigrateDataBase();
            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (tasks == null)
            {
                tasks = new List<ToDoListLibrary.Task>();
            }

            int maxId = tasks.Any() ? tasks.Max(task => task.Id) : 0;
            nextId += 1;

            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
            {
                Id = nextId,
                Title = txtFilter3.Text,
                Description = txtFilter2.Text,
                IsCompleted = false,
                UserId = userId
            };

            taskRepository.Create(newTask);

            LoadUserTasks();

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

            nextId = 1;

            RefreshDataGrid();
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

                taskRepository.Update(selectedTaskForEdit);

                tasks = taskRepository.ReadAllTasks(userId);
                dataGridTasks.ItemsSource = tasks;
                dataGridTasks.Items.Refresh();
            }
        }
    }
}

