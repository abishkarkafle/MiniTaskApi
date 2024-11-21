using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = miniTaskAPI.Models.Task;

namespace miniTaskAPI.Interface
{
    public interface ITaskService
    {
        Task<Task> CreateTaskAsync(Task task);
        Task<Task> GetTaskByIdAsync(int id);
        Task<IEnumerable<Task>> GetAllTasksAsync();
        Task<bool> UpdateTaskAsync(Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<Task>> GetFilteredTasksAsync(
            string status = null,
            string priority = null,
            string assignedToId = null,
            DateTime? dueDate = null,
            string tag = null
        );
    }
}