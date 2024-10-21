using Microsoft.AspNetCore.Mvc;
using PortfolioPsicanalise.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PortfolioPsicanalise.ViewModels;
using PortfolioPsicanalise.Controllers;
using PortfolioPsicanalise.Data;
using Microsoft.Extensions.Logging;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace PortfolioPsicanalise.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HomeController> _logger;
        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult SearchUser(string userName)
        {
            // Verificação para garantir que o nome completo foi inserido
            if (string.IsNullOrEmpty(userName) || userName.Split(' ').Length < 2)
            {
                ViewBag.UserName = userName;
                return View("UserNotFound"); // Redirecionar para a view de erro
            }

            // Buscar por usuários com nome completo
            var books = _db.Books.Where(b => b.UserId.ToLower() == userName.ToLower()).ToList();
            var articles = _db.Articles.Where(a => a.UserId.ToLower() == userName.ToLower()).ToList();
            var courses = _db.Courses.Where(c => c.UserId.ToLower() == userName.ToLower()).ToList();

            // Se nenhum resultado for encontrado, redirecionar para a página "UserNotFound"
            if (!books.Any() && !articles.Any() && !courses.Any())
            {
                ViewBag.UserName = userName;
                return View("UserNotFound");  // Redireciona para a View "UserNotFound"
            }

            // Se encontrar resultados, exibir a página com os dados
            var model = new SearchUserViewModel
            {
                Books = books,
                Articles = articles,
                Courses = courses
            };

            ViewBag.UserName = userName;
            return View(model);
        }




        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        public async Task<IActionResult> GuestSeeArticle(int? id, string userName)
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

            ViewBag.UserName = userName;
            return View(article);

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}