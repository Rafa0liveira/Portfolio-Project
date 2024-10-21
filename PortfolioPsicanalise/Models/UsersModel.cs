using PortfolioPsicanalise.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioPsicanalise.Models
{
    public class UsersModel
    {
        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
       
        public string PasswordHash
        {
            get; set;
        }

        [NotMapped]
       public string NewPassword
        {
        
        get; set;
        }
       

    }

}

