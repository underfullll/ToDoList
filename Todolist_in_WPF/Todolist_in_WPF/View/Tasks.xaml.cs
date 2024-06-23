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
        private TaskManager taskManager;
        public Tasks()
        {
            InitializeComponent();
            EnsureDataBase();
            taskManager = new TaskManager();
            dataGridTasks.ItemsSource = taskManager.Tasks;
        }
        private ObservableCollection<ToDoListLibrary.Task> tasks = new ObservableCollection<ToDoListLibrary.Task>();
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
            tasks = new ObservableCollection<ToDoListLibrary.Task>(taskRepository.ReadAllTasks());
            dataGridTasks.ItemsSource = tasks;
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            int maxId = tasks.Count > 0 ? tasks.Max(task => task.Id) : 0;
            nextId = maxId + 1;

            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
            {
                Id = nextId,
                Title = txtFilter3.Text,
                Description = txtFilter2.Text,
                IsCompleted = false,
            };

            int userId = 9;
            TaskRepository taskRepository = new TaskRepository();
            taskRepository.Create(newTask, userId);

            UpdateTasksFromDatabase();
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

            UpdateTasksFromDatabase();
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            selectedTaskForEdit = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            if (selectedTaskForEdit != null)
            {
                txtFilter3.Text = selectedTaskForEdit.Title;
                txtFilter2.Text = selectedTaskForEdit.Description;
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoListLibrary.Task selectedTask = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            if (selectedTask != null)
            {
                TaskRepository taskRepository = new TaskRepository();
                taskRepository.Delete(selectedTask.Id);

                UpdateTasksFromDatabase();
            }
        }

        private void Button_Click_Done(object sender, RoutedEventArgs e)
        {
            if (selectedTaskForEdit != null)
            {
                selectedTaskForEdit.Title = txtFilter3.Text;
                selectedTaskForEdit.Description = txtFilter2.Text;

                TaskRepository taskRepository = new TaskRepository();
                taskRepository.Update(selectedTaskForEdit);

                UpdateTasksFromDatabase();
            }
        }
    }
}
