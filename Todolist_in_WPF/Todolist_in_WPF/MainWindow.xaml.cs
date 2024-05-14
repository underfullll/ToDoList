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
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();

            //Create Datagrid Items Info
            members.Add(new Member { IsCompleted = true, Id = "1", Title = "Стать Счастливым", Description = "Да не могу я спать, когда же наконец я стану СЧАСТЛИВЫМ?", });

            membersDataGrid.ItemsSource = members;
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
            if(e.ClickCount == 2)
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

        }
    }

    public class Member
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }


    }
}
