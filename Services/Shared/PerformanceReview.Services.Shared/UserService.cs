using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PerformanceReview.Services.Shared.Data.Enums;
using PerformanceReview.Services.Shared.Entities.Interfaces;
using PerformanceReview.Services.Shared.Entities.Models;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.Services.Shared
{
    public class UserService : IUserService
    {
        #region Private Readonly Members

        private readonly string _connectionString = null;
        private readonly ILogger<UserService> _logger = null;

        #endregion

        #region CTOR

        public UserService(IConfiguration configuration, ILogger<UserService> logger)
        {
            _connectionString = configuration.GetConnectionString(ConnectionType.DefaultConnection);
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<User> GetUserByUserId(string userId, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var query = $"SELECT {nameof(User.UserId)}, {nameof(User.Username)}, {nameof(User.EncodedKey)}, {nameof(User.EncodedSalt)}, {SharedTables.Users}.{nameof(User.RoleId)}, {SharedTables.Roles}.{nameof(User.RoleName)} " +
                        $"FROM {SharedTables.Users} " +
                        $"JOIN {SharedTables.Roles} ON {SharedTables.Users}.{nameof(User.RoleId)} = {SharedTables.Roles}.{nameof(User.RoleId)} " +
                        $"WHERE {nameof(User.UserId)} = '{userId}'";
                    var user = await connection.QueryFirstAsync<User>(new CommandDefinition(query, cancellationToken: cancellationToken));
                    if (user == null)
                        return null;

                    return user;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Could not find user by userId = '{userId}'");
                    return null;
                }
            }
        }

        public async Task<User> GetUserByUsername(string username, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var query = $"SELECT {nameof(User.UserId)}, {nameof(User.Username)}, {nameof(User.EncodedKey)}, {nameof(User.EncodedSalt)}, {SharedTables.Users}.{nameof(User.RoleId)}, {SharedTables.Roles}.{nameof(User.RoleName)} " +
                        $"FROM {SharedTables.Users} " +
                        $"JOIN {SharedTables.Roles} ON {SharedTables.Users}.{nameof(User.RoleId)} = {SharedTables.Roles}.{nameof(User.RoleId)} " +
                        $"WHERE {nameof(User.Username)} = '{username}'";
                    var user = await connection.QueryFirstOrDefaultAsync<User>(new CommandDefinition(query, cancellationToken: cancellationToken));
                    if (user == null)
                        return null;

                    return user;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Could not find user by username = '{username}'");
                    return null;
                }
            }
        }

        #endregion
    }
}
