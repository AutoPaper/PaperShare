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
        public ActionResult Login()
        {
            var name = Request.Form["user-id"];
            var key = Request.Form["user-key"];
            var person = (from o in db.user_table
                         where o.name == name && o.keyhash == key
                         select o).ToArray();
            if (person.Length == 1)
            {
                HttpCookie cookie = new HttpCookie("UserID");
                cookie.Value = Convert.ToString(person[0].ID);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return RedirectToAction("Index");
        }
    }
}