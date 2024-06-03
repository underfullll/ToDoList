using BD;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Todolist_in_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateTasksFromDatabase();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximizad = false;
        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();
        private ToDoListLibrary.Task selectedTaskForEdit;
        private int nextId = 1;
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

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!IsMaximizad)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximizad = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximizad = true;
                }
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Tasks(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Calendar(object sender, RoutedEventArgs e)
        {

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

        private void Button_Click_Settings(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source = C:\\Users\\anton\\OneDrive\\Рабочий стол\\projects\\ToDoList\\Todolist_in_WPF\\BD\\BD\\DB_ToDoList.db; FailIfMissing=False"; // Подставьте сюда вашу строку подключения к базе данных

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Удаление существующих данных из таблицы ToDoList
                using (var command = new SQLiteCommand("DELETE FROM ToDoList", connection))
                {
                    command.ExecuteNonQuery();
                }

                // Добавление стандартных тасков в таблицу ToDoList
                string sqlQuery = @"
            INSERT INTO ToDoList (ID, Title, Description, IsCompleted)
            VALUES
                (1, 'Запустить ракету на Марс', 'Подготовиться к запуску и отправить ракету на Марс.', 1),
                (2, 'Закончить One Pice', 'Ну тут все понятно.', 0),
                (3, 'Выучить 5 новых языков программирования', 'Стать мастером пяти новых языков программирования за месяц.', 1),
                (4, 'Стать бесмертным', 'Разгадать генетический код, отвечающий за старение и смерть, и обрести вечную молодость.', 1),
                (5, 'Стать Счастливым', 'Да не могу я спать, когда же наконец я стану СЧАСТЛИВЫМ?', 0)";

                using (var command = new SQLiteCommand(sqlQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            // Обновление отображаемых данных в DataGrid
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
                // Обновление полей выбранной задачи
                selectedTaskForEdit.Title = txtFilter3.Text;
                selectedTaskForEdit.Description = txtFilter2.Text;

                // Обновление задачи в базе данных
                TaskRepository taskRepository = new TaskRepository();
                taskRepository.Update(selectedTaskForEdit);

                // Обновление отображаемых данных в DataGrid
                tasks = taskRepository.ReadAllTasks();
                dataGridTasks.ItemsSource = tasks;
                dataGridTasks.Items.Refresh();
            }
        }
    }

}
