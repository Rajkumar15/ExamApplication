using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ExamModel;

namespace Exam_Application.Models.DAL
{
    public class ModelClassCustome
    {
    }
    public partial class tbl_User_Profiless
    {
        public int pkid { get; set; }
        public string User_fkid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }

    public partial class tbl_GroupMasterss
    {
        public int pkid { get; set; }
        public string GroupName { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
        public Nullable<System.DateTime> lastdatemodified { get; set; }
    }

    public partial class tbl_QuestionMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> subjecttype_fkid { get; set; }
        public string Question { get; set; }
        public string Explaination { get; set; }
        public Nullable<int> Subject_fkid { get; set; }
        public string hint { get; set; }
        public Nullable<decimal> Marks { get; set; }
        public Nullable<decimal> NegativeMarks { get; set; }
        public Nullable<int> SelectLevel { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> lastModified { get; set; }
        public Nullable<int> Division_fkid { get; set; }
    }

    public partial class QuestionMaster
    {
        public int pkid { get; set; }
        public Nullable<int> subjecttype_fkid { get; set; }
        [AllowHtml]
        public string Question { get; set; }
        [AllowHtml]
        public string Explaination { get; set; }
        public Nullable<int> Subject_fkid { get; set; }
        public string hint { get; set; }
        public Nullable<decimal> Marks { get; set; }
        public Nullable<decimal> NegativeMarks { get; set; }
        public Nullable<int> SelectLevel { get; set; }
        public Nullable<int> Division_fkid { get; set; }
        
        public Nullable<int> Ques_fkid { get; set; }
        public Nullable<int> Questtype_fkid { get; set; }
        public Nullable<int> QuestSubType_fkid { get; set; }
        [AllowHtml]
        public string Answer1 { get; set; }
        [AllowHtml]
        public string Answer2 { get; set; }
        [AllowHtml]
        public string Answer3 { get; set; }
        [AllowHtml]
        public string Answer4 { get; set; }
        public string BlanckSpace { get; set; }
        public bool TrueFalse { get; set; }
        [AllowHtml]
        public string SubAnswer { get; set; }
        public Nullable<int> CorrectAnswerDD { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> lastModified { get; set; }

        public List<tbl_MatchContentQuestionMaster> MATContent { get; set; }
        public List<tbl_MatchContentQuestionMaster> FULLF { get; set; }
    }

    public partial class tbl_Stude_subjectTypeListss
    {
        public int pkid { get; set; }
        public Nullable<int> stud_fkid { get; set; }
        public Nullable<int> subtype_fkid { get; set; }
        public Nullable<System.DateTime> addate { get; set; }
    } 

    public class tbl_Subject_masterss
    {
        public int pkid { get; set; }
        public string subjectName { get; set; }
        public string subjectDescription { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
        public Nullable<System.DateTime> lastmodifieddate { get; set; }
    }

    public partial class tbl_SubjectType_Masterss
    {
        public int pkid { get; set; }
        public string subjecttype { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
    }

