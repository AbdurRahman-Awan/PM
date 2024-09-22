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
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectManagementDbContext _context;

        public ProjectController(ProjectManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/project
        [HttpGet(Name = "GetProjects")]
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

            var totalItems = await query.CountAsync(); // Get total items for pagination
            var totalPages = (int)Math.Ceiling(totalItems / (double)pagination.PageSize); // Calculate total pages

            var projects = await query
                .OrderByDescending(p => p.StartDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var paginatedResponse = new PaginatedResponse<Project>
            {
                Items = projects,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            return Ok(paginatedResponse);
        }

        // GET: api/project/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tasks) // If you want to include related entities
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound(); // Return 404 if the project is not found
            }

            return Ok(project); // Return 200 with the project data
        }


        [HttpPost(Name = "CreateProject")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project
            {
                Title = projectDto.Title,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Status = projectDto.Status
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }


        // PUT: api/project/{id}
        [HttpPut("{id}", Name = "UpdateProject")]
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

        // DELETE: api/project/{id}
        [HttpDelete("{id}", Name = "DeleteProject")]
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
