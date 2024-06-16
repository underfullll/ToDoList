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
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
        public static string HashPassword(string password, string salt)
        {
            if (salt == null)
            {
                throw new ArgumentNullException(nameof(salt), "Salt value cannot be null.");
            }

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
        private bool LoginUser(string username, string password)
        {
            UserRepository userRepository = new UserRepository();

            User user = userRepository.ReadAllUsers().FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                if (user.Salt != null)
                {
                    string salt = user.Salt;
                    string hashedPassword = HashPassword(password, salt);

                    if (user.Password == hashedPassword)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {

            string username = txtFilterUsername.Text;
            string password = txtFilterPassword.Text;

            UserRepository userRepository = new UserRepository();

            User user = userRepository.ReadAllUsers().FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }

            #region
            //string username = txtFilterUsername.Text;
            //string password = txtFilterPassword.Text;

            //string salt = GenerateSalt();
            //string hashedPassword = HashPassword(txtFilterPassword.Text, salt);

            //User newUser = new User
            //{
            //    Username = txtFilterUsername.Text,
            //    Password = hashedPassword,
            //    Salt = salt
            //};

            //if (LoginUser(username, password))
            //{
            //    MessageBox.Show("Login successful!");
            //}
            //else
            //{
            //    MessageBox.Show("Invalid username or password. Please try again.");
            //}
            #endregion
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
