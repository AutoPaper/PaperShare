using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPaper.Models;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;

namespace AutoPaper.Controllers
{

    public class CreatePaperOrQuestionController : Controller
    {
        static bool uploadDone = false;
        private PaperShareDBContext db = new PaperShareDBContext();

        public class Question
        {
            public int ID { get; set; }
            public int teacherID { get; set; }
            public string subject { get; set; }
            public string content { get; set; }
            public string answer { get; set; }
            public int questionType { get; set; }
            public int difficulty { get; set; }
            public int citationCount { get; set; }
            public DateTime updateTime { get; set; }
            public string teacherName { get; set; }
        }

        public ActionResult Default()
        {
            var t_choiceQuestion = (from a in db.question_table
                                    join b in db.user_table
                                    on a.teacherID equals b.ID
                                    where a.questionType == 1 && a.citationCount != -1
                                    orderby a.citationCount
                                    select new { a, b.name }).Take(2).ToList();
            List<Question> choiceQuestion = new List<Question>();
            foreach (var t_q in t_choiceQuestion)
            {
                Question t_qestion = new Question();
                t_qestion.ID = t_q.a.ID;
                t_qestion.teacherID = t_q.a.teacherID;
                t_qestion.subject = t_q.a.subject;
                t_qestion.content = t_q.a.content;
                t_qestion.answer = t_q.a.answer;
                t_qestion.questionType = t_q.a.questionType;
                t_qestion.difficulty = t_q.a.difficulty;
                t_qestion.citationCount = t_q.a.citationCount;
                t_qestion.updateTime = t_q.a.updateTime;
                t_qestion.teacherName = t_q.name;
                choiceQuestion.Add(t_qestion);
            }
            ViewBag.choiceQuestion = choiceQuestion;
            var t_blankQuestion = (from a in db.question_table
                                   join b in db.user_table
                                   on a.teacherID equals b.ID
                                   where a.questionType == 2 && a.citationCount != -1
                                   orderby a.citationCount
                                   select new { a, b.name }).Take(2).ToList();
            List<Question> blankQuestion = new List<Question>();
            foreach (var t_q in t_blankQuestion)
            {
                Question t_qestion = new Question();
                t_qestion.ID = t_q.a.ID;
                t_qestion.teacherID = t_q.a.teacherID;
                t_qestion.subject = t_q.a.subject;
                t_qestion.content = t_q.a.content;
                t_qestion.answer = t_q.a.answer;
                t_qestion.questionType = t_q.a.questionType;
                t_qestion.difficulty = t_q.a.difficulty;
                t_qestion.citationCount = t_q.a.citationCount;
                t_qestion.updateTime = t_q.a.updateTime;
                t_qestion.teacherName = t_q.name;
                blankQuestion.Add(t_qestion);
            }
            ViewBag.blankQuestion = blankQuestion;
            var t_judgeQuestion = (from a in db.question_table
                                   join b in db.user_table
                                   on a.teacherID equals b.ID
                                   where a.questionType == 3 && a.citationCount != -1
                                   orderby a.citationCount
                                   select new { a, b.name }).Take(2).ToList();
            List<Question> judgeQuestion = new List<Question>();
            foreach (var t_q in t_judgeQuestion)
            {
                Question t_qestion = new Question();
                t_qestion.ID = t_q.a.ID;
                t_qestion.teacherID = t_q.a.teacherID;
                t_qestion.subject = t_q.a.subject;
                t_qestion.content = t_q.a.content;
                t_qestion.answer = t_q.a.answer;
                t_qestion.questionType = t_q.a.questionType;
                t_qestion.difficulty = t_q.a.difficulty;
                t_qestion.citationCount = t_q.a.citationCount;
                t_qestion.updateTime = t_q.a.updateTime;
                t_qestion.teacherName = t_q.name;
                judgeQuestion.Add(t_qestion);
            }
            ViewBag.judgeQuestion = judgeQuestion;
            var t_integratedQuestion = (from a in db.question_table
                                        join b in db.user_table
                                        on a.teacherID equals b.ID
                                        where a.questionType == 4 && a.citationCount != -1
                                        orderby a.citationCount
                                        select new { a, b.name }).Take(2).ToList();
            List<Question> integratedQuestion = new List<Question>();
            foreach (var t_q in t_integratedQuestion)
            {
                Question t_qestion = new Question();
                t_qestion.ID = t_q.a.ID;
                t_qestion.teacherID = t_q.a.teacherID;
                t_qestion.subject = t_q.a.subject;
                t_qestion.content = t_q.a.content;
                t_qestion.answer = t_q.a.answer;
                t_qestion.questionType = t_q.a.questionType;
                t_qestion.difficulty = t_q.a.difficulty;
                t_qestion.citationCount = t_q.a.citationCount;
                t_qestion.updateTime = t_q.a.updateTime;
                t_qestion.teacherName = t_q.name;
                integratedQuestion.Add(t_qestion);
            }
            ViewBag.integratedQuestion = integratedQuestion;
            ViewBag.isUpload = "false";
            if (uploadDone == false)
                ViewBag.uploadDone = "false";
            else
            {
                uploadDone = false;
                ViewBag.uploadDone = "true";
            }
            return View();
        }

