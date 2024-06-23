using BD;
using System.Collections.Generic;
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
        public Tasks()
        {
            InitializeComponent();
            List<ToDoListLibrary.Task> tasksToDisplay = FetchTasksFromDatabase();
            DisplayUserTasks(tasksToDisplay);
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


            dataGridTasks.Items.Clear();
            foreach (var task in tasksFromDatabase)
            {
                tasks.Add(task);
            }

            dataGridTasks.ItemsSource = tasks;
        }

        private void DisplayUserTasks(List<ToDoListLibrary.Task> tasks)
        {
            // Очистить предыдущий список задач
            dataGridTasks.Items.Clear();

            // Добавить все задачи в список для отображения
            foreach (var task in tasks)
            {
                // Создать текстовое представление задачи для отображения в ListBox
                string taskDescription = $"{task.Title}: {task.Description}";

                // Добавить задачу в ListBox
                dataGridTasks.Items.Add(taskDescription);
            }
        }
        private List<ToDoListLibrary.Task> FetchTasksFromDatabase()
        {
            TaskRepository taskRepository = new TaskRepository();
            return taskRepository.ReadAllTasks();
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

            int userId = 8;
            TaskRepository taskRepository = new TaskRepository();
            taskRepository.Create(newTask, userId);

            UpdateTasksFromDatabase();

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
