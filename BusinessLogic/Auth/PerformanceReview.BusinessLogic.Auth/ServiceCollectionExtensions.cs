using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformanceReview.BusinessLogic.Auth.Data.Helpers;
using PerformanceReview.BusinessLogic.Auth.Entities.Interfaces;

namespace PerformanceReview.BusinessLogic.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<SecureHashHelper>();
            services.AddTransient<IAuthBusinessLogic, AuthBusinessLogic>();

            return services;
        }
    }
}
