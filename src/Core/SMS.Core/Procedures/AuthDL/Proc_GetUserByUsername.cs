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
    public class Proc_GetUserByUsername : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_GetUserByUsername(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var userName = (string)obj;
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);

            return _dAL.GetSingleByProcedure<UserDbModel>(GetName(), parameters);
        }

        public string GetName() => "Proc_GetUserByUsername";
    }
}
