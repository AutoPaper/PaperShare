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
    public class CreatePaperOrQuestionController : Controller
    {
        private PaperShareDBContext db = new PaperShareDBContext();

        public ActionResult Default()
        {
            var Question = ((from o in db.question_table
                                orderby o.citation
                                select o.content).Take(20)).ToList<string>();
            ViewBag.Question = Question;
            if (Request.IsAjaxRequest())
                return PartialView("_searchQuestionPartial");
            return View();   
        }
        public ActionResult searchQuestion()
        {

            return PartialView("_searchQuestionPartial");
        }
    }
}