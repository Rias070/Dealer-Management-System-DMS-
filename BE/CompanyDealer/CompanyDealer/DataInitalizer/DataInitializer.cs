using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using BCrypt.Net;

namespace CompanyDealer.DataInitalizer
{
    public static class DataInitializer
    {
        /// <summary>
        /// Seed demo data that matches the actual model properties present in the project.
        /// This avoids referencing properties/types that may not exist and keeps the seeder compilable.
        /// Call after migrations during app startup.
        /// </summary>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (await context.Dealers.AnyAsync()) return;

            // Seed Roles
            var companyAdminRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyAdmin" };
            var companyStaffRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyStaff" };
            var dealerAdminRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerAdmin" };
            var dealerStaffRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerAdmin" };
            await context.Set<Role>().AddRangeAsync(companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole);

            // Dealer
            var dealerId = Guid.NewGuid();
            var dealer = new Dealer
            {
                Id = dealerId,
                Name = "Demo Dealer",
                Location = "Hanoi, Vietnam",
                ContactInfo = "demo@dealer.local | +84 912 345 678",
                RegistrationDate = DateTime.UtcNow.AddYears(-1),
                IsActive = true
            };

            // Accounts - ensure DealerId is set (Account model has DealerId)
            var adminAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin",
                ContactPerson = "Admin Person",
                Email = "admin@dealer.local",
                Phone = "+84 912 345 678",
                Address = "123 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealerId,
                Roles = new[] { companyAdminRole }
            };

            var staffAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Staff",
                ContactPerson = "Staff Person",
                Email = "staff@dealer.local",
                Phone = "+84 912 000 000",
                Address = "123 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "staff",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                DealerId = dealerId,
                Roles = new[] { companyStaffRole }
            };

            // Categories (Category model contains Id, Name, Description)
            var sedanCategoryId = Guid.NewGuid();
            var suvCategoryId = Guid.NewGuid();
            var sedan = new Category
            {
                Id = sedanCategoryId,
                Name = "Sedan",
                Description = "4-door passenger cars"
            };
            var suv = new Category
            {
                Id = suvCategoryId,
                Name = "SUV",
                Description = "Sport Utility Vehicles"
            };

            // Inventory (Inventory model requires DealerId)
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealerId,
                LastUpdated = DateTime.UtcNow
            };

            // Vehicles (Vehicle model requires CategoryId and other basic fields)
            var vehicle1 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                VIN = "VIN-CAMRY-0001",
                Color = "White",
                Price = 30000m,
                Description = "Demo Toyota Camry",
                IsAvailable = true,
                CategoryId = sedanCategoryId
            };

            var vehicle2 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Honda",
                Model = "CR-V",
                Year = 2023,
                VIN = "VIN-CRV-0001",
                Color = "Black",
                Price = 35000m,
                Description = "Demo Honda CR-V",
                IsAvailable = true,
                CategoryId = suvCategoryId
            };

            // Add in proper order to satisfy FKs
            await context.Set<Role>().AddRangeAsync(companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole);
            await context.Dealers.AddAsync(dealer);
            await context.Categories.AddRangeAsync(sedan, suv);
            await context.Inventories.AddAsync(inventory);
            await context.Vehicles.AddRangeAsync(vehicle1, vehicle2);
            await context.Accounts.AddRangeAsync(adminAccount, staffAccount);

            await context.SaveChangesAsync();
        }
    }
}
