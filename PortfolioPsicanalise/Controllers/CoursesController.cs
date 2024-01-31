using Microsoft.AspNetCore.Mvc;
using PortfolioPsicanalise.Data;
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
            IEnumerable<CoursesModel> courses = _db.Courses;
            return View(courses);
        }
        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(CoursesModel cs)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Add(cs);
                _db.SaveChanges();
                
                TempData["SuccessfulAction"] = "Course added successfully";

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditCourse(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            CoursesModel course = _db.Courses.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpPost]
        public IActionResult EditCourse(CoursesModel cs)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Update(cs);
                _db.SaveChanges();

                TempData["SuccessfulAction"] = "Course edited successfully";
                return RedirectToAction("Index");
            }
            return View(cs);
        }

        [HttpGet]
        public IActionResult DeleteCourse(int? id)
        {
            if (id == null || id == 0)
            {
            return NotFound();
            }
            CoursesModel cs = _db.Courses.FirstOrDefault(x => x.Id == id);

            if (cs == null)
            {
            return NotFound(); 
            }
            return View(cs);
        }
        [HttpPost]
        public IActionResult DeleteCourse(CoursesModel cs)
        {
            if (cs == null)
            {
                return NotFound();
            }
            _db.Courses.Remove(cs);
            _db.SaveChanges();

            TempData["SuccessfulAction"] = "Course deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
