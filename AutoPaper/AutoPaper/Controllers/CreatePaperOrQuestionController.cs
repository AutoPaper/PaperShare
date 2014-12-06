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
            var answer = ((from o in db.question_table
                           where o.ID == id
                           select o.answer)).ToArray<string>();
            return answer[0];
        }

        public JsonResult findError(int id)//挑错
        {
            //获得该题上传者id
            var teacherId = ((from o in db.question_table
                              where o.ID == id
                              select o.teacherID)).ToArray<int>();


            //查找user_table表，找到该上传者
            int t_teacherID = teacherId[0];
            var teacherName = ((from o in db.user_table
                                where o.ID == t_teacherID
                                select o.name)).ToArray<string>();

            //获得纠错用户id
            int fromID = Convert.ToInt32(Request.Cookies["userID"].Value);
            var t_content = Request.Form["errorContent"];
            //更新数据库
            errorHistory t_errorHistory = new errorHistory();
            t_errorHistory.fromID = fromID;
            t_errorHistory.toID = teacherId[0];
            t_errorHistory.questionID = id;
            t_errorHistory.content = t_content;
            db.error_table.Add(t_errorHistory);
            db.SaveChanges();

            //返回结果
            return Json("已将您的消息发送给本题目的上传者" + teacherName[0]);
        }

        public bool addToPaper(int id)
        {
            //获得该题上传者id
            var teacherId = ((from o in db.question_table
                              where o.ID == id
                              select o.teacherID)).ToArray<int>();
            //更新数据库
            PQ t_PQ = new PQ();
            t_PQ.paperID = teacherId[0];
            t_PQ.questionID = id;
            db.PQ_table.Add(t_PQ);
            db.SaveChanges();
            return true;
        }
        public bool collectQuestion()
        {
            return true;
        }
        public ActionResult dealwithDoc()//处理上传的文档，解析并发回前端
        {
            string data = "";
            foreach (string upload in Request.Files)
            {

                Stream fileStream = Request.Files[upload].InputStream;
                //              string mimeType = Request.Files[upload].ContentType;
                string fileName = Path.GetFileName(Request.Files[upload].FileName);
                int fileLength = Request.Files[upload].ContentLength;
                //本地存放路径
                string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                //将文件已文件名filename存放在path路径中
                Request.Files[upload].SaveAs(Path.Combine(path, fileName));

                //从本地读取doc文件
                StreamReader sr = new StreamReader(path, Encoding.Unicode);
                data = sr.ReadToEnd();

                /*      //将doc中的数据存放在byte[]中
                      byte[] fileData = new byte[fileLength];
                
                      fileStream.Read(fileData, 0, fileLength);*/

            }

            List<question> questionList = new List<question>();


            int start = 3, end = 0; string p = "<c>";
            while (end < data.Length)
            {

                end = KMPMatch(data, p, start);
                question t_question = new question();
                t_question.content = data.Substring(start, end - start - 3);
                questionList.Add(t_question);
                start = end + 1;
            }
            ViewBag.questionList = questionList;
            return PartialView("_uploadPartial");
        }
        public bool docSave()//文档保存成功
        {

            question t_question = new question();
            t_question.questionType = Convert.ToInt32(Request.Form["****"]);
            t_question.answer = Request.Form["****"];
            t_question.teacherID = Convert.ToInt32(Request.Cookies["userID"].Value);
            t_question.subject = Request.Form["****"];
            t_question.difficulty = Convert.ToInt32(Request.Form["****"]);
            t_question.citationCount = 0;
            t_question.updateTime = DateTime.Now;
            db.question_table.Add(t_question);
            return true;
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
                if (k == -1 || T[j] == T[k])
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