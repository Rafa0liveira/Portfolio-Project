using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Services.SessionService
{
    public interface ISession
    {
       

        void CreateSession(UsersModel user);

        void RemoveSession();

        UsersModel SearchSession();


    }
}
