using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PM.DATA.Models.Dto;
using PM.WEB.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PM.WEB.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProjectsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationParamsDto pagination)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"/api/Project?pageNumber={pagination.PageNumber}&pageSize={pagination.PageSize}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var paginatedResponse = JsonConvert.DeserializeObject<PaginatedResponse<ProjectModel>>(jsonResponse);
                return View(paginatedResponse);
            }

            return View(new PaginatedResponse<ProjectModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectModel project)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessages"] = "Please fill in all required fields and ensure the end date is greater than the start date.";
                return View(project);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/project", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToAction("Index");
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessages"] = $"Error creating project: {errorResponse}";

            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Project ID.");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Project/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var project = JsonConvert.DeserializeObject<ProjectModel>(jsonResponse);
                return project != null ? View(project) : NotFound("Project not found.");
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("Project not found.");
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, "Error fetching project details.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectModel project)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/Project/{project.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Error updating project.";
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.DeleteAsync($"/api/Project/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Error deleting project.";
            return RedirectToAction("Index");
        }
    }
}
