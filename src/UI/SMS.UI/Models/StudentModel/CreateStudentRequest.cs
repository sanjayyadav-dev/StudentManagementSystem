namespace SMS.UI.Models.StudentModel
{
    public class CreateStudentRequest
    {
        public string StudentFName { get; set; } = string.Empty;
        public string? StudentMName { get; set; }
        public string StudentLName { get; set; } = string.Empty;
        public string? StudentGender { get; set; }
        public string StudentDOB { get; set; } = string.Empty;
        public string? StudentBloodGroup { get; set; }
        public string? StudentEmail { get; set; }
        public string? StudentContact { get; set; }
        public string? StudentAddress { get; set; }
        public string? StudentCity { get; set; }
        public string? StudentState { get; set; }
        public string? StudentPincode { get; set; }
        public string? StudentAadhar { get; set; }

        public int ClassId { get; set; }
        public int? SectionId { get; set; }
        public string AdmissionNo { get; set; } = string.Empty;
        public string AdmissionDate { get; set; } = string.Empty;
        public string? RollNumber { get; set; }
        public string? PreviousSchool { get; set; }

        public string? FatherName { get; set; }
        public string? FatherContact { get; set; }
        public string? FatherOccupation { get; set; }
        public string? MotherName { get; set; }
        public string? MotherContact { get; set; }
        public string? MotherOccupation { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianContact { get; set; }
        public string? GuardianRelation { get; set; }

        public bool StuCanLogin { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CreateStudentResponse
    {
        public int IsError { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string? StudentCode { get; set; }
    }
}
