using Microsoft.EntityFrameworkCore;
using miniTaskAPI.Data;
using miniTaskAPI.Interface;
using Task = miniTaskAPI.Models.Task;

namespace miniTaskAPI.Repository
{
    public class TaskService : ITaskService
    {
        private readonly AuthDbContext _context;

        public TaskService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<bool> UpdateTaskAsync(Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);
            if (existingTask == null)
                return false;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;
            existingTask.DueDate = task.DueDate;
            existingTask.LastUpdatedDate = DateTime.UtcNow;
            existingTask.CategoryId = task.CategoryId;

            _context.Tasks.Update(existingTask);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Task>> GetFilteredTasksAsync(
            string status = null,
            string priority = null,
            string assignedToId = null,
            DateTime? dueDate = null,
            string tag = null
        )
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(t => t.Priority == priority);

            if (!string.IsNullOrEmpty(assignedToId))
                query = query.Where(t => t.CreatedById == assignedToId);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(t => t.Category != null && t.Category.Name.Contains(tag));

            return await query
                .Include(t => t.CreatedBy)
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
}
