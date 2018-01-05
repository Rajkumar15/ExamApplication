using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam_Application.Controllers
{
   // [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Session["Menu"] = 1.ToString();
                return View();
            }
             return RedirectToAction("FirstPage", "Admin_Task");
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}