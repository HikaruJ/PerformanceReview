using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PerformanceReview.Services.Shared.Data.Enums;
using PerformanceReview.Services.Shared.Entities.Interfaces;
using PerformanceReview.Services.Shared.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.Services.Shared
{
    public class EmployeeService : IEmployeeService
    {
        #region Private Readonly Members

        private readonly string _connectionString = null;
        private readonly ILogger<EmployeeService> _logger = null;

        #endregion

        #region CTOR

        public EmployeeService(IConfiguration configuration, ILogger<EmployeeService> logger)
        {
            _connectionString = configuration.GetConnectionString(ConnectionType.DefaultConnection);
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new employee record in database.
        /// </summary>
        /// <param name="employee">New employee's information</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if record was created, false otherwise.</returns>
        public async Task<bool> CreateEmployee(Employee employee, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var sqlStatement = @$"INSERT INTO {SharedTables.Employees} ({nameof(Employee.EmployeeId)}, {nameof(Employee.Email)}, {nameof(Employee.FirstName)}, {nameof(Employee.LastName)})
                                        VALUES (@EmployeeId, @Email, @FirstName, @LastName)";
                    var updateResult = await connection.ExecuteAsync(new CommandDefinition(sqlStatement, cancellationToken: cancellationToken, parameters: employee));
                    return updateResult != 0;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to create employee with Id '{employee.EmployeeId}'");
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete an employee record from database.
        /// </summary>
        /// <param name="employeeId">Employee Id.</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if record was deleted, false otherwise.</returns>
        public async Task<bool> DeleteEmployee(int employeeId, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var sqlStatement = $"DELETE {SharedTables.Employees} WHERE {nameof(Employee.EmployeeId)} = @EmployeeId";
                    var updateResult = await connection.ExecuteAsync(new CommandDefinition(sqlStatement, cancellationToken: cancellationToken, parameters: new { EmployeeId = employeeId }));
                    return updateResult != 0;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to delete employee with Id '{employeeId}'");
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieve list of existing employees in database.
        /// </summary>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>List of employees.</returns>
        public async Task<IList<Employee>> GetEmployees(CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var query = $"SELECT {nameof(Employee.EmployeeId)}, {nameof(Employee.Email)}, {nameof(Employee.FirstName)}, {nameof(Employee.LastName)} from {SharedTables.Employees}";
                    var employees = await connection.QueryAsync<Employee>(new CommandDefinition(query, cancellationToken: cancellationToken));
                    return employees.ToList();
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to retrieve data from table Employees");
                    return null;
                }
            }
        }

        /// <summary>
        /// Update an employees information in database.
        /// </summary>
        /// <param name="employee">Employee's information.</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if update was successful and false otherwise.</returns>
        public async Task<bool> UpdateEmployee(Employee employee, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var sqlStatement = @$"UPDATE {SharedTables.Employees} SET {nameof(Employee.Email)} = @Email, {nameof(Employee.FirstName)} = @FirstName, {nameof(Employee.LastName)} = @LastName WHERE {nameof(Employee.EmployeeId)} = @EmployeeId";
                    var isSuccess = await connection.ExecuteAsync(new CommandDefinition(sqlStatement, cancellationToken: cancellationToken, parameters: employee));
                    return isSuccess != 0;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to update information for Employee with Id '{employee.EmployeeId}'");
                    return false;
                }
            }
        }

        #endregion
    }
}
