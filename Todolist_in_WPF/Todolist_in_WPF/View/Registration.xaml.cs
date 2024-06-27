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

            if (!IsValidPassword(txtFilterPassword.Password))
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
                UserId = nextUserId,
                Username = txtFilterUsername.Text,
                Password = txtFilterPassword.Password,
            };

            userRepository.CreateUser(newUser);

            users = userRepository.ReadAllUsers();

            MessageBox.Show("User is registered.");

        }
    }
}