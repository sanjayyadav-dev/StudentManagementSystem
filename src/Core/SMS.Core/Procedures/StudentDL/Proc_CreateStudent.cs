using Dapper;
using SMS.Core.Contracts;
using SMS.Core.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Procedures.StudentDL
{
    public class Proc_CreateStudent : IProcedure
    {
        private readonly IDAL _dAL;
        public Proc_CreateStudent(IDAL dAL) => _dAL = dAL;

        public object Call() => throw new NotImplementedException();

        public object Call(object obj)
        {
            var (request, passwordHash) = ((CreateStudentRequest Request, string? PasswordHash))obj;

            var parameters = new DynamicParameters();
            parameters.Add("@StudentFName", request.StudentFName);
            parameters.Add("@StudentMName", request.StudentMName);
            parameters.Add("@StudentLName", request.StudentLName);
            parameters.Add("@StudentGender", request.StudentGender);
            parameters.Add("@StudentDOB", DateTime.Parse(request.StudentDOB));
            parameters.Add("@StudentBloodGroup", request.StudentBloodGroup);
            parameters.Add("@StudentProfilePicture", request.StudentProfilePicture);
            parameters.Add("@StudentEmail", request.StudentEmail);
            parameters.Add("@StudentContact", request.StudentContact);
            parameters.Add("@StudentAddress", request.StudentAddress);
            parameters.Add("@StudentCity", request.StudentCity);
            parameters.Add("@StudentState", request.StudentState);
            parameters.Add("@StudentPincode", request.StudentPincode);
            parameters.Add("@StudentAadhar", request.StudentAadhar);
            parameters.Add("@ClassId", request.ClassId);
            parameters.Add("@SectionId", request.SectionId);
            parameters.Add("@AdmissionNo", request.AdmissionNo);
            parameters.Add("@AdmissionDate", DateTime.Parse(request.AdmissionDate));
            parameters.Add("@RollNumber", request.RollNumber);
            parameters.Add("@PreviousSchool", request.PreviousSchool);
            parameters.Add("@FatherName", request.FatherName);
            parameters.Add("@FatherContact", request.FatherContact);
            parameters.Add("@FatherOccupation", request.FatherOccupation);
            parameters.Add("@MotherName", request.MotherName);
            parameters.Add("@MotherContact", request.MotherContact);
            parameters.Add("@MotherOccupation", request.MotherOccupation);
            parameters.Add("@GuardianName", request.GuardianName);
            parameters.Add("@GuardianContact", request.GuardianContact);
            parameters.Add("@GuardianRelation", request.GuardianRelation);
            parameters.Add("@StuCanLogin", request.StuCanLogin);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@PasswordHash", passwordHash);

            return _dAL.GetSingleByProcedure<CreateStudentResponse>(GetName(), parameters);
        }

        public string GetName() => "Proc_CreateStudent";
    }
}
