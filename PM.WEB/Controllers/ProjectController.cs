using Microsoft.AspNetCore.Mvc;
using PM.WEB.Models;

namespace PM.WEB.Controllers
{
    
        public partial class ProjectController : Controller
        {
            private readonly HttpClient _httpClient;

            public ProjectController(HttpClient httpClient)
            {
                _httpClient = httpClient;
                _httpClient.BaseAddress = new Uri("http://localhost:your_api_port/api/"); // Adjust port
            }

            public async Task<IActionResult> Index()
            {
                var projects = await _httpClient.GetFromJsonAsync<List<Project>>("project");
                return View(projects);
            }

            // Add Create, Update, Delete actions as needed
        }
    
}
