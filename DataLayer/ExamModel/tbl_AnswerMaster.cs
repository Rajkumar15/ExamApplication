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
    
    public partial class tbl_AnswerMaster
    {
        public int pkid { get; set; }
        public Nullable<int> Ques_fkid { get; set; }
        public Nullable<int> Questtype_fkid { get; set; }
        public Nullable<int> QuestSubType_fkid { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public Nullable<int> CorrectAnswer { get; set; }
        public string BlanckSpace { get; set; }
        public bool TrueFalse { get; set; }
        public string SubAnswer { get; set; }
        public Nullable<System.DateTime> Adddate { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> lastmodified { get; set; }
    }
}