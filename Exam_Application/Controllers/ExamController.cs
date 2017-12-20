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
    public class ExamController : Controller
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

        public ExamController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
            IRepository<tbl_DivisionMaster> division, IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer, IRepository<tbl_MatchContentQuestionMaster> Matc, IRepository<tbl_ForceMaster> force,
            IRepository<tbl_Student_NCC_CourseMaster> ncccourse, IRepository<tbl_Student_NCCCertificateMaster> ncccer, IRepository<tbl_Student_Qualification_Master> nccQua,
            IRepository<tbl_Student_Language_Master> nccLang, IRepository<tbl_Exam_Master> Exam, IRepository<tbl_Exam_QuestionsMaster> EQUESTION, IRepository<tbl_Student_AnswerSheet> QANSWER)
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
            _Exam = Exam;
            _EQUESTION = EQUESTION;
            _WQANSWER = QANSWER;
        }

        [HttpGet]
        public ActionResult ExamMaster(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult GetExamList()
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
                var v = (from a in _Exam.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             fullname = a.Exam_Name,
                             passper = a.Total_Marks,
                             examduration = a.ExamDuration,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult AddExam(string id, string Exception)
        {
            ViewBag.Exception = Exception;
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
            ViewBag.ForceList = new SelectList(_force.GetAll(), "pkid", "ForceName");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Exam_Master model = _Exam.Get(_id);
                tbl_Exam_Masterss abc = new tbl_Exam_Masterss();
                abc.pkid = model.pkid;
                abc.Exam_Name = model.Exam_Name;
                abc.Total_Marks = model.Total_Marks;
                abc.PassingMarks = model.PassingMarks;
                abc.ExamDuration = model.ExamDuration;
                abc.Passing_Percentage = model.Passing_Percentage;
                abc.Instruction = model.Instruction;
                abc.AttemptCount = model.AttemptCount;
                abc.StartDate = model.StartDate;
                abc.EndDate = model.EndDate;
                abc.DeclareResult = model.DeclareResult;
                abc.Negative_Masrking = model.Negative_Masrking;
                abc.RandonQuestion = model.RandonQuestion;
                abc.ResultAfterFinish = model.ResultAfterFinish;
                abc.Course_fkid = model.Course_fkid;
                abc.Division_fkid = model.Division_fkid;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddExam(tbl_Exam_Masterss model)
        {
            string exception = "";
            int Exam_id = 0;
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Exam_Master abc = new tbl_Exam_Master();
                    abc.Exam_Name = model.Exam_Name;
                    abc.ExamDuration = model.ExamDuration;
                    abc.Passing_Percentage = model.Passing_Percentage;
                    abc.Total_Marks = model.Total_Marks;
                    abc.PassingMarks = model.PassingMarks;
                    abc.Instruction = model.Instruction;
                    abc.AttemptCount = model.AttemptCount;
                    abc.StartDate = model.StartDate;
                    abc.EndDate = model.EndDate;
                    abc.DeclareResult = model.DeclareResult;
                    abc.Negative_Masrking = model.Negative_Masrking;
                    abc.RandonQuestion = model.RandonQuestion;
                    abc.ResultAfterFinish = model.ResultAfterFinish;
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Adddate = DateTime.Now;
                    _Exam.Add(abc);
                    Exam_id = _Exam.GetAll().Max(x => x.pkid);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Exam_Master abc = _Exam.Get(_id);
                    abc.Exam_Name = model.Exam_Name;
                    abc.ExamDuration = model.ExamDuration;
                    abc.Passing_Percentage = model.Passing_Percentage;
                    abc.Total_Marks = model.Total_Marks;
                    abc.PassingMarks = model.PassingMarks;
                    abc.Instruction = model.Instruction;
                    abc.AttemptCount = model.AttemptCount;
                    abc.StartDate = model.StartDate;
                    abc.EndDate = model.EndDate;
                    abc.DeclareResult = model.DeclareResult;
                    abc.Negative_Masrking = model.Negative_Masrking;
                    abc.RandonQuestion = model.RandonQuestion;
                    abc.ResultAfterFinish = model.ResultAfterFinish;
                    abc.Course_fkid = model.Course_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Adddate = DateTime.Now;
                    _Exam.Update(abc);
                    exception = "Update Successfully";
                    Exam_id = _id;
                }
                if (model.RandonQuestion == false)
                {
                    return RedirectToAction("AddQuestionExam", "Exam", new { id = Exam_id, Exception = exception });
                }
                else
                { return RedirectToAction("AddAutomaticQuestionExam", "Exam", new { id = Exam_id, Exception = exception }); }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("ExamMaster", "Exam", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult AddQuestionExam(int id, string Exception)
        {
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
            var exam = _Exam.Get(id);
            ViewBag.Examid = exam.pkid;
            TempData["Examid"] = exam.pkid;
            TempData.Keep("Examid");
            ViewBag.totalmarks = exam.Total_Marks;
            ViewBag.getma = (from a in _EQUESTION.GetAll().Where(x => x.Exam_fkid == exam.pkid).ToList()
                             select new
                             {
                                 mar = _question.Get(a.Question_fkid).Marks,
                             }).Sum(x => x.mar);
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult GetQuestionList()
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
            int ExamId = Convert.ToInt32(TempData["Examid"].ToString());
            TempData.Keep("Examid");         
            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _question.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             name = a.Question,
                             marks = a.Marks,
                             st = _EQUESTION.GetAll().Where(x => x.Exam_fkid == ExamId && x.Question_fkid == a.pkid).Count(),
                             Nmar = a.NegativeMarks
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower())) select b);
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

        public JsonResult SaveQuestion(int eid, int qid)
        {
            try
            {
                tbl_Exam_QuestionsMaster abc = new tbl_Exam_QuestionsMaster();
                abc.Exam_fkid = eid;
                abc.Question_fkid = qid;
                _EQUESTION.Add(abc);
                return Json("succees", JsonRequestBehavior.AllowGet);
            }
            catch { return Json("failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult DeleteQuestion(int eid, int qid)
        {
            try
            {
                tbl_Exam_QuestionsMaster abc = _EQUESTION.GetAll().Where(x => x.Exam_fkid == eid && x.Question_fkid == qid).FirstOrDefault();
                _EQUESTION.Remove(abc, true);
                return Json("succees", JsonRequestBehavior.AllowGet);
            }
            catch { return Json("failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public ActionResult ShowQuestion(int id)
        {
            try
            {
                QuestionMaster abc = new QuestionMaster();
                tbl_QuestionMaster QUE = _question.Get(id);
                abc.subjecttype_fkid = QUE.subjecttype_fkid;
                abc.Question = QUE.Question;
                abc.Marks = QUE.Marks;
                abc.NegativeMarks = QUE.NegativeMarks;
                if (abc.subjecttype_fkid == 5)
                {
                    abc.MATContent = _Matc.GetAll().Where(x => x.Ques_fkid == id).ToList();
                }
                else if (abc.subjecttype_fkid == 7) 
                {
                    abc.FULLF = _Matc.GetAll().Where(x => x.Ques_fkid == id).ToList();
                }
                else
                {
                    tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == QUE.pkid).FirstOrDefault();
                    abc.Answer1 = ANS.Answer1;
                    abc.Answer2 = ANS.Answer2;
                    abc.Answer3 = ANS.Answer3;
                    abc.Answer4 = ANS.Answer4;
                    abc.CorrectAnswerDD = ANS.CorrectAnswer;
                    abc.BlanckSpace = ANS.BlanckSpace;
                    abc.TrueFalse = ANS.TrueFalse;

                }
               
                return PartialView(abc);
            }
            catch {
                return PartialView();
            }
        }
    }
}