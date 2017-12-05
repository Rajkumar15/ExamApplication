using DataLayer;
using DataLayer.ExamModel;
using Exam_Application.Models.BAL;
using Exam_Application.Models.DAL;
using itechDll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam_Application.Controllers
{
    [Authorize]
    public class SubjectGroupController : Controller
    {
        public readonly IRepository<tbl_User_Profile> _user;
        public readonly IRepository<tbl_GroupMaster> _group;
        public readonly IRepository<tbl_Subject_master> _subject;
        public readonly IRepository<tbl_QuestionType_Master> _subtype;
        public readonly IRepository<tbl_StudentMaster> _student;
        public readonly IRepository<tbl_Stude_subjectTypeList> _subtypeList;
        public readonly IRepository<tbl_QuestionMaster> _question;
        public readonly IRepository<tbl_AnswerMaster> _answer;


        public SubjectGroupController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_Subject_master> subject,
            IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_Stude_subjectTypeList> subtypeList, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer)
        {
            _user = user;
            _group = group;
            _subject = subject;
            _subtype = subtype;
            _student = student;
            _subtypeList = subtypeList;
            _question = question;
            _answer = answer;
        }


        [HttpGet]
        public ActionResult SubjectMaster(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult GetSubjectList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            //string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _subject.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             name = a.subjectName,
                             co = 1
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult AddSubject(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Subject_master model = _subject.Get(_id);
                tbl_Subject_masterss abc = new tbl_Subject_masterss();
                abc.pkid = model.pkid;
                abc.subjectName = model.subjectName;
                abc.subjectDescription = model.subjectDescription;
                return PartialView(abc);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSubject(tbl_Subject_masterss model)
        {
            string exception = "";

            try
            {
                if (model.pkid == 0)
                {
                    tbl_Subject_master abc = new tbl_Subject_master();
                    abc.subjectName = model.subjectName;
                    abc.subjectDescription = model.subjectDescription;
                    abc.adddate = DateTime.Now;
                    abc.lastmodifieddate = DateTime.Now;
                    _subject.Add(abc);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Subject_master abc = _subject.Get(_id);
                    abc.pkid = model.pkid;
                    abc.subjectName = model.subjectName;
                    abc.subjectDescription = model.subjectDescription;
                    abc.lastmodifieddate = DateTime.Now;
                    _subject.Update(abc);
                    exception = "Updated Successfully";
                }
                return RedirectToAction("SubjectMaster", "SubjectGroup", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("SubjectMaster", "SubjectGroup", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult AddQuestion(string id, string Exception)
        {
            ViewBag.SubjectList = new SelectList(_subject.GetAll(), "pkid", "subjectName");
            ViewBag.SubjectTypeList = new SelectList(_subtype.GetAll(), "pkid", "subjecttype");
            ViewBag.Exception = Exception;
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_QuestionMaster model = _question.Get(_id);
                QuestionMaster abc = new QuestionMaster();
                abc.pkid = model.pkid;
                abc.subjecttype_fkid = model.subjecttype_fkid;
                abc.Question = model.Question;
                abc.Explaination = model.Explaination;
                abc.Subject_fkid = Convert.ToInt32(model.Subject_fkid);
                abc.hint = model.hint;

                abc.SelectLevel = model.SelectLevel;
                abc.SelectLevel = model.SelectLevel;
                abc.NegativeMarks = model.NegativeMarks;
                abc.Marks = model.Marks;
                abc.Status = model.Status;
                abc.Adddate = model.Adddate;

                tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == model.pkid).FirstOrDefault();
                abc.Questtype_fkid = ANS.Questtype_fkid;
                abc.Answer1 = ANS.Answer1;
                abc.Answer2 = ANS.Answer2;
                abc.Answer3 = ANS.Answer3;
                abc.Answer4 = ANS.Answer4;
                abc.CorrectAnswerDD = ANS.CorrectAnswer;
                abc.BlanckSpace = ANS.BlanckSpace;
                abc.TrueFalse = ANS.TrueFalse;
                abc.SubAnswer = ANS.SubAnswer;
                abc.Status = ANS.status;
                abc.Adddate = ANS.Adddate;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddQuestion(QuestionMaster model)
        {
            string exception = "";
            try
            {
                if (model.pkid == 0)
                {
                    tbl_QuestionMaster abc = new tbl_QuestionMaster();
                    abc.subjecttype_fkid = model.Questtype_fkid;
                    abc.Question = model.Question;
                    abc.Explaination = model.Explaination;
                    abc.Subject_fkid = model.Subject_fkid;
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.SelectLevel = model.SelectLevel;
                    abc.Marks = model.Marks;
                    abc.Status = 1;
                    abc.Adddate = DateTime.Now;
                    abc.lastModified = DateTime.Now;
                    _question.Add(abc);
                    tbl_AnswerMaster ANS = new tbl_AnswerMaster();
                    ANS.Ques_fkid = _question.GetAll().Max(x => x.pkid);
                    ANS.Questtype_fkid = model.Questtype_fkid;
                    ANS.Answer1 = model.Answer1;
                    ANS.Answer2 = model.Answer2;
                    ANS.Answer3 = model.Answer3;
                    ANS.Answer4 = model.Answer4;
                    ANS.CorrectAnswer = model.CorrectAnswerDD;
                    ANS.BlanckSpace = model.BlanckSpace;
                    ANS.TrueFalse = model.TrueFalse;
                    ANS.SubAnswer = model.SubAnswer;
                    ANS.status = 1;
                    ANS.Adddate = DateTime.Now;
                    ANS.lastmodified = DateTime.Now;
                    _answer.Add(ANS);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_QuestionMaster abc = _question.Get(_id);
                    abc.subjecttype_fkid = model.Questtype_fkid;
                    abc.Question = model.Question;
                    abc.Explaination = model.Explaination;
                    abc.Subject_fkid = model.Subject_fkid;
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.Marks = model.Marks;
                    abc.Status = model.Status;
                    abc.Adddate = model.Adddate;
                    abc.lastModified = DateTime.Now;
                    _question.Update(abc);

                    tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == model.pkid).FirstOrDefault();
                    ANS.Questtype_fkid = model.Questtype_fkid;
                    ANS.Answer1 = model.Answer1;
                    ANS.Answer2 = model.Answer2;
                    ANS.Answer3 = model.Answer3;
                    ANS.Answer4 = model.Answer4;
                    ANS.CorrectAnswer = model.CorrectAnswerDD;
                    ANS.BlanckSpace = model.BlanckSpace;
                    ANS.TrueFalse = model.TrueFalse;
                    ANS.SubAnswer = model.SubAnswer;
                    ANS.status = 1;
                    ANS.Adddate = DateTime.Now;
                    ANS.lastmodified = DateTime.Now;
                    _answer.Update(ANS);
                    exception = "Updated Successfully";
                }
                return RedirectToAction("AddQuestion", "SubjectGroup", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("AddQuestion", "SubjectGroup", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult QuestionList(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult QuestionListLogic()
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
                var v = (from a in _question.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             name = a.Question,
                             subject = (a.Subject_fkid != null ? _subject.Get(a.Subject_fkid).subjectName : "")
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower()) || x.subject.ToLower().Contains(search.ToLower())) select b);
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

        public ActionResult DeleteQuestion(int id)
        {
            string exception = "";
            try
            {
                tbl_QuestionMaster QUE = _question.Get(id);
                tbl_AnswerMaster ANS = _answer.GetAll().Where(x => x.Ques_fkid == QUE.pkid).FirstOrDefault();
                _answer.Remove(ANS, true);
                _question.Remove(QUE, true);
                exception = "Deleted Successfully.";
                return Json(exception, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return Json(exception, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GroupMaster(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }

        public ActionResult GetGroupList()
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
                var v = (from a in _group.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             name = a.GroupName,
                             co = 1
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

        [HttpGet]
        public ActionResult AddGroup(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_GroupMaster model = _group.Get(_id);
                tbl_GroupMasterss abc = new tbl_GroupMasterss();
                abc.pkid = model.pkid;
                abc.GroupName = model.GroupName;
                abc.description = model.description;
                return PartialView(abc);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddGroup(tbl_GroupMasterss model)
        {
            string exception = "";
            try
            {
                if (model.pkid == 0)
                {
                    tbl_GroupMaster abc = new tbl_GroupMaster();
                    abc.pkid = model.pkid;
                    abc.GroupName = model.GroupName;
                    abc.description = model.description;
                    abc.adddate = DateTime.Now;
                    abc.lastdatemodified = DateTime.Now;
                    _group.Add(abc);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_GroupMaster abc = _group.Get(_id);
                    abc.pkid = model.pkid;
                    abc.GroupName = model.GroupName;
                    abc.description = model.description;
                    abc.lastdatemodified = DateTime.Now;
                    _group.Update(abc);
                    exception = "Updated Successfully";
                }
                return RedirectToAction("GroupMaster", "SubjectGroup", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("GroupMaster", "SubjectGroup", new { Exception = exception });
            }
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
                             email = a.Email,
                             env = a.Enrollnmentno,
                             phone = a.Email,
                             doa = a.Email,
                             co = 1
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.email.ToLower().Contains(search.ToLower()) || x.env.ToLower().Contains(search.ToLower()) || x.phone.ToLower().Contains(search.ToLower()) || x.doa.Contains(search)) select b);
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
        public ActionResult AddStudent(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_StudentMaster model = _student.Get(_id);
                tbl_StudentMasterss abc = new tbl_StudentMasterss();
                abc.pkid = model.pkid;
                abc.Email = model.Email;
                abc.Name = model.Name;
                abc.Address = model.Address;
                abc.Enrollnmentno = model.Enrollnmentno;
                abc.GuardianContact = model.GuardianContact;
                abc.status = model.status;
                return PartialView(abc);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddStudent(tbl_StudentMasterss model, HttpPostedFileBase ppic)
        {
            WebFunction web = new WebFunction();
            string exception = "";
            try
            {
                //if (String.IsNullOrWhiteSpace(model.user_fkid))
                //{
                //    exception = "Enter First EMail or Mobile And Password";
                //    return RedirectToAction("StudentMaster", "SubjectGroup", new { Exception = exception });
                //}
                if (model.pkid == 0)
                {
                    int ftcount = _student.GetAll().Where(x => x.UserName == model.UserName || x.Enrollnmentno == model.Enrollnmentno).Count();
                    if (ftcount > 0)
                    {
                        exception = "Student Already Taken.";
                        return RedirectToAction("StudentMaster", "SubjectGroup", new { Exception = exception });
                    }
                    tbl_StudentMaster abc = new tbl_StudentMaster();
                    abc.pkid = model.pkid;
                    abc.Email = model.Email;
                    abc.Name = model.Name;
                    abc.user_fkid = model.user_fkid;
                    abc.mobileno = model.mobileno;
                    abc.Address = model.Address;
                    abc.Enrollnmentno = model.Enrollnmentno;
                    abc.GuardianContact = model.GuardianContact;
                    abc.status = 1;
                    abc.Adddate = DateTime.Now;
                    abc.UserName = model.UserName;
                    abc.Password = model.Password;
                    abc.lastmodifieddate = DateTime.Now;
                    if (ppic != null)
                    {
                        abc.ProfilePhoto = web.Storefile(ppic, 1);
                    }
                    _student.Add(abc);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_StudentMaster abc = _student.Get(_id);
                    abc.pkid = model.pkid;
                    abc.Email = model.Email;
                    abc.Name = model.Name;
                    abc.user_fkid = model.user_fkid;
                    abc.mobileno = model.mobileno;
                    abc.Address = model.Address;
                    abc.Enrollnmentno = model.Enrollnmentno;
                    abc.GuardianContact = model.GuardianContact;
                    abc.status = model.status;
                    abc.lastmodifieddate = DateTime.Now;
                    if (ppic != null)
                    {
                        if (model.ProfilePhoto != null)
                        {
                            web.DeleteImage(model.ProfilePhoto);
                        }

                        abc.ProfilePhoto = web.Storefile(ppic, 1);
                    }
                    _student.Update(abc);
                    exception = "Updated Successfully";
                }
                return RedirectToAction("StudentMaster", "SubjectGroup", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("StudentMaster", "SubjectGroup", new { Exception = exception });
            }
        }

        [HttpGet]
        public ActionResult UploadExcel(string Exception)
        {
            ViewBag.Exception = Exception;
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel()
        {
            string exception = "";
            try
            {
                WebFunction comm = new WebFunction();
                HttpPostedFileBase FileUpload = Request.Files[0];
                DataTable dt = new DataTable();
                string alert = "", msg = "";

                if (FileUpload != null)
                {
                    dt = comm.GetExcesData(FileUpload);

                    //   List<tbl_MonthlyLibility> monthlyLibalityList = new List<tbl_MonthlyLibility>();
                    int i = 1;
                    foreach (DataRow dr in dt.Rows.Cast<DataRow>().Skip(2))
                    {

                        tbl_QuestionMaster QUESTION = new tbl_QuestionMaster();
                        tbl_AnswerMaster ANSWER = new tbl_AnswerMaster();
                       var q = dr[1].ToString();
                        var A = dr[2].ToString();
                        QUESTION.subjecttype_fkid = Convert.ToInt32(dr[0]);
                        QUESTION.Question = dr[1].ToString();
                        QUESTION.Explaination = dr[2].ToString();
                        QUESTION.Subject_fkid = Convert.ToInt32(dr[3]);
                        QUESTION.hint = dr[4].ToString();
                        QUESTION.Marks = Convert.ToDecimal(dr[5]);
                        try { QUESTION.NegativeMarks = Convert.ToDecimal(dr[6]); }
                        catch { QUESTION.NegativeMarks = 0; }
                        
                        QUESTION.SelectLevel = Convert.ToInt32(dr[7]);
                        QUESTION.Adddate = DateTime.Now;
                        QUESTION.Status = 1;
                        QUESTION.lastModified = DateTime.Now;
                        _question.Add(QUESTION);
                        ANSWER.Ques_fkid = _question.GetAll().Max(x => x.pkid);
                        ANSWER.Questtype_fkid = Convert.ToInt32(dr[0]);
                        ANSWER.Answer1 = dr[8].ToString();
                        ANSWER.Answer2 = dr[9].ToString();
                        ANSWER.Answer3 = dr[10].ToString();
                        ANSWER.Answer4 = dr[11].ToString();
                        try { ANSWER.CorrectAnswer = Convert.ToInt32(dr[12]); }
                        catch { }
                       
                        ANSWER.BlanckSpace = dr[13].ToString();
                        try { ANSWER.TrueFalse = Convert.ToBoolean(dr[14]); }
                        catch { }
                        ANSWER.SubAnswer = dr[15].ToString();
                        ANSWER.Adddate = DateTime.Now;
                        ANSWER.status = 1;
                        ANSWER.lastmodified = DateTime.Now;
                        _answer.Add(ANSWER);
                    }
                    exception = "All Data Uploaded Successfully";
                }
                else
                {
                    exception = "Please select File.";
                }

                return View();
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
            }
        }

        public FileResult DownloadExcel()
        {
            string path = "/Doc/Users.xlsx";
            return File(path, "application/vnd.ms-excel", "Users.xlsx");
        }
    }
}