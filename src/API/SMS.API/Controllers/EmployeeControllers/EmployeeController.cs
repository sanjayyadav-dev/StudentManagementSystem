using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Core.DTOs;
using SMS.Core.Interfaces;
using SMS.Core.Interfaces.IEmployee;

namespace SMS.API.Controllers.EmployeeControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeManager _employeeManager;

        public EmployeeController(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateEmployeeRequest request)
        {
            var result = _employeeManager.Create(request);
            if (result.IsError == 1) return BadRequest(result);
            return Ok(result);
        }
    }
}