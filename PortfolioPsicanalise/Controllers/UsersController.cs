using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPsicanalise.Data;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Controllers
{
    public class UsersController : Controller
    {
        readonly private AppDbContext _db;
        public UsersController(AppDbContext db)
        {      
        _db = db;
        }
        
        [HttpGet]

        public IActionResult Index(string userName)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }

            userName = User.Identity.Name;

            IEnumerable<UsersModel> users = _db.Users.Where(u => u.Name == userName);
            return View(users);
        }
        [HttpGet]
        public IActionResult UserProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEmail = User.Identity.Name;

            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersModel userModel)
        {
            // Remover a validação do campo NewPassword, já que ele não é necessário no registro
            ModelState.Remove("NewPassword");

            Console.WriteLine("Register method called");

            // Verificar se o ModelState é válido
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
                return View(userModel);
            }

            // Verificar se o email já existe
            var existingUser = await _db.Users.SingleOrDefaultAsync(u => u.Email == userModel.Email);
            if (existingUser != null)
            {
                // Adicionar mensagem de erro se o email já estiver registrado
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(userModel);
            }

            // Criar novo usuário e salvar no banco de dados
            var user = new UsersModel
            {
                Name = userModel.Name,
                Email = userModel.Email,
                // Criptografar a senha fornecida
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash)
            };

            Console.WriteLine("Adding user to the database");
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            Console.WriteLine("User saved");

            return RedirectToAction("Login","Account");  // Redirecionar para a página principal ou login
        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;

            UsersModel user = await _db.Users.FirstOrDefaultAsync(a => a.Name == userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(user);
        }
        /* [HttpPost]
         public IActionResult EditUser(UsersModel user)
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

             // Associar o UserId ao nome do usuário
             user.Name = userId;

             if (ModelState.IsValid)
             {
                 _db.Users.Update(user);
                 _db.SaveChanges();
                 TempData["SuccessfulAction"] = "User edited successfully";


                 return RedirectToAction("Index");
             }
             return View(user);
         }
        */
        [HttpPost]
        public async Task<IActionResult> EditUser(UsersModel user)
        {
            // Verificar se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;

            // Carregar o usuário original do banco de dados
            var userInDb = await _db.Users.FirstOrDefaultAsync(a => a.Name == userId);
            if (userInDb == null)
            {
                return NotFound();
            }

            // Atualizar os dados permitidos (Nome é ignorado)
            userInDb.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(user.NewPassword))
            {
                userInDb.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);
            }

            _db.Users.Update(userInDb);
            await _db.SaveChangesAsync();

            TempData["SuccessfulAction"] = "User updated successfully";
            
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult DeleteUser(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            UsersModel user = _db.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(UsersModel user)
        {
            if (user == null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);
            _db.SaveChanges();

            TempData["SuccessfulAction"] = "User deleted successfully";

            return RedirectToAction("Index");
        }

    }
}
