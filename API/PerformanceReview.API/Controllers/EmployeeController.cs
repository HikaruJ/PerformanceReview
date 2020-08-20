using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerformanceReview.API.Entities.Models;
using PerformanceReview.Services.Shared.Entities.Interfaces;
using PerformanceReview.Services.Shared.Entities.Models;
using System.Threading;
using System.Threading.Tasks;

namespace performanceReview.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Private Readonly Members

        private readonly IEmployeeService _employeeService = null;
        private readonly ILogger<EmployeeController> _logger = null;

        #endregion

        #region CTOR

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// REST DELETE endpoint for deleting an employee's record from database.
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if update was a success, false otherwise.</returns>
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> Delete(int employeeId, CancellationToken cancellationToken = default)
        {
            var isDeleteSuccess = await _employeeService.DeleteEmployee(employeeId, cancellationToken);
            return Ok(isDeleteSuccess);
        }

        /// <summary>
        /// REST GET endpoint for retreiving an employee list from database.
        /// </summary>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>List of employees</returns>
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeService.GetEmployees(cancellationToken);
            if (employees == null)
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(employees);
        }

        [HttpGet("{employeeId}")]
        public string Get(int employeeId)
        {
            return "value";
        }

        /// <summary>
        /// REST POST endpoint for creating a new employee in database.
        /// </summary>
        /// <param name="employee">Employee's information</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if update was a success, false otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Employee request cannot be empty.");
                return BadRequest("Employee request cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Employee request is invalid.");
                return BadRequest("Employee request is invalid.");
            }

            var isCreated = await _employeeService.CreateEmployee(new Employee()
            {
                Email = request.Email,
                EmployeeId = request.EmployeeId,
                FirstName = request.FirstName,
                LastName = request.LastName
            }, cancellationToken);

            return Ok(isCreated);
        }

        /// <summary>
        /// REST PUT endpoint for updating an employee's information in database.
        /// </summary>
        /// <param name="request">Employee's information recevied from http request.</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if update was a success, false otherwise.</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmployeeRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Employee request cannot be empty.");
                return BadRequest("Employee request cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Employee request is invalid.");
                return BadRequest("Employee request request is invalid.");
            }

            var isUpdated = await _employeeService.UpdateEmployee(new Employee()
            {
                Email = request.Email,
                EmployeeId = request.EmployeeId,
                FirstName = request.FirstName,
                LastName = request.LastName
            }, cancellationToken);

            return Ok(isUpdated);

        }
    }
}
