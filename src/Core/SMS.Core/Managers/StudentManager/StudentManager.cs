using Microsoft.AspNetCore.Identity;
using SMS.Core.Contracts;
using SMS.Core.DTOs.Student;
using SMS.Core.Interfaces.StudentInterface;
using SMS.Core.Procedures.StudentDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Managers.StudentManager
{
    public class StudentManager : IStudentManager
    {
        private readonly IDAL _iDAL;
        private readonly PasswordHasher<object> _passwordHasher = new();

        public StudentManager(IDAL iDAL)
        {
            _iDAL = iDAL;
        }
        public CreateStudentResponse Create(CreateStudentRequest request)
        {
            #region Validation
            if (string.IsNullOrWhiteSpace(request.StudentFName))
                return new CreateStudentResponse { IsError = 1, Message = "First name is required." };

            if (string.IsNullOrWhiteSpace(request.StudentLName))
                return new CreateStudentResponse { IsError = 1, Message = "Last name is required." };

            if (request.ClassId < 1)
                return new CreateStudentResponse { IsError = 1, Message = "Class is required." };

            if (string.IsNullOrWhiteSpace(request.AdmissionNo))
                return new CreateStudentResponse { IsError = 1, Message = "Admission number is required." };

            if (string.IsNullOrWhiteSpace(request.AdmissionDate))
                return new CreateStudentResponse { IsError = 1, Message = "Admission date is required." };

            if (string.IsNullOrWhiteSpace(request.StudentDOB))
                return new CreateStudentResponse { IsError = 1, Message = "Date of birth is required." };
            #endregion

            // ===== Agar login chahiye to password hash generate karo =====
            string? passwordHash = null;
            if (request.StuCanLogin)
            {
                const string defaultPassword = "Sms@123";
                passwordHash = _passwordHasher.HashPassword(null!, defaultPassword);
            }

            var proc = new Proc_CreateStudent(_iDAL);
            var result = (CreateStudentResponse)proc.Call((request, passwordHash));

            return result;
        }
    }
}
