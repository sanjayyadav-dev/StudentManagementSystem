using Dapper;
using SMS.Core.DTOs.Student;
using SMS.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Interfaces.StudentInterface
{
    public interface IStudentManager
    {
        CreateStudentResponse Create(CreateStudentRequest request);
        object GetAllBlodGrupInDdl();
    }
}
