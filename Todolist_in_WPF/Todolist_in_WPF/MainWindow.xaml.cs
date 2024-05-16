using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoListLibrary;

namespace Todolist_in_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximizad = false;
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

        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            int nextId = 1; 
            if (tasks == null)
            {
                tasks = new List<ToDoListLibrary.Task>();
            }

            // Создание нового объекта задания
            ToDoListLibrary.Task newTask = new ToDoListLibrary.Task
            {
                Id = nextId.ToString(), 
                Title = txtFilter3.Text,
                Description = txtFilter2.Text,
                IsCompleted = false 
            };

            tasks.Add(newTask);

            dataGridTasks.ItemsSource = tasks; 

            txtFilter3.Text = "Name Tasks"; 
            txtFilter2.Text = "Description Tasks"; 

            nextId++;

            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh(); // Обновить отображаемые данные в DataGrid
        }

        private void Button_Click_Settings(object sender, RoutedEventArgs e)
        {

        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoListLibrary.Task selectedTask = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh(); // Обновить отображаемые данные в DataGrid
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Получите выбранную задачу из DataGrid
            ToDoListLibrary.Task selectedTask = (ToDoListLibrary.Task)dataGridTasks.SelectedItem;

            // Удалите выбранную задачу из коллекции задач
            tasks.Remove(selectedTask);

            // Обновите DataGrid после удаления задачи
            dataGridTasks.ItemsSource = tasks;
            dataGridTasks.Items.Refresh(); // Обновить отображаемые данные в DataGrid
        }
    }
}
