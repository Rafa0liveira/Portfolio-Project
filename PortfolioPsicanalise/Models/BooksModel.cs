
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Models
{
    public class BooksModel
    {
        public int Id
        {
            get; set;
        }
        [Required(ErrorMessage ="Type in the name of the book")]
        public string Book
        {
            get; set;
        }
        [Required(ErrorMessage = "Type in the name of the author(s)")]
        public string AuthorName 
        {
            get; set;
        }
        [Required(ErrorMessage = "Type in the name of the field of knowledge)")]
        public string Field
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date
        {
            get; set;
        } = DateTime.Today;

        [BindNever]
        public string UserId
        {
            get; set;
        }
    }
}
