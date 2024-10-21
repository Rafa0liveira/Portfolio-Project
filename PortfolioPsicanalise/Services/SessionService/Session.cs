using Newtonsoft.Json;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Services.SessionService
{
    public class Session : ISession
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public Session(IHttpContextAccessor httpAccessor)
        {
                _httpAccessor= httpAccessor;
        }
        public void CreateSession(UsersModel user)
        {
            string UserJson = JsonConvert.SerializeObject(user);
            _httpAccessor.HttpContext.Session.SetString("UserActive", UserJson);
        }

        public void RemoveSession()
        {
            _httpAccessor.HttpContext.Session.Remove("UserActive");
        }

        public UsersModel SearchSession()
        {
           string userSession = _httpAccessor.HttpContext.Session.GetString("UserActive");
            if (userSession == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<UsersModel>(userSession);
                
        }
    }
}
