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
    
    public partial class tbl_PIOC_Arrival_DataMaster
    {
        public int pkid { get; set; }
        public Nullable<int> Session_fkid { get; set; }
        public Nullable<int> Course_fkid { get; set; }
        public Nullable<int> Division_fkid { get; set; }
        public Nullable<int> Force_fkid { get; set; }
        public Nullable<int> Student_fkid { get; set; }
        public string ChestNo { get; set; }
        public string ArmyNo { get; set; }
        public string Rank { get; set; }
        public string ParentUnit { get; set; }
        public string Records { get; set; }
        public string NCCUnit { get; set; }
        public string DTE { get; set; }
        public Nullable<System.DateTime> DateOfArrival { get; set; }
        public Nullable<System.DateTime> DateofDEP { get; set; }
        public string MoveOrder { get; set; }
        public string MedicalCertificate { get; set; }
        public string CharacterCertificate { get; set; }
        public string IdentityCard { get; set; }
        public string MobileNo { get; set; }
        public string IMEIno { get; set; }
        public string SignofIndividual { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}