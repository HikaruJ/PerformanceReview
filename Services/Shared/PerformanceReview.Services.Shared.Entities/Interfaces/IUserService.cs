using PerformanceReview.Services.Shared.Entities.Models;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.Services.Shared.Entities.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUserId(string userId, CancellationToken cancellationToken = default);
        Task<User> GetUserByUsername(string username, CancellationToken cancellationToken = default);
    }
}
