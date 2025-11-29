using ECommerceApp.EComm.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Data.Context
{
    public class EComDbContext(DbContextOptions<EComDbContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<UserCredentialEntity> Credentials => Set<UserCredentialEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Credentials)
                .WithOne(c => c.User)
                .HasForeignKey<UserCredentialEntity>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemEntity>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemEntity>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure one cart item per user-product combination
            modelBuilder.Entity<CartItemEntity>()
                .HasIndex(c => new { c.UserId, c.ProductId })
                .IsUnique();
        }
    }
}
