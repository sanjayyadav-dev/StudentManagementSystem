using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Core.Managers;
namespace SMS.Core.Contracts
{
    public interface IDAL
    {
        DataTable GetByProcedure(string procName, DynamicParameters parameters);
        T GetSingleByProcedure<T>(string procName, DynamicParameters parameters);
        IEnumerable<T> GetListByProcedure<T>(Response res);
        int ExecuteNonQueryByProcedure(string procName, DynamicParameters parameters);
    }
}
