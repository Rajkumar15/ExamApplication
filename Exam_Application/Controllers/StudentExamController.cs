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
    public class StudentExamController : Controller
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

        public StudentExamController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
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
        public ActionResult StudentLogin(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        [HttpPost]
        public ActionResult StudentLogin(tbl_StudentMasterss model)
        {
            string exception = "";
            try
            {
                var co = _student.GetAll().Where(x => x.UserName.ToLower() == model.UserName.ToLower() && x.Password == model.Password).FirstOrDefault();
                if (co != null)
                {
                    HttpCookie cookie = new HttpCookie("search");
                    cookie.Values["Stud"] = co.UserName;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);
                    exception = "Login Successfully";
                    return RedirectToAction("AttemptExam", "StudentExam", new { Exception = exception });
                }
                else
                {
                    exception = "User Not Available";
                    return RedirectToAction("StudentLogin", "StudentExam", new { Exception = exception });
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentLogin", "StudentExam", new { Exception = exception });
            }
        }


        [HttpGet]
        public ActionResult ForgetStudentLogin(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        [HttpPost]
        public ActionResult ForgetStudentLogin(tbl_StudentMasterss model)
        {
            string exception = "";
            try
            {
                int co = _student.GetAll().Where(x => x.UserName == model.UserName).ToList().Count();
                if (co > 0)
                {
                    tbl_StudentMaster data = _student.GetAll().Where(x => x.UserName == model.UserName).FirstOrDefault();
                    data.Password = "123456";
                    _student.Update(data);
                    exception = "Forgot Successfully";
                    return RedirectToAction("StudentLogin", "StudentExam", new { Exception = exception });
                }
                else
                {
                    exception = "User Not Available";
                    return RedirectToAction("ForgetStudentLogin", "StudentExam", new { Exception = exception });
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("ForgetStudentLogin", "StudentExam", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult AttemptExam(string id, string Exception)
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
        public ActionResult ReadInstruction(int id, string Exception)
        {
            tbl_Exam_Master data = _Exam.Get(id);
            tbl_Exam_Masterss abc = new tbl_Exam_Masterss();
            abc.pkid = id;
            abc.Instruction = data.Instruction;
            ViewBag.Exception = Exception;
            return View(abc);
        }

        [HttpGet]
        public ActionResult StudentDashboard(int Examid, string Exception)
        {
            string exception = "";
            ViewBag.Exception = Exception;
            int NO = 1;
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    MainExamModel Exammodel = new MainExamModel();
                    List<QuestionIndexer> QuesList = new List<QuestionIndexer>();
                    tbl_Exam_Master Exam = _Exam.Get(Examid);
                    Exammodel.Examid = Examid;
                    string userName = cookie.Values[0];
                    Exammodel.Studentid = _student.GetAll().Where(x => x.UserName == userName).FirstOrDefault().pkid;
                    List<tbl_Exam_QuestionsMaster> ExamQuestion = _EQUESTION.GetAll().Where(x => x.Exam_fkid == Examid).ToList();
                    foreach (var item in ExamQuestion)
                    {
                        tbl_QuestionMaster Q = _question.Get(item.Question_fkid);
                        if (Q.Status == 1)
                        {
                            QuestionIndexer QUQU = new QuestionIndexer();
                            QUQU.QueNo = NO;
                            QUQU.Questionid = Q.pkid;
                            QUQU.Quest_type = Convert.ToInt32(Q.subjecttype_fkid);
                            QUQU.ExamDuration = Exam.ExamDuration;
                            QUQU.ExamStartTime = DateTime.Now;                           
                            QuesList.Add(QUQU);
                        }
                        NO++;
                    }

                    Exammodel.QueIndexer = QuesList.OrderBy(x => x.Quest_type).ToList();
                    return View(Exammodel);
                }
                else
                {
                    exception = "Session Expired.";
                    return RedirectToAction("StudentLogin", "StudentExam", new { Exception = exception });
                }
            }

            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentLogin", "StudentExam", new { Exception = exception });
            }

        }

        public ActionResult StudentLogout()
        {
            try
            {

                HttpCookie currentUserCookie = Request.Cookies["search"];
                Response.Cookies.Remove("search");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
                return View("StudentLogin");
            }
            catch
            {
                return View("StudentLogin");
            }
        }

      
        public PartialViewResult ObjectiveQuestion(int QuestionId,int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    ObjectiveQuestions Question = new ObjectiveQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.QueId = QuestionId;
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 1)
                        {
                            tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                            Question.Answer1 = ANS.Answer1;
                            Question.Answer2 = ANS.Answer2;
                            Question.Answer3 = ANS.Answer3;
                            Question.Answer4 = ANS.Answer4;
                            Question.CorrectAnswerDD = ANS.CorrectAnswer;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";

                }               
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        public PartialViewResult TrueFalseQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    ObjectiveQuestions Question = new ObjectiveQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.QueId = QuestionId;
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 2)
                        {
                            tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                            Question.TrueFalse = ANS.TrueFalse;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";
                }
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        public PartialViewResult FillInBlanckQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    ObjectiveQuestions Question = new ObjectiveQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.QueId = QuestionId;
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 3)
                        {
                            tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                            Question.BlanckSpace = ANS.BlanckSpace;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";

                }
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        public PartialViewResult MatchContentQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    MatchContentQuestions Question = new MatchContentQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 5)
                        {
                            List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid ).ToList();
                            Question.Match = ANS;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";
                }
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        public PartialViewResult IdentifySignQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    ObjectiveQuestions Question = new ObjectiveQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 6)
                        {
                            tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                            Question.Answer1 = ANS.Answer1;
                            Question.Answer2 = ANS.Answer2;
                            Question.Answer3 = ANS.Answer3;
                            Question.Answer4 = ANS.Answer4;
                            Question.CorrectAnswerDD = ANS.CorrectAnswer;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";

                }
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        public PartialViewResult FullFormQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    MatchContentQuestions Question = new MatchContentQuestions();
                    tbl_QuestionMaster Q = _question.Get(QuestionId);
                    if (Q.Status == 1)
                    {
                        Question.Question = Q.Question;
                        Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                        Question.QuesMarks = Q.Marks;
                        Question.Queno = QuestionNo;
                        if (Q.subjecttype_fkid == 7)
                        {
                            List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid).ToList();
                            Question.Match = ANS;
                        }
                    }
                    return PartialView(Question);
                }
                else
                {
                    exception = "Session Expired.";

                }
                return PartialView();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }
    }
}