using Dapper;
using SMS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.AuthDL
{
   
        public class Proc_GetRolesByUserId : IProcedure
        {
            private readonly IDAL _dAL;
            public Proc_GetRolesByUserId(IDAL dAL) => _dAL = dAL;

            public object Call() => throw new NotImplementedException();

            public object Call(object obj)
            {
                var userId = (int)obj;
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var dt = _dAL.GetByProcedure(GetName(), parameters);
                var roles = new List<string>();
                foreach (DataRow row in dt.Rows)
                    roles.Add(row["RoleName"].ToString()!);

                return roles;
            }

            public string GetName() => "Proc_GetRolesByUserId";
        }
    
}
