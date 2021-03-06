﻿using System;
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
    public class EvaluationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluations
        [Authorize(Roles ="Administrator")]
        public ActionResult Index()
        {
            return View(db.Evaluations.Include(e => e.Author).ToList());
        }

        // GET: Evaluations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Evaluation evaluation = db.Evaluations.Include(a => a.Author).FirstOrDefault(a => a.Id == id);
            
            
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // GET: Evaluations/Create
        public ActionResult Create()
        {
            List<Language> languages = db.Languages.ToList();
            ViewBag.DropDownValues = new SelectList(languages.Select(n => n.Name).ToList());

            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,Value,Language,AuthorId,LanguageId")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                evaluation.Author = db.Users.FirstOrDefault(a => a.UserName == User.Identity.Name);
                db.Evaluations.Add(evaluation);
                List<Language> languages = db.Languages.ToList();
                evaluation.AvailableLanguages = languages;
                evaluation.LanguageId = db.Languages.FirstOrDefault(l => l.Name == evaluation.Language).Id;

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Value,Language,AuthorId,LanguageId")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evaluation evaluation = db.Evaluations.Find(id);
            db.Evaluations.Remove(evaluation);
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
