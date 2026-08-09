using Microsoft.Extensions.Configuration;
using SMS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SMS.Core.Managers;   // ✅ YE LINE ADD KARO

namespace SMS.Infrastructure
{
    public class DAL : IDAL
    {
        private readonly string _connectionString;

        public DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public DataTable GetByProcedure(string procName, DynamicParameters parameters)
        {
            using var connection = CreateConnection();
            using var reader = connection.ExecuteReader(
                procName, parameters, commandType: CommandType.StoredProcedure);

            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

        public T GetSingleByProcedure<T>(string procName, DynamicParameters parameters)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<T>(
                procName, parameters, commandType: CommandType.StoredProcedure);
        }

        public int ExecuteNonQueryByProcedure(string procName, DynamicParameters parameters)
        {
            using var connection = CreateConnection();
            return connection.Execute(
                procName, parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<T> GetListByProcedure<T>(Response res)
        {
            using var connection = CreateConnection();
            return connection.Query<T>(
                res.Name, commandType: CommandType.StoredProcedure);
        }
    }
}