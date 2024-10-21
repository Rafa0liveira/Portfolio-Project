using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PortfolioPsicanalise.Controllers
{
    public class BooksController : Controller
    {
        readonly private AppDbContext _db;
        public BooksController(AppDbContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;

            IEnumerable<BooksModel> books = _db.Books.Where(b => b.UserId == userId);
            return View(books);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(BooksModel bk)
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
            bk.UserId = userId;
            Console.WriteLine($"UserId associated with book: {bk.UserId}");

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
                return View(bk);  // Retornar a view com os erros de validação
            }

            // Tentar adicionar o livro ao banco de dados
            try
            {
                _db.Books.Add(bk);
                await _db.SaveChangesAsync();

                TempData["SuccessfulAction"] = "Book added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving book to database: {ex.Message}");
                return View(bk);  // Retornar a view com o erro
            }
        }

        [HttpGet]
        public IActionResult EditBook()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.Name;

            BooksModel book = _db.Books.FirstOrDefault(b => b.UserId == userId);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }
            return View(book);
        }
        [HttpPost]
        public IActionResult EditBook(BooksModel bk)
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
            bk.UserId = userId;

            if (ModelState.IsValid)
            {
                _db.Books.Update(bk);
                _db.SaveChanges();

                TempData["SuccessfulAction"] = "Book edited successfully";
               
                return RedirectToAction("Index");
            }
            return View(bk);
        }


            [HttpGet]
        public async Task<IActionResult> DeleteBook()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.Name;

            BooksModel book = _db.Books.FirstOrDefault(b => b.UserId == userId);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(BooksModel book)
        {
            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }
                        
            _db.Books.Remove(book);
           await _db.SaveChangesAsync();

            TempData["SuccessfulAction"] = "Book deleted successfully";

            return RedirectToAction("Index");
        }



    
}
}
