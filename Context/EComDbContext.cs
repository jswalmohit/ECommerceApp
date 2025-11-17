using ECommerceApp.EComm.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Context
{
    public class EComDbContext : DbContext
    {
        public EComDbContext(DbContextOptions<EComDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<UserCredentialEntity> Credentials => Set<UserCredentialEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Credentials)
                .WithOne(c => c.User)
                .HasForeignKey<UserCredentialEntity>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
