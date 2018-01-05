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
    public class ArrivalSchemaController : Controller
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
        public readonly IRepository<tbl_Exam_Master> _Exam;
        public readonly IRepository<tbl_Exam_QuestionsMaster> _EQUESTION;
        public readonly IRepository<tbl_Student_AnswerSheet> _WQANSWER;
        public readonly IRepository<tbl_AttemptExamAnswerSheet> _Attemptexam;
        public readonly IRepository<tbl_StudentExamResult> _examresult;
        public readonly IRepository<tbl_Session_Master> _sessionmaster;
        public readonly IRepository<tbl_REF_Arrival_DataMaster> _REFMaster;
        public readonly IRepository<tbl_PIOC_Arrival_DataMaster> _PIOCMaster;


        public ArrivalSchemaController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
            IRepository<tbl_DivisionMaster> division, IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer, IRepository<tbl_MatchContentQuestionMaster> Matc, IRepository<tbl_ForceMaster> force,
            IRepository<tbl_Student_NCC_CourseMaster> ncccourse, IRepository<tbl_Student_NCCCertificateMaster> ncccer, IRepository<tbl_Student_Qualification_Master> nccQua,
            IRepository<tbl_Student_Language_Master> nccLang, IRepository<tbl_Exam_Master> Exam, IRepository<tbl_Exam_QuestionsMaster> EQUESTION, IRepository<tbl_Student_AnswerSheet> QANSWER,
            IRepository<tbl_AttemptExamAnswerSheet> Attemptexam, IRepository<tbl_StudentExamResult> examresult, IRepository<tbl_Session_Master> sessionmaster,
            IRepository<tbl_REF_Arrival_DataMaster> REFMaster, IRepository<tbl_PIOC_Arrival_DataMaster> PIOCMaster)
        {
            _user = user;
            _group = group;
            _course = course;
            _division = division;
            _subtype = subtype;
            _Attemptexam = Attemptexam;
            _student = student;
            _question = question;
            _answer = answer;
            _Matc = Matc;
            _force = force;
            _ncccourse = ncccourse;
            _ncccer = ncccer;
            _nccLang = nccLang;
            _nccQua = nccQua;
            _Exam = Exam;
            _EQUESTION = EQUESTION;
            _WQANSWER = QANSWER;
            _examresult = examresult;
            _sessionmaster = sessionmaster;
            _REFMaster = REFMaster;
            _PIOCMaster = PIOCMaster;
        }
        // GET: ArrivalSchema

        WebFunction web = new WebFunction();

        [HttpGet]
        public ActionResult ArrivalDataList()
        {
            Session["Menu"] = 6.ToString();
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            return View();
        }

        public ActionResult GetCourseListList(int id)
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
                var v = (from a in _REFMaster.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             course = (a.Course_fkid != null ? _course.Get(a.Course_fkid).CourseName : ""),
                             divsion = (a.Division_fkid != null ? _division.Get(a.Division_fkid).DivisioName : ""),
                             studen = (a.Student_fkid != null ? _student.Get(a.Student_fkid).FullName : ""),
                             add = a.AddedDate,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.course.ToLower().Contains(search.ToLower()) || x.divsion.ToLower().Contains(search.ToLower()) || x.studen.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult REFMaster(string id)
        {
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
            ViewBag.ForceList = new SelectList(_force.GetAll(), "pkid", "ForceName");
            ViewBag.StudentList = new SelectList(_student.GetAll(), "pkid", "FullName");
            ViewBag.SessionList = new SelectList(_sessionmaster.GetAll(), "pkid", "OtherData");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_REF_Arrival_DataMaster model = _REFMaster.Get(_id);
                tbl_REF_Arrival_DataMasterss abc = new tbl_REF_Arrival_DataMasterss();
                abc.pkid = model.pkid;
                abc.Course_fkid = model.Course_fkid;
                abc.Division_fkid = model.Division_fkid;
                abc.Force_fkid = model.Force_fkid;
                abc.Session_fkid = model.Session_fkid;
                abc.Student_fkid = model.Student_fkid;
                abc.ChestNo = model.ChestNo;
                abc.NCCNo = model.NCCNo;
                abc.Rank = model.Rank;
                abc.NCCunit = model.NCCunit;
                abc.GPHQ = model.GPHQ;
                abc.DTE = model.DTE;
                abc.MoveOrder = model.MoveOrder;
                abc.NominalRoll = model.NominalRoll;
                abc.IdentemnityBond = model.IdentemnityBond;
                abc.DetailsService = model.DetailsService;
                abc.SignOfIndivisual = model.SignOfIndivisual;
                abc.MedicalCertificate = model.MedicalCertificate;
                abc.Copyofpreviouscertific = model.Copyofpreviouscertific;
                abc.RiskCertificate = model.RiskCertificate;
                abc.AddedDate = model.AddedDate;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult REFMaster(tbl_REF_Arrival_DataMasterss model, HttpPostedFileBase MED, HttpPostedFileBase RIS, HttpPostedFileBase SERCER, HttpPostedFileBase SIG)
        {
            try
            {
                if (model.pkid == 0)
                {
                    tbl_REF_Arrival_DataMaster abc = new tbl_REF_Arrival_DataMaster();
                    if (MED != null)
                    {
                        abc.MedicalCertificate = web.Storefile(MED, 3);
                    }
                    if (RIS != null)
                    {
                        abc.RiskCertificate = web.Storefile(RIS, 3);
                    }
                    abc.pkid = model.pkid;
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.Session_fkid = model.Session_fkid;
                    abc.Student_fkid = model.Student_fkid;
                    abc.ChestNo = model.ChestNo;
                    abc.NCCNo = model.NCCNo;
                    abc.Rank = model.Rank;
                    abc.NCCunit = model.NCCunit;
                    abc.GPHQ = model.GPHQ;
                    abc.DTE = model.DTE;
                    abc.MoveOrder = model.MoveOrder;
                    abc.NominalRoll = model.NominalRoll;
                    abc.IdentemnityBond = model.IdentemnityBond;
                    abc.DetailsService = model.DetailsService;
                    abc.AddedDate = DateTime.Now;
                    abc.LastModifiedDate = DateTime.Now;
                    if (SERCER != null)
                    {
                        abc.MedicalCertificate = web.Storefile(MED, 3);
                    }
                    if (SIG != null)
                    {
                        abc.SignOfIndivisual = web.Storefile(SIG, 3);
                    }
                    _REFMaster.Add(abc);
                }
                else
                {
                    tbl_REF_Arrival_DataMaster abc = _REFMaster.Get(model.pkid);
                    if (MED != null)
                    {
                        if (model.MedicalCertificate != null)
                        {
                            web.DeleteImage(model.MedicalCertificate);
                        }
                        abc.MedicalCertificate = web.Storefile(MED, 3);
                    }
                    if (RIS != null)
                    {
                        if (model.RiskCertificate != null)
                        {
                            web.DeleteImage(model.RiskCertificate);
                        }
                        abc.RiskCertificate = web.Storefile(RIS, 3);
                    }
                    if (SERCER != null)
                    {
                        if (model.Copyofpreviouscertific != null)
                        {
                            web.DeleteImage(model.Copyofpreviouscertific);
                        }
                        abc.Copyofpreviouscertific = web.Storefile(SERCER, 3);
                    }
                    if (SIG != null)
                    {
                        if (model.SignOfIndivisual != null)
                        {
                            web.DeleteImage(model.SignOfIndivisual);
                        }
                        abc.SignOfIndivisual = web.Storefile(SIG, 3);
                    }
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.Session_fkid = model.Session_fkid;
                    abc.Student_fkid = model.Student_fkid;
                    abc.ChestNo = model.ChestNo;
                    abc.NCCNo = model.NCCNo;
                    abc.Rank = model.Rank;
                    abc.NCCunit = model.NCCunit;
                    abc.GPHQ = model.GPHQ;
                    abc.DTE = model.DTE;
                    abc.MoveOrder = model.MoveOrder;
                    abc.NominalRoll = model.NominalRoll;
                    abc.IdentemnityBond = model.IdentemnityBond;
                    abc.DetailsService = model.DetailsService;
                    abc.AddedDate = model.AddedDate;
                    abc.LastModifiedDate = DateTime.Now;
                    _REFMaster.Update(abc);
                }
                return RedirectToAction("REFMaster", "ArrivalSchema");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                return RedirectToAction("REFMaster", "ArrivalSchema");
            }

        }

        [HttpGet]
        public ActionResult PIOCMAsterList()
        {
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            return View();
        }

        public ActionResult GetPIOCListList(int id)
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
                var v = (from a in _PIOCMaster.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             course = (a.Course_fkid != null ? _course.Get(a.Course_fkid).CourseName : ""),
                             divsion = (a.Division_fkid != null ? _division.Get(a.Division_fkid).DivisioName : ""),
                             studen = (a.Student_fkid != null ? _student.Get(a.Student_fkid).FullName : ""),
                             add = a.AddedDate,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.course.ToLower().Contains(search.ToLower()) || x.divsion.ToLower().Contains(search.ToLower()) || x.studen.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult PIOCMaster(string id)
        {
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
            ViewBag.ForceList = new SelectList(_force.GetAll(), "pkid", "ForceName");
            ViewBag.StudentList = new SelectList(_student.GetAll(), "pkid", "FullName");
            ViewBag.SessionList = new SelectList(_sessionmaster.GetAll(), "pkid", "OtherData");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_PIOC_Arrival_DataMasterss abc = new tbl_PIOC_Arrival_DataMasterss();
                tbl_PIOC_Arrival_DataMaster model = _PIOCMaster.Get(_id);
                abc.pkid = model.pkid;
                abc.Course_fkid = model.Course_fkid;
                abc.Division_fkid = model.Division_fkid;
                abc.Force_fkid = model.Force_fkid;
                abc.Session_fkid = model.Session_fkid;
                abc.Student_fkid = model.Student_fkid;
                abc.ChestNo = model.ChestNo;
                abc.ArmyNo = model.ArmyNo;
                abc.Rank = model.Rank;
                abc.ParentUnit = model.ParentUnit;
                abc.Records = model.Records;
                abc.NCCUnit = model.NCCUnit;
                abc.DateOfArrival = model.DateOfArrival;
                abc.DTE = model.DTE;
                abc.DateofDEP = model.DateofDEP;
                abc.MoveOrder = model.MoveOrder;
                abc.IdentityCard = model.IdentityCard;
                abc.MobileNo = model.MobileNo;
                abc.IMEIno = model.IMEIno;
                abc.AddedDate = model.AddedDate;
                abc.MedicalCertificate = model.MedicalCertificate;
                abc.SignofIndividual = model.SignofIndividual;
                abc.CharacterCertificate = model.CharacterCertificate;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult PIOCMaster(tbl_PIOC_Arrival_DataMasterss model,HttpPostedFileBase MED, HttpPostedFileBase CHR,HttpPostedFileBase SIG)
        {
            try
            {
                if (model.pkid == 0)
                {
                    tbl_PIOC_Arrival_DataMaster abc = new tbl_PIOC_Arrival_DataMaster();
                    if (MED != null)
                    {
                        abc.MedicalCertificate = web.Storefile(MED, 3);
                    }
                    if (CHR != null)
                    {
                        abc.CharacterCertificate = web.Storefile(CHR, 3);
                    }
                    if (SIG != null)
                    {
                        abc.SignofIndividual = web.Storefile(SIG, 3);
                    }
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.Session_fkid = model.Session_fkid;
                    abc.Student_fkid = model.Student_fkid;
                    abc.ChestNo = model.ChestNo;
                    abc.ArmyNo = model.ArmyNo;
                    abc.Rank = model.Rank;
                    abc.ParentUnit = model.ParentUnit;
                    abc.Records = model.Records;
                    abc.NCCUnit = model.NCCUnit;
                    abc.DateOfArrival = model.DateOfArrival;
                    abc.DTE = model.DTE;
                    abc.DateofDEP = model.DateofDEP;
                    abc.MoveOrder = model.MoveOrder;
                    abc.IdentityCard = model.IdentityCard;
                    abc.MobileNo = model.MobileNo;
                    abc.IMEIno = model.IMEIno;
                    abc.AddedDate = DateTime.Now;
                    abc.LastModifiedDate = DateTime.Now;
                    _PIOCMaster.Add(abc);
                }
                else {
                    tbl_PIOC_Arrival_DataMaster abc = _PIOCMaster.Get(model.pkid);
                    if (CHR != null)
                    {
                        if (model.CharacterCertificate != null)
                        {
                            web.DeleteImage(model.CharacterCertificate);
                        }
                        abc.CharacterCertificate = web.Storefile(CHR, 3);
                    }
                    if (MED != null)
                    {
                        if (model.MedicalCertificate != null)
                        {
                            web.DeleteImage(model.MedicalCertificate);
                        }
                        abc.MedicalCertificate = web.Storefile(MED, 3);
                    }
                    if (SIG != null)
                    {
                        if (model.SignofIndividual != null)
                        {
                            web.DeleteImage(model.SignofIndividual);
                        }
                        abc.SignofIndividual = web.Storefile(SIG, 3);
                    }
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Force_fkid = model.Force_fkid;
                    abc.Session_fkid = model.Session_fkid;
                    abc.Student_fkid = model.Student_fkid;
                    abc.ChestNo = model.ChestNo;
                    abc.ArmyNo = model.ArmyNo;
                    abc.Rank = model.Rank;
                    abc.ParentUnit = model.ParentUnit;
                    abc.Records = model.Records;
                    abc.NCCUnit = model.NCCUnit;
                    abc.DateOfArrival = model.DateOfArrival;
                    abc.DTE = model.DTE;
                    abc.DateofDEP = model.DateofDEP;
                    abc.MoveOrder = model.MoveOrder;
                    abc.IdentityCard = model.IdentityCard;
                    abc.MobileNo = model.MobileNo;
                    abc.IMEIno = model.IMEIno;
                    abc.AddedDate = model.AddedDate;
                    abc.LastModifiedDate = DateTime.Now;
                    _PIOCMaster.Update(abc);
                }
                return RedirectToAction("PIOCMaster", "ArrivalSchema");
            }             
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                return RedirectToAction("PIOCMaster", "ArrivalSchema");
            }
        }
    }
}