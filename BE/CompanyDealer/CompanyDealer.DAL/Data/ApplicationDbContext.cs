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
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Token> Tokens { get; set; } = null!; 
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
        public DbSet<InventoryVehicle> InventoryVehicles { get; set; } = null!; 
        public DbSet<RestockRequest> RestockRequests { get; set; } = null!; 

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

            // Vehicle - Category
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Category)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle - Inventory many-to-many (with Quantity)
            modelBuilder.Entity<InventoryVehicle>()
                .HasKey(iv => new { iv.InventoryId, iv.VehicleId });

            modelBuilder.Entity<InventoryVehicle>()
                .HasOne(iv => iv.Inventory)
                .WithMany(i => i.InventoryVehicles)
                .HasForeignKey(iv => iv.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryVehicle>()
                .HasOne(iv => iv.Vehicle)
                .WithMany(v => v.InventoryVehicles)
                .HasForeignKey(iv => iv.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

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

            // Account - Role many-to-many
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Roles)
                .WithMany(r => r.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "AccountRole",
                    j => j
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AccountRole_Role_RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Account>()
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK_AccountRole_Account_AccountId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("AccountId", "RoleId");
                        j.ToTable("AccountRole");
                    }
                );

            // Account - Token one-to-many
            modelBuilder.Entity<Token>()
                .HasOne(t => t.Account)
                .WithMany() // or .WithMany(a => a.Tokens) if you have a collection
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // RestockRequest relationships
            modelBuilder.Entity<RestockRequest>()
                .HasOne(r => r.Account)
                .WithMany()
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RestockRequest>()
                .HasOne(r => r.Dealer)
                .WithMany()
                .HasForeignKey(r => r.DealerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RestockRequest>()
                .HasOne(r => r.Vehicle)
                .WithMany()
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
