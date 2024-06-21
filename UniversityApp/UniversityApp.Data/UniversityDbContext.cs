using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversityApp.Data.Configurations;
using UniversityApp.Core.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UniversityApi.Data
{
    public class UniversityDbContext:IdentityDbContext<AppUser>
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration<Group>(new GroupConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
