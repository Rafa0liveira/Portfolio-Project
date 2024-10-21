using Microsoft.EntityFrameworkCore;
using PortfolioPsicanalise.Models;

namespace PortfolioPsicanalise.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<BooksModel> Books
        {
            get; set;
        }
        public DbSet<CoursesModel> Courses
        {
            get; set;
        }

        public DbSet<ArticlesModel> Articles
        {
        get; set; 
        }
        public DbSet<UsersModel> Users

        {
        get; set; }

      

    }
}
