using DataLayer;
using DataLayer.ExamModel;
using Exam_Application.Models.DAL;
using itechDll;
using System;
using System.Collections.Generic;
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
        public readonly IRepository<tbl_SubjectType_Master> _subtype;
        public readonly IRepository<tbl_StudentMaster> _student;
        public readonly IRepository<tbl_Stude_subjectTypeList> _subtypeList;
        public readonly IRepository<tbl_QuestionMaster> _question;

        public SubjectGroupController(IRepository<tbl_User_Profile> user, IRepository<tbl_GroupMaster> group, IRepository<tbl_Subject_master> subject,
            IRepository<tbl_SubjectType_Master> subtype, IRepository<tbl_StudentMaster> student, IRepository<tbl_Stude_subjectTypeList> subtypeList, IRepository<tbl_QuestionMaster> question)
        {
            _user = user;
            _group = group;
            _subject = subject;
            _subtype = subtype;
            _student = student;
            _subtypeList = subtypeList;
            _question = question;
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
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
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
                return RedirectToAction("SubjectMaster", "SubjectGroup", new { Exception =exception});
            }
        }

        [HttpGet]
        public ActionResult AddQuestion(string id,string Exception)
        {
            ViewBag.SubjectList = new SelectList(_subject.GetAll(), "pkid", "subjectName");
            ViewBag.SubjectTypeList = new SelectList(_subtype.GetAll(), "pkid", "subjecttype");
            ViewBag.Exception = Exception;
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_QuestionMaster model = _question.Get(_id);
                tbl_QuestionMasterss abc = new tbl_QuestionMasterss();
                abc.pkid = model.pkid;
                abc.subjecttype_fkid = model.subjecttype_fkid;
                abc.Question = model.Question;
                abc.Explaination = model.Explaination;
                abc.Subject_fkid = Convert.ToInt32(model.Subject_fkid);
                abc.hint = model.hint;
                abc.SelectLevel = model.SelectLevel;
                abc.NegativeMarks = model.NegativeMarks;
                abc.Marks = model.Marks;
                abc.Status = model.Status;
                abc.Adddate = model.Adddate;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddQuestion(tbl_QuestionMasterss model)
        {
            string exception = "";
            try
            {
                if (model.pkid == 0)
                {
                    tbl_QuestionMaster abc = new tbl_QuestionMaster();
                    abc.pkid = model.pkid;
                    abc.subjecttype_fkid = model.subjecttype_fkid;
                    abc.Question = model.Question;
                    abc.Explaination = model.Explaination;
                    abc.Subject_fkid = model.Subject_fkid.ToString();
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.Marks = model.Marks;
                    abc.Status = model.Status;
                    abc.Adddate =DateTime.Now;
                    abc.lastModified = DateTime.Now;
                    _question.Add(abc);
                    exception = "Save Successfully";
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_QuestionMaster abc = _question.Get(_id);
                    abc.pkid = model.pkid;
                    abc.subjecttype_fkid = model.subjecttype_fkid;
                    abc.Question = model.Question;
                    abc.Explaination = model.Explaination;
                    abc.Subject_fkid = model.Subject_fkid.ToString();
                    abc.hint = model.hint;
                    abc.SelectLevel = model.SelectLevel;
                    abc.NegativeMarks = model.NegativeMarks;
                    abc.Marks = model.Marks;
                    abc.Status = model.Status;                  
                    abc.lastModified = DateTime.Now;
                    _question.Update(abc);
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
        public ActionResult QuestionList()
        {
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
        public ActionResult AddStudent(tbl_StudentMasterss model)
        {
            string exception = "";
            try
            {                
                if (String.IsNullOrWhiteSpace(model.user_fkid))
                {
                    exception = "Enter First EMail or Mobile And Password";
                    return RedirectToAction("StudentMaster", "SubjectGroup", new { Exception = exception });
                }
                if (model.pkid == 0)
                {
                    tbl_StudentMaster abc = new tbl_StudentMaster();
                    abc.pkid = model.pkid;                  
                    abc.Email = model.Email;
                    abc.Name = model.Name;
                    abc.user_fkid = model.user_fkid;
                    abc.mobileno = model.mobileno;                   
                    abc.Address = model.Address;
                    abc.Enrollnmentno = model.Enrollnmentno;
                    abc.GuardianContact = model.GuardianContact;
                    abc.status = model.status;
                    abc.Adddate = DateTime.Now;
                    abc.lastmodifieddate = DateTime.Now;
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
    }
}