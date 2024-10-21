
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace PortfolioPsicanalise.Models
{
    public class CoursesModel
    {
        public int Id
        {
        get; set; }
        public string Name
        {
        get; set; }

        public string Field
        {
            get; set;
        }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FinishDate
        {
            get; set;
        }
        public int Length
        {
            get; set;
        }

        [BindNever]
        public string UserId
        {
            get; set;
        }

    }
}
