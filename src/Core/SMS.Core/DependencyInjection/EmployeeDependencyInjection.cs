using Microsoft.Extensions.DependencyInjection;
using SMS.Core.Interfaces.IEmployee;
using SMS.Core.Managers;

namespace SMS.Core.DependencyInjection
{
    public static class EmployeeDependencyInjection
    {
        public static IServiceCollection EmployeeServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeManager, EmployeeManager>();

            return services;
        }
    }
}