using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Core.DTOs.Student;
using SMS.Core.Interfaces.StudentInterface;
using SMS.Core.Managers.StudentManager;

namespace SMS.API.Controllers.Student
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class StudentController : Controller
    {
        private readonly IStudentManager _studentManager;

        public StudentController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentRequest request)
        {
            var result = _studentManager.Create(request);
            if (result.IsError == 1) return BadRequest(result);
            return Ok(result);
        }
    }
}
