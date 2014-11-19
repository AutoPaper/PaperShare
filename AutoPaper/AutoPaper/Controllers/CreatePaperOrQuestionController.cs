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

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult AssembleResult()
        {
            ViewBag.PaperName = Request.Form["assemble-paper-name"];
            var choice_count = Request.Form["assemble-paper-choice-count"];
            var choice_score = Request.Form["assemble-paper-choice-score"];
            var choice_order = Request.Form["assemble-paper-choice-order"];
            var blanks_count = Request.Form["assemble-paper-fill-in-blanks-count"];
            var blanks_score = Request.Form["assemble-paper-fill-in-blanks-score"];
            var blanks_order = Request.Form["assemble-paper-fill-in-blanks-order"];
            var judge_count = Request.Form["assemble-paper-judge-count"];
            var judge_score = Request.Form["assemble-paper-judge-score"];
            var judge_order = Request.Form["assemble-paper-judge-order"];
            var easy = Request.Form["assemble-hardness-easy"];
            var medium = Request.Form["assemble-hardness-medium"];
            var hard = Request.Form["assemble-hardness-hard"];
            string[] knowledgeArray = Request.Form["assemble-knowledge-value"].Split
                (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<question> questionList = new List<question>();//题目的总集合

            List<string> allKnowledge = new List<string>();
            findAllKnowledge(knowledgeArray, ref allKnowledge);//抓取层次模型中所有命中的考点
            var qk = (from o in db.QK_table
                      where allKnowledge.Contains(o.knowledge)
                      select o).ToArray();
            int[] qID = new int[qk.Length];
            for (int i = 0; i < qk.Length; i++)
                qID[i] = qk[i].questionID;
            var q = from o in db.question_table
                    where qID.Contains<int>(o.ID)
                    select o;                                  //依据考点筛选出待选题目

            var history = (from o in db.selectHistory_table
                           where o.teacherID == int.Parse(Request.Cookies["userID"].Value)
                           select o).ToArray();                            
            int[] questionHistory = new int[history.Length];
            for (int i = 0; i < history.Length; i++)
                questionHistory[i] = history[i].questionID;    //筛出该教师历史出题记录


            List<question> choicelist = new List<question>();
            List<question> blanklist = new List<question>();
            List<question> judgelist = new List<question>();
            try                                                //开始出题
            {
                #region
                if (choice_count != "")
                {
                    int choiceCount = 5;//int.Parse(choice_count);
                    var choicelist1 = ((from o in db.question_table
                                        where o.questionType == 1 && o.difficulty == 1
                                        //&& !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                        select o).Take(5))//(choiceCount * int.Parse(easy) /
                                       //(int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                   // if (choicelist1.Count() < choiceCount * int.Parse(easy) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                   // {
                   //     throw new ReiterationException(11);
                  //  }                                              //选择题-简单

                    var choicelist2 = ((from o in db.question_table
                                        where o.questionType == 1 && o.difficulty == 2
                                       // && !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                         select o).Take(5))//(choiceCount * int.Parse(medium) /
                                      // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                   // if (choicelist2.Count() < choiceCount * int.Parse(medium) /
                   //                   (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                  //      throw new ReiterationException(12);
                  //  }                                              //选择题-中档

                    var choicelist3 = ((from o in db.question_table
                                        where o.questionType == 1 && o.difficulty == 3
                                       // && !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                         select o).Take(5))//(choiceCount * int.Parse(hard) /
                                       // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                   // if (choicelist3.Count() < choiceCount * int.Parse(hard) /
                   //                   (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                   // {
                   //     throw new ReiterationException(13);
                   // }                                              //选择题-难题
                    choicelist.AddRange(choicelist1);
                    choicelist.AddRange(choicelist2);
                    choicelist.AddRange(choicelist3);
                }
                #endregion
                #region
                if (blanks_count != "")
                {
                    int blankCount = 5;// int.Parse(blanks_count);
                    var blanklist1 = ((from o in db.question_table
                                        where o.questionType == 2 && o.difficulty == 1
                                       // && !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                        select o).Take(5))//(blankCount * int.Parse(easy) /
                                      // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                  //  if (blanklist1.Count() < blankCount * int.Parse(easy) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                  //      throw new ReiterationException(21);
                  //  }                                              //填空题-简单

                    var blanklist2 = ((from o in db.question_table
                                        where o.questionType == 2 && o.difficulty == 2
                                       // && !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                        select o).Take(5))//(blankCount * int.Parse(medium) /
                                      // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                 //   if (blanklist2.Count() < blankCount * int.Parse(medium) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                  //      throw new ReiterationException(22);
                  //  }                                              //填空题-中档

                    var blanklist3 = ((from o in db.question_table
                                        where o.questionType == 2 && o.difficulty == 3
                                       // && !questionHistory.Contains(o.ID)
                                        orderby o.citation descending
                                        select o).Take(5))//(blankCount * int.Parse(hard) /
                                      //  (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                  //  if (blanklist3.Count() < blankCount * int.Parse(hard) /
                   //                   (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                   //     throw new ReiterationException(23);
                  //  }                                              //填空题-难题
                    blanklist.AddRange(blanklist1);
                    blanklist.AddRange(blanklist2);
                    blanklist.AddRange(blanklist3);
                }
                #endregion
                #region
                if (judge_count != "")
                {
                    int judgeCount = 5;// int.Parse(judge_count);
                    var judgelist1 = ((from o in db.question_table
                                       where o.questionType == 3 && o.difficulty == 1
                                      // && !questionHistory.Contains(o.ID)
                                       orderby o.citation descending
                                       select o).Take(5))//(judgeCount * int.Parse(easy) /
                                      // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                  //  if (judgelist1.Count() < judgeCount * int.Parse(easy) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                  //      throw new ReiterationException(31);
                  //  }                                              //判断题-简单

                    var judgelist2 = ((from o in db.question_table
                                       where o.questionType == 3 && o.difficulty == 2
                                      // && !questionHistory.Contains(o.ID)
                                       orderby o.citation descending
                                        select o).Take(5))//(judgeCount * int.Parse(medium) /
                                     //  (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                 //   if (judgelist2.Count() < judgeCount * int.Parse(medium) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                 //       throw new ReiterationException(32);
                 //   }                                              //判断题-中档

                    var judgelist3 = ((from o in db.question_table
                                       where o.questionType == 3 && o.difficulty == 3
                                      // && !questionHistory.Contains(o.ID)
                                       orderby o.citation descending
                                       select o).Take(5))//(judgeCount * int.Parse(hard) /
                                       // (int.Parse(easy) + int.Parse(medium) + int.Parse(hard))))
                                        .ToList();
                 //   if (judgelist3.Count() < judgeCount * int.Parse(hard) /
                  //                    (int.Parse(easy) + int.Parse(medium) + int.Parse(hard)))
                  //  {
                  //      throw new ReiterationException(33);
                  //  }                                              //判断题-难题
                    judgelist.AddRange(judgelist1);
                    judgelist.AddRange(judgelist2);
                    judgelist.AddRange(judgelist3);
                }
                #endregion
            }
            catch(ReiterationException)                        //题目重复异常处理
            {
                //建设中
            }
            int choice = int.Parse(choice_order);
            int blank = int.Parse(blanks_order);
            int judge = int.Parse(judge_order);
            #region
            if (choice > blank)
            {
                if (blank > judge)
                {
                    questionList.AddRange(judgelist);
                    questionList.AddRange(blanklist);
                    questionList.AddRange(choicelist);
                    ViewBag.FirstQuestionLength = int.Parse(judge_count);
                    ViewBag.FirstQuestionName = "判断题";
                    ViewBag.SecondQuestionLength = int.Parse(blanks_count);
                    ViewBag.SecondQuestionName = "填空题";
                    ViewBag.ThirdQuestionLength = int.Parse(choice_count);
                    ViewBag.ThirdQuestionName = "选择题";
                }
                else if (choice > judge)
                {
                    questionList.AddRange(blanklist);
                    questionList.AddRange(judgelist);
                    questionList.AddRange(choicelist);
                    ViewBag.FirstQuestionLength = int.Parse(blanks_count);
                    ViewBag.FirstQuestionName = "填空题";
                    ViewBag.SecondQuestionLength = int.Parse(judge_count);
                    ViewBag.SecondQuestionName = "判断题";
                    ViewBag.ThirdQuestionLength = int.Parse(choice_count);
                    ViewBag.ThirdQuestionName = "选择题";
                }
                else
                {
                    questionList.AddRange(blanklist);
                    questionList.AddRange(choicelist);
                    questionList.AddRange(judgelist);
                    ViewBag.FirstQuestionLength = int.Parse(blanks_count);
                    ViewBag.FirstQuestionName = "填空题";
                    ViewBag.SecondQuestionLength = int.Parse(choice_count);
                    ViewBag.SecondQuestionName = "选择题";
                    ViewBag.ThirdQuestionLength = int.Parse(judge_count);
                    ViewBag.ThirdQuestionName = "判断题";
                }
            }
            else if (choice > judge)
            {
                questionList.AddRange(judgelist);
                questionList.AddRange(choicelist);
                questionList.AddRange(blanklist);
                ViewBag.FirstQuestionLength = int.Parse(judge_count);
                ViewBag.FirstQuestionName = "判断题";
                ViewBag.SecondQuestionLength = int.Parse(choice_count);
                ViewBag.SecondQuestionName = "选择题";
                ViewBag.ThirdQuestionLength = int.Parse(blanks_count);
                ViewBag.ThirdQuestionName = "填空题";
            }
            else if (blank > judge)
            {
                questionList.AddRange(choicelist);
                questionList.AddRange(judgelist);
                questionList.AddRange(blanklist);
                ViewBag.FirstQuestionLength = int.Parse(choice_count);
                ViewBag.FirstQuestionName = "选择题";
                ViewBag.SecondQuestionLength = int.Parse(judge_count);
                ViewBag.SecondQuestionName = "判断题";
                ViewBag.ThirdQuestionLength = int.Parse(blanks_count);
                ViewBag.ThirdQuestionName = "填空题";
            }
            else
            {
               /* questionList.AddRange(choicelist);
                questionList.AddRange(blanklist);
                questionList.AddRange(judgelist);
                ViewBag.FirstQuestionLength =int.Parse(choice_count);
                ViewBag.FirstQuestionName = "选择题";
                ViewBag.SecondQuestionLength = int.Parse(blanks_count);
                ViewBag.SecondQuestionName = "填空题";
                ViewBag.ThirdQuestionLength = int.Parse(judge_count);
                ViewBag.ThirdQuestionName = "判断题";*/
            }
            #endregion
            return View(questionList);
        }
        public void findAllKnowledge(string[] knowledgeArray, ref List<string> allKnowledge) 
        {
            var p = from o in db.knowledgeTree_table 
                         where knowledgeArray.Contains(o.knowledge)
                         select o;
            var parent = p.ToArray();
            int[] parentID = new int[parent.Length];

            for (int i = 0; i < parentID.Length; i++)
            {
                parentID[i] = parent[i].ID;
                allKnowledge.Add(parent[i].knowledge);
            }

            while (true)
            {
                var c = from o in db.knowledgeTree_table
                        where parentID.Contains(o.parentID)
                        select o;
                var child = c.ToArray();

                if (child.Length == 0)
                    break;
                parentID = new int[child.Length];

                for (int i = 0; i < parentID.Length; i++)
                {
                    parentID[i] = child[i].ID;
                    allKnowledge.Add(child[i].knowledge);
                }
            }
        }
        public class ReiterationException : Exception
        {
            private int type;
            public ReiterationException(int type)
                : base()
            {
                this.type = type;
            }
            public int Type
            {
                get
                {
                    return this.type;
                }
            }
        }//题目重复的异常
	}
}