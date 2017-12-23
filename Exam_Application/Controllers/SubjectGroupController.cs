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
    public class SubjectGroupController : Controller
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

        public SubjectGroupController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_CourseMaster> course,
            IRepository<tbl_DivisionMaster> division, IRepository<tbl_QuestionType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_QuestionMaster> question,
            IRepository<tbl_AnswerMaster> answer, IRepository<tbl_MatchContentQuestionMaster> Matc)
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
        }


        //[HttpGet]
        //public ActionResult SubjectMaster(string Exception)
        //{
        //    ViewBag.Exception = Exception;
        //    return View();
        //}

        //public ActionResult GetSubjectList()
        //{
        //    var search = Request.Form.GetValues("search[value]")[0];
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    //Find Order Column
        //    //string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    //string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int recordsTotal = 0;

        //    //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
        //    try
        //    {
        //        var v = (from a in _subject.GetAll()
        //                 select new
        //                 {
        //                     pkid = a.pkid,
        //                     name = a.subjectName,
        //                     co = 1
        //                 });
        //        if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
        //        {
        //            v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower())) select b);
        //        }

        //        recordsTotal = v.Count();
        //        var data = v.Skip(skip).Take(pageSize).ToList();
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

        //    }
        //}

        //[HttpGet]
        //public ActionResult AddSubject(string id)
        //{
        //    if (!String.IsNullOrWhiteSpace(id))
        //    {
        //        int _id = Convert.ToInt32(id);
        //        tbl_Subject_master model = _subject.Get(_id);
        //        tbl_Subject_masterss abc = new tbl_Subject_masterss();
        //        abc.pkid = model.pkid;
        //        abc.subjectName = model.subjectName;
        //        abc.subjectDescription = model.subjectDescription;
        //        return PartialView(abc);
        //    }
        //    return PartialView();
        //}

        //[HttpPost]
        //public ActionResult AddSubject(tbl_Subject_masterss model)
        //{
        //    string exception = "";

        //    try
        //    {
        //        if (model.pkid == 0)
        //        {
        //            tbl_Subject_master abc = new tbl_Subject_master();
        //            abc.subjectName = model.subjectName;
        //            abc.subjectDescription = model.subjectDescription;
        //            abc.adddate = DateTime.Now;
        //            abc.lastmodifieddate = DateTime.Now;
        //            _subject.Add(abc);
        //            exception = "Save Successfully";
        //        }
        //        else
        //        {
        //            int _id = Convert.ToInt32(model.pkid);
        //            tbl_Subject_master abc = _subject.Get(_id);
        //            abc.pkid = model.pkid;
        //            abc.subjectName = model.subjectName;
        //            abc.subjectDescription = model.subjectDescription;
        //            abc.lastmodifieddate = DateTime.Now;
        //            _subject.Update(abc);
        //            exception = "Updated Successfully";
        //        }
        //        return RedirectToAction("SubjectMaster", "SubjectGroup", new { Exception = exception });
        //    }
        //    catch (Exception e)
        //    {
        //        Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
        //        exception = e.Message;
        //        return RedirectToAction("SubjectMaster", "SubjectGroup", new { Exception = exception });
        //    }
        //}

        [HttpGet]
        public ActionResult AddQuestion(string id, string Exception)
        {
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.DivisionList = new SelectList(_division.GetAll(), "pkid", "DivisioName");
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
                abc.Division_fkid = model.Division_fkid;
                abc.hint = model.hint;

                abc.SelectLevel = model.SelectLevel;
                abc.SelectLevel = model.SelectLevel;
                abc.NegativeMarks = model.NegativeMarks;
                abc.Marks = model.Marks;
                abc.Status = model.Status;
                abc.Adddate = model.Adddate;

                if (model.subjecttype_fkid == 5)
                {
                    abc.MATContent = _Matc.GetAll().Where(x => x.Ques_fkid == model.pkid).ToList();
                }
                else if (model.subjecttype_fkid == 7)
                {
                    abc.FULLF = _Matc.GetAll().Where(x => x.Ques_fkid == model.pkid).ToList();
                }
                else
                {
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
                }
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddQuestion(QuestionMaster model)
        {
            WebFunction web = new WebFunction();
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
                    abc.Division_fkid = model.Division_fkid;
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.Marks = model.Marks;
                    abc.Status = 1;
                    abc.Adddate = DateTime.Now;
                    abc.lastModified = DateTime.Now;
                    _question.Add(abc);
                    int Maxid = _question.GetAll().Max(x => x.pkid);
                    if (model.Questtype_fkid == 5)
                    {
                        foreach (var item in model.MATContent)
                        {
                            if (!string.IsNullOrWhiteSpace(item.FirstColoumn) && !string.IsNullOrWhiteSpace(item.OppositeColoumn) && !string.IsNullOrWhiteSpace(item.AnsweColoumn))
                            {
                                tbl_MatchContentQuestionMaster MA = new tbl_MatchContentQuestionMaster();
                                MA.Ques_fkid = Maxid;
                                MA.FirstColoumn = item.FirstColoumn;
                                MA.OppositeColoumn = item.OppositeColoumn;
                                MA.AnsweColoumn = item.AnsweColoumn;
                                MA.LastDatetime = DateTime.Now;
                                _Matc.Add(MA);
                            }
                        }
                    }
                    else if (model.Questtype_fkid == 7)
                    {
                        foreach (var item in model.FULLF)
                        {
                            if (!string.IsNullOrWhiteSpace(item.FirstColoumn) && !string.IsNullOrWhiteSpace(item.AnsweColoumn))
                            {
                                tbl_MatchContentQuestionMaster MA = new tbl_MatchContentQuestionMaster();
                                MA.Ques_fkid = Maxid;
                                MA.FirstColoumn = item.FirstColoumn;
                                MA.AnsweColoumn = item.AnsweColoumn;
                                MA.LastDatetime = DateTime.Now;
                                _Matc.Add(MA);
                            }
                        }
                    }
                    else if (model.Questtype_fkid == 6)
                    {
                        tbl_AnswerMaster ANS = new tbl_AnswerMaster();
                        ANS.Ques_fkid = _question.GetAll().Max(x => x.pkid);
                        ANS.Questtype_fkid = model.Questtype_fkid;
                        var httpRequest = System.Web.HttpContext.Current.Request;
                        if (httpRequest.Files.Count > 0)
                        {
                            var docfiles = new List<string>();
                            for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                            {
                                string path = "";
                                var postedFile = httpRequest.Files[i];
                                if (i == 0)
                                {
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer1 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 1)
                                {
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer2 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 2)
                                {
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer3 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 3)
                                {
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer4 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                            }
                        }
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
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_QuestionMaster abc = _question.Get(_id);
                    abc.subjecttype_fkid = model.Questtype_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.Question = model.Question;
                    abc.Explaination = model.Explaination;
                    abc.Subject_fkid = model.Subject_fkid;
                    abc.Division_fkid = model.Division_fkid;
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.Marks = model.Marks;
                    abc.Status = model.Status;
                    abc.Adddate = model.Adddate;
                    abc.lastModified = DateTime.Now;
                    _question.Update(abc);

                    if (model.Questtype_fkid == 5)
                    {
                        foreach (var item in model.MATContent)
                        {

                            if (!string.IsNullOrWhiteSpace(item.FirstColoumn) && !string.IsNullOrWhiteSpace(item.OppositeColoumn) && !string.IsNullOrWhiteSpace(item.AnsweColoumn))
                            {
                                tbl_MatchContentQuestionMaster MA = _Matc.Get(item.pkid);
                                MA.FirstColoumn = item.FirstColoumn;
                                MA.OppositeColoumn = item.OppositeColoumn;
                                MA.AnsweColoumn = item.AnsweColoumn;
                                MA.LastDatetime = DateTime.Now;
                                _Matc.Update(MA);
                            }
                        }
                    }
                    else if (model.Questtype_fkid == 7)
                    {
                        foreach (var item in model.FULLF)
                        {

                            if (!string.IsNullOrWhiteSpace(item.FirstColoumn) && !string.IsNullOrWhiteSpace(item.AnsweColoumn))
                            {
                                tbl_MatchContentQuestionMaster MA = _Matc.Get(item.pkid);
                                MA.FirstColoumn = item.FirstColoumn;
                                MA.AnsweColoumn = item.AnsweColoumn;
                                MA.LastDatetime = DateTime.Now;
                                _Matc.Update(MA);
                            }
                        }
                    }
                    else if (model.Questtype_fkid == 6)
                    {
                        tbl_AnswerMaster ANS = _answer.GetAll().Where(x=>x.Ques_fkid == _id).FirstOrDefault();
                        ANS.Ques_fkid = _id;
                        ANS.Questtype_fkid = model.Questtype_fkid;
                        var httpRequest = System.Web.HttpContext.Current.Request;
                        if (httpRequest.Files.Count > 0)
                        {
                            var docfiles = new List<string>();
                            for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                            {
                                string path = "";
                                var postedFile = httpRequest.Files[i];
                                if (i == 0)
                                {
                                    if (System.IO.File.Exists(ANS.Answer1))
                                    {
                                        System.IO.File.Delete(ANS.Answer1);
                                    }
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer1 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 1)
                                {
                                    if (System.IO.File.Exists(ANS.Answer2))
                                    {
                                        System.IO.File.Delete(ANS.Answer2);
                                    }
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer2 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 2)
                                {
                                    if (System.IO.File.Exists(ANS.Answer3))
                                    {
                                        System.IO.File.Delete(ANS.Answer3);
                                    }
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer3 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                                else if (i == 3)
                                {
                                    if (System.IO.File.Exists(ANS.Answer4))
                                    {
                                        System.IO.File.Delete(ANS.Answer4);
                                    }
                                    DateTime date = DateTime.Now;
                                    string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                                    ANS.Answer4 = "/UploadFiles/QuestionImage/" + dates + postedFile.FileName;
                                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/UploadFiles/QuestionImage/"), dates + postedFile.FileName);
                                    postedFile.SaveAs(path);
                                }
                            }
                        }
                        ANS.CorrectAnswer = model.CorrectAnswerDD;
                        ANS.BlanckSpace = model.BlanckSpace;
                        ANS.TrueFalse = model.TrueFalse;
                        ANS.SubAnswer = model.SubAnswer;
                        ANS.status = 1;
                        ANS.Adddate = DateTime.Now;
                        ANS.lastmodified = DateTime.Now;
                        _answer.Update(ANS);
                        exception = "Save Successfully";
                    }
                    else
                    {
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
                    }
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
                             subject = (a.Subject_fkid != null ? _course.Get(a.Subject_fkid).CourseName : ""),
                             div = (a.Division_fkid != null ? _division.Get(a.Division_fkid).DivisioName : "")
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower()) || x.subject.ToLower().Contains(search.ToLower()) || x.div.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult UploadExcel(string Exception)
        {
            ViewBag.QueTypeList = new SelectList(_subtype.GetAll().Where(x => x.pkid != 4).ToList(), "pkid", "Questiontype");
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
            ViewBag.Exception = Exception;
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel(QuestionMaster model)
        {
            ViewBag.QueTypeList = new SelectList(_subtype.GetAll().Where(x => x.pkid != 4).ToList(), "pkid", "Questiontype");
            ViewBag.CourseList = new SelectList(_course.GetAll(), "pkid", "CourseName");
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
                        if (model.Questtype_fkid == 5)
                        {
                            if (FileUpload.FileName == "Match_Content5.xlsx")
                            {
                                tbl_QuestionMaster QUESTION = new tbl_QuestionMaster();


                                var q = dr[1].ToString();
                                var A = dr[2].ToString();
                                QUESTION.subjecttype_fkid = Convert.ToInt32(dr[0]);
                                QUESTION.Question = dr[1].ToString();
                                QUESTION.Explaination = dr[2].ToString();
                                QUESTION.Subject_fkid = Convert.ToInt32(dr[3]);
                                QUESTION.Division_fkid = Convert.ToInt32(dr[4]);
                                QUESTION.hint = dr[5].ToString();
                                QUESTION.Marks = Convert.ToDecimal(dr[6]);
                                try { QUESTION.NegativeMarks = Convert.ToDecimal(dr[7]); }
                                catch { QUESTION.NegativeMarks = 0; }

                                QUESTION.SelectLevel = Convert.ToInt32(dr[8]);
                                QUESTION.Adddate = DateTime.Now;
                                QUESTION.Status = 1;
                                QUESTION.lastModified = DateTime.Now;
                                _question.Add(QUESTION);

                                int _Maxid = _question.GetAll().Max(x => x.pkid);
                                for (int p = 1; p <= 10; p++)
                                {
                                    tbl_MatchContentQuestionMaster ANSWER = new tbl_MatchContentQuestionMaster();
                                    int AA = 0; int BB = 0; int CC = 0;
                                    if (p == 1) { AA = 9; BB = 10; CC = 11; }
                                    else if (p == 2) { AA = 12; BB = 13; CC = 14; }
                                    else if (p == 3) { AA = 15; BB = 16; CC = 17; }
                                    else if (p == 4) { AA = 18; BB = 19; CC = 20; }
                                    else if (p == 5) { AA = 21; BB = 22; CC = 23; }
                                    else if (p == 6) { AA = 24; BB = 25; CC = 26; }
                                    else if (p == 7) { AA = 27; BB = 28; CC = 29; }
                                    else if (p == 8) { AA = 30; BB = 31; CC = 32; }
                                    else if (p == 9) { AA = 33; BB = 34; CC = 35; }
                                    else { AA = 36; BB = 37; CC = 38; }

                                    ANSWER.Ques_fkid = _Maxid;
                                    ANSWER.FirstColoumn = dr[AA].ToString();
                                    ANSWER.OppositeColoumn = dr[BB].ToString();
                                    ANSWER.AnsweColoumn = dr[CC].ToString();
                                    ANSWER.LastDatetime = DateTime.Now;
                                    if (!string.IsNullOrWhiteSpace(ANSWER.FirstColoumn) && !string.IsNullOrWhiteSpace(ANSWER.OppositeColoumn) && !string.IsNullOrWhiteSpace(ANSWER.AnsweColoumn))
                                    {
                                        _Matc.Add(ANSWER);
                                    }
                                }

                            }
                            else
                            {
                                exception = "Select Correct File.";
                                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
                            }
                        }
                        else if (model.Questtype_fkid == 7)
                        {
                            if (FileUpload.FileName == "Full_Form7.xlsx")
                            {
                                tbl_QuestionMaster QUESTION = new tbl_QuestionMaster();

                                var q = dr[1].ToString();
                                var A = dr[2].ToString();
                                QUESTION.subjecttype_fkid = Convert.ToInt32(dr[0]);
                                QUESTION.Question = dr[1].ToString();
                                QUESTION.Explaination = dr[2].ToString();
                                QUESTION.Subject_fkid = Convert.ToInt32(dr[3]);
                                QUESTION.Division_fkid = Convert.ToInt32(dr[4]);
                                QUESTION.hint = dr[5].ToString();
                                QUESTION.Marks = Convert.ToDecimal(dr[6]);
                                try { QUESTION.NegativeMarks = Convert.ToDecimal(dr[7]); }
                                catch { QUESTION.NegativeMarks = 0; }

                                QUESTION.SelectLevel = Convert.ToInt32(dr[8]);
                                QUESTION.Adddate = DateTime.Now;
                                QUESTION.Status = 1;
                                QUESTION.lastModified = DateTime.Now;
                                _question.Add(QUESTION);

                                int _Maxid = _question.GetAll().Max(x => x.pkid);
                                for (int p = 1; p <= 10; p++)
                                {
                                    tbl_MatchContentQuestionMaster ANSWER = new tbl_MatchContentQuestionMaster();
                                    int AA = 0; int CC = 0;
                                    if (p == 1) { AA = 9; CC = 10; }
                                    else if (p == 2) { AA = 11; CC = 12; }
                                    else if (p == 3) { AA = 13; CC = 14; }
                                    else if (p == 4) { AA = 15; CC = 16; }
                                    else if (p == 5) { AA = 17; CC = 18; }
                                    else if (p == 6) { AA = 19; CC = 20; }
                                    else if (p == 7) { AA = 21; CC = 22; }
                                    else if (p == 8) { AA = 23; CC = 24; }
                                    else if (p == 9) { AA = 25; CC = 26; }
                                    else { AA = 27; CC = 28; }


                                    ANSWER.Ques_fkid = _Maxid;
                                    ANSWER.FirstColoumn = dr[AA].ToString();
                                    ANSWER.AnsweColoumn = dr[CC].ToString();
                                    ANSWER.LastDatetime = DateTime.Now;
                                    if (!string.IsNullOrWhiteSpace(ANSWER.FirstColoumn) && !string.IsNullOrWhiteSpace(ANSWER.AnsweColoumn))
                                    {
                                        _Matc.Add(ANSWER);
                                    }
                                }
                            }
                            else
                            {
                                exception = "Select Correct File.";
                                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
                            }
                        }
                        else
                        {
                            if (FileUpload.FileName == "QuestionAnswer.xlsx")
                            {
                                tbl_QuestionMaster QUESTION = new tbl_QuestionMaster();
                                tbl_AnswerMaster ANSWER = new tbl_AnswerMaster();

                                var q = dr[1].ToString();
                                var A = dr[2].ToString();
                                QUESTION.subjecttype_fkid = Convert.ToInt32(dr[0]);
                                QUESTION.Question = dr[1].ToString();
                                QUESTION.Explaination = dr[2].ToString();
                                QUESTION.Subject_fkid = Convert.ToInt32(dr[3]);
                                QUESTION.Division_fkid = Convert.ToInt32(dr[4]);
                                QUESTION.hint = dr[5].ToString();
                                QUESTION.Marks = Convert.ToDecimal(dr[6]);
                                try { QUESTION.NegativeMarks = Convert.ToDecimal(dr[7]); }
                                catch { QUESTION.NegativeMarks = 0; }

                                QUESTION.SelectLevel = Convert.ToInt32(dr[8]);
                                QUESTION.Adddate = DateTime.Now;
                                QUESTION.Status = 1;
                                QUESTION.lastModified = DateTime.Now;
                                _question.Add(QUESTION);
                                ANSWER.Ques_fkid = _question.GetAll().Max(x => x.pkid);
                                ANSWER.Questtype_fkid = Convert.ToInt32(dr[0]);
                                ANSWER.Answer1 = dr[9].ToString();
                                ANSWER.Answer2 = dr[10].ToString();
                                ANSWER.Answer3 = dr[11].ToString();
                                ANSWER.Answer4 = dr[12].ToString();
                                try { ANSWER.CorrectAnswer = Convert.ToInt32(dr[13]); }
                                catch { }

                                ANSWER.BlanckSpace = dr[14].ToString();
                                try { ANSWER.TrueFalse = Convert.ToBoolean(dr[15]); }
                                catch { }
                                ANSWER.Adddate = DateTime.Now;
                                ANSWER.status = 1;
                                ANSWER.lastmodified = DateTime.Now;
                                _answer.Add(ANSWER);
                            }
                            else
                            {
                                exception = "Select Correct File.";
                                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
                            }
                        }
                    }
                    exception = "All Data Uploaded Successfully";
                }
                else
                {
                    exception = "Please select File.";
                }

                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                exception = e.Message;
                return RedirectToAction("UploadExcel", "SubjectGroup", new { Exception = exception });
            }
        }

        public FileResult DownloadExcel(int p)
        {
            string path = "";
            if (p == 1)
            {
                path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/SampleFiles/QuestionAnswer.xlsx"));
            }
            else
            {
                path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/SampleFiles/QuestionAnswer"));
            }
            return File(path, "application/vnd.ms-excel", "Users.xlsx");
        }
    }
}