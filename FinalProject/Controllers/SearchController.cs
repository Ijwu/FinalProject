using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index(string searchString)
        {
            var posts = db.Posts.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                posts = (from s in db.Posts
                         where s.Title.Contains(searchString) || s.Content.Contains(searchString)
                         select s).ToList();

            }
            return View(posts);
        }
    }
}
