using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Growtify.Infrastructure.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(GrowtifyDbContext context)
        {
            if (context.AppUsers.Any()) return;

            var json = await File.ReadAllTextAsync("../Growtify.Infrastructure/Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<SeedUserDto>>(json);

            if (users == null)
            {
                Console.WriteLine("No members in seed data.");
                return;
            }

            foreach (var u in users)
            {
                using var hmac = new HMACSHA512();

                AppUser appUser = new AppUser
                {
                    Id = u.Id,
                    UserName = u.UserName.ToLower(),
                    Email = u.Email,
                    ImageUrl = u.ImageUrl,
                    CreatedAt = u.Created,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                    PasswordSalt = hmac.Key
                };

                Member member = new Member
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ImageUrl = u.ImageUrl,
                    DateOfBirth = u.DateOfBirth,
                    Created = u.Created,
                    LastActive = u.LastActive,
                    Gender = u.Gender,
                    Description = u.Description,
                    City = u.City,
                    Country = u.Country
                };

                appUser.Member = member;
                appUser.Member.Photos.Add(new Photo
                {
                    Url = member.ImageUrl!,
                    MemberId = member.Id
                });

                context.AppUsers.Add(appUser);
                context.Members.Add(member);
            }

            await context.SaveChangesAsync();
        }
    }
}
