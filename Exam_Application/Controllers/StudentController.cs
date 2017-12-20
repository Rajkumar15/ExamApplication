using System;
using DataLayer;
using DataLayer.ExamModel;
using Exam_Application.Models.BAL;
using Exam_Application.Models.DAL;
using itechDll;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam_Application.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        public readonly IRepository<tbl_User_Profile> _user;
        public readonly IRepository<tbl_GroupMaster> _group;
        public readonly IRepository<tbl_CourseMaster> _course;
        public readonly IRepository<tbl_DivisionMaster> _division;
        public readonly IRepository<tbl_QuestionType_Master> _subtype;
        public readonly IRepository<tbl_StudentMaster> _student;
        public readonly IRepository<tbl_QuestionMaster> _question;
        public readonly IRepository<tbl_AnswerMaster> _answer;
        public readonly IRepository<tbl_MatchContentQuestionMaster> _Matc;
        public readonly IRepository<tbl_ForceMaster> _force;
        public readonly IRepository<tbl_Student_NCC_CourseMaster> _ncccourse;
        public readonly IRepository<tbl_Student_NCCCertificateMaster> _ncccer;
        public readonly IRepository<tbl_Student_Language_Master> _nccLang;
        public readonly IRepository<tbl_Student_Qualification_Master> _nccQua;

        public StudentController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
            IRepository<tbl_DivisionMaster> division, IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer, IRepository<tbl_MatchContentQuestionMaster> Matc, IRepository<tbl_ForceMaster> force,
            IRepository<tbl_Student_NCC_CourseMaster> ncccourse, IRepository<tbl_Student_NCCCertificateMaster> ncccer, IRepository<tbl_Student_Qualification_Master> nccQua,
            IRepository<tbl_Student_Language_Master> nccLang)
        {
            _user = user;
            _group = group;
            _course = course;
            _division = division;
            _subtype = subtype;
            _student = student;
            _question = question;
            _answer = answer;
            _Matc = Matc;
            _force = force;
            _ncccourse = ncccourse;
            _ncccer = ncccer;
            _nccLang = nccLang;
            _nccQua = nccQua;
        }


        [HttpGet]
        public ActionResult StudentMaster(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult GetStudentList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _student.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             fullname = a.FullName,
                             chestno = a.ChestNo,
                             commno = a.CommisionNo,
                             Cour = a.CourseSeries,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower()) || x.chestno.ToLower().Contains(search.ToLower()) || x.commno.ToLower().Contains(search.ToLower()) || x.Cour.Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult AddStudent(string id, string Exception)
        {
            ViewBag.Exception = Exception;
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
            ViewBag.ForceList = new SelectList(_force.GetAll(), "pkid", "ForceName");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_StudentMaster model = _student.Get(_id);
                tbl_StudentMasterss abc = new tbl_StudentMasterss();
                abc.pkid = model.pkid;
                abc.Course_fkid = model.Course_fkid;
                abc.Division_fkid = model.Division_fkid;
                abc.Force_fkid = model.Force_fkid;
                abc.UserName = model.UserName;
                abc.Password = model.Password;
                abc.ConfirmedPassword = model.ConfirmedPassword;
                abc.CommisionNo = model.CommisionNo;
                abc.DateComm = model.DateComm;
                abc.Rank = model.Rank;
                abc.FullName = model.FullName;
                abc.DateOfBirth = model.DateOfBirth;
                abc.PlaceOfBirth = model.PlaceOfBirth;
                abc.IdentificationMarks = model.IdentificationMarks;
                abc.Height = model.Height;
                abc.Weight = model.Weight;
                abc.ColorOfEye = model.ColorOfEye;
                abc.ColorOfHair = model.ColorOfHair;
                abc.Religion = model.Religion;
                abc.Caste = model.Caste;
                abc.Nationality = model.Nationality;
                abc.UnitAndLocation = model.UnitAndLocation;
                abc.HQGroup = model.HQGroup;
                abc.Directorate = model.Directorate;
                abc.State = model.State;
                abc.CourseSeries = model.CourseSeries;
                abc.ChestNo = model.ChestNo;
                abc.DUCFrom = model.DUCFrom;
                abc.DUCTo = model.DUCTo;
                abc.ArrivalInNCCOTA = model.ArrivalInNCCOTA;
                abc.MotherToughe = model.MotherToughe;
                abc.PresentAddress = model.PresentAddress;
                abc.ParmanentAddress = model.ParmanentAddress;
                abc.MaritalStatus = model.MaritalStatus;
                abc.NOKFullName = model.NOKFullName;
                abc.NOKAddress = model.NOKAddress;
                abc.NOKRelation = model.NOKRelation;
                abc.TeachingInstitideName = model.TeachingInstitideName;
                abc.TeachingSubject = model.TeachingSubject;
                abc.TeachingDateOfEmplyment = model.TeachingDateOfEmplyment;
                abc.TeachingEmpStatus = model.TeachingEmpStatus;
                abc.NCCorOTUMember = model.NCCorOTUMember;
                abc.WhichDivision = model.WhichDivision;
                abc.WhichForce = model.WhichForce;
                abc.TrainingPeriod = model.TrainingPeriod;
                abc.RankofNCC = model.RankofNCC;
                abc.MedicallyExamined = model.MedicallyExamined;
                abc.MedicalCertificate = model.MedicalCertificate;
                abc.AlimentName = model.AlimentName;
                abc.MovementOrderNo = model.MovementOrderNo;
                abc.MovementDate = model.MovementDate;
                abc.Movement_copy = model.Movement_copy;
                abc.Games = model.Games;
                abc.OtherQualification = model.OtherQualification;
                abc.ReadNCCActRules = model.ReadNCCActRules;
                abc.ReadNCCorDG = model.ReadNCCorDG;
                abc.ReadSyllabus = model.ReadSyllabus;
                abc.ReadHandbook = model.ReadHandbook;
                abc.ReadNCCCompPlanning = model.ReadNCCCompPlanning;
                abc.ReadNCCCompInstruction = model.ReadNCCCompInstruction;
                abc.DateOfPreCourseTraining = model.DateOfPreCourseTraining;
                abc.BIODataForm = model.BIODataForm;
                abc.Place = model.Place;
                abc.AddedDate = DateTime.Now;
                return View(abc);
            }
            return View();
        }

        public ActionResult SaveStudent(string Name)
        {
            tbl_StudentMaster abc = new tbl_StudentMaster();
            abc.FullName = Name;
            _student.Add(abc);
            int id = _student.GetAll().Max(x => x.pkid);
            return Json(id, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddStudent(tbl_StudentMasterss model, HttpPostedFileBase Mcopy, HttpPostedFileBase BIODATA, HttpPostedFileBase Medicalcer)
        {
            WebFunction web = new WebFunction();
            string exception = "";
            try
            {
              
                if (model.pkid == 0)
                {
                    tbl_StudentMaster abc = new tbl_StudentMaster();
                    if (Mcopy != null)
                    {
                        abc.Movement_copy = web.Storefile(Mcopy, 2);
                    }
                    if (BIODATA != null)
                    {
                        abc.BIODataForm = web.Storefile(BIODATA, 2);
                    }
                    if (Medicalcer != null)
                    {
                        abc.MedicalCertificate = web.Storefile(Medicalcer, 2);
                    }
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.UserName = model.UserName;
                    abc.Password = model.Password;
                    abc.ConfirmedPassword = model.ConfirmedPassword;
                    abc.CommisionNo = model.CommisionNo;
                    abc.DateComm = model.DateComm;
                    abc.Rank = model.Rank;
                    abc.FullName = model.FullName;
                    abc.DateOfBirth = model.DateOfBirth;
                    abc.PlaceOfBirth = model.PlaceOfBirth;
                    abc.IdentificationMarks = model.IdentificationMarks;
                    abc.Height = model.Height;
                    abc.Weight = model.Weight;
                    abc.ColorOfEye = model.ColorOfEye;
                    abc.ColorOfHair = model.ColorOfHair;
                    abc.Religion = model.Religion;
                    abc.Caste = model.Caste;
                    abc.Nationality = model.Nationality;
                    abc.UnitAndLocation = model.UnitAndLocation;
                    abc.HQGroup = model.HQGroup;
                    abc.Directorate = model.Directorate;
                    abc.State = model.State;
                    abc.CourseSeries = model.CourseSeries;
                    abc.ChestNo = model.ChestNo;
                    abc.DUCFrom = model.DUCFrom;
                    abc.DUCTo = model.DUCTo;
                    abc.ArrivalInNCCOTA = model.ArrivalInNCCOTA;
                    abc.MotherToughe = model.MotherToughe;
                    abc.PresentAddress = model.PresentAddress;
                    abc.ParmanentAddress = model.ParmanentAddress;
                    abc.MaritalStatus = model.MaritalStatus;
                    abc.NOKFullName = model.NOKFullName;
                    abc.NOKAddress = model.NOKAddress;
                    abc.NOKRelation = model.NOKRelation;
                    abc.TeachingInstitideName = model.TeachingInstitideName;
                    abc.TeachingSubject = model.TeachingSubject;
                    abc.TeachingDateOfEmplyment = model.TeachingDateOfEmplyment;
                    abc.TeachingEmpStatus = model.TeachingEmpStatus;
                    abc.NCCorOTUMember = model.NCCorOTUMember;
                    abc.WhichDivision = model.WhichDivision;
                    abc.WhichForce = model.WhichForce;
                    abc.TrainingPeriod = model.TrainingPeriod;
                    abc.RankofNCC = model.RankofNCC;
                    abc.MedicallyExamined = model.MedicallyExamined;                  
                    abc.AlimentName = model.AlimentName;
                    abc.MovementOrderNo = model.MovementOrderNo;
                    abc.MovementDate = model.MovementDate;            
                    abc.Games = model.Games;
                    abc.OtherQualification = model.OtherQualification;
                    abc.ReadNCCActRules = model.ReadNCCActRules;
                    abc.ReadNCCorDG = model.ReadNCCorDG;
                    abc.ReadSyllabus = model.ReadSyllabus;
                    abc.ReadHandbook = model.ReadHandbook;
                    abc.ReadNCCCompPlanning = model.ReadNCCCompPlanning;
                    abc.ReadNCCCompInstruction = model.ReadNCCCompInstruction;
                    abc.DateOfPreCourseTraining = model.DateOfPreCourseTraining;                 
                    abc.Place = model.Place;
                    abc.AddedDate = DateTime.Now;                 
                    _student.Add(abc);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_StudentMaster abc = _student.Get(_id);
                    if (Mcopy != null)
                    {
                        if (model.Movement_copy != null)
                        {
                            web.DeleteImage(model.Movement_copy);
                        }

                        abc.Movement_copy = web.Storefile(Mcopy, 2);
                    }
                    if (BIODATA != null)
                    {
                        if (model.BIODataForm != null)
                        {
                            web.DeleteImage(model.BIODataForm);
                        }

                        abc.BIODataForm = web.Storefile(BIODATA, 2);
                    }
                    if (Medicalcer != null)
                    {
                        if (model.MedicalCertificate != null)
                        {
                            web.DeleteImage(model.MedicalCertificate);
                        }

                        abc.MedicalCertificate = web.Storefile(Medicalcer, 2);
                    }
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.UserName = model.UserName;
                    abc.Password = model.Password;
                    abc.ConfirmedPassword = model.ConfirmedPassword;
                    abc.CommisionNo = model.CommisionNo;
                    abc.DateComm = model.DateComm;
                    abc.Rank = model.Rank;
                    abc.FullName = model.FullName;
                    abc.DateOfBirth = model.DateOfBirth;
                    abc.PlaceOfBirth = model.PlaceOfBirth;
                    abc.IdentificationMarks = model.IdentificationMarks;
                    abc.Height = model.Height;
                    abc.Weight = model.Weight;
                    abc.ColorOfEye = model.ColorOfEye;
                    abc.ColorOfHair = model.ColorOfHair;
                    abc.Religion = model.Religion;
                    abc.Caste = model.Caste;
                    abc.Nationality = model.Nationality;
                    abc.UnitAndLocation = model.UnitAndLocation;
                    abc.HQGroup = model.HQGroup;
                    abc.Directorate = model.Directorate;
                    abc.State = model.State;
                    abc.CourseSeries = model.CourseSeries;
                    abc.ChestNo = model.ChestNo;
                    abc.DUCFrom = model.DUCFrom;
                    abc.DUCTo = model.DUCTo;
                    abc.ArrivalInNCCOTA = model.ArrivalInNCCOTA;
                    abc.MotherToughe = model.MotherToughe;
                    abc.PresentAddress = model.PresentAddress;
                    abc.ParmanentAddress = model.ParmanentAddress;
                    abc.MaritalStatus = model.MaritalStatus;
                    abc.NOKFullName = model.NOKFullName;
                    abc.NOKAddress = model.NOKAddress;
                    abc.NOKRelation = model.NOKRelation;
                    abc.TeachingInstitideName = model.TeachingInstitideName;
                    abc.TeachingSubject = model.TeachingSubject;
                    abc.TeachingDateOfEmplyment = model.TeachingDateOfEmplyment;
                    abc.TeachingEmpStatus = model.TeachingEmpStatus;
                    abc.NCCorOTUMember = model.NCCorOTUMember;
                    abc.WhichDivision = model.WhichDivision;
                    abc.WhichForce = model.WhichForce;
                    abc.TrainingPeriod = model.TrainingPeriod;
                    abc.RankofNCC = model.RankofNCC;
                    abc.MedicallyExamined = model.MedicallyExamined;
                    abc.AlimentName = model.AlimentName;
                    abc.MovementOrderNo = model.MovementOrderNo;
                    abc.MovementDate = model.MovementDate;
                    abc.Games = model.Games;
                    abc.OtherQualification = model.OtherQualification;
                    abc.ReadNCCActRules = model.ReadNCCActRules;
                    abc.ReadNCCorDG = model.ReadNCCorDG;
                    abc.ReadSyllabus = model.ReadSyllabus;
                    abc.ReadHandbook = model.ReadHandbook;
                    abc.ReadNCCCompPlanning = model.ReadNCCCompPlanning;
                    abc.ReadNCCCompInstruction = model.ReadNCCCompInstruction;
                    abc.DateOfPreCourseTraining = model.DateOfPreCourseTraining;
                    abc.Place = model.Place;
                    abc.AddedDate = DateTime.Now;     
                    _student.Update(abc);
                    exception = "Updated Successfully";
                }
                return RedirectToAction("StudentMaster", "Student", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentMaster", "Student", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult AddQualification(string id,string stud_id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Student_Qualification_Master model = _nccQua.Get(_id);
                tbl_Student_Qualification_Masterss abc = new tbl_Student_Qualification_Masterss();
                abc.pkid = model.pkid;
                abc.Student_fkid = model.Student_fkid;
                abc.CertificateEducation = model.CertificateEducation;
                abc.Subject = model.Subject;
                abc.Division = model.Division;
                abc.YearofPasswed = model.YearofPasswed;
                abc.Colleage = model.Colleage;
                abc.AddedDate = DateTime.Now;
                return PartialView(abc);
            }
            ViewBag.sid = stud_id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddQualification(tbl_Student_Qualification_Masterss model)
        {
            string exception = "";
            if (model.pkid == 0)
            {
                tbl_Student_Qualification_Master abc = new tbl_Student_Qualification_Master();
                abc.Student_fkid = model.Student_fkid;
                abc.CertificateEducation = model.CertificateEducation;
                abc.Subject = model.Subject;
                abc.Division = model.Division;
                abc.YearofPasswed = model.YearofPasswed;
                abc.Colleage = model.Colleage;
                abc.AddedDate = DateTime.Now;
                _nccQua.Add(abc);
                exception = "Save Successfully";
            }
            else
            {
                tbl_Student_Qualification_Master abc = _nccQua.Get(model.pkid);
                abc.Student_fkid = model.Student_fkid;
                abc.CertificateEducation = model.CertificateEducation;
                abc.Subject = model.Subject;
                abc.Division = model.Division;
                abc.YearofPasswed = model.YearofPasswed;
                abc.Colleage = model.Colleage;
                abc.AddedDate = DateTime.Now;
                _nccQua.Update(abc);
                exception = "Update Successfully";
            }
            return RedirectToAction("AddStudent", "Student", new { id = model.Student_fkid, Exception = exception });
        }

        [HttpGet]
        public ActionResult AddNCCCourse(string id, string stud_id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Student_NCC_CourseMaster model = _ncccourse.Get(_id);
                tbl_Student_NCC_CourseMasterss abc = new tbl_Student_NCC_CourseMasterss();
                abc.pkid = model.pkid;
                abc.Student_fkid = model.Student_fkid;
                abc.CourseName = model.CourseName;
                abc.TrainingInstitution = model.TrainingInstitution;
                abc.TotalDuration = model.TotalDuration;
                abc.DurationFromDate = model.DurationFromDate;
                abc.DurationToDate = model.DurationToDate;
                abc.Grading = model.Grading;
                abc.CheckRTU = model.CheckRTU;
                abc.RTUReason = model.RTUReason;      
                return PartialView(abc);
            }
            ViewBag.sid = stud_id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNCCCourse(tbl_Student_NCC_CourseMasterss model)
        {
            string exception = "";
            if (model.pkid == 0)
            {
                tbl_Student_NCC_CourseMaster abc = new tbl_Student_NCC_CourseMaster();
                abc.Student_fkid = model.Student_fkid;
                abc.CourseName = model.CourseName;
                abc.TrainingInstitution = model.TrainingInstitution;
                abc.TotalDuration = model.TotalDuration;
                abc.DurationFromDate = model.DurationFromDate;
                abc.DurationToDate = model.DurationToDate;
                abc.Grading = model.Grading;
                abc.CheckRTU = model.CheckRTU;
                abc.RTUReason = model.RTUReason;      
                _ncccourse.Add(abc);
                exception = "Save Successfully";
            }
            else
            {
                tbl_Student_NCC_CourseMaster abc = _ncccourse.Get(model.pkid);
                abc.Student_fkid = model.Student_fkid;
                abc.CourseName = model.CourseName;
                abc.TrainingInstitution = model.TrainingInstitution;
                abc.TotalDuration = model.TotalDuration;
                abc.DurationFromDate = model.DurationFromDate;
                abc.DurationToDate = model.DurationToDate;
                abc.Grading = model.Grading;
                abc.CheckRTU = model.CheckRTU;
                abc.RTUReason = model.RTUReason;
                _ncccourse.Update(abc);
                exception = "Update Successfully";
            }
            return RedirectToAction("AddStudent", "Student", new { id = model.Student_fkid, Exception = exception });
        }

        [HttpGet]
        public ActionResult AddNCCCertificate(string id, string stud_id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Student_NCCCertificateMaster model = _ncccer.Get(_id);
                tbl_Student_NCCCertificateMasterss abc = new tbl_Student_NCCCertificateMasterss();
                abc.pkid = model.pkid;
                abc.Student_fkid = model.Student_fkid;
                abc.Institution = model.Institution;
                abc.Year = model.Year;
                abc.Certificate_Obtained = model.Certificate_Obtained;
                abc.Type_campus = model.Type_campus;
                abc.AppointedYear = model.AppointedYear;
                abc.AttendedYear = model.AttendedYear;
                abc.LastModifieddate = DateTime.Now;
                return PartialView(abc);
            }
            ViewBag.sid = stud_id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNCCCertificate(tbl_Student_NCCCertificateMasterss model)
        {
            string exception = "";
            if (model.pkid == 0)
            {
                tbl_Student_NCCCertificateMaster abc = new tbl_Student_NCCCertificateMaster();
                abc.Student_fkid = model.Student_fkid;
                abc.Institution = model.Institution;
                abc.Year = model.Year;
                abc.Certificate_Obtained = model.Certificate_Obtained;
                abc.Type_campus = model.Type_campus;
                abc.AppointedYear = model.AppointedYear;
                abc.AttendedYear = model.AttendedYear;
                abc.LastModifieddate = DateTime.Now;
                _ncccer.Add(abc);
                exception = "Save Successfully";
            }
            else
            {
                tbl_Student_NCCCertificateMaster abc = _ncccer.Get(model.pkid);
                abc.Student_fkid = model.Student_fkid;
                abc.Institution = model.Institution;
                abc.Year = model.Year;
                abc.Certificate_Obtained = model.Certificate_Obtained;
                abc.Type_campus = model.Type_campus;
                abc.AppointedYear = model.AppointedYear;
                abc.AttendedYear = model.AttendedYear;
                abc.LastModifieddate = DateTime.Now;
                _ncccer.Update(abc);
                exception = "Update Successfully";
            }
            return RedirectToAction("AddStudent", "Student", new { id = model.Student_fkid, Exception = exception });
        }

        [HttpGet]
        public ActionResult AddNCCLanguage(string id, string stud_id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Student_Language_Master model = _nccLang.Get(_id);
                tbl_Student_Language_Masterss abc = new tbl_Student_Language_Masterss();
                abc.pkid = model.pkid;
                abc.Student_fkid = model.Student_fkid;
                abc.Language = model.Language;
                abc.ReadL = model.ReadL;
                abc.Write = model.Write;
                abc.Speak = model.Speak;
                abc.Upderstand = model.Upderstand;              
                return PartialView(abc);
            }
            ViewBag.sid = stud_id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNCCLanguage(tbl_Student_Language_Masterss model)
        {
            string exception = "";
            if (model.pkid == 0)
            {
                tbl_Student_Language_Master abc = new tbl_Student_Language_Master();
                abc.Student_fkid = model.Student_fkid;
                abc.Language = model.Language;
                abc.ReadL = model.ReadL;
                abc.Write = model.Write;
                abc.Speak = model.Speak;
                abc.Upderstand = model.Upderstand;
                _nccLang.Add(abc);
                exception = "Save Successfully";
            }
            else
            {
                tbl_Student_Language_Master abc = _nccLang.Get(model.pkid);
                abc.Student_fkid = model.Student_fkid;
                abc.Language = model.Language;
                abc.ReadL = model.ReadL;
                abc.Write = model.Write;
                abc.Speak = model.Speak;
                abc.Upderstand = model.Upderstand;
                _nccLang.Update(abc);
                exception = "Update Successfully";
            }
            return RedirectToAction("AddStudent", "Student", new { id = model.Student_fkid, Exception = exception });
        }

        public ActionResult CheckUserName(string Name)
        {
            try
            {
                int co = _student.GetAll().Where(x => x.UserName.Trim() == Name.Trim()).ToList().Count();
                if (co > 0)
                {
                    return Json("yes", JsonRequestBehavior.AllowGet);
                }
                return Json("no", JsonRequestBehavior.AllowGet);
            }
            catch { return Json("no", JsonRequestBehavior.AllowGet); }
        }
    }
}