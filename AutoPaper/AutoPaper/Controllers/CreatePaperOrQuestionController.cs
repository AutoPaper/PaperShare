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

        // GET: /CreatePaperOrQuestion/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AssembleInfo()
        {
            return RedirectToAction("Index");
        }
	}
}