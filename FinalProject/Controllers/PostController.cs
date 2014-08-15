using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace FinalProject.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
             {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (post.User == null)
            {
                post.User = UserManager.FindById(post.UserId);
            }
            return View(post);
        }

        //GET: Post/Username/Ijwu
        public async Task<ActionResult> Username(string username)
        {
            if (username.IsNullOrWhiteSpace())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(user);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
                return View(new PostCreateViewModel());
            }
            ViewBag.ReturnUrl = Url.Action("Create");
            return RedirectToAction("Login","Account");
        }

        // POST: Post/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Content,StartingAmount,Transactions")] PostCreateViewModel postModel)
        {
            var post = new Post {Title = postModel.Title, Content = postModel.Content, BudgetBoxs = new List<BudgetBox>()};
            var box = new BudgetBox
            {
                StartingAmount = postModel.StartingAmount,
                Post = post,
                Title = postModel.Title,
                Transactions = new Collection<Transaction>()
            };
            postModel.Transactions.ForEach(x => box.Transactions.Add(x));
            post.BudgetBoxs.Add(box);
            if (User.Identity.IsAuthenticated)
            {
                post.UserId = User.Identity.GetUserId();
                post.CreationDate = DateTime.UtcNow;
                //post.User = await UserManager.FindByIdAsync(post.UserId);
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Details", new {id=post.Id});
                }

                ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
                return View(post);
            }
            return RedirectToAction("Index","User", new {id=User.Identity.GetUserId()});
        }

        // GET: Post/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            Debug.WriteLine(db.Entry(post).State);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (post == null)
            {
                return HttpNotFound();
            }
            var poster = await UserManager.FindByIdAsync(post.UserId);
            if (!poster.Equals(user))
            {
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
            return View(new PostEditViewModel
            {
                Id = id.Value,
                Content = post.Content, 
                StartingAmount = post.BudgetBoxs.First().StartingAmount,
                Title = post.Title,
                Transactions = post.BudgetBoxs.First().Transactions,
                CreationDate = post.CreationDate,
                UserId = post.UserId
            });
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Content,StartingAmount,Transactions,UserId,CreationDate,Id")] PostEditViewModel postModel)
        {
            var post = new Post
            {
                Id = postModel.Id,
                BudgetBoxs = new List<BudgetBox>(),
                Content = postModel.Content,
                Title = postModel.Title,
                UserId = postModel.UserId,
                CreationDate = postModel.CreationDate
            };
            post.BudgetBoxs.Add(new BudgetBox
            {
                PostId = postModel.Id,
                Post = post, 
                Title = postModel.Title, 
                StartingAmount = postModel.StartingAmount,
                Transactions = postModel.Transactions
            });
            postModel.Transactions.ForEach(x => x.BudgetBox = post.BudgetBoxs.First());
            //postModel.Transactions.ForEach(x => x.BudgetBoxId = post.BudgetBoxs.First().Id);
            postModel.Transactions.ForEach(x =>
            {
                db.Transactions.Find(x.Id).Quantity = x.Quantity;
                db.Transactions.Find(x.Id).UnitPrice = x.UnitPrice;
                db.Transactions.Find(x.Id).Description = x.Description; //TODO: Fix this
            });
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Post", new { Id = post.Id });
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
            return RedirectToAction("Details", "Post", new {Id = post.Id});
        }

        // GET: Post/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var poster = await UserManager.FindByIdAsync(post.UserId);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (!poster.Equals(user))
            {
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetBudgetBox(int id)
        {
            var box = db.BudgetBoxs.First(x => x.Id == id);
            return PartialView("_BudgetBox", box);
        }

        public ActionResult BlankBoxEditor()
        {
            return View("_BudgetBoxEditor", new BudgetBox());
        }

        public ActionResult BlankTransactionEditor()
        {
            return View("_TransactionEditor", new Transaction());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
