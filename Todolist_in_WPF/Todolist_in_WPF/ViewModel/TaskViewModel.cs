using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist_in_WPF.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ToDoListLibrary.Task> tasks;
        public ObservableCollection<ToDoListLibrary.Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }

        public TaskViewModel()
        {
            tasks = new ObservableCollection<ToDoListLibrary.Task>();
            LoadTasks();
        }

        private void LoadTasks()
        {
            // Загрузка задач из репозитория
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
