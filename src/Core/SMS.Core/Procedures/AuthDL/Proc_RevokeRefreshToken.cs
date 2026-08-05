using Dapper;
using SMS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.AuthDLk
{
    public class Proc_RevokeRefreshToken : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_RevokeRefreshToken(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var (tokenHash, replacedBy) = ((string TokenHash, string? ReplacedBy))obj;

            var parameters = new DynamicParameters();
            parameters.Add("@TokenHash", tokenHash);
            parameters.Add("@ReplacedByTokenHash", replacedBy);

            _dAL.ExecuteNonQueryByProcedure(GetName(), parameters);
            return true;
        }

        public string GetName() => "Proc_RevokeRefreshToken";
    }
}
