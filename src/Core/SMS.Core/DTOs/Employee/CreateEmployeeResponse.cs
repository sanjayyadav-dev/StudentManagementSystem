namespace SMS.Core.DTOs.Employee
{
    public class CreateEmployeeResponse
    {
        public int IsError { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? EmpCode { get; set; }
        public int EmployeeId { get; set; }
    }
}
