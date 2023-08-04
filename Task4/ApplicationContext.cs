using Microsoft.EntityFrameworkCore;
using Task4.Models;

namespace Task4
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1,
                    Login = "1",
                    Password = "1",
                    Name = "Vi", 
                    Email = "ff@gmail.com", 
                    RegisterDate = new DateTime(2023, 8, 4), 
                    LastLogin = DateTime.Now 
                });
        }
    }
}
