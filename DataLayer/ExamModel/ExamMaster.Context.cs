﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Exam_MasterEntities : DbContext
    {
        public Exam_MasterEntities()
            : base("name=Exam_MasterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_GroupMaster> tbl_GroupMaster { get; set; }
        public virtual DbSet<tbl_QuestionMaster> tbl_QuestionMaster { get; set; }
        public virtual DbSet<tbl_Stude_subjectTypeList> tbl_Stude_subjectTypeList { get; set; }
        public virtual DbSet<tbl_StudentMaster> tbl_StudentMaster { get; set; }
        public virtual DbSet<tbl_Subject_master> tbl_Subject_master { get; set; }
        public virtual DbSet<tbl_SubjectType_Master> tbl_SubjectType_Master { get; set; }
        public virtual DbSet<tbl_User_Profile> tbl_User_Profile { get; set; }
    }
}
