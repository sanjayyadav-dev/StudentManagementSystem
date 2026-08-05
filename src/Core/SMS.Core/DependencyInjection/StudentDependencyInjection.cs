using Microsoft.Extensions.DependencyInjection;
using SMS.Core.Interfaces.StudentInterface;
using SMS.Core.Managers.StudentManager;

namespace SMS.Core.DependencyInjection
{
    public static class StudentDependencyInjection
    {
        public static IServiceCollection StudentServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentManager, StudentManager>();
            return services;
        }
    }
}
