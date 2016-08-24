using OldNewTeamProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OldNewTeamProject.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var languages = db.Languages.ToList();
            var evaluations = db.Evaluations.ToList();

            foreach (var language in languages)
            {
                List<Evaluation> posts = new List<Evaluation>();
                foreach (var post in evaluations)
                {
                    if (post.LanguageId == language.Id)
                    {
                        posts.Add(post);
                    }
                }
                language.Posts = posts;
                int positiveEvaluations = 0; //language.Posts.Select(p => p).Where(p => p.PositiveOrNegative == "rocks").Count();
                int negativeEvaluations = 0; // language.Posts.Select(p => p).Where(p => p.PositiveOrNegative == "sucks").Count();
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
            }

            languages = languages.OrderByDescending(l => l.ratio).ToList();

            return View(languages.ToList());
        }


    }
}