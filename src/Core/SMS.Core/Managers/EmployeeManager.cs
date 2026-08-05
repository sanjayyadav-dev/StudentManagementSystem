using Microsoft.AspNetCore.Identity;
using SMS.Core.Contracts;
using SMS.Core.DTOs;
using SMS.Core.DTOs.Employee;
using SMS.Core.Interfaces.IEmployee;
using SMS.Core.Procedures.EmployeeDL;

namespace SMS.Core.Managers
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IDAL _iDAL;
        private readonly PasswordHasher<object> _passwordHasher = new();

        public EmployeeManager(IDAL iDAL)
        {
            _iDAL = iDAL;
        }

        public CreateEmployeeResponse Create(CreateEmployeeRequest request)
        {
            #region Validation
            if (string.IsNullOrWhiteSpace(request.EmpFName))
                return new CreateEmployeeResponse { IsError = 1, Message = "First name is required." };

            if (request.EmpDesignationID < 1)
                return new CreateEmployeeResponse { IsError = 1, Message = "Designation is required." };

            if (string.IsNullOrWhiteSpace(request.EmpEmail))
                return new CreateEmployeeResponse { IsError = 1, Message = "Email is required." };
            #endregion

            // ===== Agar login chahiye to password hash generate karo =====
            string? passwordHash = null;
            if (request.EmpCanLogin)
            {
                // Default password — production me isse email/SMS se bhejna, ya random generate karna
                const string defaultPassword = "Sms@123";
                passwordHash = _passwordHasher.HashPassword(null!, defaultPassword);
            }

            var proc = new Proc_CreateEmployee(_iDAL);
            var result = (CreateEmployeeResponse)proc.Call((request, passwordHash));

            return result;
        }
    }
}