        public ActionResult getQuestion()
        {
            string match = Request.Form["search-input"];
            if (match != "")
            {
                var t_choiceQuestion = (from a in db.question_table
                                        join d in db.user_table
                                        on a.teacherID equals d.ID
                                        where
                                        (from b in db.QTags_table
                                         where
                                         (from c in db.tag_table
                                          where c.content.Contains(match)
                                          select c.ID).Contains(b.tagID)

                                         select b.questionID).Contains(a.ID) && a.questionType == 1 && a.citationCount != -1
                                        orderby a.citationCount

                                        select new { a, d.name }).Take(10).ToList();

                List<Question> choiceQuestion = new List<Question>();
                foreach (var t_q in t_choiceQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    choiceQuestion.Add(t_qestion);
                }
                ViewBag.choiceQuestion = choiceQuestion;

                var t_blankQuestion = (from a in db.question_table
                                       join d in db.user_table
                                       on a.teacherID equals d.ID
                                       where
                                       (from b in db.QTags_table
                                        where
                                        (from c in db.tag_table
                                         where c.content.Contains(match)
                                         select c.ID).Contains(b.tagID)
                                        select b.questionID).Contains(a.ID) && a.questionType == 2 && a.citationCount != -1
                                       orderby a.citationCount
                                       select new { a, d.name }).Take(10).ToList();

                List<Question> blankQuestion = new List<Question>();
                foreach (var t_q in t_blankQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    blankQuestion.Add(t_qestion);
                }
                ViewBag.blankQuestion = blankQuestion;
                var t_judgeQuestion = (from a in db.question_table
                                       join d in db.user_table
                                       on a.teacherID equals d.ID
                                       where
                                       (from b in db.QTags_table
                                        where
                                        (from c in db.tag_table
                                         where c.content.Contains(match)
                                         select c.ID).Contains(b.tagID)
                                        select b.questionID).Contains(a.ID) && a.questionType == 3 && a.citationCount != -1
                                       orderby a.citationCount
                                       select new { a, d.name }).Take(10).ToList();

                List<Question> judgeQuestion = new List<Question>();
                foreach (var t_q in t_judgeQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    judgeQuestion.Add(t_qestion);
                }
                ViewBag.judgeQuestion = judgeQuestion;
                var t_integratedQuestion = (from a in db.question_table
                                            join d in db.user_table
                                            on a.teacherID equals d.ID
                                            where
                                            (from b in db.QTags_table
                                             where
                                             (from c in db.tag_table
                                              where c.content.Contains(match)
                                              select c.ID).Contains(b.tagID)
                                             select b.questionID).Contains(a.ID) && a.questionType == 4 && a.citationCount != -1
                                            orderby a.citationCount
                                            select new { a, d.name }).Take(10).ToList();

                List<Question> integratedQuestion = new List<Question>();
                foreach (var t_q in t_integratedQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    integratedQuestion.Add(t_qestion);
                }
                ViewBag.integratedQuestion = integratedQuestion;
            }
            else
            {
                var t_choiceQuestion = (from a in db.question_table
                                        join b in db.user_table
                                        on a.teacherID equals b.ID
                                        where a.questionType == 1 && a.citationCount != -1
                                        orderby a.citationCount
                                        select new { a, b.name }).Take(2).ToList();
                List<Question> choiceQuestion = new List<Question>();
                foreach (var t_q in t_choiceQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    choiceQuestion.Add(t_qestion);
                }
                ViewBag.choiceQuestion = choiceQuestion;
                var t_blankQuestion = (from a in db.question_table
                                       join b in db.user_table
                                       on a.teacherID equals b.ID
                                       where a.questionType == 2 && a.citationCount != -1
                                       orderby a.citationCount
                                       select new { a, b.name }).Take(2).ToList();
                List<Question> blankQuestion = new List<Question>();
                foreach (var t_q in t_blankQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    blankQuestion.Add(t_qestion);
                }
                ViewBag.blankQuestion = blankQuestion;
                var t_judgeQuestion = (from a in db.question_table
                                       join b in db.user_table
                                       on a.teacherID equals b.ID
                                       where a.questionType == 3 && a.citationCount != -1
                                       orderby a.citationCount
                                       select new { a, b.name }).Take(2).ToList();
                List<Question> judgeQuestion = new List<Question>();
                foreach (var t_q in t_judgeQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    judgeQuestion.Add(t_qestion);
                }
                ViewBag.judgeQuestion = judgeQuestion;
                var t_integratedQuestion = (from a in db.question_table
                                            join b in db.user_table
                                            on a.teacherID equals b.ID
                                            where a.questionType == 4 && a.citationCount != -1
                                            orderby a.citationCount
                                            select new { a, b.name }).Take(2).ToList();
                List<Question> integratedQuestion = new List<Question>();
                foreach (var t_q in t_integratedQuestion)
                {
                    Question t_qestion = new Question();
                    t_qestion.ID = t_q.a.ID;
                    t_qestion.teacherID = t_q.a.teacherID;
                    t_qestion.subject = t_q.a.subject;
                    t_qestion.content = t_q.a.content;
                    t_qestion.answer = t_q.a.answer;
                    t_qestion.questionType = t_q.a.questionType;
                    t_qestion.difficulty = t_q.a.difficulty;
                    t_qestion.citationCount = t_q.a.citationCount;
                    t_qestion.updateTime = t_q.a.updateTime;
                    t_qestion.teacherName = t_q.name;
                    integratedQuestion.Add(t_qestion);
                }
                ViewBag.integratedQuestion = integratedQuestion;

            }
            return PartialView("_searchPartial");
        }

