using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Dealer> Dealers { get; set; } = null!;
        public DbSet<DealerContract> DealerContracts { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<TestDriveRecord> TestDriveRecords { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Quotation> Quotations { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Promotion> Promotions { get; set; } = null!;
        public DbSet<Bill> Bills { get; set; } = null!;
        public DbSet<SaleContract> SaleContracts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Account - Dealer relationship
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Dealer)
                .WithMany(d => d.Accounts)
                .HasForeignKey(a => a.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            // DealerContract - Dealer relationship
            modelBuilder.Entity<DealerContract>()
                .HasOne(dc => dc.Dealer)
                .WithMany(d => d.DealerContracts)
                .HasForeignKey(dc => dc.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Feedback - Dealer relationship
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Dealer)
                .WithMany(d => d.Feedbacks)
                .HasForeignKey(f => f.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            // TestDriveRecord relationships
            modelBuilder.Entity<TestDriveRecord>()
                .HasOne(t => t.Dealer)
                .WithMany(d => d.TestDriveRecords)
                .HasForeignKey(t => t.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestDriveRecord>()
                .HasOne(t => t.Vehicle)
                .WithMany(v => v.TestDriveRecords)
                .HasForeignKey(t => t.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Inventory - Dealer relationship
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Dealer)
                .WithMany(d => d.Inventories)
                .HasForeignKey(i => i.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vehicle relationships
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Inventory)
                .WithMany(i => i.Vehicles)
                .HasForeignKey(v => v.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Category)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quotation relationships
            modelBuilder.Entity<Quotation>()
                .HasOne(q => q.Category)
                .WithMany(c => c.Quotations)
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quotation>()
                .HasOne(q => q.Order)
                .WithMany(o => o.Quotations)
                .HasForeignKey(q => q.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Order - Dealer relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Dealer)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DealerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-Bill one-to-one (Bill is dependent)
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Bill)
                .HasForeignKey<Bill>(b => b.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-SaleContract one-to-one (SaleContract is dependent)
            modelBuilder.Entity<SaleContract>()
                .HasOne(sc => sc.Order)
                .WithOne(o => o.SaleContract)
                .HasForeignKey<SaleContract>(sc => sc.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-Promotion many-to-many
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Promotions)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderPromotions"));
        }
    }
}
