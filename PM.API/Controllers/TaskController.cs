using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM.API.Data;
using PM.DATA.Models;
using PM.DATA.Models.Dto;

namespace PM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ProjectManagementDbContext _context;

        public TaskController(ProjectManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] PaginationParamsDto pagination)
        {
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.StartDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task.StartDate >= task.EndDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            if (task.StartDate >= task.EndDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            existingTask.Title = task.Title;
            existingTask.StartDate = task.StartDate;
            existingTask.EndDate = task.EndDate;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;
            existingTask.IsRecurring = task.IsRecurring;
            existingTask.RecurrencePattern = task.RecurrencePattern;
            existingTask.AssignedToUserId = task.AssignedToUserId;

            _context.Tasks.Update(existingTask);
            await _context.SaveChangesAsync();
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("assign/{taskId}")]
        public async Task<IActionResult> AssignTask(int taskId, [FromBody] string userId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task.AssignedToUserId = userId;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }
    }

}
