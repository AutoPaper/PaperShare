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
            var choiceQuestion = ((from o in db.question_table
                               where o.questionType == 1
                               orderby o.citationCount
                               select o).Take(2)).ToList<question>();
            ViewBag.choiceQuestion = choiceQuestion;
            var blankQuestion = ((from o in db.question_table
                              where o.questionType == 2
                              orderby o.citationCount
                              select o).Take(2)).ToList<question>();
            ViewBag.blankQuestion = blankQuestion;
            var judgeQuestion = ((from o in db.question_table
                              where o.questionType == 3
                              orderby o.citationCount
                              select o).Take(2)).ToList<question>();
            ViewBag.judgeQuestion = judgeQuestion;
            var integratedQuestion = ((from o in db.question_table
                                   where o.questionType == 4
                                   orderby o.citationCount
                                   select o).Take(2)).ToList<question>();
            ViewBag.integratedQuestion = integratedQuestion;
            return View();
        }
        public ActionResult getQuestion()
        {
            var choiceQuestion = ((from o in db.question_table
                               where o.questionType == 1
                               orderby o.citationCount
                               select o).Take(10)).ToList<question>();
            ViewBag.choiceQuestion = choiceQuestion;
            var blankQuestion = ((from o in db.question_table
                              where o.questionType == 2
                              orderby o.citationCount
                              select o).Take(10)).ToList<question>();
            ViewBag.blankQuestion = blankQuestion;
            var judgeQuestion = ((from o in db.question_table
                              where o.questionType == 3
                              orderby o.citationCount
                              select o).Take(10)).ToList<question>();
            ViewBag.judgeQuestion = judgeQuestion;
            var integratedQuestion = ((from o in db.question_table
                                   where o.questionType == 4
                                   orderby o.citationCount
                                   select o).Take(10)).ToList<question>();
            ViewBag.integratedQuestion = integratedQuestion;
            return PartialView("_searchPartial");
        }
        public string getAnswer(int id)//答案
        {
            return " ";
        }
        public string findError(int id)//挑错
        {
            return "已将您的消息发送给本题目的上传者" + "";
        }
        public ActionResult dealwithDoc()//处理上传的文档，解析并发回前端
        {
            return PartialView("_uploadPartial");
        }
        public bool docSave()//文档保存成功
        {
            return true;
        }
    }
}