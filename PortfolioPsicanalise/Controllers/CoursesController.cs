using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.Migrations;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db)
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

            IEnumerable<CoursesModel> courses = _db.Courses.Where(c=> c.UserId == userId);
            return View(courses);
        }
        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AddCourse(CoursesModel cs)
        
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var userId = User.Identity.Name;
            ModelState.Remove("UserId");

            cs.UserId = userId;

            if (ModelState.IsValid)
            {
                _db.Courses.Add(cs);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditCourse(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.Name;
           CoursesModel course = await _db.Courses.FirstOrDefaultAsync(c => c.UserId == userId );

            if (course == null)
            {
                return RedirectToAction("Index", "Courses");
            }
            return View(course);
        }
        [HttpPost]
        public async Task<IActionResult> EditCourse(CoursesModel cs)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.Name;
            ModelState.Remove("UserId");

            cs.UserId = userId;

            if (ModelState.IsValid)
            {
                _db.Courses.Update(cs);
                await _db.SaveChangesAsync();

                TempData["SuccessfulAction"] = "Course edited successfully";
                return RedirectToAction("Index");
            }
            return View(cs);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCourse()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.Name;
            
            CoursesModel course = await _db.Courses.FirstOrDefaultAsync(c => c.UserId == userId);

            if (course == null)
            {
                return RedirectToAction("Index", "Courses");
            }
            return View(course);
        }
        [HttpPost]
        public IActionResult DeleteCourse(CoursesModel cs)
        {
            if (cs == null)
            {
                return RedirectToAction("Index", "Courses");
            }
            _db.Courses.Remove(cs);
            _db.SaveChanges();

            TempData["SuccessfulAction"] = "Course deleted successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SeeCourse(int? id, string userName)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Courses");
            }
            CoursesModel course = _db.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.UserName = userName;
            return View(course);

        }
    }
}
