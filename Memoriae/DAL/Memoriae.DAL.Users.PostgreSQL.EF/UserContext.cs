using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memoriae.DAL.Users.PostgreSQL.EF
{
    public partial class UserContext : IdentityDbContext<IdentityUser>
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("memoriae_users");

            base.OnModelCreating(modelBuilder);            
        }       

    }
}
