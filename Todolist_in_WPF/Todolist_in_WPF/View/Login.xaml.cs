using BD;
using BD.Repository;
using Todolist_in_WPF.View;
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
        TaskRepository taskRepository;
        UserRepository userRepository;
        public Login()
        {
            InitializeComponent();
            userRepository = new UserRepository();
        }

        private bool VerifyPassword(string password, string savedPasswordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreateUser(string username, string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            User newUser = new User(username, savedPasswordHash);
            userRepository.CreateUser(newUser);
        }

        private bool LoginUser(string username, string password)
        {
            User user = userRepository.ReadAllUsers().FirstOrDefault(u => u.Username == username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                int userId = user.Id;
                List<ToDoListLibrary.Task> userTasks = taskRepository.GetTasksByUserId(userId);
                return true;
            }

            return false;
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            string username = txtFilterUsername.Text;
            string password = txtFilterPassword.Password;

            if (LoginUser(username, password))
            {
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void Registration_Button(object sender, RoutedEventArgs e)
        {
            Registration registrationControl = new Registration();

            if (MainGrid != null)
            {
                Grid.SetColumn(registrationControl, 1);
                MainGrid.Children.Add(registrationControl);
            }
        }
    }
}
