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
    
    public partial class tbl_StudentExamResult
    {
        public int pkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public Nullable<int> Exam_fkid { get; set; }
        public Nullable<decimal> TotalMarks { get; set; }
        public Nullable<decimal> TotalGainMarks { get; set; }
        public Nullable<decimal> TotalPercentage { get; set; }
        public Nullable<decimal> TotalGainPercentage { get; set; }
        public Nullable<System.DateTime> AttemptExamDate { get; set; }
        public string Result { get; set; }
        public Nullable<bool> FinalStatus { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
}