    public partial class tbl_StudentMasterss
    {
        public int pkid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public Nullable<int> Course_fkid { get; set; }
        public Nullable<int> Division_fkid { get; set; }
        public Nullable<int> Force_fkid { get; set; }
        public string CommisionNo { get; set; }
        public Nullable<System.DateTime> DateComm { get; set; }
        public string Rank { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IdentificationMarks { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string ColorOfEye { get; set; }
        public string ColorOfHair { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string Nationality { get; set; }
        public string UnitAndLocation { get; set; }
        public string HQGroup { get; set; }
        public string Directorate { get; set; }
        public string State { get; set; }
        public string CourseSeries { get; set; }
        public string ChestNo { get; set; }
        public Nullable<System.DateTime> DUCFrom { get; set; }
        public Nullable<System.DateTime> DUCTo { get; set; }
        public Nullable<System.DateTime> ArrivalInNCCOTA { get; set; }
        public string MotherToughe { get; set; }
        public string PresentAddress { get; set; }
        public string ParmanentAddress { get; set; }
        public string MaritalStatus { get; set; }
        public string NOKFullName { get; set; }
        public string NOKRelation { get; set; }
        public string NOKAddress { get; set; }
        public string TeachingInstitideName { get; set; }
        public string TeachingSubject { get; set; }
        public Nullable<System.DateTime> TeachingDateOfEmplyment { get; set; }
        public string TeachingEmpStatus { get; set; }
        public Nullable<bool> NCCorOTUMember { get; set; }
        public Nullable<int> WhichDivision { get; set; }
        public Nullable<int> WhichForce { get; set; }
        public string TrainingPeriod { get; set; }
        public string RankofNCC { get; set; }
        public Nullable<bool> MedicallyExamined { get; set; }
        public string MedicalCertificate { get; set; }
        public string AlimentName { get; set; }
        public string MovementOrderNo { get; set; }
        public Nullable<System.DateTime> MovementDate { get; set; }
        public string Movement_copy { get; set; }
        public string Games { get; set; }
        public string OtherQualification { get; set; }
        public Nullable<bool> ReadNCCActRules { get; set; }
        public Nullable<bool> ReadNCCorDG { get; set; }
        public Nullable<bool> ReadSyllabus { get; set; }
        public Nullable<bool> ReadHandbook { get; set; }
        public Nullable<bool> ReadNCCCompPlanning { get; set; }
        public Nullable<bool> ReadNCCCompInstruction { get; set; }
        public Nullable<System.DateTime> DateOfPreCourseTraining { get; set; }
        public string BIODataForm { get; set; }
        public string Place { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }

    public partial class tbl_Student_Language_Masterss
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public string Language { get; set; }
        public string ReadL { get; set; }
        public string Write { get; set; }
        public string Speak { get; set; }
        public string Upderstand { get; set; }
    }

    public partial class tbl_Student_NCC_CourseMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public string CourseName { get; set; }
        public string TrainingInstitution { get; set; }
        public string TotalDuration { get; set; }
        public Nullable<System.DateTime> DurationFromDate { get; set; }
        public Nullable<System.DateTime> DurationToDate { get; set; }
        public string Grading { get; set; }
        public Nullable<bool> CheckRTU { get; set; }
        public string RTUReason { get; set; }
    }

    public partial class tbl_Student_NCCCertificateMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public string Institution { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
        public string Certificate_Obtained { get; set; }
        public string Type_campus { get; set; }
        public Nullable<System.DateTime> AttendedYear { get; set; }
        public Nullable<System.DateTime> AppointedYear { get; set; }
        public Nullable<System.DateTime> LastModifieddate { get; set; }
    }

    public partial class tbl_Student_Qualification_Masterss
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public string CertificateEducation { get; set; }
        public string Subject { get; set; }
        public string Division { get; set; }
        public Nullable<System.DateTime> YearofPasswed { get; set; }
        public string Colleage { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }

    public partial class tbl_Exam_Masterss
    {
        public int pkid { get; set; }
        public string Exam_Name { get; set; }
        public Nullable<decimal> Total_Marks { get; set; }
        public Nullable<decimal> PassingMarks { get; set; }
        public Nullable<decimal> Passing_Percentage { get; set; }
        [AllowHtml]
        public string Instruction { get; set; }
        public string ExamDuration { get; set; }
        public Nullable<int> AttemptCount { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> DeclareResult { get; set; }
        public Nullable<int> Group_fkid { get; set; }
        public Nullable<bool> Negative_Masrking { get; set; }
        public Nullable<bool> RandonQuestion { get; set; }
        public Nullable<bool> ResultAfterFinish { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<System.DateTime> LastModifieddate { get; set; }
        public Nullable<int> Course_fkid { get; set; }
        public Nullable<int> Force_fkid { get; set; }
        public Nullable<int> Division_fkid { get; set; }
    }

    public partial class tbl_Exam_QuestionsMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> Exam_fkid { get; set; }
        public Nullable<int> Question_fkid { get; set; }
        public Nullable<int> Course_fkid { get; set; }
        public Nullable<int> Division_fkid { get; set; }
        public Nullable<int> SelectLevel { get; set; }
        public Nullable<decimal> TotalMarks { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
    }

    public partial class tbl_Student_AnswerSheetss
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public Nullable<int> Exam_fkid { get; set; }
        public Nullable<int> Question_fkid { get; set; }
        public Nullable<int> Answer_fkid { get; set; }
        public Nullable<decimal> TotalMarks { get; set; }
        public Nullable<decimal> GainMarks { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
    }

    public partial class MainExamModel
    {
        public int RowNo { get; set; }
        public int pkid { get; set; }
        public Nullable<int> Examid { get; set; }
        public string ExamDuration { get; set; }
        public Nullable<int> Studentid { get; set; }
        public Nullable<int> subjecttype_fkid { get; set; }
        public string QuestypeName { get; set; }
        [AllowHtml]
        public string Question { get; set; }
        [AllowHtml]
        public string Explaination { get; set; }
        public Nullable<int> Subject_fkid { get; set; }
        public string hint { get; set; }
        public Nullable<decimal> Marks { get; set; }
        public Nullable<decimal> NegativeMarks { get; set; }
        public Nullable<int> SelectLevel { get; set; }
        public Nullable<int> Division_fkid { get; set; }

        public Nullable<int> Ques_fkid { get; set; }
        public Nullable<int> Questtype_fkid { get; set; }
        public Nullable<int> QuestSubType_fkid { get; set; }
        [AllowHtml]
        public string Answer1 { get; set; }
        [AllowHtml]
        public string Answer2 { get; set; }
        [AllowHtml]
        public string Answer3 { get; set; }
        [AllowHtml]
        public string Answer4 { get; set; }
        public string BlanckSpace { get; set; }
        public bool TrueFalse { get; set; }
        [AllowHtml]
        public string SubAnswer { get; set; }
        public Nullable<int> CorrectAnswerDD { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<int> Status { get; set; }

        public Nullable<System.DateTime> lastModified { get; set; }
        public Nullable<System.DateTime> ExamStartDatetime { get; set; }
        public List<tbl_MatchContentQuestionMaster> MATContent { get; set; }
        public List<tbl_MatchContentQuestionMaster> FULLF { get; set; }
        public List<QuestionIndexer> QueIndexer { get; set; }
    }

    public partial class QuestionIndexer
    {
        public int QueNo { get; set; }
        public int Quest_type { get; set; }
        public int Questionid { get; set; }
        public string ExamDuration { get; set; }
        public Nullable<System.DateTime> ExamStartTime { get; set; }
    }

    public partial class ObjectiveQuestions 
    {
        public Nullable<int> Examid { get; set; }
        public string ExamDuration { get; set; }
        public Nullable<int> Studentid { get; set; }
        public Nullable<int> QueId { get; set; }
        public Nullable<int> Queno { get; set; }
        public Nullable<int> QueTypeId { get; set; }
        public Nullable<decimal> QuesMarks { get; set; }
        public string QuestypeName { get; set; }
        [AllowHtml]
        public string Question { get; set; }

        [AllowHtml]
        public string Answer1 { get; set; }
        [AllowHtml]
        public string Answer2 { get; set; }
        [AllowHtml]
        public string Answer3 { get; set; }
        [AllowHtml]
        public string Answer4 { get; set; }

        public Nullable<int> CorrectAnswerDD { get; set; }
        public Nullable<bool> TrueFalse { get; set; }
        public string BlanckSpace { get; set; }


        public Nullable<int> StudentGiveAnswer { get; set; }
        public Nullable<int> CorrecrtWrong { get; set; }
        public Nullable<decimal> GainMarks { get; set; }

    }

    public partial class MatchContentQuestions
    {
        public Nullable<int> Examid { get; set; }
        public string ExamDuration { get; set; }
        public Nullable<int> Studentid { get; set; }
        public Nullable<int> QueId { get; set; }
        public Nullable<int> Queno { get; set; }
        public Nullable<decimal> QuesMarks { get; set; }
        public string QuestypeName { get; set; }
        [AllowHtml]
        public string Question { get; set; }

        public List<tbl_MatchContentQuestionMaster> Match { get; set; }

        public Nullable<int> StudentGiveAnswer { get; set; }
        public Nullable<int> CorrecrtWrong { get; set; }
        public Nullable<decimal> GainMarks { get; set; }

    }
}