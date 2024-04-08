using Domain.Entities.Auth;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public partial class AppDbInitializer
    {
        public async Task SeedAuthEverything(AppDbContext context, IServiceScope serviceScope, string contentPath)
        {
            if (!context.Users.Any())
            {
                SeedUsers(context);
            }

            if (!context.Roles.Any())
            {
                SeedRoles(context);
            }
            else
            {
                SeedRoles(context);
            }

            if (!context.UserRoles.Any())
            {
                SeedUserRoles(context);
            }

            if(!context.Cities.Any())
            {
                SeedCities(context);
            }

            if(!context.Governates.Any())
            {
                SeedGovernorates(context);
            }            
        }

        public void SeedUsers(AppDbContext context)
        {
            var users = new[] {
        new User() {
          AccessFailedCount = 0,
          ConcurrencyStamp = Guid.NewGuid().ToString(),
          Email = "Admin@Apps.com",
          EmailConfirmed = true,
          Id = Guid.NewGuid().ToString(),
          LockoutEnabled = false,
          LockoutEnd = null,
          NormalizedEmail = ("Admin@Apps.com").ToUpper(),
          NormalizedUserName = ("Admin").ToUpper(),
          PasswordHash = "AQAAAAEAACcQAAAAEIsWjNQ5zjWAxoVt9Hr9Z3XUpWtkXhhil17iNtANiIuQnkIGRynUkDy529Cqpk/Epg==",
          PhoneNumber = "",
          PhoneNumberConfirmed = true,
          SecurityStamp = Guid.NewGuid().ToString(),
          TwoFactorEnabled = false,
          UserName = "Admin",
          CreatedDate = DateTime.UtcNow,
          CreatedBy = "System"
        },
      };
            var userCount = new UserCount { Id = "1", Count = 1 };

            context.UserCounts.Add(userCount);
            context.Users.AddRange(users);

            context.SaveChanges();
        }

        public void SeedRoles(AppDbContext context)
        {
            var roles = new List<Role>();
            var dbRoles = context.Roles.ToList();
            foreach (RolesKey item in Enum.GetValues(typeof(RolesKey)))
            {
                if (!dbRoles.Any(a => a.Name == item.ToString()))
                    roles.Add(new Role(item.ToString()) { NormalizedName = item.ToString().ToUpper() });
            }

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        public void SeedUserRoles(AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == "Admin");
            var role = context.Roles.FirstOrDefault(r => r.Name == RolesKey.SuperAdmin.ToString());

            context.UserRoles.Add(new UserRole() { RoleId = role?.Id, UserId = user?.Id });
            context.SaveChanges();
        }

        public void SeedCities(AppDbContext context)
        {
            try
            {
                var Cities = new List<City>();
                var file = File.ReadAllText(Path.Combine("seeds", "cities.json"));
                var jsoncities = JsonConvert.DeserializeObject<List<City>>(file);
                foreach (var c in jsoncities)
                {
                    if (c != null)
                    {

                        var City = new City()
                        {
                            Id = c.Id.ToString(),
                            NameAr = c.NameAr,
                            NameEn = c.NameEn
                        };
                        Cities.Add(City);
                    }
                }
                context.Cities.AddRange(Cities);
                context.SaveChanges();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SeedGovernorates(AppDbContext context)
        {
            try
            {
                var Governorates = new List<Governate>();
                var file = File.ReadAllText(Path.Combine("seeds", "governorates.json"));
                var jsongovernorates = JsonConvert.DeserializeObject<List<Governate>>(file);
                foreach (var c in jsongovernorates)
                {
                    if (c != null)
                    {

                        var Governorate = new Governate()
                        {
                            Id = c.Id.ToString(),
                            NameAr = c.NameAr,
                            NameEn = c.NameEn,
                        };
                        Governorates.Add(Governorate);
                    }
                }
                context.AddRange(Governorates);
                context.SaveChanges();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }

    public class UserJson
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}