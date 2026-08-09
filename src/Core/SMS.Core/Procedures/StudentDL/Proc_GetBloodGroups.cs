using Dapper;
using SMS.Core.Contracts;
using SMS.Core.DTOs.BlodGoupModel;
using SMS.Core.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SMS.Core.Managers;
using System.Text;
using System.Threading.Tasks;
namespace SMS.Core.Procedures.StudentDL
{
    public class Proc_GetBloodGroups : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_GetBloodGroups(IDAL dAL) => _dAL = dAL;

        public object Call()
        {
            var res = new Response { Name = GetName() };
            IEnumerable<BloodGroup> list = _dAL.GetListByProcedure<BloodGroup>(res);
            return list;
        }

        public object Call(object obj) => Call();

        public string GetName() => "Proc_GetBloodGroups";
    }
}
