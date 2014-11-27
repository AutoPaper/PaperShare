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

        public ActionResult Default(string id = "")
        {
            var choiceQuestion = new List<question>();
            var blankQuestion = new List<question>();
            var judgeQuestion = new List<question>();
            var integratedQuestion = new List<question>();

            if (Request.IsAjaxRequest())
            {
                if (id == "search")
                {
                    choiceQuestion = ((from o in db.question_table
                                       where o.questionType == 1
                                       orderby o.citation
                                       select o).Take(10)).ToList<question>();
                    ViewBag.choiceQuestion = choiceQuestion;
                    blankQuestion = ((from o in db.question_table
                                      where o.questionType == 2
                                      orderby o.citation
                                      select o).Take(10)).ToList<question>();
                    ViewBag.blankQuestion = blankQuestion;
                    judgeQuestion = ((from o in db.question_table
                                      where o.questionType == 3
                                      orderby o.citation
                                      select o).Take(10)).ToList<question>();
                    ViewBag.judgeQuestion = judgeQuestion;
                    integratedQuestion = ((from o in db.question_table
                                           where o.questionType == 4
                                           orderby o.citation
                                           select o).Take(10)).ToList<question>();
                    ViewBag.integratedQuestion = integratedQuestion;
                    return PartialView("_searchPartial");
                }
                else if (id == "upload")
                {
                    return PartialView("_uploadPartial");
                }
                else return View();
            }
            choiceQuestion = ((from o in db.question_table
                               where o.questionType == 1
                               orderby o.citation
                               select o).Take(2)).ToList<question>();
            ViewBag.choiceQuestion = choiceQuestion;
            blankQuestion = ((from o in db.question_table
                              where o.questionType == 2
                              orderby o.citation
                              select o).Take(2)).ToList<question>();
            ViewBag.blankQuestion = blankQuestion;
            judgeQuestion = ((from o in db.question_table
                              where o.questionType == 3
                              orderby o.citation
                              select o).Take(2)).ToList<question>();
            ViewBag.judgeQuestion = judgeQuestion;
            integratedQuestion = ((from o in db.question_table
                                   where o.questionType == 4
                                   orderby o.citation
                                   select o).Take(2)).ToList<question>();
            ViewBag.integratedQuestion = integratedQuestion;
            return View();
        }
    }
}