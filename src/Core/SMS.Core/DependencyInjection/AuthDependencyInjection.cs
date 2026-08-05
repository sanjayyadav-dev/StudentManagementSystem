using Microsoft.Extensions.DependencyInjection;
using SMS.Core.Helpers;
using SMS.Core.Interfaces.IAuthManager;
using SMS.Core.Managers.AuthManager;

namespace SMS.Core.DependencyInjection
{
    public static class AuthDependencyInjection
    {
        public static IServiceCollection AuthServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<JwtHelper>();

            return services;   // ⚠️ ye missing tha
        }
    }
}