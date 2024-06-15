using BD;
using BD.Repository;
using System;
using System.Collections.Generic;
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

namespace Todolist_in_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        private List<User> users = new List<User>();
        private int nextUserId = 1;

        public Registration()
        {
            InitializeComponent();
            EnsureDataBase();
            UpdateUsersFromDatabase();
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
            int maxUserId = UserRepository.GetMaxUserId();
            nextUserId = maxUserId + 1;

            User newUser = new User
            {
                Id = nextUserId,
                Username = txtFilterUsername1.Text, 
                Password = txtFilterPassword1.Text,
            };

            UserRepository userRepository = new UserRepository();
            userRepository.CreateUsers(newUser);

            //users = userRepository.ReadAllUsers();
        }
    }
}
