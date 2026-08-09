using Microsoft.AspNetCore.Mvc;
using SMS.API.Controllers.BaseAdminAuth;
using SMS.UI.Models.EmployeeModel;
using System.Net.Http.Json;

namespace SMS.UI.Controllers
{
    public class EmployeeController : BaseAdminController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IHttpClientFactory httpClientFactory, ILogger<EmployeeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest model)
        {
            if (model == null)
            {
                return Json(new { isError = 1, message = "Invalid data" });
            }

            model.CreatedBy = 1; // baad me session se le lena

            var client = _httpClientFactory.CreateClient("SMSApi");

            // AccessToken cookie (login ke time set hui thi) ko Authorization header me
            // attach karo — API pe [Authorize(Policy = "AdminOnly")] laga hai, isliye
            // token ke bina request 401 Unauthorized dega
            var accessToken = Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                return Json(new { isError = 1, message = "Session expired. Please login again." });
            }

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("api/employee", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reach Employee API");
                return Json(new { isError = 1, message = "Unable to reach the server. Please try again." });
            }

            // Agar token expire ho chuka hai, API 401 dega
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { isError = 1, message = "Session expired. Please login again." });
            }

            var rawContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(rawContent))
            {
                _logger.LogWarning("Employee API returned empty response. Status: {StatusCode}", response.StatusCode);
                return Json(new
                {
                    isError = 1,
                    message = $"Server returned an empty response (Status: {(int)response.StatusCode})."
                });
            }

            CreateEmployeeResponse? result;
            try
            {
                result = System.Text.Json.JsonSerializer.Deserialize<CreateEmployeeResponse>(
                    rawContent,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Employee API returned non-JSON content: {RawContent}", rawContent);
                return Json(new { isError = 1, message = "Unexpected response from server." });
            }

            if (response.IsSuccessStatusCode && result?.IsError == 0)
            {
                return Json(new
                {
                    isError = 0,
                    message = result.Message,
                    empCode = result.EmpCode
                });
            }

            return Json(new
            {
                isError = 1,
                message = result?.Message ?? $"Something went wrong (Status: {(int)response.StatusCode})."
            });
        }
    }
}