using Microsoft.AspNetCore.Identity;
using SMS.Core.Contracts;
using SMS.Core.DTOs.Auth;
using SMS.Core.Helpers;
using SMS.Core.Interfaces.IAuthManager;
using SMS.Core.Procedures.AuthDL;
using SMS.Core.Procedures.AuthDLk;


namespace SMS.Core.Managers.AuthManager
{
    public class AuthManager : IAuthManager
    {
        private readonly IDAL _iDAL;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher<object> _passwordHasher = new();

        public AuthManager(IDAL iDAL, JwtHelper jwtHelper)
        {
            _iDAL = iDAL;
            _jwtHelper = jwtHelper;
        }

        public LoginResponseDto Login(LoginRequestDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return new LoginResponseDto { IsError = 1, Message = "Username and password are required." };

            var getUserProc = new Proc_GetUserByUsername(_iDAL);
            var user = (UserDbModel?)getUserProc.Call(request.UserName);

            if (user == null)
                return new LoginResponseDto { IsError = 1, Message = "Invalid username or password." };

            if (!user.IsActive)
                return new LoginResponseDto { IsError = 1, Message = "Account is inactive." };

            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
                return new LoginResponseDto { IsError = 1, Message = "Account is locked. Try again later." };

            // ASP.NET Identity PasswordHasher se verify karo
            var verifyResult = _passwordHasher.VerifyHashedPassword(null!, user.PasswordHash!, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                return new LoginResponseDto { IsError = 1, Message = "Invalid username or password." };

            // Roles fetch karo
            var getRolesProc = new Proc_GetRolesByUserId(_iDAL);
            var roles = (List<string>)getRolesProc.Call(user.UserId);

            // Tokens generate karo
            var accessToken = _jwtHelper.GenerateAccessToken(user.UserId, user.UserName, user.EmployeeId, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshTokenHash = _jwtHelper.HashToken(refreshToken);

            var saveProc = new Proc_SaveRefreshToken(_iDAL);
            saveProc.Call((user.UserId, refreshTokenHash, DateTime.UtcNow.AddDays(7), ipAddress));

            return new LoginResponseDto
            {
                IsError = 0,
                Message = "Login successful.",
                UserId = user.UserId,
                UserName = user.UserName,
                EmpFName = user.EmpFName,
                EmpLName = user.EmpLName,
                EmpCode = user.EmpCode,
                Roles = roles,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public LoginResponseDto RefreshToken(RefreshTokenRequest request, string ipAddress)
        {
            var tokenHash = _jwtHelper.HashToken(request.RefreshToken);

            var getTokenProc = new Proc_GetValidRefreshToken(_iDAL);
            var storedToken = (RefreshTokenDbModel?)getTokenProc.Call(tokenHash);

            if (storedToken == null)
                return new LoginResponseDto { IsError = 1, Message = "Invalid refresh token." };

            // ===== Reuse Detection =====
            if (storedToken.IsRevoked)
            {
                // Ye token pehle se revoke ho chuka hai lekin dobara use ho raha hai — possible theft!
                var revokeAllProc = new Proc_RevokeAllTokensForUser(_iDAL);
                revokeAllProc.Call(storedToken.UserId);
                return new LoginResponseDto { IsError = 1, Message = "Token reuse detected. All sessions revoked for security." };
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return new LoginResponseDto { IsError = 1, Message = "Refresh token expired. Please login again." };

            // User + roles dobara fetch karo
            var getUserProc = new Proc_GetUserByUsername(_iDAL);
            // NOTE: yahan UserId se fetch karna behtar hoga, agar chaho to Proc_GetUserById bhi bana sakte hain
            var getRolesProc = new Proc_GetRolesByUserId(_iDAL);
            var roles = (List<string>)getRolesProc.Call(storedToken.UserId);

            // ===== Rotation: purana revoke, naya generate =====
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            var newRefreshTokenHash = _jwtHelper.HashToken(newRefreshToken);

            var revokeProc = new Proc_RevokeRefreshToken(_iDAL);
            revokeProc.Call((tokenHash, newRefreshTokenHash));

            var saveProc = new Proc_SaveRefreshToken(_iDAL);
            saveProc.Call((storedToken.UserId, newRefreshTokenHash, DateTime.UtcNow.AddDays(7), ipAddress));

            var newAccessToken = _jwtHelper.GenerateAccessToken(storedToken.UserId, "", null, roles);

            return new LoginResponseDto
            {
                IsError = 0,
                Message = "Token refreshed successfully.",
                UserId = storedToken.UserId,
                Roles = roles,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public bool Logout(string refreshToken)
        {
            var tokenHash = _jwtHelper.HashToken(refreshToken);
            var revokeProc = new Proc_RevokeRefreshToken(_iDAL);
            revokeProc.Call((tokenHash, (string?)null));
            return true;
        }
    }
}
