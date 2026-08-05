using SMS.Core.Entities;
using SMS.Core.Managers;

namespace SMS.Core.DTOs
{
    public class CreateEmployeeRequest
    {
        // Extra fields jo table me nahi hain, lekin creation ke time chahiye
        public int CreatedBy { get; set; }
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; } = string.Empty;
        public string EmpFName { get; set; } = string.Empty;
        public bool IsError { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? EmpMName { get; set; }
        public string EmpLName { get; set; } = string.Empty;
        public string EmpEmail { get; set; } = string.Empty;
        public string EmpContact { get; set; } = string.Empty;
        public string EmpPAN { get; set; } = string.Empty;
        public string EmpAadhar { get; set; } = string.Empty;
        public string EmpDOB { get; set; } = string.Empty;
        public string EmpJoiningDate { get; set; } = string.Empty;
        public string? EmpExitDate { get; set; }
        public int EmpDesignationID { get; set; }
        public bool EmpCanLogin { get; set; }
        public int BankId { get; set; }
        public string EmpBankIFSC { get; set; } = string.Empty;
        public string EmpBankAccount { get; set; } = string.Empty;
        public string? EmpESICode { get; set; }
        public string? EmpPFUAN { get; set; }
        public string? EmpProfilePicture { get; set; }
    }
}
