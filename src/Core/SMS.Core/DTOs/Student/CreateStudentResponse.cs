using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.DTOs.Student
{
    public class CreateStudentResponse
    {
        public int IsError { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string? StudentCode { get; set; }
    }
}
