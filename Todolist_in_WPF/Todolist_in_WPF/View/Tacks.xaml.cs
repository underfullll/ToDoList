//using BD;
//using System;
//using System.Collections.Generic;
//using System.Data.SQLite;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using Todolist_in_WPF.ViewModel;

//namespace Todolist_in_WPF.View
//{
//    /// <summary>
//    /// Логика взаимодействия для Tacks.xaml
//    /// </summary>
//    public partial class Tacks : Window
//    {
//        public Tacks()
//        {
//            InitializeComponent();
//            EnsureDataBase();
//            UpdateTasksFromDatabase();
//        }

//        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (e.ChangedButton == MouseButton.Left)
//            {
//                this.DragMove();
//            }
//        }
//        private bool IsMaximizad = false;
//        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();
//        private ToDoListLibrary.Task selectedTaskForEdit;
//        private int nextId = 1;

//        private void EnsureDataBase()
//        {
//            if (!TaskRepository.IsDatabaseExist())
//            {
//                TaskRepository.CreateDataBase();
//                TaskRepository.MigrateDataBase();
//            }
//        }

//        private void UpdateTasksFromDatabase()
//        {
//            TaskRepository taskRepository = new TaskRepository();
//            List<ToDoListLibrary.Task> tasksFromDatabase = taskRepository.ReadAllTasks();


//            tasks.Clear();
//            foreach (var task in tasksFromDatabase)
//            {
//                tasks.Add(task);
//            }

//            dataGridTasks.ItemsSource = tasks;
//        }

//        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//        {
//            if (e.ClickCount == 2)
//            {
//                if (!IsMaximizad)
//                {
//                    this.WindowState = WindowState.Normal;
//                    this.Width = 1080;
//                    this.Height = 720;

//                    IsMaximizad = false;
//                }
//                else
//                {
//                    this.WindowState = WindowState.Maximized;

//                    IsMaximizad = true;
//                }
//            }
//        }

//        private void Button_Click_Exit(object sender, RoutedEventArgs e)
//        {
//            this.Close();
//        }


//        private void Button_Click_Add(object sender, RoutedEventArgs e)
//        {
//            if (tasks == null)
//            {
//                tasks = new List<ToDoListLibrary.Task>();
//            }

//            int maxId = tasks.Any() ? tasks.Max(task => task.Id) : 0;
//            nextId = maxId + 1;

//            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
//            {
//                Id = nextId,
//                Title = txtFilter3.Text,
//                Description = txtFilter2.Text,
//                IsCompleted = false,
//            };

//            TaskRepository taskRepository = new TaskRepository();
//            taskRepository.Create(newTask);

//            tasks = taskRepository.ReadAllTasks();

//            dataGridTasks.ItemsSource = tasks;

//            txtFilter3.Text = "Name Tasks";
//            txtFilter2.Text = "Description Tasks";

//            dataGridTasks.Items.Refresh();
//        }

//        private void Button_Click_DeliteOll(object sender, RoutedEventArgs e)
//        {
//            // Здесь вы можете использовать тот же connectionString, что и в классе TaskRepository
//            string connectionString = "Data Source = DB_ToDoList.sqlite";

//            using (var connection = new SQLiteConnection(connectionString))
//            {
//                connection.Open();

//                // Удаление существующих данных из таблицы ToDoList
//                using (var command = new SQLiteCommand("DELETE FROM ToDoList", connection))
//                {
//                    command.ExecuteNonQuery();
//                }

//                connection.Close();
//            }

//            RefreshDataGrid();
//        }

//        private void RefreshDataGrid()
//        {
//            // Обновление отображаемых данных в DataGrid
//            TaskRepository taskRepository = new TaskRepository();
//            tasks = taskRepository.ReadAllTasks();
//            dataGridTasks.ItemsSource = tasks;
//            dataGridTasks.Items.Refresh();
//        }

//        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
//        {
//            selectedTaskForEdit = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

//            txtFilter3.Text = selectedTaskForEdit.Title;
//            txtFilter2.Text = selectedTaskForEdit.Description;
//        }

//        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
//        {
//            ToDoListLibrary.Task selectedTask = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

//            tasks.Remove(selectedTask);

//            TaskRepository taskRepository = new TaskRepository();
//            taskRepository.Delete(selectedTask.Id);

//            dataGridTasks.ItemsSource = tasks;
//            dataGridTasks.Items.Refresh();
//        }

//        private void Button_Click_Done(object sender, RoutedEventArgs e)
//        {
//            if (selectedTaskForEdit != null)
//            {
//                // Обновление полей выбранной задачи
//                selectedTaskForEdit.Title = txtFilter3.Text;
//                selectedTaskForEdit.Description = txtFilter2.Text;

//                // Обновление задачи в базе данных
//                TaskRepository taskRepository = new TaskRepository();
//                taskRepository.Update(selectedTaskForEdit);

//                // Обновление отображаемых данных в DataGrid
//                tasks = taskRepository.ReadAllTasks();
//                dataGridTasks.ItemsSource = tasks;
//                dataGridTasks.Items.Refresh();
//            }
//        }
//    }
//}
