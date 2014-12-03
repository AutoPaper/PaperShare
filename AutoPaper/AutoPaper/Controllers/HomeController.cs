﻿using System;
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
                HttpCookie[] userCookie = new HttpCookie[4];
                userCookie[0] = new HttpCookie("userID", Convert.ToString(person[0].ID));
                if (person[0].name != null)
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(person[0].name));
                else
                    userCookie[1] = new HttpCookie("userName", Convert.ToString(person[0].email));
                userCookie[2] = new HttpCookie("userType", Convert.ToString(person[0].role));
                foreach (var cookie in userCookie)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);//7天过期
                    Response.Cookies.Add(cookie);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return RedirectToAction("Index");
        }
    }
}