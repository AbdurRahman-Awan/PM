using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PM.WEB.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PM.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            // Send login request to API
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(result).Token;

                // Save the token in session
                HttpContext.Session.SetString("JWToken", token);

                // Redirect to a protected page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle login failure
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View(model);
            }
        }

     
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
