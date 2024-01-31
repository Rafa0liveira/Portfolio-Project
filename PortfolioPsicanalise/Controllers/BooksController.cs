using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.Models;
using System.Collections.Generic;

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
            IEnumerable<BooksModel> books = _db.Books;
            return View(books);
        }
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBook(BooksModel bk)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Add(bk);
                _db.SaveChanges();
                
                TempData["SuccessfulAction"] = "Book added successfully";
               
                return RedirectToAction("Index");

                
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditBook(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            BooksModel book = _db.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public IActionResult EditBook(BooksModel bk)
        {
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
        public IActionResult DeleteBook(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            BooksModel bk = _db.Books.FirstOrDefault(x =>x.Id == id);

            if (bk == null)
            {
                return NotFound();
            }
            return View(bk);
        }

        [HttpPost]
        public IActionResult DeleteBook(BooksModel bk)
        {
            if (bk == null)
            {
                return NotFound();
            }
                        
            _db.Books.Remove(bk);
            _db.SaveChanges();

            TempData["SuccessfulAction"] = "Book deleted successfully";

            return RedirectToAction("Index");
        }



    
}
}
