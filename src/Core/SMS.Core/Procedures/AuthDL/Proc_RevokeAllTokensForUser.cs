using Dapper;
using SMS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.AuthDL
{
    public class Proc_RevokeAllTokensForUser : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_RevokeAllTokensForUser(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var userId = (int)obj;
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            _dAL.ExecuteNonQueryByProcedure(GetName(), parameters);
            return true;
        }

        public string GetName() => "Proc_RevokeAllTokensForUser";
    }

}
