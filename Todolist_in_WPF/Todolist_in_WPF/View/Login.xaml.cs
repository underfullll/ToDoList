using BD;
using BD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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

namespace Todolist_in_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            string username = txtFilterUsername.Text;
            string password = txtFilterPassword.Password;
            UserRepository userRepository = new UserRepository();
            User user = userRepository.ReadAllUsers().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                MessageBox.Show("Login successful!");

                TaskRepository taskRepository = new TaskRepository();
                List<ToDoListLibrary.Task> userTasks = taskRepository.ReadAllTasks(user.UserId);
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void Button_Create_Image_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
