using Microsoft.EntityFrameworkCore;

using Growtify.Domain.Entities;

namespace Growtify.Infrastructure.Data
{
    public class GrowtifyDbContext : DbContext
    {
        public GrowtifyDbContext()
        {

        }

        public GrowtifyDbContext(DbContextOptions<GrowtifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();

                // TODO: make every entity's configuration in a separate class - AppUserConfiguration
            });
        }
    }
}
