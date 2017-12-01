using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
    public partial class tbl_Stude_subjectTypeListss
    {
        public int pkid { get; set; }
        public Nullable<int> stud_fkid { get; set; }
        public Nullable<int> subtype_fkid { get; set; }
        public Nullable<System.DateTime> addate { get; set; }
    }
    public partial class tbl_StudentMasterss
    {
        public int pkid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string mobileno { get; set; }
        public string GuardianContact { get; set; }
        public string Enrollnmentno { get; set; }
        public Nullable<int> status { get; set; }
        public string ProfilePhoto { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<System.DateTime> lastmodifieddate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string user_fkid { get; set; }
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
}