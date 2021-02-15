using DotNetConf.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Infrastructures.Database
{
    public class DotNetConfDbContext : DbContext
    {
        public DotNetConfDbContext(DbContextOptions<DotNetConfDbContext> options)
           : base(options)
        {
        }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<RepositoryEntity> Repositories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RepositoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<RepositoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.User)
                    .WithMany(x => x.Repositories)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
