using SMS.Core.DTOs;
using SMS.Core.DTOs.Employee;
using SMS.Core.Entities;

namespace SMS.Core.Interfaces.IEmployee
{
    public interface IEmployeeManager
    {
        CreateEmployeeResponse Create(CreateEmployeeRequest request); // sync, no Task
    }
}
