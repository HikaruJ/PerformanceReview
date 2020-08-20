using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PerformanceReview.BusinessLogic.Auth;
using PerformanceReview.BusinessLogic.Auth.Entities.Models;
using PerformanceReview.Services.Shared;
using System.Text;

namespace performanceReviewAPI
{
    public class Startup
    {
        #region Private Readonly Members

        private readonly IConfiguration _configuration = null;
        private readonly string _specificOriginsPolicyName = "_myAllowSpecificOrigins";

        #endregion

        #region CTOR

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Public Methods

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                else
                    app.UseHsts();

                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseCors(_specificOriginsPolicyName);

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure strongly typed settings object
            services.Configure<AuthSettings>(_configuration.GetSection("AuthSettings"));
            services.Configure<JwtIssuerOptions>(_configuration.GetSection("JwtIssuerOptions"));

            services.AddControllers();
            services.AddLogging();
            services.AddCors(options =>
            {
                options.AddPolicy(_specificOriginsPolicyName,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200",
                                        "http://localhost:4300")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            var authSettings = _configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
            var jwtIssuerOptions = _configuration.GetSection(nameof(JwtIssuerOptions)).Get<JwtIssuerOptions>();

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtIssuerOptions.Issuer,
                        ValidAudience = jwtIssuerOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                    };
                });

            services.ConfigureAuthBusinessLogic(_configuration);         
            services.ConfigureSharedServices();
        }

        #endregion
    }
}
