using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PerformanceReview.BusinessLogic.Auth.Data.Helpers;
using PerformanceReview.BusinessLogic.Auth.Entities.Interfaces;
using PerformanceReview.BusinessLogic.Auth.Entities.Models;
using PerformanceReview.Services.Shared.Entities.Interfaces;
using PerformanceReview.Services.Shared.Entities.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.BusinessLogic.Auth
{
    public class AuthBusinessLogic : IAuthBusinessLogic
    {
        #region Private Readonly Members

        private readonly AuthSettings _authSettings = null;
        private readonly JwtIssuerOptions _jwtIssuerOptions = null;
        private readonly ILogger<AuthBusinessLogic> _logger = null;
        private readonly SecureHashHelper _secureHashHelper = null;
        private readonly IUserService _userService = null;

        #endregion

        #region CTOR

        public AuthBusinessLogic(IOptions<AuthSettings> authSettings, IOptions<JwtIssuerOptions> jwtIssuerOptions, ILogger<AuthBusinessLogic> logger, SecureHashHelper secureHashHelper, IUserService userService)
        {
            _authSettings = authSettings.Value;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _logger = logger;
            _secureHashHelper = secureHashHelper;
            _userService = userService;
        }

        #endregion

        #region Public Methods

        public async Task<AuthenticateResponse> Authenticate(string password, string username, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetUserByUsername(username);

            if (user == null)
            {
                _logger.LogWarning($"Could not find user with username '{username}'");
                return null;
            }

            var isPasswordMatch = _secureHashHelper.IsPasswordMatch(user.EncodedKey, user.EncodedSalt, password);
            if (!isPasswordMatch)
            {
                _logger.LogWarning($"Invalid password for username '{username}'");
                return null;

            }

            // Generate jwt token for successful authentication
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse()
            {
                RoleName = user.RoleName,
                Token = token,
                UserId = user.UserId.ToString(),
                Username = user.Username
            };
        }

        #endregion

        #region Private Methods

        private string GenerateJwtToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                audience: _jwtIssuerOptions.Audience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                issuer: _jwtIssuerOptions.Issuer,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        #endregion
    }
}
