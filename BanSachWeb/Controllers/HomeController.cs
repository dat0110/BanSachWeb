using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ban_sach.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MainContent()
        {
            ViewBag.Message = "MainContent";
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}