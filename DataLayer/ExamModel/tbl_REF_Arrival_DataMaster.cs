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
    
    public partial class tbl_REF_Arrival_DataMaster
    {
        public int pkid { get; set; }
        public string ChestNo { get; set; }
        public string NCCNo { get; set; }
        public string Rank { get; set; }
        public string NCCunit { get; set; }
        public string GPHQ { get; set; }
        public string DTE { get; set; }
        public string MoveOrder { get; set; }
        public string NominalRoll { get; set; }
        public string IdentemnityBond { get; set; }
        public string MedicalCertificate { get; set; }
        public string RiskCertificate { get; set; }
        public string Copyofpreviouscertific { get; set; }
        public string DetailsService { get; set; }
        public string SignOfIndivisual { get; set; }
        public Nullable<int> Session_fkid { get; set; }
        public Nullable<int> Course_fkid { get; set; }
        public Nullable<int> Division_fkid { get; set; }
        public Nullable<int> Force_fkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
