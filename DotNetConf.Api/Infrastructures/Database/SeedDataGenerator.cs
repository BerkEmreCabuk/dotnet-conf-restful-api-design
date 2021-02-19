using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
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
                for (int i = 1; i < 5; i++)
                {
                    var tempRepository = new RepositoryEntity();
                    tempRepository.Name = "Repo" + i;
                    tempRepository.Description = "Description";
                    tempRepository.IsFork = false;
                    tempRepository.IsPrivate = true;
                    tempRepository.UserId = 1;
                    tempRepository.Add();
                    context.Repositories.Add(tempRepository);
                }
                context.SaveChanges();

                var test = context.Repositories.Include(x=>x.User).ToList();
            }
        }
    }
}
