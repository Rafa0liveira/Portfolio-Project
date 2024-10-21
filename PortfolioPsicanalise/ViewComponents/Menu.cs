using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessionUser = HttpContext.Session.GetString("UserActive");
            if (string.IsNullOrEmpty(sessionUser))
                return null;
            UsersModel user = JsonConvert.DeserializeObject<UsersModel>(sessionUser);

            return View();
        }
    }
}
