﻿using BD;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Todolist_in_WPF.Utilities;
using Todolist_in_WPF.View;
using Todolist_in_WPF.ViewModel;


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
        private List<ToDoListLibrary.Task> tasks = new List<ToDoListLibrary.Task>();

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

        private void Btn_Tasks(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            Tasks tasksControl = new Tasks();

            if (MainGrid != null)
            {
                Grid.SetColumn(tasksControl, 1);
                MainGrid.Children.Add(tasksControl);
            }

        }

        private void Btn_Calendar(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            View.Calendar calendarControl = new View.Calendar();

            if (MainGrid != null)
            {
                Grid.SetColumn(calendarControl, 1);
                MainGrid.Children.Add(calendarControl);
            }
        }

        private void Btn_Notes(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            Notes notesControl = new Notes();

            if (MainGrid != null)
            {
                Grid.SetColumn(notesControl, 1);
                MainGrid.Children.Add(notesControl);
            }
        }

        private void Btn_Contacts(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            Contacts contactsControl = new Contacts();

            if (MainGrid != null)
            {
                Grid.SetColumn(contactsControl, 1);
                MainGrid.Children.Add(contactsControl);
            }
        }

        private void Registration_Button(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            Registration registrationControl = new Registration();

            if (MainGrid != null)
            {
                Grid.SetColumn(registrationControl, 1);
                MainGrid.Children.Add(registrationControl);
            }

        }

        private void Log_in_Button(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();

            Login loginControl = new Login();

            if (MainGrid != null)
            {
                Grid.SetColumn(loginControl, 1);
                MainGrid.Children.Add(loginControl);
            }

        }
    }

}
