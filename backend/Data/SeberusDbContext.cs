using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Models;
namespace Serberus_Racket_Store.Data
{
    public class SeberusDbContext : DbContext
    {
        public SeberusDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Shipping_Info> Shippinginfo { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Rackets> Rackets { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(u => u.userCode)
                .HasComputedColumnSql("CONCAT('US', FORMAT(userId, 'D2'))");
            modelBuilder.Entity<Reviews>()
                .Property(re => re.reviewCode)
                .HasComputedColumnSql("CONCAT('RE', FORMAT(reviewId, 'D2'))");
            modelBuilder.Entity<Orders>()
                .Property(o => o.orderCode)
                .HasComputedColumnSql("CONCAT('OR', FORMAT(orderId, 'D2'))");
            modelBuilder.Entity<Shipping_Info>()
                .Property(s => s.shippingCode)
                .HasComputedColumnSql("CONCAT('SH', FORMAT(shippingId, 'D2'))");

            modelBuilder.Entity<Rackets>()
                .Property(r => r.racketCode)
                .HasComputedColumnSql("CONCAT('RA', FORMAT(racketId, 'D2'))");
            modelBuilder.Entity<OrderItems>()
                .Property(oi => oi.orderItemCode)
                .HasComputedColumnSql("CONCAT('OI', FORMAT(orderItemId, 'D2'))");
            modelBuilder.Entity<CartItems>()
                .Property(c => c.cartItemCode)
                .HasComputedColumnSql("CONCAT('CA', FORMAT(cartItemId, 'D2'))");
            modelBuilder.Entity<Brands>()
                .Property(b => b.brandCode)
                .HasComputedColumnSql("CONCAT('US', FORMAT(brandId, 'D2'))");


            modelBuilder.Entity<Brands>().HasQueryFilter(b => !b.isDelete);
            modelBuilder.Entity<CartItems>().HasQueryFilter(c => !c.isDelete);
            modelBuilder.Entity<OrderItems>().HasQueryFilter(or => !or.isDelete);
            modelBuilder.Entity<Orders>().HasQueryFilter(o => !o.isDelete);
            modelBuilder.Entity<Rackets>().HasQueryFilter(r => !r.isDelete);
            modelBuilder.Entity<Reviews>().HasQueryFilter(re => !re.isDelete);
            modelBuilder.Entity<Shipping_Info>().HasQueryFilter(s => !s.isDelete);
            modelBuilder.Entity<Users>().HasQueryFilter(u => !u.isDelete);



            modelBuilder.Entity<Users>().HasKey(u => u.userId);
            modelBuilder.Entity<Orders>().HasKey(o => o.orderId);
            modelBuilder.Entity<Shipping_Info>().HasKey(s => s.shippingId);
            modelBuilder.Entity<Brands>().HasKey(b => b.brandId);
            modelBuilder.Entity<Rackets>().HasKey(r => r.racketId);
            modelBuilder.Entity<Reviews>().HasKey(re => re.reviewId);
            modelBuilder.Entity<CartItems>().HasKey(c => c.cartItemId);
            modelBuilder.Entity<OrderItems>().HasKey(or => or.orderItemId);

            /*USERS*/
            modelBuilder.Entity<Users>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Users)
                .HasForeignKey(o => o.userId);
            modelBuilder.Entity<Users>()
                .HasMany(u => u.CartItems)
                .WithOne(c => c.Users)
                .HasForeignKey(c => c.userId);
            modelBuilder.Entity<Users>()
                .HasMany(u => u.Reviews)
                .WithOne(re => re.Users)
                .HasForeignKey(re => re.userId);

            /*ORDERS*/
            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderItems)
                .WithOne(or => or.Orders)
                .HasForeignKey(or => or.orderId);
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Shipping_Info)
                .WithOne(s => s.Orders)
                .HasForeignKey<Shipping_Info>(s => s.orderId);

            /*BRANDS*/
            modelBuilder.Entity<Brands>()
                .HasMany(b => b.Rackets)
                .WithOne(r => r.Brands)
                .HasForeignKey(r => r.brandId);

            /*RACKETS*/
            modelBuilder.Entity<Rackets>()
                .HasMany(r => r.Reviews)
                .WithOne(re => re.Rackets)
                .HasForeignKey(re => re.racketId);
            modelBuilder.Entity<Rackets>()
                .HasMany(r => r.CartItems)
                .WithOne(c => c.Rackets)
                .HasForeignKey(c => c.racketId);
            modelBuilder.Entity<Rackets>()
                .HasMany(r => r.OrderItems)
                .WithOne(or => or.Rackets)
                .HasForeignKey(or => or.racketId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
