using PortfolioPsicanalise.Models;
using PortfolioPsicanalise.ViewModels;


namespace PortfolioPsicanalise.ViewModels
{
    public class SearchUserViewModel
    {
     public  IEnumerable<BooksModel> Books; 
     public  IEnumerable<ArticlesModel> Articles ;
     public  IEnumerable<CoursesModel> Courses ;   
    }
}
