namespace SMS.UI.Models.AuthModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ApiLoginResponse
    {
        public int IsError { get; set; }
        public string? Message { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? EmpFName { get; set; }
        public string? EmpLName { get; set; }
        public string? EmpCode { get; set; }
        public List<string> Roles { get; set; } = new();
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
