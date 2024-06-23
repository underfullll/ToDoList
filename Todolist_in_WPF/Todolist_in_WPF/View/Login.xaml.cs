﻿using BD;
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
        TaskRepository taskRepository;
        public Login()
        {
            InitializeComponent();

            taskRepository = new TaskRepository();
        }
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private bool LoginUser(string username, string password)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.ReadAllUsers().FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
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
            string password = txtFilterPassword.Text;

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
