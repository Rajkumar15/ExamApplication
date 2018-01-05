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
        public readonly IRepository<tbl_AttemptExamAnswerSheet> _Attemptexam;
        public readonly IRepository<tbl_StudentExamResult> _examresult;

        public StudentExamController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
            IRepository<tbl_DivisionMaster> division, IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer, IRepository<tbl_MatchContentQuestionMaster> Matc, IRepository<tbl_ForceMaster> force,
            IRepository<tbl_Student_NCC_CourseMaster> ncccourse, IRepository<tbl_Student_NCCCertificateMaster> ncccer, IRepository<tbl_Student_Qualification_Master> nccQua,
            IRepository<tbl_Student_Language_Master> nccLang, IRepository<tbl_Exam_Master> Exam, IRepository<tbl_Exam_QuestionsMaster> EQUESTION, IRepository<tbl_Student_AnswerSheet> QANSWER,
            IRepository<tbl_AttemptExamAnswerSheet> Attemptexam, IRepository<tbl_StudentExamResult> examresult)
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

        public ActionResult SetCookiesforTiming(int eid)
        {
            try
            {
                tbl_Exam_Master abc = _Exam.Get(eid);
                HttpCookie cookie = new HttpCookie("exam");
                HttpCookie cookieS = new HttpCookie("Startt");
                HttpCookie TIMER = new HttpCookie("timer");
                cookie.Values["ExamDuration"] = abc.ExamDuration;
                cookie.Expires = DateTime.Now.AddDays(1);
                cookieS.Values["StartTime"] = DateTime.Now.ToString();
                if (abc.ExamDuration != null)
                {
                    var timr = abc.ExamDuration.Split('.');
                    DateTime expirytime = DateTime.Now.Add(new TimeSpan(Convert.ToInt32(timr[0]), Convert.ToInt32(timr[1]), 1));
                    TIMER.Values["timer"] = expirytime.ToString();
                    Response.Cookies.Add(TIMER);
                }
                cookieS.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
                Response.Cookies.Add(cookieS);
                return Json("s", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("StudentLogin", "StudentExam", new { Exception = "Session Expired." });
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
            try
            {

                HttpCookie cookie = Request.Cookies["search"];
                string userName = cookie.Values[0];
                int studid = _student.GetAll().Where(x => x.UserName == userName).FirstOrDefault().pkid;

                var datafds = _examresult.GetAll().Where(x => x.Student_fkid == studid && x.Exam_fkid == id).ToList();
                if (datafds.Count() == 0)
                {
                    tbl_Exam_Master data = _Exam.Get(id);
                    tbl_Exam_Masterss abc = new tbl_Exam_Masterss();
                    abc.pkid = id;
                    abc.Instruction = data.Instruction;
                    ViewBag.Exception = Exception;
                    return View(abc);
                }
                else
                {
                    return RedirectToAction("AttemptExam", "StudentExam", new { Exception = "You Have Already Submitted These Exam." });
                }
            }
            catch
            {
                return RedirectToAction("StudentLogin", "StudentExam", new { Exception = "Session Expired." });
            }
        }

        [HttpGet]
        public ActionResult StudentDashboard(int Examid, string Exception, string firs)
        {
            string exception = "";
            if (firs == "1")
            {
                ViewBag.fi = "sds";
            }
            ViewBag.Exception = Exception;
            int NO = 1;
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                HttpCookie cookieRaj = Request.Cookies["exam"];
                HttpCookie cookieRajTime = Request.Cookies["Startt"];
                HttpCookie TIMER = Request.Cookies["timer"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    MainExamModel Exammodel = new MainExamModel();
                    List<QuestionIndexer> QuesList = new List<QuestionIndexer>();
                    TimeSpan t = Convert.ToDateTime(TIMER.Values[0]) - DateTime.Now;
                    int hour = t.Hours; int min = t.Minutes; int Sec = t.Seconds;
                    Exammodel.ExamDuration = hour + "," + min + "," + Sec;
                    tbl_Exam_Master Exam = _Exam.Get(Examid);
                    Exammodel.Examid = Examid;
                    TempData["Eid"] = Examid;
                    string userName = cookie.Values[0];
                    ViewBag.duration = cookieRaj.Values[0];
                    Exammodel.ExamStartDatetime = Convert.ToDateTime(cookieRajTime.Values[0]);
                    Exammodel.Studentid = _student.GetAll().Where(x => x.UserName == userName).FirstOrDefault().pkid;
                    TempData["Sid"] = Exammodel.Studentid;
                    TimeSpan p = DateTime.Now - DateTime.Now;
                    if (p > t)
                    {
                        return RedirectToAction("SubmitExamServer", "StudentExam", new { eid = Examid, sid = Exammodel.Studentid });
                    }
                    //  List<tbl_Exam_QuestionsMaster> ExamQuestion = _EQUESTION.GetAll().Where(x => x.Exam_fkid == Examid).ToList();
                    List<tbl_Exam_QuestionsMasterRAJ> ExamQuestion = (from a in _EQUESTION.GetAll().Where(x => x.Exam_fkid == Examid)
                                                                      select new tbl_Exam_QuestionsMasterRAJ
                                                                      {
                                                                          pkid = a.pkid,
                                                                          Exam_fkid = a.Exam_fkid,
                                                                          Question_fkid = a.Question_fkid,
                                                                          type = _question.Get(a.Question_fkid).subjecttype_fkid
                                                                      }).ToList();
                    foreach (var item in ExamQuestion.OrderBy(x => x.type))
                    {
                        tbl_QuestionMaster Q = _question.Get(item.Question_fkid);
                        if (Q.Status == 1)
                        {
                            QuestionIndexer QUQU = new QuestionIndexer();
                            QUQU.QueNo = NO;
                            QUQU.Questionid = Q.pkid;
                            QUQU.Quest_type = Convert.ToInt32(Q.subjecttype_fkid);
                            // QUQU.ExamDuration = Exam.ExamDuration;
                            //  QUQU.ExamStartTime = DateTime.Now;
                            QuesList.Add(QUQU);
                        }
                        NO++;
                    }
                    Exammodel.QueIndexer = QuesList.ToList();
                    int qu_id = 1; int Qno = 1;
                    try
                    {
                        qu_id = Convert.ToInt32(TempData.Peek("Qid").ToString());
                        Qno = Convert.ToInt32(TempData.Peek("Qno").ToString());
                        Qno = Qno + 1;
                    }
                    catch { }
                    if (QuesList.Count() < Qno)
                    {
                        Qno = Qno - 1;
                        Exammodel.Ques_fkid = QuesList.Where(x => x.QueNo == Qno).FirstOrDefault().Questionid;
                        Exammodel.Qnono = Qno;
                    }
                    else
                    {
                        Exammodel.Ques_fkid = QuesList.Where(x => x.QueNo == Qno).FirstOrDefault().Questionid;
                        Exammodel.Qnono = Qno;
                    }
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
                HttpCookie currentUserCookiee = Request.Cookies["exam"];
                Response.Cookies.Remove("exam");
                HttpCookie currentUserCookieT = Request.Cookies["Startt"];
                Response.Cookies.Remove("Startt");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookiee.Expires = DateTime.Now.AddDays(-10);
                currentUserCookieT.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                currentUserCookiee.Value = null;
                currentUserCookieT.Value = null;
                Response.SetCookie(currentUserCookie); Response.SetCookie(currentUserCookiee); Response.SetCookie(currentUserCookieT);
                return View("StudentLogin");
            }
            catch
            {
                return View("StudentLogin");
            }
        }

        [HttpGet]
        public ActionResult ObjectiveQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 2)
                    {
                        return RedirectToAction("TrueFalseQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 3)
                    {
                        return RedirectToAction("FillInBlanckQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 5)
                    {
                        return RedirectToAction("MatchContentQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 6)
                    {
                        return RedirectToAction("IdentifySignQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 7)
                    {
                        return RedirectToAction("FullFormQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }

                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).FirstOrDefault();
                    if (data != null)
                    {
                        tbl_AttemptExamAnswerSheetss abc = new tbl_AttemptExamAnswerSheetss();
                        tbl_AttemptExamAnswerSheet model = _Attemptexam.Get(data.pkid);
                        abc.QueId = model.QueId;
                        abc.pkid = model.pkid;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.CorrecrtWrong = model.CorrecrtWrong;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.CorrectAnswerDD = model.CorrectAnswerDD;
                        abc.StudentGiveAnswer = model.StudentGiveAnswer;
                        abc.GainMarks = (decimal)model.GainMarks;
                        abc.Answer1 = model.Answer1;
                        abc.Answer2 = model.Answer2;
                        abc.Answer3 = model.Answer3;
                        abc.Answer4 = model.Answer4;
                        return PartialView(abc);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetss Question = new tbl_AttemptExamAnswerSheetss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
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
                }
                else
                {
                    exception = "Session Expired.";
                    return PartialView();
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return PartialView();
            }

        }

        [HttpPost]
        public ActionResult ObjectiveQuestion(tbl_AttemptExamAnswerSheetss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;
                if (model.pkid == 0)
                {
                    tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    abc.Examid = model.Examid;
                    abc.Studentid = model.Studentid;
                    abc.CorrectAnswerDD = model.CorrectAnswerDD;
                    abc.StudentGiveAnswer = model.StudentGiveAnswer;
                    abc.GainMarks = model.GainMarks;
                    abc.Answer1 = model.Answer1;
                    abc.Answer2 = model.Answer2;
                    abc.Answer3 = model.Answer3;
                    abc.Answer4 = model.Answer4;
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Add(abc, true);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }
                else
                {
                    tbl_AttemptExamAnswerSheet abc = _Attemptexam.Get(model.pkid);
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    //abc.Examid = model.Examid;
                    //abc.Studentid = model.Studentid;
                    abc.CorrectAnswerDD = model.CorrectAnswerDD;
                    abc.StudentGiveAnswer = model.StudentGiveAnswer;
                    abc.GainMarks = model.GainMarks;
                    abc.Answer1 = model.Answer1;
                    abc.Answer2 = model.Answer2;
                    abc.Answer3 = model.Answer3;
                    abc.Answer4 = model.Answer4;
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Update(abc);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        [HttpGet]
        public ActionResult TrueFalseQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 1)
                    {
                        return RedirectToAction("ObjectiveQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 3)
                    {
                        return RedirectToAction("FillInBlanckQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 5)
                    {
                        return RedirectToAction("MatchContentQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 6)
                    {
                        return RedirectToAction("IdentifySignQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 7)
                    {
                        return RedirectToAction("FullFormQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }


                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).FirstOrDefault();
                    if (data != null)
                    {
                        tbl_AttemptExamAnswerSheetss abc = new tbl_AttemptExamAnswerSheetss();
                        tbl_AttemptExamAnswerSheet model = _Attemptexam.Get(data.pkid);
                        abc.QueId = model.QueId;
                        abc.pkid = model.pkid;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.CorrecrtWrong = model.CorrecrtWrong;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.StudentGiveAnswerTrueFalse = model.StudentGiveAnswerTrueFalse;
                        abc.GainMarks = (decimal)model.GainMarks;
                        abc.TrueFalse = model.TrueFalse;
                        return PartialView(abc);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetss Question = new tbl_AttemptExamAnswerSheetss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            if (Q.subjecttype_fkid == 2)
                            {
                                tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                                Question.TrueFalse = ANS.TrueFalse;
                            }
                        }
                        return PartialView(Question);
                    }
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

        [HttpPost]
        public ActionResult TrueFalseQuestion(tbl_AttemptExamAnswerSheetss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;
                if (model.pkid == 0)
                {
                    tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    abc.Examid = model.Examid;
                    abc.Studentid = model.Studentid;
                    abc.StudentGiveAnswerTrueFalse = model.StudentGiveAnswerTrueFalse;
                    abc.GainMarks = model.GainMarks;
                    abc.TrueFalse = model.TrueFalse;
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Add(abc, true);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }
                else
                {
                    tbl_AttemptExamAnswerSheet abc = _Attemptexam.Get(model.pkid);
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    //abc.Examid = model.Examid;
                    //abc.Studentid = model.Studentid;
                    abc.StudentGiveAnswerTrueFalse = model.StudentGiveAnswerTrueFalse;
                    abc.GainMarks = model.GainMarks;
                    abc.TrueFalse = model.TrueFalse;
                    _Attemptexam.Update(abc);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        [HttpGet]
        public ActionResult FillInBlanckQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 1)
                    {
                        return RedirectToAction("ObjectiveQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 2)
                    {
                        return RedirectToAction("TrueFalseQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 5)
                    {
                        return RedirectToAction("MatchContentQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 6)
                    {
                        return RedirectToAction("IdentifySignQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 7)
                    {
                        return RedirectToAction("FullFormQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }

                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).FirstOrDefault();
                    if (data != null)
                    {
                        tbl_AttemptExamAnswerSheetss abc = new tbl_AttemptExamAnswerSheetss();
                        tbl_AttemptExamAnswerSheet model = _Attemptexam.Get(data.pkid);
                        abc.QueId = model.QueId;
                        abc.pkid = model.pkid;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.CorrecrtWrong = model.CorrecrtWrong;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.StudentGiveAnswerfillblank = model.StudentGiveAnswerfillblank;
                        abc.GainMarks = (decimal)model.GainMarks;
                        abc.BlanckSpace = model.BlanckSpace;
                        return PartialView(abc);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetss Question = new tbl_AttemptExamAnswerSheetss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            if (Q.subjecttype_fkid == 3)
                            {
                                tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == Q.pkid && x.status == 1).FirstOrDefault();
                                Question.BlanckSpace = ANS.BlanckSpace;
                            }
                        }
                        return PartialView(Question);
                    }
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

        [HttpPost]
        public ActionResult FillInBlanckQuestion(tbl_AttemptExamAnswerSheetss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;
                if (model.pkid == 0)
                {
                    tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.Examid = model.Examid;
                    abc.Studentid = model.Studentid;
                    abc.StudentGiveAnswerfillblank = model.StudentGiveAnswerfillblank;
                    abc.BlanckSpace = model.BlanckSpace;
                    if (model.BlanckSpace.Trim().ToLower() == model.StudentGiveAnswerfillblank.Trim().ToLower())
                    {
                        abc.CorrecrtWrong = 1;
                        abc.GainMarks = (decimal)model.QuesMarks;
                    }
                    else
                    {
                        abc.CorrecrtWrong = 0;
                        abc.GainMarks = 0;
                    }
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Add(abc, true);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }
                else
                {
                    tbl_AttemptExamAnswerSheet abc = _Attemptexam.Get(model.pkid);
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    //abc.Examid = model.Examid;
                    //abc.Studentid = model.Studentid;
                    abc.StudentGiveAnswerfillblank = model.StudentGiveAnswerfillblank;
                    abc.BlanckSpace = model.BlanckSpace;
                    if (model.BlanckSpace.Trim().ToLower() == model.StudentGiveAnswerfillblank.Trim().ToLower())
                    {
                        abc.CorrecrtWrong = 1;
                        abc.GainMarks = (decimal)model.QuesMarks;
                    }
                    else
                    {
                        abc.CorrecrtWrong = 0;
                        abc.GainMarks = 0;
                    }
                    _Attemptexam.Update(abc);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        [HttpGet]
        public ActionResult MatchContentQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 1)
                    {
                        return RedirectToAction("ObjectiveQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 2)
                    {
                        return RedirectToAction("TrueFalseQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 3)
                    {
                        return RedirectToAction("FillInBlanckQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 6)
                    {
                        return RedirectToAction("IdentifySignQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 7)
                    {
                        return RedirectToAction("FullFormQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }


                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).Select(x => x.StudentGiveAnswerMatch).ToList();
                    if (data.Count() != 0)
                    {
                        string combindedString = string.Join(",", data.ToArray());
                        tbl_AttemptExamAnswerSheetMatchSepss Question = new tbl_AttemptExamAnswerSheetMatchSepss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.pkid = 1;
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            Question.BlanckSpace = combindedString;
                            if (Q.subjecttype_fkid == 5)
                            {
                                List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid).ToList();
                                Question.MATContent = ANS;
                            }
                        }
                        return PartialView(Question);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetMatchSepss Question = new tbl_AttemptExamAnswerSheetMatchSepss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            if (Q.subjecttype_fkid == 5)
                            {
                                List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid).ToList();
                                Question.MATContent = ANS;
                            }
                        }
                        return PartialView(Question);
                    }
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

        [HttpPost]
        public ActionResult MatchContentQuestion(tbl_AttemptExamAnswerSheetMatchSepss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;


                if (model.pkid == 0)
                {

                    List<int> StudAnswer = model.StudentGiveAnswerMatch.Split(',').Select(int.Parse).ToList();
                    List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == model.QueId).ToList();
                    int n = 0;
                    foreach (var item in StudAnswer)
                    {
                        tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                        abc.QueId = model.QueId;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.StudentGiveAnswerMatch = item;
                        abc.MatchAnswer = Convert.ToInt32(ANS.ElementAt(n).AnsweColoumn);

                        if (abc.StudentGiveAnswerMatch == abc.MatchAnswer)
                        {
                            abc.CorrecrtWrong = 1;
                            abc.GainMarks = (model.QuesMarks / ANS.Count());
                        }
                        else
                        {
                            abc.CorrecrtWrong = 0;
                            abc.GainMarks = 0;
                        }
                        n++;
                        abc.Addeddate = DateTime.Now;
                        _Attemptexam.Add(abc, true);
                    }
                }
                else
                {
                    var qeta = _Attemptexam.GetAll().Where(x => x.Examid == model.Examid && x.QueId == model.QueId && x.Studentid == model.Studentid && x.QNo == model.QNo).ToList();
                    foreach (var deta in qeta)
                    {
                        tbl_AttemptExamAnswerSheet ad = _Attemptexam.Get(deta.pkid);
                        _Attemptexam.Remove(ad, true);
                    }

                    List<int> StudAnswer = model.StudentGiveAnswerMatch.Split(',').Select(int.Parse).ToList();
                    List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == model.QueId).ToList();
                    int n = 0;
                    foreach (var item in StudAnswer)
                    {
                        tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                        abc.QueId = model.QueId;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        //abc.Examid = model.Examid;
                        //abc.Studentid = model.Studentid;
                        abc.StudentGiveAnswerMatch = item;
                        abc.MatchAnswer = Convert.ToInt32(ANS.ElementAt(n).AnsweColoumn);

                        if (abc.StudentGiveAnswerMatch == abc.MatchAnswer)
                        {
                            abc.CorrecrtWrong = 1;
                            abc.GainMarks = (model.QuesMarks / ANS.Count());
                        }
                        else
                        {
                            abc.CorrecrtWrong = 0;
                            abc.GainMarks = 0;
                        }
                        n++;
                        abc.Addeddate = DateTime.Now;
                        _Attemptexam.Add(abc, true);
                    }
                }
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        [HttpGet]
        public ActionResult IdentifySignQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 1)
                    {
                        return RedirectToAction("ObjectiveQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 2)
                    {
                        return RedirectToAction("TrueFalseQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 3)
                    {
                        return RedirectToAction("FillInBlanckQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 5)
                    {
                        return RedirectToAction("MatchContentQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 7)
                    {
                        return RedirectToAction("FullFormQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }


                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).FirstOrDefault();
                    if (data != null)
                    {
                        tbl_AttemptExamAnswerSheetss abc = new tbl_AttemptExamAnswerSheetss();
                        tbl_AttemptExamAnswerSheet model = _Attemptexam.Get(data.pkid);
                        abc.QueId = model.QueId;
                        abc.pkid = model.pkid;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.CorrecrtWrong = model.CorrecrtWrong;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.CorrectAnswerDD = model.CorrectAnswerDD;
                        abc.StudentGiveAnswer = model.StudentGiveAnswer;
                        abc.GainMarks = (decimal)model.GainMarks;
                        abc.Answer1 = model.Answer1;
                        abc.Answer2 = model.Answer2;
                        abc.Answer3 = model.Answer3;
                        abc.Answer4 = model.Answer4;
                        return PartialView(abc);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetss Question = new tbl_AttemptExamAnswerSheetss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
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

        [HttpPost]
        public ActionResult IdentifySignQuestion(tbl_AttemptExamAnswerSheetss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;
                if (model.pkid == 0)
                {
                    tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    abc.Examid = model.Examid;
                    abc.Studentid = model.Studentid;
                    abc.CorrectAnswerDD = model.CorrectAnswerDD;
                    abc.StudentGiveAnswer = model.StudentGiveAnswer;
                    abc.GainMarks = model.GainMarks;
                    abc.Answer1 = model.Answer1;
                    abc.Answer2 = model.Answer2;
                    abc.Answer3 = model.Answer3;
                    abc.Answer4 = model.Answer4;
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Add(abc, true);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }
                else
                {
                    tbl_AttemptExamAnswerSheet abc = _Attemptexam.Get(model.pkid);
                    abc.QueId = model.QueId;
                    abc.Question = model.Question;
                    abc.QuestypeName = model.QuestypeName;
                    abc.QNo = model.QNo;
                    abc.QuesMarks = model.QuesMarks;
                    abc.CorrecrtWrong = model.CorrecrtWrong;
                    //abc.Examid = model.Examid;
                    //abc.Studentid = model.Studentid;
                    abc.CorrectAnswerDD = model.CorrectAnswerDD;
                    abc.StudentGiveAnswer = model.StudentGiveAnswer;
                    abc.GainMarks = model.GainMarks;
                    abc.Answer1 = model.Answer1;
                    abc.Answer2 = model.Answer2;
                    abc.Answer3 = model.Answer3;
                    abc.Answer4 = model.Answer4;
                    abc.Addeddate = DateTime.Now;
                    _Attemptexam.Update(abc);
                    return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        [HttpGet]
        public ActionResult FullFormQuestion(int QuestionId, int QuestionNo)
        {
            string exception = "";
            try
            {
                HttpCookie cookie = Request.Cookies["search"];
                if ((cookie != null) && (cookie.Value != ""))
                {
                    int Examid = Convert.ToInt32(TempData.Peek("Eid").ToString());
                    int Studentid = Convert.ToInt32(TempData.Peek("Sid").ToString());
                    int Type_fkid = Convert.ToInt32(_question.Get(QuestionId).subjecttype_fkid);
                    if (Type_fkid == 1)
                    {
                        return RedirectToAction("ObjectiveQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 2)
                    {
                        return RedirectToAction("TrueFalseQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 3)
                    {
                        return RedirectToAction("FillInBlanckQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 5)
                    {
                        return RedirectToAction("MatchContentQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }
                    else if (Type_fkid == 6)
                    {
                        return RedirectToAction("IdentifySignQuestion", "StudentExam", new { QuestionId = QuestionId, QuestionNo = QuestionNo });
                    }

                    var data = _Attemptexam.GetAll().Where(x => x.Examid == Examid && x.QueId == QuestionId && x.Studentid == Studentid).Select(x => x.StudentZGiveAnswerFullform).ToList();
                    if (data.Count != 0)
                    {
                        tbl_AttemptExamAnswerSheetMatchSepss Question = new tbl_AttemptExamAnswerSheetMatchSepss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.pkid = 1;
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            if (Q.subjecttype_fkid == 7)
                            {
                                List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid).ToList();
                                Question.MATContent = ANS;
                                List<StudentGiveAnswerfull> LI = new List<StudentGiveAnswerfull>();
                                int check = 0;
                                foreach (var item in ANS)
                                {
                                    StudentGiveAnswerfull veta = new StudentGiveAnswerfull();
                                    try
                                    {
                                        veta.Ans = data[check];
                                    }
                                    catch { veta.Ans = ""; }
                                    LI.Add(veta);
                                    check++;
                                }
                                Question.FULLF = LI;
                            }
                        }
                        return PartialView(Question);
                    }
                    else
                    {
                        tbl_AttemptExamAnswerSheetMatchSepss Question = new tbl_AttemptExamAnswerSheetMatchSepss();
                        tbl_QuestionMaster Q = _question.Get(QuestionId);
                        if (Q.Status == 1)
                        {
                            Question.QueId = QuestionId;
                            Question.Question = Q.Question;
                            Question.QuestypeName = _subtype.Get(Q.subjecttype_fkid).Questiontype;
                            Question.QuesMarks = Q.Marks;
                            Question.QNo = QuestionNo;
                            if (Q.subjecttype_fkid == 7)
                            {
                                List<tbl_MatchContentQuestionMaster> ANS = _Matc.GetAll().Where(x => x.Ques_fkid == Q.pkid).ToList();
                                Question.MATContent = ANS;
                            }
                        }
                        return PartialView(Question);
                    }
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

        [HttpPost]
        public ActionResult FullFormQuestion(tbl_AttemptExamAnswerSheetMatchSepss model)
        {
            string exception = "";
            try
            {
                TempData["Qid"] = model.QueId;
                TempData["Qno"] = model.QNo;


                if (model.pkid == 0)
                {
                    int n = 0;
                    foreach (var item in model.FULLF)
                    {
                        tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                        abc.QueId = model.QueId;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.StudentZGiveAnswerFullform = item.Ans;
                        abc.FullForm = model.MATContent.ElementAt(n).AnsweColoumn;

                        if (abc.StudentZGiveAnswerFullform.Trim().ToLower() == abc.FullForm.Trim().ToLower())
                        {
                            abc.CorrecrtWrong = 1;
                            abc.GainMarks = (model.QuesMarks / model.FULLF.Count());
                        }
                        else
                        {
                            abc.CorrecrtWrong = 0;
                            abc.GainMarks = 0;
                        }
                        n++;
                        abc.Addeddate = DateTime.Now;
                        _Attemptexam.Add(abc, true);
                    }
                }
                else
                {
                    var qeta = _Attemptexam.GetAll().Where(x => x.Examid == model.Examid && x.QueId == model.QueId && x.Studentid == model.Studentid && x.QNo == model.QNo).ToList();
                    foreach (var deta in qeta)
                    {
                        tbl_AttemptExamAnswerSheet ad = _Attemptexam.Get(deta.pkid);
                        _Attemptexam.Remove(ad, true);
                    }

                    int n = 0;
                    foreach (var item in model.FULLF)
                    {
                        tbl_AttemptExamAnswerSheet abc = new tbl_AttemptExamAnswerSheet();
                        abc.QueId = model.QueId;
                        abc.Question = model.Question;
                        abc.QuestypeName = model.QuestypeName;
                        abc.QNo = model.QNo;
                        abc.QuesMarks = model.QuesMarks;
                        abc.Examid = model.Examid;
                        abc.Studentid = model.Studentid;
                        abc.StudentZGiveAnswerFullform = item.Ans;
                        abc.FullForm = model.MATContent.ElementAt(n).AnsweColoumn;

                        if (abc.StudentZGiveAnswerFullform.Trim().ToLower() == abc.FullForm.Trim().ToLower())
                        {
                            abc.CorrecrtWrong = 1;
                            abc.GainMarks = (model.QuesMarks / model.FULLF.Count());
                        }
                        else
                        {
                            abc.CorrecrtWrong = 0;
                            abc.GainMarks = 0;
                        }
                        n++;
                        abc.Addeddate = DateTime.Now;
                        _Attemptexam.Add(abc, true);
                    }
                }
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentDashboard", "StudentExam", new { Examid = model.Examid, Exception = exception });

            }
        }

        public ActionResult SubmitExam(int eid, int sid)
        {
            try
            {
                if (eid != 0 && sid != 0)
                {
                    var data = _examresult.GetAll().Where(x => x.Exam_fkid == eid && x.Student_fkid == sid).ToList();
                    tbl_Exam_Master exam = _Exam.Get(eid);
                    if (data.Count() == 0)
                    {
                        tbl_StudentExamResult abc = new tbl_StudentExamResult();

                        abc.Student_fkid = sid;
                        abc.Exam_fkid = eid;
                        abc.TotalMarks = exam.Total_Marks;
                        abc.TotalGainMarks = _Attemptexam.GetAll().Where(x => x.Studentid == abc.Student_fkid && x.Examid == abc.Exam_fkid).Sum(x => x.GainMarks);
                        abc.AttemptExamDate = DateTime.Now;
                        if (abc.TotalGainMarks >= exam.PassingMarks)
                        {
                            abc.Result = "Pass";
                        }
                        else { abc.Result = "Failed"; }
                        abc.FinalStatus = true;
                        _examresult.Add(abc);
                        return Json("s", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var model = data.FirstOrDefault();
                        tbl_StudentExamResult abc = _examresult.Get(model.pkid);

                        abc.Student_fkid = model.Student_fkid;
                        abc.Exam_fkid = model.Exam_fkid;
                        abc.TotalMarks = exam.Total_Marks;
                        abc.TotalGainMarks = _Attemptexam.GetAll().Where(x => x.Studentid == abc.Student_fkid && x.Examid == abc.Exam_fkid).Sum(x => x.GainMarks);
                        abc.AttemptExamDate = DateTime.Now;
                        if (abc.TotalGainMarks >= exam.PassingMarks)
                        {
                            abc.Result = "Pass";
                        }
                        else { abc.Result = "Failed"; }
                        abc.FinalStatus = true;
                        _examresult.Update(abc);
                        return Json("s", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                { return Json("P", JsonRequestBehavior.AllowGet); }
            }
            catch
            {
                return Json("P", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SubmitExamServer(int eid, int sid)
        {
            try
            {
                if (eid != 0 && sid != 0)
                {
                    var data = _examresult.GetAll().Where(x => x.Exam_fkid == eid && x.Student_fkid == sid).ToList();
                    tbl_Exam_Master exam = _Exam.Get(eid);
                    if (data.Count() == 0)
                    {
                        tbl_StudentExamResult abc = new tbl_StudentExamResult();

                        abc.Student_fkid = sid;
                        abc.Exam_fkid = eid;
                        abc.TotalMarks = exam.Total_Marks;
                        abc.TotalGainMarks = _Attemptexam.GetAll().Where(x => x.Studentid == abc.Student_fkid && x.Examid == abc.Exam_fkid).Sum(x => x.GainMarks);
                        abc.AttemptExamDate = DateTime.Now;
                        if (abc.TotalGainMarks >= exam.PassingMarks)
                        {
                            abc.Result = "Pass";
                        }
                        else { abc.Result = "Failed"; }
                        abc.FinalStatus = true;
                        _examresult.Add(abc);
                        return RedirectToAction("AfterSubmitForm", "StudentExam");
                    }
                    else
                    {
                        var model = data.FirstOrDefault();
                        tbl_StudentExamResult abc = _examresult.Get(model.pkid);

                        abc.Student_fkid = model.Student_fkid;
                        abc.Exam_fkid = model.Exam_fkid;
                        abc.TotalMarks = exam.Total_Marks;
                        abc.TotalGainMarks = _Attemptexam.GetAll().Where(x => x.Studentid == abc.Student_fkid && x.Examid == abc.Exam_fkid).Sum(x => x.GainMarks);
                        abc.AttemptExamDate = DateTime.Now;
                        if (abc.TotalGainMarks >= exam.PassingMarks)
                        {
                            abc.Result = "Pass";
                        }
                        else { abc.Result = "Failed"; }
                        abc.FinalStatus = true;
                        _examresult.Update(abc);
                        return RedirectToAction("AfterSubmitForm", "StudentExam");
                    }
                }
                else
                { return RedirectToAction("AfterSubmitForm", "StudentExam"); }
            }
            catch
            {
                return RedirectToAction("AfterSubmitForm", "StudentExam");
            }
        }

        [HttpGet]
        public ActionResult AfterSubmitForm()
        {
            try
            {
                HttpCookie currentUserCookie = Request.Cookies["search"];
                HttpCookie ExamCoo = Request.Cookies["exam"];
                HttpCookie StartTimeCo = Request.Cookies["Startt"];
                HttpCookie TimerCo = Request.Cookies["timer"];
                Response.Cookies.Remove("search");
                Response.Cookies.Remove("exam");
                Response.Cookies.Remove("Startt");
                Response.Cookies.Remove("timer");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                ExamCoo.Expires = DateTime.Now.AddDays(-10);
                StartTimeCo.Expires = DateTime.Now.AddDays(-10);
                TimerCo.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                ExamCoo.Value = null;
                StartTimeCo.Value = null;
                TimerCo.Value = null;
                Response.SetCookie(currentUserCookie);
                Response.SetCookie(ExamCoo);
                Response.SetCookie(StartTimeCo);
                Response.SetCookie(TimerCo);
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}