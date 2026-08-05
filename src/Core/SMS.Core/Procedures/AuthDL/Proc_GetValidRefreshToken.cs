using Dapper;
using SMS.Core.Contracts;
using SMS.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.AuthDL
{
    public class Proc_GetValidRefreshToken : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_GetValidRefreshToken(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var tokenHash = (string)obj;
            var parameters = new DynamicParameters();
            parameters.Add("@TokenHash", tokenHash);

            return _dAL.GetSingleByProcedure<RefreshTokenDbModel>(GetName(), parameters);
        }

        public string GetName() => "Proc_GetValidRefreshToken";
    }
}
