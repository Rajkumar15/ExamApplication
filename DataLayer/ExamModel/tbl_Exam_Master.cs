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
    
    public partial class tbl_Exam_Master
    {
        public int pkid { get; set; }
        public string Exam_Name { get; set; }
        public Nullable<decimal> Passing_Percentage { get; set; }
        public string Instruction { get; set; }
        public Nullable<System.TimeSpan> ExamDuration { get; set; }
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
    }
}