using JwtIntro.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtIntro.Data
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
