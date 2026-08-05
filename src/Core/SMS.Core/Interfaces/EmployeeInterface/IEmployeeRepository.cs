
using SMS.Core.DTOs;
using SMS.Core.Entities;

namespace SMS.Core.Interfaces.IEmployee
{
    public interface IEmployeeRepository
    {
        Task<CreateEmployeeRequest> CreateEmployeeAsync(CreateEmployeeRequest request);
    }
}
