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

        public ActionResult Paper(int id = 0)
        {
            //试卷信息
            var paperName = (from o in db.paper_table
                             where o.ID == id
                             select o.name).ToList<string>();
            ViewBag.paperName = paperName;

            var teacher = (from c in db.user_table
                           where (from o in db.PT_table
                                  where o.paperID == id
                                  select o.teacherID).Contains(c.ID)
                           select c.name).ToList<string>();
            ViewBag.teacher = teacher;

            var paperTags = (from c in db.tag_table
                             where (from o in db.PTags_table
                                    where o.paperID == id
                                    select o.tagID).Contains(c.ID)
                             select c.content).ToList<string>();
            ViewBag.paperTags = paperTags;
            //质量及难度评价
            var quality = (from o in db.paper_table
                           where o.ID == id
                           select o.quality).ToList<float>();
            ViewBag.quality = quality;

            var qualityVotes = (from o in db.paper_table
                                where o.ID == id
                                select o.qualityVotes).ToList<int>();
            ViewBag.qualityVotes = qualityVotes;

            var difficulty = (from o in db.paper_table
                              where o.ID == id
                              select o.difficulty).ToList<float>();
            ViewBag.difficulty = difficulty;

            var difficultyVotes = (from o in db.paper_table
                                   where o.ID == id
                                   select o.difficultyVotes).ToList<int>();
            ViewBag.difficultyVotes = difficultyVotes;
            //评论部分
            var comment = (from o in db.PU_Transcation_table
                           where o.paperID == id && o.comment != null
                           orderby o.commentTime descending
                           select o).ToList<PU_Transcation>();
            ViewBag.comment = comment;
            //题目部分
            var choiceQuestion = (from c in db.question_table
                                  where (from o in db.PQ_table
                                         where o.paperID == id
                                         select o.questionID).Contains(c.ID)
                                         && c.questionType == 1
                                  orderby c.citationCount
                                  select c).ToList<question>();
            ViewBag.choiceQuestion = choiceQuestion;
            var blankQuestion = (from c in db.question_table
                                 where (from o in db.PQ_table
                                        where o.paperID == id
                                        select o.questionID).Contains(c.ID)
                                         && c.questionType == 2
                                 orderby c.citationCount
                                 select c).ToList<question>();
            ViewBag.blankQuestion = blankQuestion;
            var judgeQuestion = (from c in db.question_table
                                 where (from o in db.PQ_table
                                        where o.paperID == id
                                        select o.questionID).Contains(c.ID)
                                         && c.questionType == 3
                                 orderby c.citationCount
                                 select c).ToList<question>();
            ViewBag.judgeQuestion = judgeQuestion;
            var integratedQuestion = (from c in db.question_table
                                      where (from o in db.PQ_table
                                             where o.paperID == id
                                             select o.questionID).Contains(c.ID)
                                             && c.questionType == 4
                                      orderby c.citationCount
                                      select c).ToList<question>();
            ViewBag.integratedQuestion = integratedQuestion;
            return View();
        }

        public bool qualityVotes(int id, int vote)
        {
            papers p = db.paper_table.First(c => c.ID == id);
            p.quality = (p.qualityVotes * p.quality + vote) / (++p.qualityVotes);
            db.SaveChanges();
            return true;
        }

        public bool difficultyVotes(int id, int vote)
        {
            papers p = db.paper_table.First(c => c.ID == id);
            p.difficulty = (p.difficultyVotes * p.difficulty + vote) / (++p.difficultyVotes);
            db.SaveChanges();
            return true;
        }

        [HttpPost]
        public ActionResult Comment(int id)
        {
            if (Request.Cookies["userID"] != null)
            {
                var user_id = int.Parse(Request.Cookies["userID"].Value);
                var user_type = int.Parse(Request.Cookies["userType"].Value);
                var pu = (from o in db.PU_table
                          where o.paperID == id && o.userID == user_id
                          select o);
                var comment = (from o in db.PU_Transcation_table
                               where o.paperID == id && o.userID == user_id
                               select o.comment);
                var logAddr = (from o in db.PU_Transcation_table
                               where o.paperID == id && o.userID == user_id
                               select o.logAddr);
                if (user_type == 1)  //当前用户为老师
                {
                    var CMTcontent = Request.Form["comment_content"];
                    if (pu == null)
                    {
                        PU_Transcation p = new PU_Transcation();
                        p.userID = user_id;
                        p.paperID = id;
                        p.comment = CMTcontent;
                        p.commentTime = DateTime.Now;
                        db.PU_Transcation_table.Add(p);
                        db.SaveChanges();
                    }
                }
                else if (user_type == 2)
                {
                    if (logAddr != null && comment == null)
                    {
                        var CMTcontent = Request.Form["comment_content"];
                        PU_Transcation p = db.PU_Transcation_table.First(c => c.paperID == id && c.userID == user_id);
                        p.comment = CMTcontent;
                        p.commentTime = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Paper");
        }
        [HttpPost]
        public bool hasSHA()
        {
            return true;
        }
    }
}