        public string getAnswer(int id)//答案
        {
            var answer = ((from o in db.question_table
                           where o.ID == id && o.citationCount != -1
                           select o.answer)).ToArray<string>();
            if (answer.Length > 0)
                return answer[0];
            else
                return "未找到答案";
        }

        public JsonResult findError(int id)//挑错
        {
            //获得该题上传者id
            var teacherId = ((from o in db.question_table
                              where o.ID == id
                              select o.teacherID)).ToArray<int>();
            int t_teacherID = teacherId[0];
            string message = "";
            //获得纠错用户id
            int fromID = Convert.ToInt32(Request.Cookies["userID"].Value);
            var isExist = (from o in db.error_table
                           where o.fromID == fromID && o.toID == t_teacherID && o.questionID == id
                           select o).ToList<errorHistory>();
            if (isExist.Count > 0)//记录已存在
            {
                message = "你已报过错,无法再次对同一题报错";
                return Json(message);
            }
            //查找user_table表，找到该上传者
            if (teacherId.Length > 0)
            {

                var teacherName = ((from o in db.user_table
                                    where o.ID == t_teacherID
                                    select o.name)).ToArray<string>();
                if (teacherName.Length > 0)
                    message = "已将您的消息发送给本题目的上传者" + teacherName[0];
                else
                {
                    message = "未找到本题上传者";
                    return Json(message);
                }
            }
            else
            {
                message = "未找到本题上传者";
                return Json(message);
            }

            var t_content = Request.Form["errorContent"];
            //更新数据库
            //errorHistory
            errorHistory t_errorHistory = new errorHistory();
            t_errorHistory.fromID = fromID;
            t_errorHistory.toID = teacherId[0];
            t_errorHistory.questionID = id;
            t_errorHistory.content = t_content;
            db.error_table.Add(t_errorHistory);
            //notices
            notices t_notices = new notices();
            t_notices.userID = teacherId[0];
            t_notices.noticeType = 0;
            t_notices.content = fromID.ToString() + t_content;
            db.notice_table.Add(t_notices);
            db.SaveChanges();

            //返回结果
            return Json(message);
        }

