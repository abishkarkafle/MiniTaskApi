using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniTaskAPI.DTOs;
using miniTaskAPI.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = miniTaskAPI.Models.Task;

namespace miniTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound("Task not found.");
                return Ok(task);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var task = new Task
            {
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                Status = taskCreateDto.Status,
                Priority = taskCreateDto.Priority,
                CreatedById = userId,
                DueDate = taskCreateDto.DueDate,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskUpdateDto)
        {
            if (id != taskUpdateDto.Id)
                return BadRequest("Task ID mismatch.");

            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
                return NotFound("Task not found.");

            existingTask.Title = taskUpdateDto.Title ?? existingTask.Title;
            existingTask.Description = taskUpdateDto.Description ?? existingTask.Description;
            existingTask.Status = taskUpdateDto.Status ?? existingTask.Status;
            existingTask.Priority = taskUpdateDto.Priority ?? existingTask.Priority;
            existingTask.LastUpdatedDate = DateTime.UtcNow;

            try
            {
                var isUpdated = await _taskService.UpdateTaskAsync(existingTask);
                if (!isUpdated)
                    return StatusCode(500, "Error updating task.");
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var isDeleted = await _taskService.DeleteTaskAsync(id);
                if (!isDeleted)
                    return NotFound("Task not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("filter")]
        [Authorize]
        public async Task<IActionResult> GetFilteredTasks(
            [FromQuery] string status = null,
            [FromQuery] string priority = null,
            [FromQuery] string assignedToId = null,
            [FromQuery] DateTime? dueDate = null,
            [FromQuery] string tag = null)
        {
            try
            {
                var filteredTasks = await _taskService.GetFilteredTasksAsync(status, priority, assignedToId, dueDate, tag);
                return Ok(filteredTasks);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
