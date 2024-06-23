using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repository
{
    public class TaskManager
    {
        public ObservableCollection<ToDoListLibrary.Task> Tasks { get; private set; }

        public TaskManager()
        {
            Tasks = new ObservableCollection<ToDoListLibrary.Task>();
            UpdateTasksFromDatabase();
        }

        public void UpdateTasksFromDatabase()
        {
            TaskRepository taskRepository = new TaskRepository();
            Tasks.Clear();
            foreach (var task in taskRepository.ReadAllTasks())
            {
                Tasks.Add(task);
            }
        }

        private TaskRepository taskRepository;

        public void FetchAndDisplayUserTasks(int userId)
        {
            List<ToDoListLibrary.Task> userTasks = taskRepository.ReadUserTasks(userId);

            DisplayUserTasks(userTasks);
        }

        private void DisplayUserTasks(List<ToDoListLibrary.Task> tasks)
        {
            foreach (var task in tasks)
            {
                Console.WriteLine($"Task ID: {task.Id}, Title: {task.Title}, Description: {task.Description}, Completed: {task.IsCompleted}");
            }
        }
    }
}
