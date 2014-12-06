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
    public class HomeController : Controller
    {
        private PaperShareDBContext db = new PaperShareDBContext();

        public ActionResult Index()
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
                if (person[0].name != null)
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
                    RedirectToAction(nextActionName);
                }
                else
                    RedirectToAction("Index");
                return "";
            }
            else
            {
                return "用户名或密码错误！";
            }
        }
        public ActionResult Register()
        {
            return RedirectToAction("Index");
        }
    }
}