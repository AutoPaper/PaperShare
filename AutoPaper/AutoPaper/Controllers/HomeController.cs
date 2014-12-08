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

        public ActionResult Index()
        {
            List<hotPaper> hotPapers = new List<hotPaper>();
            var ids = (from o in db.PT_table
                       orderby o.doneCount descending
                       select o.paperID).Take(4);
            foreach (var id in ids)
            {
                hotPaper hp = new hotPaper();
                hp.paperName = (from o in db.paper_table
                                where o.ID == id
                                select o.name).ToString();
                hp.teacher = (from c in db.user_table
                              where (from o in db.PT_table
                                     where o.paperID == id
                                     select o.teacherID).Contains(c.ID)
                              select c.name).ToString();
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
            List<hotTeacher> hotTeachers = new List<hotTeacher>();
            var ids = ((from o in db.PT_table
                        orderby o.doneCount descending
                        select o.teacherID).Distinct()).Take(4);
            foreach (var id in ids)
            {
                hotTeacher ht = new hotTeacher();
                ht.teacherName = (from o in db.user_table
                                  where o.ID == id
                                  select o.name).ToString();
                List<nameADdonecount> Name_doneCount = new List<nameADdonecount>();
                var pt = from o in db.PT_table
                         where o.teacherID == id
                         orderby o.doneCount descending
                         select o;
                foreach (var p in pt)
                {
                    nameADdonecount ndc = new nameADdonecount();
                    ndc.paperName = (from o in db.paper_table
                                     where o.ID == p.paperID
                                     select o.name).ToString();
                    ndc.doneCount = p.doneCount;
                    Name_doneCount.Add(ndc);
                }
                ht.Name_doneCount = Name_doneCount;
                hotTeachers.Add(ht);
            }
            ViewBag.hotTeachers = hotTeachers;
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
                userCookie[3] = new HttpCookie("userSubject", "Maths");
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
            if (email == null || key == null || checkKey == null || userType == null)
                return "请输入完整注册信息！";
            else if(!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                return "请输入正确邮箱！";
            else if(!Regex.IsMatch(key,@"^[a-zA-Z0-9_]{6,16}$"))
                return "密码必须为6-16位字母、数字或下划线的组合！";
            else if (key != checkKey)
                return "两次密码不一致！";
            else
            {
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
                userCookie[3] = new HttpCookie("userSubject", "Maths");
                return "";
            }
        }

        public class hotPaper
        {
            public string paperName;
            public string teacher;
            public List<string> paperTags;
        }
        public class hotTeacher
        {
            public string teacherName;
            public List<nameADdonecount> Name_doneCount;
        }
        public class nameADdonecount
        {
            public string paperName;
            public int doneCount;
        }
    }
}