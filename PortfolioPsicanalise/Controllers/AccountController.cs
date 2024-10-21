using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.ViewModels;
using System.Security.Claims;
using PortfolioPsicanalise.Services.SessionService;
using ISession = PortfolioPsicanalise.Services.SessionService.ISession;

namespace PortfolioPsicanalise.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISession _session;

        readonly private AppDbContext _db;
        public AccountController(AppDbContext db, ISession session)
        {
            _db = db;
            _session = session;
        }


        // GET: AccountController/Login
        public ActionResult Login()
        {
            if(_session.SearchSession()!= null) return RedirectToAction("Index","Home");
            return View();
        }

        public IActionResult UserProfile()
        {
            // Recuperar o ID do usuário da sessão
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Buscar o usuário no banco de dados usando o ID
            var user = _db.Users.SingleOrDefault(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AccountController/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users
                .SingleOrDefaultAsync
                (u => u.Email == loginModel.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Armazenando o ID do usuário nas claims
                    };
                    var claimsIdentity = new ClaimsIdentity
                        (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };
                   
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Armazenar o ID do usuário na sessão
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(loginModel);
        }


        
        [HttpPost]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpar a sessão
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

    }
}
