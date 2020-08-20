using Microsoft.Extensions.DependencyInjection;
using PerformanceReview.Services.Shared.Entities.Interfaces;

namespace PerformanceReview.Services.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharedServices(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
