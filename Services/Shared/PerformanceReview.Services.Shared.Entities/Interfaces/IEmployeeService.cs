using PerformanceReview.Services.Shared.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.Services.Shared.Entities.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Create a new employee record in database.
        /// </summary>
        /// <param name="employee">New employee's information</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if record was created, false otherwise.</returns>
        Task<bool> CreateEmployee(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an employee record from database.
        /// </summary>
        /// <param name="employeeId">Employee Id.</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if record was deleted, false otherwise.</returns>
        Task<bool> DeleteEmployee(int employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve list of existing employees in database.
        /// </summary>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>List of employees.</returns>
        Task<IList<Employee>> GetEmployees(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an employees information in database.
        /// </summary>
        /// <param name="employee">Employee's information.</param>
        /// <param name="cancellationToken">Token that propagates notification that an operation should be cancelled.</param>
        /// <returns>True if update was successful and false otherwise.</returns>
        Task<bool> UpdateEmployee(Employee employee, CancellationToken cancellationToken = default);
    }
}