        public ActionResult GetMyPaperList()
        {
            int userID = Convert.ToInt32(Request.Cookies["userID"].Value);
            var paperList = (from a in db.paper_table
                             join b in db.PT_table
                             on a.ID equals b.paperID
                             where b.teacherID == userID
                             select a).ToList<papers>();
            ViewBag.paperList = paperList;
            return PartialView("*******");
        }

        public bool addToPaper(int id)
        {
            //获得试卷name
            string paperName = Request.Form["*****"];
            //获得试卷id
            var paperId = (from o in db.paper_table
                           where o.name == paperName
                           select o.ID).ToArray<int>();

            if (paperId.Length > 0)
            {
                //更新数据库
                PQ t_PQ = new PQ();
                int t_paperId = paperId[0];
                t_PQ.paperID = t_paperId;
                t_PQ.questionID = id;
                var isExist = (from o in db.PQ_table
                               where o.paperID == t_paperId && o.questionID == id
                               select o).ToList<PQ>();
                if (isExist.Count > 0)  //记录已存在
                    return false;
                db.PQ_table.Add(t_PQ);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool collectQuestion(int id)//收藏试题
        {
            int ID = Convert.ToInt32(Request.Cookies["userID"].Value);
            QU t_pu = new QU();
            t_pu.userID = ID;
            t_pu.questionID = id;
            var isExist = (from o in db.QU_table
                           where o.userID == ID && o.questionID == id
                           select o).ToList<QU>();
            if (isExist.Count > 0)  //记录已存在
                return false;
            db.QU_table.Add(t_pu);
            db.SaveChanges();
            return true;

        }

        public void dealwithDoc()//处理上传的文档，解析并发回前端
        {
            string data = "";

            //        
            //       Word.Document doc = null; //一会要记录word打开的文档
            Stream fileStream = Request.Files["upload-doc"].InputStream;

            //    Stream fileStream = Request.Files[upload].InputStream;

            string fileName = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".") + Path.GetFileName(Request.Files["upload-doc"].FileName);
            int fileLength = Request.Files["upload-doc"].ContentLength;
            //本地存放路径
            string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
            //将文件以文件名filename存放在path路径中
            Request.Files["upload-doc"].SaveAs(Path.Combine(path, fileName));

            //打开本地word
            Word.Application wordapp = new Microsoft.Office.Interop.Word.Application();//可以打开word程序
            object fileobj = Path.Combine(path + fileName);
            object nullobj = System.Reflection.Missing.Value;
            Word.Document doc = wordapp.Documents.Open(ref fileobj, ref nullobj,
                ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
                ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
                ref nullobj, ref nullobj, ref nullobj, ref nullobj);
            data = doc.Content.Text.Replace("\a", "").Replace("\r", "\r\n");
            //从本地读取doc文件

            //           StreamReader sr = new StreamReader(path, Encoding.Unicode);
            //将数据存入data中 

            //      StreamReader sr = new StreamReader(fileStream, System.Text.Encoding.Default);
            //      data = sr.ReadToEnd();


            //        List<question> questionList = new List<question>();
            int start = 0, end = 0;
            int ID = Convert.ToInt32(Request.Cookies["userID"].Value);
            //     Regex r = new Regex("\\r\\n\\d+\\.");
            data = Regex.Replace(data, "\\r\\n\\d+\\.", "@@@");
            start = KMPMatch(data, "@@@", start) + 1;
            while (end < data.Length - 2)
            {
                end = KMPMatch(data, "@@@", start) - 3;
                if (end == -4)
                    end = data.Length - 2;
                question t_question = new question();
                t_question.content = data.Substring(start - 1, end - start + 1);
                t_question.teacherID = ID;
                t_question.citationCount = -1;
                t_question.updateTime = DateTime.Now;
                db.question_table.Add(t_question);
                db.SaveChanges();
                //         questionList.Add(t_question);
                //         uploadNum++;
                start = end + 4;
            }
            // data.Replace(r, "@@@");
            /*
                        start = KMPMatch(data, "@@@", start) + 1;
                        while (end < data.Length)
                        {
                            end = KMPMatch(data, "@@@", start) - 3;
                            if (end == -4)
                                end = data.Length;
                            string t_question = "";
                            t_question = data.Substring(start - 1, end - start + 1);
                            questionList.Add(t_question);
                            uploadNum++;
                            start = end + 4;
                        }*/
            //     ViewBag.questionList = questionList;

            //     return PartialView("_uploadPartial");
        }

        public bool docSave(int uploadNum)//文档保存成功
        {
            uploadDone = true;
            if (uploadNum <= 0)
                return false;
            //获取试卷名
            string paperName = Request.Form["paper-name"];
            papers t_paper = new papers();
            t_paper.name = paperName;
            //加入paper_table
            db.paper_table.Add(t_paper);
            db.SaveChanges();
            int paperId = t_paper.ID;
            int ID = Convert.ToInt32(Request.Cookies["userID"].Value);
            //加入PT_table
            PT t_pt = new PT();
            t_pt.paperID = paperId;
            t_pt.teacherID = ID;
            t_pt.createTime = DateTime.Now;
            t_pt.doneCount = 0;
            db.PT_table.Add(t_pt);
            db.SaveChanges();

            var questionList = (from o in db.question_table
                                where o.teacherID == ID && o.citationCount == -1
                                select o).ToArray<question>();
            for (int i = 0; i < uploadNum; i++)
            {
                questionList[i].subject = Request.Form["subject-" + i.ToString()];
                //questionList[i].content = Request.Form["content-" + i.ToString()];
                questionList[i].answer = Request.Form["answer-" + i.ToString()];
                string str = Request.Form["questionType-" + i.ToString()];
                if (str == "选择题")
                    questionList[i].questionType = 0;
                else if (str == "填空题")
                    questionList[i].questionType = 1;
                else if (str == "判断题")
                    questionList[i].questionType = 2;
                else if (str == "综合题")
                    questionList[i].questionType = 3;
                str = Request.Form["difficulty-" + i.ToString()];
                if (str == "简单")
                    questionList[i].difficulty = 1;
                else if (str == "一般")
                    questionList[i].difficulty = 2;
                else if (str == "困难")
                    questionList[i].difficulty = 3;
                questionList[i].citationCount = 0;
                questionList[i].updateTime = DateTime.Now;
                //更新question_table                
                db.SaveChanges();
                int questionId = questionList[i].ID;
                PQ t_pq = new PQ();
                t_pq.paperID = paperId;
                t_pq.questionID = questionId;
                //加入PQ_table
                db.PQ_table.Add(t_pq);
                db.SaveChanges();
            }
            return true;
        }

        public ActionResult returnDoc()
        {
            int ID = Convert.ToInt32(Request.Cookies["userID"].Value);
            var questionList = (from o in db.question_table
                                where o.teacherID == ID && o.citationCount == -1
                                select o.content).ToList<string>();
            ViewBag.questionList = questionList;
            ViewBag.isUpload = "true";
            return PartialView("_uploadPartial");
        }

        int KMPMatch(string s, string p, int start)
        {
            int[] next = new int[100];
            int i, j;
            i = start;
            j = 0;
            Getnext(p, next);
            while (i < s.Length)
            {
                if (j == -1 || s[i] == p[j])
                {
                    i++;
                    j++;
                }
                else
                {
                    j = next[j];       //消除了指针i的回溯
                }
                if (j == p.Length)
                    return i;       //返回end下标
            }
            return -1;
        }
        void Getnext(string T, int[] next)
        {
            int k = -1;
            int j = 0;
            next[0] = -1;
            while (j < T.Length - 1)
            {
                if (k == -1 || T.Take(j) == T.Take(k))
                {
                    j++;
                    k++;
                    next[j] = k;
                }
                else
                    k = next[k];
            }
        }
    }
}