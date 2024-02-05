using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace PortfolioPsicanalise.Models
{
    public class ArticlesModel
    {
        public int Id
        {
        get; set; 
        }
        public string Title
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
        public DateTime LastUpdate
        {
            get; set;
        } = DateTime.Now;



    }
}
