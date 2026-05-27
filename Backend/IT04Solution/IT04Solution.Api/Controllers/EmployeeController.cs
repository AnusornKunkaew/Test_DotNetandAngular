using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;
using IT04Solution.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT04Solution.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(request);
                return Ok(new { success = true, message = "save data success", data = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating employee");
                return BadRequest(new { success = false, errors = ex.Message.Split(", ") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeService.GetEmployeeByIdAsync(id);
                if (result == null)
                    return NotFound(new { success = false, message = "Employee not found" });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeesAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeRequest request)
        {
            try
            {
                var result = await _employeeService.UpdateEmployeeAsync(id, request);
                return Ok(new { success = true, message = "Employee updated successfully", data = result });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Employee not found");
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating employee");
                return BadRequest(new { success = false, errors = ex.Message.Split(", ") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                if (!result)
                    return NotFound(new { success = false, message = "Employee not found" });

                return Ok(new { success = true, message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Internal server error" });
            }
        }
    }
}
