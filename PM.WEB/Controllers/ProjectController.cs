using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/Project");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonResponse);
                return View(projects);
            }

            return View(new List<ProjectModel>());
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

            // Updated endpoint
            var response = await client.PostAsync("api/project", content); // Changed to "api/project"

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToAction("Index");
            }

            // Optionally, log response details for debugging
            var errorResponse = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessages"] = $"Error creating project: {errorResponse}";

            return View(project);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Ensure that the ID is valid
            if (id <= 0)
            {
                return BadRequest("Invalid Project ID.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"api/Project/{id}");

                // Check if the API call was successful
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var project = JsonConvert.DeserializeObject<ProjectModel>(jsonResponse);

                    // Check if the project was found
                    if (project == null)
                    {
                        return NotFound("Project not found.");
                    }

                    // Return the view with the project model to edit
                    return View(project);
                }
                else
                {
                    // Handle different response codes
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound("Project not found.");
                    }

                    // Log the response for debugging purposes
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    // Log the error here if needed

                    return StatusCode((int)response.StatusCode, "Error fetching project details.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error occurred while fetching project details.");

                return StatusCode(500, "Internal server error. Please try again later.");
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
