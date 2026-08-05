using Dapper;
using SMS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.AuthDL
{
    public class Proc_SaveRefreshToken : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_SaveRefreshToken(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var (userId, tokenHash, expiresAt, ip) = ((int UserId, string TokenHash, DateTime ExpiresAt, string Ip))obj;

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@TokenHash", tokenHash);
            parameters.Add("@ExpiresAt", expiresAt);
            parameters.Add("@CreatedByIp", ip);

            _dAL.ExecuteNonQueryByProcedure(GetName(), parameters);
            return true;
        }

        public string GetName() => "Proc_SaveRefreshToken";
    }
}
