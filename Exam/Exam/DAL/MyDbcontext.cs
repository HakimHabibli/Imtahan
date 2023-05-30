using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.DAL
{
    public class MyDbcontext : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=CA-R215-PC11\SQLEXPRESS;Database=Imtahan2;Integrated Security=true;");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Team> Teams { get; set; }  
    }
}
