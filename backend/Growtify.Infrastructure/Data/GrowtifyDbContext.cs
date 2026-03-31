using Microsoft.EntityFrameworkCore;
using Growtify.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Growtify.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Growtify.Infrastructure.Data
{
    public class GrowtifyDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<MemberLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Id = "member-id", Name = "Member", NormalizedName = "MEMBER" },
                    new IdentityRole { Id = "moderator-id", Name = "Moderator", NormalizedName = "MODERATOR" },
                    new IdentityRole { Id = "admin-id", Name = "Admin", NormalizedName = "ADMIN" }
                );

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GrowtifyDbContext).Assembly);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    } else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }
    }
}
