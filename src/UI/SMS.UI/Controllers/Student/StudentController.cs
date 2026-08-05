using Microsoft.AspNetCore.Mvc;
using SMS.UI.Models.StudentModel;
using System.Text.Json;

namespace SMS.UI.Controllers.Student
{
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
        {
            var client = _httpClientFactory.CreateClient("SMSApi");
            var response = await client.PostAsJsonAsync("api/student", request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; // <-- ye line add ki

            if (string.IsNullOrWhiteSpace(responseBody))
            {
                return Json(new
                {
                    isError = 1,
                    message = $"API returned empty response. Status: {response.StatusCode}"
                });
            }

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResult = JsonSerializer.Deserialize<CreateStudentResponse>(responseBody, options);
                    return Json(errorResult);
                }
                catch
                {
                    return Json(new { isError = 1, message = $"API Error ({(int)response.StatusCode}): {responseBody}" });
                }
            }

            var result = JsonSerializer.Deserialize<CreateStudentResponse>(responseBody, options);
            return Json(result);
        }
    }
}