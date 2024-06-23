using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repository
{
    class TaskManager
    {
        private TaskRepository taskRepository;

        public TaskManager()
        {
            taskRepository = new TaskRepository();
        }

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
