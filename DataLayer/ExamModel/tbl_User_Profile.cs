//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.ExamModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_User_Profile
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
}