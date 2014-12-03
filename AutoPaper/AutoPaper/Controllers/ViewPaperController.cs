using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPaper.Models;

namespace AutoPaper.Controllers
{
    public class ViewPaperController : Controller
    {
        private PaperShareDBContext db = new PaperShareDBContext();

        public ActionResult Paper(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Comment()
        {
            return RedirectToAction("Paper");
        }
    }
}