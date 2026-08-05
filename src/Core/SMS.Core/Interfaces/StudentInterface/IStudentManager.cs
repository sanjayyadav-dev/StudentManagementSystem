using SMS.Core.DTOs.Student;
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
    }
}
