using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM.API.Data;
using PM.API.Utilities;
using PM.DATA.Enums;
using PM.DATA.Models;
using PM.DATA.Models.Dto;

namespace PM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectManagementDbContext _context;

        public ProjectController(ProjectManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] PaginationParamsDto pagination, [FromQuery] string status = null)
        {
            // Basic validation for pagination parameters
            if (pagination.PageNumber <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (pagination.PageSize <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }

            var query = _context.Projects.AsQueryable();

            // Handle status filter if provided
            if (!string.IsNullOrEmpty(status))
            {
                if (status.TryParseEnum(out ProjectStatus projectStatus))
                {
                    query = query.Where(p => p.Status == projectStatus);
                }
                else
                {
                    return BadRequest("Invalid status parameter.");
                }
            }

            var projects = await query
                .OrderByDescending(p => p.StartDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project.StartDate >= project.EndDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return NotFound();
            }

            if (project.StartDate >= project.EndDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            existingProject.Title = project.Title;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.Status = project.Status;

            _context.Projects.Update(existingProject);
            await _context.SaveChangesAsync();
            return Ok(existingProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
