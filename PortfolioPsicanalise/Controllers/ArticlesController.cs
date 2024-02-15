using Microsoft.AspNetCore.Mvc;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Controllers
{
    public class ArticlesController : Controller
    {
        readonly private AppDbContext _db;
        public ArticlesController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ArticlesModel> articles = _db.Articles; 
            return View(articles);
        }
        [HttpGet]
        public IActionResult AddArticle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddArticle(ArticlesModel art)
        {
            if (ModelState.IsValid)
            {
                _db.Articles.Add(art);
                _db.SaveChanges();
                TempData["SuccessfulAction"] = "Article added successfully";

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditArticle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ArticlesModel article = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
        [HttpPost]
        public IActionResult EditArticle(ArticlesModel art)
        {
            if (ModelState.IsValid)
            {
                _db.Articles.Update(art);
                _db.SaveChanges();
                TempData["SuccessfulAction"] = "Article edited successfully";


                return RedirectToAction("Index");
            }
            return View(art);
        }
        [HttpGet]
        public IActionResult DeleteArticle(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ArticlesModel article = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
        [HttpPost]
        public IActionResult DeleteArticle(ArticlesModel article)
        {
            if (article == null)
            {
                return NotFound();
            }

            _db.Articles.Remove(article);
            _db.SaveChanges();
            TempData["SuccessfulAction"] = "Article deleted successfully";

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult SeeArticle(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ArticlesModel article = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
        [HttpPost]
        public IActionResult SeeArticle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ArticlesModel article = _db.Articles.FirstOrDefault(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

    }
}
