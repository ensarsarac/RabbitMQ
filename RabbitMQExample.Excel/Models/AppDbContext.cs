using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RabbitMQExample.Excel.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<UserFile> UserFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<IdentityUser>().HasData(new IdentityUser { Id = "8098d13c-292a-4863-9ba7-bf9d0ae4bacb", Email = "ensar.src94@gmail.com", UserName="ensarsarac", PasswordHash = hasher.HashPassword(null,"123456aA."), NormalizedEmail="ENSAR.SRC94@GMAIL.COM", NormalizedUserName="ENSARSARAC" });

            base.OnModelCreating(builder);
        }
    }
}
