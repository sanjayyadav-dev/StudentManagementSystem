// Infrastructure/Repositories/ProcDataLayer/EmployeeDL/Proc_CreateEmployee.cs
using Dapper;
using SMS.Core.Contracts;
using SMS.Core.DTOs;
using SMS.Core.DTOs.Employee;

namespace SMS.Infrastructure.Repositories.ProcDataLayer.EmployeeDL
{
    public class Proc_CreateEmployee : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_CreateEmployee(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var response = new CreateEmployeeResponse
            {
                IsError = 1,
                Message = "Something went wrong."
            };

            try
            {
                var request = (CreateEmployeeRequest)obj;   // ✅ FIX: Employee nahi, CreateEmployeeRequest

                var parameters = new DynamicParameters();
                parameters.Add("@EmpFName", request.EmpFName);
                parameters.Add("@EmpMName", request.EmpMName);
                parameters.Add("@EmpLName", request.EmpLName);
                parameters.Add("@EmpEmail", request.EmpEmail);
                parameters.Add("@EmpContact", request.EmpContact);
                parameters.Add("@EmpPAN", request.EmpPAN);
                parameters.Add("@EmpAadhar", request.EmpAadhar);
                parameters.Add("@EmpDOB", request.EmpDOB);
                parameters.Add("@EmpJoiningDate", request.EmpJoiningDate);
                parameters.Add("@EmpExitDate", request.EmpExitDate);
                parameters.Add("@EmpDesignationID", request.EmpDesignationID);
                parameters.Add("@EmpCanLogin", request.EmpCanLogin);
                parameters.Add("@BankId", request.BankId);
                parameters.Add("@EmpBankIFSC", request.EmpBankIFSC);
                parameters.Add("@EmpBankAccount", request.EmpBankAccount);
                parameters.Add("@EmpESICode", request.EmpESICode);
                parameters.Add("@EmpPFUAN", request.EmpPFUAN);
                parameters.Add("@CreatedBy", request.CreatedBy);   // ✅ ab uncomment kar sakte ho, CreatedBy milega

                var result = _dAL.GetSingleByProcedure<CreateEmployeeResponse>(GetName(), parameters);

                if (result != null)
                    response = result;
            }
            catch (Exception ex)
            {
                response.IsError = 1;
                response.Message = "Error: " + ex.Message;
            }

            return response;
        }

        public string GetName() => "Proc_CreateEmployee";
    }
}