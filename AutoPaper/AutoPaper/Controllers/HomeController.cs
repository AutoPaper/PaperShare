using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPaper.Models;
using System.Text.RegularExpressions;

namespace AutoPaper.Controllers
{
    public class HomeController : Controller
    {
        private PaperShareDBContext db = new PaperShareDBContext();

        [HttpGet]
        public ActionResult searchResult(string keywords)
        {
            List<Paper> matchPapers = new List<Paper>();
            var ids = (from c in db.paper_table
                       where c.name.Contains(keywords)
                       select c.ID).ToArray<int>();
            foreach (var id in ids)
            {
                Paper hp = new Paper();
                var paper = (from o in db.paper_table
                             where o.ID == id
                             select o).ToArray<papers>();
                hp.paper = paper[0];
                var user = (from c in db.user_table
                            where (from o in db.PT_table
                                   where o.paperID == id
                                   select o.teacherID).Contains(c.ID)
                            select c).ToArray<users>();
                hp.teacher = user[0];
                hp.paperTags = (from c in db.tag_table
                                where (from o in db.PTags_table
                                       where o.paperID == id
                                       select o.tagID).Contains(c.ID)
                                select c.content).ToList<string>();
                matchPapers.Add(hp);
            }
            ViewBag.matchPapers = matchPapers;

            List<Teacher> matchTeachers = new List<Teacher>();
            ids = (from c in db.user_table
                   where c.name.Contains(keywords) && c.role == 1
                   select c.ID).ToArray<int>();
            foreach (var id in ids)
            {
                Teacher ht = new Teacher();
                var teacher = (from o in db.user_table
                               where o.ID == id
                               select o).ToArray<users>();
                ht.teacher = teacher[0];
                List<nameADdonecount> Name_doneCount = new List<nameADdonecount>();
                var pt = from o in db.PT_table
                         where o.teacherID == id
                         orderby o.doneCount descending
                         select o;
                foreach (var p in pt)
                {
                    nameADdonecount ndc = new nameADdonecount();
                    var paper = (from o in db.paper_table
                                 where o.ID == p.paperID
                                 select o).ToArray<papers>();
                    ndc.paper = paper[0];
                    ndc.doneCount = p.doneCount;
                    Name_doneCount.Add(ndc);
                }
                ht.Name_doneCount = Name_doneCount;
                matchTeachers.Add(ht);
            }
            ViewBag.matchTeachers = matchTeachers;

            List<question> matchQuestions = new List<question>();
            ids = (from c in db.question_table
                   where c.content.Contains(keywords)
                   select c.ID).ToArray<int>();
            foreach (var id in ids)
            {
                Question hq = new Question();
                var qt = (from o in db.question_table
                          where o.ID == id
                          select o).ToArray<question>();
                hq.qt = qt[0];

            }

            return View();
        }

        public ActionResult Index()
        {
            List<Paper> hotPapers = new List<Paper>();
            var ids = (from o in db.PT_table
                       orderby o.doneCount descending
                       select o.paperID).Take(20);
            foreach (var id in ids)
            {
                Paper hp = new Paper();
                var paper = (from o in db.paper_table
                             where o.ID == id
                             select o).ToArray<papers>();
                hp.paper = paper[0];
                var user = (from c in db.user_table
                            where (from o in db.PT_table
                                   where o.paperID == id
                                   select o.teacherID).Contains(c.ID)
                            select c).ToArray<users>();
                hp.teacher = user[0];
                hp.paperTags = (from c in db.tag_table
                                where (from o in db.PTags_table
                                       where o.paperID == id
                                       select o.tagID).Contains(c.ID)
                                select c.content).ToList<string>();
                hotPapers.Add(hp);
            }
            ViewBag.hotPapers = hotPapers;
            return View();
        }
        public ActionResult HotTeacher()
        {
            List<Teacher> hotTeachers = new List<Teacher>();
            var ids = ((from o in db.PT_table
                        orderby o.doneCount descending
                        select o.teacherID).Distinct()).Take(4);
            foreach (var id in ids)
            {
                Teacher ht = new Teacher();
                var teacher = (from o in db.user_table
                               where o.ID == id
                               select o).ToArray<users>();
                ht.teacher = teacher[0];
                List<nameADdonecount> Name_doneCount = new List<nameADdonecount>();
                var pt = from o in db.PT_table
                         where o.teacherID == id
                         orderby o.doneCount descending
                         select o;
                foreach (var p in pt)
                {
                    nameADdonecount ndc = new nameADdonecount();
                    var paper = (from o in db.paper_table
                                 where o.ID == p.paperID
                                 select o).ToArray<papers>();
                    ndc.paper = paper[0];
                    ndc.doneCount = p.doneCount;
                    Name_doneCount.Add(ndc);
                }
                ht.Name_doneCount = Name_doneCount;
                hotTeachers.Add(ht);
            }
            ViewBag.hotTeachers = hotTeachers;
            return View();
        }
        public ActionResult HotQuestion()
        {
            return View();
        }

