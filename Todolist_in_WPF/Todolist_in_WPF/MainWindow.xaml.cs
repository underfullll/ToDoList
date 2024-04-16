using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ToDoListLibrary;

namespace Todolist_in_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ToDoListLibrary.Task> TasksCollection { get; set; }
        private TaskList taskList;
        private int nextTaskId = 1;

        public MainWindow()
        {
            InitializeComponent();
            TasksCollection = new ObservableCollection<ToDoListLibrary.Task>();
            TaskListView.ItemsSource = TasksCollection; // Привязка источника данных к ListView
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ShowTaskPanelButton_Click(object sender, RoutedEventArgs e)
        {
            TaskPanel.Visibility = TaskPanel.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
            {
                Id = nextTaskId,
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                IsCompleted = false
            };

            // Добавляем новую задачу в коллекцию
            TasksCollection.Add(newTask);

            nextTaskId++;

            // Очищаем текстовые поля
            TaskTitleTextBox.Text = "Введите название задачи";
            TaskDescriptionTextBox.Text = "Введите описание задачи";

            DockPanel.SetDock(TaskPanel, Dock.Bottom);
        }
        
    }
}
