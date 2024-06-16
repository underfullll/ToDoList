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

            if (!IsValidPassword(txtFilterPassword.Text))
            {
                MessageBox.Show("Please enter a valid password. It should be at least 8 characters long.");
                return;
            }

            UserRepository userRepository = new UserRepository();

            if (userRepository.UserExists(txtFilterUsername.Text))
            {
                MessageBox.Show("User already exists. Please choose a different username.");
                return;
            }

            int maxUserId = UserRepository.GetMaxUserId();
            int nextUserId = maxUserId + 1;

            User newUser = new User
            {
                Id = nextUserId,
                Username = txtFilterUsername.Text,
                Password = txtFilterPassword.Text,
            };

            userRepository.CreateUser(newUser);

            users = userRepository.ReadAllUsers();

            MessageBox.Show("User is registered.");
            #region
            //if (!IsValidPassword(txtFilterPassword.Text))
            //{
            //    MessageBox.Show("Please enter a valid password. It should be at least 8 characters long.");
            //    return;
            //}

            //UserRepository userRepository = new UserRepository();

            //// Проверка существования пользователя
            //if (userRepository.UserExists(txtFilterUsername.Text))
            //{
            //    MessageBox.Show("User already exists. Please choose a different username.");
            //    return;
            //}

            //int maxUserId = UserRepository.GetMaxUserId();
            //int nextUserId = maxUserId + 1;

            //string salt = GenerateSalt();
            //string hashedPassword = HashPassword(txtFilterPassword.Text, salt);

            //User newUser = new User
            //{
            //    Id = nextUserId,
            //    Username = txtFilterUsername.Text,
            //    Password = hashedPassword,
            //    Salt = salt
            //};

            //userRepository.CreateUser(newUser);

            //users = userRepository.ReadAllUsers();
            #endregion
        }
    }
}
