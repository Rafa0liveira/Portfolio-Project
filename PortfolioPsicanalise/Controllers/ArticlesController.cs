using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;

            IEnumerable<ArticlesModel> articles = _db.Articles.Where(a => a.UserId == userId); 
            return View(articles);
        }
        [HttpGet]
        public IActionResult AddArticle()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArticle(ArticlesModel art)
        {
            // Verificar se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Associar o UserId ao nome do usuário autenticado
            var userId = User.Identity.Name;
            Console.WriteLine($"UserId (User.Identity.Name): {userId}");

            // Remover a validação do campo UserId do ModelState
            ModelState.Remove("UserId");

            // Associar o UserId ao registro do livro
            art.UserId = userId;
            Console.WriteLine($"UserId associated with book: {art.UserId}");

            // Verificar se o ModelState é válido
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid. Errors:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
                return View(art);  // Retornar a view com os erros de validação
            }

            // Tentar adicionar o livro ao banco de dados
            try
            {
                _db.Articles.Add(art);
                await _db.SaveChangesAsync();

                TempData["SuccessfulAction"] = "Article added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving article to database: {ex.Message}");
                return View(art);  // Retornar a view com o erro
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditArticle()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;
           
            ArticlesModel article = await _db.Articles.FirstOrDefaultAsync(a => a.UserId == userId);

            if (article == null)
            {
                return RedirectToAction("Index", "Articles");
            }
            return View(article);
        }
        [HttpPost]
        public IActionResult EditArticle(ArticlesModel art)
        {
            // Verificar se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Associar o UserId ao nome do usuário autenticado
            var userId = User.Identity.Name;
            // Remover a validação do campo UserId do ModelState
            ModelState.Remove("UserId");

            // Associar o UserId ao registro do livro
            art.UserId = userId;

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
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Articles");
            }
            

            ArticlesModel article = await _db.Articles.FirstOrDefaultAsync(x => x.Id == id );

            if (article == null)
            {
                return RedirectToAction("Index", "Articles");
            }
            return View(article);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(ArticlesModel article)
        {
            if (article == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
            TempData["SuccessfulAction"] = "Article deleted successfully";

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SeeArticle(int? id, string userName)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Articles");
            }
            ArticlesModel article = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            ViewBag.UserName = userName;
            return View(article);

        }
        

        }
    }

