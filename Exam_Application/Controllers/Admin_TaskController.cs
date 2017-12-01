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
    public class Admin_TaskController : Controller
    {
        // GET: Admin_Task
          public readonly IRepository<tbl_User_Profile> _user;
          public Admin_TaskController(IRepository<tbl_User_Profile> user)
        {
            _user = user;
        }

        [HttpGet]
        public ActionResult SaveCustomer()
        {
            string userid = "";
            try
            {
                userid = TempData["id"].ToString();
                string ro = TempData["role"].ToString();
                string Uname = null;
                try { Uname = TempData["username"].ToString(); }
                catch { }
                string email = null;
                try { email = TempData["ema"].ToString(); }
                catch { }
                string pas = TempData["passwo"].ToString();
                string funame = null;
                try { funame = TempData["fname"].ToString(); }
                catch { }
                string mobino = null;
                try {mobino= TempData["mobi"].ToString(); }
                catch { }

                tbl_User_Profile abc = new tbl_User_Profile();
                abc.User_fkid = userid;
                abc.UserName = Uname;
                abc.RoleName = ro;
                abc.EmailAddress = email;
                abc.FirstName = funame;
                abc.MobileNumber = mobino;
                abc.RegisteredDate = DateTime.Now;
                abc.LastModifiedDate = DateTime.Now;
                _user.Add(abc);
                return RedirectToAction("AdminUserRegister", "Account");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = e.Message});
            }
        }

        public ActionResult GetUserList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
           // string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _user.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             fullname = a.FirstName,
                             email = a.EmailAddress,
                             role = a.RoleName,
                             userid = a.User_fkid,                            
                             mob = a.MobileNumber,
                             un = a.UserName,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.un.Contains(search) || x.fullname.ToLower().Contains(search.ToLower())) select b);                  
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
        public ActionResult EditUserDetails(int id)
        {
            var data = _user.Get(id);
            tbl_User_Profiless abc = new tbl_User_Profiless();
            abc.pkid = data.pkid;
            abc.FirstName = data.FirstName;
            abc.UserName = data.UserName;
            abc.AddressLine1 = data.AddressLine1;
            abc.AddressLine2 = data.AddressLine2;
            abc.MobileNumber = data.MobileNumber;
            abc.AddressLine1 = data.AddressLine1;
            abc.RoleName = data.RoleName;
            abc.EmailAddress = data.EmailAddress;
            abc.LastModifiedDate = DateTime.Now;
            return PartialView(abc);
        }

        [HttpPost]
        public ActionResult EditUserDetails(tbl_User_Profiless model)
        {
            string exception = "";
            try
            {
                if (!string.IsNullOrEmpty(model.EmailAddress))
                {
                    int mobdt = _user.GetAll().Where(x => x.EmailAddress == model.EmailAddress && x.pkid != model.pkid).Count();
                    if (mobdt > 0)
                    {
                        exception = "User Email Already Taken.";
                        return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
                    }
                }
                if (!string.IsNullOrEmpty(model.MobileNumber))
                {
                    int mobdt = _user.GetAll().Where(x => x.MobileNumber == model.MobileNumber && x.pkid != model.pkid).Count();
                    if (mobdt > 0)
                    {
                        exception = "User Mobile Number Already Taken.";
                        return RedirectToAction("AdminUserRegister", "Account", new { Exceptionmsg = exception });
                    }
                }
                tbl_User_Profile abc = _user.Get(model.pkid);
                abc.FirstName = model.FirstName;
                abc.UserName = model.UserName;
                abc.MobileNumber = model.MobileNumber;
                abc.AddressLine1 = model.AddressLine1;
                abc.RoleName = model.RoleName;
                abc.AddressLine1 = model.AddressLine1;
                abc.AddressLine2 = model.AddressLine2;
                abc.EmailAddress = model.EmailAddress;
                abc.LastModifiedDate = DateTime.Now;
                _user.Update(abc);
                return RedirectToAction("AdminUserRegister", "Account");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return RedirectToAction("AdminUserRegister", "Account");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            string id = User.Identity.Name;
            var data = _user.GetAll().Where(x => x.UserName == id).FirstOrDefault();
            tbl_User_Profiless abc = new tbl_User_Profiless();           
            abc.FirstName = data.FirstName;
            abc.UserName = data.UserName;
            abc.AddressLine1 = data.AddressLine1;
            abc.AddressLine2 = data.AddressLine2;
            abc.MobileNumber = data.MobileNumber;
            abc.AddressLine1 = data.AddressLine1;
            abc.RoleName = data.RoleName;
            abc.EmailAddress = data.EmailAddress;
            abc.RegisteredDate = data.RegisteredDate;     
            return View(abc);
        }

        [HttpGet]
        public ActionResult FirstPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminDashoard()
        {
            return View();
        }
    }
}