using PerformanceReview.BusinessLogic.Auth.Entities.Models;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceReview.BusinessLogic.Auth.Entities.Interfaces
{
    public interface IAuthBusinessLogic
    {
        Task<AuthenticateResponse> Authenticate(string password, string username, CancellationToken cancellationToken = default);
    }
}
