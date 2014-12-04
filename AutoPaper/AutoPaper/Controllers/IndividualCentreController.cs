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
    public class IndividualCentreController : Controller
    {
        private PaperShareDBContext db = new PaperShareDBContext();

        public ActionResult Default()
        {
            return View();
        }
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult MyPapers()
        {
            return View();
        }
        public ActionResult MyCollectQuestion()
        {
            return View();
        }
        public ActionResult MyCollectPapers()
        {
            return View();
        }
        public ActionResult MyErrorsFound()
        {
            return View();
        }
    }
}