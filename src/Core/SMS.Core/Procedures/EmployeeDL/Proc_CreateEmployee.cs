using Dapper;
using SMS.Core.Contracts;
using SMS.Core.DTOs;
using SMS.Core.DTOs.Employee;

namespace SMS.Core.Procedures.EmployeeDL
{
    public class Proc_CreateEmployee : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_CreateEmployee(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var (request, passwordHash) = ((CreateEmployeeRequest Request, string? PasswordHash))obj;

            var parameters = new DynamicParameters();
            parameters.Add("@EmpFName", request.EmpFName);
            parameters.Add("@EmpMName", request.EmpMName);
            parameters.Add("@EmpLName", request.EmpLName);
            parameters.Add("@EmpEmail", request.EmpEmail);
            parameters.Add("@EmpContact", request.EmpContact);
            parameters.Add("@EmpPAN", request.EmpPAN);
            parameters.Add("@EmpAadhar", request.EmpAadhar);
            parameters.Add("@EmpDOB", string.IsNullOrWhiteSpace(request.EmpDOB) ? (DateTime?)null : DateTime.Parse(request.EmpDOB));
            parameters.Add("@EmpJoiningDate", DateTime.Parse(request.EmpJoiningDate));
            parameters.Add("@EmpExitDate", string.IsNullOrWhiteSpace(request.EmpExitDate) ? (DateTime?)null : DateTime.Parse(request.EmpExitDate));
            parameters.Add("@EmpDesignationID", request.EmpDesignationID);
            parameters.Add("@EmpCanLogin", request.EmpCanLogin);
            parameters.Add("@BankId", request.BankId);
            parameters.Add("@EmpBankIFSC", request.EmpBankIFSC);
            parameters.Add("@EmpBankAccount", request.EmpBankAccount);
            parameters.Add("@EmpESICode", request.EmpESICode);
            parameters.Add("@EmpPFUAN", request.EmpPFUAN);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@PasswordHash", passwordHash);

            return _dAL.GetSingleByProcedure<CreateEmployeeResponse>(GetName(), parameters);
        }

        public string GetName() => "Proc_CreateEmployee";
    }
}