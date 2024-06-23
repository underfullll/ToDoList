using BD;
using BD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        private List<User> users = new List<User>();

        public Registration()
        {
            InitializeComponent();
            EnsureDataBase();
            UpdateUsersFromDatabase();
        }
        public bool IsValidPassword(string password)
        {
            return password.Length >= 8;
        }
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }


        private void EnsureDataBase()
        {
            if (!UserRepository.IsDatabaseExist())
            {
                UserRepository.CreateDataBase();
                UserRepository.MigrateDataBase();
            }
        }

        private void UpdateUsersFromDatabase()
        {
            UserRepository userRepository = new UserRepository();
            users = userRepository.ReadAllUsers();
        }

        private void Button_Click_AddAccount(object sender, RoutedEventArgs e)
        {
            string username = txtFilterUsername.Text;
            string password = txtFilterPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a valid username and password.");
                return;
            }

            UserRepository userRepository = new UserRepository();

            if (userRepository.UserExists(username))
            {
                MessageBox.Show("User already exists. Please choose a different username.");
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Please enter a valid password. It should be at least 8 characters long.");
                return;
            }

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            User newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            userRepository.CreateUser(newUser);

            UpdateUsersFromDatabase();

            MessageBox.Show("User is registered.");
        }
    }
}
