using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListLibrary
{
    public class TaskList
    {
        private List<Task> tasks;

        public TaskList()
        {
            tasks = new List<Task>();
        }
        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            tasks.Remove(task);
        }

        public List<Task> GetAllTasks()
        {
            return tasks;
        }

    }
}
