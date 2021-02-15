using DotNetConf.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Infrastructures.Database
{
    public class SeedDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DotNetConfDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<DotNetConfDbContext>>()))
            {
                // Look for any board games.
                if (context.Users.Any())
                {
                    return;   // Data was already seeded
                }
                for (int i = 1; i < 5; i++)
                {
                    var tempUser = new UserEntity();
                    tempUser.Name = "User"+i;
                    tempUser.Username = "User"+i;
                    tempUser.Company = "Blabla";
                    tempUser.Bio = $"Hi my name {tempUser.Name} and work {tempUser.Company}";
                    tempUser.Add();
                    context.Users.Add(tempUser);
                }

                context.SaveChanges();

                var test = context.Users.ToList();
            }
        }
    }
}
