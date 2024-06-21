using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private HttpClient _client;
        public AccountController()
        {
            _client = new HttpClient(); 
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var content = new StringContent(JsonSerializer.Serialize(loginRequest, options),System.Text.Encoding.UTF8,"application/json");
            using(var response = await _client.PostAsync("https://localhost:7061/api/Auth/login", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), options);

                    Response.Cookies.Append("token", "Bearer "+loginResponse.Token);
                    return RedirectToAction("index", "group");
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("","Email or Password incorrect!");
                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong";
                }
            }

            return View();
        }
    }
}
