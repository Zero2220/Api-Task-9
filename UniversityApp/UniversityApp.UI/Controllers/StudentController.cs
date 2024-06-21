using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using UniversityApp.UI.Filters;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class StudentController : Controller
    {
        private HttpClient _client;
        public StudentController()
        {
            _client  = new HttpClient();    
        }
        public async Task<IActionResult> Index(int page=1,int size=4)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);
            var queryString = new StringBuilder();
            queryString.Append("?page=").Append(Uri.EscapeDataString(page.ToString()));
            queryString.Append("&size=").Append(Uri.EscapeDataString(size.ToString()));

            
            string requestUrl = "https://localhost:7061/api/students" + queryString;
            using (var response = await _client.GetAsync(requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<PaginatedResponse<StudentListItemGetResponse>>(await response.Content.ReadAsStringAsync(),options);
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
        public async Task<IActionResult> Create(StudentCreateResponse createRequest)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(createRequest.FullName), "Fullname");
            content.Add(new StringContent(createRequest.Email), "Email");
            content.Add(new StringContent(createRequest.BirthDate.ToString()), "Birthdate");
            content.Add(new StringContent(createRequest.GroupId.ToString()), "GroupId");

            if(createRequest.formFile != null)
            {
                var streamContent = new StreamContent(createRequest.formFile.OpenReadStream());
                streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "File",
                    FileName = createRequest.formFile.Name
                };
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(createRequest.formFile.ContentType);

                content.Add(streamContent, "File", createRequest.formFile.FileName);
            }

            using (HttpResponseMessage response = await _client.PostAsync("https://localhost:7061/api/Students", content))
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

            using (var response = await _client.GetAsync("https://localhost:7061/api/Students/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    StudentCreateResponse request = JsonSerializer.Deserialize<StudentCreateResponse>(await response.Content.ReadAsStringAsync(), options);
                    return View(request);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    TempData["Error"] = "Students not found";
                else
                    TempData["Error"] = "Something went wrong!";
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentCreateResponse createRequest, int id)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(createRequest.FullName), "Fullname");
            content.Add(new StringContent(createRequest.Email), "Email");
            content.Add(new StringContent(createRequest.BirthDate.ToString()), "Birthdate");
            content.Add(new StringContent(createRequest.GroupId.ToString()), "GroupId");

            if (createRequest.formFile != null)
            {
                var streamContent = new StreamContent(createRequest.formFile.OpenReadStream());
                streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "File",
                    FileName = createRequest.formFile.Name
                };
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(createRequest.formFile.ContentType);

                content.Add(streamContent, "File", createRequest.formFile.FileName);
            }

            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

         
            using (HttpResponseMessage response = await _client.PutAsync("https://localhost:7061/api/Students/" + id, content))
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
        public async Task<IActionResult> Delete(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.DeleteAsync("https://localhost:7061/api/Students/" + id))
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
