using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Microsoft.Win32;
using System.Text;
using System.Text.Json;
using UniversityApp.UI.Filters;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class GroupController : Controller
    {
        private HttpClient _client;
        public GroupController()
        {
            _client = new HttpClient();
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);
            using (var response = await _client.GetAsync("https://localhost:7061/api/Groups?page=" + page + "&size=2"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var bodyStr = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    PaginatedResponse<GroupListItemGetResponse> data = JsonSerializer.Deserialize<PaginatedResponse<GroupListItemGetResponse>>(bodyStr, options);
                    if (data.TotalPages < page) return RedirectToAction("index", new { page = data.TotalPages });

                    return View(data);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else
                {
                    return RedirectToAction("error", "home");
                }
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateRequest createRequest)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

            var content = new StringContent(JsonSerializer.Serialize(createRequest), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _client.PostAsync("https://localhost:7061/api/Groups", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStringAsync(), options);

                    foreach (var item in errorResponse.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong!";
                }
            }

            return View(createRequest);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.GetAsync("https://localhost:7061/api/Groups/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    GroupCreateRequest request = JsonSerializer.Deserialize<GroupCreateRequest>(await response.Content.ReadAsStringAsync(), options);
                    return View(request);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    TempData["Error"] = "Group not found";
                else
                    TempData["Error"] = "Something went wrong!";
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupCreateRequest editRequest, int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

            var content = new StringContent(JsonSerializer.Serialize(editRequest), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _client.PutAsync("https://localhost:7061/api/Groups/" + id, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStringAsync(), options);

                    foreach (var item in errorResponse.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong!";
                }
            }

            return View(editRequest);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.DeleteAsync("https://localhost:7061/api/Groups/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }
    }
}
