using Microsoft.EntityFrameworkCore;

namespace CRUD_Operation.Models
{
    public class ApplicationDbContext:DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Gerne> Gernes { get; set; }
    }
}