        [HttpPost]
        public string Login()
        {
            var email = Request.Form["user-email"];
            var key = Request.Form["user-key"];
            var person = (from o in db.user_table
                          where o.email == email && o.keyhash == key
                          select o).ToArray();
            if (person.Length == 1)
            {
                HttpCookie[] userCookie = new HttpCookie[4];
                userCookie[0] = new HttpCookie("userID", Convert.ToString(person[0].ID));
                if (person[0].name != "")
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(person[0].name));
                else
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(person[0].email));
                userCookie[2] = new HttpCookie("userType", Convert.ToString(person[0].role));
                userCookie[3] = new HttpCookie("userSubject", "数学");
                foreach (var cookie in userCookie)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);//7天过期
                    Response.Cookies.Add(cookie);
                }
                if (Session["nextActionName"] != null)
                {
                    string nextActionName = Session["nextActionName"].ToString();
                    Session.Contents.Remove("nextActionName");
                    return nextActionName;
                }
                else
                    return "";
            }
            else
            {
                return "用户名或密码错误！";
            }
        }
        [HttpPost]
        public string Register()
        {
            var email = Request.Form["reg-email"];
            var key = Request.Form["reg-key"];
            var checkKey = Request.Form["reg-confirm-email"];
            var userName = Request.Form["reg-name"];
            var userType = Request.Form["userType"];
            if (email == "" || key == "" || checkKey == "" || userType == "")
                return "请输入完整注册信息！";
            else if(!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                return "请输入正确邮箱！";
            else if(!Regex.IsMatch(key,@"^[a-zA-Z0-9_]{6,16}$"))
                return "密码必须为6-16位字母、数字或下划线的组合！";
            else if (key != checkKey)
                return "两次密码不一致！";
            else
            {
                string em = (from o in db.user_table
                             where o.email == email
                             select o.email).ToString();
                if (em != null)
                    return "该邮箱已被注册！";
                users u = new users();
                u.keyhash = key;
                u.email = email;
                if (userName != null)
                    u.name = userName;
                if (userType == "学生")
                    u.role = 2;
                else
                    u.role = 1;
                db.user_table.Add(u);
                db.SaveChanges();
                var id = from o in db.user_table
                         where o.email == u.email
                         select o.ID;
                HttpCookie[] userCookie = new HttpCookie[4];
                userCookie[0] = new HttpCookie("userID", Convert.ToString(id));
                if (u.name != "")
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(u.name));
                else
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(u.email));
                userCookie[2] = new HttpCookie("userType", Convert.ToString(u.role));
                userCookie[3] = new HttpCookie("userSubject", "数学");
                return "";
            }
        }

        public class Paper
        {
            public papers paper;
            public users teacher;
            public List<string> paperTags;
        }
        public class Teacher
        {
            public users teacher;
            public List<nameADdonecount> Name_doneCount;
        }
        public class nameADdonecount
        {
            public papers paper;
            public int doneCount;
        }
        public class Question
        {
            public question qt;
            public string teacherName;
        }
    }
}