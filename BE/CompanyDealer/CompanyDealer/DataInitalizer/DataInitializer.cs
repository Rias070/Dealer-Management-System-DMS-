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
            var dealerStaffRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerStaff" };
            var dealerManagerRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerManager" };
            var companyManagerRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyManager" };

            await context.Set<Role>().AddRangeAsync(
                companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole, dealerManagerRole, companyManagerRole
            );

            // Seed Dealers
            var dealer1Id = Guid.NewGuid();
            var dealer2Id = Guid.NewGuid();
            var dealer3Id = Guid.NewGuid();

            var dealer1 = new Dealer
            {
                Id = dealer1Id,
                Name = "Demo Dealer 1",
                Location = "Hanoi, Vietnam",
                ContactInfo = "demo1@dealer.local | +84 912 345 678",
                RegistrationDate = DateTime.UtcNow.AddYears(-1),
                IsActive = true
            };

            var dealer2 = new Dealer
            {
                Id = dealer2Id,
                Name = "Demo Dealer 2",
                Location = "Ho Chi Minh City, Vietnam",
                ContactInfo = "demo2@dealer.local | +84 912 111 222",
                RegistrationDate = DateTime.UtcNow.AddYears(-2),
                IsActive = true
            };

            var dealer3 = new Dealer
            {
                Id = dealer3Id,
                Name = "Demo Dealer 3",
                Location = "Da Nang, Vietnam",
                ContactInfo = "demo3@dealer.local | +84 912 333 444",
                RegistrationDate = DateTime.UtcNow.AddYears(-3),
                IsActive = true
            };

            // Seed Categories
            var sedanCategory = new Category { Id = Guid.NewGuid(), Name = "Sedan", Description = "Sedan cars" };
            var suvCategory = new Category { Id = Guid.NewGuid(), Name = "SUV", Description = "SUV cars" };
            await context.Categories.AddRangeAsync(sedanCategory, suvCategory);

            // Seed Vehicles (do NOT assign InventoryId directly)
            var vehicle1 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                VIN = "VIN001",
                Color = "Black",
                Price = 35000,
                Description = "Comfortable sedan",
                IsAvailable = true,
                CategoryId = sedanCategory.Id
            };
            var vehicle2 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Honda",
                Model = "CR-V",
                Year = 2023,
                VIN = "VIN002",
                Color = "White",
                Price = 40000,
                Description = "Spacious SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vehicle3 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Ford",
                Model = "Focus",
                Year = 2021,
                VIN = "VIN003",
                Color = "Blue",
                Price = 25000,
                Description = "Economical sedan",
                IsAvailable = true,
                CategoryId = sedanCategory.Id
            };
            await context.Vehicles.AddRangeAsync(vehicle1, vehicle2, vehicle3);

            // Seed Inventories
            var inventory1 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer1Id,
                LastUpdated = DateTime.UtcNow
            };
            var inventory2 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer2Id,
                LastUpdated = DateTime.UtcNow
            };
            var inventory3 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer3Id,
                LastUpdated = DateTime.UtcNow
            };
            await context.Inventories.AddRangeAsync(inventory1, inventory2, inventory3);

            // Link Vehicles to Inventories via InventoryVehicle
            var inventoryVehicle1 = new InventoryVehicle
            {
                InventoryId = inventory1.Id,
                VehicleId = vehicle1.Id,
                Quantity = 5
            };
            var inventoryVehicle2 = new InventoryVehicle
            {
                InventoryId = inventory1.Id,
                VehicleId = vehicle2.Id,
                Quantity = 2
            };
            var inventoryVehicle3 = new InventoryVehicle
            {
                InventoryId = inventory2.Id,
                VehicleId = vehicle3.Id,
                Quantity = 3
            };
            var inventoryVehicle4 = new InventoryVehicle
            {
                InventoryId = inventory3.Id,
                VehicleId = vehicle1.Id,
                Quantity = 1
            };
            await context.InventoryVehicles.AddRangeAsync(inventoryVehicle1, inventoryVehicle2, inventoryVehicle3, inventoryVehicle4);

            // Accounts for each dealer
            var adminAccount1 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 1",
                ContactPerson = "Admin Person 1",
                Email = "admin1@dealer.local",
                Phone = "+84 912 345 678",
                Address = "123 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin1",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer1Id,
                Roles = new[] { companyAdminRole }
            };

            var adminAccount2 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 2",
                ContactPerson = "Admin Person 2",
                Email = "admin2@dealer.local",
                Phone = "+84 912 111 222",
                Address = "456 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin2",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer2Id,
                Roles = new[] { companyAdminRole }
            };

            var adminAccount3 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 3",
                ContactPerson = "Admin Person 3",
                Email = "admin3@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin3",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer3Id,
                Roles = new[] { companyAdminRole }
            };

            var companyStaff1 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Staff 1",
                ContactPerson = "Admin Person 3",
                Email = "asdf@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "staff1",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                DealerId = dealer3Id,
                Roles = new[] { companyStaffRole }
            };
            var companyStaff2 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Staff 2",
                ContactPerson = "Admin Person 2",
                Email = "asdf@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "staff2",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                DealerId = dealer3Id,
                Roles = new[] { companyStaffRole }
            };

            // Seed DealerManager account
            var dealerManagerAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Dealer Manager 1",
                ContactPerson = "Manager Person 1",
                Email = "manager1@dealer.local",
                Phone = "+84 912 555 666",
                Address = "321 Manager Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "manager1",
                Password = BCrypt.Net.BCrypt.HashPassword("manager123"),
                DealerId = dealer1Id, // or assign to another dealer as needed
                Roles = new[] { dealerManagerRole }
            };

            await context.Dealers.AddRangeAsync(dealer1, dealer2, dealer3);
            await context.Accounts.AddRangeAsync(
                adminAccount1, adminAccount2, adminAccount3,
                companyStaff1, companyStaff2,
                dealerManagerAccount
            );

            await context.SaveChangesAsync();
        }
    }
}
