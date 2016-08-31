using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OldNewTeamProject.Models;

namespace OldNewTeamProject.Controllers
{
    public class LanguagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Languages
        [Authorize(Roles ="Administrator")]
        public ActionResult Index()
        {
            return View(db.Languages.Include(l => l.Uploader).ToList());
        }

        // GET: Languages/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Languages.Include(l => l.Uploader).FirstOrDefault(l => l.Id == id);
            var evaluations = db.Evaluations.ToList();

            List<Evaluation> posts = new List<Evaluation>();

            

            foreach (var post in evaluations)
            {
                if (post.LanguageId == language.Id)
                {
                    posts.Add(post);
                }
            }
            language.Posts = posts;
            int positiveEvaluations = 0;
            int negativeEvaluations = 0;
            foreach (var post in language.Posts)
            {
                if (post.Value == "rocks")
                {
                    positiveEvaluations++;
                }
                else
                {
                    negativeEvaluations++;
                }
            }
            language.positives = positiveEvaluations;
            language.negatives = negativeEvaluations;
            if (language.Posts.Count > 0)
            {
                language.ratio = (double)positiveEvaluations / language.Posts.Count * 100;
            }
            language.Posts = language.Posts.OrderByDescending(date => date.Date).ToList();

            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // GET: Languages/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Date,ratio,positives,negatives")] Language language)
        {
            if (ModelState.IsValid)
            {
                language.Uploader = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Languages.Add(language);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(language);
        }

        // GET: Languages/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Languages.Find(id);
            var evaluations = db.Evaluations.OrderBy(p => p.Date).ToList();
            
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Date,ratio,positives,negatives")] Language language)
        {
            if (ModelState.IsValid)
            {
                db.Entry(language).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Language language = db.Languages.Find(id);
            var evaluations = db.Evaluations;

            foreach (var evaluation in evaluations)
            {
                if (evaluation.LanguageId == language.Id)
                {
                    db.Evaluations.Remove(evaluation);
                }
            }


            db.Languages.Remove(language);
            db.SaveChanges();
            return RedirectToAction("Index");
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
