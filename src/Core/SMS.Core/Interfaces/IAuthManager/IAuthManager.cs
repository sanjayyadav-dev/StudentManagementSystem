using SMS.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Interfaces.IAuthManager
{
    public interface IAuthManager
    {
        LoginResponseDto Login(LoginRequestDto request, string ipAddress);
        LoginResponseDto RefreshToken(RefreshTokenRequest request, string ipAddress);
        bool Logout(string refreshToken);
    }
}
