using Microsoft.AspNetCore.Mvc;
using PerformanceReview.API.Entities.Models;
using PerformanceReview.BusinessLogic.Auth.Entities.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace performanceReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        #region Private Readonly Members

        private readonly IAuthBusinessLogic _authBusinessLogic = null;

        #endregion

        #region CTOR

        public LoginController(IAuthBusinessLogic authBusinessLogic)
        {
            _authBusinessLogic = authBusinessLogic;
        }

        #endregion

        [Route("authorize")]
        [HttpPost]
        public async Task<IActionResult> Authorize([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _authBusinessLogic.Authenticate(request.Password, request.Username, cancellationToken);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
